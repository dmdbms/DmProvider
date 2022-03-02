using System;
using Dm.util;

namespace Dm
{
	internal class EP
	{
		internal string host;

		internal int port;

		internal bool alive;

		internal DateTime statusRefreshTs;

		internal int serverMode = -1;

		internal int serverStatus = -1;

		internal bool dscControl;

		internal int sort = int.MaxValue;

		internal static TimeSpan STATUS_VALID_TIME = new TimeSpan(0, 0, 20);

		internal const int SORT_SERVER_MODE_INVALID = -1;

		internal const int SORT_SERVER_NOT_ALIVE = -2;

		internal const int SORT_UNKNOWN = int.MaxValue;

		internal const int SORT_NORMAL = 30;

		internal const int SORT_PRIMARY = 20;

		internal const int SORT_STANDBY = 10;

		internal const int SORT_OPEN = 3;

		internal const int SORT_MOUNT = 2;

		internal const int SORT_SUSPEND = 1;

		internal int epSeqno;

		internal const int EP_STATUS_OK = 1;

		internal const int EP_STATUS_ERROR = 2;

		internal int epStatus;

		internal int getSort(bool checkTime)
		{
			if (checkTime)
			{
				if (!(DateTime.Now - statusRefreshTs < STATUS_VALID_TIME))
				{
					return int.MaxValue;
				}
				return sort;
			}
			return sort;
		}

		private int calcSort(int loginMode)
		{
			int num = 0;
			switch (loginMode)
			{
			case 0:
				switch (serverMode)
				{
				case 0:
					num += 300;
					break;
				case 1:
					num += 2000;
					break;
				case 2:
					num += 10;
					break;
				}
				break;
			case 3:
				switch (serverMode)
				{
				case 0:
					num += 30;
					break;
				case 1:
					num += 200;
					break;
				case 2:
					num += 1000;
					break;
				}
				break;
			case 1:
				if (serverMode != 1)
				{
					return -1;
				}
				num += 20;
				break;
			case 2:
				if (serverMode != 2)
				{
					return -1;
				}
				num += 10;
				break;
			}
			switch (serverStatus)
			{
			case 3:
				num += 2;
				break;
			case 4:
				num += 3;
				break;
			case 5:
				num++;
				break;
			}
			return num;
		}

		public EP(string host, int port)
		{
			this.host = host;
			this.port = port;
		}

		private void refreshStatus(bool alive, DmConnection conn)
		{
			this.alive = alive;
			statusRefreshTs = DateTime.Now;
			serverMode = (alive ? conn.ConnProperty.SvrMode : (-1));
			serverStatus = (alive ? conn.ConnProperty.SvrStat : (-1));
			dscControl = alive && conn.ConnProperty.DscControl;
			sort = (alive ? calcSort((int)conn.ConnProperty.LoginPrimary.Value) : (-2));
		}

		public void connect(DmConnection conn)
		{
			conn.ConnProperty.Server = host;
			conn.ConnProperty.Port = port;
			try
			{
				conn.do_Open();
				conn.ConnProperty.ServerActual = conn.ConnProperty.Server;
				conn.ConnProperty.PortActual = conn.ConnProperty.Port;
				refreshStatus(alive: true, conn);
			}
			catch (Exception ex)
			{
				refreshStatus(alive: false, conn);
				throw ex;
			}
		}

		public static string getServerStatusDesc(int serverStatus)
		{
			string text = "";
			return serverStatus switch
			{
				4 => "OPEN", 
				3 => "MOUNT", 
				5 => "SUSPEND", 
				_ => "UNKNOW", 
			};
		}

		public static string getServerModeDesc(int serverMode)
		{
			string text = "";
			return serverMode switch
			{
				0 => "NORMAL", 
				1 => "PRIMARY", 
				2 => "STANDBY", 
				_ => "UNKNOW", 
			};
		}

		public override string ToString()
		{
			return StringUtil.trimToEmpty(host) + ":" + port + " (" + getServerModeDesc(serverMode) + ", " + getServerStatusDesc(serverStatus) + (dscControl ? ", DSC CONTROL)" : ")");
		}
	}
}
