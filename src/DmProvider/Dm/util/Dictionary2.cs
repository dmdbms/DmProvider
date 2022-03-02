using System.Collections.Generic;

namespace Dm.util
{
	internal class Dictionary2<TKey1, TKey2, TValue>
	{
		private Dictionary<TKey1, Dictionary<TKey2, TValue>> map1 = new Dictionary<TKey1, Dictionary<TKey2, TValue>>(8);

		internal void Add(TKey1 key1, TKey2 key2, TValue value)
		{
			Dictionary<TKey2, TValue> dictionary = null;
			if (map1.ContainsKey(key1))
			{
				dictionary = map1[key1];
			}
			else
			{
				dictionary = new Dictionary<TKey2, TValue>(8);
				map1[key1] = dictionary;
			}
			dictionary[key2] = value;
		}

		internal bool ContainsKey(TKey1 key1, TKey2 key2)
		{
			if (!map1.ContainsKey(key1))
			{
				return false;
			}
			if (!map1[key1].ContainsKey(key2))
			{
				return false;
			}
			return true;
		}

		internal bool Remove(TKey1 key1, TKey2 key2)
		{
			if (!map1.ContainsKey(key1))
			{
				return false;
			}
			Dictionary<TKey2, TValue> dictionary = map1[key1];
			if (!dictionary.ContainsKey(key2))
			{
				return false;
			}
			return dictionary.Remove(key2);
		}

		public bool TryGetValue(TKey1 key1, TKey2 key2, out TValue value)
		{
			if (!map1.ContainsKey(key1))
			{
				value = default(TValue);
				return false;
			}
			Dictionary<TKey2, TValue> dictionary = map1[key1];
			if (!dictionary.ContainsKey(key2))
			{
				value = default(TValue);
				return false;
			}
			return dictionary.TryGetValue(key2, out value);
		}
	}
}
