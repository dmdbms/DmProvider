using System;

namespace Dm
{
	public class DmIntervalYM
	{
		private byte[] m_Ym = new byte[12];

		public const byte QUA_Y = 0;

		public const byte QUA_YM = 1;

		public const byte QUA_MO = 2;

		private int m_Prec;

		private int m_Years;

		private int m_Months;

		public DmIntervalYM(byte[] bs, int pre)
		{
			m_Ym = bs;
			m_Prec = pre;
		}

		public DmIntervalYM(byte[] bs)
		{
			m_Ym = bs;
		}

		public DmIntervalYM(string str)
		{
			new DmIntervalYM(str, 2);
		}

		public DmIntervalYM(string str, int pre)
		{
			string[] array = str.Split(' ');
			int t = 1;
			m_Prec = pre;
			if (array[2].ToUpper().StartsWith("YEAR"))
			{
				t = ((array.Length > 3) ? 1 : 0);
			}
			else if (array[2].ToUpper().StartsWith("MONTH"))
			{
				t = 2;
			}
			ConvertStrToBs(array[1], t);
		}

		public int GetYear()
		{
			byte[] array = new byte[4];
			Array.Copy(m_Ym, 0, array, 0, 4);
			return DmConvertion.FourByteToInt(array);
		}

		public int GetMonth()
		{
			byte[] array = new byte[4];
			Array.Copy(m_Ym, 4, array, 0, 4);
			return DmConvertion.FourByteToInt(array);
		}

		public byte GetYMType()
		{
			return m_Ym[9];
		}

		public int GetLoadPrec()
		{
			return m_Prec;
		}

		public int GetPrec()
		{
			return (GetYMType() << 8) + (m_Prec << 4);
		}

		public byte[] GetByteArrayValue()
		{
			return m_Ym;
		}

		public int Abs(int i)
		{
			if (i < 0)
			{
				i = -i;
			}
			return i;
		}

		public string GetYMString()
		{
			string text = "INTERVAL '";
			byte yMType = GetYMType();
			int loadPrec = GetLoadPrec();
			int num = 0;
			switch (yMType)
			{
			case 0:
			{
				string text3 = GetYear().ToString();
				num = text3.Length - loadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text3 + "' YEAR";
				break;
			}
			case 1:
			{
				string text3 = GetYear().ToString();
				num = text3.Length - loadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text3 + "-";
				string text2 = Abs(GetMonth()).ToString();
				num = text2.Length - loadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text2 + "' YEAR TO MONTH";
				break;
			}
			case 2:
			{
				string text2 = GetMonth().ToString();
				num = text2.Length - loadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text2 + "' MONTH";
				break;
			}
			}
			return text;
		}

		public override string ToString()
		{
			return GetYMString();
		}

		private void ConvertStrToBs(string str, int t)
		{
			str = ((str.IndexOf("'") >= 0) ? str.Substring(1, str.Length - 2) : str);
			string[] array = null;
			array = ((t != 0 && t != 2) ? str.Split('-') : new string[1] { str });
			int num = 0;
			int num2 = 0;
			switch (t)
			{
			case 0:
				num = int.Parse(array[0], DmConst.invariantCulture);
				if ((double)num > Math.Pow(10.0, m_Prec) - 1.0 || (double)num < 1.0 - Math.Pow(10.0, m_Prec))
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_TIME_INTERVAL);
				}
				Array.Copy(DmConvertion.IntToByteArray(num), 0, m_Ym, 0, 4);
				m_Years = num;
				m_Ym[9] = 0;
				m_Ym[8] = (byte)((uint)(m_Prec << 4) | 0u);
				break;
			case 1:
				num = int.Parse(array[0], DmConst.invariantCulture);
				if ((double)num > Math.Pow(10.0, m_Prec) - 1.0 || (double)num < 1.0 - Math.Pow(10.0, m_Prec))
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_TIME_INTERVAL);
				}
				num2 = int.Parse(array[1], DmConst.invariantCulture);
				Array.Copy(DmConvertion.IntToByteArray(num), 0, m_Ym, 0, 4);
				Array.Copy(DmConvertion.IntToByteArray(num2), 0, m_Ym, 4, 4);
				m_Years = num;
				m_Months = num2;
				m_Ym[9] = 1;
				m_Ym[8] = (byte)((uint)(m_Prec << 4) | 0u);
				break;
			case 2:
				num2 = int.Parse(array[0], DmConst.invariantCulture);
				if ((double)num2 > Math.Pow(10.0, m_Prec) - 1.0 || (double)num2 < 1.0 - Math.Pow(10.0, m_Prec))
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_TIME_INTERVAL);
				}
				Array.Copy(DmConvertion.IntToByteArray(num2), 0, m_Ym, 4, 4);
				m_Months = num2;
				m_Ym[9] = 2;
				m_Ym[8] = (byte)((uint)(m_Prec << 4) | 0u);
				break;
			}
		}

		public byte[] ConvertStrToBs()
		{
			byte[] array = new byte[12];
			int prec = GetPrec();
			Array.Copy(DmConvertion.IntToByteArray(m_Years), 0, array, 0, 4);
			Array.Copy(DmConvertion.IntToByteArray(m_Months), 0, array, 4, 4);
			Array.Copy(DmConvertion.IntToByteArray(prec), 0, array, 8, 4);
			return array;
		}

		public void Clear()
		{
			m_Years = 0;
			m_Months = 0;
		}
	}
}
