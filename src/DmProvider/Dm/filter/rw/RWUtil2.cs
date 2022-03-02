using System;
using System.Data;
using System.Data.Common;
using Dm.Config;
using Dm.filter.log;
using Dm.util;

namespace Dm.filter.rw
{
	internal class RWUtil2
	{
		private static ILogger log = LogFactory.getLog(typeof(RWUtil2));

		internal const string SQL_SELECT_STANDBY = "select distinct mailIni.inst_name, mailIni.INST_IP, mailIni.INST_PORT, archIni.arch_status from  v$arch_status archIni left join (select * from V$DM_MAL_INI) mailIni on archIni.arch_dest = mailIni.inst_name left join V$MAL_LINK_STATUS on CTL_LINK_STATUS  = 'CONNECTED' AND DATA_LINK_STATUS = 'CONNECTED' where archIni.arch_type in ('TIMELY', 'REALTIME') AND  archIni.arch_status = 'VALID'";

		internal const string SQL_SELECT_STANDBY2 = "select distinct mailIni.mal_inst_name, mailIni.mal_INST_HOST, mailIni.mal_INST_PORT, archIni.arch_status from v$arch_status archIni left join (select * from V$DM_MAL_INI) mailIni on archIni.arch_dest = mailIni.mal_inst_name left join V$MAL_LINK_STATUS on CTL_LINK_STATUS  = 'CONNECTED' AND DATA_LINK_STATUS = 'CONNECTED' where archIni.arch_type in ('TIMELY', 'REALTIME') AND  archIni.arch_status = 'VALID'";

		public static void reconnect(DmConnection connection)
		{
			if (connection.RWInfo != null)
			{
				removeStandby(connection);
				connection.Reconnect();
				connection.RWInfo.cleanup();
				connection.RWInfo.rwCounter = RWCounter.getInstance(connection);
				connectStandby(connection);
			}
		}

		public static void recoverStandby(DmConnection connection)
		{
			if (connection.do_State != 0 && !isStandbyAlive(connection))
			{
				DateTime now = DateTime.Now;
				int rwStandbyRecoverTime = connection.ConnProperty.RwStandbyRecoverTime;
				if (rwStandbyRecoverTime > 0 && !((now - connection.RWInfo.tryRecoverTs).TotalMilliseconds < (double)rwStandbyRecoverTime))
				{
					connectStandby(connection);
					connection.RWInfo.tryRecoverTs = now;
				}
			}
		}

		internal static void connectStandby(DmConnection connection)
		{
			EP eP = chooseValidStandby(connection);
			if (eP == null)
			{
				return;
			}
			try
			{
				DmConnection dmConnection = new DmConnection(connection.ConnectionString);
				dmConnection.ConnProperty.Server = eP.host;
				dmConnection.ConnProperty.Port = eP.port;
				dmConnection.ConnProperty.RWStandby = true;
				dmConnection.ConnProperty.EPGroup = null;
				dmConnection.ConnProperty.LoginPrimary = LoginModeFlag.onlystandby;
				dmConnection.ConnProperty.SwitchTimes = 0;
				dmConnection.Connect();
				connection.RWInfo.connStandby = dmConnection;
				if (connection.RWInfo.connStandby.ConnProperty.SvrMode != 2 || connection.RWInfo.connStandby.ConnProperty.SvrStat != 4)
				{
					removeStandby(connection);
				}
			}
			catch (Exception ex)
			{
				log.Info(connection, "connStandby", ex.Message);
			}
		}

