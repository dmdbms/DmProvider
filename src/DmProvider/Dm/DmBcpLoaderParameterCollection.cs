using System;
using System.Collections;
using System.Collections.Generic;

namespace Dm
{
	public sealed class DmBcpLoaderParameterCollection : IList, ICollection, IEnumerable
	{
		private List<DmBcpLoaderParameter> _items;

		private static Type ItemType = typeof(DmBcpLoaderParameter);

		public int Count
		{
			get
			{
				if (_items == null)
				{
					return 0;
				}
				return _items.Count;
			}
		}

		private List<DmBcpLoaderParameter> InnerList
		{
			get
			{
				List<DmBcpLoaderParameter> list = _items;
				if (list == null)
				{
					list = (_items = new List<DmBcpLoaderParameter>());
				}
				return list;
			}
		}

		public bool IsFixedSize => ((IList)InnerList).IsFixedSize;

		public bool IsReadOnly => ((IList)InnerList).IsReadOnly;

		public bool IsSynchronized => ((ICollection)InnerList).IsSynchronized;

		public DmBcpLoaderParameter this[int index]
		{
			get
			{
				return InnerList[index];
			}
			set
			{
				InnerList[index] = value;
			}
		}

		public object SyncRoot => ((ICollection)InnerList).SyncRoot;

		object IList.this[int index]
		{
			get
			{
				return InnerList[index];
			}
			set
			{
				InnerList[index] = (DmBcpLoaderParameter)value;
			}
		}

		internal DmBcpLoaderParameterCollection()
		{
		}

		public DmBcpLoaderParameter Add(DmBcpLoaderParameter value)
		{
			InnerList.Add(value);
			return value;
		}

		public int Add(object value)
		{
			ValidateType(value);
			Add((DmBcpLoaderParameter)value);
			return Count - 1;
		}

		public void AddRange(DmBcpLoaderParameter[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			DmBcpLoaderParameter[] array = values;
			foreach (object value in array)
			{
				ValidateType(value);
			}
			array = values;
			foreach (DmBcpLoaderParameter item in array)
			{
				InnerList.Add(item);
			}
		}

		public void AddRange(Array values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			foreach (object value2 in values)
			{
				ValidateType(value2);
			}
			foreach (DmBcpLoaderParameter value3 in values)
			{
				Add(value3);
			}
		}

		public void Clear()
		{
			InnerList?.Clear();
		}

		public bool Contains(DmBcpLoaderParameter value)
		{
			return -1 != IndexOf(value);
		}

		public bool Contains(object value)
		{
			return -1 != IndexOf(value);
		}

		public void CopyTo(DmBcpLoaderParameter[] array, int index)
		{
			InnerList.CopyTo(array, index);
		}

		public void CopyTo(Array array, int index)
		{
			CopyTo(array, index);
		}

		public IEnumerator GetEnumerator()
		{
			return InnerList.GetEnumerator();
		}

		public int IndexOf(DmBcpLoaderParameter value)
		{
			List<DmBcpLoaderParameter> innerList = InnerList;
			if (innerList != null)
			{
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					if (value == innerList[i])
					{
						return i;
					}
				}
			}
			return -1;
		}

		public int IndexOf(object value)
		{
			if (value != null)
			{
				ValidateType(value);
				return IndexOf((DmBcpLoaderParameter)value);
			}
			return -1;
		}

		public void Insert(int index, DmBcpLoaderParameter value)
		{
			InnerList.Insert(index, value);
		}

		public void Insert(int index, object value)
		{
			ValidateType(value);
			Insert(index, (DmBcpLoaderParameter)value);
		}

		private void RangeCheck(int index)
		{
			if (index < 0 || Count <= index)
			{
				throw new IndexOutOfRangeException("index");
			}
		}

		public void Remove(DmBcpLoaderParameter value)
		{
			int num = IndexOf(value);
			if (-1 != num)
			{
				RemoveIndex(num);
			}
		}

		public void Remove(object value)
		{
			ValidateType(value);
			Remove((DmBcpLoaderParameter)value);
		}

		public void RemoveAt(int index)
		{
			RangeCheck(index);
			RemoveIndex(index);
		}

		private void RemoveIndex(int index)
		{
			List<DmBcpLoaderParameter> innerList = InnerList;
			_ = innerList[index];
			innerList.RemoveAt(index);
		}

		private void Replace(int index, object newValue)
		{
			List<DmBcpLoaderParameter> innerList = InnerList;
			ValidateType(newValue);
			_ = innerList[index];
			innerList[index] = (DmBcpLoaderParameter)newValue;
		}

		private void ValidateType(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!ItemType.IsInstanceOfType(value))
			{
				DmError.ThrowDmException(DmErrorDefinition.EC_FAIL);
			}
		}
	}
}
