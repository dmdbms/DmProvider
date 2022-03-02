using System;
using System.Text;

namespace Dm
{
	public class DmError
	{
		private string m_Schema = string.Empty;

		private string m_Table = string.Empty;

		private string m_Col = string.Empty;

		private string m_ErrInfo = string.Empty;

		private int m_SqlCode;

		public string Schema
		{
			get
			{
				return m_Schema;
			}
			set
			{
				m_Schema = value;
			}
		}

		public string Table
		{
			get
			{
				return m_Table;
			}
			set
			{
				m_Table = value;
			}
		}

		public string Col
		{
			get
			{
				return m_Col;
			}
			set
			{
				m_Col = value;
			}
		}

		public string Message
		{
			get
			{
				return m_ErrInfo;
			}
			set
			{
				m_ErrInfo = value;
			}
		}

		public int State
		{
			get
			{
				return m_SqlCode;
			}
			set
			{
				m_SqlCode = value;
			}
		}

		internal DmError()
		{
		}

		internal DmError(int sqlcode)
		{
			m_SqlCode = sqlcode;
			m_ErrInfo = GetErrorInfoByErrorCode(sqlcode);
		}

		internal DmError(int sqlcode, string errInfo)
		{
			m_SqlCode = sqlcode;
			m_ErrInfo = errInfo;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("{0},{1}", m_SqlCode, m_ErrInfo);
			return stringBuilder.ToString();
		}

		public string ToStringOnlyInfo()
		{
			return m_ErrInfo.ToString();
		}

		private string GetSQLStateByErrorCode(int sqlcode)
		{
			for (int i = 0; i < SQLStateMapping.mappings.Length; i++)
			{
				if (sqlcode == SQLStateMapping.mappings[i].GetErr)
				{
					return SQLStateMapping.mappings[i].GetSQLState;
				}
			}
			for (int j = 0; j < SQLStateRange.ranges.Length; j++)
			{
				if (sqlcode >= SQLStateRange.ranges[j].GetLow && sqlcode <= SQLStateRange.ranges[j].GetHigh)
				{
					return SQLStateRange.ranges[j].GetSQLState;
				}
			}
			return "22000";
		}

		private string GetErrorInfoByErrorCode(int sqlcode)
		{
			for (int i = 0; i < SQLStateMapping.mappings.Length; i++)
			{
				if (sqlcode == SQLStateMapping.mappings[i].GetErr)
				{
					return SQLStateMapping.mappings[i].GetErrInfo;
				}
			}
			return "";
		}

		internal static void ThrowDmException(DmError err)
		{
			DmTrace.TracePrint("ex :" + err.State + " " + err.Message.ToString());
			DmTrace.TracePrintStack();
			throw new DmException(err);
		}

		internal static void ThrowDmException(string errInfo, int code)
		{
			DmError dmError = new DmError(code, errInfo);
			DmTrace.TracePrint("ex :" + dmError.State + " " + dmError.Message.ToString());
			DmTrace.TracePrintStack();
			throw new DmException(dmError.ToString(), dmError);
		}

		internal static void ThrowDmException(string errInfo)
		{
			DmError dmError = new DmError(-1, errInfo);
			DmTrace.TracePrint("ex :" + errInfo.ToString());
			DmTrace.TracePrintStack();
			throw new DmException(dmError.ToString(), dmError);
		}

		internal static void ThrowDmException(Exception ex)
		{
			DmTrace.TracePrint("ex :" + ex.Message);
			DmTrace.TracePrintStack();
			throw ex;
		}

		internal static void ThrowDmException(int code)
		{
			DmError dmError = new DmError(code);
			DmTrace.TracePrint("ex :" + dmError.State + " " + dmError.Message.ToString());
			DmTrace.TracePrintStack();
			throw new DmException(dmError.ToString(), dmError);
		}

		internal static void ThrowUnsupportedException()
		{
			DmError dmError = new DmError(DmErrorDefinition.ECNET_UNSUPPORED_INTERFACE);
			DmTrace.TracePrint("ex :" + dmError.State + " " + dmError.Message.ToString());
			DmTrace.TracePrintStack();
			throw new DmException(dmError.ToString(), dmError);
		}
	}
}