		private static EP chooseValidStandby(DmConnection connection)
		{
			DmDataReader dmDataReader = null;
			Exception ex = null;
			try
			{
				dmDataReader = DriverUtil.executeQuery(connection, "select distinct mailIni.mal_inst_name, mailIni.mal_INST_HOST, mailIni.mal_INST_PORT, archIni.arch_status from v$arch_status archIni left join (select * from V$DM_MAL_INI) mailIni on archIni.arch_dest = mailIni.mal_inst_name left join V$MAL_LINK_STATUS on CTL_LINK_STATUS  = 'CONNECTED' AND DATA_LINK_STATUS = 'CONNECTED' where archIni.arch_type in ('TIMELY', 'REALTIME') AND  archIni.arch_status = 'VALID'", null);
			}
			catch (DbException)
			{
				dmDataReader.do_Close();
				try
				{
					dmDataReader = DriverUtil.executeQuery(connection, "select distinct mailIni.inst_name, mailIni.INST_IP, mailIni.INST_PORT, archIni.arch_status from  v$arch_status archIni left join (select * from V$DM_MAL_INI) mailIni on archIni.arch_dest = mailIni.inst_name left join V$MAL_LINK_STATUS on CTL_LINK_STATUS  = 'CONNECTED' AND DATA_LINK_STATUS = 'CONNECTED' where archIni.arch_type in ('TIMELY', 'REALTIME') AND  archIni.arch_status = 'VALID'", null);
				}
				catch (Exception ex2)
				{
					ex = ex2;
				}
			}
			catch (Exception ex4)
			{
				ex = ex4;
			}
			try
			{
				int num = (int)((dmDataReader != null) ? dmDataReader.RowCount : 0);
				if (num > 0)
				{
					int num2 = 0;
					int num3 = connection.RWInfo.rwCounter.Random(num);
					while (dmDataReader.do_Read())
					{
						if (num2 == num3)
						{
							return new EP(dmDataReader.do_GetString(1), dmDataReader.do_GetInt32(2));
						}
						num2++;
					}
				}
			}
			catch (Exception ex5)
			{
				ex = ex5;
			}
			finally
			{
				dmDataReader.do_Close();
			}
			if (ex != null)
			{
				throw new InvalidOperationException("choose valid standby error!", ex);
			}
			return null;
		}

		public static void afterExceptionOnStandby(DmConnection connection, DbException e)
		{
			if (e is DmException && ((DmException)e).Number == DmErrorDefinition.ECNET_COMMUNITION_ERROR)
			{
				removeStandby(connection);
			}
		}

		private static void removeStandby(DmConnection connection)
		{
			try
			{
				if (connection.RWInfo.connStandby != null)
				{
					connection.RWInfo.connStandby.do_Close();
					connection.RWInfo.connStandby = null;
				}
			}
			catch (Exception)
			{
			}
		}

		public static bool isCreateStandbyStmt(DmCommand stmt)
		{
			if (stmt != null && stmt.RWInfo.readOnly && isStandbyAlive(stmt.do_DbConnection))
			{
				return true;
			}
			return false;
		}

		public static T execute<T>(DmCommand cmd, execute<T> exec, closeDmDataReader<T> closeDmDataReader, executeByOther<T> execOther)
		{
			if (cmd.RWInfo.cmdCurrent == null)
			{
				cmd.RWInfo.cmdCurrent = cmd;
				cmd.RWInfo.readOnly = checkReadonly(cmd, null);
				try
				{
					if (isCreateStandbyStmt(cmd))
					{
						cmd.RWInfo.cmdStandby = cmd.do_DbConnection.RWInfo.connStandby.do_CreateDbCommand();
					}
				}
				catch (DbException e)
				{
					afterExceptionOnStandby(cmd.do_DbConnection, e);
				}
			}
			DmCommand cmdCurrent = cmd.RWInfo.cmdCurrent;
			string commandText = cmd.GetCommandText();
			recoverStandby(cmd.do_DbConnection);
			distribute(cmd, commandText);
			if (cmdCurrent != cmd.RWInfo.cmdCurrent)
			{
				cmd.RWInfo.cmdCurrent.ResetSqlAndParameters(cmdCurrent);
			}
			T val = default(T);
			bool flag = false;
			try
			{
				val = exec();
				DmCommand cmdCurrent2 = cmd.RWInfo.cmdCurrent;
				switch (cmdCurrent2.RetCmdType)
				{
				case 147:
				case 148:
				case 151:
				case 153:
				case 165:
				case 166:
					try
					{
						DmCommand dmCommand = ((cmdCurrent2 != cmd) ? cmd : cmd.RWInfo.cmdStandby);
						if (dmCommand != null)
						{
							dmCommand.ResetSqlAndParameters(cmdCurrent2);
							execOther(dmCommand);
						}
					}
					catch (Exception)
					{
					}
					break;
				case 162:
				{
					string text = (StringUtil.isNotEmpty(commandText) ? commandText.Trim() : (StringUtil.isNotEmpty(cmd.GetCommandText()) ? cmd.GetCommandText().Trim() : "")).Split(new char[1] { '(' }, 2, StringSplitOptions.RemoveEmptyEntries)[0];
					if (!text.Equals("SP_SET_PARA_VALUE", StringComparison.OrdinalIgnoreCase) && !text.Equals("SP_SET_SESSION_READONLY", StringComparison.OrdinalIgnoreCase))
					{
						break;
					}
					try
					{
						DmCommand dmCommand2 = ((cmdCurrent2 != cmd) ? cmd : cmd.RWInfo.cmdStandby);
						if (dmCommand2 != null)
						{
							dmCommand2.ResetSqlAndParameters(cmdCurrent2);
							execOther(dmCommand2);
						}
					}
					catch (Exception)
					{
					}
					break;
				}
				case 160:
					if (cmd.do_DbConnection.ConnProperty.RWHA && cmdCurrent2 == cmd.RWInfo.cmdStandby && cmdCurrent2.CurResultSetCache.RowsNum == 0L)
					{
						flag = true;
					}
					break;
				}
			}
			catch (DbException ex3)
			{
				if (cmd.RWInfo.cmdCurrent != cmd.RWInfo.cmdStandby)
				{
					throw ex3;
				}
				afterExceptionOnStandby(cmd.do_DbConnection, ex3);
				flag = true;
			}
			try
			{
				if (flag)
				{
					closeDmDataReader(val);
					cmd.do_DbConnection.RWInfo.toPrimary();
					cmd.RWInfo.cmdCurrent = cmd;
					cmd.ResetSqlAndParameters(cmd.RWInfo.cmdStandby);
					return execOther(cmd);
				}
				return val;
			}
			catch (DbException ex4)
			{
				throw ex4;
			}
		}

