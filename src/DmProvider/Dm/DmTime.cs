using System;
using Dm.util;

namespace Dm
{
	public class DmTime
	{
		public const int DMDT_HOUR_POS = 0;

		public const int DMDT_HOUR_LEN = 5;

		public const int DMDT_MIN_POS = 5;

		public const int DMDT_MIN_LEN = 6;

		public const int DMDT_SEC_POS = 11;

		public const int DMDT_SEC_LEN = 6;

		public const int DMDT_MSEC_POS = 17;

		public const int DMDT_MSEC_LEN = 20;

		private byte[] m_Times = new byte[7];

		private int prec;

		public DmTime()
		{
		}

		public DmTime(byte[] bt)
		{
			m_Times = bt;
		}

		public DmTime(DateTime dt, int prec, int scale, short mTimeZone)
		{
			dt = MathUtil.Round(dt, scale);
			this.prec = prec;
			byte data = (byte)dt.Hour;
			byte data2 = (byte)dt.Minute;
			byte data3 = (byte)dt.Second;
			DmConvertion.SetBit(m_Times, 0, 5, data);
			DmConvertion.SetBit(m_Times, 5, 6, data2);
			DmConvertion.SetBit(m_Times, 11, 6, data3);
			DmConvertion.SetBit(m_Times, 17, 20, dt.Millisecond * 1000);
			byte[] array = DmConvertion.ShortToByteArray(mTimeZone);
			m_Times[5] = array[0];
			m_Times[6] = array[1];
		}

		public byte[] GetByteArrayValue()
		{
			string timeWithNano = GetTimeWithNano();
			byte[] array = new byte[12];
			char[] separator = ":.".ToCharArray();
			string[] array2 = timeWithNano.Split(separator);
			byte b = byte.Parse(array2[0], DmConst.invariantCulture);
			byte b2 = byte.Parse(array2[1], DmConst.invariantCulture);
			byte b3 = byte.Parse(array2[2], DmConst.invariantCulture);
			array[4] = b;
			array[5] = b2;
			array[6] = b3;
			int num = 0;
			if (array2.Length > 3)
			{
				num = (int)(double.Parse(string.Concat(string.Concat("" + "0", "."), array2[3]), DmConst.invariantCulture) * 1000000.0);
			}
			Array.Copy(DmConvertion.IntToByteArray(num), 0, array, 7, 3);
			return array;
		}

		public byte[] GetTzByteArrayValue()
		{
			string timeWithNano = GetTimeWithNano();
			byte[] array = new byte[12];
			char[] separator = ":.".ToCharArray();
			string[] array2 = timeWithNano.Split(separator);
			byte b = byte.Parse(array2[0], DmConst.invariantCulture);
			byte b2 = byte.Parse(array2[1], DmConst.invariantCulture);
			byte b3 = byte.Parse(array2[2], DmConst.invariantCulture);
			array[4] = b;
			array[5] = b2;
			array[6] = b3;
			int num = 0;
			if (array2.Length > 3)
			{
				num = (int)(double.Parse(string.Concat(string.Concat("" + "0", "."), array2[3]), DmConst.invariantCulture) * 1000000.0);
			}
			Array.Copy(DmConvertion.IntToByteArray(num), 0, array, 7, 3);
			byte[] array3 = DmConvertion.ShortToByteArray(GetTZ());
			array[10] = array3[0];
			array[11] = array3[1];
			return array;
		}

		public byte GetHour()
		{
			return (byte)DmConvertion.GetBit(m_Times, 0, 5);
		}

		public byte GetMinute()
		{
			return (byte)DmConvertion.GetBit(m_Times, 5, 6);
		}

		public byte GetSecond()
		{
			return (byte)DmConvertion.GetBit(m_Times, 11, 6);
		}

		public int GetNano()
		{
			return DmConvertion.GetBit(m_Times, 17, 20);
		}

		public short GetTZ()
		{
			return DmConvertion.GetShort(m_Times, 5);
		}

