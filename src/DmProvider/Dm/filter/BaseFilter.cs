using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace Dm.filter
{
	internal class BaseFilter : IFilter
	{
		public string getServerVersion(FilterChain chain, DmConnection conn)
		{
			return chain.getServerVersion(conn);
		}

		public string getDataSource(FilterChain chain, DmConnection conn)
		{
			return chain.getDataSource(conn);
		}

		public string getDatabase(FilterChain chain, DmConnection conn)
		{
			return chain.getDatabase(conn);
		}

		public int getConnectionTimeout(FilterChain chain, DmConnection conn)
		{
			return chain.getConnectionTimeout(conn);
		}

		public string getConnectionString(FilterChain chain, DmConnection conn)
		{
			return chain.getConnectionString(conn);
		}

		public DbProviderFactory getDbProviderFactory(FilterChain chain, DmConnection conn)
		{
			return chain.getDbProviderFactory(conn);
		}

		public ConnectionState getState(FilterChain chain, DmConnection conn)
		{
			return chain.getState(conn);
		}

		public DmTransaction BeginDbTransaction(FilterChain chain, DmConnection conn, System.Data.IsolationLevel isolationLevel)
		{
			return chain.BeginDbTransaction(conn, isolationLevel);
		}

		public void ChangeDatabase(FilterChain chain, DmConnection conn, string databaseName)
		{
			chain.ChangeDatabase(conn, databaseName);
		}

		public void Close(FilterChain chain, DmConnection conn)
		{
			chain.Close(conn);
		}

		public DmCommand CreateDbCommand(FilterChain chain, DmConnection conn)
		{
			return chain.CreateDbCommand(conn);
		}

		public void EnlistTransaction(FilterChain chain, DmConnection conn, Transaction transaction)
		{
			chain.EnlistTransaction(conn, transaction);
		}

		public DataTable GetSchema(FilterChain chain, DmConnection conn)
		{
			return chain.GetSchema(conn);
		}

		public DataTable GetSchema(FilterChain chain, DmConnection conn, string collectionName, string[] restrictionValues)
		{
			return chain.GetSchema(conn, collectionName, restrictionValues);
		}

		public DataTable GetSchema(FilterChain chain, DmConnection conn, string collectionName)
		{
			return chain.GetSchema(conn, collectionName);
		}

		public void Open(FilterChain chain, DmConnection conn)
		{
			chain.Open(conn);
		}

		public bool getDesignTimeVisible(FilterChain chain, DmCommand command)
		{
			return chain.getDesignTimeVisible(command);
		}

		public void setDesignTimeVisible(FilterChain chain, DmCommand command, bool value)
		{
			chain.setDesignTimeVisible(command, value);
		}

		public CommandType getCommandType(FilterChain chain, DmCommand command)
		{
			return chain.getCommandType(command);
		}

		public void setCommandType(FilterChain chain, DmCommand command, CommandType value)
		{
			chain.setCommandType(command, value);
		}

		public int getCommandTimeout(FilterChain chain, DmCommand command)
		{
			return chain.getCommandTimeout(command);
		}

		public void setCommandTimeout(FilterChain chain, DmCommand command, int value)
		{
			chain.setCommandTimeout(command, value);
		}

		public string getCommandText(FilterChain chain, DmCommand command)
		{
			return chain.getCommandText(command);
		}

		public void setCommandText(FilterChain chain, DmCommand command, string value)
		{
			chain.setCommandText(command, value);
		}

		public UpdateRowSource getUpdatedRowSource(FilterChain chain, DmCommand command)
		{
			return chain.getUpdatedRowSource(command);
		}

		public void setUpdatedRowSource(FilterChain chain, DmCommand command, UpdateRowSource value)
		{
			chain.setUpdatedRowSource(command, value);
		}

		public DmConnection getDbConnection(FilterChain chain, DmCommand command)
		{
			return chain.getDbConnection(command);
		}

		public void setDbConnection(FilterChain chain, DmCommand command, DmConnection value)
		{
			chain.setDbConnection(command, value);
		}

		public DmParameterCollection getDbParameterCollection(FilterChain chain, DmCommand command)
		{
			return chain.getDbParameterCollection(command);
		}

		public DmTransaction getDbTransaction(FilterChain chain, DmCommand command)
		{
			return chain.getDbTransaction(command);
		}

		public void setDbTransaction(FilterChain chain, DmCommand command, DmTransaction value)
		{
			chain.setDbTransaction(command, value);
		}

		public void Cancel(FilterChain chain, DmCommand command)
		{
			chain.Cancel(command);
		}

		public int ExecuteNonQuery(FilterChain chain, DmCommand command)
		{
			return chain.ExecuteNonQuery(command);
		}

		public object ExecuteScalar(FilterChain chain, DmCommand command)
		{
			return chain.ExecuteScalar(command);
		}

		public void Prepare(FilterChain chain, DmCommand command)
		{
			chain.Prepare(command);
		}

		public DmParameter CreateDbParameter(FilterChain chain, DmCommand command)
		{
			return chain.CreateDbParameter(command);
		}

		public DmDataReader ExecuteDbDataReader(FilterChain chain, DmCommand command, CommandBehavior behavior)
		{
			return chain.ExecuteDbDataReader(command, behavior);
		}

		public System.Data.IsolationLevel getIsolationLevel(FilterChain chain, DmTransaction transaction)
		{
			return chain.getIsolationLevel(transaction);
		}

		public DmConnection getDbConnection(FilterChain chain, DmTransaction transaction)
		{
			return chain.getDbConnection(transaction);
		}

		public void Commit(FilterChain chain, DmTransaction transaction)
		{
			chain.Commit(transaction);
		}

		public void Rollback(FilterChain chain, DmTransaction transaction)
		{
			chain.Rollback(transaction);
		}

		public void Dispose(FilterChain chain, DmTransaction transaction, bool disposing)
		{
			chain.Dispose(transaction, disposing);
		}

		public object getThis(FilterChain chain, DmDataReader dataReader, int index)
		{
			return chain.getThis(dataReader, index);
		}

		public object getThis(FilterChain chain, DmDataReader dataReader, string name)
		{
			return chain.getThis(dataReader, name);
		}

		public int getDepth(FilterChain chain, DmDataReader dataReader)
		{
			return chain.getDepth(dataReader);
		}

		public bool getHasRows(FilterChain chain, DmDataReader dataReader)
		{
			return chain.getHasRows(dataReader);
		}

		public int getVisibleFieldCount(FilterChain chain, DmDataReader dataReader)
		{
			return chain.getVisibleFieldCount(dataReader);
		}

		public int getRecordsAffected(FilterChain chain, DmDataReader dataReader)
		{
			return chain.getRecordsAffected(dataReader);
		}

		public bool getIsClosed(FilterChain chain, DmDataReader dataReader)
		{
			return chain.getIsClosed(dataReader);
		}

		public int getFieldCount(FilterChain chain, DmDataReader dataReader)
		{
			return chain.getFieldCount(dataReader);
		}

		public void Close(FilterChain chain, DmDataReader dataReader)
		{
			chain.Close(dataReader);
		}

		public bool GetBoolean(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetBoolean(dataReader, ordinal);
		}

		public byte GetByte(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetByte(dataReader, ordinal);
		}

		public long GetBytes(FilterChain chain, DmDataReader dataReader, int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
		{
			return chain.GetBytes(dataReader, ordinal, dataOffset, buffer, bufferOffset, length);
		}

		public char GetChar(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetChar(dataReader, ordinal);
		}

		public long GetChars(FilterChain chain, DmDataReader dataReader, int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
		{
			return chain.GetChars(dataReader, ordinal, dataOffset, buffer, bufferOffset, length);
		}

		public string GetDataTypeName(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetDataTypeName(dataReader, ordinal);
		}

		public DateTime GetDateTime(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetDateTime(dataReader, ordinal);
		}

		public decimal GetDecimal(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetDecimal(dataReader, ordinal);
		}

		public double GetDouble(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetDouble(dataReader, ordinal);
		}

		public IEnumerator GetEnumerator(FilterChain chain, DmDataReader dataReader)
		{
			return chain.GetEnumerator(dataReader);
		}

		public Type GetFieldType(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetFieldType(dataReader, ordinal);
		}

		public float GetFloat(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetFloat(dataReader, ordinal);
		}

		public Guid GetGuid(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetGuid(dataReader, ordinal);
		}

		public short GetInt16(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetInt16(dataReader, ordinal);
		}

		public int GetInt32(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetInt32(dataReader, ordinal);
		}

		public long GetInt64(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetInt64(dataReader, ordinal);
		}

		public string GetName(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetName(dataReader, ordinal);
		}

		public int GetOrdinal(FilterChain chain, DmDataReader dataReader, string name)
		{
			return chain.GetOrdinal(dataReader, name);
		}

		public Type GetProviderSpecificFieldType(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetProviderSpecificFieldType(dataReader, ordinal);
		}

		public object GetProviderSpecificValue(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetProviderSpecificValue(dataReader, ordinal);
		}

		public int GetProviderSpecificValues(FilterChain chain, DmDataReader dataReader, object[] values)
		{
			return chain.GetProviderSpecificValues(dataReader, values);
		}

		public DataTable GetSchemaTable(FilterChain chain, DmDataReader dataReader)
		{
			return chain.GetSchemaTable(dataReader);
		}

		public string GetString(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetString(dataReader, ordinal);
		}

		public object GetValue(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetValue(dataReader, ordinal);
		}

		public int GetValues(FilterChain chain, DmDataReader dataReader, object[] values)
		{
			return chain.GetValues(dataReader, values);
		}

		public bool IsDBNull(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.IsDBNull(dataReader, ordinal);
		}

		public bool NextResult(FilterChain chain, DmDataReader dataReader)
		{
			return chain.NextResult(dataReader);
		}

		public bool Read(FilterChain chain, DmDataReader dataReader)
		{
			return chain.Read(dataReader);
		}

		public void Dispose(FilterChain chain, DmDataReader dataReader, bool disposing)
		{
			chain.Dispose(dataReader, disposing);
		}

		public DmDataReader GetDbDataReader(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			return chain.GetDbDataReader(dataReader, ordinal);
		}

		public object getSyncRoot(FilterChain chain, DmParameterCollection parameterCollection)
		{
			return chain.getSyncRoot(parameterCollection);
		}

		public bool getIsSynchronized(FilterChain chain, DmParameterCollection parameterCollection)
		{
			return chain.getIsSynchronized(parameterCollection);
		}

		public bool getIsReadOnly(FilterChain chain, DmParameterCollection parameterCollection)
		{
			return chain.getIsReadOnly(parameterCollection);
		}

		public bool getIsFixedSize(FilterChain chain, DmParameterCollection parameterCollection)
		{
			return chain.getIsFixedSize(parameterCollection);
		}

		public int getCount(FilterChain chain, DmParameterCollection parameterCollection)
		{
			return chain.getCount(parameterCollection);
		}

		public int Add(FilterChain chain, DmParameterCollection parameterCollection, object value)
		{
			return chain.Add(parameterCollection, value);
		}

		public void AddRange(FilterChain chain, DmParameterCollection parameterCollection, Array values)
		{
			chain.AddRange(parameterCollection, values);
		}

		public void Clear(FilterChain chain, DmParameterCollection parameterCollection)
		{
			chain.Clear(parameterCollection);
		}

		public bool Contains(FilterChain chain, DmParameterCollection parameterCollection, object value)
		{
			return chain.Contains(parameterCollection, value);
		}

		public bool Contains(FilterChain chain, DmParameterCollection parameterCollection, string value)
		{
			return chain.Contains(parameterCollection, value);
		}

		public void CopyTo(FilterChain chain, DmParameterCollection parameterCollection, Array array, int index)
		{
			chain.CopyTo(parameterCollection, array, index);
		}

		public IEnumerator GetEnumerator(FilterChain chain, DmParameterCollection parameterCollection)
		{
			return chain.GetEnumerator(parameterCollection);
		}

		public int IndexOf(FilterChain chain, DmParameterCollection parameterCollection, object value)
		{
			return chain.IndexOf(parameterCollection, value);
		}

		public int IndexOf(FilterChain chain, DmParameterCollection parameterCollection, string parameterName)
		{
			return chain.IndexOf(parameterCollection, parameterName);
		}

		public void Insert(FilterChain chain, DmParameterCollection parameterCollection, int index, object value)
		{
			chain.Insert(parameterCollection, index, value);
		}

		public void Remove(FilterChain chain, DmParameterCollection parameterCollection, object value)
		{
			chain.Remove(parameterCollection, value);
		}

		public void RemoveAt(FilterChain chain, DmParameterCollection parameterCollection, string parameterName)
		{
			chain.RemoveAt(parameterCollection, parameterName);
		}

		public void RemoveAt(FilterChain chain, DmParameterCollection parameterCollection, int index)
		{
			chain.RemoveAt(parameterCollection, index);
		}

		public DmParameter GetParameter(FilterChain chain, DmParameterCollection parameterCollection, string parameterName)
		{
			return chain.GetParameter(parameterCollection, parameterName);
		}

		public DmParameter GetParameter(FilterChain chain, DmParameterCollection parameterCollection, int index)
		{
			return chain.GetParameter(parameterCollection, index);
		}

		public void SetParameter(FilterChain chain, DmParameterCollection parameterCollection, int index, DmParameter value)
		{
			chain.SetParameter(parameterCollection, index, value);
		}

		public void SetParameter(FilterChain chain, DmParameterCollection parameterCollection, string parameterName, DmParameter value)
		{
			chain.SetParameter(parameterCollection, parameterName, value);
		}

		public DbType getDbType(FilterChain chain, DmParameter parameter)
		{
			return chain.getDbType(parameter);
		}

		public void setDbType(FilterChain chain, DmParameter parameter, DbType value)
		{
			chain.setDbType(parameter, value);
		}

		public ParameterDirection getDirection(FilterChain chain, DmParameter parameter)
		{
			return chain.getDirection(parameter);
		}

		public void setDirection(FilterChain chain, DmParameter parameter, ParameterDirection value)
		{
			chain.setDirection(parameter, value);
		}

		public bool getIsNullable(FilterChain chain, DmParameter parameter)
		{
			return chain.getIsNullable(parameter);
		}

		public void setIsNullable(FilterChain chain, DmParameter parameter, bool value)
		{
			chain.setIsNullable(parameter, value);
		}

		public string getParameterName(FilterChain chain, DmParameter parameter)
		{
			return chain.getParameterName(parameter);
		}

		public void setParameterName(FilterChain chain, DmParameter parameter, string value)
		{
			chain.setParameterName(parameter, value);
		}

		public byte getPrecision(FilterChain chain, DmParameter parameter)
		{
			return chain.getPrecision(parameter);
		}

		public void setPrecision(FilterChain chain, DmParameter parameter, byte value)
		{
			chain.setPrecision(parameter, value);
		}

		public byte getScale(FilterChain chain, DmParameter parameter)
		{
			return chain.getScale(parameter);
		}

		public void setScale(FilterChain chain, DmParameter parameter, byte value)
		{
			chain.setScale(parameter, value);
		}

		public int getSize(FilterChain chain, DmParameter parameter)
		{
			return chain.getSize(parameter);
		}

		public void setSize(FilterChain chain, DmParameter parameter, int value)
		{
			chain.setSize(parameter, value);
		}

		public string getSourceColumn(FilterChain chain, DmParameter parameter)
		{
			return chain.getSourceColumn(parameter);
		}

		public void setSourceColumn(FilterChain chain, DmParameter parameter, string value)
		{
			chain.setSourceColumn(parameter, value);
		}

		public bool getSourceColumnNullMapping(FilterChain chain, DmParameter parameter)
		{
			return chain.getSourceColumnNullMapping(parameter);
		}

		public void setSourceColumnNullMapping(FilterChain chain, DmParameter parameter, bool value)
		{
			chain.setSourceColumnNullMapping(parameter, value);
		}

		public DataRowVersion getSourceVersion(FilterChain chain, DmParameter parameter)
		{
			return chain.getSourceVersion(parameter);
		}

		public void setSourceVersion(FilterChain chain, DmParameter parameter, DataRowVersion value)
		{
			chain.setSourceVersion(parameter, value);
		}

		public object getValue(FilterChain chain, DmParameter parameter)
		{
			return chain.getValue(parameter);
		}

		public void setValue(FilterChain chain, DmParameter parameter, object value)
		{
			chain.setValue(parameter, value);
		}

		public void ResetDbType(FilterChain chain, DmParameter parameter)
		{
			chain.ResetDbType(parameter);
		}

		public int getUpdateBatchSize(FilterChain chain, DmDataAdapter dataAdapter)
		{
			return chain.getUpdateBatchSize(dataAdapter);
		}

		public void setUpdateBatchSize(FilterChain chain, DmDataAdapter dataAdapter, int value)
		{
			chain.setUpdateBatchSize(dataAdapter, value);
		}

		public int AddToBatch(FilterChain chain, DmDataAdapter dataAdapter, DmCommand command)
		{
			return chain.AddToBatch(dataAdapter, command);
		}

		public void ClearBatch(FilterChain chain, DmDataAdapter dataAdapter)
		{
			chain.ClearBatch(dataAdapter);
		}

		public RowUpdatedEventArgs CreateRowUpdatedEvent(FilterChain chain, DmDataAdapter dataAdapter, DataRow dataRow, DmCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return chain.CreateRowUpdatedEvent(dataAdapter, dataRow, command, statementType, tableMapping);
		}

		public RowUpdatingEventArgs CreateRowUpdatingEvent(FilterChain chain, DmDataAdapter dataAdapter, DataRow dataRow, DmCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return chain.CreateRowUpdatingEvent(dataAdapter, dataRow, command, statementType, tableMapping);
		}

		public int ExecuteBatch(FilterChain chain, DmDataAdapter dataAdapter)
		{
			return chain.ExecuteBatch(dataAdapter);
		}

		public int Fill(FilterChain chain, DmDataAdapter dataAdapter, DataTable[] dataTables, int startRecord, int maxRecords, DmCommand command, CommandBehavior behavior)
		{
			return chain.Fill(dataAdapter, dataTables, startRecord, maxRecords, command, behavior);
		}

		public int Fill(FilterChain chain, DmDataAdapter dataAdapter, DataTable dataTable, DmCommand command, CommandBehavior behavior)
		{
			return chain.Fill(dataAdapter, dataTable, command, behavior);
		}

		public int Fill(FilterChain chain, DmDataAdapter dataAdapter, DataSet dataSet, int startRecord, int maxRecords, string srcTable, DmCommand command, CommandBehavior behavior)
		{
			return chain.Fill(dataAdapter, dataSet, startRecord, maxRecords, srcTable, command, behavior);
		}

		public DataTable FillSchema(FilterChain chain, DmDataAdapter dataAdapter, DataTable dataTable, SchemaType schemaType, DmCommand command, CommandBehavior behavior)
		{
			return chain.FillSchema(dataAdapter, dataTable, schemaType, command, behavior);
		}

		public DataTable[] FillSchema(FilterChain chain, DmDataAdapter dataAdapter, DataSet dataSet, SchemaType schemaType, DmCommand command, string srcTable, CommandBehavior behavior)
		{
			return chain.FillSchema(dataAdapter, dataSet, schemaType, command, srcTable, behavior);
		}

		public IDataParameter GetBatchedParameter(FilterChain chain, DmDataAdapter dataAdapter, int commandIdentifier, int parameterIndex)
		{
			return chain.GetBatchedParameter(dataAdapter, commandIdentifier, parameterIndex);
		}

		public bool GetBatchedRecordsAffected(FilterChain chain, DmDataAdapter dataAdapter, int commandIdentifier, out int recordsAffected, out Exception error)
		{
			return chain.GetBatchedRecordsAffected(dataAdapter, commandIdentifier, out recordsAffected, out error);
		}

		public void InitializeBatching(FilterChain chain, DmDataAdapter dataAdapter)
		{
			chain.InitializeBatching(dataAdapter);
		}

		public void OnRowUpdated(FilterChain chain, DmDataAdapter dataAdapter, RowUpdatedEventArgs value)
		{
			chain.OnRowUpdated(dataAdapter, value);
		}

		public void OnRowUpdating(FilterChain chain, DmDataAdapter dataAdapter, RowUpdatingEventArgs value)
		{
			chain.OnRowUpdating(dataAdapter, value);
		}

		public void TerminateBatching(FilterChain chain, DmDataAdapter dataAdapter)
		{
			chain.TerminateBatching(dataAdapter);
		}

		public int Update(FilterChain chain, DmDataAdapter dataAdapter, DataRow[] dataRows, DataTableMapping tableMapping)
		{
			return chain.Update(dataAdapter, dataRows, tableMapping);
		}

		public string getQuoteSuffix(FilterChain chain, DmCommandBuilder commandBuilder)
		{
			return chain.getQuoteSuffix(commandBuilder);
		}

		public void setQuoteSuffix(FilterChain chain, DmCommandBuilder commandBuilder, string value)
		{
			chain.setQuoteSuffix(commandBuilder, value);
		}

		public string getQuotePrefix(FilterChain chain, DmCommandBuilder commandBuilder)
		{
			return chain.getQuotePrefix(commandBuilder);
		}

		public void setQuotePrefix(FilterChain chain, DmCommandBuilder commandBuilder, string value)
		{
			chain.setQuotePrefix(commandBuilder, value);
		}

		public string getCatalogSeparator(FilterChain chain, DmCommandBuilder commandBuilder)
		{
			return chain.getCatalogSeparator(commandBuilder);
		}

		public void setCatalogSeparator(FilterChain chain, DmCommandBuilder commandBuilder, string value)
		{
			chain.setCatalogSeparator(commandBuilder, value);
		}

		public CatalogLocation getCatalogLocation(FilterChain chain, DmCommandBuilder commandBuilder)
		{
			return chain.getCatalogLocation(commandBuilder);
		}

		public void setCatalogLocation(FilterChain chain, DmCommandBuilder commandBuilder, CatalogLocation value)
		{
			chain.setCatalogLocation(commandBuilder, value);
		}

		public ConflictOption getConflictOption(FilterChain chain, DmCommandBuilder commandBuilder)
		{
			return chain.getConflictOption(commandBuilder);
		}

		public void setConflictOption(FilterChain chain, DmCommandBuilder commandBuilder, ConflictOption value)
		{
			chain.setConflictOption(commandBuilder, value);
		}

		public string getSchemaSeparator(FilterChain chain, DmCommandBuilder commandBuilder)
		{
			return chain.getSchemaSeparator(commandBuilder);
		}

		public void setSchemaSeparator(FilterChain chain, DmCommandBuilder commandBuilder, string value)
		{
			chain.setSchemaSeparator(commandBuilder, value);
		}

		public string QuoteIdentifier(FilterChain chain, DmCommandBuilder commandBuilder, string unquotedIdentifier)
		{
			return chain.QuoteIdentifier(commandBuilder, unquotedIdentifier);
		}

		public void RefreshSchema(FilterChain chain, DmCommandBuilder commandBuilder)
		{
			chain.RefreshSchema(commandBuilder);
		}

		public string UnquoteIdentifier(FilterChain chain, DmCommandBuilder commandBuilder, string quotedIdentifier)
		{
			return chain.UnquoteIdentifier(commandBuilder, quotedIdentifier);
		}

		public void ApplyParameterInfo(FilterChain chain, DmCommandBuilder commandBuilder, DmParameter parameter, DataRow row, StatementType statementType, bool whereClause)
		{
			chain.ApplyParameterInfo(commandBuilder, parameter, row, statementType, whereClause);
		}

		public string GetParameterName(FilterChain chain, DmCommandBuilder commandBuilder, int parameterOrdinal)
		{
			return chain.GetParameterName(commandBuilder, parameterOrdinal);
		}

		public string GetParameterName(FilterChain chain, DmCommandBuilder commandBuilder, string parameterName)
		{
			return chain.GetParameterName(commandBuilder, parameterName);
		}

		public string GetParameterPlaceholder(FilterChain chain, DmCommandBuilder commandBuilder, int parameterOrdinal)
		{
			return chain.GetParameterPlaceholder(commandBuilder, parameterOrdinal);
		}

		public DataTable GetSchemaTable(FilterChain chain, DmCommandBuilder commandBuilder, DmCommand sourceCommand)
		{
			return chain.GetSchemaTable(commandBuilder, sourceCommand);
		}

		public DmCommand InitializeCommand(FilterChain chain, DmCommandBuilder commandBuilder, DmCommand command)
		{
			return chain.InitializeCommand(commandBuilder, command);
		}

		public void SetRowUpdatingHandler(FilterChain chain, DmCommandBuilder commandBuilder, DmDataAdapter adapter)
		{
			chain.SetRowUpdatingHandler(commandBuilder, adapter);
		}

		public object getThis(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword)
		{
			return chain.getThis(connectionStringBuilder, keyword);
		}

		public void setThis(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword, object value)
		{
			chain.setThis(connectionStringBuilder, keyword, value);
		}

		public bool getIsFixedSize(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder)
		{
			return chain.getIsFixedSize(connectionStringBuilder);
		}

		public int getCount(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder)
		{
			return chain.getCount(connectionStringBuilder);
		}

		public ICollection getKeys(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder)
		{
			return chain.getKeys(connectionStringBuilder);
		}

		public ICollection getValues(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder)
		{
			return chain.getValues(connectionStringBuilder);
		}

		public void Clear(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder)
		{
			chain.Clear(connectionStringBuilder);
		}

		public bool ContainsKey(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword)
		{
			return chain.ContainsKey(connectionStringBuilder, keyword);
		}

		public bool EquivalentTo(FilterChain chain, DmConnectionStringBuilder sourceConnectionStringBuilder, DmConnectionStringBuilder destConnectionStringBuilder)
		{
			return chain.EquivalentTo(sourceConnectionStringBuilder, destConnectionStringBuilder);
		}

		public bool Remove(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword)
		{
			return chain.Remove(connectionStringBuilder, keyword);
		}

		public bool ShouldSerialize(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword)
		{
			return chain.ShouldSerialize(connectionStringBuilder, keyword);
		}

		public bool TryGetValue(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword, out object value)
		{
			return chain.TryGetValue(connectionStringBuilder, keyword, out value);
		}

		public void GetProperties(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, Hashtable propertyDescriptors)
		{
			chain.GetProperties(connectionStringBuilder, propertyDescriptors);
		}
	}
}
