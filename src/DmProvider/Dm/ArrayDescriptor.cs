using Dm.util;

namespace Dm
{
	internal class ArrayDescriptor
	{
		internal TypeDescriptor m_typeDesc;

		internal ArrayDescriptor(string fulName, DmConnection conn)
		{
			if (StringUtil.isEmpty(fulName))
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_COMPLEX_TYPE_NAME);
			}
			m_typeDesc = new TypeDescriptor(fulName, conn);
			m_typeDesc.ParseDescByName();
		}

		internal ArrayDescriptor(TypeDescriptor desc)
		{
			m_typeDesc = desc;
		}

		internal TypeDescriptor GetMDesc()
		{
			return m_typeDesc;
		}

		internal TypeDescriptor GetItemDesc()
		{
			return m_typeDesc.m_arrObj;
		}

		internal int GetLength()
		{
			return m_typeDesc.m_length;
		}
	}
}
