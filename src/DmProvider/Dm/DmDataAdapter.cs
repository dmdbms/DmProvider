using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using Dm.filter;
using Dm.filter.log;
using Dm.filter.reconnect;
using Dm.filter.rw;

namespace Dm
{
	public sealed class DmDataAdapter : DbDataAdapter, IDbDataAdapter, IDataAdapter, ICloneable, IFilterInfo
	{
		internal long id = -1L;

		internal static long idGenerator = 0L;

		private static readonly string ClassName = "DmDataAdapter";

		private DmCommand m_SelectCommand;

		private DmCommand m_UpdateCommand;

		private DmCommand m_DeleteCommand;

		private DmCommand m_InsertCommand;

		private int m_UpdateBatchSize;

		private List<DmCommand> m_CommandBatch;

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

		internal int do_UpdateBatchSize
		{
			get
			{
				return m_UpdateBatchSize;
			}
			set
			{
				_ = 0;
				m_UpdateBatchSize = value;
			}
		}

		public override int UpdateBatchSize
		{
			get
			{
				if (FilterChain == null)
				{
					return do_UpdateBatchSize;
				}
				return FilterChain.reset().getUpdateBatchSize(this);
			}
			set
			{
				if (FilterChain == null)
				{
					do_UpdateBatchSize = value;
				}
				else
				{
					FilterChain.reset().setUpdateBatchSize(this, value);
				}
			}
		}

		ITableMappingCollection IDataAdapter.TableMappings => base.TableMappings;

		IDbCommand IDbDataAdapter.DeleteCommand
		{
			get
			{
				return DeleteCommand;
			}
			set
			{
				DeleteCommand = (DmCommand)value;
			}
		}

		public new DmCommand DeleteCommand
		{
			get
			{
				DmTrace.TracePropertyGet(TraceLevel.Debug, ClassName, "DeleteCommand");
				return m_DeleteCommand;
			}
			set
			{
				DmTrace.TracePropertySet(TraceLevel.Debug, ClassName, "DeleteCommand");
				m_DeleteCommand = value;
			}
		}

		IDbCommand IDbDataAdapter.SelectCommand
		{
			get
			{
				return SelectCommand;
			}
			set
			{
				SelectCommand = (DmCommand)value;
			}
		}

		public new DmCommand SelectCommand
		{
			get
			{
				DmTrace.TracePropertyGet(TraceLevel.Debug, ClassName, "SelectCommand");
				return m_SelectCommand;
			}
			set
			{
				DmTrace.TracePropertySet(TraceLevel.Debug, ClassName, "SelectCommand");
				m_SelectCommand = value;
			}
		}

		IDbCommand IDbDataAdapter.UpdateCommand
		{
			get
			{
				return UpdateCommand;
			}
			set
			{
				UpdateCommand = (DmCommand)value;
			}
		}

		public new DmCommand UpdateCommand
		{
			get
			{
				DmTrace.TracePropertyGet(TraceLevel.Debug, ClassName, "UpdateCommand");
				return m_UpdateCommand;
			}
			set
			{
				DmTrace.TracePropertySet(TraceLevel.Debug, ClassName, "UpdateCommand");
				m_UpdateCommand = value;
			}
		}

		IDbCommand IDbDataAdapter.InsertCommand
		{
			get
			{
				return InsertCommand;
			}
			set
			{
				InsertCommand = (DmCommand)value;
			}
		}

		public new DmCommand InsertCommand
		{
			get
			{
				DmTrace.TracePropertyGet(TraceLevel.Debug, ClassName, "InsertCommand");
				return m_InsertCommand;
			}
			set
			{
				DmTrace.TracePropertySet(TraceLevel.Debug, ClassName, "InsertCommand");
				m_InsertCommand = value;
			}
		}

		public event DmRowUpdatedEventHandler RowUpdated;

		public event DmRowUpdatingEventHandler RowUpdating;

		public DmDataAdapter()
		{
			FilterChain.createFilterChain(this);
			m_UpdateBatchSize = 1;
		}

		public DmDataAdapter(DmCommand selectCommand)
			: this()
		{
			m_SelectCommand = selectCommand;
			if (m_SelectCommand.Statement != null)
			{
				m_SelectCommand.Statement.Clear();
			}
		}

		public DmDataAdapter(string selectCommandText, DmConnection selectConnection)
			: this(new DmCommand(selectCommandText, selectConnection))
		{
		}

