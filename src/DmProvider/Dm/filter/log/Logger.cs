using System;
using System.Text;
using System.Threading;
using Dm.Config;
using Dm.util;

namespace Dm.filter.log
{
	internal class Logger : ILogger
	{
		public bool ErrorEnabled => DmSvcConfig.logLevel >= LogLevel.ERROR;

		public bool InfoEnabled => DmSvcConfig.logLevel >= LogLevel.INFO;

		public bool SqlEnabled => DmSvcConfig.logLevel >= LogLevel.SQL;

		public Logger(string name)
		{
		}

		public void Info(string message)
		{
			try
			{
				if (InfoEnabled)
				{
					Println(FormatHead("INFO ") + message.ToString());
				}
			}
			catch (Exception)
			{
			}
		}

		public void Info(object source, string method, string info)
		{
			try
			{
				if (InfoEnabled)
				{
					Println(FormatHead("INFO ") + FormatTrace(source, method, info));
				}
			}
			catch (Exception)
			{
			}
		}

		public void Info(object source, string method, params object[] @params)
		{
			try
			{
				if (InfoEnabled)
				{
					Println(FormatHead("INFO ") + FormatTrace(source, method, @params));
				}
			}
			catch (Exception)
			{
			}
		}

		public void Sql(string message)
		{
			try
			{
				if (SqlEnabled)
				{
					Println(FormatHead("SQL  ") + message);
				}
			}
			catch (Exception)
			{
			}
		}

		public void Error(string message)
		{
			Error(message, null);
		}

		public void Error(string message, Exception t)
		{
			try
			{
				if (ErrorEnabled)
				{
					Println(FormatHead("ERROR") + message + StringUtil.LINE_SEPARATOR + GetStackTrace(t));
				}
			}
			catch (Exception)
			{
			}
		}

		private string FormatHead(string head)
		{
			return "[" + head + " - " + StringUtil.formatTime() + "] tid:" + Thread.CurrentThread.ManagedThreadId + " (IsBackground-" + Thread.CurrentThread.IsBackground + ") ";
		}

		private void Println(string msg)
		{
			LogWriter.Instance.WriteLine(StringUtil.trimToEmpty(msg));
		}

		internal static string GetStackTrace(Exception t)
		{
			if (t == null)
			{
				return "";
			}
			return t.StackTrace + StringUtil.LINE_SEPARATOR;
		}

		internal static string FormatTrace(object source, string method, string info)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (source != null)
			{
				stringBuilder.Append("{ " + FormatSource(source) + " } ");
			}
			stringBuilder.Append(method).Append("();  ");
			stringBuilder.Append(info);
			return stringBuilder.ToString();
		}

		internal static string FormatTrace(object source, string method, params object[] @params)
		{
			return FormatTrace(source, method, null, @params);
		}

