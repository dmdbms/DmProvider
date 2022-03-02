using System;

namespace Dm
{
	internal static class DmConvertion
	{
		public static double GetDouble(byte[] b)
		{
			return BitConverter.ToDouble(b, 0);
		}

		public static float GetSingle(byte[] b)
		{
			return BitConverter.ToSingle(b, 0);
		}

		public static byte[] ByteToByteArray(byte b)
		{
			return new byte[1] { b };
		}

		public static byte[] DoubleToByteArray(double d)
		{
			return BitConverter.GetBytes(d);
		}

		public static byte[] FloatToByteArray(float f)
		{
			return BitConverter.GetBytes(f);
		}

		public static byte[] ShortToByteArray(int i)
		{
			byte[] array = new byte[2];
			int num = 0;
			array[num++] = (byte)((uint)i & 0xFFu);
			array[num] = (byte)(i >> 8);
			return array;
		}

		public static byte[] IntToByteArray(long i)
		{
			byte[] array = new byte[4];
			int num = 0;
			array[num++] = (byte)(i & 0xFF);
			array[num++] = (byte)(i >> 8);
			array[num++] = (byte)(i >> 16);
			array[num] = (byte)(i >> 24);
			return array;
		}

		public static byte[] LongToByteArray(long i)
		{
			byte[] array = new byte[8];
			int num = 0;
			array[num++] = (byte)(i & 0xFF);
			array[num++] = (byte)(i >> 8);
			array[num++] = (byte)(i >> 16);
			array[num++] = (byte)(i >> 24);
			array[num++] = (byte)(i >> 32);
			array[num++] = (byte)(i >> 40);
			array[num++] = (byte)(i >> 48);
			array[num++] = (byte)(i >> 56);
			return array;
		}

		public static byte[] DecimalToByteArray(decimal x)
		{
			bool flag = ((x >= 0m) ? true : false);
			string s = x.ToString();
			byte[] bytes = DmConnProperty.encodingMap["default"].GetBytes(s);
			int num = bytes.Length;
			byte[] array = new byte[num + 1];
			Array.Copy(bytes, 0, array, 1, num);
			if (!flag)
			{
				array[0] = 45;
			}
			else
			{
				array[0] = 43;
			}
			return array;
		}

		public static sbyte OneByteToSByte(byte[] byte1)
		{
			return (sbyte)(0xFF & byte1[0]);
		}

		public static short TwoByteToShort(byte[] byte1)
		{
			long num = 0L;
			int num2 = 2;
			for (int i = 0; i < 2; i++)
			{
				num = (0xFF & byte1[--num2]) | (num << 8);
			}
			return (short)(0xFFFFFFFFu & num);
		}

		public static int FourByteToInt(byte[] byte1)
		{
			if (byte1.Length == 2)
			{
				return TwoByteToShort(byte1);
			}
			long num = 0L;
			int num2 = 4;
			for (int i = 0; i < 4; i++)
			{
				num = (0xFF & byte1[--num2]) | (num << 8);
			}
			return (int)(0xFFFFFFFFu & num);
		}

		public static long EightByteToLong(byte[] byte1)
		{
			long num = 0L;
			int num2 = 8;
			for (int i = 0; i < 8; i++)
			{
				num = (0xFF & byte1[--num2]) | (num << 8);
			}
			return num;
		}

		public static byte GetByte(byte[] buf, int off)
		{
			return buf[off];
		}

		public static void SetByte(byte[] buf, int off, byte val)
		{
			buf[off] = val;
		}

		public static short GetShort(byte[] buf, int off)
		{
			byte @byte = GetByte(buf, off);
			return (short)((GetByte(buf, off + 1) << 8) | (0xFF & @byte));
		}

		public static void SetShort(byte[] buf, int off, short val)
		{
			buf[off] = (byte)((uint)val & 0xFFu);
			buf[off + 1] = (byte)((uint)(val >> 8) & 0xFFu);
		}

