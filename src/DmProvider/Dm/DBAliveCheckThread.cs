using System;
using System.Collections.Generic;
using System.Threading;
using Dm.Config;
using Dm.filter.log;
using Dm.util;

namespace Dm
{
	internal class DBAliveCheckThread
	{
		private static object obj = new object();

		private static DBAliveCheckThread _dbAliveCheckThread;

		private BlockingQueue<DmConnInstance> _queue = new BlockingQueue<DmConnInstance>();

		private readonly ILogger log = LogFactory.getLog(typeof(DBAliveCheckThread));

		internal static DBAliveCheckThread CheckThread
		{
			get
			{
				if (_dbAliveCheckThread == null)
				{
					lock (obj)
					{
						if (_dbAliveCheckThread == null)
						{
							_dbAliveCheckThread = new DBAliveCheckThread();
						}
					}
				}
				return _dbAliveCheckThread;
			}
		}

		private DBAliveCheckThread()
		{
			Thread thread = new Thread(new ThreadStart(run));
			thread.Name = "DB-ALIVE-CHECK-THREAD";
			thread.IsBackground = true;
			thread.Start();
		}

		public void AddConnInstance(DmConnInstance connInstance)
		{
			if (DmSvcConfig.dbAliveCheckFreq > 0)
			{
				_queue.Enqueue(connInstance);
			}
		}

		private void run()
		{
			Dictionary<string, bool> dictionary = null;
			while (true)
			{
				try
				{
					dictionary = new Dictionary<string, bool>(8);
					DmConnInstance dmConnInstance = null;
					List<DmConnInstance> list = _queue.Dequeue(_queue.Count);
					for (int i = 0; i < list.Count; i++)
					{
						dmConnInstance = list[i];
						if (dmConnInstance == null || !dmConnInstance.AliveCheck)
						{
							continue;
						}
						try
						{
							string key = dmConnInstance.ConnProperty.Server + ":" + dmConnInstance.ConnProperty.Port;
							if (!dictionary.TryGetValue(key, out var value))
							{
								value = (dictionary[key] = checkDbAlive(dmConnInstance.ConnProperty.Server, dmConnInstance.ConnProperty.Port));
							}
							if (!value)
							{
								dmConnInstance.GetCsi().CloseForDbAliveCheck();
								continue;
							}
						}
						catch (Exception)
						{
						}
						if (dmConnInstance.AliveCheck)
						{
							_queue.Enqueue(dmConnInstance);
						}
					}
				}
				catch (Exception)
				{
				}
				try
				{
					Thread.Sleep(DmSvcConfig.dbAliveCheckFreq);
				}
				catch (Exception)
				{
				}
			}
		}

		private bool checkDbAlive(string host, int port)
		{
			bool result = false;
			DmCommTcpip dmCommTcpip = null;
			try
			{
				dmCommTcpip = new DmCommTcpip(host, port, DmSvcConfig.dbAliveCheckTimeout);
				result = true;
				return result;
			}
			catch (Exception ex)
			{
				log.Info("checkDbAlive failed: " + ex.Message);
				return result;
			}
			finally
			{
				try
				{
					dmCommTcpip.Close();
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