		internal static string FormatTrace(object source, string method, object returnValue, params object[] @params)
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			if (source != null)
			{
				stringBuilder.Append("{ " + FormatSource(source) + " } ");
			}
			stringBuilder.Append(method + "(");
			int num = 0;
			if (@params != null && @params.Length > num)
			{
				for (int i = num; i < @params.Length; i++)
				{
					if (i != num)
					{
						stringBuilder.Append(", ");
					}
					if (@params[i] != null)
					{
						stringBuilder.Append(@params[i].GetType().Name);
					}
					else
					{
						stringBuilder.Append("null");
					}
				}
			}
			stringBuilder.Append(")");
			if (returnValue != null)
			{
				stringBuilder.Append(" [RETURN]: " + FormatReturn(returnValue));
			}
			stringBuilder.Append(";  ");
			if (@params != null && @params.Length > num)
			{
				stringBuilder.Append(" [PARAMS]: ");
				for (int j = num; j < @params.Length; j++)
				{
					if (j != 0)
					{
						stringBuilder.Append(", ");
					}
					if (@params[j] is string)
					{
						stringBuilder.Append("\"").Append(@params[j]).Append("\"");
					}
					else if (@params[j] is DmConnection)
					{
						stringBuilder.Append("conn" + ((DmConnection)@params[j]).ID);
					}
					else if (@params[j] is DmCommand)
					{
						stringBuilder.Append("command" + ((DmCommand)@params[j]).ID);
					}
					else if (@params[j] is DmDataReader)
					{
						stringBuilder.Append("dataReader" + ((DmDataReader)@params[j]).ID);
					}
					else if (@params[j] is DmTransaction)
					{
						stringBuilder.Append("transaction" + ((DmTransaction)@params[j]).ID);
					}
					else if (@params[j] is DmParameterCollection)
					{
						stringBuilder.Append("parameterCollection" + ((DmParameterCollection)@params[j]).ID);
					}
					else if (@params[j] is DmParameter)
					{
						stringBuilder.Append("paramter" + ((DmParameter)@params[j]).ID);
					}
					else if (@params[j] is DmDataAdapter)
					{
						stringBuilder.Append("dataAdapter" + ((DmDataAdapter)@params[j]).ID);
					}
					else
					{
						stringBuilder.Append(@params[j]);
					}
				}
				stringBuilder.Append("; ");
			}
			return stringBuilder.ToString();
		}

		internal static string FormatSource(object source)
		{
			if (source == null)
			{
				return StringUtil.EMPTY;
			}
			StringBuilder stringBuilder = new StringBuilder(128);
			if (source is string)
			{
				stringBuilder.Append(source);
			}
			else if (source is DmConnection)
			{
				stringBuilder.Append(FormatConn((DmConnection)source));
			}
			else if (source is DmCommand)
			{
				DmCommand dmCommand = (DmCommand)source;
				if (dmCommand.do_DbConnection != null)
				{
					stringBuilder.Append(FormatConn(dmCommand.do_DbConnection)).Append(", ");
				}
				stringBuilder.Append(FormatCommand(dmCommand));
			}
			else if (source is DmTransaction)
			{
				DmTransaction dmTransaction = (DmTransaction)source;
				if (dmTransaction.do_DbConnection != null)
				{
					stringBuilder.Append(FormatConn(dmTransaction.do_DbConnection)).Append(", ");
				}
				stringBuilder.Append(FormatTransaction(dmTransaction));
			}
			else if (source is DmParameter)
			{
				DmParameter dmParameter = (DmParameter)source;
				if (dmParameter.parameterCollection != null)
				{
					if (dmParameter.parameterCollection.Command != null)
					{
						if (dmParameter.parameterCollection.Command.do_DbConnection != null)
						{
							stringBuilder.Append(FormatConn(dmParameter.parameterCollection.Command.do_DbConnection)).Append(", ");
						}
						stringBuilder.Append(FormatCommand(dmParameter.parameterCollection.Command)).Append(", ");
					}
					stringBuilder.Append(FormatParameterCollection(dmParameter.parameterCollection)).Append(", ");
				}
				stringBuilder.Append(FormatParameter(dmParameter));
			}
			else if (source is DmParameterCollection)
			{
				DmParameterCollection dmParameterCollection = (DmParameterCollection)source;
				if (dmParameterCollection.Command != null)
				{
					if (dmParameterCollection.Command.do_DbConnection != null)
					{
						stringBuilder.Append(FormatConn(dmParameterCollection.Command.do_DbConnection)).Append(", ");
					}
					stringBuilder.Append(FormatCommand(dmParameterCollection.Command)).Append(", ");
				}
				stringBuilder.Append(FormatParameterCollection(dmParameterCollection));
			}
			else if (source is DmDataReader)
			{
				DmDataReader dmDataReader = (DmDataReader)source;
				if (dmDataReader.m_Statement != null && dmDataReader.m_Statement.Command != null)
				{
					if (dmDataReader.m_Statement.Command.do_DbConnection != null)
					{
						stringBuilder.Append(FormatConn(dmDataReader.m_Statement.Command.do_DbConnection)).Append(", ");
					}
					stringBuilder.Append(FormatCommand(dmDataReader.m_Statement.Command)).Append(", ");
				}
				stringBuilder.Append(FormatDataReader((DmDataReader)source));
			}
			else if (source is DmDataAdapter)
			{
				stringBuilder.Append(FormatDataAdapter((DmDataAdapter)source));
			}
			else
			{
				stringBuilder.Append(source.GetType().Name + "@" + source.GetHashCode().ToString("x"));
			}
			return stringBuilder.ToString();
		}

		internal static string FormatReturn(object returnObject)
		{
			if (returnObject == null)
			{
				return StringUtil.EMPTY;
			}
			if (returnObject is DmConnection)
			{
				return FormatConn((DmConnection)returnObject);
			}
			if (returnObject is DmCommand)
			{
				return FormatCommand((DmCommand)returnObject);
			}
			if (returnObject is DmTransaction)
			{
				return FormatTransaction((DmTransaction)returnObject);
			}
			if (returnObject is DmParameter)
			{
				return FormatParameter((DmParameter)returnObject);
			}
			if (returnObject is DmParameterCollection)
			{
				return FormatParameterCollection((DmParameterCollection)returnObject);
			}
			if (returnObject is DmDataReader)
			{
				return FormatDataReader((DmDataReader)returnObject);
			}
			if (returnObject is DmDataAdapter)
			{
				return FormatDataAdapter((DmDataAdapter)returnObject);
			}
			return returnObject.ToString();
		}

		internal static string FormatConn(DmConnection connection)
		{
			if (connection != null && connection.LogInfo != null)
			{
				string text = "conn-" + connection.ID;
				if (connection.m_ConnInst != null && connection.m_ConnInst.ConnProperty != null && connection.m_ConnInst.ConnProperty.SessId != -1)
				{
					text = text + " (sessId:" + connection.m_ConnInst.ConnProperty.SessId + ")";
				}
				return text;
			}
			return "conn-null";
		}

		internal static string FormatCommand(DmCommand command)
		{
			if (command != null && command.LogInfo != null)
			{
				return "command-" + command.ID;
			}
			return "command-null";
		}

		internal static string FormatTransaction(DmTransaction transaction)
		{
			if (transaction != null && transaction.LogInfo != null)
			{
				return "transaction-" + transaction.ID;
			}
			return "transaction-null";
		}

		internal static string FormatParameterCollection(DmParameterCollection parameterCollection)
		{
			if (parameterCollection != null && parameterCollection.LogInfo != null)
			{
				return "parameterCollection-" + parameterCollection.ID;
			}
			return "parameterCollection-null";
		}

		internal static string FormatParameter(DmParameter parameter)
		{
			if (parameter != null && parameter.LogInfo != null)
			{
				return "parameter-" + parameter.ID;
			}
			return "parameter-null";
		}

		internal static string FormatDataReader(DmDataReader dateReader)
		{
			if (dateReader != null && dateReader.LogInfo != null)
			{
				return "dateReader-" + dateReader.ID;
			}
			return "dateReader-null";
		}

		internal static string FormatDataAdapter(DmDataAdapter dataAdapter)
		{
			if (dataAdapter != null && dataAdapter.LogInfo != null)
			{
				return "dataAdapter-" + dataAdapter.ID;
			}
			return "dataAdapter-null";
		}
	}
}
