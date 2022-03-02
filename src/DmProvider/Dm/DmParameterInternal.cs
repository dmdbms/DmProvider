using System.Collections.Generic;
using System.Data;

namespace Dm
{
	internal sealed class DmParameterInternal : DmField
	{
		private List<DmParamValue> m_Val;

		public DmParameterInternal(DmConnInstance conn)
			: base(conn)
		{
			m_Val = new List<DmParamValue>();
			m_Val.Add(new DmParamValue());
		}

		public bool IsNullable()
		{
			return GetNullable();
		}

		public ParameterDirection GetParameterMode()
		{
			return GetInOutType() switch
			{
				0 => ParameterDirection.Input, 
				1 => ParameterDirection.Output, 
				2 => ParameterDirection.InputOutput, 
				_ => ParameterDirection.Input, 
			};
		}

		public bool GetInDataBound()
		{
			return m_Val[0].GetInDataBound();
		}

		public void SetInDataBound(bool inDataBound)
		{
			m_Val[0].SetInDataBound(inDataBound);
		}

		public bool GetOutDataBound()
		{
			return m_Val[0].GetOutDataBound();
		}

		public void SetOutDataBound(bool outDataBound)
		{
			m_Val[0].SetOutDataBound(outDataBound);
		}

		public bool GetIsInDataNull(int i)
		{
			return m_Val[i].GetIsInDataNull();
		}

		public void SetInNull(int i)
		{
			m_Val[i].SetInNull();
		}

		public bool GetIsOutDataNull()
		{
			return m_Val[0].GetIsOutDataNull();
		}

		public void SetOutNull()
		{
			m_Val[0].SetOutNull();
		}

		public byte[] GetInValue(int i)
		{
			return m_Val[i].GetInValue();
		}

		public void GetInValue(ref byte[] InValue, int i)
		{
			m_Val[i].GetInValue(ref InValue);
		}

		public void SetInValue(byte[] inValue, int i)
		{
			m_Val[i].SetInValue(inValue);
		}

		public byte[] GetOutValue()
		{
			return m_Val[0].GetOutValue();
		}

		public void SetOutValue(byte[] outValue)
		{
			m_Val[0].SetOutValue(outValue);
		}

		public void ClearInParam(int i)
		{
			m_Val[i].ClearInParam();
		}

		public void ClearOutParam()
		{
			m_Val[0].ClearOutParam();
		}

		public void SetRegisterParamType(int type)
		{
			m_Val[0].SetRegisterParamType(type);
		}

		public int GetRegisterParamType()
		{
			return m_Val[0].GetRegisterParamType();
		}

		public bool HasRegisterType()
		{
			return m_Val[0].HasRegisterType();
		}

		public void SetRegisterParamScale(int scale)
		{
			m_Val[0].SetRegisterParamScale(scale);
		}

		public int GetRegisterParamScale()
		{
			return m_Val[0].GetRegisterParamScale();
		}

		public bool HasRegisterScale()
		{
			return m_Val[0].HasRegisterScale();
		}

		public int GetStreamLen(int i)
		{
			return m_Val[i].GetStreamLen();
		}

		public int GetBytes(ref byte[] val, int val_off, int off, int len, int i)
		{
			return m_Val[i].GetBytes(ref val, val_off, off, len);
		}

		public List<DmParamValue> GetParamValue()
		{
			return m_Val;
		}

		public int GetSqlType()
		{
			return m_Val[0].GetSqlType();
		}

		public void SetSqlType(int sqlType)
		{
			m_Val[0].SetSqlType(sqlType);
		}

		public int GetPrec()
		{
			return m_Val[0].GetPrec();
		}

		public int GetBindScale()
		{
			return m_Val[0].GetScale();
		}
	}
}