		public string GetTimeWithoutNano()
		{
			int hour = GetHour();
			int minute = GetMinute();
			int second = GetSecond();
			string text = hour.ToString() ?? "";
			string text2 = minute.ToString() ?? "";
			string text3 = second.ToString() ?? "";
			if (text.Length < 2)
			{
				text = "0" + text;
			}
			if (text2.Length < 2)
			{
				text2 = "0" + text2;
			}
			if (text3.Length < 2)
			{
				text3 = "0" + text3;
			}
			return text + ":" + text2 + ":" + text3;
		}

		public string GetTimeWithNano()
		{
			string text = GetNano().ToString() ?? "";
			text = text.PadLeft(prec, '0');
			return GetTimeWithoutNano() + "." + text;
		}

		public override string ToString()
		{
			if (prec == 0)
			{
				return GetTimeWithoutNano();
			}
			return GetTimeWithNano();
		}

		public static byte[] TimeTzEncodeFast(byte[] dt)
		{
			byte[] array = new byte[7];
			byte b = dt[4];
			byte b2 = dt[5];
			byte b3 = dt[6];
			int num = dt[7] + (dt[8] << 8) + (dt[9] << 16);
			array[0] = (byte)(b | ((b2 & 7) << 5));
			array[1] = (byte)(((b2 & 0x38) >> 3) | ((b3 & 0x1F) << 3));
			array[2] = (byte)(((b3 & 0x20) >> 5) | ((num & 0x7F) << 1));
			array[3] = (byte)((uint)(num >> 7) & 0xFFu);
			array[4] = (byte)((uint)(num >> 15) & 0xFFu);
			array[5] = dt[10];
			array[6] = dt[11];
			return array;
		}

		public static byte[] TimeTzEncodeFast(int[] dt)
		{
			byte[] array = new byte[7];
			int num = dt[3];
			int num2 = dt[4];
			int num3 = dt[5];
			int num4 = dt[6];
			byte[] array2 = DmConvertion.ShortToByteArray((dt[OracleDateFormat.OFFSET_TIMEZONE_SIGN] > 0) ? (-1 * dt[7]) : dt[7]);
			array[0] = (byte)(num | ((num2 & 7) << 5));
			array[1] = (byte)(((num2 & 0x38) >> 3) | ((num3 & 0x1F) << 3));
			array[2] = (byte)(((num3 & 0x20) >> 5) | ((num4 & 0x7F) << 1));
			array[3] = (byte)((uint)(num4 >> 7) & 0xFFu);
			array[4] = (byte)((uint)(num4 >> 15) & 0xFFu);
			array[5] = array2[0];
			array[6] = array2[1];
			return array;
		}

		public static byte[] TimeEncodeFast(int[] dt)
		{
			byte[] array = new byte[5];
			int num = dt[3];
			int num2 = dt[4];
			int num3 = dt[5];
			int num4 = dt[6];
			array[0] = (byte)(num | ((num2 & 7) << 5));
			array[1] = (byte)(((num2 & 0x38) >> 3) | ((num3 & 0x1F) << 3));
			array[2] = (byte)(((num3 & 0x20) >> 5) | ((num4 & 0x7F) << 1));
			array[3] = (byte)((uint)(num4 >> 7) & 0xFFu);
			array[4] = (byte)((uint)(num4 >> 15) & 0xFFu);
			return array;
		}

		public static byte[] TimeEncodeFast(byte[] dt)
		{
			byte[] array = new byte[5];
			byte b = dt[4];
			byte b2 = dt[5];
			byte b3 = dt[6];
			int num = dt[7] + ((dt[8] & 0xFF) << 8) + ((dt[9] & 0xFF) << 16);
			array[0] = (byte)(b | ((b2 & 7) << 5));
			array[1] = (byte)(((b2 & 0x38) >> 3) | ((b3 & 0x1F) << 3));
			array[2] = (byte)(((b3 & 0x20) >> 5) | ((num & 0x7F) << 1));
			array[3] = (byte)((uint)(num >> 7) & 0xFFu);
			array[4] = (byte)((uint)(num >> 15) & 0xFFu);
			return array;
		}
	}
}
