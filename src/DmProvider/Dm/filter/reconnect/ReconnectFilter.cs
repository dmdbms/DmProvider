using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Transactions;
using Dm.Config;
using Dm.filter.rw;
using Dm.util;

namespace Dm.filter.reconnect
{
	public class ReconnectFilter : IFilter
	{
		private class ReconnectFilterThreadHolder
		{
			internal static readonly ReconnectFilter instance = new ReconnectFilter();
		}

		internal static ReconnectFilter Instance => ReconnectFilterThreadHolder.instance;

		private ReconnectFilter()
		{
		}

		private void autoReconnect(DmConnection connection, DbException e)
		{
			if (e is DmException && ((DmException)e).Number == DmErrorDefinition.ECNET_COMMUNITION_ERROR)
			{
				reconnect(connection, e.Message);
				return;
			}
			throw e;
		}

		public static void reconnect(DmConnection connection, string reasons)
		{
			try
			{
				if (connection.ConnProperty.RwSeparate)
				{
					RWUtil2.reconnect(connection);
				}
				else
				{
					connection.Reconnect();
				}
			}
			catch (Exception)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_CONNECTION_SWITCH_FAILED);
			}
			DmError.ThrowDmException(DmErrorDefinition.ECNET_CONNECTION_SWITCHED);
		}

		public void checkAndRecover(DmConnection connection, DmTransaction transaction)
		{
			if (connection.ConnProperty.DoSwitch != DoSwitch.EP_RECOVER)
			{
				return;
			}
			int num = 0;
			if ((transaction != null && transaction.Valid) || (num = connection.getIndexOnDBGroup()) == 0 || (DateTime.Now - connection.RecoverInfo.checkEpRecoverTs).TotalMilliseconds < (double)connection.ConnProperty.SwitchInterval)
			{
				return;
			}
			List<EP> list = ((connection.ConnProperty.Cluster.Value == CLUSTER.DSC) ? DriverUtil.loadDscEpSites(connection) : null);
			if (list == null || list.Count == 0)
			{
				return;
			}
			bool flag = false;
			foreach (EP item in list)
			{
				if (item.epStatus != 1)
				{
					continue;
				}
				for (int i = 0; i < num; i++)
				{
					EP eP = connection.ConnProperty.EPGroup.epList[i];
					if (item.host.Equals(eP.host, StringComparison.OrdinalIgnoreCase) && item.port == eP.port)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			connection.RecoverInfo.checkEpRecoverTs = DateTime.Now;
			if (flag)
			{
				connection.Reconnect();
			}
		}

		public void Open(FilterChain chain, DmConnection conn)
		{
			if (conn.do_State != ConnectionState.Open)
			{
				try
				{
					chain.Open(conn);
				}
				catch (DbException e)
				{
					autoReconnect(conn, e);
				}
			}
		}

		public void Close(FilterChain chain, DmConnection conn)
		{
			if (conn.do_State != 0)
			{
				try
				{
					chain.Close(conn);
				}
				catch (DbException e)
				{
					autoReconnect(conn, e);
				}
			}
		}

		public string getServerVersion(FilterChain chain, DmConnection conn)
		{
			try
			{
				return chain.getServerVersion(conn);
			}
			catch (DbException e)
			{
				autoReconnect(conn, e);
				return string.Empty;
			}
		}

		public string getDataSource(FilterChain chain, DmConnection conn)
		{
			try
			{
				return chain.getDataSource(conn);
			}
			catch (DbException e)
			{
				autoReconnect(conn, e);
				return string.Empty;
			}
		}

		public string getDatabase(FilterChain chain, DmConnection conn)
		{
			try
			{
				return chain.getDatabase(conn);
			}
			catch (DbException e)
			{
				autoReconnect(conn, e);
				return string.Empty;
			}
		}

		public int getConnectionTimeout(FilterChain chain, DmConnection conn)
		{
			try
			{
				return chain.getConnectionTimeout(conn);
			}
			catch (DbException e)
			{
				autoReconnect(conn, e);
				return 0;
			}
		}

		public string getConnectionString(FilterChain chain, DmConnection conn)
		{
			try
			{
				return chain.getConnectionString(conn);
			}
			catch (DbException e)
			{
				autoReconnect(conn, e);
				return string.Empty;
			}
		}

		public DbProviderFactory getDbProviderFactory(FilterChain chain, DmConnection conn)
		{
			try
			{
				return chain.getDbProviderFactory(conn);
			}
			catch (DbException e)
			{
				autoReconnect(conn, e);
				return null;
			}
		}

		public ConnectionState getState(FilterChain chain, DmConnection conn)
		{
			try
			{
				return chain.getState(conn);
			}
			catch (DbException e)
			{
				autoReconnect(conn, e);
				return ConnectionState.Open;
			}
		}

		public DmTransaction BeginDbTransaction(FilterChain chain, DmConnection conn, System.Data.IsolationLevel isolationLevel)
		{
			if (conn.do_State == ConnectionState.Closed)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_CONNCTION_NOT_OPENED);
			}
			try
			{
				return chain.BeginDbTransaction(conn, isolationLevel);
			}
			catch (DbException e)
			{
				autoReconnect(conn, e);
				return null;
			}
		}

		public void ChangeDatabase(FilterChain chain, DmConnection conn, string databaseName)
		{
			try
			{
				chain.ChangeDatabase(conn, databaseName);
			}
			catch (DbException e)
			{
				autoReconnect(conn, e);
			}
		}

		public DmCommand CreateDbCommand(FilterChain chain, DmConnection conn)
		{
			try
			{
				return chain.CreateDbCommand(conn);
			}
			catch (DbException e)
			{
				autoReconnect(conn, e);
				return null;
			}
		}

		public void EnlistTransaction(FilterChain chain, DmConnection conn, Transaction transaction)
		{
			try
			{
				chain.EnlistTransaction(conn, transaction);
			}
			catch (DbException e)
			{
				autoReconnect(conn, e);
			}
		}

		public DataTable GetSchema(FilterChain chain, DmConnection conn)
		{
			try
			{
				return chain.GetSchema(conn);
			}
			catch (DbException e)
			{
				autoReconnect(conn, e);
				return null;
			}
		}

		public DataTable GetSchema(FilterChain chain, DmConnection conn, string collectionName, string[] restrictionValues)
		{
			try
			{
				return chain.GetSchema(conn, collectionName, restrictionValues);
			}
			catch (DbException e)
			{
				autoReconnect(conn, e);
				return null;
			}
		}

		public DataTable GetSchema(FilterChain chain, DmConnection conn, string collectionName)
		{
			try
			{
				return chain.GetSchema(conn, collectionName);
			}
			catch (DbException e)
			{
				autoReconnect(conn, e);
				return null;
			}
		}

		public bool getDesignTimeVisible(FilterChain chain, DmCommand command)
		{
			try
			{
				return chain.getDesignTimeVisible(command);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
				return false;
			}
		}

		public void setDesignTimeVisible(FilterChain chain, DmCommand command, bool value)
		{
			try
			{
				chain.setDesignTimeVisible(command, value);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
			}
		}

		public CommandType getCommandType(FilterChain chain, DmCommand command)
		{
			try
			{
				return chain.getCommandType(command);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
				return CommandType.Text;
			}
		}

		public void setCommandType(FilterChain chain, DmCommand command, CommandType value)
		{
			try
			{
				chain.setCommandType(command, value);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
			}
		}

		public int getCommandTimeout(FilterChain chain, DmCommand command)
		{
			try
			{
				return chain.getCommandTimeout(command);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
				return 0;
			}
		}

		public void setCommandTimeout(FilterChain chain, DmCommand command, int value)
		{
			try
			{
				chain.setCommandTimeout(command, value);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
			}
		}

		public string getCommandText(FilterChain chain, DmCommand command)
		{
			try
			{
				return chain.getCommandText(command);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
				return null;
			}
		}

		public void setCommandText(FilterChain chain, DmCommand command, string value)
		{
			try
			{
				chain.setCommandText(command, value);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
			}
		}

		public UpdateRowSource getUpdatedRowSource(FilterChain chain, DmCommand command)
		{
			try
			{
				return chain.getUpdatedRowSource(command);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
				return UpdateRowSource.None;
			}
		}

		public void setUpdatedRowSource(FilterChain chain, DmCommand command, UpdateRowSource value)
		{
			try
			{
				chain.setUpdatedRowSource(command, value);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
			}
		}

		public DmConnection getDbConnection(FilterChain chain, DmCommand command)
		{
			try
			{
				return chain.getDbConnection(command);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
				return null;
			}
		}

		public void setDbConnection(FilterChain chain, DmCommand command, DmConnection value)
		{
			try
			{
				chain.setDbConnection(command, value);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
			}
		}

		public DmParameterCollection getDbParameterCollection(FilterChain chain, DmCommand command)
		{
			try
			{
				return chain.getDbParameterCollection(command);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
				return null;
			}
		}

		public DmTransaction getDbTransaction(FilterChain chain, DmCommand command)
		{
			try
			{
				return chain.getDbTransaction(command);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
				return null;
			}
		}

		public void setDbTransaction(FilterChain chain, DmCommand command, DmTransaction value)
		{
			try
			{
				chain.setDbTransaction(command, value);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
			}
		}

		public void Cancel(FilterChain chain, DmCommand command)
		{
			try
			{
				chain.Cancel(command);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
			}
		}

		public int ExecuteNonQuery(FilterChain chain, DmCommand command)
		{
			if (command.do_DbConnection == null || command.do_DbConnection.do_State == ConnectionState.Closed)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_CONNCTION_NOT_OPENED);
			}
			try
			{
				checkAndRecover(command.do_DbConnection, command.do_DbTransaction);
				return chain.ExecuteNonQuery(command);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
				return 0;
			}
		}

		public object ExecuteScalar(FilterChain chain, DmCommand command)
		{
			if (command.do_DbConnection == null || command.do_DbConnection.do_State == ConnectionState.Closed)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_CONNCTION_NOT_OPENED);
			}
			try
			{
				checkAndRecover(command.do_DbConnection, command.do_DbTransaction);
				return chain.ExecuteScalar(command);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
				return null;
			}
		}

		public void Prepare(FilterChain chain, DmCommand command)
		{
			if (command.do_DbConnection == null || command.do_DbConnection.do_State == ConnectionState.Closed)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_CONNCTION_NOT_OPENED);
			}
			try
			{
				checkAndRecover(command.do_DbConnection, command.do_DbTransaction);
				chain.Prepare(command);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
			}
		}

		public DmParameter CreateDbParameter(FilterChain chain, DmCommand command)
		{
			return chain.CreateDbParameter(command);
		}

		public DmDataReader ExecuteDbDataReader(FilterChain chain, DmCommand command, CommandBehavior behavior)
		{
			if (command.do_DbConnection == null || command.do_DbConnection.do_State == ConnectionState.Closed)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_CONNCTION_NOT_OPENED);
			}
			try
			{
				checkAndRecover(command.do_DbConnection, command.do_DbTransaction);
				return chain.ExecuteDbDataReader(command, behavior);
			}
			catch (DbException e)
			{
				autoReconnect(command.do_DbConnection, e);
				return null;
			}
		}

		public System.Data.IsolationLevel getIsolationLevel(FilterChain chain, DmTransaction transaction)
		{
			try
			{
				return chain.getIsolationLevel(transaction);
			}
			catch (DbException e)
			{
				autoReconnect(transaction.do_DbConnection, e);
				return System.Data.IsolationLevel.Unspecified;
			}
		}

		public DmConnection getDbConnection(FilterChain chain, DmTransaction transaction)
		{
			return chain.getDbConnection(transaction);
		}

		public void Commit(FilterChain chain, DmTransaction transaction)
		{
			try
			{
				chain.Commit(transaction);
				checkAndRecover(transaction.do_DbConnection, transaction);
			}
			catch (DbException e)
			{
				autoReconnect(transaction.do_DbConnection, e);
			}
		}

		public void Rollback(FilterChain chain, DmTransaction transaction)
		{
			try
			{
				chain.Rollback(transaction);
				checkAndRecover(transaction.do_DbConnection, transaction);
			}
			catch (DbException e)
			{
				autoReconnect(transaction.do_DbConnection, e);
			}
		}

		public void Dispose(FilterChain chain, DmTransaction transaction, bool disposing)
		{
			try
			{
				chain.Dispose(transaction, disposing);
				checkAndRecover(transaction.do_DbConnection, transaction);
			}
			catch (DbException e)
			{
				autoReconnect(transaction.do_DbConnection, e);
			}
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