		public static int GetInt(byte[] buf, int off)
		{
			short @short = GetShort(buf, off);
			return (GetShort(buf, off + 2) << 16) | (0xFFFF & @short);
		}

		public static void SetInt(byte[] buf, int off, int val)
		{
			buf[off++] = (byte)((uint)val & 0xFFu);
			val >>= 8;
			buf[off++] = (byte)((uint)val & 0xFFu);
			val >>= 8;
			buf[off++] = (byte)((uint)val & 0xFFu);
			val >>= 8;
			buf[off++] = (byte)((uint)val & 0xFFu);
			val >>= 8;
		}

		public static long GetLong(byte[] buf, int off)
		{
			long num = GetInt(buf, off);
			return ((long)GetInt(buf, off + 4) << 32) | (0xFFFFFFFFu & num);
		}

		public static byte[] GetBytes(string str, string charsetName)
		{
			byte[] array = null;
			if (str == null)
			{
				return null;
			}
			if (charsetName != null)
			{
				try
				{
					return DmConnProperty.encodingMap[charsetName].GetBytes(str);
				}
				catch (Exception)
				{
					throw new ArgumentOutOfRangeException("charsetName is not support!");
				}
			}
			return DmConnProperty.encodingMap["default"].GetBytes(str);
		}

		public static byte[] GetBytesWithNTS(string str, string charsetName)
		{
			byte[] array = null;
			byte[] array2 = null;
			array2 = GetBytes(str, charsetName);
			array = new byte[array2.Length + 1];
			Array.Copy(array2, array, array2.Length);
			array[array2.Length] = 0;
			return array;
		}

		public static string GetString(byte[] buffer, int offset, int byteLen, string charsetName)
		{
			string result = null;
			if (buffer == null)
			{
				return result;
			}
			if (charsetName == null)
			{
				return DmConnProperty.encodingMap["default"].GetString(buffer, offset, byteLen);
			}
			try
			{
				return DmConnProperty.encodingMap[charsetName].GetString(buffer, offset, byteLen);
			}
			catch (Exception)
			{
				throw new ArgumentOutOfRangeException("charsetName is not support!");
			}
		}

		internal static byte[] GetBytes(byte[] buffer, int offset, int len)
		{
			if (buffer == null)
			{
				return null;
			}
			if (offset < 0 || offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("DmConvertion.GetBytes.offset");
			}
			if (len < 0 || len > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("DmConvertion.GetBytes.len");
			}
			byte[] array = new byte[len];
			Array.Copy(buffer, offset, array, 0, len);
			return array;
		}

