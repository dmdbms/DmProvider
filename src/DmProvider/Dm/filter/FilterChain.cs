using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Transactions;
using Dm.Config;
using Dm.filter.log;
using Dm.filter.reconnect;
using Dm.filter.rw;
using Dm.util;

namespace Dm.filter
{
	public class FilterChain
	{
		internal IFilter[] filters;

		internal int fpos;

		internal FilterChain(params IFilter[] filters)
		{
			this.filters = filters;
			fpos = 0;
		}

		internal FilterChain reset()
		{
			fpos = 0;
			return this;
		}

		internal static void createFilterChain(IFilterInfo filterInfo, DmConnProperty connProperty = null)
		{
			FilterChain filterChain = null;
			IList<IFilter> list = null;
			if (DmSvcConfig.logLevel != 0)
			{
				list = list ?? new List<IFilter>();
				list.Add(LogFilter.Instance);
				filterInfo.LogInfo = new LogInfo();
			}
			if (connProperty != null)
			{
				if (connProperty.ConnPooling)
				{
					list = list ?? new List<IFilter>();
					ConnPoolFilter.Instance.ConnPoolSize = connProperty.ConnPoolSize;
					list.Add(ConnPoolFilter.Instance);
				}
				if (connProperty.DoSwitch.Value != 0)
				{
					list = list ?? new List<IFilter>();
					list.Add(ReconnectFilter.Instance);
					filterInfo.RecoverInfo = new RecoverInfo();
				}
				if (connProperty.RwSeparate)
				{
					list = list ?? new List<IFilter>();
					list.Add(RWFilter2.Instance);
					filterInfo.RWInfo = new RWInfo();
				}
			}
			if (list != null && list.Count > 0)
			{
				filterChain = new FilterChain(ListUtil.toArray(list));
			}
			filterInfo.FilterChain = filterChain;
		}

