using System;

namespace Dm
{
	public class DmIntervalDT
	{
		public const byte QUA_Y = 0;

		public const byte QUA_YM = 1;

		public const byte QUA_MO = 2;

		public const byte QUA_D = 3;

		public const byte QUA_DH = 4;

		public const byte QUA_DHM = 5;

		public const byte QUA_DHMS = 6;

		public const byte QUA_H = 7;

		public const byte QUA_HM = 8;

		public const byte QUA_HMS = 9;

		public const byte QUA_M = 10;

		public const byte QUA_MS = 11;

		public const byte QUA_S = 12;

		private byte[] m_Dt = new byte[24];

		internal int LoadPrec { get; set; }

		internal int SecPrec { get; set; }

		internal int Days { get; set; }

		internal int Hours { get; set; }

		internal int Minutes { get; set; }

		internal int Seconds { get; set; }

		internal int Fraction { get; set; }

		internal byte Qualifier { get; set; }

		internal int Prec => (Qualifier << 8) + (LoadPrec << 4) + SecPrec;

		public DmIntervalDT(int days, int hours, int minutes, int seconds, int millseconds)
		{
			Days = days;
			Hours = hours;
			Minutes = minutes;
			Seconds = seconds;
			Fraction = millseconds * 1000;
			SecPrec = 0;
			LoadPrec = 2;
			Qualifier = 6;
		}

		public DmIntervalDT(byte[] interval, int loadPre = 2, int secPre = 6)
		{
			m_Dt = interval;
			LoadPrec = loadPre;
			SecPrec = secPre;
			GetDay();
			GetHour();
			GetMinute();
			GetSecond();
			GetNano();
			GetDTType();
		}

