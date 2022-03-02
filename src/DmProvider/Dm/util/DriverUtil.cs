using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Dm.filter.log;

namespace Dm.util
{
	internal static class DriverUtil
	{
		public static ILogger log = LogFactory.getLog(typeof(DriverUtil));

		internal const string SQL_GET_DSC_EP_SITE = "SELECT dsc.ep_seqno, (CASE mal.MAL_INST_HOST WHEN '' THEN mal.MAL_HOST ELSE mal.MAL_INST_HOST END) as ep_host, dcr.EP_PORT, dsc.EP_STATUS FROM V$DSC_EP_INFO dsc LEFT join V$DM_MAL_INI mal on dsc.EP_NAME = mal.MAL_INST_NAME LEFT join (SELECT grp.GROUP_TYPE GROUP_TYPE, ep.* FROM SYS.\"V$DCR_GROUP\" grp, SYS.\"V$DCR_EP\" ep where grp.GROUP_NAME = ep.GROUP_NAME) dcr on dsc.EP_NAME = dcr.EP_NAME and GROUP_TYPE = 'DB' order by  dsc.ep_seqno asc;";

		internal static bool isLocalHost(string host)
		{
			if (StringUtil.isEmpty(host))
			{
				return false;
			}
			if ("localhost".Equals(host, StringComparison.OrdinalIgnoreCase) || "127.0.0.1".Equals(host) || "::1".Equals(host))
			{
				return true;
			}
			return false;
		}

		internal static void executeNonQuery(DmConnection conn, string sql, DmParameter[] parameters)
		{
			DmCommand dmCommand = new DmCommand(sql, conn);
			dmCommand.do_DbParameterCollection.do_AddRange(parameters);
			dmCommand.do_ExecuteNonQuery();
			dmCommand.Close();
		}

		internal static DmDataReader executeQuery(DmConnection conn, string sql, DmParameter[] parameters)
		{
			DmCommand dmCommand = new DmCommand(sql, conn);
			dmCommand.do_DbParameterCollection.do_AddRange(parameters);
			DmDataReader result = dmCommand.do_ExecuteDbDataReader(CommandBehavior.Default);
			dmCommand.Close();
			return result;
		}

		internal static List<EP> loadDscEpSites(DmConnection connection)
		{
			try
			{
				List<EP> list = new List<EP>();
				DmDataReader dmDataReader = executeQuery(connection, "SELECT dsc.ep_seqno, (CASE mal.MAL_INST_HOST WHEN '' THEN mal.MAL_HOST ELSE mal.MAL_INST_HOST END) as ep_host, dcr.EP_PORT, dsc.EP_STATUS FROM V$DSC_EP_INFO dsc LEFT join V$DM_MAL_INI mal on dsc.EP_NAME = mal.MAL_INST_NAME LEFT join (SELECT grp.GROUP_TYPE GROUP_TYPE, ep.* FROM SYS.\"V$DCR_GROUP\" grp, SYS.\"V$DCR_EP\" ep where grp.GROUP_NAME = ep.GROUP_NAME) dcr on dsc.EP_NAME = dcr.EP_NAME and GROUP_TYPE = 'DB' order by  dsc.ep_seqno asc;", null);
				while (dmDataReader.do_Read())
				{
					EP eP = new EP(dmDataReader.do_GetString(1), dmDataReader.do_GetInt32(2));
					eP.epSeqno = dmDataReader.do_GetInt32(0);
					eP.epStatus = (dmDataReader.do_GetString(3).Equals("OK", StringComparison.OrdinalIgnoreCase) ? 1 : 2);
					list.Add(eP);
				}
				dmDataReader.do_Close();
				return list;
			}
			catch (DbException ex)
			{
				log.Info("Get ep sites failed!" + ex.Message);
			}
			return null;
		}
	}
}
