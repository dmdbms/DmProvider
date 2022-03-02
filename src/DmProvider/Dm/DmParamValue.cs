using System;

namespace Dm
{
	internal class DmParamValue
	{
		private bool m_InDataBound;

		private bool m_OutDataBound;

		private bool m_IsInDataNull;

		private bool m_IsOutDataNull;

		internal byte[] m_InValue;

		private byte[] m_OutValue;

		private bool m_HasRegisterType;

		private int m_RegisterType;

		private bool m_HasRegisterScale;

		private int m_RegisterScale;

		private int m_RowSetType;

		private int m_Length;

		private int m_SqlType;

		private int m_Prec;

		private int m_Scale;

		public bool GetInDataBound()
		{
			return m_InDataBound;
		}

		public void SetInDataBound(bool inDataBound)
		{
			m_InDataBound = inDataBound;
		}

		public bool GetOutDataBound()
		{
			return m_OutDataBound;
		}

		public void SetOutDataBound(bool outDataBound)
		{
			m_OutDataBound = outDataBound;
		}

		public bool GetIsInDataNull()
		{
			if (GetInDataBound())
			{
				return m_IsInDataNull;
			}
			DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_INPUT_PARAMETER_VALUE);
			return false;
		}

		public void SetInNull()
		{
			m_IsInDataNull = true;
			m_InValue = new byte[0];
			m_InDataBound = true;
		}

		public bool GetIsOutDataNull()
		{
			if (GetOutDataBound())
			{
				return m_IsOutDataNull;
			}
			DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_OUTPUT_PARAMETER_VALUE);
			return false;
		}

		public void SetOutNull()
		{
			m_IsOutDataNull = true;
			m_OutDataBound = true;
			m_OutValue = new byte[0];
		}

		public byte[] GetInValue()
		{
			if (GetInDataBound())
			{
				return m_InValue;
			}
			DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_INPUT_PARAMETER_VALUE);
			return new byte[0];
		}

		public void GetInValue(ref byte[] InValue)
		{
			if (GetInDataBound())
			{
				InValue = m_InValue;
			}
			else
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_INPUT_PARAMETER_VALUE);
			}
		}

		public void SetInValue(byte[] inValue)
		{
			if (inValue == null)
			{
				SetInNull();
				return;
			}
			m_InValue = inValue;
			m_Length = inValue.Length;
			m_IsInDataNull = false;
			m_InDataBound = true;
		}

		public void SetInValue(ref byte[] inValue)
		{
			m_InValue = inValue;
			m_Length = m_InValue.Length;
			m_IsInDataNull = false;
			m_InDataBound = true;
		}

		public void SetInValue()
		{
			m_Length = m_InValue.Length;
			m_IsInDataNull = false;
			m_InDataBound = true;
		}

		public byte[] GetOutValue()
		{
			if (GetOutDataBound())
			{
				return m_OutValue;
			}
			DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_OUTPUT_PARAMETER_VALUE);
			return new byte[0];
		}

		public void SetOutValue(byte[] outValue)
		{
			m_OutValue = outValue;
			m_OutDataBound = true;
			m_IsOutDataNull = false;
		}

		public void ClearInParam()
		{
			m_InValue = null;
			m_InDataBound = false;
			m_IsInDataNull = false;
		}

		public void ClearOutParam()
		{
			m_OutValue = null;
			m_OutDataBound = false;
			m_IsOutDataNull = false;
		}

		public void SetRegisterParamType(int type)
		{
			m_RegisterType = type;
			m_HasRegisterType = true;
		}

		public int GetRegisterParamType()
		{
			return m_RegisterType;
		}

		public bool HasRegisterType()
		{
			return m_HasRegisterType;
		}

		public void SetRegisterParamScale(int scale)
		{
			m_RegisterScale = scale;
			m_HasRegisterScale = true;
		}

		public int GetRegisterParamScale()
		{
			return m_RegisterScale;
		}

		public bool HasRegisterScale()
		{
			return m_HasRegisterScale;
		}

		public int GetStreamLen()
		{
			return m_Length;
		}

		public int GetBytes(ref byte[] val, int val_off, int off, int len)
		{
			if (len > 32000)
			{
				return 0;
			}
			if (off >= m_Length)
			{
				return 0;
			}
			int num = ((off + len <= m_Length) ? len : (m_Length - off));
			Array.Copy(m_InValue, off, val, val_off, num);
			return num;
		}

		public void SetRowsetType(int type)
		{
			m_RowSetType = type;
		}

		public int GetRowSetType()
		{
			return m_RowSetType;
		}

		public void SetSqlType(int sqlType)
		{
			m_SqlType = sqlType;
		}

		public int GetSqlType()
		{
			return m_SqlType;
		}

		public void SetPrec(int prec)
		{
			m_Prec = prec;
		}

		public int GetPrec()
		{
			return m_Prec;
		}

		public void SetScale(int scale)
		{
			m_Scale = scale;
		}

		public int GetScale()
		{
			return m_Scale;
		}
	}
}