		internal string getServerVersion(DmConnection conn)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getServerVersion(this, conn);
			}
			return conn.do_ServerVersion;
		}

		internal string getDataSource(DmConnection conn)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getDataSource(this, conn);
			}
			return conn.do_DataSource;
		}

		internal string getDatabase(DmConnection conn)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getDatabase(this, conn);
			}
			return conn.do_Database;
		}

		internal int getConnectionTimeout(DmConnection conn)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getConnectionTimeout(this, conn);
			}
			return conn.do_ConnectionTimeout;
		}

		internal string getConnectionString(DmConnection conn)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getConnectionString(this, conn);
			}
			return conn.do_ConnectionString;
		}

		internal DbProviderFactory getDbProviderFactory(DmConnection conn)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getDbProviderFactory(this, conn);
			}
			return conn.do_DbProviderFactory;
		}

		internal ConnectionState getState(DmConnection conn)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getState(this, conn);
			}
			return conn.do_State;
		}

		internal void ChangeDatabase(DmConnection conn, string databaseName)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].ChangeDatabase(this, conn, databaseName);
			}
			else
			{
				conn.do_ChangeDatabase(databaseName);
			}
		}

		internal DmTransaction BeginDbTransaction(DmConnection conn, System.Data.IsolationLevel isolationLevel)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].BeginDbTransaction(this, conn, isolationLevel);
			}
			return conn.do_BeginDbTransaction(isolationLevel);
		}

		internal DmCommand CreateDbCommand(DmConnection conn)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].CreateDbCommand(this, conn);
			}
			return conn.do_CreateDbCommand();
		}

		internal void Close(DmConnection conn)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].Close(this, conn);
			}
			else
			{
				conn.do_Close();
			}
		}

		internal void EnlistTransaction(DmConnection conn, Transaction transaction)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].EnlistTransaction(this, conn, transaction);
			}
			else
			{
				conn.do_EnlistTransaction(transaction);
			}
		}

		internal DataTable GetSchema(DmConnection conn)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetSchema(this, conn);
			}
			return conn.do_GetSchema();
		}

		internal DataTable GetSchema(DmConnection conn, string collectionName, string[] restrictionValues)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetSchema(this, conn, collectionName, restrictionValues);
			}
			return conn.do_GetSchema(collectionName, restrictionValues);
		}

		internal DataTable GetSchema(DmConnection conn, string collectionName)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetSchema(this, conn, collectionName);
			}
			return conn.do_GetSchema(collectionName);
		}

		internal void Open(DmConnection conn)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].Open(this, conn);
			}
			else
			{
				conn.Connect();
			}
		}

		internal bool getDesignTimeVisible(DmCommand command)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getDesignTimeVisible(this, command);
			}
			return command.do_DesignTimeVisible;
		}

		internal void setDesignTimeVisible(DmCommand command, bool value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setDesignTimeVisible(this, command, value);
			}
			else
			{
				command.do_DesignTimeVisible = value;
			}
		}

		internal CommandType getCommandType(DmCommand command)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getCommandType(this, command);
			}
			return command.do_CommandType;
		}

		internal void setCommandType(DmCommand command, CommandType value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setCommandType(this, command, value);
			}
			else
			{
				command.do_CommandType = value;
			}
		}

		internal int getCommandTimeout(DmCommand command)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getCommandTimeout(this, command);
			}
			return command.do_CommandTimeout;
		}

		internal void setCommandTimeout(DmCommand command, int value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setCommandTimeout(this, command, value);
			}
			else
			{
				command.do_CommandTimeout = value;
			}
		}

		internal string getCommandText(DmCommand command)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getCommandText(this, command);
			}
			return command.do_CommandText;
		}

		internal void setCommandText(DmCommand command, string value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setCommandText(this, command, value);
			}
			else
			{
				command.do_CommandText = value;
			}
		}

		internal UpdateRowSource getUpdatedRowSource(DmCommand command)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getUpdatedRowSource(this, command);
			}
			return command.do_UpdatedRowSource;
		}

		internal void setUpdatedRowSource(DmCommand command, UpdateRowSource value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setUpdatedRowSource(this, command, value);
			}
			else
			{
				command.do_UpdatedRowSource = value;
			}
		}

		internal DmConnection getDbConnection(DmCommand command)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getDbConnection(this, command);
			}
			return command.do_DbConnection;
		}

		internal void setDbConnection(DmCommand command, DmConnection value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setDbConnection(this, command, value);
			}
			else
			{
				command.do_DbConnection = value;
			}
		}

		internal DmParameterCollection getDbParameterCollection(DmCommand command)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getDbParameterCollection(this, command);
			}
			return command.do_DbParameterCollection;
		}

		internal DmTransaction getDbTransaction(DmCommand command)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getDbTransaction(this, command);
			}
			return command.do_DbTransaction;
		}

		internal void setDbTransaction(DmCommand command, DmTransaction value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setDbTransaction(this, command, value);
			}
			else
			{
				command.do_DbTransaction = value;
			}
		}

		internal void Cancel(DmCommand command)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].Cancel(this, command);
			}
			else
			{
				command.do_Cancel();
			}
		}

		internal int ExecuteNonQuery(DmCommand command)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].ExecuteNonQuery(this, command);
			}
			return command.do_ExecuteNonQuery();
		}

		internal object ExecuteScalar(DmCommand command)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].ExecuteScalar(this, command);
			}
			return command.do_ExecuteScalar();
		}

		internal void Prepare(DmCommand command)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].Prepare(this, command);
			}
			else
			{
				command.do_Prepare();
			}
		}

		internal DmParameter CreateDbParameter(DmCommand command)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].CreateDbParameter(this, command);
			}
			return command.do_CreateDbParameter();
		}

		internal DmDataReader ExecuteDbDataReader(DmCommand command, CommandBehavior behavior)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].ExecuteDbDataReader(this, command, behavior);
			}
			return command.do_ExecuteDbDataReader(behavior);
		}

		internal System.Data.IsolationLevel getIsolationLevel(DmTransaction transaction)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getIsolationLevel(this, transaction);
			}
			return transaction.do_IsolationLevel;
		}

		internal DmConnection getDbConnection(DmTransaction transaction)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getDbConnection(this, transaction);
			}
			return transaction.do_DbConnection;
		}

		internal void Commit(DmTransaction transaction)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].Commit(this, transaction);
			}
			else
			{
				transaction.do_Commit();
			}
		}

		internal void Rollback(DmTransaction transaction)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].Rollback(this, transaction);
			}
			else
			{
				transaction.do_Rollback();
			}
		}

		internal void Dispose(DmTransaction transaction, bool disposing)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].Dispose(this, transaction, disposing);
			}
			else
			{
				transaction.do_Dispose(disposing);
			}
		}

		internal object getThis(DmDataReader dataReader, int index)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getThis(this, dataReader, index);
			}
			return dataReader.do_this(index);
		}

		internal object getThis(DmDataReader dataReader, string name)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getThis(this, dataReader, name);
			}
			return dataReader.do_this(name);
		}

		internal int getDepth(DmDataReader dataReader)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getDepth(this, dataReader);
			}
			return dataReader.do_Depth;
		}

		internal bool getHasRows(DmDataReader dataReader)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getHasRows(this, dataReader);
			}
			return dataReader.do_HasRows;
		}

		internal int getVisibleFieldCount(DmDataReader dataReader)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getVisibleFieldCount(this, dataReader);
			}
			return dataReader.do_VisibleFieldCount;
		}

		internal int getRecordsAffected(DmDataReader dataReader)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getRecordsAffected(this, dataReader);
			}
			return dataReader.do_RecordsAffected;
		}

		internal bool getIsClosed(DmDataReader dataReader)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getIsClosed(this, dataReader);
			}
			return dataReader.do_IsClosed;
		}

		internal int getFieldCount(DmDataReader dataReader)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getFieldCount(this, dataReader);
			}
			return dataReader.do_FieldCount;
		}

		internal void Close(DmDataReader dataReader)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].Close(this, dataReader);
			}
			else
			{
				dataReader.do_Close();
			}
		}

		internal bool GetBoolean(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetBoolean(this, dataReader, ordinal);
			}
			return dataReader.do_GetBoolean(ordinal);
		}

		internal byte GetByte(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetByte(this, dataReader, ordinal);
			}
			return dataReader.do_GetByte(ordinal);
		}

		internal long GetBytes(DmDataReader dataReader, int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetBytes(this, dataReader, ordinal, dataOffset, buffer, bufferOffset, length);
			}
			return dataReader.do_GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
		}

		internal char GetChar(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetChar(this, dataReader, ordinal);
			}
			return dataReader.do_GetChar(ordinal);
		}

		internal long GetChars(DmDataReader dataReader, int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetChars(this, dataReader, ordinal, dataOffset, buffer, bufferOffset, length);
			}
			return dataReader.do_GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
		}

		internal string GetDataTypeName(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetDataTypeName(this, dataReader, ordinal);
			}
			return dataReader.do_GetDataTypeName(ordinal);
		}

		internal DateTime GetDateTime(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetDateTime(this, dataReader, ordinal);
			}
			return dataReader.do_GetDateTime(ordinal);
		}

		internal decimal GetDecimal(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetDecimal(this, dataReader, ordinal);
			}
			return dataReader.do_GetDecimal(ordinal);
		}

		internal double GetDouble(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetDouble(this, dataReader, ordinal);
			}
			return dataReader.do_GetDouble(ordinal);
		}

		internal IEnumerator GetEnumerator(DmDataReader dataReader)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetEnumerator(this, dataReader);
			}
			return dataReader.do_GetEnumerator();
		}

		internal Type GetFieldType(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetFieldType(this, dataReader, ordinal);
			}
			return dataReader.do_GetFieldType(ordinal);
		}

		internal float GetFloat(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetFloat(this, dataReader, ordinal);
			}
			return dataReader.do_GetFloat(ordinal);
		}

		internal Guid GetGuid(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetGuid(this, dataReader, ordinal);
			}
			return dataReader.do_GetGuid(ordinal);
		}

		internal short GetInt16(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetInt16(this, dataReader, ordinal);
			}
			return dataReader.do_GetInt16(ordinal);
		}

		internal int GetInt32(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetInt32(this, dataReader, ordinal);
			}
			return dataReader.do_GetInt32(ordinal);
		}

		internal long GetInt64(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetInt64(this, dataReader, ordinal);
			}
			return dataReader.do_GetInt64(ordinal);
		}

		internal string GetName(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetName(this, dataReader, ordinal);
			}
			return dataReader.do_GetName(ordinal);
		}

		internal int GetOrdinal(DmDataReader dataReader, string name)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetOrdinal(this, dataReader, name);
			}
			return dataReader.do_GetOrdinal(name);
		}

		internal Type GetProviderSpecificFieldType(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetProviderSpecificFieldType(this, dataReader, ordinal);
			}
			return dataReader.do_GetProviderSpecificFieldType(ordinal);
		}

		internal object GetProviderSpecificValue(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetProviderSpecificValue(this, dataReader, ordinal);
			}
			return dataReader.do_GetProviderSpecificValue(ordinal);
		}

		internal int GetProviderSpecificValues(DmDataReader dataReader, object[] values)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetProviderSpecificValues(this, dataReader, values);
			}
			return dataReader.do_GetProviderSpecificValues(values);
		}

		internal DataTable GetSchemaTable(DmDataReader dataReader)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetSchemaTable(this, dataReader);
			}
			return dataReader.do_GetSchemaTable();
		}

		internal string GetString(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetString(this, dataReader, ordinal);
			}
			return dataReader.do_GetString(ordinal);
		}

		internal object GetValue(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetValue(this, dataReader, ordinal);
			}
			return dataReader.do_GetValue(ordinal);
		}

		internal int GetValues(DmDataReader dataReader, object[] values)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetValues(this, dataReader, values);
			}
			return dataReader.do_GetValues(values);
		}

		internal bool IsDBNull(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].IsDBNull(this, dataReader, ordinal);
			}
			return dataReader.do_IsDBNull(ordinal);
		}

		internal bool NextResult(DmDataReader dataReader)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].NextResult(this, dataReader);
			}
			return dataReader.do_NextResult();
		}

		internal bool Read(DmDataReader dataReader)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].Read(this, dataReader);
			}
			return dataReader.do_Read();
		}

		internal void Dispose(DmDataReader dataReader, bool disposing)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].Dispose(this, dataReader, disposing);
			}
			else
			{
				dataReader.do_Dispose(disposing);
			}
		}

		internal DmDataReader GetDbDataReader(DmDataReader dataReader, int ordinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetDbDataReader(this, dataReader, ordinal);
			}
			return dataReader.do_GetDbDataReader(ordinal);
		}

		internal object getSyncRoot(DmParameterCollection parameterCollection)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getSyncRoot(this, parameterCollection);
			}
			return parameterCollection.do_SyncRoot;
		}

		internal bool getIsSynchronized(DmParameterCollection parameterCollection)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getIsSynchronized(this, parameterCollection);
			}
			return parameterCollection.do_IsSynchronized;
		}

		internal bool getIsReadOnly(DmParameterCollection parameterCollection)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getIsReadOnly(this, parameterCollection);
			}
			return parameterCollection.do_IsReadOnly;
		}

		internal bool getIsFixedSize(DmParameterCollection parameterCollection)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getIsFixedSize(this, parameterCollection);
			}
			return parameterCollection.do_IsFixedSize;
		}

		internal int getCount(DmParameterCollection parameterCollection)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getCount(this, parameterCollection);
			}
			return parameterCollection.do_Count;
		}

		internal int Add(DmParameterCollection parameterCollection, object value)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].Add(this, parameterCollection, value);
			}
			return parameterCollection.do_Add(value);
		}

		internal void AddRange(DmParameterCollection parameterCollection, Array values)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].AddRange(this, parameterCollection, values);
			}
			else
			{
				parameterCollection.do_AddRange(values);
			}
		}

		internal void Clear(DmParameterCollection parameterCollection)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].Clear(this, parameterCollection);
			}
			else
			{
				parameterCollection.do_Clear();
			}
		}

		internal bool Contains(DmParameterCollection parameterCollection, object value)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].Contains(this, parameterCollection, value);
			}
			return parameterCollection.do_Contains(value);
		}

		internal bool Contains(DmParameterCollection parameterCollection, string value)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].Contains(this, parameterCollection, value);
			}
			return parameterCollection.do_Contains(value);
		}

		internal void CopyTo(DmParameterCollection parameterCollection, Array array, int index)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].CopyTo(this, parameterCollection, array, index);
			}
			else
			{
				parameterCollection.do_CopyTo(array, index);
			}
		}

		internal IEnumerator GetEnumerator(DmParameterCollection parameterCollection)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetEnumerator(this, parameterCollection);
			}
			return parameterCollection.do_GetEnumerator();
		}

		internal int IndexOf(DmParameterCollection parameterCollection, object value)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].IndexOf(this, parameterCollection, value);
			}
			return parameterCollection.do_IndexOf(value);
		}

		internal int IndexOf(DmParameterCollection parameterCollection, string parameterName)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].IndexOf(this, parameterCollection, parameterName);
			}
			return parameterCollection.do_IndexOf(parameterName);
		}

		internal void Insert(DmParameterCollection parameterCollection, int index, object value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].Insert(this, parameterCollection, index, value);
			}
			else
			{
				parameterCollection.do_Insert(index, value);
			}
		}

		internal void Remove(DmParameterCollection parameterCollection, object value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].Remove(this, parameterCollection, value);
			}
			else
			{
				parameterCollection.do_Remove(value);
			}
		}

		internal void RemoveAt(DmParameterCollection parameterCollection, string parameterName)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].RemoveAt(this, parameterCollection, parameterName);
			}
			else
			{
				parameterCollection.do_RemoveAt(parameterName);
			}
		}

		internal void RemoveAt(DmParameterCollection parameterCollection, int index)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].RemoveAt(this, parameterCollection, index);
			}
			else
			{
				parameterCollection.do_RemoveAt(index);
			}
		}

		internal DmParameter GetParameter(DmParameterCollection parameterCollection, string parameterName)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetParameter(this, parameterCollection, parameterName);
			}
			return parameterCollection.do_GetParameter(parameterName);
		}

		internal DmParameter GetParameter(DmParameterCollection parameterCollection, int index)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetParameter(this, parameterCollection, index);
			}
			return parameterCollection.do_GetParameter(index);
		}

		internal void SetParameter(DmParameterCollection parameterCollection, int index, DmParameter value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].SetParameter(this, parameterCollection, index, value);
			}
			else
			{
				parameterCollection.do_SetParameter(index, value);
			}
		}

		internal void SetParameter(DmParameterCollection parameterCollection, string parameterName, DmParameter value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].SetParameter(this, parameterCollection, parameterName, value);
			}
			else
			{
				parameterCollection.do_SetParameter(parameterName, value);
			}
		}

		internal DbType getDbType(DmParameter parameter)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getDbType(this, parameter);
			}
			return parameter.do_DbType;
		}

		internal void setDbType(DmParameter parameter, DbType value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setDbType(this, parameter, value);
			}
			else
			{
				parameter.do_DbType = value;
			}
		}

		internal ParameterDirection getDirection(DmParameter parameter)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getDirection(this, parameter);
			}
			return parameter.do_Direction;
		}

		internal void setDirection(DmParameter parameter, ParameterDirection value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setDirection(this, parameter, value);
			}
			else
			{
				parameter.do_Direction = value;
			}
		}

		internal bool getIsNullable(DmParameter parameter)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getIsNullable(this, parameter);
			}
			return parameter.do_IsNullable;
		}

		internal void setIsNullable(DmParameter parameter, bool value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setIsNullable(this, parameter, value);
			}
			else
			{
				parameter.do_IsNullable = value;
			}
		}

		internal string getParameterName(DmParameter parameter)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getParameterName(this, parameter);
			}
			return parameter.do_ParameterName;
		}

		internal void setParameterName(DmParameter parameter, string value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setParameterName(this, parameter, value);
			}
			else
			{
				parameter.do_ParameterName = value;
			}
		}

		internal byte getPrecision(DmParameter parameter)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getPrecision(this, parameter);
			}
			return parameter.do_Precision;
		}

		internal void setPrecision(DmParameter parameter, byte value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setPrecision(this, parameter, value);
			}
			else
			{
				parameter.do_Precision = value;
			}
		}

		internal byte getScale(DmParameter parameter)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getScale(this, parameter);
			}
			return parameter.do_Scale;
		}

		internal void setScale(DmParameter parameter, byte value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setScale(this, parameter, value);
			}
			else
			{
				parameter.do_Scale = value;
			}
		}

		internal int getSize(DmParameter parameter)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getSize(this, parameter);
			}
			return parameter.do_Size;
		}

		internal void setSize(DmParameter parameter, int value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setSize(this, parameter, value);
			}
			else
			{
				parameter.do_Size = value;
			}
		}

		internal string getSourceColumn(DmParameter parameter)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getSourceColumn(this, parameter);
			}
			return parameter.do_SourceColumn;
		}

		internal void setSourceColumn(DmParameter parameter, string value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setSourceColumn(this, parameter, value);
			}
			else
			{
				parameter.do_SourceColumn = value;
			}
		}

		internal bool getSourceColumnNullMapping(DmParameter parameter)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getSourceColumnNullMapping(this, parameter);
			}
			return parameter.do_SourceColumnNullMapping;
		}

		internal void setSourceColumnNullMapping(DmParameter parameter, bool value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setSourceColumnNullMapping(this, parameter, value);
			}
			else
			{
				parameter.do_SourceColumnNullMapping = value;
			}
		}

		internal DataRowVersion getSourceVersion(DmParameter parameter)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getSourceVersion(this, parameter);
			}
			return parameter.do_SourceVersion;
		}

		internal void setSourceVersion(DmParameter parameter, DataRowVersion value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setSourceVersion(this, parameter, value);
			}
			else
			{
				parameter.do_SourceVersion = value;
			}
		}

		internal object getValue(DmParameter parameter)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getValue(this, parameter);
			}
			return parameter.do_Value;
		}

		internal void setValue(DmParameter parameter, object value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setValue(this, parameter, value);
			}
			else
			{
				parameter.do_Value = value;
			}
		}

		internal void ResetDbType(DmParameter parameter)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].ResetDbType(this, parameter);
			}
			else
			{
				parameter.do_ResetDbType();
			}
		}

		internal int getUpdateBatchSize(DmDataAdapter dataAdapter)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getUpdateBatchSize(this, dataAdapter);
			}
			return dataAdapter.do_UpdateBatchSize;
		}

		internal void setUpdateBatchSize(DmDataAdapter dataAdapter, int value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setUpdateBatchSize(this, dataAdapter, value);
			}
			else
			{
				dataAdapter.do_UpdateBatchSize = value;
			}
		}

		internal int AddToBatch(DmDataAdapter dataAdapter, DmCommand command)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].AddToBatch(this, dataAdapter, command);
			}
			return dataAdapter.do_AddToBatch(command);
		}

		internal void ClearBatch(DmDataAdapter dataAdapter)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].ClearBatch(this, dataAdapter);
			}
			else
			{
				dataAdapter.do_ClearBatch();
			}
		}

		internal RowUpdatedEventArgs CreateRowUpdatedEvent(DmDataAdapter dataAdapter, DataRow dataRow, DmCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].CreateRowUpdatedEvent(this, dataAdapter, dataRow, command, statementType, tableMapping);
			}
			return dataAdapter.do_CreateRowUpdatedEvent(dataRow, command, statementType, tableMapping);
		}

		internal RowUpdatingEventArgs CreateRowUpdatingEvent(DmDataAdapter dataAdapter, DataRow dataRow, DmCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].CreateRowUpdatingEvent(this, dataAdapter, dataRow, command, statementType, tableMapping);
			}
			return dataAdapter.do_CreateRowUpdatingEvent(dataRow, command, statementType, tableMapping);
		}

		internal int ExecuteBatch(DmDataAdapter dataAdapter)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].ExecuteBatch(this, dataAdapter);
			}
			return dataAdapter.do_ExecuteBatch();
		}

		internal int Fill(DmDataAdapter dataAdapter, DataTable[] dataTables, int startRecord, int maxRecords, DmCommand command, CommandBehavior behavior)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].Fill(this, dataAdapter, dataTables, startRecord, maxRecords, command, behavior);
			}
			return dataAdapter.do_Fill(dataTables, startRecord, maxRecords, command, behavior);
		}

		internal int Fill(DmDataAdapter dataAdapter, DataTable dataTable, DmCommand command, CommandBehavior behavior)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].Fill(this, dataAdapter, dataTable, command, behavior);
			}
			return dataAdapter.do_Fill(dataTable, command, behavior);
		}

		internal int Fill(DmDataAdapter dataAdapter, DataSet dataSet, int startRecord, int maxRecords, string srcTable, DmCommand command, CommandBehavior behavior)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].Fill(this, dataAdapter, dataSet, startRecord, maxRecords, srcTable, command, behavior);
			}
			return dataAdapter.do_Fill(dataSet, startRecord, maxRecords, srcTable, command, behavior);
		}

		internal DataTable FillSchema(DmDataAdapter dataAdapter, DataTable dataTable, SchemaType schemaType, DmCommand command, CommandBehavior behavior)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].FillSchema(this, dataAdapter, dataTable, schemaType, command, behavior);
			}
			return dataAdapter.do_FillSchema(dataTable, schemaType, command, behavior);
		}

		internal DataTable[] FillSchema(DmDataAdapter dataAdapter, DataSet dataSet, SchemaType schemaType, DmCommand command, string srcTable, CommandBehavior behavior)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].FillSchema(this, dataAdapter, dataSet, schemaType, command, srcTable, behavior);
			}
			return dataAdapter.do_FillSchema(dataSet, schemaType, command, srcTable, behavior);
		}

		internal IDataParameter GetBatchedParameter(DmDataAdapter dataAdapter, int commandIdentifier, int parameterIndex)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetBatchedParameter(this, dataAdapter, commandIdentifier, parameterIndex);
			}
			return dataAdapter.do_GetBatchedParameter(commandIdentifier, parameterIndex);
		}

		internal bool GetBatchedRecordsAffected(DmDataAdapter dataAdapter, int commandIdentifier, out int recordsAffected, out Exception error)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetBatchedRecordsAffected(this, dataAdapter, commandIdentifier, out recordsAffected, out error);
			}
			return dataAdapter.do_GetBatchedRecordsAffected(commandIdentifier, out recordsAffected, out error);
		}

		internal void InitializeBatching(DmDataAdapter dataAdapter)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].InitializeBatching(this, dataAdapter);
			}
			else
			{
				dataAdapter.do_InitializeBatching();
			}
		}

		internal void OnRowUpdated(DmDataAdapter dataAdapter, RowUpdatedEventArgs value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].OnRowUpdated(this, dataAdapter, value);
			}
			else
			{
				dataAdapter.do_OnRowUpdated(value);
			}
		}

		internal void OnRowUpdating(DmDataAdapter dataAdapter, RowUpdatingEventArgs value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].OnRowUpdating(this, dataAdapter, value);
			}
			else
			{
				dataAdapter.do_OnRowUpdating(value);
			}
		}

		internal void TerminateBatching(DmDataAdapter dataAdapter)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].TerminateBatching(this, dataAdapter);
			}
			else
			{
				dataAdapter.do_TerminateBatching();
			}
		}

		internal int Update(DmDataAdapter dataAdapter, DataRow[] dataRows, DataTableMapping tableMapping)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].Update(this, dataAdapter, dataRows, tableMapping);
			}
			return dataAdapter.do_Update(dataRows, tableMapping);
		}

		internal string getQuoteSuffix(DmCommandBuilder commandBuilder)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getQuoteSuffix(this, commandBuilder);
			}
			return commandBuilder.do_QuoteSuffix;
		}

		internal void setQuoteSuffix(DmCommandBuilder commandBuilder, string value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setQuoteSuffix(this, commandBuilder, value);
			}
			else
			{
				commandBuilder.do_QuoteSuffix = value;
			}
		}

		internal string getQuotePrefix(DmCommandBuilder commandBuilder)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getQuotePrefix(this, commandBuilder);
			}
			return commandBuilder.do_QuotePrefix;
		}

		internal void setQuotePrefix(DmCommandBuilder commandBuilder, string value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setQuotePrefix(this, commandBuilder, value);
			}
			else
			{
				commandBuilder.do_QuotePrefix = value;
			}
		}

		internal string getCatalogSeparator(DmCommandBuilder commandBuilder)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getCatalogSeparator(this, commandBuilder);
			}
			return commandBuilder.do_CatalogSeparator;
		}

		internal void setCatalogSeparator(DmCommandBuilder commandBuilder, string value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setCatalogSeparator(this, commandBuilder, value);
			}
			else
			{
				commandBuilder.do_CatalogSeparator = value;
			}
		}

		internal CatalogLocation getCatalogLocation(DmCommandBuilder commandBuilder)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getCatalogLocation(this, commandBuilder);
			}
			return commandBuilder.do_CatalogLocation;
		}

		internal void setCatalogLocation(DmCommandBuilder commandBuilder, CatalogLocation value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setCatalogLocation(this, commandBuilder, value);
			}
			else
			{
				commandBuilder.do_CatalogLocation = value;
			}
		}

		internal ConflictOption getConflictOption(DmCommandBuilder commandBuilder)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getConflictOption(this, commandBuilder);
			}
			return commandBuilder.do_ConflictOption;
		}

		internal void setConflictOption(DmCommandBuilder commandBuilder, ConflictOption value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setConflictOption(this, commandBuilder, value);
			}
			else
			{
				commandBuilder.do_ConflictOption = value;
			}
		}

		internal string getSchemaSeparator(DmCommandBuilder commandBuilder)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getSchemaSeparator(this, commandBuilder);
			}
			return commandBuilder.do_SchemaSeparator;
		}

		internal void setSchemaSeparator(DmCommandBuilder commandBuilder, string value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setSchemaSeparator(this, commandBuilder, value);
			}
			else
			{
				commandBuilder.do_SchemaSeparator = value;
			}
		}

		internal string QuoteIdentifier(DmCommandBuilder commandBuilder, string unquotedIdentifier)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].QuoteIdentifier(this, commandBuilder, unquotedIdentifier);
			}
			return commandBuilder.do_QuoteIdentifier(unquotedIdentifier);
		}

		internal void RefreshSchema(DmCommandBuilder commandBuilder)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].RefreshSchema(this, commandBuilder);
			}
			else
			{
				commandBuilder.do_RefreshSchema();
			}
		}

		internal string UnquoteIdentifier(DmCommandBuilder commandBuilder, string quotedIdentifier)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].UnquoteIdentifier(this, commandBuilder, quotedIdentifier);
			}
			return commandBuilder.do_UnquoteIdentifier(quotedIdentifier);
		}

		internal void ApplyParameterInfo(DmCommandBuilder commandBuilder, DmParameter parameter, DataRow row, StatementType statementType, bool whereClause)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].ApplyParameterInfo(this, commandBuilder, parameter, row, statementType, whereClause);
			}
			else
			{
				commandBuilder.do_ApplyParameterInfo(parameter, row, statementType, whereClause);
			}
		}

		internal string GetParameterName(DmCommandBuilder commandBuilder, int parameterOrdinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetParameterName(this, commandBuilder, parameterOrdinal);
			}
			return commandBuilder.do_GetParameterName(parameterOrdinal);
		}

		internal string GetParameterName(DmCommandBuilder commandBuilder, string parameterName)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetParameterName(this, commandBuilder, parameterName);
			}
			return commandBuilder.do_GetParameterName(parameterName);
		}

		internal string GetParameterPlaceholder(DmCommandBuilder commandBuilder, int parameterOrdinal)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetParameterPlaceholder(this, commandBuilder, parameterOrdinal);
			}
			return commandBuilder.do_GetParameterPlaceholder(parameterOrdinal);
		}

		internal DataTable GetSchemaTable(DmCommandBuilder commandBuilder, DmCommand sourceCommand)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].GetSchemaTable(this, commandBuilder, sourceCommand);
			}
			return commandBuilder.do_GetSchemaTable(sourceCommand);
		}

		internal DmCommand InitializeCommand(DmCommandBuilder commandBuilder, DmCommand command)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].InitializeCommand(this, commandBuilder, command);
			}
			return commandBuilder.do_InitializeCommand(command);
		}

		internal void SetRowUpdatingHandler(DmCommandBuilder commandBuilder, DmDataAdapter adapter)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].SetRowUpdatingHandler(this, commandBuilder, adapter);
			}
			else
			{
				commandBuilder.do_SetRowUpdatingHandler(adapter);
			}
		}

		internal object getThis(DmConnectionStringBuilder connectionStringBuilder, string keyword)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getThis(this, connectionStringBuilder, keyword);
			}
			return connectionStringBuilder.do_getThis(keyword);
		}

		internal void setThis(DmConnectionStringBuilder connectionStringBuilder, string keyword, object value)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].setThis(this, connectionStringBuilder, keyword, value);
			}
			else
			{
				connectionStringBuilder.do_setThis(keyword, value);
			}
		}

		internal bool getIsFixedSize(DmConnectionStringBuilder connectionStringBuilder)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getIsFixedSize(this, connectionStringBuilder);
			}
			return connectionStringBuilder.do_IsFixedSize;
		}

		internal int getCount(DmConnectionStringBuilder connectionStringBuilder)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getCount(this, connectionStringBuilder);
			}
			return connectionStringBuilder.do_Count;
		}

		internal ICollection getKeys(DmConnectionStringBuilder connectionStringBuilder)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getKeys(this, connectionStringBuilder);
			}
			return connectionStringBuilder.do_Keys;
		}

		internal ICollection getValues(DmConnectionStringBuilder connectionStringBuilder)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].getValues(this, connectionStringBuilder);
			}
			return connectionStringBuilder.do_Values;
		}

		internal void Clear(DmConnectionStringBuilder connectionStringBuilder)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].Clear(this, connectionStringBuilder);
			}
			else
			{
				connectionStringBuilder.do_Clear();
			}
		}

		internal bool ContainsKey(DmConnectionStringBuilder connectionStringBuilder, string keyword)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].ContainsKey(this, connectionStringBuilder, keyword);
			}
			return connectionStringBuilder.do_ContainsKey(keyword);
		}

		internal bool EquivalentTo(DmConnectionStringBuilder sourceConnectionStringBuilder, DmConnectionStringBuilder destConnectionStringBuilder)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].EquivalentTo(this, sourceConnectionStringBuilder, destConnectionStringBuilder);
			}
			return sourceConnectionStringBuilder.do_EquivalentTo(destConnectionStringBuilder);
		}

		internal bool Remove(DmConnectionStringBuilder connectionStringBuilder, string keyword)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].Remove(this, connectionStringBuilder, keyword);
			}
			return connectionStringBuilder.do_Remove(keyword);
		}

		internal bool ShouldSerialize(DmConnectionStringBuilder connectionStringBuilder, string keyword)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].ShouldSerialize(this, connectionStringBuilder, keyword);
			}
			return connectionStringBuilder.do_ShouldSerialize(keyword);
		}

		internal bool TryGetValue(DmConnectionStringBuilder connectionStringBuilder, string keyword, out object value)
		{
			if (fpos < filters.Length)
			{
				return filters[fpos++].TryGetValue(this, connectionStringBuilder, keyword, out value);
			}
			return connectionStringBuilder.do_TryGetValue(keyword, out value);
		}

		internal void GetProperties(DmConnectionStringBuilder connectionStringBuilder, Hashtable propertyDescriptors)
		{
			if (fpos < filters.Length)
			{
				filters[fpos++].GetProperties(this, connectionStringBuilder, propertyDescriptors);
			}
			else
			{
				connectionStringBuilder.do_GetProperties(propertyDescriptors);
			}
		}
	}
}
