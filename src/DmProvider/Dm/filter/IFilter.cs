using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace Dm.filter
{
	internal interface IFilter
	{
		string getServerVersion(FilterChain chain, DmConnection conn);

		string getDataSource(FilterChain chain, DmConnection conn);

		string getDatabase(FilterChain chain, DmConnection conn);

		int getConnectionTimeout(FilterChain chain, DmConnection conn);

		string getConnectionString(FilterChain chain, DmConnection conn);

		DbProviderFactory getDbProviderFactory(FilterChain chain, DmConnection conn);

		ConnectionState getState(FilterChain chain, DmConnection conn);

		DmTransaction BeginDbTransaction(FilterChain chain, DmConnection conn, System.Data.IsolationLevel isolationLevel);

		void ChangeDatabase(FilterChain chain, DmConnection conn, string databaseName);

		void Close(FilterChain chain, DmConnection conn);

		DmCommand CreateDbCommand(FilterChain chain, DmConnection conn);

		void EnlistTransaction(FilterChain chain, DmConnection conn, Transaction transaction);

		DataTable GetSchema(FilterChain chain, DmConnection conn);

		DataTable GetSchema(FilterChain chain, DmConnection conn, string collectionName, string[] restrictionValues);

		DataTable GetSchema(FilterChain chain, DmConnection conn, string collectionName);

		void Open(FilterChain chain, DmConnection conn);

		bool getDesignTimeVisible(FilterChain chain, DmCommand command);

		void setDesignTimeVisible(FilterChain chain, DmCommand command, bool value);

		CommandType getCommandType(FilterChain chain, DmCommand command);

		void setCommandType(FilterChain chain, DmCommand command, CommandType value);

		int getCommandTimeout(FilterChain chain, DmCommand command);

		void setCommandTimeout(FilterChain chain, DmCommand command, int value);

		string getCommandText(FilterChain chain, DmCommand command);

		void setCommandText(FilterChain chain, DmCommand command, string value);

		UpdateRowSource getUpdatedRowSource(FilterChain chain, DmCommand command);

		void setUpdatedRowSource(FilterChain chain, DmCommand command, UpdateRowSource value);

		DmConnection getDbConnection(FilterChain chain, DmCommand command);

		void setDbConnection(FilterChain chain, DmCommand command, DmConnection value);

		DmParameterCollection getDbParameterCollection(FilterChain chain, DmCommand command);

		DmTransaction getDbTransaction(FilterChain chain, DmCommand command);

		void setDbTransaction(FilterChain chain, DmCommand command, DmTransaction value);

		void Cancel(FilterChain chain, DmCommand command);

		int ExecuteNonQuery(FilterChain chain, DmCommand command);

		object ExecuteScalar(FilterChain chain, DmCommand command);

		void Prepare(FilterChain chain, DmCommand command);

		DmParameter CreateDbParameter(FilterChain chain, DmCommand command);

		DmDataReader ExecuteDbDataReader(FilterChain chain, DmCommand command, CommandBehavior behavior);

		System.Data.IsolationLevel getIsolationLevel(FilterChain chain, DmTransaction transaction);

		DmConnection getDbConnection(FilterChain chain, DmTransaction transaction);

		void Commit(FilterChain chain, DmTransaction transaction);

		void Rollback(FilterChain chain, DmTransaction transaction);

		void Dispose(FilterChain chain, DmTransaction transaction, bool disposing);

		object getThis(FilterChain chain, DmDataReader dataReader, int index);

		object getThis(FilterChain chain, DmDataReader dataReader, string name);

		int getDepth(FilterChain chain, DmDataReader dataReader);

		bool getHasRows(FilterChain chain, DmDataReader dataReader);

		int getVisibleFieldCount(FilterChain chain, DmDataReader dataReader);

		int getRecordsAffected(FilterChain chain, DmDataReader dataReader);

		bool getIsClosed(FilterChain chain, DmDataReader dataReader);

		int getFieldCount(FilterChain chain, DmDataReader dataReader);

		void Close(FilterChain chain, DmDataReader dataReader);

		bool GetBoolean(FilterChain chain, DmDataReader dataReader, int ordinal);

		byte GetByte(FilterChain chain, DmDataReader dataReader, int ordinal);

		long GetBytes(FilterChain chain, DmDataReader dataReader, int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length);

		char GetChar(FilterChain chain, DmDataReader dataReader, int ordinal);

		long GetChars(FilterChain chain, DmDataReader dataReader, int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length);

		string GetDataTypeName(FilterChain chain, DmDataReader dataReader, int ordinal);

		DateTime GetDateTime(FilterChain chain, DmDataReader dataReader, int ordinal);

		decimal GetDecimal(FilterChain chain, DmDataReader dataReader, int ordinal);

		double GetDouble(FilterChain chain, DmDataReader dataReader, int ordinal);

		IEnumerator GetEnumerator(FilterChain chain, DmDataReader dataReader);

		Type GetFieldType(FilterChain chain, DmDataReader dataReader, int ordinal);

		float GetFloat(FilterChain chain, DmDataReader dataReader, int ordinal);

		Guid GetGuid(FilterChain chain, DmDataReader dataReader, int ordinal);

		short GetInt16(FilterChain chain, DmDataReader dataReader, int ordinal);

		int GetInt32(FilterChain chain, DmDataReader dataReader, int ordinal);

		long GetInt64(FilterChain chain, DmDataReader dataReader, int ordinal);

		string GetName(FilterChain chain, DmDataReader dataReader, int ordinal);

		int GetOrdinal(FilterChain chain, DmDataReader dataReader, string name);

		Type GetProviderSpecificFieldType(FilterChain chain, DmDataReader dataReader, int ordinal);

		object GetProviderSpecificValue(FilterChain chain, DmDataReader dataReader, int ordinal);

		int GetProviderSpecificValues(FilterChain chain, DmDataReader dataReader, object[] values);

		DataTable GetSchemaTable(FilterChain chain, DmDataReader dataReader);

		string GetString(FilterChain chain, DmDataReader dataReader, int ordinal);

		object GetValue(FilterChain chain, DmDataReader dataReader, int ordinal);

		int GetValues(FilterChain chain, DmDataReader dataReader, object[] values);

		bool IsDBNull(FilterChain chain, DmDataReader dataReader, int ordinal);

		bool NextResult(FilterChain chain, DmDataReader dataReader);

		bool Read(FilterChain chain, DmDataReader dataReader);

		void Dispose(FilterChain chain, DmDataReader dataReader, bool disposing);

		DmDataReader GetDbDataReader(FilterChain chain, DmDataReader dataReader, int ordinal);

		object getSyncRoot(FilterChain chain, DmParameterCollection parameterCollection);

		bool getIsSynchronized(FilterChain chain, DmParameterCollection parameterCollection);

		bool getIsReadOnly(FilterChain chain, DmParameterCollection parameterCollection);

		bool getIsFixedSize(FilterChain chain, DmParameterCollection parameterCollection);

		int getCount(FilterChain chain, DmParameterCollection parameterCollection);

		int Add(FilterChain chain, DmParameterCollection parameterCollection, object value);

		void AddRange(FilterChain chain, DmParameterCollection parameterCollection, Array values);

		void Clear(FilterChain chain, DmParameterCollection parameterCollection);

		bool Contains(FilterChain chain, DmParameterCollection parameterCollection, object value);

		bool Contains(FilterChain chain, DmParameterCollection parameterCollection, string value);

		void CopyTo(FilterChain chain, DmParameterCollection parameterCollection, Array array, int index);

		IEnumerator GetEnumerator(FilterChain chain, DmParameterCollection parameterCollection);

		int IndexOf(FilterChain chain, DmParameterCollection parameterCollection, object value);

		int IndexOf(FilterChain chain, DmParameterCollection parameterCollection, string parameterName);

		void Insert(FilterChain chain, DmParameterCollection parameterCollection, int index, object value);

		void Remove(FilterChain chain, DmParameterCollection parameterCollection, object value);

		void RemoveAt(FilterChain chain, DmParameterCollection parameterCollection, string parameterName);

		void RemoveAt(FilterChain chain, DmParameterCollection parameterCollection, int index);

		DmParameter GetParameter(FilterChain chain, DmParameterCollection parameterCollection, string parameterName);

		DmParameter GetParameter(FilterChain chain, DmParameterCollection parameterCollection, int index);

		void SetParameter(FilterChain chain, DmParameterCollection parameterCollection, int index, DmParameter value);

		void SetParameter(FilterChain chain, DmParameterCollection parameterCollection, string parameterName, DmParameter value);

		DbType getDbType(FilterChain chain, DmParameter parameter);

		void setDbType(FilterChain chain, DmParameter parameter, DbType value);

		ParameterDirection getDirection(FilterChain chain, DmParameter parameter);

		void setDirection(FilterChain chain, DmParameter parameter, ParameterDirection value);

		bool getIsNullable(FilterChain chain, DmParameter parameter);

		void setIsNullable(FilterChain chain, DmParameter parameter, bool value);

		string getParameterName(FilterChain chain, DmParameter parameter);

		void setParameterName(FilterChain chain, DmParameter parameter, string value);

		byte getPrecision(FilterChain chain, DmParameter parameter);

		void setPrecision(FilterChain chain, DmParameter parameter, byte value);

		byte getScale(FilterChain chain, DmParameter parameter);

		void setScale(FilterChain chain, DmParameter parameter, byte value);

		int getSize(FilterChain chain, DmParameter parameter);

		void setSize(FilterChain chain, DmParameter parameter, int value);

		string getSourceColumn(FilterChain chain, DmParameter parameter);

		void setSourceColumn(FilterChain chain, DmParameter parameter, string value);

		bool getSourceColumnNullMapping(FilterChain chain, DmParameter parameter);

		void setSourceColumnNullMapping(FilterChain chain, DmParameter parameter, bool value);

		DataRowVersion getSourceVersion(FilterChain chain, DmParameter parameter);

		void setSourceVersion(FilterChain chain, DmParameter parameter, DataRowVersion value);

		object getValue(FilterChain chain, DmParameter parameter);

		void setValue(FilterChain chain, DmParameter parameter, object value);

		void ResetDbType(FilterChain chain, DmParameter parameter);

		int getUpdateBatchSize(FilterChain chain, DmDataAdapter dataAdapter);

		void setUpdateBatchSize(FilterChain chain, DmDataAdapter dataAdapter, int value);

		int AddToBatch(FilterChain chain, DmDataAdapter dataAdapter, DmCommand command);

		void ClearBatch(FilterChain chain, DmDataAdapter dataAdapter);

		RowUpdatedEventArgs CreateRowUpdatedEvent(FilterChain chain, DmDataAdapter dataAdapter, DataRow dataRow, DmCommand command, StatementType statementType, DataTableMapping tableMapping);

		RowUpdatingEventArgs CreateRowUpdatingEvent(FilterChain chain, DmDataAdapter dataAdapter, DataRow dataRow, DmCommand command, StatementType statementType, DataTableMapping tableMapping);

		int ExecuteBatch(FilterChain chain, DmDataAdapter dataAdapter);

		int Fill(FilterChain chain, DmDataAdapter dataAdapter, DataTable[] dataTables, int startRecord, int maxRecords, DmCommand command, CommandBehavior behavior);

		int Fill(FilterChain chain, DmDataAdapter dataAdapter, DataTable dataTable, DmCommand command, CommandBehavior behavior);

		int Fill(FilterChain chain, DmDataAdapter dataAdapter, DataSet dataSet, int startRecord, int maxRecords, string srcTable, DmCommand command, CommandBehavior behavior);

		DataTable FillSchema(FilterChain chain, DmDataAdapter dataAdapter, DataTable dataTable, SchemaType schemaType, DmCommand command, CommandBehavior behavior);

		DataTable[] FillSchema(FilterChain chain, DmDataAdapter dataAdapter, DataSet dataSet, SchemaType schemaType, DmCommand command, string srcTable, CommandBehavior behavior);

		IDataParameter GetBatchedParameter(FilterChain chain, DmDataAdapter dataAdapter, int commandIdentifier, int parameterIndex);

		bool GetBatchedRecordsAffected(FilterChain chain, DmDataAdapter dataAdapter, int commandIdentifier, out int recordsAffected, out Exception error);

		void InitializeBatching(FilterChain chain, DmDataAdapter dataAdapter);

		void OnRowUpdated(FilterChain chain, DmDataAdapter dataAdapter, RowUpdatedEventArgs value);

		void OnRowUpdating(FilterChain chain, DmDataAdapter dataAdapter, RowUpdatingEventArgs value);

		void TerminateBatching(FilterChain chain, DmDataAdapter dataAdapter);

		int Update(FilterChain chain, DmDataAdapter dataAdapter, DataRow[] dataRows, DataTableMapping tableMapping);

		string getQuoteSuffix(FilterChain chain, DmCommandBuilder commandBuilder);

		void setQuoteSuffix(FilterChain chain, DmCommandBuilder commandBuilder, string value);

		string getQuotePrefix(FilterChain chain, DmCommandBuilder commandBuilder);

		void setQuotePrefix(FilterChain chain, DmCommandBuilder commandBuilder, string value);

		string getCatalogSeparator(FilterChain chain, DmCommandBuilder commandBuilder);

		void setCatalogSeparator(FilterChain chain, DmCommandBuilder commandBuilder, string value);

		CatalogLocation getCatalogLocation(FilterChain chain, DmCommandBuilder commandBuilder);

		void setCatalogLocation(FilterChain chain, DmCommandBuilder commandBuilder, CatalogLocation value);

		ConflictOption getConflictOption(FilterChain chain, DmCommandBuilder commandBuilder);

		void setConflictOption(FilterChain chain, DmCommandBuilder commandBuilder, ConflictOption value);

		string getSchemaSeparator(FilterChain chain, DmCommandBuilder commandBuilder);

		void setSchemaSeparator(FilterChain chain, DmCommandBuilder commandBuilder, string value);

		string QuoteIdentifier(FilterChain chain, DmCommandBuilder commandBuilder, string unquotedIdentifier);

		void RefreshSchema(FilterChain chain, DmCommandBuilder commandBuilder);

		string UnquoteIdentifier(FilterChain chain, DmCommandBuilder commandBuilder, string quotedIdentifier);

		void ApplyParameterInfo(FilterChain chain, DmCommandBuilder commandBuilder, DmParameter parameter, DataRow row, StatementType statementType, bool whereClause);

		string GetParameterName(FilterChain chain, DmCommandBuilder commandBuilder, int parameterOrdinal);

		string GetParameterName(FilterChain chain, DmCommandBuilder commandBuilder, string parameterName);

		string GetParameterPlaceholder(FilterChain chain, DmCommandBuilder commandBuilder, int parameterOrdinal);

		DataTable GetSchemaTable(FilterChain chain, DmCommandBuilder commandBuilder, DmCommand sourceCommand);

		DmCommand InitializeCommand(FilterChain chain, DmCommandBuilder commandBuilder, DmCommand command);

		void SetRowUpdatingHandler(FilterChain chain, DmCommandBuilder commandBuilder, DmDataAdapter adapter);

		object getThis(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword);

		void setThis(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword, object value);

		bool getIsFixedSize(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder);

		int getCount(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder);

		ICollection getKeys(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder);

		ICollection getValues(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder);

		void Clear(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder);

		bool ContainsKey(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword);

		bool EquivalentTo(FilterChain chain, DmConnectionStringBuilder sourceConnectionStringBuilder, DmConnectionStringBuilder destConnectionStringBuilder);

		bool Remove(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword);

		bool ShouldSerialize(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword);

		bool TryGetValue(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword, out object value);

		void GetProperties(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, Hashtable propertyDescriptors);
	}
}
