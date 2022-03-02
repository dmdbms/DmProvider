using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Transactions;
using Dm.Config;
using Dm.filter;
using Dm.filter.log;
using Dm.filter.reconnect;
using Dm.filter.rw;
using Dm.util;

namespace Dm
{
	public sealed class DmConnection : DbConnection, ICloneable, IFilterInfo
	{
		internal long id = -1L;

		internal static long idGenerator = 0L;

		private static readonly string ClassName = "DmConnection";

		internal DmConnInstance m_ConnInst;

		private volatile bool m_AlreadyDisposed;

		private DmSchema m_Schema;

		private bool forEFCore;

		private ConnectionState connectionState;

		internal static RsLRUCache rsLRUCache;

		internal static object obj = new object();

		public long ID
		{
			get
			{
				if (id < 0)
				{
					id = Interlocked.Increment(ref idGenerator);
				}
				return id;
			}
		}

		public FilterChain FilterChain { get; set; }

		public LogInfo LogInfo { get; set; }

		public RWInfo RWInfo { get; set; }

		public RecoverInfo RecoverInfo { get; set; }

		internal string do_ServerVersion
		{
			get
			{
				if (do_State == ConnectionState.Closed)
				{
					throw new InvalidOperationException("connection is closed");
				}
				return ConnProperty.ServerVersion;
			}
		}

		internal string do_DataSource
		{
			get
			{
				if (do_State == ConnectionState.Closed)
				{
					return null;
				}
				return ConnProperty.Server;
			}
		}

		internal string do_Database
		{
			get
			{
				if (do_State == ConnectionState.Closed)
				{
					return "";
				}
				return ConnProperty.Database;
			}
		}

		internal int do_ConnectionTimeout => ConnProperty.ConnectionTimeout;

		internal string do_ConnectionString
		{
			get
			{
				return ConnProperty.ConnectionString;
			}
			set
			{
				ConnProperty.ConnectionString = value;
				if (rsLRUCache == null && ConnProperty.EnRsCache)
				{
					lock (obj)
					{
						if (rsLRUCache == null)
						{
							rsLRUCache = new RsLRUCache(ConnProperty.RsCacheSize * 1024 * 1024);
						}
					}
				}
				FilterChain.createFilterChain(this, ConnProperty);
			}
		}

		internal ConnectionState do_State => connectionState;

		internal DbProviderFactory do_DbProviderFactory => DmClientFactory.Instance;

		public override string ServerVersion
		{
			get
			{
				if (FilterChain == null)
				{
					return do_ServerVersion;
				}
				return FilterChain.reset().getServerVersion(this);
			}
		}

		public override string DataSource
		{
			get
			{
				if (FilterChain == null)
				{
					return do_DataSource;
				}
				return FilterChain.reset().getDataSource(this);
			}
		}

		public override string Database
		{
			get
			{
				if (FilterChain == null)
				{
					return do_Database;
				}
				return FilterChain.reset().getDatabase(this);
			}
		}

		public override int ConnectionTimeout
		{
			get
			{
				if (FilterChain == null)
				{
					return do_ConnectionTimeout;
				}
				return FilterChain.reset().getConnectionTimeout(this);
			}
		}

		public override string ConnectionString
		{
			get
			{
				if (FilterChain == null)
				{
					return do_ConnectionString;
				}
				return FilterChain.reset().getConnectionString(this);
			}
			set
			{
				do_ConnectionString = value;
			}
		}

		protected override DbProviderFactory DbProviderFactory
		{
			get
			{
				if (FilterChain == null)
				{
					return do_DbProviderFactory;
				}
				return FilterChain.reset().getDbProviderFactory(this);
			}
		}

		public override ConnectionState State
		{
			get
			{
				if (FilterChain == null)
				{
					return do_State;
				}
				return FilterChain.reset().getState(this);
			}
		}

		public DmMppType MppType
		{
			get
			{
				if (1 == ConnProperty.MppType)
				{
					return DmMppType.LOGIN_MPP_LOCAL;
				}
				return DmMppType.LOGIN_MPP_GLOBAL;
			}
			set
			{
				if (value == DmMppType.LOGIN_MPP_LOCAL)
				{
					ConnProperty.MppType = 1;
					return;
				}
				if (DmMppType.LOGIN_MPP_GLOBAL == value)
				{
					ConnProperty.MppType = 0;
					return;
				}
				throw new InvalidOperationException("invalid mpp status");
			}
		}

		public string User => ConnProperty?.User;

		public string Password => ConnProperty?.Pwd;

		internal DmConnProperty ConnProperty { get; set; }

		public string Schema
		{
			get
			{
				if (do_State == ConnectionState.Closed)
				{
					return "";
				}
				return ConnProperty.Schema;
			}
			set
			{
				ConnProperty.Schema = value;
			}
		}

