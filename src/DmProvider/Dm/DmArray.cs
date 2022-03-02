using System;
using System.Data;

namespace Dm
{
	internal class DmArray : TypeData
	{
		internal ArrayDescriptor m_arrDesc;

		internal TypeData[] m_arrData;

		internal object m_objArray;

		internal int m_itemCount;

		internal int m_itemSize;

		internal int m_objCount;

		internal int m_strCount;

		internal int[] m_objStrOffs;

		private void initData()
		{
			m_itemCount = 0;
			m_itemSize = 0;
			m_objCount = 0;
			m_strCount = 0;
			m_objStrOffs = null;
			m_dumyData = null;
			m_offset = 0;
			m_objArray = null;
		}

		public DmArray(TypeData[] atData, TypeDescriptor desc)
			: base(null, null)
		{
			m_arrDesc = new ArrayDescriptor(desc);
			m_arrData = atData;
		}

		public DmArray(ArrayDescriptor arrDesc, DmConnection conn, Array objArr)
			: base(null, null)
		{
			if (arrDesc == null)
			{
				throw new InvalidOperationException("DmArray");
			}
			if (conn.do_State == ConnectionState.Closed)
			{
				throw new InvalidOperationException("connection has closed");
			}
			initData();
			m_arrDesc = arrDesc;
			if (objArr == null)
			{
				m_arrData = new TypeData[0];
			}
			else
			{
				if (arrDesc.GetMDesc() == null || (arrDesc.GetMDesc().GetDType() == 122 && objArr.Length > arrDesc.GetMDesc().GetStaticArrayLength()))
				{
					throw new InvalidOperationException("DmArray");
				}
				m_arrData = TypeData.toArray(objArr, m_arrDesc.GetMDesc());
			}
			m_itemCount = m_arrData.Length;
		}
	}
}
