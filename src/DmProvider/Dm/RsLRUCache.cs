using System.Collections.Generic;

namespace Dm
{
	internal class RsLRUCache
	{
		private LinkedList<KeyValuePair<RsKey, DmResultSetCache>> list;

		private Dictionary<RsKey, LinkedListNode<KeyValuePair<RsKey, DmResultSetCache>>> map;

		private int capacity;

		private int count;

		internal RsLRUCache(int capacity)
		{
			this.capacity = capacity;
			list = new LinkedList<KeyValuePair<RsKey, DmResultSetCache>>();
			map = new Dictionary<RsKey, LinkedListNode<KeyValuePair<RsKey, DmResultSetCache>>>(64);
		}

		internal bool Add(RsKey key, DmResultSetCache value)
		{
			if (value.BytesCount > capacity)
			{
				return false;
			}
			lock (map)
			{
				if (map.TryGetValue(key, out var value2))
				{
					list.Remove(value2);
					count -= value2.Value.Value.BytesCount;
				}
				while (count + value.BytesCount > capacity)
				{
					LinkedListNode<KeyValuePair<RsKey, DmResultSetCache>> last = list.Last;
					list.Remove(last);
					count -= last.Value.Value.BytesCount;
					map.Remove(last.Value.Key);
				}
				LinkedListNode<KeyValuePair<RsKey, DmResultSetCache>> linkedListNode = new LinkedListNode<KeyValuePair<RsKey, DmResultSetCache>>(new KeyValuePair<RsKey, DmResultSetCache>(key, value));
				list.AddFirst(linkedListNode);
				count += linkedListNode.Value.Value.BytesCount;
				map[key] = linkedListNode;
				return true;
			}
		}

		internal DmResultSetCache Find(RsKey key)
		{
			lock (map)
			{
				if (!map.TryGetValue(key, out var value))
				{
					return null;
				}
				list.Remove(value);
				list.AddFirst(value);
				return value.Value.Value;
			}
		}
	}
}