		internal static void SetBytes(byte[] buffer, int offset, byte[] data, int dataOffset, int dataLen)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("DmConvertion.GetBytes.buffer");
			}
			if (offset < 0 || offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("DmConvertion.GetBytes.offset");
			}
			if (data == null)
			{
				throw new ArgumentNullException("DmConvertion.GetBytes.data");
			}
			if (dataOffset < 0 || dataOffset > data.Length)
			{
				throw new ArgumentOutOfRangeException("DmConvertion.GetBytes.dataOffset");
			}
			if (dataLen < 0 || dataLen > data.Length - dataOffset || dataLen > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("DmConvertion.GetBytes.dataLen");
			}
			Array.Copy(data, dataOffset, buffer, offset, dataLen);
		}

		internal static void SetBytes(byte[] buffer, int offset, byte[] data)
		{
			SetBytes(buffer, offset, data, 0, data.Length);
		}

		public static byte[] GetBytes(string str, int offset, int len, string charsetName, string destCharsetName)
		{
			byte[] result = null;
			if (str == null)
			{
				return null;
			}
			if (destCharsetName != null)
			{
				try
				{
					result = DmConnProperty.encodingMap[destCharsetName].GetBytes(str);
					return result;
				}
				catch (Exception)
				{
					Console.WriteLine("encoding methods not supported!");
					return result;
				}
			}
			return DmConnProperty.encodingMap["default"].GetBytes(str);
		}

		public static string BytesToHexString(byte[] bs)
		{
			if (bs == null)
			{
				return null;
			}
			if (bs.Length == 0)
			{
				return "";
			}
			string text = "0123456789ABCDEF";
			string text2 = "";
			for (int i = 0; i < bs.Length; i++)
			{
				int index = 0xF & (bs[i] >> 4);
				int index2 = 0xF & bs[i];
				text2 += text[index];
				text2 += text[index2];
			}
			return text2;
		}

		public static string IntToHex(int num)
		{
			string text = "";
			for (int i = 0; i < 2; i++)
			{
				int num2;
				if (i == 0)
				{
					num2 = num & 0xF0;
					num2 >>= 4;
				}
				else
				{
					num2 = num & 0xF;
				}
				if (num2 < 10)
				{
					text += num2;
					continue;
				}
				switch (num2)
				{
				case 10:
					text += "A";
					break;
				case 11:
					text += "B";
					break;
				case 12:
					text += "C";
					break;
				case 13:
					text += "D";
					break;
				case 14:
					text += "E";
					break;
				case 15:
					text += "F";
					break;
				default:
					return "";
				}
			}
			return text;
		}

		public static int BitMoveToRight(long val, int size)
		{
			return (int)(val >> size);
		}

		public static void ArrayMemSet(byte[] x, byte b)
		{
			for (int i = 0; i < x.Length; i++)
			{
				x[i] = b;
			}
		}

		public static void ArrayMemSet(byte[] x, byte b, int startindex, int endindex)
		{
			for (int i = startindex; i < endindex; i++)
			{
				x[i] = b;
			}
		}

		public static int GetBit(byte[] x, int start, int len)
		{
			int num = 0;
			int num2 = start & 7;
			int num3 = start >> 3;
			int num4 = (start + len - 1 >> 3) + 1 - num3;
			int num5 = start >> 3;
			switch (num4)
			{
			case 1:
			{
				byte[] array3 = new byte[4];
				Array.Copy(x, num5, array3, 0, 1);
				num = FourByteToInt(array3);
				break;
			}
			case 2:
			{
				byte[] array2 = new byte[4];
				Array.Copy(x, num5, array2, 0, 1);
				int num9 = FourByteToInt(array2);
				Array.Copy(x, num5 + 1, array2, 0, 1);
				int num10 = FourByteToInt(array2);
				num10 = (int)((num10 << 8) & 0xFFFFFFFFu);
				num = num9 + num10;
				break;
			}
			case 3:
			{
				byte[] array = new byte[4];
				Array.Copy(x, num5, array, 0, 1);
				int num6 = FourByteToInt(array);
				Array.Copy(x, num5 + 1, array, 0, 1);
				int num7 = FourByteToInt(array);
				num7 = (int)((num7 << 8) & 0xFFFFFFFFu);
				Array.Copy(x, num5 + 2, array, 0, 1);
				int num8 = FourByteToInt(array);
				num8 = (int)((num8 << 16) & 0xFFFFFFFFu);
				num = num6 + num7 + num8;
				break;
			}
			}
			int num11 = BitMoveToRight(4294967295L, 32 - len);
			return (num >> num2) & num11;
		}

		public static void SetBit(byte[] x, int start, int len, int data)
		{
			int num = start & 7;
			int num2 = (start + len) & 7;
			int num3 = start >> 3;
			int num4 = (start + len - 1 >> 3) + 1;
			data <<= num;
			byte[] array = IntToByteArray(data);
			for (int i = num3; i < num4; i++)
			{
				if (i == num3)
				{
					int num5 = BitMoveToRight(255L, 8 - num);
					x[i] = (byte)(x[i] & num5);
					x[i] = (byte)(x[i] & (255 >> 8 - num));
					x[i] = (byte)(x[i] | array[i - num3]);
				}
				else if (i == num4 - 1)
				{
					x[i] = (byte)(x[i] & (255 << num2));
					x[i] = (byte)(x[i] | array[i - num3]);
				}
				else
				{
					x[i] = array[i - num3];
				}
			}
		}
	}
}
