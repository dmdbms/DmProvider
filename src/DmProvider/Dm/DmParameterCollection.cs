using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using Dm.filter;
using Dm.filter.log;
using Dm.filter.reconnect;
using Dm.filter.rw;

namespace Dm
{
	public sealed class DmParameterCollection : DbParameterCollection, IFilterInfo
	{
		internal long id = -1L;

		internal static long idGenerator = 0L;

		private static readonly string ClassName = "DmParameterCollection";

		private List<DmParameter> InternalList = new List<DmParameter>();

		private Dictionary<string, int> parameterNameDictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

		public long ID
		{
			get
			{
				if (id < 0)
				{
					id = Interlocked.Increment(ref idGenerator);
				}
				return id;
			}
		}

		public FilterChain FilterChain { get; set; }

		public LogInfo LogInfo { get; set; }

		public RWInfo RWInfo { get; set; }

		public RecoverInfo RecoverInfo { get; set; }

		internal object do_SyncRoot => ((ICollection)InternalList).SyncRoot;

		internal bool do_IsSynchronized => ((ICollection)InternalList).IsSynchronized;

		internal bool do_IsReadOnly => ((IList)InternalList).IsReadOnly;

		internal bool do_IsFixedSize => ((IList)InternalList).IsFixedSize;

		internal int do_Count => InternalList.Count;

		public override object SyncRoot
		{
			get
			{
				if (FilterChain == null)
				{
					return do_SyncRoot;
				}
				return FilterChain.reset().getSyncRoot(this);
			}
		}

		public override bool IsSynchronized
		{
			get
			{
				if (FilterChain == null)
				{
					return do_IsSynchronized;
				}
				return FilterChain.reset().getIsSynchronized(this);
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				if (FilterChain == null)
				{
					return do_IsReadOnly;
				}
				return FilterChain.reset().getIsReadOnly(this);
			}
		}

		public override bool IsFixedSize
		{
			get
			{
				if (FilterChain == null)
				{
					return do_IsFixedSize;
				}
				return FilterChain.reset().getIsFixedSize(this);
			}
		}

		public override int Count
		{
			get
			{
				if (FilterChain == null)
				{
					return do_Count;
				}
				return FilterChain.reset().getCount(this);
			}
		}

		internal DmCommand Command { get; set; }

		public new DbParameter this[int index]
		{
			get
			{
				return do_GetParameter(index);
			}
			set
			{
				do_SetParameter(index, (DmParameter)value);
			}
		}

		public new DbParameter this[string parameterName]
		{
			get
			{
				return do_GetParameter(parameterName);
			}
			set
			{
				do_SetParameter(parameterName, (DmParameter)value);
			}
		}

		internal DmParameterCollection()
		{
			do_Clear();
		}

		internal int do_Add(object value)
		{
			DmParameter dmParameter = ConvertType(value);
			if (string.IsNullOrEmpty(dmParameter.do_ParameterName) || dmParameter.do_ParameterName.Equals("?") || dmParameter.do_ParameterName.Equals(":?") || dmParameter.do_ParameterName.Equals("@?"))
			{
				dmParameter.do_ParameterName = $"Parameter{GetNextIndex()}";
			}
			int num = do_IndexOf(dmParameter.do_ParameterName);
			if (num >= 0)
			{
				do_SetParameter(do_IndexOf(dmParameter.do_ParameterName), dmParameter);
			}
			else
			{
				InternalList.Add(dmParameter);
				num = InternalList.Count - 1;
				parameterNameDictionary.Add(GetBasename(dmParameter.do_ParameterName), num);
				dmParameter.parameterCollection = this;
			}
			return num;
		}

		internal void do_AddRange(Array values)
		{
			if (values == null)
			{
				return;
			}
			foreach (object value in values)
			{
				do_Add(value);
			}
		}

		internal void do_Clear()
		{
			foreach (DmParameter @internal in InternalList)
			{
				@internal.parameterCollection = null;
			}
			InternalList.Clear();
			parameterNameDictionary.Clear();
		}

		internal bool do_Contains(string value)
		{
			CheckArgument(value);
			return parameterNameDictionary.ContainsKey(GetBasename(value));
		}

		internal bool do_Contains(object value)
		{
			CheckArgument(value);
			return InternalList.Contains(ConvertType(value));
		}

		internal void do_CopyTo(Array array, int index)
		{
			InternalList.ToArray().CopyTo(array, index);
		}

		internal IEnumerator do_GetEnumerator()
		{
			return InternalList.GetEnumerator();
		}