		public bool ForEFCore
		{
			get
			{
				return forEFCore;
			}
			set
			{
				forEFCore = value;
			}
		}

		public DmConnection()
		{
			ConnProperty = new DmConnProperty();
		}

		public DmConnection(bool forEF)
			: this()
		{
			ForEFCore = true;
		}

		public DmConnection(string connectionString)
			: this()
		{
			do_ConnectionString = connectionString;
		}

		public DmConnection(string connectionString, bool forEFCore)
			: this(forEFCore)
		{
			do_ConnectionString = connectionString;
		}

		internal DmTransaction do_BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
		{
			if (isolationLevel == System.Data.IsolationLevel.Unspecified)
			{
				isolationLevel = ConnProperty.IsolationLevel;
			}
			if (do_State == ConnectionState.Closed)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_CONNCTION_NOT_OPENED);
			}
			return m_ConnInst.BeginTrx(isolationLevel);
		}

		internal void do_ChangeDatabase(string databaseName)
		{
			DmError.ThrowDmException(DmErrorDefinition.ECNET_DO_NOT_SUPPORT_CATALOG);
		}

		internal void do_Close()
		{
			if (m_ConnInst != null && m_ConnInst.CurrentTransaction != null)
			{
				connectionState = ConnectionState.Closed;
				return;
			}
			ReleaseUnmanagedResource(pooled: false);
			SetState(ConnectionState.Closed);
		}

		internal DmCommand do_CreateDbCommand()
		{
			return new DmCommand("", this);
		}

		internal void Reconnect()
		{
			do_Close();
			lock (this)
			{
				Connect();
			}
		}

		internal void do_EnlistTransaction(Transaction transaction)
		{
			if (transaction == null)
			{
				return;
			}
			if (m_ConnInst.CurrentTransaction != null)
			{
				if (m_ConnInst.CurrentTransaction.BaseTransaction == transaction)
				{
					return;
				}
				throw new InvalidOperationException("Already enlisted");
			}
			DmConnInstance dmConnInstanceInTransaction = DmConnInstanceTransactionManager.GetDmConnInstanceInTransaction(transaction);
			if (dmConnInstanceInTransaction != null)
			{
				_ = dmConnInstanceInTransaction.ConnProperty.ConnectionString;
				_ = do_ConnectionString;
				do_Close();
				m_ConnInst = dmConnInstanceInTransaction;
			}
			if (m_ConnInst == null)
			{
				throw new InvalidOperationException("connInstance is null");
			}
			if (m_ConnInst.CurrentTransaction == null)
			{
				DmPromotableTransaction dmPromotableTransaction = new DmPromotableTransaction(this, transaction);
				if (!transaction.EnlistPromotableSinglePhase(dmPromotableTransaction))
				{
					DmError.ThrowUnsupportedException();
				}
				m_ConnInst.CurrentTransaction = dmPromotableTransaction;
				DmConnInstanceTransactionManager.SetDmConnInstanceInTransaction(m_ConnInst);
			}
		}

		internal void do_EnlistTransaction2(Transaction transaction)
		{
			if (transaction == null)
			{
				return;
			}
			if (m_ConnInst.CurrentTransaction != null)
			{
				if (m_ConnInst.CurrentTransaction.BaseTransaction == transaction)
				{
					return;
				}
				throw new InvalidOperationException("Already enlisted");
			}
			if (m_ConnInst == null)
			{
				throw new InvalidOperationException("connInstance is null");
			}
			DmNotificationTransaction enlistmentNotification = new DmNotificationTransaction(transaction);
			transaction.EnlistDurable(DmNotificationTransaction.RMID, enlistmentNotification, EnlistmentOptions.None);
			DmNotificationTransaction.TransactionMap.Add(transaction.TransactionInformation.LocalIdentifier, m_ConnInst.ConnProperty.SessId, m_ConnInst);
		}

		internal DataTable do_GetSchema()
		{
			return do_GetSchema(null);
		}

		internal DataTable do_GetSchema(string collectionName)
		{
			if (collectionName == null || collectionName.Equals(""))
			{
				collectionName = "METADATACOLLECTIONS";
			}
			return do_GetSchema(collectionName, null);
		}

		internal DataTable do_GetSchema(string collectionName, string[] restrictionValues)
		{
			if (m_Schema == null)
			{
				m_Schema = new DmSchema(this);
			}
			if (collectionName == null || collectionName.Equals(""))
			{
				collectionName = "METADATACOLLECTIONS";
			}
			return m_Schema.GetSchema(collectionName, restrictionValues);
		}

		internal void do_Open()
		{
			if (do_State != ConnectionState.Open)
			{
				if (do_State == ConnectionState.Broken)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_COMMUNITION_ERROR);
				}
				CheckProperty();
				m_ConnInst = new DmConnInstance(this);
				SetState(ConnectionState.Open);
				if (ConnProperty.Enlist && Transaction.Current != null)
				{
					do_EnlistTransaction(Transaction.Current);
				}
				if (ConnProperty.Schema != DmOptionHelper.schemaDef)
				{
					DriverUtil.executeNonQuery(this, "set schema " + ConnProperty.Schema, null);
				}
			}
		}

		protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
		{
			if (FilterChain == null)
			{
				return do_BeginDbTransaction(isolationLevel);
			}
			return FilterChain.reset().BeginDbTransaction(this, isolationLevel);
		}

		public override void ChangeDatabase(string databaseName)
		{
			if (FilterChain == null)
			{
				do_ChangeDatabase(databaseName);
			}
			else
			{
				FilterChain.reset().ChangeDatabase(this, databaseName);
			}
		}

		public override void Close()
		{
			if (FilterChain == null)
			{
				do_Close();
			}
			else
			{
				FilterChain.reset().Close(this);
			}
		}

		protected override DbCommand CreateDbCommand()
		{
			if (FilterChain == null)
			{
				return do_CreateDbCommand();
			}
			return FilterChain.reset().CreateDbCommand(this);
		}

		public override void EnlistTransaction(Transaction transaction)
		{
			if (FilterChain == null)
			{
				do_EnlistTransaction(transaction);
			}
			else
			{
				FilterChain.reset().EnlistTransaction(this, transaction);
			}
		}

		public override DataTable GetSchema()
		{
			if (FilterChain == null)
			{
				return do_GetSchema();
			}
			return FilterChain.reset().GetSchema(this);
		}

		public override DataTable GetSchema(string collectionName)
		{
			if (FilterChain == null)
			{
				return do_GetSchema(collectionName);
			}
			return FilterChain.reset().GetSchema(this, collectionName);
		}

		public override DataTable GetSchema(string collectionName, string[] restrictionValues)
		{
			if (FilterChain == null)
			{
				return do_GetSchema(collectionName, restrictionValues);
			}
			return FilterChain.reset().GetSchema(this, collectionName, restrictionValues);
		}

		public override void Open()
		{
			if (FilterChain == null)
			{
				Connect();
			}
			else
			{
				FilterChain.reset().Open(this);
			}
		}

		internal void Connect()
		{
			if (do_State != ConnectionState.Open)
			{
				ConnProperty.EPGroup.connect(this);
			}
		}

		internal int getIndexOnDBGroup()
		{
			if (ConnProperty.EPGroup == null || ConnProperty.EPGroup.epList == null)
			{
				return -1;
			}
			EPGroup ePGroup = ConnProperty.EPGroup;
			for (int i = 0; i < ePGroup.epList.Count; i++)
			{
				EP eP = ePGroup.epList[i];
				if (ConnProperty.Server.Equals(eP.host, StringComparison.OrdinalIgnoreCase) && ConnProperty.Port == eP.port)
				{
					return i;
				}
			}
			return -1;
		}

		~DmConnection()
		{
			ReleaseUnmanagedResource(pooled: false);
		}

		internal void ReleaseUnmanagedResource(bool pooled)
		{
			if (connectionState == ConnectionState.Closed || m_ConnInst == null)
			{
				return;
			}
			if (m_ConnInst.Transaction != null)
			{
				try
				{
					m_ConnInst.Transaction.Dispose();
				}
				catch (Exception)
				{
				}
				finally
				{
					m_ConnInst.Transaction = null;
				}
			}
			m_ConnInst.Close(pooled);
			m_ConnInst = null;
		}

		protected override void Dispose(bool disposing)
		{
			if (!m_AlreadyDisposed)
			{
				try
				{
					Close();
				}
				finally
				{
					m_AlreadyDisposed = true;
					base.Dispose(disposing);
				}
			}
		}

		internal void SetState(ConnectionState newConnectionState)
		{
			ConnectionState originalState = connectionState;
			connectionState = newConnectionState;
			OnStateChange(new StateChangeEventArgs(originalState, connectionState));
		}

		public void SetDatabase(string db)
		{
			ConnProperty.Database = db;
		}

		public DmTransaction BeginTransaction(System.Data.IsolationLevel il, bool for_ef)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "BeginTransaction(IsolationLevel il)");
			if (do_State == ConnectionState.Closed)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_CONNCTION_NOT_OPENED);
			}
			return m_ConnInst.BeginTrx(il, for_ef);
		}

		internal DmConnInstance GetConnInstance()
		{
			return m_ConnInst;
		}

		internal void CheckProperty()
		{
			if (ConnProperty.Server == null)
			{
				throw new InvalidOperationException("Cannot open a connection without specifying a data source or server.");
			}
			if (string.IsNullOrEmpty(do_ConnectionString))
			{
				throw new InvalidOperationException("There is no connectionString.");
			}
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		public DmConnection Clone()
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "Clone()");
			return new DmConnection(do_ConnectionString);
		}
	}
}
