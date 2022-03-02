using System;
using System.Data;

namespace Dm
{
	internal class DmStruct : TypeData
	{
		internal StructDescriptor m_strctDesc;

		internal TypeData[] m_attribs;

		internal int m_objCount;

		internal int m_strCount;

		public DmStruct(TypeData[] atData, TypeDescriptor desc)
			: base(null, null)
		{
			m_strctDesc = new StructDescriptor(desc);
			m_attribs = atData;
		}

		public TypeData[] getAttribsTypeData()
		{
			return m_attribs;
		}

		public DmStruct(StructDescriptor desc, DmConnection conn, object[] objArr)
			: base(null, null)
		{
			if (desc == null)
			{
				throw new InvalidOperationException("DmStruct");
			}
			if (conn.do_State == ConnectionState.Closed)
			{
				throw new InvalidOperationException("connection has closed");
			}
			m_strctDesc = desc;
			if (objArr == null)
			{
				m_attribs = new TypeData[desc.GetSize()];
				return;
			}
			if (desc.GetSize() != objArr.Length && desc.GetObjId() != 4)
			{
				throw new InvalidOperationException("DmStruct");
			}
			m_attribs = TypeData.toStruct(objArr, m_strctDesc.m_typeDesc);
		}
	}
}