		internal int do_IndexOf(object value)
		{
			return InternalList.IndexOf(ConvertType(value));
		}

		internal int do_IndexOf(string parameterName)
		{
			CheckArgument(parameterName);
			int value = -1;
			if (parameterNameDictionary.TryGetValue(GetBasename(parameterName), out value))
			{
				return value;
			}
			return -1;
		}

		internal void do_Insert(int index, object value)
		{
			if (index != do_Count)
			{
				CheckIndex(index);
			}
			DmParameter dmParameter = ConvertType(value);
			if (string.IsNullOrEmpty(dmParameter.do_ParameterName))
			{
				dmParameter.do_ParameterName = $"Parameter{GetNextIndex()}";
			}
			int num = index;
			for (int i = 0; i < do_Count; i++)
			{
				if (i >= index)
				{
					num = (parameterNameDictionary[GetBasename(do_GetParameter(i).do_ParameterName)] = num + 1);
				}
			}
			dmParameter.parameterCollection = this;
			parameterNameDictionary[GetBasename(dmParameter.do_ParameterName)] = index;
			InternalList.Insert(index, dmParameter);
		}

		internal void do_RemoveAt(int index)
		{
			DmParameter dmParameter = do_GetParameter(CheckIndex(index));
			int num = index;
			for (int i = 0; i < do_Count; i++)
			{
				if (i > index)
				{
					parameterNameDictionary[GetBasename(do_GetParameter(i).do_ParameterName)] = num++;
				}
			}
			dmParameter.parameterCollection = null;
			parameterNameDictionary.Remove(GetBasename(dmParameter.do_ParameterName));
			InternalList.RemoveAt(index);
		}

		internal void do_Remove(object value)
		{
			do_RemoveAt(do_IndexOf(value));
		}

		internal void do_RemoveAt(string parameterName)
		{
			do_RemoveAt(do_IndexOf(parameterName));
		}

		internal DmParameter do_GetParameter(int index)
		{
			return InternalList[CheckIndex(index)];
		}

		internal DmParameter do_GetParameter(string parameterName)
		{
			int value = -1;
			CheckArgument(parameterName);
			if (parameterNameDictionary.TryGetValue(GetBasename(parameterName), out value))
			{
				return do_GetParameter(value);
			}
			throw new ArgumentException("Parameter '" + parameterName + "' not found in the collection.");
		}

		internal void do_SetParameter(int index, DmParameter value)
		{
			CheckIndex(index);
			DmParameter dmParameter = ConvertType(value);
			if (string.IsNullOrEmpty(dmParameter.do_ParameterName))
			{
				dmParameter.do_ParameterName = $"Parameter{GetNextIndex()}";
			}
			DmParameter dmParameter2 = do_GetParameter(index);
			InternalList[index] = dmParameter;
			parameterNameDictionary.Remove(GetBasename(dmParameter2.do_ParameterName));
			parameterNameDictionary.Add(GetBasename(dmParameter.do_ParameterName), index);
			dmParameter.parameterCollection = this;
		}

		internal void do_SetParameter(string parameterName, DmParameter value)
		{
			do_SetParameter(do_IndexOf(parameterName), value);
		}

		public override int Add(object value)
		{
			if (FilterChain == null)
			{
				return do_Add(value);
			}
			return FilterChain.reset().Add(this, value);
		}

		public override void AddRange(Array values)
		{
			if (FilterChain == null)
			{
				do_AddRange(values);
			}
			else
			{
				FilterChain.reset().AddRange(this, values);
			}
		}

		public override void Clear()
		{
			if (FilterChain == null)
			{
				do_Clear();
			}
			else
			{
				FilterChain.reset().Clear(this);
			}
		}

		public override bool Contains(string value)
		{
			if (FilterChain == null)
			{
				return do_Contains(value);
			}
			return FilterChain.reset().Contains(this, value);
		}

		public override bool Contains(object value)
		{
			if (FilterChain == null)
			{
				return do_Contains(value);
			}
			return FilterChain.reset().Contains(this, value);
		}

		public override void CopyTo(Array array, int index)
		{
			if (FilterChain == null)
			{
				do_CopyTo(array, index);
			}
			else
			{
				FilterChain.reset().CopyTo(this, array, index);
			}
		}

		public override IEnumerator GetEnumerator()
		{
			if (FilterChain == null)
			{
				return do_GetEnumerator();
			}
			return FilterChain.reset().GetEnumerator(this);
		}

		public override int IndexOf(object value)
		{
			if (FilterChain == null)
			{
				return do_IndexOf(value);
			}
			return FilterChain.reset().IndexOf(this, value);
		}

