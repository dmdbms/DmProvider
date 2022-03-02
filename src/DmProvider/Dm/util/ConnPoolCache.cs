using System.Collections.Generic;

namespace Dm.util
{
	internal class ConnPoolCache
	{
		private Dictionary<string, BlockingQueue<DmConnInstance>> _connPoolMap = new Dictionary<string, BlockingQueue<DmConnInstance>>();

		private int _limit;

		internal int Limit
		{
			set
			{
				_limit = value;
			}
		}

		internal ConnPoolCache()
		{
		}

		internal DmConnInstance Get(string key)
		{
			if (_connPoolMap.ContainsKey(key))
			{
				return _connPoolMap[key].Dequeue();
			}
			return null;
		}

		internal bool Put(DmConnInstance connInstance, string key)
		{
			if (!_connPoolMap.ContainsKey(key))
			{
				lock (_connPoolMap)
				{
					if (!_connPoolMap.ContainsKey(key))
					{
						_connPoolMap[key] = new BlockingQueue<DmConnInstance>(_limit);
					}
				}
			}
			_connPoolMap[key].Enqueue(connInstance, out var succ);
			return succ;
		}
	}
}
