using System;
using System.Text;

namespace Dm.filter.log
{
	internal class LogRecord
	{
		internal class NullData
		{
		}

		public static readonly NullData NULL = new NullData();

		private string source;

		private string method;

		private object[] @params;

		private object returnValue;

		private Exception e;

		private int usedTime;

		private string sql;

		public virtual Exception Throwable
		{
			get
			{
				return e;
			}
			set
			{
				e = value;
			}
		}

		public virtual object ReturnValue
		{
			get
			{
				return returnValue;
			}
			set
			{
				if (value == null)
				{
					returnValue = NULL;
				}
				else
				{
					returnValue = value;
				}
			}
		}

		public virtual int UsedTime
		{
			get
			{
				return usedTime;
			}
			set
			{
				usedTime = value;
			}
		}

		public virtual string Sql
		{
			get
			{
				return sql;
			}
			set
			{
				sql = value;
			}
		}

		public LogRecord(object source, string method, params object[] @params)
		{
			this.source = Logger.FormatSource(source);
			this.method = method;
			this.@params = @params;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			stringBuilder.Append(Logger.FormatTrace(source, method, returnValue, @params));
			if (sql != null && usedTime > 0)
			{
				stringBuilder.Append(formatSql(sql));
				stringBuilder.Append(formatUsedTime(usedTime));
			}
			return stringBuilder.ToString();
		}

		private string formatUsedTime(int nanosecond)
		{
			if ((double)nanosecond < Math.Pow(10.0, 6.0))
			{
				return " [USED TIME]: " + nanosecond + "ns;";
			}
			if ((double)nanosecond < Math.Pow(10.0, 9.0))
			{
				return " [USED TIME]: " + (double)nanosecond / Math.Pow(10.0, 6.0) + "ms;";
			}
			return " [USED TIME]: " + (double)nanosecond / Math.Pow(10.0, 9.0) + "s;";
		}

		private string formatSql(string sql)
		{
			return "[SQL]: " + sql;
		}
	}
}
