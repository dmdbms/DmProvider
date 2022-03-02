using System;
using System.Data;
using System.IO;
using System.Text;
using Dm.util;

namespace Dm
{
	internal class DmSysTypeConvertion
	{
		internal static string StringConvertion(string val, DmParameter param)
		{
			if (val == null)
			{
				return val;
			}
			if ((param.do_DbType == DbType.AnsiStringFixedLength || param.do_DbType == DbType.StringFixedLength) && val.Length < param.do_Size)
			{
				val = val.PadRight(param.do_Size, ' ');
			}
			if (val.Length > param.do_Size && param.m_SetSizeFlag && param.do_Size != 0)
			{
				val = val.Substring(0, param.do_Size);
			}
			return val;
		}

		internal static decimal DecimalConvertion(decimal dec, DmParameter param)
		{
			string[] array = dec.ToString().Split('.');
			if (param.do_DbType == DbType.Currency)
			{
				if (decimal.Compare(dec, new decimal(-922337203685477.63)) < 0 || decimal.Compare(dec, new decimal(922337203685477.63)) > 0)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_DATA_CONVERTION_ERROR);
				}
				param.do_Precision = 19;
				param.do_Scale = 4;
				param.m_SetPrecFlag = true;
				param.m_SetScaleFlag = true;
			}
			if (param.m_SetPrecFlag)
			{
				int length = array[0].Length;
				if (array[0].StartsWith("+") || array[0].StartsWith("-"))
				{
					length--;
				}
			}
			if (array.Length == 2)
			{
				dec = decimal.Parse(array[0] + "." + array[1], DmConst.invariantCulture);
			}
			return dec;
		}

		internal static object TypeConvertion(DmParameter param)
		{
			object obj = param.do_Value;
			DbType do_DbType = param.do_DbType;
			Type type = DmSqlType.DbTypeToType(param.do_DbType);
			int do_Size = param.do_Size;
			_ = param.do_Precision;
			_ = param.do_Scale;
			if (param.do_Value == null || param.do_Value == DBNull.Value || param.DmSqlType == DmDbType.ARRAY)
			{
				return param.do_Value;
			}
			if (!param.m_SetDbTypeFlag)
			{
				if (Type.GetTypeCode(type) == TypeCode.String && param.m_SetSizeFlag)
				{
					if (param.do_Value is string)
					{
						return StringConvertion((string)param.do_Value, param);
					}
					if (param.do_Value is string[])
					{
						string[] array = param.do_Value as string[];
						if (array != null)
						{
							string[] array2 = new string[array.Length];
							for (int i = 0; i < array.Length; i++)
							{
								array2[i] = StringConvertion(array[i], param);
							}
							return array2;
						}
					}
					else if (param.do_Value is TextReader)
					{
						return StreamUtil.readString(param.do_Value as TextReader, param.do_Size);
					}
					DmError.ThrowDmException(DmErrorDefinition.ECNET_DATA_CONVERTION_ERROR);
					return null;
				}
				if (Type.GetTypeCode(type) == TypeCode.Decimal)
				{
					if (param.do_Value is decimal)
					{
						return DecimalConvertion((decimal)param.do_Value, param);
					}
					if (param.do_Value is decimal[])
					{
						decimal[] array3 = param.do_Value as decimal[];
						if (array3 != null)
						{
							decimal[] array4 = new decimal[array3.Length];
							for (int j = 0; j < array3.Length; j++)
							{
								array4[j] = DecimalConvertion(array3[j], param);
							}
							return array4;
						}
					}
					DmError.ThrowDmException(DmErrorDefinition.ECNET_DATA_CONVERTION_ERROR);
					return null;
				}
				return param.do_Value;
			}
			try
			{
				if (Type.GetTypeCode(type) != TypeCode.Object)
				{
					object obj2 = null;
					Array array5 = null;
					try
					{
						obj2 = Convert.ChangeType(obj, type);
					}
					catch (InvalidCastException)
					{
						if (!obj.GetType().IsArray)
						{
							if (param.do_Value is TextReader)
							{
								return StreamUtil.readString(param.do_Value as TextReader, param.do_Size);
							}
							if (param.do_Value is Stream)
							{
								return StreamUtil.readBytes(param.do_Value as Stream, param.do_Size);
							}
							DmError.ThrowDmException(DmErrorDefinition.ECNET_DATA_CONVERTION_ERROR);
							return null;
						}
						array5 = obj as Array;
						for (int k = 0; k < array5.Length; k++)
						{
							array5.SetValue(Convert.ChangeType(array5.GetValue(k), type), k);
						}
					}
					if (do_DbType == DbType.Date)
					{
						if (obj2 != null)
						{
							obj2 = ((DateTime)obj2).Date;
						}
						else if (array5 != null)
						{
							for (int l = 0; l < array5.Length; l++)
							{
								array5.SetValue(((DateTime)array5.GetValue(l)).Date, l);
							}
						}
					}
					if (Type.GetTypeCode(type) == TypeCode.String)
					{
						if (obj2 != null)
						{
							obj2 = StringConvertion((string)obj2, param);
						}
						else if (array5 != null)
						{
							for (int m = 0; m < array5.Length; m++)
							{
								array5.SetValue(StringConvertion((string)array5.GetValue(m), param), m);
							}
						}
					}
					if (Type.GetTypeCode(type) == TypeCode.Decimal)
					{
						if (obj2 != null)
						{
							obj2 = DecimalConvertion((decimal)obj2, param);
						}
						else if (array5 != null)
						{
							for (int n = 0; n < array5.Length; n++)
							{
								array5.SetValue(DecimalConvertion((decimal)array5.GetValue(n), param), n);
							}
						}
					}
					if (obj2 != null)
					{
						return obj2;
					}
					if (array5 != null)
					{
						return array5;
					}
					DmError.ThrowDmException(DmErrorDefinition.ECNET_DATA_CONVERTION_ERROR);
					return null;
				}
				if (do_DbType == DbType.Binary)
				{
					if (obj is sbyte[] || obj is byte[] || obj is sbyte[][] || obj is byte[][])
					{
						return obj;
					}
					if (!(obj is bool) && !(obj is sbyte) && !(obj is short) && !(obj is int) && !(obj is long))
					{
						try
						{
							object obj3 = Convert.ChangeType(obj, typeof(string));
							return Encoding.Default.GetBytes((string)obj3);
						}
						catch (InvalidCastException)
						{
							if (obj.GetType().IsArray)
							{
								object[] array6 = obj as object[];
								byte[][] array7 = new byte[array6.Length][];
								for (int num = 0; num < array6.Length; num++)
								{
									array7[num] = Encoding.Default.GetBytes((string)Convert.ChangeType(array6[num], typeof(string)));
								}
								return array7;
							}
							if (param.do_Value is Stream)
							{
								return StreamUtil.readBytes(param.do_Value as Stream, param.do_Size);
							}
						}
						DmError.ThrowDmException(DmErrorDefinition.ECNET_DATA_CONVERTION_ERROR);
						return null;
					}
					DmError.ThrowDmException(DmErrorDefinition.ECNET_DATA_CONVERTION_ERROR);
				}
				if (do_DbType == DbType.Object && obj is string)
				{
					if (do_Size < ((string)obj).Length)
					{
						obj = ((string)obj).Substring(0, do_Size);
					}
				}
				else if (do_DbType == DbType.Object && obj is string[])
				{
					string[] array8 = obj as string[];
					string[] array9 = new string[array8.Length];
					for (int num2 = 0; num2 < array8.Length; num2++)
					{
						if (do_Size < array8[num2].Length)
						{
							array9[num2] = array8[num2].Substring(0, do_Size);
						}
					}
					return array9;
				}
				if (do_DbType == DbType.Time)
				{
					if (obj is DateTime)
					{
						return new TimeSpan(0, ((DateTime)obj).Hour, ((DateTime)obj).Minute, ((DateTime)obj).Second, ((DateTime)obj).Millisecond);
					}
					if (obj is DateTime[])
					{
						DateTime[] array10 = obj as DateTime[];
						TimeSpan[] array11 = new TimeSpan[array10.Length];
						for (int num3 = 0; num3 < array10.Length; num3++)
						{
							array11[num3] = new TimeSpan(0, array10[num3].Hour, array10[num3].Minute, array10[num3].Second, array10[num3].Millisecond);
						}
						return array11;
					}
					return TimeSpan.Parse(obj.ToString(), DmConst.invariantCulture);
				}
				return obj;
			}
			catch (Exception)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_DATA_CONVERTION_ERROR);
				return null;
			}
		}
	}
}
