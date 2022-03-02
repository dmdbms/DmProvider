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

namespace Dm
{
	public sealed class DmTransaction : DbTransaction, IFilterInfo
	{
		internal long id = -1L;

		internal static long idGenerator;

		private DmStatement m_stmt;

		private DmConnInstance m_ConnInst;

		private bool m_AlreadyDisposed;

		private bool m_StmtSerial;

		private System.Data.IsolationLevel m_il;

		private bool m_Valid = true;

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

		internal System.Data.IsolationLevel do_IsolationLevel => m_il;

		internal DmConnection do_DbConnection
		{
			get
			{
				if (!m_Valid)
				{
					return null;
				}
				return m_ConnInst.Conn;
			}
		}

		public override System.Data.IsolationLevel IsolationLevel
		{
			get
			{
				if (FilterChain == null)
				{
					return do_IsolationLevel;
				}
				return FilterChain.reset().getIsolationLevel(this);
			}
		}

		protected override DbConnection DbConnection
		{
			get
			{
				if (FilterChain == null)
				{
					return do_DbConnection;
				}
				return FilterChain.reset().getDbConnection(this);
			}
		}

		internal DmStatement Stmt
		{
			get
			{
				return m_stmt;
			}
			set
			{
				m_stmt = value;
			}
		}

		public bool Valid
		{
			get
			{
				return m_Valid;
			}
			set
			{
				m_Valid = value;
			}
		}

		internal DmTransaction(DmConnInstance connInst)
		{
			FilterChain.createFilterChain(this, connInst.ConnProperty);
			m_ConnInst = connInst;
			m_il = System.Data.IsolationLevel.ReadCommitted;
		}

		internal DmTransaction(DmConnInstance connInst, System.Data.IsolationLevel il)
			: this(connInst)
		{
			m_il = il;
		}

		internal void do_Commit()
		{
			try
			{
				if (m_ConnInst.CurrentTransaction == null || m_ConnInst.CurrentTransaction.inCommitOrRollback)
				{
					CheckValid();
					CheckTransactionStatus();
					m_ConnInst.Commit(for_new_tran: false);
				}
			}
			catch (DmException ex)
			{
				if (ex.ErrorCode != 6042)
				{
					throw ex;
				}
			}
			catch (Exception ex2)
			{
				throw ex2;
			}
			finally
			{
				Clear();
			}
		}

		internal void do_Rollback()
		{
			if (m_ConnInst.CurrentTransaction != null && !m_ConnInst.CurrentTransaction.inCommitOrRollback)
			{
				return;
			}
			CheckValid();
			CheckTransactionStatus();
			try
			{
				m_ConnInst.Rollback();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				Clear();
			}
		}

		internal void do_Dispose(bool disposing)
		{
			if (m_AlreadyDisposed)
			{
				return;
			}
			m_AlreadyDisposed = true;
			GC.SuppressFinalize(this);
			base.Dispose(disposing);
			if (m_Valid)
			{
				if (m_ConnInst.ConnProperty.CompatibleMode == CompatibleMode.ORACLE)
				{
					do_Commit();
				}
				else
				{
					do_Rollback();
				}
			}
		}

		public override void Commit()
		{
			if (FilterChain == null)
			{
				do_Commit();
			}
			else
			{
				FilterChain.reset().Commit(this);
			}
		}

		public override void Rollback()
		{
			if (FilterChain == null)
			{
				do_Rollback();
			}
			else
			{
				FilterChain.reset().Rollback(this);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (FilterChain == null)
			{
				do_Dispose(disposing);
			}
			else
			{
				FilterChain.reset().Dispose(this, disposing);
			}
		}

		public void Dispose(bool disposing, bool for_ef)
		{
			if (for_ef)
			{
				do_Dispose(disposing);
			}
		}

		public new void Dispose()
		{
			do_Dispose(disposing: true);
		}

		internal void SetStmtSerial(int level)
		{
			if (level == 3)
			{
				m_StmtSerial = true;
			}
		}

		public bool GetStmtSerial()
		{
			return m_StmtSerial;
		}

		private void CheckValid()
		{
			if (!m_Valid)
			{
				throw new InvalidOperationException("此Transaction已完成，不可再用");
			}
		}

		private void CheckTransactionStatus()
		{
			Transaction current = Transaction.Current;
			if (!(current != null))
			{
				return;
			}
			bool flag = false;
			if (m_ConnInst.CurrentTransaction != null)
			{
				flag = m_ConnInst.CurrentTransaction.InRollback;
			}
			if (!flag)
			{
				TransactionStatus transactionStatus = TransactionStatus.InDoubt;
				try
				{
					transactionStatus = current.TransactionInformation.Status;
				}
				catch (TransactionException)
				{
				}
				if (transactionStatus == TransactionStatus.Aborted)
				{
					throw new TransactionAbortedException();
				}
			}
		}

		public void Clear()
		{
			m_Valid = false;
			m_stmt = null;
		}
	}
}