		public DmDataAdapter(string selectCommandText, string selectConnectionString)
			: this(selectCommandText, new DmConnection(selectConnectionString))
		{
		}

		internal int do_AddToBatch(DmCommand command)
		{
			DmCommand item = command.Clone();
			m_CommandBatch.Add(item);
			return m_CommandBatch.Count - 1;
		}

		internal void do_ClearBatch()
		{
			m_CommandBatch.Clear();
		}

		internal RowUpdatedEventArgs do_CreateRowUpdatedEvent(DataRow dataRow, DmCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new DmSqlRowUpdatedEventArgs(dataRow, command, statementType, tableMapping);
		}

		internal RowUpdatingEventArgs do_CreateRowUpdatingEvent(DataRow dataRow, DmCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new DmSqlRowUpdatingEventArgs(dataRow, command, statementType, tableMapping);
		}

		internal int do_ExecuteBatch()
		{
			int num = 0;
			int num2 = 0;
			while (num2 < m_CommandBatch.Count)
			{
				DmCommand dmCommand = m_CommandBatch[num2++];
				num += dmCommand.do_ExecuteNonQuery();
			}
			return num;
		}

		internal int do_Fill(DataTable[] dataTables, int startRecord, int maxRecords, DmCommand command, CommandBehavior behavior)
		{
			return base.Fill(dataTables, startRecord, maxRecords, command, behavior);
		}

		internal int do_Fill(DataTable dataTable, DmCommand command, CommandBehavior behavior)
		{
			return base.Fill(dataTable, command, behavior);
		}

		internal int do_Fill(DataSet dataSet, int startRecord, int maxRecords, string srcTable, DmCommand command, CommandBehavior behavior)
		{
			return base.Fill(dataSet, startRecord, maxRecords, srcTable, command, behavior);
		}

		internal DataTable do_FillSchema(DataTable dataTable, SchemaType schemaType, DmCommand command, CommandBehavior behavior)
		{
			return base.FillSchema(dataTable, schemaType, command, behavior);
		}

		internal DataTable[] do_FillSchema(DataSet dataSet, SchemaType schemaType, DmCommand command, string srcTable, CommandBehavior behavior)
		{
			return base.FillSchema(dataSet, schemaType, command, srcTable, behavior);
		}

		internal IDataParameter do_GetBatchedParameter(int commandIdentifier, int parameterIndex)
		{
			return base.GetBatchedParameter(commandIdentifier, parameterIndex);
		}

		internal bool do_GetBatchedRecordsAffected(int commandIdentifier, out int recordsAffected, out Exception error)
		{
			return base.GetBatchedRecordsAffected(commandIdentifier, out recordsAffected, out error);
		}

		internal void do_InitializeBatching()
		{
			m_CommandBatch = new List<DmCommand>();
		}

		internal void do_OnRowUpdated(RowUpdatedEventArgs value)
		{
			if (this.RowUpdated != null && value is DmSqlRowUpdatedEventArgs)
			{
				this.RowUpdated(this, (DmSqlRowUpdatedEventArgs)value);
			}
		}

		internal void do_OnRowUpdating(RowUpdatingEventArgs value)
		{
			if (this.RowUpdating != null && value is DmSqlRowUpdatingEventArgs)
			{
				this.RowUpdating(this, (DmSqlRowUpdatingEventArgs)value);
			}
		}

		internal void do_TerminateBatching()
		{
			do_ClearBatch();
			m_CommandBatch = null;
		}

		internal int do_Update(DataRow[] dataRows, DataTableMapping tableMapping)
		{
			return base.Update(dataRows, tableMapping);
		}

		protected override int AddToBatch(IDbCommand command)
		{
			if (FilterChain == null)
			{
				return do_AddToBatch((DmCommand)command);
			}
			return FilterChain.reset().AddToBatch(this, (DmCommand)command);
		}

		protected override void ClearBatch()
		{
			if (FilterChain == null)
			{
				do_ClearBatch();
			}
			else
			{
				FilterChain.reset().ClearBatch(this);
			}
		}

		protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			if (FilterChain == null)
			{
				return do_CreateRowUpdatedEvent(dataRow, (DmCommand)command, statementType, tableMapping);
			}
			return FilterChain.reset().CreateRowUpdatedEvent(this, dataRow, (DmCommand)command, statementType, tableMapping);
		}

		protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			if (FilterChain == null)
			{
				return do_CreateRowUpdatingEvent(dataRow, (DmCommand)command, statementType, tableMapping);
			}
			return FilterChain.reset().CreateRowUpdatingEvent(this, dataRow, (DmCommand)command, statementType, tableMapping);
		}

		protected override int ExecuteBatch()
		{
			if (FilterChain == null)
			{
				return do_ExecuteBatch();
			}
			return FilterChain.reset().ExecuteBatch(this);
		}

		protected override int Fill(DataTable[] dataTables, int startRecord, int maxRecords, IDbCommand command, CommandBehavior behavior)
		{
			if (FilterChain == null)
			{
				return do_Fill(dataTables, startRecord, maxRecords, (DmCommand)command, behavior);
			}
			return FilterChain.reset().Fill(this, dataTables, startRecord, maxRecords, (DmCommand)command, behavior);
		}

		protected override int Fill(DataTable dataTable, IDbCommand command, CommandBehavior behavior)
		{
			if (FilterChain == null)
			{
				return do_Fill(dataTable, (DmCommand)command, behavior);
			}
			return FilterChain.reset().Fill(this, dataTable, (DmCommand)command, behavior);
		}

		protected override int Fill(DataSet dataSet, int startRecord, int maxRecords, string srcTable, IDbCommand command, CommandBehavior behavior)
		{
			if (FilterChain == null)
			{
				return do_Fill(dataSet, startRecord, maxRecords, srcTable, (DmCommand)command, behavior);
			}
			return FilterChain.reset().Fill(this, dataSet, startRecord, maxRecords, srcTable, (DmCommand)command, behavior);
		}

		protected override DataTable FillSchema(DataTable dataTable, SchemaType schemaType, IDbCommand command, CommandBehavior behavior)
		{
			if (FilterChain == null)
			{
				return do_FillSchema(dataTable, schemaType, (DmCommand)command, behavior);
			}
			return FilterChain.reset().FillSchema(this, dataTable, schemaType, (DmCommand)command, behavior);
		}

		protected override DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType, IDbCommand command, string srcTable, CommandBehavior behavior)
		{
			if (FilterChain == null)
			{
				return do_FillSchema(dataSet, schemaType, (DmCommand)command, srcTable, behavior);
			}
			return FilterChain.reset().FillSchema(this, dataSet, schemaType, (DmCommand)command, srcTable, behavior);
		}

		protected override IDataParameter GetBatchedParameter(int commandIdentifier, int parameterIndex)
		{
			if (FilterChain == null)
			{
				return do_GetBatchedParameter(commandIdentifier, parameterIndex);
			}
			return FilterChain.reset().GetBatchedParameter(this, commandIdentifier, parameterIndex);
		}

		protected override bool GetBatchedRecordsAffected(int commandIdentifier, out int recordsAffected, out Exception error)
		{
			if (FilterChain == null)
			{
				return do_GetBatchedRecordsAffected(commandIdentifier, out recordsAffected, out error);
			}
			return FilterChain.reset().GetBatchedRecordsAffected(this, commandIdentifier, out recordsAffected, out error);
		}

		protected override void InitializeBatching()
		{
			if (FilterChain == null)
			{
				do_InitializeBatching();
			}
			else
			{
				FilterChain.reset().InitializeBatching(this);
			}
		}

		protected override void OnRowUpdated(RowUpdatedEventArgs value)
		{
			if (FilterChain == null)
			{
				do_OnRowUpdated(value);
			}
			else
			{
				FilterChain.reset().OnRowUpdated(this, value);
			}
		}

		protected override void OnRowUpdating(RowUpdatingEventArgs value)
		{
			if (FilterChain == null)
			{
				do_OnRowUpdating(value);
			}
			else
			{
				FilterChain.reset().OnRowUpdating(this, value);
			}
		}

		protected override void TerminateBatching()
		{
			if (FilterChain == null)
			{
				do_TerminateBatching();
			}
			else
			{
				FilterChain.reset().TerminateBatching(this);
			}
		}

		protected override int Update(DataRow[] dataRows, DataTableMapping tableMapping)
		{
			if (FilterChain == null)
			{
				return do_Update(dataRows, tableMapping);
			}
			return FilterChain.reset().Update(this, dataRows, tableMapping);
		}
	}
}
