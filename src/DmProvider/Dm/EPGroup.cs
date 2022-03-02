using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dm.Config;
using Dm.filter.log;
using Dm.util;

namespace Dm
{
	internal class EPGroup
	{
		private readonly ILogger LOG = LogFactory.getLog(typeof(EPGroup));

		internal string name;

		internal List<EP> epList;

		private WellDistributeSelector wellSelection;

		private HeadFirstSelector headFirstSelection;

		internal const int CLUSTER_TYPE_NORMAL = 0;

		internal const int CLUSTER_TYPE_RW = 1;

		internal const int CLUSTER_TYPE_DW = 2;

		internal const int CLUSTER_TYPE_DSC = 3;

		internal const int CLUSTER_TYPE_MPP = 4;

		internal string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		internal EPGroup(List<EP> serverList)
		{
			epList = serverList;
			headFirstSelection = new HeadFirstSelector(serverList);
			wellSelection = new WellDistributeSelector(serverList);
		}

		public EPSelector getDbSelection(DmConnection conn)
		{
			if (conn.ConnProperty.EpSelector.Value == EpSelector.HEAD_FIRST)
			{
				return headFirstSelection;
			}
			return wellSelection;
		}

		public void connect(DmConnection conn)
		{
			EPSelector dbSelection = getDbSelection(conn);
			Exception ex = null;
			int num = ((epList.Count == 1) ? 1 : (conn.ConnProperty.SwitchTimes + 1));
			for (int i = 0; i < num; i++)
			{
				LOG.Info("try connect loop " + i);
				try
				{
					EP[] array = dbSelection.sortDBList(i == 0);
					traverseServerList(array, conn, i == 0, i == num - 1);
					return;
				}
				catch (Exception ex2)
				{
					ex = ex2;
					try
					{
						Thread.Sleep(conn.ConnProperty.SwitchInterval);
					}
					catch (Exception)
					{
					}
				}
			}
			if (ex != null)
			{
				DmError.ThrowDmException(ex);
			}
			else
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_COMMUNITION_ERROR);
			}
		}

		private void traverseServerList(EP[] epList, DmConnection conn, bool firstTime, bool lastTime)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Exception ex = null;
			foreach (EP eP in epList)
			{
				try
				{
					eP.connect(conn);
					if (!getDbSelection(conn).checkServerMode(lastTime, conn))
					{
						conn.do_Close();
						DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_SERVER_MODE);
					}
					LOG.Info("try connect success [" + conn.ConnProperty.Server + ":" + conn.ConnProperty.Port + "]");
					return;
				}
				catch (Exception ex2)
				{
					if (ex2 is DmException && ((DmException)ex2).Number == DmErrorDefinition.ECNET_INVALID_SERVER_MODE)
					{
						ex = ex2;
					}
					stringBuilder.Append("[").Append(eP.ToString()).Append("]")
						.Append(ex2.Message)
						.Append(StringUtil.LINE_SEPARATOR);
					LOG.Info("try connect fail [" + eP.ToString() + "] " + ex2.Message);
				}
			}
			if (ex != null)
			{
				DmError.ThrowDmException(stringBuilder.ToString(), DmErrorDefinition.ECNET_INVALID_SERVER_MODE);
			}
			else if (stringBuilder.Length > 0)
			{
				DmError.ThrowDmException(stringBuilder.ToString(), DmErrorDefinition.ECNET_COMMUNITION_ERROR);
			}
			DmError.ThrowDmException(DmErrorDefinition.ECNET_COMMUNITION_ERROR);
		}
	}
}
