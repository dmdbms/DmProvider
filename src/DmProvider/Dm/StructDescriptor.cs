using Dm.util;

namespace Dm
{
	internal class StructDescriptor
	{
		internal TypeDescriptor m_typeDesc;

		internal StructDescriptor(string fulName, DmConnection conn)
		{
			if (StringUtil.isEmpty(fulName))
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_COMPLEX_TYPE_NAME);
			}
			m_typeDesc = new TypeDescriptor(fulName, conn);
			m_typeDesc.ParseDescByName();
		}

		internal static StructDescriptor CreateDescriptor(string fulName, DmConnection conn)
		{
			return new StructDescriptor(fulName, conn);
		}

		internal StructDescriptor(TypeDescriptor desc)
		{
			m_typeDesc = desc;
		}

		internal int GetSize()
		{
			return m_typeDesc.m_size;
		}

		internal int GetObjId()
		{
			return m_typeDesc.m_objId;
		}

		internal TypeDescriptor[] GetItemsDesc()
		{
			return m_typeDesc.m_fieldsObj;
		}
	}
}
