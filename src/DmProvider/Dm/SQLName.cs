using System.Data;
using Dm.util;

namespace Dm
{
	internal class SQLName
	{
		internal string m_name = "";

		internal string m_pkgName = "";

		internal string m_schName = "";

		internal string m_fulName = "";

		internal int m_schId = -1;

		internal int m_packId = -1;

		internal DmConnection m_conn;

		internal SQLName(string fulName)
		{
			m_fulName = fulName;
		}

		internal SQLName(DmConnection conn)
		{
			m_conn = conn;
		}

		internal string GetFulName()
		{
			if (m_fulName.Length > 0)
			{
				return m_fulName;
			}
			if (StringUtil.isEmpty(m_name))
			{
				return null;
			}
			if (m_packId != 0 || m_schId != 0)
			{
				string sql = "SELECT NAME INTO ? FROM SYS.SYSOBJECTS WHERE ID=?";
				DmParameter dmParameter = new DmParameter();
				dmParameter.do_Direction = ParameterDirection.Output;
				dmParameter.do_DbType = DbType.String;
				DmParameter dmParameter2 = new DmParameter();
				dmParameter2.do_Value = ((m_packId != 0) ? m_packId : m_schId);
				DriverUtil.executeNonQuery(m_conn, sql, new DmParameter[2] { dmParameter, dmParameter2 });
				if (m_packId != 0)
				{
					m_pkgName = (string)dmParameter.do_Value;
					m_fulName = m_pkgName + "." + m_name;
				}
				else
				{
					m_schName = (string)dmParameter.do_Value;
					m_fulName = m_schName + "." + m_name;
				}
			}
			if (m_fulName.Length > 0)
			{
				return m_fulName;
			}
			return m_conn.ConnProperty.CurrentSchema + "." + m_name;
		}
	}
}
