using System;
using System.Globalization;
using Dm.util;

namespace Dm
{
	internal class DmSetValue
	{
		private string m_ServerEncoding;

		private DmStatement m_Statement;

		internal DmConnProperty ConnProperty => m_Statement.ConnInst.ConnProperty;

		public DmSetValue(string servEncoding)
		{
			m_ServerEncoding = servEncoding;
		}

		public DmSetValue(string servEncoding, DmStatement stmt)
		{
			m_ServerEncoding = servEncoding;
			m_Statement = stmt;
		}

		public void ChangeSetValue(string servEncoding, DmStatement stmt)
		{
			m_ServerEncoding = servEncoding;
			m_Statement = stmt;
		}

		public void SetNull(DmParamValue paraVal)
		{
			paraVal.SetInNull();
		}

		private void SetBoolean(DmParamValue paraVal, bool x, int cType, int prec, int scale, byte typeFlag)
		{
			int num = (x ? 1 : 0);
			if (typeFlag != 1)
			{
				paraVal.SetInValue(DmConvertion.ByteToByteArray((byte)num));
				paraVal.SetSqlType(5);
				paraVal.SetPrec(1);
				paraVal.SetScale(0);
				return;
			}
			switch (cType)
			{
			case 3:
				paraVal.SetInValue(DmConvertion.ByteToByteArray((byte)num));
				break;
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, num.ToString(), cType, prec, scale, typeFlag);
				break;
			case 5:
				SetByte(paraVal, (sbyte)num, cType, prec, scale, typeFlag);
				break;
			case 6:
				SetShort(paraVal, (short)num, cType, prec, scale, typeFlag);
				break;
			case 7:
				SetInt(paraVal, num, cType, prec, scale, typeFlag);
				break;
			case 8:
				SetLong(paraVal, num, cType, prec, scale, typeFlag);
				break;
			case 9:
			case 24:
				SetBigDecimal(paraVal, new decimal(num), cType, prec, scale, typeFlag);
				break;
			case 10:
				SetFloat(paraVal, num, cType, prec, scale, typeFlag);
				break;
			case 11:
				SetDouble(paraVal, num, cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetByte(DmParamValue paraVal, sbyte x, int cType, int prec, int scale, byte typeFlag)
		{
			if (typeFlag != 1)
			{
				paraVal.SetInValue(DmConvertion.ByteToByteArray((byte)x));
				paraVal.SetSqlType(5);
				paraVal.SetPrec(1);
				paraVal.SetScale(0);
				return;
			}
			switch (cType)
			{
			case 3:
			{
				byte b = 0;
				if (x != 0)
				{
					b = 1;
				}
				paraVal.SetInValue(DmConvertion.ByteToByteArray(b));
				break;
			}
			case 5:
				paraVal.SetInValue(DmConvertion.ByteToByteArray((byte)x));
				break;
			case 6:
				SetShort(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 7:
				SetInt(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 8:
				SetLong(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 10:
				SetFloat(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 11:
				SetDouble(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 9:
			case 24:
				SetBigDecimal(paraVal, new decimal(x), cType, prec, scale, typeFlag);
				break;
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetShort(DmParamValue paraVal, short x, int cType, int prec, int scale, byte typeFlag)
		{
			if (typeFlag != 1)
			{
				paraVal.SetInValue(DmConvertion.ShortToByteArray(x));
				paraVal.SetSqlType(6);
				paraVal.SetPrec(2);
				paraVal.SetScale(0);
				return;
			}
			switch (cType)
			{
			case 6:
				paraVal.SetInValue(DmConvertion.ShortToByteArray(x));
				break;
			case 3:
			case 5:
				if (x > 127 || x < -128)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetByte(paraVal, (sbyte)x, cType, prec, scale, typeFlag);
				break;
			case 7:
				SetInt(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 8:
				SetLong(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 10:
				SetFloat(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 11:
				SetDouble(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 9:
			case 24:
				SetBigDecimal(paraVal, new decimal(x), cType, prec, scale, typeFlag);
				break;
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetInt(DmParamValue paraVal, int x, int cType, int prec, int scale, byte typeFlag)
		{
			if (typeFlag != 1)
			{
				paraVal.SetInValue(DmConvertion.IntToByteArray(x));
				paraVal.SetSqlType(7);
				paraVal.SetPrec(4);
				paraVal.SetScale(0);
				return;
			}
			switch (cType)
			{
			case 7:
				paraVal.SetInValue(DmConvertion.IntToByteArray(x));
				break;
			case 3:
			case 5:
				if (x > 127 || x < -128)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetByte(paraVal, (sbyte)x, cType, prec, scale, typeFlag);
				break;
			case 6:
				if (x > 32767 || x < -32768)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetShort(paraVal, (short)x, cType, prec, scale, typeFlag);
				break;
			case 8:
				SetLong(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 10:
				SetFloat(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 11:
				SetDouble(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 9:
			case 24:
				SetBigDecimal(paraVal, new decimal(x), cType, prec, scale, typeFlag);
				break;
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetLong(DmParamValue paraVal, long x, int cType, int prec, int scale, byte typeFlag)
		{
			if (typeFlag != 1)
			{
				if (x <= int.MaxValue && x >= int.MinValue && cType == 7)
				{
					SetInt(paraVal, (int)x, cType, prec, scale, typeFlag);
					return;
				}
				paraVal.SetInValue(DmConvertion.LongToByteArray(x));
				paraVal.SetSqlType(8);
				paraVal.SetPrec(8);
				paraVal.SetScale(0);
				return;
			}
			switch (cType)
			{
			case 8:
				paraVal.SetInValue(DmConvertion.LongToByteArray(x));
				break;
			case 3:
			case 5:
				if (x > 127 || x < -128)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetByte(paraVal, (sbyte)x, cType, prec, scale, typeFlag);
				break;
			case 6:
				if (x > 32767 || x < -32768)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetShort(paraVal, (short)x, cType, prec, scale, typeFlag);
				break;
			case 7:
				if (x > int.MaxValue || x < int.MinValue)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetInt(paraVal, (int)x, cType, prec, scale, typeFlag);
				break;
			case 10:
				SetFloat(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 11:
				SetDouble(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 9:
			case 24:
				SetBigDecimal(paraVal, new decimal(x), cType, prec, scale, typeFlag);
				break;
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetUShort(DmParamValue paraVal, ushort x, int cType, int prec, int scale, byte typeFlag)
		{
			if (typeFlag != 1)
			{
				paraVal.SetInValue(DmConvertion.ShortToByteArray(x));
				paraVal.SetSqlType(6);
				paraVal.SetPrec(2);
				paraVal.SetScale(0);
				return;
			}
			switch (cType)
			{
			case 6:
				if (x > 32767)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				paraVal.SetInValue(DmConvertion.ShortToByteArray(x));
				break;
			case 3:
			case 5:
				if (x > 127)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetByte(paraVal, (sbyte)x, cType, prec, scale, typeFlag);
				break;
			case 7:
				SetInt(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 8:
				SetLong(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 10:
				SetFloat(paraVal, (int)x, cType, prec, scale, typeFlag);
				break;
			case 11:
				SetDouble(paraVal, (int)x, cType, prec, scale, typeFlag);
				break;
			case 9:
			case 24:
				SetBigDecimal(paraVal, new decimal(x), cType, prec, scale, typeFlag);
				break;
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetUByte(DmParamValue paraVal, byte x, int cType, int prec, int scale, byte typeFlag)
		{
			if (typeFlag != 1)
			{
				paraVal.SetInValue(DmConvertion.ByteToByteArray(x));
				paraVal.SetSqlType(5);
				paraVal.SetPrec(1);
				paraVal.SetScale(0);
				return;
			}
			switch (cType)
			{
			case 3:
			{
				byte b = 0;
				if (x != 0)
				{
					b = 1;
				}
				paraVal.SetInValue(DmConvertion.ByteToByteArray(b));
				break;
			}
			case 5:
				if (x > 127)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				paraVal.SetInValue(DmConvertion.ByteToByteArray(x));
				break;
			case 6:
				SetShort(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 7:
				SetInt(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 8:
				SetLong(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 10:
				SetFloat(paraVal, (int)x, cType, prec, scale, typeFlag);
				break;
			case 11:
				SetDouble(paraVal, (int)x, cType, prec, scale, typeFlag);
				break;
			case 9:
			case 24:
				SetBigDecimal(paraVal, new decimal(x), cType, prec, scale, typeFlag);
				break;
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetUInt(DmParamValue paraVal, uint x, int cType, int prec, int scale, byte typeFlag)
		{
			if (typeFlag != 1)
			{
				paraVal.SetInValue(DmConvertion.IntToByteArray(x));
				paraVal.SetSqlType(7);
				paraVal.SetPrec(4);
				paraVal.SetScale(0);
				return;
			}
			switch (cType)
			{
			case 7:
				if (x > int.MaxValue)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				paraVal.SetInValue(DmConvertion.IntToByteArray(x));
				break;
			case 3:
			case 5:
				if (x > 127)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetByte(paraVal, (sbyte)x, cType, prec, scale, typeFlag);
				break;
			case 6:
				if (x > 32767)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetShort(paraVal, (short)x, cType, prec, scale, typeFlag);
				break;
			case 8:
				SetLong(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 10:
				SetFloat(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 11:
				SetDouble(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 9:
			case 24:
				SetBigDecimal(paraVal, new decimal(x), cType, prec, scale, typeFlag);
				break;
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetULong(DmParamValue paraVal, ulong x, int cType, int prec, int scale, byte typeFlag)
		{
			if (typeFlag != 1)
			{
				if (x <= uint.MaxValue && cType == 7)
				{
					SetInt(paraVal, (int)x, cType, prec, scale, typeFlag);
					return;
				}
				paraVal.SetInValue(DmConvertion.LongToByteArray((long)x));
				paraVal.SetSqlType(8);
				paraVal.SetPrec(8);
				paraVal.SetScale(0);
				return;
			}
			switch (cType)
			{
			case 8:
				if (x > long.MaxValue)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				paraVal.SetInValue(DmConvertion.LongToByteArray((long)x));
				break;
			case 3:
			case 5:
				if (x > 127)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetByte(paraVal, (sbyte)x, cType, prec, scale, typeFlag);
				break;
			case 6:
				if (x > 32767)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetShort(paraVal, (short)x, cType, prec, scale, typeFlag);
				break;
			case 7:
				if (x > int.MaxValue)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetInt(paraVal, (int)x, cType, prec, scale, typeFlag);
				break;
			case 10:
				SetFloat(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 11:
				SetDouble(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 9:
			case 24:
				SetBigDecimal(paraVal, new decimal(x), cType, prec, scale, typeFlag);
				break;
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetFloat(DmParamValue paraVal, float x, int cType, int prec, int scale, byte typeFlag)
		{
			if (typeFlag != 1)
			{
				paraVal.SetInValue(DmConvertion.FloatToByteArray(x));
				paraVal.SetSqlType(10);
				paraVal.SetPrec(0);
				paraVal.SetScale(0);
				return;
			}
			switch (cType)
			{
			case 10:
				paraVal.SetInValue(DmConvertion.FloatToByteArray(x));
				break;
			case 3:
			case 5:
				if (x > 127f || x < -128f)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetByte(paraVal, (sbyte)x, cType, prec, scale, typeFlag);
				break;
			case 6:
				if (x > 32767f || x < -32768f)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetShort(paraVal, (short)x, cType, prec, scale, typeFlag);
				break;
			case 7:
				if (x > 2.14748365E+09f || x < -2.14748365E+09f)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetInt(paraVal, (int)x, cType, prec, scale, typeFlag);
				break;
			case 8:
				if (x > 9.223372E+18f || x < -9.223372E+18f)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetLong(paraVal, (long)x, cType, prec, scale, typeFlag);
				break;
			case 11:
				SetDouble(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 9:
			case 24:
				SetDmDecimal(paraVal, new DmXDec().Parse(x.ToString()), cType, prec, scale, typeFlag);
				break;
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetDouble(DmParamValue paraVal, double x, int cType, int prec, int scale, byte typeFlag)
		{
			if (typeFlag != 1)
			{
				paraVal.SetInValue(DmConvertion.DoubleToByteArray(x));
				paraVal.SetSqlType(11);
				paraVal.SetPrec(0);
				paraVal.SetScale(0);
				return;
			}
			switch (cType)
			{
			case 11:
				paraVal.SetInValue(DmConvertion.DoubleToByteArray(x));
				break;
			case 3:
			case 5:
				if (x > 127.0 || x < -128.0)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetByte(paraVal, (sbyte)x, cType, prec, scale, typeFlag);
				break;
			case 6:
				if (x > 32767.0 || x < -32768.0)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetShort(paraVal, (short)x, cType, prec, scale, typeFlag);
				break;
			case 7:
				if (x > 2147483647.0 || x < -2147483648.0)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetInt(paraVal, (int)x, cType, prec, scale, typeFlag);
				break;
			case 8:
				if (x > 9.2233720368547758E+18 || x < -9.2233720368547758E+18)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetLong(paraVal, (long)x, cType, prec, scale, typeFlag);
				break;
			case 10:
				if (x > 3.4028234663852886E+38 || x < -3.4028234663852886E+38)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OVER_FLOW);
				}
				SetFloat(paraVal, (float)x, cType, prec, scale, typeFlag);
				break;
			case 9:
			case 24:
				SetDmDecimal(paraVal, new DmXDec().Parse(x.ToString()), cType, prec, scale, typeFlag);
				break;
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetDmDecimal(DmParamValue paraVal, DmXDec x, int cType, int prec, int scale, byte typeFlag)
		{
			if (x == null)
			{
				SetNull(paraVal);
				return;
			}
			if (typeFlag != 1)
			{
				paraVal.SetSqlType(cType);
				paraVal.SetPrec(prec);
				paraVal.SetScale(scale);
			}
			switch (cType)
			{
			case 9:
			{
				string str = x.ToString();
				byte[] inValue = new DmXDec().StrToDec(str, prec, scale, dmxdec_direct: true);
				paraVal.SetInValue(inValue);
				break;
			}
			case 3:
			case 5:
				SetByte(paraVal, Convert.ToSByte(x.ToString()), cType, prec, scale, typeFlag);
				break;
			case 6:
				SetShort(paraVal, Convert.ToInt16(x.ToString()), cType, prec, scale, typeFlag);
				break;
			case 7:
				SetInt(paraVal, Convert.ToInt32(x.ToString()), cType, prec, scale, typeFlag);
				break;
			case 8:
				SetLong(paraVal, Convert.ToInt64(x.ToString()), cType, prec, scale, typeFlag);
				break;
			case 10:
				SetFloat(paraVal, Convert.ToSingle(x.ToString()), cType, prec, scale, typeFlag);
				break;
			case 11:
				SetDouble(paraVal, Convert.ToDouble(x.ToString()), cType, prec, scale, typeFlag);
				break;
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetBigDecimal(DmParamValue paraVal, decimal? x, int cType, int prec, int scale, byte typeFlag)
		{
			if (!x.HasValue)
			{
				SetNull(paraVal);
				return;
			}
			if (typeFlag != 1)
			{
				paraVal.SetSqlType(cType);
				paraVal.SetPrec(prec);
				paraVal.SetScale(scale);
			}
			switch (cType)
			{
			case 9:
				new DmXDec().StrToDec(ref paraVal.m_InValue, x.ToString(), prec, scale, dmxdec_direct: false);
				paraVal.SetInValue();
				break;
			case 3:
			case 5:
				SetByte(paraVal, decimal.ToSByte(x.Value), cType, prec, scale, typeFlag);
				break;
			case 6:
				SetShort(paraVal, decimal.ToInt16(x.Value), cType, prec, scale, typeFlag);
				break;
			case 7:
				SetInt(paraVal, decimal.ToInt32(x.Value), cType, prec, scale, typeFlag);
				break;
			case 8:
				SetLong(paraVal, decimal.ToInt64(x.Value), cType, prec, scale, typeFlag);
				break;
			case 10:
				SetFloat(paraVal, decimal.ToSingle(x.Value), cType, prec, scale, typeFlag);
				break;
			case 11:
				SetDouble(paraVal, decimal.ToDouble(x.Value), cType, prec, scale, typeFlag);
				break;
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetString(DmParamValue paraVal, string x, int cType, int prec, int scale, byte typeFlag)
		{
			byte[] array = null;
			if (x == null)
			{
				SetNull(paraVal);
				return;
			}
			if (typeFlag != 1)
			{
				array = DmConvertion.GetBytes(x, m_ServerEncoding);
				paraVal.SetInValue(array);
				paraVal.SetSqlType(2);
				paraVal.SetPrec(8188);
				paraVal.SetScale(6);
				return;
			}
			switch (cType)
			{
			case 0:
			{
				array = DmConvertion.GetBytes(x, m_ServerEncoding);
				int num = array.Length;
				byte[] array2;
				if (prec == num || prec == 0)
				{
					array2 = array;
				}
				else if (prec > num)
				{
					array2 = new byte[prec];
					Array.Copy(array, 0, array2, 0, num);
					for (int j = num; j < prec; j++)
					{
						array2[j] = 32;
					}
				}
				else
				{
					array2 = new byte[prec];
					Array.Copy(array, 0, array2, 0, prec);
				}
				paraVal.SetInValue(array2);
				break;
			}
			case 1:
			case 2:
			case 19:
			{
				paraVal.m_InValue = DmConvertion.GetBytes(x, m_ServerEncoding);
				int num = paraVal.m_InValue.Length;
				if (prec < num && prec != 0)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_STR_CUT);
				}
				paraVal.SetInValue();
				break;
			}
			case 3:
			{
				byte b = 0;
				string text = x.Trim().ToUpper();
				if (text.ToUpper().Equals("FALSE") || text.Equals("0"))
				{
					b = 0;
				}
				else if (text.ToUpper().Equals("TRUE") || text.Equals("1"))
				{
					b = 1;
				}
				else
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_DATA_CONVERTION_ERROR);
				}
				SetByte(paraVal, (sbyte)b, cType, prec, scale, typeFlag);
				break;
			}
			case 5:
				if (x.Trim().Length > 0)
				{
					SetByte(paraVal, sbyte.Parse(x, DmConst.invariantCulture), cType, prec, scale, typeFlag);
				}
				else
				{
					SetNull(paraVal);
				}
				break;
			case 6:
				if (x.Trim().Length > 0)
				{
					SetShort(paraVal, short.Parse(x, DmConst.invariantCulture), cType, prec, scale, typeFlag);
				}
				else
				{
					SetNull(paraVal);
				}
				break;
			case 7:
				if (x.Trim().Length > 0)
				{
					SetInt(paraVal, int.Parse(x, DmConst.invariantCulture), cType, prec, scale, typeFlag);
				}
				else
				{
					SetNull(paraVal);
				}
				break;
			case 8:
				if (x.Trim().Length > 0)
				{
					SetLong(paraVal, long.Parse(x, DmConst.invariantCulture), cType, prec, scale, typeFlag);
				}
				else
				{
					SetNull(paraVal);
				}
				break;
			case 10:
				if (x.Trim().Length > 0)
				{
					SetFloat(paraVal, float.Parse(x, DmConst.invariantCulture), cType, prec, scale, typeFlag);
				}
				else
				{
					SetNull(paraVal);
				}
				break;
			case 11:
				if (x.Trim().Length > 0)
				{
					SetDouble(paraVal, double.Parse(x, DmConst.invariantCulture), cType, prec, scale, typeFlag);
				}
				else
				{
					SetNull(paraVal);
				}
				break;
			case 9:
			case 24:
				if (x.Trim().Length > 19)
				{
					SetDmDecimal(paraVal, new DmXDec().Parse(x), cType, prec, scale, typeFlag);
				}
				else if (x.Trim().Length > 0)
				{
					SetBigDecimal(paraVal, decimal.Parse(x, NumberStyles.Any, DmConst.invariantCulture), cType, prec, scale, typeFlag);
				}
				else
				{
					SetNull(paraVal);
				}
				break;
			case 17:
			{
				array = (byte[])(object)StringUtil.hexStringToBytes(x);
				int num = array.Length;
				byte[] array2;
				if (prec == num || prec == 0)
				{
					array2 = array;
				}
				else
				{
					array2 = new byte[prec];
					if (prec > num)
					{
						Array.Copy(array, 0, array2, 0, num);
						for (int i = num; i < prec; i++)
						{
							array2[i] = 0;
						}
					}
					else
					{
						Array.Copy(array, 0, array2, 0, prec);
					}
				}
				paraVal.SetInValue(array2);
				break;
			}
			case 12:
			case 18:
			{
				array = (byte[])(object)StringUtil.hexStringToBytes(x);
				int num = array.Length;
				byte[] array2;
				if (prec < num && prec != 0)
				{
					array2 = new byte[prec];
					Array.Copy(array, 0, array2, 0, prec);
				}
				else
				{
					array2 = array;
				}
				paraVal.SetInValue(array2);
				break;
			}
			case 14:
				if (x.Trim().Length > 0)
				{
					int[] dt = OracleDateFormat.Parse(x, ConnProperty.oracleDateFormat, ConnProperty.nlsDateLang);
					if (typeFlag != 1)
					{
						paraVal.SetSqlType(14);
						paraVal.SetPrec(3);
						paraVal.SetScale(6);
						scale = 6;
					}
					dt = OracleDateFormat.Round(dt, scale);
					paraVal.SetInValue(DmDateTime.DateEncodeFast(dt));
				}
				else
				{
					SetNull(paraVal);
				}
				break;
			case 15:
				if (x.Trim().Length > 0)
				{
					int[] dt5 = OracleDateFormat.Parse(x, ConnProperty.oracleTimeFormat, ConnProperty.nlsDateLang);
					if (typeFlag != 1)
					{
						paraVal.SetInValue(DmTime.TimeEncodeFast(dt5));
						paraVal.SetSqlType(15);
						paraVal.SetPrec(5);
						paraVal.SetScale(6);
						scale = 6;
					}
					dt5 = OracleDateFormat.Round(dt5, scale);
					paraVal.SetInValue(DmTime.TimeEncodeFast(dt5));
				}
				else
				{
					SetNull(paraVal);
				}
				break;
			case 22:
				if (x.Trim().Length > 0)
				{
					int[] dt4 = OracleDateFormat.Parse(x, ConnProperty.oracleTimeTZFormat, ConnProperty.nlsDateLang);
					dt4 = OracleDateFormat.Round(dt4, scale);
					paraVal.SetInValue(DmTime.TimeTzEncodeFast(dt4));
				}
				else
				{
					SetNull(paraVal);
				}
				break;
			case 16:
				if (x.Trim().Length > 0)
				{
					int[] dt3 = OracleDateFormat.Parse(x, ConnProperty.oracleTimestampFormat, ConnProperty.nlsDateLang);
					if (typeFlag != 1)
					{
						paraVal.SetSqlType(16);
						paraVal.SetPrec(8);
						paraVal.SetScale(6);
						scale = 6;
					}
					dt3 = OracleDateFormat.Round(dt3, scale);
					paraVal.SetInValue(DmDateTime.DmdtEncodeFast(dt3));
				}
				else
				{
					SetNull(paraVal);
				}
				break;
			case 23:
				if (x.Trim().Length > 0)
				{
					int[] dt2 = OracleDateFormat.Parse(x, ConnProperty.oracleTimestampTZFormat, ConnProperty.nlsDateLang);
					dt2 = OracleDateFormat.Round(dt2, scale);
					paraVal.SetInValue(DmDateTime.DmdttzEncodeFast(dt2));
				}
				else
				{
					SetNull(paraVal);
				}
				break;
			case 21:
			{
				int secPre = scale & 0xF;
				int loadPre = (scale >> 4) & 0xF;
				if (x.Trim().Length > 0)
				{
					SetINTERVALDT(paraVal, new DmIntervalDT(x, loadPre, secPre), cType, prec, scale, typeFlag);
				}
				else
				{
					SetNull(paraVal);
				}
				break;
			}
			case 20:
			{
				int pre = (scale >> 4) & 0xF;
				if (x.Trim().Length > 0)
				{
					SetINTERVALYM(paraVal, new DmIntervalYM(x, pre), cType, prec, scale, typeFlag);
				}
				else
				{
					SetNull(paraVal);
				}
				break;
			}
			default:
				throw new InvalidCastException();
			}
		}

		public void SetINTERVALYM(DmParamValue paraVal, DmIntervalYM ym, int cType, int prec, int scale, byte typeFlag)
		{
			if (ym == null)
			{
				SetNull(paraVal);
			}
			else if (typeFlag != 1)
			{
				paraVal.SetInValue(ym.ConvertStrToBs());
				paraVal.SetSqlType(20);
				paraVal.SetPrec(0);
				paraVal.SetScale(ym.GetPrec());
			}
			else if (cType == 20)
			{
				paraVal.SetInValue(ym.ConvertStrToBs());
			}
			else
			{
				SetString(paraVal, ym.ToString(), cType, prec, scale, typeFlag);
			}
		}

		public void SetINTERVALDT(DmParamValue paraVal, DmIntervalDT dt, int cType, int prec, int scale, byte typeFlag)
		{
			if (dt == null)
			{
				SetNull(paraVal);
			}
			else if (typeFlag != 1)
			{
				paraVal.SetInValue(dt.ConvertStrToBs());
				paraVal.SetSqlType(21);
				paraVal.SetPrec(0);
				paraVal.SetScale(dt.Prec);
			}
			else if (cType == 21)
			{
				paraVal.SetInValue(dt.ConvertStrToBs());
			}
			else
			{
				SetString(paraVal, dt.ToString(), cType, prec, scale, typeFlag);
			}
		}

		private void SetBytes(DmParamValue paraVal, byte[] x, int cType, int prec, int scale, byte typeFlag)
		{
			if (x == null)
			{
				SetNull(paraVal);
				return;
			}
			if (typeFlag != 1)
			{
				paraVal.SetInValue(x);
				paraVal.SetSqlType(18);
				paraVal.SetPrec(8188);
				paraVal.SetScale(0);
				return;
			}
			switch (cType)
			{
			case 17:
			{
				int num = x.Length;
				byte[] array;
				if (prec == num || prec == 0)
				{
					array = x;
				}
				else
				{
					array = new byte[prec];
					if (prec > num)
					{
						Array.Copy(x, 0, array, 0, num);
						for (int i = num; i < prec; i++)
						{
							array[i] = 0;
						}
					}
					else
					{
						Array.Copy(x, 0, array, 0, prec);
					}
				}
				paraVal.SetInValue(array);
				break;
			}
			case 1:
			case 2:
			case 12:
			case 18:
			case 19:
			{
				int num = x.Length;
				if (prec < num && prec != 0)
				{
					byte[] array = new byte[prec];
					Array.Copy(x, 0, array, 0, prec);
					paraVal.SetInValue(array);
				}
				else
				{
					paraVal.SetInValue(ref x);
				}
				break;
			}
			case 0:
			{
				int num = x.Length;
				byte[] array;
				if (prec == num || prec == 0)
				{
					array = x;
				}
				else if (prec > num)
				{
					array = new byte[prec];
					Array.Copy(x, 0, array, 0, num);
					for (int j = num; j < prec; j++)
					{
						array[j] = 32;
					}
				}
				else
				{
					array = new byte[prec];
					Array.Copy(x, 0, array, 0, prec);
				}
				paraVal.SetInValue(array);
				break;
			}
			case 3:
			{
				byte[] array = new byte[1];
				if (x[0] != 0)
				{
					array[0] = 1;
				}
				else
				{
					array[0] = 0;
				}
				paraVal.SetInValue(array);
				break;
			}
			case 5:
				paraVal.SetInValue(new byte[1] { x[0] });
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetDate(DmParamValue paraVal, DateTime x, int cType, int prec, int scale, byte typeFlag)
		{
			if (typeFlag != 1)
			{
				DmDateTime dmDateTime = new DmDateTime(x, prec, 6, 0, m_Statement.ConnInst.ConnProperty.TimeZone);
				paraVal.SetInValue(DmDateTime.DateEncodeFast(dmDateTime.GetByteArrayValue()));
				paraVal.SetSqlType(14);
				paraVal.SetPrec(3);
				paraVal.SetScale(6);
				return;
			}
			switch (cType)
			{
			case 14:
			{
				DmDateTime dmDateTime2 = new DmDateTime(x, prec, scale, 0, m_Statement.ConnInst.ConnProperty.TimeZone);
				paraVal.SetInValue(DmDateTime.DateEncodeFast(dmDateTime2.GetByteArrayValue()));
				break;
			}
			case 16:
			case 23:
				SetTimestamp(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetTime(DmParamValue paraVal, DateTime x, int cType, int prec, int scale, byte typeFlag)
		{
			if (typeFlag != 1)
			{
				DmTime dmTime = new DmTime(x, prec, 6, m_Statement.ConnInst.ConnProperty.TimeZone);
				paraVal.SetInValue(DmTime.TimeEncodeFast(dmTime.GetByteArrayValue()));
				paraVal.SetSqlType(15);
				paraVal.SetPrec(5);
				paraVal.SetScale(6);
				return;
			}
			switch (cType)
			{
			case 15:
			{
				DmTime dmTime2 = new DmTime(x, prec, scale, m_Statement.ConnInst.ConnProperty.TimeZone);
				paraVal.SetInValue(DmTime.TimeEncodeFast(dmTime2.GetByteArrayValue()));
				break;
			}
			case 22:
			{
				DmTime dmTime2 = new DmTime(x, prec, scale, m_Statement.ConnInst.ConnProperty.TimeZone);
				paraVal.SetInValue(DmTime.TimeTzEncodeFast(dmTime2.GetTzByteArrayValue()));
				break;
			}
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		public void SetTime(DmParamValue paraVal, DmTime t, int cType, int prec, int scale, byte typeFlag)
		{
			if (t == null)
			{
				SetNull(paraVal);
				return;
			}
			if (typeFlag != 1)
			{
				paraVal.SetInValue(DmTime.TimeEncodeFast(t.GetByteArrayValue()));
				paraVal.SetSqlType(15);
				paraVal.SetPrec(5);
				paraVal.SetScale(6);
				return;
			}
			switch (cType)
			{
			case 15:
				paraVal.SetInValue(DmTime.TimeEncodeFast(t.GetByteArrayValue()));
				break;
			case 22:
				paraVal.SetInValue(DmTime.TimeTzEncodeFast(t.GetTzByteArrayValue()));
				break;
			case 16:
			case 23:
			{
				DateTime x = DateTime.Parse(t.ToString(), DmConst.invariantCulture);
				SetTimestamp(paraVal, x, cType, prec, scale, typeFlag);
				break;
			}
			default:
				SetString(paraVal, t.ToString(), cType, prec, scale, typeFlag);
				break;
			}
		}

		private void SetTimestamp(DmParamValue paraVal, DateTime x, int cType, int prec, int scale, byte typeFlag)
		{
			byte[] ret = null;
			if (typeFlag != 1)
			{
				DmDateTime dmDateTime = new DmDateTime(x, prec, 6, 2, m_Statement.ConnInst.ConnProperty.TimeZone);
				paraVal.SetInValue(DmDateTime.DmdtEncodeFast(dmDateTime.GetByteArrayValue()));
				paraVal.SetSqlType(16);
				paraVal.SetPrec(8);
				paraVal.SetScale(6);
				return;
			}
			switch (cType)
			{
			case 14:
				SetDate(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 15:
			case 22:
				SetTime(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 16:
			{
				DmDateTime dmDateTime2 = new DmDateTime(x, prec, scale, 2, m_Statement.ConnInst.ConnProperty.TimeZone);
				dmDateTime2.GetByteArrayValue(ref ret);
				if (ret.Length == 8)
				{
					paraVal.SetInValue(ret);
					break;
				}
				DmDateTime.DmdtEncodeFast(ref paraVal.m_InValue, ref ret);
				paraVal.SetInValue();
				break;
			}
			case 23:
			{
				DmDateTime dmDateTime2 = new DmDateTime(x, prec, scale, 3, m_Statement.ConnInst.ConnProperty.TimeZone);
				paraVal.SetInValue(DmDateTime.DmdttzEncodeFast(dmDateTime2.GetByteArrayValue()));
				break;
			}
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetTimeTZ(DmParamValue paraVal, DateTimeOffset x, int cType, int prec, int scale, byte typeFlag)
		{
			if (typeFlag != 1)
			{
				DmTime dmTime = new DmTime(x.DateTime, prec, scale, Convert.ToInt16(x.Offset.TotalMinutes));
				paraVal.SetInValue(DmTime.TimeTzEncodeFast(dmTime.GetByteArrayValue()));
				paraVal.SetSqlType(22);
				paraVal.SetPrec(5);
				paraVal.SetScale(6);
				return;
			}
			switch (cType)
			{
			case 15:
			{
				DmTime dmTime2 = new DmTime(x.DateTime, prec, scale, Convert.ToInt16(x.Offset.TotalMinutes));
				paraVal.SetInValue(DmTime.TimeEncodeFast(dmTime2.GetByteArrayValue()));
				break;
			}
			case 22:
			{
				DmTime dmTime2 = new DmTime(x.DateTime, prec, scale, Convert.ToInt16(x.Offset.TotalMinutes));
				paraVal.SetInValue(DmTime.TimeTzEncodeFast(dmTime2.GetTzByteArrayValue()));
				break;
			}
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetTimestampTZ(DmParamValue paraVal, DateTimeOffset x, int cType, int prec, int scale, byte typeFlag)
		{
			byte[] ret = null;
			if (typeFlag != 1)
			{
				DmDateTime dmDateTime = new DmDateTime(x.DateTime, prec, 6, 3, Convert.ToInt16(x.Offset.TotalMinutes));
				paraVal.SetInValue(DmDateTime.DmdttzEncodeFast(dmDateTime.GetByteArrayValue()));
				paraVal.SetSqlType(23);
				paraVal.SetPrec(8);
				paraVal.SetScale(6);
				return;
			}
			switch (cType)
			{
			case 14:
				SetDate(paraVal, x.DateTime, cType, prec, scale, typeFlag);
				break;
			case 15:
				SetTime(paraVal, x.DateTime, cType, prec, scale, typeFlag);
				break;
			case 22:
				SetTimeTZ(paraVal, x, cType, prec, scale, typeFlag);
				break;
			case 16:
			{
				DmDateTime dmDateTime2 = new DmDateTime(x.DateTime, prec, scale, 2, Convert.ToInt16(x.Offset.TotalMinutes));
				dmDateTime2.GetByteArrayValue(ref ret);
				if (ret.Length == 8)
				{
					paraVal.SetInValue(ret);
					break;
				}
				DmDateTime.DmdtEncodeFast(ref paraVal.m_InValue, ref ret);
				paraVal.SetInValue();
				break;
			}
			case 23:
			{
				DmDateTime dmDateTime2 = new DmDateTime(x.DateTime, prec, scale, 3, Convert.ToInt16(x.Offset.TotalMinutes));
				paraVal.SetInValue(DmDateTime.DmdttzEncodeFast(dmDateTime2.GetByteArrayValue()));
				break;
			}
			case 0:
			case 1:
			case 2:
			case 19:
				SetString(paraVal, x.ToString(), cType, prec, scale, typeFlag);
				break;
			default:
				throw new InvalidCastException();
			}
		}

		private void SetArray(DmParamValue paraVal, Array x, DmConnection conn, string typeName, int cType, DmParameterInternal paraInternal)
		{
			DmArray data = new DmArray(new ArrayDescriptor(typeName, conn), conn, x);
			if (paraInternal.GetTypeFlag() != 1)
			{
				paraVal.SetInValue(TypeData.arrayToBytes(data, paraInternal.TypeDesc));
				paraVal.SetSqlType(117);
				return;
			}
			switch (cType)
			{
			case 122:
				paraVal.SetInValue(TypeData.sarrayToBytes(data, paraInternal.TypeDesc));
				break;
			case 117:
			case 119:
				paraVal.SetInValue(TypeData.arrayToBytes(data, paraInternal.TypeDesc));
				break;
			default:
				throw new InvalidCastException();
			}
		}

		public void SetObject(DmParamValue paraVal, object x, DmConnection conn, string typeName, int cType, DmParameterInternal paraInternal)
		{
			int precision = paraInternal.GetPrecision();
			int scale = paraInternal.GetScale();
			byte typeFlag = paraInternal.GetTypeFlag();
			if (x == null)
			{
				SetNull(paraVal);
				return;
			}
			if (x is DBNull)
			{
				SetNull(paraVal);
				return;
			}
			if (x is byte)
			{
				SetInt(paraVal, (byte)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is sbyte)
			{
				SetInt(paraVal, (sbyte)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is string)
			{
				SetString(paraVal, (string)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is char)
			{
				SetString(paraVal, x.ToString(), cType, precision, scale, typeFlag);
				return;
			}
			if (x is char[])
			{
				string x2 = new string((char[])x);
				SetString(paraVal, x2, cType, precision, scale, typeFlag);
				return;
			}
			if (x is decimal)
			{
				SetBigDecimal(paraVal, (decimal)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is short)
			{
				SetShort(paraVal, (short)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is ushort)
			{
				SetInt(paraVal, (ushort)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is int)
			{
				SetInt(paraVal, (int)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is uint)
			{
				SetLong(paraVal, (uint)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is long)
			{
				SetLong(paraVal, (long)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is ulong)
			{
				SetULong(paraVal, (ulong)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is float)
			{
				SetFloat(paraVal, (float)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is double)
			{
				SetDouble(paraVal, (double)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is sbyte[])
			{
				SetBytes(paraVal, (byte[])x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is byte[])
			{
				SetBytes(paraVal, (byte[])x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is DateTime)
			{
				SetTimestamp(paraVal, (DateTime)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is DateTimeOffset)
			{
				SetTimestampTZ(paraVal, (DateTimeOffset)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is bool)
			{
				SetBoolean(paraVal, (bool)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is DmTime)
			{
				SetTime(paraVal, (DmTime)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is DmIntervalYM)
			{
				SetINTERVALYM(paraVal, (DmIntervalYM)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is DmIntervalDT)
			{
				SetINTERVALDT(paraVal, (DmIntervalDT)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is TimeSpan)
			{
				TimeSpan timeSpan = (TimeSpan)x;
				if (cType == 3 || cType == 4 || cType == 5 || cType == 6 || cType == 7 || cType == 8 || cType == 9 || cType == 10 || cType == 11 || cType == 12 || cType == 21)
				{
					SetINTERVALDT(paraVal, new DmIntervalDT(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds), cType, precision, scale, typeFlag);
				}
				else
				{
					SetString(paraVal, ((TimeSpan)x).ToString(), cType, precision, scale, typeFlag);
				}
				return;
			}
			if (x is DmXDec)
			{
				SetDmDecimal(paraVal, (DmXDec)x, cType, precision, scale, typeFlag);
				return;
			}
			if (x is Guid)
			{
				if ((cType == 0 || cType == 2 || cType == 1) && (precision == 36 || precision == 8188))
				{
					SetString(paraVal, ((Guid)x).ToString(), cType, precision, scale, typeFlag);
					return;
				}
				throw new InvalidCastException("not support this GUID cast");
			}
			if (x.GetType().BaseType == typeof(Enum))
			{
				string[] names = Enum.GetNames(x.GetType());
				int num = 0;
				string[] array = names;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].Equals(x.ToString()))
					{
						SetInt(paraVal, num, cType, precision, scale, typeFlag);
						return;
					}
					num++;
				}
				throw new SystemException("Value is of unknown data type");
			}
			if (x is Array && !typeName.Equals(""))
			{
				SetArray(paraVal, (Array)x, conn, typeName, cType, paraInternal);
				return;
			}
			throw new SystemException("Value is of unknown data type");
		}

		private string fixDecString(string val, int prec, int scale, int signNum)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			text3 = ((signNum >= 0) ? "+" : "-");
			string[] array = val.Split(new char[1] { '.' });
			if (array.Length == 1)
			{
				if (array[0].Length < prec - scale)
				{
					for (int i = 0; i < prec - scale - array[0].Length; i++)
					{
						text += "0";
					}
					array[0] = text + array[0];
				}
				for (int j = 0; j < scale; j++)
				{
					text2 += "0";
				}
				return text3 + array[0] + "." + text2;
			}
			if (array[0].Length > prec - scale)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DIGITAL_FORMAT);
			}
			if (array[0].Length < prec - scale)
			{
				if (array[0].Equals("0"))
				{
					array[0] = "";
				}
				for (int k = 0; k < prec - scale - array[0].Length; k++)
				{
					text += "0";
				}
			}
			array[0] = text + array[0];
			if (array[1].Length > scale)
			{
				array[1] = array[1].Substring(0, scale);
			}
			else if (array[1].Length < scale)
			{
				for (int l = 0; l < scale - array[1].Length; l++)
				{
					text2 += "0";
				}
			}
			array[1] += text2;
			return text3 + array[0] + "." + array[1];
		}

		private byte[] decStringToBcd(string decString)
		{
			bool flag = true;
			byte[] array = new byte[(decString.Length + 1) / 2];
			int num = 0;
			char[] array2 = decString.ToCharArray();
			for (int i = 0; i < array2.Length; i++)
			{
				byte b = Convert.ToByte(('0' <= array2[i] && array2[i] <= '9') ? ((byte)(array2[i] - 48)) : (array2[i] switch
				{
					'.' => 10, 
					'+' => 11, 
					'-' => 12, 
					_ => 15, 
				}));
				if (flag)
				{
					array[num] = (byte)((array[num] & 0xF0u) | (b & 0xFu));
					flag = false;
				}
				else
				{
					array[num] = (byte)((array[num] & 0xFu) | ((uint)(b << 4) & 0xF0u));
					flag = true;
					num++;
				}
			}
			if (!flag)
			{
				array[num] = (byte)((array[num] & 0xFu) | 0xF0u);
			}
			return array;
		}
	}
}