		public override int IndexOf(string parameterName)
		{
			if (FilterChain == null)
			{
				return do_IndexOf(parameterName);
			}
			return FilterChain.reset().IndexOf(this, parameterName);
		}

		public override void Insert(int index, object value)
		{
			if (FilterChain == null)
			{
				do_Insert(index, value);
			}
			else
			{
				FilterChain.reset().Insert(this, index, value);
			}
		}

		public override void RemoveAt(int index)
		{
			if (FilterChain == null)
			{
				do_RemoveAt(index);
			}
			else
			{
				FilterChain.reset().RemoveAt(this, index);
			}
		}

		public override void Remove(object value)
		{
			if (FilterChain == null)
			{
				do_Remove(value);
			}
			else
			{
				FilterChain.reset().Remove(this, value);
			}
		}

		public override void RemoveAt(string parameterName)
		{
			if (FilterChain == null)
			{
				do_RemoveAt(parameterName);
			}
			else
			{
				FilterChain.reset().RemoveAt(this, parameterName);
			}
		}

		protected override DbParameter GetParameter(int index)
		{
			if (FilterChain == null)
			{
				return do_GetParameter(index);
			}
			return FilterChain.reset().GetParameter(this, index);
		}

		protected override DbParameter GetParameter(string parameterName)
		{
			if (FilterChain == null)
			{
				return do_GetParameter(parameterName);
			}
			return FilterChain.reset().GetParameter(this, parameterName);
		}

		protected override void SetParameter(int index, DbParameter value)
		{
			if (FilterChain == null)
			{
				do_SetParameter(index, (DmParameter)value);
			}
			else
			{
				FilterChain.reset().SetParameter(this, index, (DmParameter)value);
			}
		}

		protected override void SetParameter(string parameterName, DbParameter value)
		{
			if (FilterChain == null)
			{
				do_SetParameter(parameterName, (DmParameter)value);
			}
			else
			{
				FilterChain.reset().SetParameter(this, parameterName, (DmParameter)value);
			}
		}

		private int CheckIndex(int index)
		{
			if (index < 0 || index > InternalList.Count - 1)
			{
				throw new IndexOutOfRangeException("Parameter Index Out Of Range");
			}
			return index;
		}

		private object CheckArgument(object value)
		{
			if (value == null)
			{
				throw new ArgumentException();
			}
			return value;
		}

		private DmParameter ConvertType(object value)
		{
			DmParameter obj = CheckArgument(value) as DmParameter;
			if (obj == null)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_ONLY_DMPARAMETER);
			}
			return obj;
		}

		private int GetNextIndex()
		{
			int num = do_Count + 1;
			while (true)
			{
				string key = "parameter" + num;
				if (!parameterNameDictionary.ContainsKey(key))
				{
					break;
				}
				num++;
			}
			return num;
		}

		private string GetBasename(string name)
		{
			if (!string.IsNullOrEmpty(name) && (name[0] == ':' || name[0] == '@'))
			{
				return name.Substring(1);
			}
			return name;
		}

		public void ChangeName(DmParameter parameter, string oldname, string newname)
		{
			CheckArgument(oldname);
			CheckArgument(newname);
			parameterNameDictionary.Remove(GetBasename(oldname));
			parameterNameDictionary.Add(GetBasename(newname), do_IndexOf(parameter));
		}

		private void CheckDmDbType(DmDbType dbtype)
		{
			if (dbtype > DmDbType.TimeOffset || dbtype < DmDbType.Blob)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_ENUM_VALUE);
			}
		}

		public DmParameter Add(string parameterName, object value)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "DmParameter Add(string parameterName, object value)");
			return InternalList[do_Add(new DmParameter(parameterName, value))];
		}

		public DmParameter Add(string parameterName, DmDbType parameterType)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "Add(string parameterName, DmDbType parameterType)");
			CheckDmDbType(parameterType);
			return InternalList[do_Add(new DmParameter(parameterName, parameterType))];
		}

		public DmParameter Add(string parameterName, DmDbType parameterType, int size)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "Add(string parameterName, DmDbType parameterType, int size)");
			CheckDmDbType(parameterType);
			return InternalList[do_Add(new DmParameter(parameterName, parameterType, size))];
		}

		public DmParameter Add(string parameterName, DmDbType parameterType, int size, string sourceColumn)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "Add(string parameterName, DmDbType parameterType, int size, string sourceColumn)");
			CheckDmDbType(parameterType);
			return InternalList[do_Add(new DmParameter(parameterName, parameterType, size, sourceColumn))];
		}
	}
}