		public DmIntervalDT(string str, int loadPre = 2, int secPre = 6)
		{
			string[] array = str.Split(' ');
			string text = null;
			LoadPrec = loadPre;
			SecPrec = secPre;
			switch (array.Length)
			{
			case 3:
			{
				int num5 = array[2].IndexOf("(");
				text = ((num5 >= 0) ? array[2].Substring(0, num5) : array[2]);
				if (text.ToLower().Equals("day"))
				{
					SetDay(array[1]);
				}
				else if (text.ToLower().Equals("hour"))
				{
					SetHour(array[1]);
				}
				else if (text.ToLower().Equals("minute"))
				{
					SetMinute(array[1]);
				}
				else if (text.ToLower().Equals("second"))
				{
					SetSecond(array[1]);
				}
				else
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_TIME_INTERVAL);
				}
				break;
			}
			case 5:
			{
				int num3 = array[2].IndexOf("(");
				int num4 = array[4].IndexOf("(");
				text = ((num3 >= 0) ? array[2].Substring(0, num3) : array[2]) + ((num4 >= 0) ? array[4].Substring(0, num4) : array[4]);
				if (text.ToLower().Equals("hoursecond"))
				{
					SetHourToSecond(array[1]);
				}
				else if (text.ToLower().Equals("hourminute"))
				{
					SetHourToMinute(array[1]);
				}
				else if (text.ToLower().Equals("minutesecond"))
				{
					SetMinuteToSecond(array[1]);
				}
				else
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_TIME_INTERVAL);
				}
				break;
			}
			case 6:
			{
				int num = array[3].IndexOf("(");
				int num2 = array[5].IndexOf("(");
				text = ((num >= 0) ? array[3].Substring(0, num) : array[3]) + ((num2 >= 0) ? array[5].Substring(0, num2) : array[5]);
				if (text.ToLower().Equals("dayhour"))
				{
					SetDayToHour(array[1] + " " + array[2]);
				}
				else if (text.ToLower().Equals("dayminute"))
				{
					SetDayToMinute(array[1] + " " + array[2]);
				}
				else if (text.ToLower().Equals("daysecond"))
				{
					SetDayToSecond(array[1] + " " + array[2]);
				}
				else
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_TIME_INTERVAL);
				}
				break;
			}
			default:
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_TIME_INTERVAL);
				break;
			}
			m_Dt[21] = Qualifier;
		}

		private void SetDay(string value)
		{
			Qualifier = 3;
			value = ((value.IndexOf("'") >= 0) ? value.Substring(1, value.Length - 2) : value);
			int num = int.Parse(value, DmConst.invariantCulture);
			Array.Copy(DmConvertion.IntToByteArray(num), 0, m_Dt, 0, 4);
			Days = num;
		}

		private void SetHour(string value)
		{
			Qualifier = 7;
			value = ((value.IndexOf("'") >= 0) ? value.Substring(1, value.Length - 2) : value);
			int num = int.Parse(value, DmConst.invariantCulture);
			Array.Copy(DmConvertion.IntToByteArray(num), 0, m_Dt, 4, 4);
			Hours = num;
		}

		private void SetMinute(string value)
		{
			Qualifier = 10;
			value = ((value.IndexOf("'") >= 0) ? value.Substring(1, value.Length - 2) : value);
			int num = int.Parse(value);
			Array.Copy(DmConvertion.IntToByteArray(num), 0, m_Dt, 8, 4);
			Minutes = num;
		}

		private void SetSecond(string value)
		{
			Qualifier = 12;
			value = ((value.IndexOf("'") >= 0) ? value.Substring(1, value.Length - 2) : value);
			string[] array = value.Split('.');
			int num = int.Parse(array[0], DmConst.invariantCulture);
			int num2 = 0;
			if (array.Length > 1)
			{
				double num3 = double.Parse(string.Concat(string.Concat("" + "0", "."), array[1]), DmConst.invariantCulture);
				int num4 = (int)Math.Pow(10.0, SecPrec);
				num2 = (int)(num3 * (double)num4);
			}
			Array.Copy(DmConvertion.IntToByteArray(num), 0, m_Dt, 12, 4);
			Array.Copy(DmConvertion.IntToByteArray(num2), 0, m_Dt, 16, 4);
			Seconds = num;
			Fraction = num2;
		}

		private void SetHourToSecond(string value)
		{
			Qualifier = 9;
			value = ((value.IndexOf("'") >= 0) ? value.Substring(1, value.Length - 2) : value);
			string[] array = value.Split(':', '.');
			int num = int.Parse(array[0], DmConst.invariantCulture);
			int num2 = int.Parse(array[1], DmConst.invariantCulture);
			int num3 = int.Parse(array[2], DmConst.invariantCulture);
			int num4 = 0;
			if (array.Length > 3)
			{
				double num5 = double.Parse(string.Concat(string.Concat("" + "0", "."), array[3]), DmConst.invariantCulture);
				int num6 = (int)Math.Pow(10.0, SecPrec);
				num4 = (int)(num5 * (double)num6);
			}
			Array.Copy(DmConvertion.IntToByteArray(num), 0, m_Dt, 4, 4);
			Array.Copy(DmConvertion.IntToByteArray(num2), 0, m_Dt, 8, 4);
			Array.Copy(DmConvertion.IntToByteArray(num3), 0, m_Dt, 12, 4);
			Array.Copy(DmConvertion.IntToByteArray(num4), 0, m_Dt, 16, 4);
			Hours = num;
			Minutes = num2;
			Seconds = num3;
			Fraction = num4;
		}

		private void SetHourToMinute(string value)
		{
			Qualifier = 8;
			value = ((value.IndexOf("'") >= 0) ? value.Substring(1, value.Length - 2) : value);
			string[] array = value.Split(':');
			int num = int.Parse(array[0], DmConst.invariantCulture);
			int num2 = int.Parse(array[1], DmConst.invariantCulture);
			Array.Copy(DmConvertion.IntToByteArray(num), 0, m_Dt, 4, 4);
			Array.Copy(DmConvertion.IntToByteArray(num2), 0, m_Dt, 8, 4);
			Hours = num;
			Minutes = num2;
		}

		private void SetMinuteToSecond(string value)
		{
			Qualifier = 11;
			value = ((value.IndexOf("'") >= 0) ? value.Substring(1, value.Length - 2) : value);
			string[] array = value.Split(':', '.');
			int num = int.Parse(array[0], DmConst.invariantCulture);
			int num2 = int.Parse(array[1], DmConst.invariantCulture);
			int num3 = 0;
			if (array.Length > 2)
			{
				double num4 = double.Parse(string.Concat(string.Concat("" + "0", "."), array[2]), DmConst.invariantCulture);
				int num5 = (int)Math.Pow(10.0, SecPrec);
				num3 = (int)(num4 * (double)num5);
			}
			Array.Copy(DmConvertion.IntToByteArray(num), 0, m_Dt, 8, 4);
			Array.Copy(DmConvertion.IntToByteArray(num2), 0, m_Dt, 12, 4);
			Array.Copy(DmConvertion.IntToByteArray(num3), 0, m_Dt, 16, 4);
			Minutes = num;
			Seconds = num2;
			Fraction = num3;
		}

		private void SetDayToHour(string value)
		{
			Qualifier = 4;
			value = ((value.IndexOf("'") >= 0) ? value.Substring(1, value.Length - 2) : value);
			string[] array = value.Split(' ');
			int num = int.Parse(array[0], DmConst.invariantCulture);
			int num2 = int.Parse(array[1], DmConst.invariantCulture);
			Array.Copy(DmConvertion.IntToByteArray(num), 0, m_Dt, 0, 4);
			Array.Copy(DmConvertion.IntToByteArray(num2), 0, m_Dt, 4, 4);
			Days = num;
			Hours = num2;
		}

		private void SetDayToMinute(string value)
		{
			Qualifier = 5;
			value = ((value.IndexOf("'") >= 0) ? value.Substring(1, value.Length - 2) : value);
			string[] array = value.Split(':', ' ');
			int num = int.Parse(array[0], DmConst.invariantCulture);
			int num2 = int.Parse(array[1], DmConst.invariantCulture);
			int num3 = int.Parse(array[2], DmConst.invariantCulture);
			Array.Copy(DmConvertion.IntToByteArray(num), 0, m_Dt, 0, 4);
			Array.Copy(DmConvertion.IntToByteArray(num2), 0, m_Dt, 4, 4);
			Array.Copy(DmConvertion.IntToByteArray(num3), 0, m_Dt, 8, 4);
			Days = num;
			Hours = num2;
			Minutes = num3;
		}

		private void SetDayToSecond(string value)
		{
			Qualifier = 6;
			value = ((value.IndexOf("'") >= 0) ? value.Substring(1, value.Length - 2) : value);
			string[] array = value.Split(':', '.', ' ');
			int num = int.Parse(array[0], DmConst.invariantCulture);
			int num2 = int.Parse(array[1], DmConst.invariantCulture);
			int num3 = int.Parse(array[2], DmConst.invariantCulture);
			int num4 = int.Parse(array[3], DmConst.invariantCulture);
			int num5 = 0;
			if (array.Length > 4)
			{
				double num6 = double.Parse(string.Concat(string.Concat("" + "0", "."), array[4]), DmConst.invariantCulture);
				int num7 = (int)Math.Pow(10.0, SecPrec);
				num5 = (int)(num6 * (double)num7);
			}
			Array.Copy(DmConvertion.IntToByteArray(num), 0, m_Dt, 0, 4);
			Array.Copy(DmConvertion.IntToByteArray(num2), 0, m_Dt, 4, 4);
			Array.Copy(DmConvertion.IntToByteArray(num3), 0, m_Dt, 8, 4);
			Array.Copy(DmConvertion.IntToByteArray(num4), 0, m_Dt, 12, 4);
			Array.Copy(DmConvertion.IntToByteArray(num5), 0, m_Dt, 16, 4);
			Days = num;
			Hours = num2;
			Minutes = num3;
			Seconds = num4;
			Fraction = num5;
		}

		public int GetDay()
		{
			byte[] array = new byte[4];
			Array.Copy(m_Dt, 0, array, 0, 4);
			return Days = DmConvertion.FourByteToInt(array);
		}

		public int GetHour()
		{
			byte[] array = new byte[4];
			Array.Copy(m_Dt, 4, array, 0, 4);
			return Hours = DmConvertion.FourByteToInt(array);
		}

		public int GetMinute()
		{
			byte[] array = new byte[4];
			Array.Copy(m_Dt, 8, array, 0, 4);
			return Minutes = DmConvertion.FourByteToInt(array);
		}

		public int GetSecond()
		{
			byte[] array = new byte[4];
			Array.Copy(m_Dt, 12, array, 0, 4);
			return Seconds = DmConvertion.FourByteToInt(array);
		}

		public int GetNano()
		{
			byte[] array = new byte[4];
			Array.Copy(m_Dt, 16, array, 0, 4);
			return Fraction = DmConvertion.FourByteToInt(array);
		}

		public byte GetDTType()
		{
			Qualifier = m_Dt[21];
			return Qualifier;
		}

		public int Abs(int i)
		{
			if (i < 0)
			{
				i = -i;
			}
			return i;
		}

		public string GetDTString()
		{
			string text = "INTERVAL '";
			Qualifier = GetDTType();
			int num = 0;
			switch (Qualifier)
			{
			case 3:
			{
				string text6 = GetDay().ToString();
				num = text6.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text6 + "' DAY";
				break;
			}
			case 4:
			{
				string text6 = GetDay().ToString();
				num = text6.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text6 + " ";
				string text4 = Abs(GetHour()).ToString();
				num = text4.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text4 + "' DAY TO HOUR";
				break;
			}
			case 5:
			{
				string text6 = GetDay().ToString();
				num = text6.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text6 + " ";
				string text4 = Abs(GetHour()).ToString();
				num = text4.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text4 + ":";
				string text5 = Abs(GetMinute()).ToString();
				num = text5.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text5 + "' DAY TO MINUTE";
				break;
			}
			case 6:
			{
				string text6 = GetDay().ToString();
				num = text6.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text6 + " ";
				string text4 = Abs(GetHour()).ToString();
				num = text4.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text4 + ":";
				string text5 = Abs(GetMinute()).ToString();
				num = text5.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text5 + ":";
				string text2 = Abs(GetSecond()).ToString();
				num = text2.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text2 + ".";
				string text3 = Abs(GetNano()).ToString();
				num = text3.Length - SecPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text3 + "' DAY TO SECOND";
				break;
			}
			case 7:
			{
				string text4 = GetHour().ToString();
				num = text4.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text4 + "' HOUR";
				break;
			}
			case 8:
			{
				string text4 = GetHour().ToString();
				num = text4.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text4 + ":";
				string text5 = Abs(GetMinute()).ToString();
				num = text5.Length - LoadPrec;
				while (num++ > 0)
				{
					text += "0";
				}
				text = text + text5 + "' HOUR TO MINUTE";
				break;
			}
			case 9:
			{
				string text4 = GetHour().ToString();
				num = text4.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text4 + ":";
				string text5 = Abs(GetMinute()).ToString();
				num = text5.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text5 + ":";
				string text2 = Abs(GetSecond()).ToString();
				num = text2.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text2 + ".";
				string text3 = Abs(GetNano()).ToString();
				num = text3.Length - SecPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text3 + "' HOUR TO SECOND";
				break;
			}
			case 10:
			{
				string text5 = GetMinute().ToString();
				num = text5.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text5 + "' MINUTE";
				break;
			}
			case 11:
			{
				string text5 = GetMinute().ToString();
				num = text5.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text5 + ":";
				string text2 = Abs(GetSecond()).ToString();
				num = text2.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text2 + ".";
				string text3 = Abs(GetNano()).ToString();
				num = text3.Length - SecPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text3 + "' MINUTE TO SECOND";
				break;
			}
			case 12:
			{
				string text2 = GetSecond().ToString();
				num = text2.Length - LoadPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text2 + ".";
				string text3 = Abs(GetNano()).ToString();
				num = text3.Length - SecPrec;
				while (num++ < 0)
				{
					text += "0";
				}
				text = text + text3 + "' SECOND";
				break;
			}
			}
			return text;
		}

		public override string ToString()
		{
			return GetDTString();
		}

		public byte[] ConvertStrToBs()
		{
			byte[] array = new byte[24];
			Array.Copy(DmConvertion.IntToByteArray(Days), 0, array, 0, 4);
			Array.Copy(DmConvertion.IntToByteArray(Hours), 0, array, 4, 4);
			Array.Copy(DmConvertion.IntToByteArray(Minutes), 0, array, 8, 4);
			Array.Copy(DmConvertion.IntToByteArray(Seconds), 0, array, 12, 4);
			Array.Copy(DmConvertion.IntToByteArray(Fraction), 0, array, 16, 4);
			Array.Copy(DmConvertion.IntToByteArray(Prec), 0, array, 20, 4);
			return array;
		}

		public void Clear()
		{
			Days = 0;
			Hours = 0;
			Minutes = 0;
			Seconds = 0;
			Fraction = 0;
		}
	}
}
