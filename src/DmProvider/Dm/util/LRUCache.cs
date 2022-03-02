using System.Collections.Generic;

namespace Dm.util
{
	internal class LRUCache<TKey, TValue>
	{
		private LinkedList<KeyValuePair<TKey, TValue>> _linkedList;

		private Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> _map;

		private int _capacity;

		internal LRUCache(int capacity)
		{
			_capacity = capacity;
			_linkedList = new LinkedList<KeyValuePair<TKey, TValue>>();
			_map = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>(capacity);
		}

		internal TValue Add(TKey key, TValue value)
		{
			LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode = null;
			LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode2 = new LinkedListNode<KeyValuePair<TKey, TValue>>(new KeyValuePair<TKey, TValue>(key, value));
			if (_map.TryGetValue(key, out var value2))
			{
				_linkedList.Remove(value2);
			}
			if (_linkedList.Count == _capacity)
			{
				linkedListNode = _linkedList.Last;
				_linkedList.Remove(linkedListNode);
				_map.Remove(linkedListNode.Value.Key);
			}
			_linkedList.AddFirst(linkedListNode2);
			_map[key] = linkedListNode2;
			if (linkedListNode == null)
			{
				return default(TValue);
			}
			return linkedListNode.Value.Value;
		}

		internal TValue FindValue(TKey key)
		{
			if (!_map.TryGetValue(key, out var value))
			{
				return default(TValue);
			}
			_linkedList.Remove(value);
			_linkedList.AddFirst(value);
			return value.Value.Value;
		}
	}
}
