using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Dm
{
	[StructLayout(LayoutKind.Sequential)]
	internal class Dmxdec
	{
		private byte sign;

		private byte ndigits;

		private byte rscale;

		private short weight;

		private byte len;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
		private byte[] value;

		public Dmxdec()
		{
			value = new byte[22];
		}

		public byte[] GetValue()
		{
			return value;
		}

		public byte GetLen()
		{
			return len;
		}

		[DllImport("dmcalc.dll")]
		public static extern int xdec_from_char(IntPtr xdec, string str, uint len);
	}
	public class DmXDec
	{
		private const int XDEC_TEMPBUF_SIZE = 21;

		private const int XDEC_POSITIVE = 193;

		private const int XDEC_NEGTIVE = 62;

		private const int XDEC_NEGTIVE_NUM = 101;

		private const int XDEC_SIGN_POSITIVE = 1;

		private const int XDEC_SIGN_NEGTIVE = 0;

		private const int XDEC_MAX_LEN = 38;

		private int n;

		private int m;

		private int m_sign = 1;

		private int m_exp;

		private string m_strInt;

		private string m_strDec;

		private int m_max_len = 38;

		private byte[] m_b;

		public DmXDec()
		{
		}

		internal DmXDec(byte[] b)
		{
			m_b = b;
		}

		private string checkStr(string strOrg, int prec, int scale)
		{
			string text = null;
			try
			{
				decimal d = decimal.Parse(strOrg, DmConst.invariantCulture);
				text = d.ToString();
				if (prec != 0 || scale != 0)
				{
					text = decimal.Round(d, scale).ToString();
					m_max_len = prec;
					return text;
				}
				return text;
			}
			catch (Exception)
			{
				throw new InvalidCastException();
			}
		}

		private string setSign(string strOrg)
		{
			m_sign = 1;
			if (strOrg.StartsWith("-"))
			{
				m_sign = 0;
				return strOrg.Substring(1);
			}
			if (strOrg.StartsWith("+"))
			{
				m_sign = 1;
				return strOrg.Substring(1);
			}
			return strOrg;
		}

		private string rmvUnnecessaryZeros(string strOrg)
		{
			int i;
			for (i = 0; i < strOrg.Length && '0' == strOrg[i]; i++)
			{
			}
			int num = strOrg.Substring(i).IndexOf('.');
			int num2 = strOrg.Length - 1;
			while (-1 != num && '0' == strOrg[num2] && num2 >= 0)
			{
				num2--;
			}
			if (-1 != num)
			{
				return strOrg.Substring(i, num2 + 1 - i);
			}
			return strOrg.Substring(i);
		}

		private void setDecInt(string strRet)
		{
			int num = strRet.IndexOf('.');
			if (num == 0)
			{
				m_strInt = null;
				if (1 == strRet.Length)
				{
					m_strDec = null;
				}
				else
				{
					m_strDec = strRet.Substring(1);
				}
			}
			else if (-1 == num || strRet.Length - 1 == num)
			{
				m_strDec = null;
				if (-1 == num)
				{
					m_strInt = strRet;
				}
				else
				{
					m_strInt = strRet.Substring(0, strRet.Length - 1);
				}
			}
			else
			{
				m_strInt = strRet.Substring(0, num);
				m_strDec = strRet.Substring(num + 1);
			}
		}

		private bool checkZero(string strRet)
		{
			decimal num = decimal.Parse(strRet, DmConst.invariantCulture);
			decimal value = 0m;
			if (num.CompareTo(value) == 0)
			{
				return true;
			}
			return false;
		}

		private bool checkZero2(string strRet)
		{
			int num = strRet.IndexOf('.');
			int num2 = strRet.Length;
			int num3 = 0;
			int num4 = strRet.IndexOf('e');
			if (-1 == num4)
			{
				num4 = strRet.IndexOf('E');
			}
			if (-1 != num4)
			{
				num2 = num4;
			}
			for (num3 = 0; num3 < num; num3++)
			{
				if (strRet[num3] != '0')
				{
					return false;
				}
			}
			for (num3 = num2 - 1; num3 > num; num3--)
			{
				if (strRet[num3] != '0')
				{
					return false;
				}
			}
			if (num == 0 && num2 == 1 && strRet[num3] != '0')
			{
				return false;
			}
			return true;
		}

		private int cntZeroEnd(string strInt)
		{
			int num = strInt.Length - 1;
			while (num >= 0 && '0' == strInt[num])
			{
				num--;
			}
			return strInt.Length - 1 - num;
		}

		private int cntZeroStart(string strDec)
		{
			int i;
			for (i = 0; i < strDec.Length && '0' == strDec[i]; i++)
			{
			}
			return i;
		}

		private byte[] processStrInt()
		{
			if (m_strInt == null)
			{
				return null;
			}
			int length = m_strInt.Length;
			int num = 0;
			int i = 0;
			int num2 = length / 2;
			if (length % 2 != 0)
			{
				num2++;
			}
			byte[] array = new byte[num2];
			bool flag = false;
			if (length % 2 != 0)
			{
				num = Convert.ToInt32(m_strInt.Substring(0, 1));
				if (m_sign == 1)
				{
					array[i] = (byte)(num + 1);
				}
				else
				{
					array[i] = (byte)(101 - num);
				}
				i++;
				flag = true;
			}
			for (; i < num2; i++)
			{
				num = ((!flag) ? Convert.ToInt32(m_strInt.Substring(i * 2, 2)) : Convert.ToInt32(m_strInt.Substring(i * 2 - 1, 2)));
				if (m_sign == 1)
				{
					array[i] = (byte)(num + 1);
				}
				else
				{
					array[i] = (byte)(101 - num);
				}
			}
			return array;
		}

		private byte[] processStrDec()
		{
			if (m_strDec == null)
			{
				return null;
			}
			int length = m_strDec.Length;
			int num = 0;
			if (length % 2 != 0)
			{
				m_strDec += "0";
				length = m_strDec.Length;
			}
			int num2 = length / 2;
			byte[] array = new byte[num2];
			for (int i = 0; i < num2; i++)
			{
				num = Convert.ToInt32(m_strDec.Substring(i * 2, 2));
				if (m_sign == 1)
				{
					array[i] = (byte)(num + 1);
				}
				else
				{
					array[i] = (byte)(101 - num);
				}
			}
			return array;
		}

		private byte fixFlag()
		{
			byte b = 0;
			if (m_sign == 1)
			{
				return (byte)(193 + m_exp / 2);
			}
			return (byte)(62 - m_exp / 2);
		}

		private byte[] fixDec(byte flag, byte[] btBefore, byte[] btAfter)
		{
			int num = 0;
			byte[] array = new byte[21];
			array[0] = flag;
			num++;
			if (btBefore != null)
			{
				byte[] array2 = null;
				if (btBefore.Length > 21 - num)
				{
					array2 = new byte[Math.Abs(21 - num)];
					Array.Copy(btBefore, 0, array2, 0, array2.Length);
				}
				else
				{
					array2 = btBefore;
				}
				Array.Copy(array2, 0, array, num, array2.Length);
				num += array2.Length;
			}
			if (btAfter != null)
			{
				byte[] array3 = null;
				if (btAfter.Length > 21 - num)
				{
					array3 = new byte[Math.Abs(21 - num)];
					Array.Copy(btAfter, 0, array3, 0, array3.Length);
				}
				else
				{
					array3 = btAfter;
				}
				Array.Copy(array3, 0, array, num, array3.Length);
				num += array3.Length;
			}
			if (m_sign == 0 && num < 20)
			{
				array[num++] = 102;
			}
			if (num < 20)
			{
				array[num] = 0;
			}
			byte[] array4 = new byte[num];
			Array.Copy(array, 0, array4, 0, num);
			m_b = array4;
			return array4;
		}

		private byte[] fixDec(ref byte[] Ret, byte flag, byte[] btBefore, byte[] btAfter)
		{
			int num = 0;
			byte[] array = new byte[21];
			array[0] = flag;
			num++;
			if (btBefore != null)
			{
				byte[] array2 = null;
				if (btBefore.Length > 21 - num)
				{
					array2 = new byte[Math.Abs(21 - num)];
					Array.Copy(btBefore, 0, array2, 0, array2.Length);
				}
				else
				{
					array2 = btBefore;
				}
				Array.Copy(array2, 0, array, num, array2.Length);
				num += array2.Length;
			}
			if (btAfter != null)
			{
				byte[] array3 = null;
				if (btAfter.Length > 21 - num)
				{
					array3 = new byte[Math.Abs(21 - num)];
					Array.Copy(btAfter, 0, array3, 0, array3.Length);
				}
				else
				{
					array3 = btAfter;
				}
				Array.Copy(array3, 0, array, num, array3.Length);
				num += array3.Length;
			}
			if (m_sign == 0 && num < 20)
			{
				array[num++] = 102;
			}
			if (num < 20)
			{
				array[num] = 0;
			}
			Ret = new byte[num];
			Array.Copy(array, 0, Ret, 0, num);
			m_b = Ret;
			return Ret;
		}

		private byte[] fixZero()
		{
			return new byte[1] { 128 };
		}

		private void fixZero(ref byte[] ret)
		{
			ret = new byte[1];
			ret[0] = 128;
		}

		private string setExp(string strOrg)
		{
			int num = strOrg.IndexOf('e');
			if (-1 == num)
			{
				num = strOrg.IndexOf('E');
			}
			if (-1 == num)
			{
				m_exp = 0;
				return strOrg;
			}
			string value = strOrg.Substring(num + 1);
			m_exp = Convert.ToInt32(value);
			return strOrg.Substring(0, num);
		}

		private void checkOverFlow()
		{
			if (m_exp > 123 || m_exp < -128)
			{
				DmError.ThrowDmException(DmErrorDefinition.EC_DATA_OVERFLOW);
			}
		}

		private void processExp()
		{
			if (m_exp % 2 == 0)
			{
				return;
			}
			string strDec = m_strDec;
			string strInt = m_strInt;
			if (strInt == null && strDec != null)
			{
				if (strDec.StartsWith("0"))
				{
					m_strDec = m_strDec.Substring(1);
				}
				else
				{
					m_strInt = m_strDec.Substring(0, 1);
					m_strDec = m_strDec.Substring(1);
				}
			}
			else if (strInt != null && strDec == null)
			{
				m_strInt += "0";
			}
			else
			{
				m_strInt += m_strDec.Substring(0, 1);
				m_strDec = m_strDec.Substring(1);
			}
			m_exp--;
			if (m_strInt != null)
			{
				m_strInt = rmvUnnecessaryZeros(m_strInt);
			}
			if (m_strDec != null)
			{
				m_strDec = rmvUnnecessaryZeros("." + m_strDec).Substring(1);
			}
		}

		private void setMN()
		{
			string strInt = m_strInt;
			string text = m_strDec;
			if (strInt != null && text == null)
			{
				n = m_strInt.Length;
				m = cntZeroEnd(m_strInt);
				if (m % 2 != 0)
				{
					m--;
				}
				m_exp += m;
				m_strInt = m_strInt.Substring(0, n - m);
				n = m_strInt.Length;
				if (n % 2 != 0)
				{
					n++;
				}
				m_exp += n - 2;
			}
			else if (strInt == null && text != null)
			{
				n = text.Length;
				m = cntZeroStart(text);
				if (m % 2 != 0)
				{
					m--;
				}
				m_exp -= m;
				if (n - m < 2)
				{
					text += "0";
				}
				m_strInt = text.Substring(m, 2);
				m_strDec = text.Substring(m + 2);
				m_exp -= 2;
				if (m_strInt.StartsWith("0"))
				{
					m_strInt = m_strInt.Substring(1);
				}
			}
			else
			{
				n = strInt.Length;
				if (n % 2 != 0)
				{
					n++;
				}
				m_exp += n - 2;
			}
		}

		private void checkMaxLen()
		{
			int num = ((m_strInt != null) ? m_strInt.Length : 0);
			int num2 = ((m_strDec != null) ? m_strDec.Length : 0);
			if (num + num2 > m_max_len)
			{
				DmError.ThrowDmException(DmErrorDefinition.EC_DATA_OVERFLOW);
			}
		}

		internal byte[] StrToDec(string str, int prec, int scal, bool dmxdec_direct)
		{
			str = str.Trim();
			if (!dmxdec_direct)
			{
				str = checkStr(str, prec, scal);
				if (checkZero(str))
				{
					return fixZero();
				}
			}
			else
			{
				if (prec != 0)
				{
					m_max_len = prec;
				}
				else
				{
					m_max_len = 38;
				}
				if (checkZero2(str))
				{
					return fixZero();
				}
			}
			str = setExp(str);
			str = setSign(str);
			string decInt = rmvUnnecessaryZeros(str);
			setDecInt(decInt);
			processExp();
			setMN();
			checkOverFlow();
			return fixDec(fixFlag(), processStrInt(), processStrDec());
		}

		internal void StrToDec(ref byte[] ret, string str, int prec, int scal, bool dmxdec_direct)
		{
			str = str.Trim();
			if (!dmxdec_direct)
			{
				str = checkStr(str, prec, scal);
				if (checkZero(str))
				{
					fixZero(ref ret);
					return;
				}
			}
			else
			{
				if (prec != 0)
				{
					m_max_len = prec;
				}
				else
				{
					m_max_len = 38;
				}
				if (checkZero2(str))
				{
					fixZero(ref ret);
					return;
				}
			}
			str = setExp(str);
			str = setSign(str);
			string decInt = rmvUnnecessaryZeros(str);
			setDecInt(decInt);
			processExp();
			setMN();
			checkMaxLen();
			checkOverFlow();
			fixDec(ref ret, fixFlag(), processStrInt(), processStrDec());
		}

		internal string decToString(byte[] arr)
		{
			int num = arr.Length;
			bool flag = true;
			short num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			bool flag2 = true;
			int num6 = 0;
			bool flag3 = false;
			bool flag4 = false;
			if (arr.Length == 0 || arr.Length > 21)
			{
				throw new InvalidCastException();
			}
			if (arr[0] == 128)
			{
				return "0";
			}
			StringBuilder stringBuilder = new StringBuilder();
			num2 = arr[0];
			while (num > 0 && arr[num - 1] == 0)
			{
				num--;
			}
			if ((arr[0] & 0x80) == 128)
			{
				num3 = num2 - 193;
				if (num > 1)
				{
					stringBuilder.Append(arr[1] - 1);
				}
				for (num6 = 2; num6 < num; num6++)
				{
					if (arr[num6] - 1 < 10)
					{
						stringBuilder.Append("0");
					}
					stringBuilder.Append(arr[num6] - 1);
				}
				if (arr[1] - 1 < 10)
				{
					flag2 = false;
				}
			}
			else
			{
				flag = false;
				num3 = 62 - num2;
				if (num > 1 && arr[1] != 102)
				{
					stringBuilder.Append(101 - arr[1]);
				}
				for (num6 = 2; num6 < num; num6++)
				{
					if (arr[num6] != 102)
					{
						if (101 - arr[num6] < 10)
						{
							stringBuilder.Append("0");
						}
						stringBuilder.Append(101 - arr[num6]);
					}
				}
				if (101 - arr[1] < 10)
				{
					flag2 = false;
				}
			}
			if (num3 > 0)
			{
				num5 = ((!flag2) ? (num3 * 2 + 1) : (num3 * 2 + 2));
				if (num5 > stringBuilder.Length)
				{
					num4 = num5 - stringBuilder.Length;
					stringBuilder.Append('0', num4);
				}
				else if (num5 < stringBuilder.Length)
				{
					stringBuilder.Insert(num5, '.');
					flag3 = true;
				}
			}
			else if (num3 == 0)
			{
				if (stringBuilder.Length > 2)
				{
					if (flag2)
					{
						stringBuilder.Insert(2, '.');
						num5 = 2;
						flag3 = true;
					}
					else
					{
						stringBuilder.Insert(1, '.');
						num5 = 1;
						flag3 = true;
					}
				}
			}
			else
			{
				num3 *= -1;
				num4 = ((!flag2) ? (num3 * 2 - 1) : (num3 * 2 - 2));
				stringBuilder.Insert(0, "0.");
				flag3 = true;
				num5 = 2;
				for (num6 = 0; num6 < num4; num6++)
				{
					stringBuilder.Insert(2, "0");
				}
				stringBuilder = new StringBuilder(rmvUnnecessaryZeros(stringBuilder.ToString().Substring(0, stringBuilder.Length)));
				stringBuilder.Insert(0, "0");
			}
			if (flag3)
			{
				while (stringBuilder[stringBuilder.Length - 1] == '0' || stringBuilder[stringBuilder.Length - 1] == '.')
				{
					if (stringBuilder[stringBuilder.Length - 1] == '.')
					{
						flag4 = true;
					}
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
					if (flag4)
					{
						break;
					}
				}
			}
			if (flag)
			{
				return stringBuilder.ToString();
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("-");
			stringBuilder2.Append(stringBuilder.ToString());
			return stringBuilder2.ToString();
		}

		public DmXDec Parse(string s)
		{
			return new DmXDec(StrToDec(s, 0, 0, dmxdec_direct: true));
		}

		public override string ToString()
		{
			return decToString(m_b);
		}
	}
}
