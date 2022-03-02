using System;
using System.Data;

namespace Dm
{
	public class DmBcpLoaderParameter
	{
		private DbType m_Dbtype;

		private string m_DestColumnName;

		private int m_DestColumnOrdinal;

		private object m_Data;

		public DbType DbType
		{
			get
			{
				return m_Dbtype;
			}
			set
			{
				m_Dbtype = value;
			}
		}

		public string DestColumnName
		{
			get
			{
				if (m_DestColumnName != null)
				{
					return m_DestColumnName;
				}
				return string.Empty;
			}
			set
			{
				m_DestColumnName = value;
			}
		}

		public int DestColumnOrdinal
		{
			get
			{
				return m_DestColumnOrdinal;
			}
			set
			{
				if (value < 0)
				{
					throw new IndexOutOfRangeException("DestColumnOrdinal");
				}
				m_DestColumnOrdinal = value;
			}
		}

		public object DataValue
		{
			get
			{
				return m_Data;
			}
			set
			{
				if (value == null)
				{
					throw new InvalidExpressionException("null error");
				}
				m_Data = value;
			}
		}

		public DmBcpLoaderParameter()
		{
			m_Dbtype = DbType.String;
			m_DestColumnName = "";
			m_DestColumnOrdinal = -1;
			m_Data = null;
		}

		public DmBcpLoaderParameter(DbType dbtype, string destColumnName, object data)
			: this()
		{
			m_Dbtype = dbtype;
			m_DestColumnName = destColumnName;
			m_Data = data;
		}

		public DmBcpLoaderParameter(DbType dbtype, int destColumnOrdinal, object data)
			: this()
		{
			m_Dbtype = dbtype;
			m_DestColumnOrdinal = destColumnOrdinal;
			m_Data = data;
		}
	}
}