		public static bool checkReadonly(DmCommand cmd, string sql)
		{
			bool result = true;
			if (StringUtil.isNotEmpty(sql))
			{
				string text = sql.Trim().Split(new char[1] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries)[0];
				result = text.Equals("SELECT", StringComparison.OrdinalIgnoreCase) || ((!text.Equals("INSERT", StringComparison.OrdinalIgnoreCase) && !text.Equals("UPDATE", StringComparison.OrdinalIgnoreCase) && !text.Equals("DELETE", StringComparison.OrdinalIgnoreCase) && !text.Equals("CREATE", StringComparison.OrdinalIgnoreCase) && !text.Equals("TRUNCATE", StringComparison.OrdinalIgnoreCase) && !text.Equals("DROP", StringComparison.OrdinalIgnoreCase) && !text.Equals("ALTER", StringComparison.OrdinalIgnoreCase)) ? true : false);
			}
			return result;
		}

		public static RWSite distribute(DmCommand cmd, string sql)
		{
			RWSite rWSite = ((!isStandbyAlive(cmd.do_DbConnection)) ? cmd.do_DbConnection.RWInfo.toPrimary() : ((!checkReadonly(cmd, sql)) ? cmd.do_DbConnection.RWInfo.toPrimary() : ((cmd.do_DbTransaction != null && cmd.do_DbTransaction.Valid) ? cmd.do_DbConnection.RWInfo.distribute : ((cmd.do_DbTransaction == null || cmd.do_DbTransaction.do_IsolationLevel != IsolationLevel.Serializable) ? cmd.do_DbConnection.RWInfo.toAny() : cmd.do_DbConnection.RWInfo.toPrimary()))));
			if (rWSite == RWSite.STANDBY && !isStandbyCmdValid(cmd))
			{
				try
				{
					cmd.RWInfo.cmdStandby = new DmCommand("", cmd.do_DbConnection.RWInfo.connStandby);
				}
				catch (Exception)
				{
					rWSite = cmd.do_DbConnection.RWInfo.toPrimary();
				}
			}
			cmd.RWInfo.cmdCurrent = ((rWSite == RWSite.PRIMARY) ? cmd : cmd.RWInfo.cmdStandby);
			return rWSite;
		}

		public static bool isStandbyAlive(DmConnection connection)
		{
			if (connection.RWInfo != null && connection.RWInfo.connStandby != null)
			{
				return connection.RWInfo.connStandby.do_State != ConnectionState.Closed;
			}
			return false;
		}

		public static bool isStandbyCmdValid(DmCommand cmd)
		{
			return cmd.RWInfo.cmdStandby != null;
		}
	}
}
