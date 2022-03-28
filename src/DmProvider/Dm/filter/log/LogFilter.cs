using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace Dm.filter.log
{
	internal class LogFilter : IFilter
	{
		private class LogFilterThreadHolder
		{
			internal static readonly LogFilter instance = new LogFilter();
		}

		private readonly ILogger LOG = LogFactory.getLog(typeof(LogFilter));

		internal static LogFilter Instance => LogFilterThreadHolder.instance;

		private LogFilter()
		{
		}

		private void doLog(LogRecord logRecord)
		{
			if (logRecord == null)
			{
				return;
			}
			try
			{
				if (logRecord.Throwable != null)
				{
					LOG.Error(logRecord.ToString(), logRecord.Throwable);
				}
				else if (logRecord.Sql != null && LOG.SqlEnabled)
				{
					LOG.Sql(logRecord.ToString());
				}
				else
				{
					LOG.Info(logRecord.ToString());
				}
			}
			catch (Exception t)
			{
				LOG.Error("Log failed!", t);
			}
		}

		public string getServerVersion(FilterChain chain, DmConnection conn)
		{
			LogRecord logRecord = new LogRecord(conn, "getServerVersion");
			try
			{
				return (string)(logRecord.ReturnValue = chain.getServerVersion(conn));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string getDataSource(FilterChain chain, DmConnection conn)
		{
			LogRecord logRecord = new LogRecord(conn, "getDataSource");
			try
			{
				return (string)(logRecord.ReturnValue = chain.getDataSource(conn));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string getDatabase(FilterChain chain, DmConnection conn)
		{
			LogRecord logRecord = new LogRecord(conn, "getDatabase");
			try
			{
				return (string)(logRecord.ReturnValue = chain.getDatabase(conn));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int getConnectionTimeout(FilterChain chain, DmConnection conn)
		{
			LogRecord logRecord = new LogRecord(conn, "getConnectionTimeout");
			try
			{
				int connectionTimeout = chain.getConnectionTimeout(conn);
				logRecord.ReturnValue = connectionTimeout;
				return connectionTimeout;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string getConnectionString(FilterChain chain, DmConnection conn)
		{
			LogRecord logRecord = new LogRecord(conn, "getConnectionString");
			try
			{
				return (string)(logRecord.ReturnValue = chain.getConnectionString(conn));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DbProviderFactory getDbProviderFactory(FilterChain chain, DmConnection conn)
		{
			LogRecord logRecord = new LogRecord(conn, "getDbProviderFactory");
			try
			{
				return (DbProviderFactory)(logRecord.ReturnValue = chain.getDbProviderFactory(conn));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public ConnectionState getState(FilterChain chain, DmConnection conn)
		{
			LogRecord logRecord = new LogRecord(conn, "getState");
			try
			{
				ConnectionState state = chain.getState(conn);
				logRecord.ReturnValue = state;
				return state;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DmTransaction BeginDbTransaction(FilterChain chain, DmConnection conn, System.Data.IsolationLevel isolationLevel)
		{
			LogRecord logRecord = new LogRecord(conn, "BeginTransaction", isolationLevel);
			try
			{
				return (DmTransaction)(logRecord.ReturnValue = chain.BeginDbTransaction(conn, isolationLevel));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void ChangeDatabase(FilterChain chain, DmConnection conn, string databaseName)
		{
			LogRecord logRecord = new LogRecord(conn, "ChangeDatabase", databaseName);
			try
			{
				chain.ChangeDatabase(conn, databaseName);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void Close(FilterChain chain, DmConnection conn)
		{
			LogRecord logRecord = new LogRecord(conn, "Close");
			try
			{
				chain.Close(conn);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DmCommand CreateDbCommand(FilterChain chain, DmConnection conn)
		{
			LogRecord logRecord = new LogRecord(conn, "CreateDbCommand");
			try
			{
				return (DmCommand)(logRecord.ReturnValue = chain.CreateDbCommand(conn));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void EnlistTransaction(FilterChain chain, DmConnection conn, Transaction transaction)
		{
			LogRecord logRecord = new LogRecord(conn, "EnlistTransaction");
			try
			{
				chain.EnlistTransaction(conn, transaction);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DataTable GetSchema(FilterChain chain, DmConnection conn)
		{
			LogRecord logRecord = new LogRecord(conn, "GetSchema");
			try
			{
				return (DataTable)(logRecord.ReturnValue = chain.GetSchema(conn));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DataTable GetSchema(FilterChain chain, DmConnection conn, string collectionName, string[] restrictionValues)
		{
			LogRecord logRecord = new LogRecord(conn, "GetSchema", collectionName, restrictionValues);
			try
			{
				return (DataTable)(logRecord.ReturnValue = chain.GetSchema(conn, collectionName, restrictionValues));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DataTable GetSchema(FilterChain chain, DmConnection conn, string collectionName)
		{
			LogRecord logRecord = new LogRecord(conn, "GetSchema", collectionName);
			try
			{
				return (DataTable)(logRecord.ReturnValue = chain.GetSchema(conn, collectionName));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void Open(FilterChain chain, DmConnection conn)
		{
			LogRecord logRecord = new LogRecord(conn, "Open");
			try
			{
				chain.Open(conn);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool getDesignTimeVisible(FilterChain chain, DmCommand command)
		{
			LogRecord logRecord = new LogRecord(command, "getDesignTimeVisible");
			try
			{
				bool designTimeVisible = chain.getDesignTimeVisible(command);
				logRecord.ReturnValue = designTimeVisible;
				return designTimeVisible;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setDesignTimeVisible(FilterChain chain, DmCommand command, bool value)
		{
			LogRecord logRecord = new LogRecord(command, "setDesignTimeVisible", value);
			try
			{
				chain.setDesignTimeVisible(command, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public CommandType getCommandType(FilterChain chain, DmCommand command)
		{
			LogRecord logRecord = new LogRecord(command, "getCommandType");
			try
			{
				CommandType commandType = chain.getCommandType(command);
				logRecord.ReturnValue = commandType;
				return commandType;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setCommandType(FilterChain chain, DmCommand command, CommandType value)
		{
			LogRecord logRecord = new LogRecord(command, "setCommandType", value);
			try
			{
				chain.setCommandType(command, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int getCommandTimeout(FilterChain chain, DmCommand command)
		{
			LogRecord logRecord = new LogRecord(command, "getCommandTimeout");
			try
			{
				int commandTimeout = chain.getCommandTimeout(command);
				logRecord.ReturnValue = commandTimeout;
				return commandTimeout;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setCommandTimeout(FilterChain chain, DmCommand command, int value)
		{
			LogRecord logRecord = new LogRecord(command, "setCommandTimeout", value);
			try
			{
				chain.setCommandTimeout(command, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string getCommandText(FilterChain chain, DmCommand command)
		{
			LogRecord logRecord = new LogRecord(command, "getCommandText");
			try
			{
				return (string)(logRecord.ReturnValue = chain.getCommandText(command));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setCommandText(FilterChain chain, DmCommand command, string value)
		{
			LogRecord logRecord = new LogRecord(command, "setCommandText", value);
			try
			{
				chain.setCommandText(command, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public UpdateRowSource getUpdatedRowSource(FilterChain chain, DmCommand command)
		{
			LogRecord logRecord = new LogRecord(command, "getUpdatedRowSource");
			try
			{
				UpdateRowSource updatedRowSource = chain.getUpdatedRowSource(command);
				logRecord.ReturnValue = updatedRowSource;
				return updatedRowSource;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setUpdatedRowSource(FilterChain chain, DmCommand command, UpdateRowSource value)
		{
			LogRecord logRecord = new LogRecord(command, "setUpdatedRowSource", value);
			try
			{
				chain.setUpdatedRowSource(command, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DmConnection getDbConnection(FilterChain chain, DmCommand command)
		{
			LogRecord logRecord = new LogRecord(command, "getDbConnection");
			try
			{
				return (DmConnection)(logRecord.ReturnValue = chain.getDbConnection(command));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setDbConnection(FilterChain chain, DmCommand command, DmConnection value)
		{
			LogRecord logRecord = new LogRecord(command, "setDbConnection", value);
			try
			{
				chain.setDbConnection(command, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DmParameterCollection getDbParameterCollection(FilterChain chain, DmCommand command)
		{
			LogRecord logRecord = new LogRecord(command, "getDbParameterCollection");
			try
			{
				return (DmParameterCollection)(logRecord.ReturnValue = chain.getDbParameterCollection(command));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DmTransaction getDbTransaction(FilterChain chain, DmCommand command)
		{
			LogRecord logRecord = new LogRecord(command, "getDbTransaction");
			try
			{
				return (DmTransaction)(logRecord.ReturnValue = chain.getDbTransaction(command));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setDbTransaction(FilterChain chain, DmCommand command, DmTransaction value)
		{
			LogRecord logRecord = new LogRecord(command, "setDbTransaction", value);
			try
			{
				chain.setDbTransaction(command, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void Cancel(FilterChain chain, DmCommand command)
		{
			LogRecord logRecord = new LogRecord(command, "Cancel");
			try
			{
				chain.Cancel(command);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int ExecuteNonQuery(FilterChain chain, DmCommand command)
		{
			LogRecord logRecord = new LogRecord(command, "ExecuteNonQuery");
			try
			{
				logRecord.Sql = command.do_CommandText;
				ExecuteBefore(command, "ExecuteNonQuery");
				int num = chain.ExecuteNonQuery(command);
				logRecord.ReturnValue = num;
				return num;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				ExecuteAfter(command, logRecord);
				doLog(logRecord);
			}
		}

		public object ExecuteScalar(FilterChain chain, DmCommand command)
		{
			LogRecord logRecord = new LogRecord(command, "ExecuteScalar");
			try
			{
				logRecord.Sql = command.do_CommandText;
				ExecuteBefore(command, "ExecuteScalar");
				return logRecord.ReturnValue = chain.ExecuteScalar(command);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				ExecuteAfter(command, logRecord);
				doLog(logRecord);
			}
		}

		public void Prepare(FilterChain chain, DmCommand command)
		{
			LogRecord logRecord = new LogRecord(command, "Prepare");
			try
			{
				logRecord.Sql = command.do_CommandText;
				ExecuteBefore(command, "Prepare");
				chain.Prepare(command);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				ExecuteAfter(command, logRecord);
				doLog(logRecord);
			}
		}

		public DmParameter CreateDbParameter(FilterChain chain, DmCommand command)
		{
			LogRecord logRecord = new LogRecord(command, "CreateDbParameter");
			try
			{
				return (DmParameter)(logRecord.ReturnValue = chain.CreateDbParameter(command));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DmDataReader ExecuteDbDataReader(FilterChain chain, DmCommand command, CommandBehavior behavior)
		{
			LogRecord logRecord = new LogRecord(command, "ExecuteDbDataReader", behavior);
			try
			{
				logRecord.Sql = command.do_CommandText;
				ExecuteBefore(command, "ExecuteDbDataReader");
				return (DmDataReader)(logRecord.ReturnValue = chain.ExecuteDbDataReader(command, behavior));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				ExecuteAfter(command, logRecord);
				doLog(logRecord);
			}
		}

		private void ExecuteBefore(DmCommand command, string method)
		{
			if (LOG.SqlEnabled)
			{
				command.LogInfo.lastExecuteStartNano = DateTime.Now;
			}
		}

		private void ExecuteAfter(DmCommand command, LogRecord logRecord)
		{
			if (LOG.SqlEnabled)
			{
				logRecord.UsedTime = (int)((DateTime.Now - command.LogInfo.lastExecuteStartNano).TotalMilliseconds * Math.Pow(10.0, 6.0));
			}
		}

		public System.Data.IsolationLevel getIsolationLevel(FilterChain chain, DmTransaction transaction)
		{
			LogRecord logRecord = new LogRecord(transaction, "getIsolationLevel");
			try
			{
				System.Data.IsolationLevel isolationLevel = chain.getIsolationLevel(transaction);
				logRecord.ReturnValue = isolationLevel;
				return isolationLevel;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DmConnection getDbConnection(FilterChain chain, DmTransaction transaction)
		{
			LogRecord logRecord = new LogRecord(transaction, "getDbConnection");
			try
			{
				return (DmConnection)(logRecord.ReturnValue = chain.getDbConnection(transaction));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void Commit(FilterChain chain, DmTransaction transaction)
		{
			LogRecord logRecord = new LogRecord(transaction, "Commit");
			try
			{
				chain.Commit(transaction);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void Rollback(FilterChain chain, DmTransaction transaction)
		{
			LogRecord logRecord = new LogRecord(transaction, "Rollback");
			try
			{
				chain.Rollback(transaction);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void Dispose(FilterChain chain, DmTransaction transaction, bool disposing)
		{
			LogRecord logRecord = new LogRecord(transaction, "Dispose", disposing);
			try
			{
				chain.Dispose(transaction, disposing);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public object getThis(FilterChain chain, DmDataReader dataReader, int index)
		{
			LogRecord logRecord = new LogRecord(dataReader, "getThis", index);
			try
			{
				return logRecord.ReturnValue = chain.getThis(dataReader, index);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public object getThis(FilterChain chain, DmDataReader dataReader, string name)
		{
			LogRecord logRecord = new LogRecord(dataReader, "getThis", name);
			try
			{
				return logRecord.ReturnValue = chain.getThis(dataReader, name);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int getDepth(FilterChain chain, DmDataReader dataReader)
		{
			LogRecord logRecord = new LogRecord(dataReader, "getDepth");
			try
			{
				int depth = chain.getDepth(dataReader);
				logRecord.ReturnValue = depth;
				return depth;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool getHasRows(FilterChain chain, DmDataReader dataReader)
		{
			LogRecord logRecord = new LogRecord(dataReader, "getHasRows");
			try
			{
				bool hasRows = chain.getHasRows(dataReader);
				logRecord.ReturnValue = hasRows;
				return hasRows;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int getVisibleFieldCount(FilterChain chain, DmDataReader dataReader)
		{
			LogRecord logRecord = new LogRecord(dataReader, "getVisibleFieldCount");
			try
			{
				int visibleFieldCount = chain.getVisibleFieldCount(dataReader);
				logRecord.ReturnValue = visibleFieldCount;
				return visibleFieldCount;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int getRecordsAffected(FilterChain chain, DmDataReader dataReader)
		{
			LogRecord logRecord = new LogRecord(dataReader, "getRecordsAffected");
			try
			{
				int recordsAffected = chain.getRecordsAffected(dataReader);
				logRecord.ReturnValue = recordsAffected;
				return recordsAffected;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool getIsClosed(FilterChain chain, DmDataReader dataReader)
		{
			LogRecord logRecord = new LogRecord(dataReader, "getIsClosed");
			try
			{
				bool isClosed = chain.getIsClosed(dataReader);
				logRecord.ReturnValue = isClosed;
				return isClosed;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int getFieldCount(FilterChain chain, DmDataReader dataReader)
		{
			LogRecord logRecord = new LogRecord(dataReader, "getFieldCount");
			try
			{
				int fieldCount = chain.getFieldCount(dataReader);
				logRecord.ReturnValue = fieldCount;
				return fieldCount;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void Close(FilterChain chain, DmDataReader dataReader)
		{
			LogRecord logRecord = new LogRecord(dataReader, "Close");
			try
			{
				chain.Close(dataReader);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool GetBoolean(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetBoolean", ordinal);
			try
			{
				bool boolean = chain.GetBoolean(dataReader, ordinal);
				logRecord.ReturnValue = boolean;
				return boolean;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public byte GetByte(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetByte", ordinal);
			try
			{
				byte @byte = chain.GetByte(dataReader, ordinal);
				logRecord.ReturnValue = @byte;
				return @byte;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public long GetBytes(FilterChain chain, DmDataReader dataReader, int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetBytes", ordinal, dataOffset, buffer, bufferOffset, length);
			try
			{
				long bytes = chain.GetBytes(dataReader, ordinal, dataOffset, buffer, bufferOffset, length);
				logRecord.ReturnValue = bytes;
				return bytes;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public char GetChar(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetChar", ordinal);
			try
			{
				char @char = chain.GetChar(dataReader, ordinal);
				logRecord.ReturnValue = @char;
				return @char;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public long GetChars(FilterChain chain, DmDataReader dataReader, int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetChars", ordinal, dataOffset, buffer, bufferOffset, length);
			try
			{
				long chars = chain.GetChars(dataReader, ordinal, dataOffset, buffer, bufferOffset, length);
				logRecord.ReturnValue = chars;
				return chars;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string GetDataTypeName(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetDataTypeName", ordinal);
			try
			{
				return (string)(logRecord.ReturnValue = chain.GetDataTypeName(dataReader, ordinal));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DateTime GetDateTime(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetDateTime", ordinal);
			try
			{
				DateTime dateTime = chain.GetDateTime(dataReader, ordinal);
				logRecord.ReturnValue = dateTime;
				return dateTime;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public decimal GetDecimal(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetDecimal", ordinal);
			try
			{
				decimal @decimal = chain.GetDecimal(dataReader, ordinal);
				logRecord.ReturnValue = @decimal;
				return @decimal;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public double GetDouble(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetDouble", ordinal);
			try
			{
				double @double = chain.GetDouble(dataReader, ordinal);
				logRecord.ReturnValue = @double;
				return @double;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public IEnumerator GetEnumerator(FilterChain chain, DmDataReader dataReader)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetEnumerator");
			try
			{
				return (IEnumerator)(logRecord.ReturnValue = chain.GetEnumerator(dataReader));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public Type GetFieldType(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetFieldType", ordinal);
			try
			{
				return (Type)(logRecord.ReturnValue = chain.GetFieldType(dataReader, ordinal));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public float GetFloat(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetFloat", ordinal);
			try
			{
				float @float = chain.GetFloat(dataReader, ordinal);
				logRecord.ReturnValue = @float;
				return @float;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public Guid GetGuid(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetGuid", ordinal);
			try
			{
				Guid guid = chain.GetGuid(dataReader, ordinal);
				logRecord.ReturnValue = guid;
				return guid;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public short GetInt16(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetInt16", ordinal);
			try
			{
				short @int = chain.GetInt16(dataReader, ordinal);
				logRecord.ReturnValue = @int;
				return @int;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int GetInt32(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetInt32", ordinal);
			try
			{
				int @int = chain.GetInt32(dataReader, ordinal);
				logRecord.ReturnValue = @int;
				return @int;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public long GetInt64(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetInt64", ordinal);
			try
			{
				long @int = chain.GetInt64(dataReader, ordinal);
				logRecord.ReturnValue = @int;
				return @int;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string GetName(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetName", ordinal);
			try
			{
				return (string)(logRecord.ReturnValue = chain.GetName(dataReader, ordinal));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int GetOrdinal(FilterChain chain, DmDataReader dataReader, string name)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetOrdinal", name);
			try
			{
				int ordinal = chain.GetOrdinal(dataReader, name);
				logRecord.ReturnValue = ordinal;
				return ordinal;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public Type GetProviderSpecificFieldType(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetProviderSpecificFieldType", ordinal);
			try
			{
				return (Type)(logRecord.ReturnValue = chain.GetProviderSpecificFieldType(dataReader, ordinal));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public object GetProviderSpecificValue(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetProviderSpecificValue", ordinal);
			try
			{
				return logRecord.ReturnValue = chain.GetProviderSpecificValue(dataReader, ordinal);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int GetProviderSpecificValues(FilterChain chain, DmDataReader dataReader, object[] values)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetProviderSpecificValues", values);
			try
			{
				int providerSpecificValues = chain.GetProviderSpecificValues(dataReader, values);
				logRecord.ReturnValue = providerSpecificValues;
				return providerSpecificValues;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DataTable GetSchemaTable(FilterChain chain, DmDataReader dataReader)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetSchemaTable");
			try
			{
				return (DataTable)(logRecord.ReturnValue = chain.GetSchemaTable(dataReader));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string GetString(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetString", ordinal);
			try
			{
				return (string)(logRecord.ReturnValue = chain.GetString(dataReader, ordinal));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public object GetValue(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetValue", ordinal);
			try
			{
				return logRecord.ReturnValue = chain.GetValue(dataReader, ordinal);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int GetValues(FilterChain chain, DmDataReader dataReader, object[] values)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetValues", values);
			try
			{
				int values2 = chain.GetValues(dataReader, values);
				logRecord.ReturnValue = values2;
				return values2;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool IsDBNull(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "IsDBNull", ordinal);
			try
			{
				bool flag = chain.IsDBNull(dataReader, ordinal);
				logRecord.ReturnValue = flag;
				return flag;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool NextResult(FilterChain chain, DmDataReader dataReader)
		{
			LogRecord logRecord = new LogRecord(dataReader, "NextResult");
			try
			{
				bool flag = chain.NextResult(dataReader);
				logRecord.ReturnValue = flag;
				return flag;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool Read(FilterChain chain, DmDataReader dataReader)
		{
			LogRecord logRecord = new LogRecord(dataReader, "Read");
			try
			{
				bool flag = chain.Read(dataReader);
				logRecord.ReturnValue = flag;
				return flag;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void Dispose(FilterChain chain, DmDataReader dataReader, bool disposing)
		{
			LogRecord logRecord = new LogRecord(dataReader, "Dispose", disposing);
			try
			{
				chain.Dispose(dataReader, disposing);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DmDataReader GetDbDataReader(FilterChain chain, DmDataReader dataReader, int ordinal)
		{
			LogRecord logRecord = new LogRecord(dataReader, "GetDbDataReader", ordinal);
			try
			{
				return (DmDataReader)(logRecord.ReturnValue = chain.GetDbDataReader(dataReader, ordinal));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public object getSyncRoot(FilterChain chain, DmParameterCollection parameterCollection)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "getSyncRoot");
			try
			{
				return logRecord.ReturnValue = chain.getSyncRoot(parameterCollection);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool getIsSynchronized(FilterChain chain, DmParameterCollection parameterCollection)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "getIsSynchronized");
			try
			{
				bool isSynchronized = chain.getIsSynchronized(parameterCollection);
				logRecord.ReturnValue = isSynchronized;
				return isSynchronized;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool getIsReadOnly(FilterChain chain, DmParameterCollection parameterCollection)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "getIsReadOnly");
			try
			{
				bool isReadOnly = chain.getIsReadOnly(parameterCollection);
				logRecord.ReturnValue = isReadOnly;
				return isReadOnly;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool getIsFixedSize(FilterChain chain, DmParameterCollection parameterCollection)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "getIsFixedSize");
			try
			{
				bool isFixedSize = chain.getIsFixedSize(parameterCollection);
				logRecord.ReturnValue = isFixedSize;
				return isFixedSize;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int getCount(FilterChain chain, DmParameterCollection parameterCollection)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "getCount");
			try
			{
				int count = chain.getCount(parameterCollection);
				logRecord.ReturnValue = count;
				return count;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int Add(FilterChain chain, DmParameterCollection parameterCollection, object value)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "Add", value);
			try
			{
				int num = chain.Add(parameterCollection, value);
				logRecord.ReturnValue = num;
				return num;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void AddRange(FilterChain chain, DmParameterCollection parameterCollection, Array values)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "AddRange", values);
			try
			{
				chain.AddRange(parameterCollection, values);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void Clear(FilterChain chain, DmParameterCollection parameterCollection)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "Clear");
			try
			{
				chain.Clear(parameterCollection);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool Contains(FilterChain chain, DmParameterCollection parameterCollection, object value)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "Contains", value);
			try
			{
				bool flag = chain.Contains(parameterCollection, value);
				logRecord.ReturnValue = flag;
				return flag;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool Contains(FilterChain chain, DmParameterCollection parameterCollection, string value)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "Contains", value);
			try
			{
				bool flag = chain.Contains(parameterCollection, value);
				logRecord.ReturnValue = flag;
				return flag;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void CopyTo(FilterChain chain, DmParameterCollection parameterCollection, Array array, int index)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "CopyTo", array, index);
			try
			{
				chain.CopyTo(parameterCollection, array, index);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public IEnumerator GetEnumerator(FilterChain chain, DmParameterCollection parameterCollection)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "GetEnumerator");
			try
			{
				return (IEnumerator)(logRecord.ReturnValue = chain.GetEnumerator(parameterCollection));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int IndexOf(FilterChain chain, DmParameterCollection parameterCollection, object value)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "IndexOf", value);
			try
			{
				int num = chain.IndexOf(parameterCollection, value);
				logRecord.ReturnValue = num;
				return num;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int IndexOf(FilterChain chain, DmParameterCollection parameterCollection, string parameterName)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "IndexOf", parameterName);
			try
			{
				int num = chain.IndexOf(parameterCollection, parameterName);
				logRecord.ReturnValue = num;
				return num;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void Insert(FilterChain chain, DmParameterCollection parameterCollection, int index, object value)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "Insert", index, value);
			try
			{
				chain.Insert(parameterCollection, index, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void Remove(FilterChain chain, DmParameterCollection parameterCollection, object value)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "Remove", value);
			try
			{
				chain.Remove(parameterCollection, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void RemoveAt(FilterChain chain, DmParameterCollection parameterCollection, string parameterName)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "RemoveAt", parameterName);
			try
			{
				chain.RemoveAt(parameterCollection, parameterName);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void RemoveAt(FilterChain chain, DmParameterCollection parameterCollection, int index)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "RemoveAt", index);
			try
			{
				chain.RemoveAt(parameterCollection, index);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DmParameter GetParameter(FilterChain chain, DmParameterCollection parameterCollection, string parameterName)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "GetParameter", parameterName);
			try
			{
				return (DmParameter)(logRecord.ReturnValue = chain.GetParameter(parameterCollection, parameterName));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DmParameter GetParameter(FilterChain chain, DmParameterCollection parameterCollection, int index)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "GetParameter", index);
			try
			{
				return (DmParameter)(logRecord.ReturnValue = chain.GetParameter(parameterCollection, index));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void SetParameter(FilterChain chain, DmParameterCollection parameterCollection, int index, DmParameter value)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "SetParameter", index, value);
			try
			{
				chain.SetParameter(parameterCollection, index, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void SetParameter(FilterChain chain, DmParameterCollection parameterCollection, string parameterName, DmParameter value)
		{
			LogRecord logRecord = new LogRecord(parameterCollection, "SetParameter", parameterName, value);
			try
			{
				chain.SetParameter(parameterCollection, parameterName, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DbType getDbType(FilterChain chain, DmParameter parameter)
		{
			LogRecord logRecord = new LogRecord(parameter, "getDbType");
			try
			{
				DbType dbType = chain.getDbType(parameter);
				logRecord.ReturnValue = dbType;
				return dbType;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setDbType(FilterChain chain, DmParameter parameter, DbType value)
		{
			LogRecord logRecord = new LogRecord(parameter, "setDbType", value);
			try
			{
				chain.setDbType(parameter, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public ParameterDirection getDirection(FilterChain chain, DmParameter parameter)
		{
			LogRecord logRecord = new LogRecord(parameter, "getDirection");
			try
			{
				ParameterDirection direction = chain.getDirection(parameter);
				logRecord.ReturnValue = direction;
				return direction;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setDirection(FilterChain chain, DmParameter parameter, ParameterDirection value)
		{
			LogRecord logRecord = new LogRecord(parameter, "setDirection", value);
			try
			{
				chain.setDirection(parameter, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool getIsNullable(FilterChain chain, DmParameter parameter)
		{
			LogRecord logRecord = new LogRecord(parameter, "getIsNullable");
			try
			{
				bool isNullable = chain.getIsNullable(parameter);
				logRecord.ReturnValue = isNullable;
				return isNullable;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setIsNullable(FilterChain chain, DmParameter parameter, bool value)
		{
			LogRecord logRecord = new LogRecord(parameter, "setIsNullable", value);
			try
			{
				chain.setIsNullable(parameter, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string getParameterName(FilterChain chain, DmParameter parameter)
		{
			LogRecord logRecord = new LogRecord(parameter, "getParameterName");
			try
			{
				return (string)(logRecord.ReturnValue = chain.getParameterName(parameter));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setParameterName(FilterChain chain, DmParameter parameter, string value)
		{
			LogRecord logRecord = new LogRecord(parameter, "setParameterName", value);
			try
			{
				chain.setParameterName(parameter, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public byte getPrecision(FilterChain chain, DmParameter parameter)
		{
			LogRecord logRecord = new LogRecord(parameter, "getPrecision");
			try
			{
				byte precision = chain.getPrecision(parameter);
				logRecord.ReturnValue = precision;
				return precision;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setPrecision(FilterChain chain, DmParameter parameter, byte value)
		{
			LogRecord logRecord = new LogRecord(parameter, "setPrecision", value);
			try
			{
				chain.setPrecision(parameter, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public byte getScale(FilterChain chain, DmParameter parameter)
		{
			LogRecord logRecord = new LogRecord(parameter, "getScale");
			try
			{
				byte scale = chain.getScale(parameter);
				logRecord.ReturnValue = scale;
				return scale;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setScale(FilterChain chain, DmParameter parameter, byte value)
		{
			LogRecord logRecord = new LogRecord(parameter, "setScale", value);
			try
			{
				chain.setScale(parameter, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int getSize(FilterChain chain, DmParameter parameter)
		{
			LogRecord logRecord = new LogRecord(parameter, "getSize");
			try
			{
				int size = chain.getSize(parameter);
				logRecord.ReturnValue = size;
				return size;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setSize(FilterChain chain, DmParameter parameter, int value)
		{
			LogRecord logRecord = new LogRecord(parameter, "setSize", value);
			try
			{
				chain.setSize(parameter, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string getSourceColumn(FilterChain chain, DmParameter parameter)
		{
			LogRecord logRecord = new LogRecord(parameter, "getSourceColumn");
			try
			{
				return (string)(logRecord.ReturnValue = chain.getSourceColumn(parameter));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setSourceColumn(FilterChain chain, DmParameter parameter, string value)
		{
			LogRecord logRecord = new LogRecord(parameter, "setSourceColumn", value);
			try
			{
				chain.setSourceColumn(parameter, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool getSourceColumnNullMapping(FilterChain chain, DmParameter parameter)
		{
			LogRecord logRecord = new LogRecord(parameter, "getSourceColumnNullMapping");
			try
			{
				bool sourceColumnNullMapping = chain.getSourceColumnNullMapping(parameter);
				logRecord.ReturnValue = sourceColumnNullMapping;
				return sourceColumnNullMapping;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setSourceColumnNullMapping(FilterChain chain, DmParameter parameter, bool value)
		{
			LogRecord logRecord = new LogRecord(parameter, "setSourceColumnNullMapping", value);
			try
			{
				chain.setSourceColumnNullMapping(parameter, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DataRowVersion getSourceVersion(FilterChain chain, DmParameter parameter)
		{
			LogRecord logRecord = new LogRecord(parameter, "getSourceVersion");
			try
			{
				DataRowVersion sourceVersion = chain.getSourceVersion(parameter);
				logRecord.ReturnValue = sourceVersion;
				return sourceVersion;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setSourceVersion(FilterChain chain, DmParameter parameter, DataRowVersion value)
		{
			LogRecord logRecord = new LogRecord(parameter, "setSourceVersion", value);
			try
			{
				chain.setSourceVersion(parameter, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public object getValue(FilterChain chain, DmParameter parameter)
		{
			LogRecord logRecord = new LogRecord(parameter, "getValue");
			try
			{
				return logRecord.ReturnValue = chain.getValue(parameter);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setValue(FilterChain chain, DmParameter parameter, object value)
		{
			LogRecord logRecord = new LogRecord(parameter, "setValue", value);
			try
			{
				chain.setValue(parameter, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void ResetDbType(FilterChain chain, DmParameter parameter)
		{
			LogRecord logRecord = new LogRecord(parameter, "ResetDbType");
			try
			{
				chain.ResetDbType(parameter);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int getUpdateBatchSize(FilterChain chain, DmDataAdapter dataAdapter)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "getUpdateBatchSize");
			try
			{
				int updateBatchSize = chain.getUpdateBatchSize(dataAdapter);
				logRecord.ReturnValue = updateBatchSize;
				return updateBatchSize;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setUpdateBatchSize(FilterChain chain, DmDataAdapter dataAdapter, int value)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "setUpdateBatchSize", value);
			try
			{
				chain.setUpdateBatchSize(dataAdapter, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int AddToBatch(FilterChain chain, DmDataAdapter dataAdapter, DmCommand command)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "AddToBatch", command);
			try
			{
				int num = chain.AddToBatch(dataAdapter, command);
				logRecord.ReturnValue = num;
				return num;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void ClearBatch(FilterChain chain, DmDataAdapter dataAdapter)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "ClearBatch");
			try
			{
				chain.ClearBatch(dataAdapter);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public RowUpdatedEventArgs CreateRowUpdatedEvent(FilterChain chain, DmDataAdapter dataAdapter, DataRow dataRow, DmCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "CreateRowUpdatedEvent", dataRow, command, statementType, tableMapping);
			try
			{
				return (RowUpdatedEventArgs)(logRecord.ReturnValue = chain.CreateRowUpdatedEvent(dataAdapter, dataRow, command, statementType, tableMapping));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public RowUpdatingEventArgs CreateRowUpdatingEvent(FilterChain chain, DmDataAdapter dataAdapter, DataRow dataRow, DmCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "CreateRowUpdatingEvent", dataRow, command, statementType, tableMapping);
			try
			{
				return (RowUpdatingEventArgs)(logRecord.ReturnValue = chain.CreateRowUpdatingEvent(dataAdapter, dataRow, command, statementType, tableMapping));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int ExecuteBatch(FilterChain chain, DmDataAdapter dataAdapter)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "ExecuteBatch");
			try
			{
				int num = chain.ExecuteBatch(dataAdapter);
				logRecord.ReturnValue = num;
				return num;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int Fill(FilterChain chain, DmDataAdapter dataAdapter, DataTable[] dataTables, int startRecord, int maxRecords, DmCommand command, CommandBehavior behavior)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "Fill", dataTables, startRecord, maxRecords, command, behavior);
			try
			{
				int num = chain.Fill(dataAdapter, dataTables, startRecord, maxRecords, command, behavior);
				logRecord.ReturnValue = num;
				return num;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int Fill(FilterChain chain, DmDataAdapter dataAdapter, DataTable dataTable, DmCommand command, CommandBehavior behavior)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "Fill", dataTable, command, behavior);
			try
			{
				int num = chain.Fill(dataAdapter, dataTable, command, behavior);
				logRecord.ReturnValue = num;
				return num;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int Fill(FilterChain chain, DmDataAdapter dataAdapter, DataSet dataSet, int startRecord, int maxRecords, string srcTable, DmCommand command, CommandBehavior behavior)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "Fill", dataSet, startRecord, maxRecords, srcTable, command, behavior);
			try
			{
				int num = chain.Fill(dataAdapter, dataSet, startRecord, maxRecords, srcTable, command, behavior);
				logRecord.ReturnValue = num;
				return num;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DataTable FillSchema(FilterChain chain, DmDataAdapter dataAdapter, DataTable dataTable, SchemaType schemaType, DmCommand command, CommandBehavior behavior)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "FillSchema", dataTable, schemaType, command, behavior);
			try
			{
				return (DataTable)(logRecord.ReturnValue = chain.FillSchema(dataAdapter, dataTable, schemaType, command, behavior));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DataTable[] FillSchema(FilterChain chain, DmDataAdapter dataAdapter, DataSet dataSet, SchemaType schemaType, DmCommand command, string srcTable, CommandBehavior behavior)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "FillSchema", dataSet, schemaType, command, srcTable, behavior);
			try
			{
				return (DataTable[])(logRecord.ReturnValue = chain.FillSchema(dataAdapter, dataSet, schemaType, command, srcTable, behavior));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public IDataParameter GetBatchedParameter(FilterChain chain, DmDataAdapter dataAdapter, int commandIdentifier, int parameterIndex)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "GetBatchedParameter", commandIdentifier, parameterIndex);
			try
			{
				return (IDataParameter)(logRecord.ReturnValue = chain.GetBatchedParameter(dataAdapter, commandIdentifier, parameterIndex));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool GetBatchedRecordsAffected(FilterChain chain, DmDataAdapter dataAdapter, int commandIdentifier, out int recordsAffected, out Exception error)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "GetBatchedRecordsAffected", commandIdentifier);
			try
			{
				bool batchedRecordsAffected = chain.GetBatchedRecordsAffected(dataAdapter, commandIdentifier, out recordsAffected, out error);
				logRecord.ReturnValue = batchedRecordsAffected;
				return batchedRecordsAffected;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void InitializeBatching(FilterChain chain, DmDataAdapter dataAdapter)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "InitializeBatching");
			try
			{
				chain.InitializeBatching(dataAdapter);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void OnRowUpdated(FilterChain chain, DmDataAdapter dataAdapter, RowUpdatedEventArgs value)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "OnRowUpdated", value);
			try
			{
				chain.OnRowUpdated(dataAdapter, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void OnRowUpdating(FilterChain chain, DmDataAdapter dataAdapter, RowUpdatingEventArgs value)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "OnRowUpdating", value);
			try
			{
				chain.OnRowUpdating(dataAdapter, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void TerminateBatching(FilterChain chain, DmDataAdapter dataAdapter)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "TerminateBatching");
			try
			{
				chain.TerminateBatching(dataAdapter);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int Update(FilterChain chain, DmDataAdapter dataAdapter, DataRow[] dataRows, DataTableMapping tableMapping)
		{
			LogRecord logRecord = new LogRecord(dataAdapter, "Update", dataRows, tableMapping);
			try
			{
				int num = chain.Update(dataAdapter, dataRows, tableMapping);
				logRecord.ReturnValue = num;
				return num;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string getQuoteSuffix(FilterChain chain, DmCommandBuilder commandBuilder)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "getQuoteSuffix");
			try
			{
				return (string)(logRecord.ReturnValue = chain.getQuoteSuffix(commandBuilder));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setQuoteSuffix(FilterChain chain, DmCommandBuilder commandBuilder, string value)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "setQuoteSuffix", value);
			try
			{
				chain.setQuoteSuffix(commandBuilder, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string getQuotePrefix(FilterChain chain, DmCommandBuilder commandBuilder)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "getQuotePrefix");
			try
			{
				return (string)(logRecord.ReturnValue = chain.getQuotePrefix(commandBuilder));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setQuotePrefix(FilterChain chain, DmCommandBuilder commandBuilder, string value)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "setQuotePrefix", value);
			try
			{
				chain.setQuotePrefix(commandBuilder, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string getCatalogSeparator(FilterChain chain, DmCommandBuilder commandBuilder)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "getCatalogSeparator");
			try
			{
				return (string)(logRecord.ReturnValue = chain.getCatalogSeparator(commandBuilder));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setCatalogSeparator(FilterChain chain, DmCommandBuilder commandBuilder, string value)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "setCatalogSeparator", value);
			try
			{
				chain.setCatalogSeparator(commandBuilder, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public CatalogLocation getCatalogLocation(FilterChain chain, DmCommandBuilder commandBuilder)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "getCatalogLocation");
			try
			{
				CatalogLocation catalogLocation = chain.getCatalogLocation(commandBuilder);
				logRecord.ReturnValue = catalogLocation;
				return catalogLocation;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setCatalogLocation(FilterChain chain, DmCommandBuilder commandBuilder, CatalogLocation value)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "setCatalogLocation", value);
			try
			{
				chain.setCatalogLocation(commandBuilder, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public ConflictOption getConflictOption(FilterChain chain, DmCommandBuilder commandBuilder)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "getConflictOption");
			try
			{
				ConflictOption conflictOption = chain.getConflictOption(commandBuilder);
				logRecord.ReturnValue = conflictOption;
				return conflictOption;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setConflictOption(FilterChain chain, DmCommandBuilder commandBuilder, ConflictOption value)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "setConflictOption", value);
			try
			{
				chain.setConflictOption(commandBuilder, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string getSchemaSeparator(FilterChain chain, DmCommandBuilder commandBuilder)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "getSchemaSeparator");
			try
			{
				return (string)(logRecord.ReturnValue = chain.getSchemaSeparator(commandBuilder));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setSchemaSeparator(FilterChain chain, DmCommandBuilder commandBuilder, string value)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "setSchemaSeparator", value);
			try
			{
				chain.setSchemaSeparator(commandBuilder, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string QuoteIdentifier(FilterChain chain, DmCommandBuilder commandBuilder, string unquotedIdentifier)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "QuoteIdentifier", unquotedIdentifier);
			try
			{
				return (string)(logRecord.ReturnValue = chain.QuoteIdentifier(commandBuilder, unquotedIdentifier));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void RefreshSchema(FilterChain chain, DmCommandBuilder commandBuilder)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "RefreshSchema");
			try
			{
				chain.RefreshSchema(commandBuilder);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string UnquoteIdentifier(FilterChain chain, DmCommandBuilder commandBuilder, string quotedIdentifier)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "UnquoteIdentifier", quotedIdentifier);
			try
			{
				return (string)(logRecord.ReturnValue = chain.UnquoteIdentifier(commandBuilder, quotedIdentifier));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void ApplyParameterInfo(FilterChain chain, DmCommandBuilder commandBuilder, DmParameter parameter, DataRow row, StatementType statementType, bool whereClause)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "ApplyParameterInfo", parameter, row, statementType, whereClause);
			try
			{
				chain.ApplyParameterInfo(commandBuilder, parameter, row, statementType, whereClause);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string GetParameterName(FilterChain chain, DmCommandBuilder commandBuilder, int parameterOrdinal)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "GetParameterName", parameterOrdinal);
			try
			{
				return (string)(logRecord.ReturnValue = chain.GetParameterName(commandBuilder, parameterOrdinal));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string GetParameterName(FilterChain chain, DmCommandBuilder commandBuilder, string parameterName)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "GetParameterName", parameterName);
			try
			{
				return (string)(logRecord.ReturnValue = chain.GetParameterName(commandBuilder, parameterName));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public string GetParameterPlaceholder(FilterChain chain, DmCommandBuilder commandBuilder, int parameterOrdinal)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "GetParameterPlaceholder", parameterOrdinal);
			try
			{
				return (string)(logRecord.ReturnValue = chain.GetParameterPlaceholder(commandBuilder, parameterOrdinal));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DataTable GetSchemaTable(FilterChain chain, DmCommandBuilder commandBuilder, DmCommand sourceCommand)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "GetSchemaTable", sourceCommand);
			try
			{
				return (DataTable)(logRecord.ReturnValue = chain.GetSchemaTable(commandBuilder, sourceCommand));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public DmCommand InitializeCommand(FilterChain chain, DmCommandBuilder commandBuilder, DmCommand command)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "InitializeCommand", command);
			try
			{
				return (DmCommand)(logRecord.ReturnValue = chain.InitializeCommand(commandBuilder, command));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void SetRowUpdatingHandler(FilterChain chain, DmCommandBuilder commandBuilder, DmDataAdapter adapter)
		{
			LogRecord logRecord = new LogRecord(commandBuilder, "SetRowUpdatingHandler", adapter);
			try
			{
				chain.SetRowUpdatingHandler(commandBuilder, adapter);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public object getThis(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword)
		{
			LogRecord logRecord = new LogRecord(connectionStringBuilder, "geThis");
			try
			{
				return logRecord.ReturnValue = chain.getThis(connectionStringBuilder, keyword);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void setThis(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword, object value)
		{
			LogRecord logRecord = new LogRecord(connectionStringBuilder, "setThis", value);
			try
			{
				chain.setThis(connectionStringBuilder, keyword, value);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool getIsFixedSize(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder)
		{
			LogRecord logRecord = new LogRecord(connectionStringBuilder, "getIsFixedSize");
			try
			{
				bool isFixedSize = chain.getIsFixedSize(connectionStringBuilder);
				logRecord.ReturnValue = isFixedSize;
				return isFixedSize;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public int getCount(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder)
		{
			LogRecord logRecord = new LogRecord(connectionStringBuilder, "getCount");
			try
			{
				int count = chain.getCount(connectionStringBuilder);
				logRecord.ReturnValue = count;
				return count;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public ICollection getKeys(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder)
		{
			LogRecord logRecord = new LogRecord(connectionStringBuilder, "getKeys");
			try
			{
				return (ICollection)(logRecord.ReturnValue = chain.getKeys(connectionStringBuilder));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public ICollection getValues(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder)
		{
			LogRecord logRecord = new LogRecord(connectionStringBuilder, "getValues");
			try
			{
				return (ICollection)(logRecord.ReturnValue = chain.getValues(connectionStringBuilder));
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void Clear(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder)
		{
			LogRecord logRecord = new LogRecord(connectionStringBuilder, "Clear");
			try
			{
				chain.Clear(connectionStringBuilder);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool ContainsKey(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword)
		{
			LogRecord logRecord = new LogRecord(connectionStringBuilder, "ContainsKey", keyword);
			try
			{
				bool flag = chain.ContainsKey(connectionStringBuilder, keyword);
				logRecord.ReturnValue = flag;
				return flag;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool EquivalentTo(FilterChain chain, DmConnectionStringBuilder sourceConnectionStringBuilder, DmConnectionStringBuilder destConnectionStringBuilder)
		{
			LogRecord logRecord = new LogRecord(sourceConnectionStringBuilder, "EquivalentTo", destConnectionStringBuilder);
			try
			{
				bool flag = chain.EquivalentTo(sourceConnectionStringBuilder, destConnectionStringBuilder);
				logRecord.ReturnValue = flag;
				return flag;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool Remove(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword)
		{
			LogRecord logRecord = new LogRecord(connectionStringBuilder, "Remove", keyword);
			try
			{
				bool flag = chain.Remove(connectionStringBuilder, keyword);
				logRecord.ReturnValue = flag;
				return flag;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool ShouldSerialize(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword)
		{
			LogRecord logRecord = new LogRecord(connectionStringBuilder, "ShouldSerialize", keyword);
			try
			{
				bool flag = chain.ShouldSerialize(connectionStringBuilder, keyword);
				logRecord.ReturnValue = flag;
				return flag;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public bool TryGetValue(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, string keyword, out object value)
		{
			LogRecord logRecord = new LogRecord(connectionStringBuilder, "TryGetValue", keyword);
			try
			{
				bool flag = chain.TryGetValue(connectionStringBuilder, keyword, out value);
				logRecord.ReturnValue = flag;
				return flag;
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}

		public void GetProperties(FilterChain chain, DmConnectionStringBuilder connectionStringBuilder, Hashtable propertyDescriptors)
		{
			LogRecord logRecord = new LogRecord(connectionStringBuilder, "GetProperties", propertyDescriptors);
			try
			{
				chain.GetProperties(connectionStringBuilder, propertyDescriptors);
			}
			catch (Exception ex)
			{
				Exception ex3 = (logRecord.Throwable = ex);
				throw (ex3 is DmException) ? ((DmException)ex3) : new DmException(new DmError(DmErrorDefinition.ERROR_FOR_LOG));
			}
			finally
			{
				doLog(logRecord);
			}
		}
	}
}
