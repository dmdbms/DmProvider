using System;
using System.Text;

namespace Dm.util
{
	public class ByteUtil
	{
		public static int setByte(sbyte[] bytes, int offset, sbyte b)
		{
			bytes[offset] = b;
			return 1;
		}

		public static int setShort(sbyte[] bytes, int offset, short s)
		{
			bytes[offset++] = (sbyte)s;
			bytes[offset++] = (sbyte)(s >> 8);
			return 2;
		}

		public static int setInt(sbyte[] bytes, int offset, int i)
		{
			bytes[offset++] = (sbyte)i;
			bytes[offset++] = (sbyte)(i >> 8);
			bytes[offset++] = (sbyte)(i >> 16);
			bytes[offset++] = (sbyte)(i >> 24);
			return 4;
		}

		public static int setLong(sbyte[] bytes, int offset, long l)
		{
			bytes[offset++] = (sbyte)l;
			bytes[offset++] = (sbyte)(l >> 8);
			bytes[offset++] = (sbyte)(l >> 16);
			bytes[offset++] = (sbyte)(l >> 24);
			bytes[offset++] = (sbyte)(l >> 32);
			bytes[offset++] = (sbyte)(l >> 40);
			bytes[offset++] = (sbyte)(l >> 48);
			bytes[offset++] = (sbyte)(l >> 56);
			return 8;
		}

		public static int setFloat(sbyte[] bytes, int offset, float f)
		{
			return setBytes(bytes, offset, (sbyte[])(object)BitConverter.GetBytes(f));
		}

		public static int setDouble(sbyte[] bytes, int offset, double d)
		{
			return setLong(bytes, offset, BitConverter.DoubleToInt64Bits(d));
		}

		public static int setUB1(sbyte[] bytes, int offset, int i)
		{
			bytes[offset] = (sbyte)i;
			return 1;
		}

		public static int setUB2(sbyte[] bytes, int offset, int i)
		{
			bytes[offset++] = (sbyte)i;
			bytes[offset++] = (sbyte)(i >> 8);
			return 2;
		}

		public static int setUB3(sbyte[] bytes, int offset, int i)
		{
			bytes[offset++] = (sbyte)i;
			bytes[offset++] = (sbyte)(i >> 8);
			bytes[offset++] = (sbyte)(i >> 16);
			return 3;
		}

		public static int setUB4(sbyte[] bytes, int offset, long l)
		{
			bytes[offset++] = (sbyte)l;
			bytes[offset++] = (sbyte)(l >> 8);
			bytes[offset++] = (sbyte)(l >> 16);
			bytes[offset++] = (sbyte)(l >> 24);
			return 4;
		}

		public static int setBytes(sbyte[] bytes, int offset, sbyte[] srcBytes)
		{
			Array.Copy(srcBytes, 0, bytes, offset, srcBytes.Length);
			return srcBytes.Length;
		}

		public static int setBytes(sbyte[] bytes, int offset, sbyte[] srcBytes, int srcOffset, int len)
		{
			Array.Copy(srcBytes, srcOffset, bytes, offset, len);
			return len;
		}

		public static sbyte getByte(sbyte[] bytes, int offset)
		{
			return bytes[offset];
		}

		public static short getShort(sbyte[] bytes, int offset)
		{
			return (short)((short)(bytes[offset++] & 0xFF) | (short)((short)(bytes[offset++] & 0xFF) << 8));
		}

		public static int getInt(sbyte[] bytes, int offset)
		{
			return (bytes[offset++] & 0xFF) | ((bytes[offset++] & 0xFF) << 8) | ((bytes[offset++] & 0xFF) << 16) | ((bytes[offset++] & 0xFF) << 24);
		}

		public static long getLong(sbyte[] bytes, int offset)
		{
			return (bytes[offset++] & 0xFF) | ((long)(bytes[offset++] & 0xFF) << 8) | ((long)(bytes[offset++] & 0xFF) << 16) | ((long)(bytes[offset++] & 0xFF) << 24) | ((long)(bytes[offset++] & 0xFF) << 32) | ((long)(bytes[offset++] & 0xFF) << 40) | ((long)(bytes[offset++] & 0xFF) << 48) | ((long)(bytes[offset++] & 0xFF) << 56);
		}

		public static float getFloat(sbyte[] bytes, int offset)
		{
			return (float)BitConverter.Int64BitsToDouble(getInt(bytes, offset));
		}

		public static double getDouble(sbyte[] bytes, int offset)
		{
			return BitConverter.Int64BitsToDouble(getLong(bytes, offset));
		}

		public static int getUB1(sbyte[] bytes, int offset)
		{
			return bytes[offset] & 0xFF;
		}

		public static int getUB2(sbyte[] bytes, int offset)
		{
			return (bytes[offset++] & 0xFF) | ((bytes[offset++] & 0xFF) << 8);
		}

		public static int getUB3(sbyte[] bytes, int offset)
		{
			return (bytes[offset++] & 0xFF) | ((bytes[offset++] & 0xFF) << 8) | ((bytes[offset++] & 0xFF) << 16);
		}

		public static long getUB4(sbyte[] bytes, int offset)
		{
			return (bytes[offset++] & 0xFF) | ((long)(bytes[offset++] & 0xFF) << 8) | ((long)(bytes[offset++] & 0xFF) << 16) | ((long)(bytes[offset++] & 0xFF) << 24);
		}

		public static sbyte[] getBytesWithLength(sbyte[] bytes, int offset)
		{
			int @int = getInt(bytes, offset);
			sbyte[] array = new sbyte[@int];
			Array.Copy(bytes, offset + 4, array, 0, @int);
			return array;
		}

		public static sbyte[] getBytesWithLength2(sbyte[] bytes, int offset)
		{
			int uB = getUB2(bytes, offset);
			sbyte[] array = new sbyte[uB];
			Array.Copy(bytes, offset + 2, array, 0, uB);
			return array;
		}

		public static sbyte[] getBytes(sbyte[] bytes, int offset, int len)
		{
			sbyte[] array = new sbyte[len];
			Array.Copy(bytes, offset, array, 0, len);
			return array;
		}

		public static sbyte[] getBytes(string str, string encoding)
		{
			if (str == null)
			{
				return new sbyte[0];
			}
			try
			{
				return StringUtil.getBytes(str, encoding);
			}
			catch (ArgumentException)
			{
				DmError.ThrowDmException(DmErrorDefinition.EC_CHAR_CODE_NOT_SUPPORTED);
			}
			return new sbyte[0];
		}

		public static string getString(sbyte[] bytes, string encoding)
		{
			return getString(bytes, 0, (bytes != null) ? bytes.Length : 0, encoding);
		}

		public static string getString(sbyte[] bytes, int offset, int len, string encoding)
		{
			string result = null;
			try
			{
				result = Encoding.GetEncoding(encoding).GetString((byte[])(object)bytes, offset, len);
				return result;
			}
			catch (ArgumentException)
			{
				DmError.ThrowDmException(DmErrorDefinition.EC_CHAR_CODE_NOT_SUPPORTED);
				return result;
			}
		}

		public static string getStringWithLength(sbyte[] bytes, int offset, string encoding)
		{
			int @int = getInt(bytes, offset);
			offset += 4;
			return getString(bytes, offset, @int, encoding);
		}

		public static string getStringWithLength2(sbyte[] bytes, int offset, string encoding)
		{
			int uB = getUB2(bytes, offset);
			offset += 2;
			return getString(bytes, offset, uB, encoding);
		}

		public static int indexOf(sbyte[] subByteArray, sbyte[] totalByteArray)
		{
			int result = -1;
			int num = totalByteArray.Length - subByteArray.Length;
			if (num < 0)
			{
				return result;
			}
			for (int i = 0; i <= num; i++)
			{
				int j;
				for (j = 0; j < subByteArray.Length; j++)
				{
					int num2 = subByteArray[j] & 0xFF;
					int num3 = totalByteArray[i + j] & 0xFF;
					if (num2 != num3)
					{
						break;
					}
				}
				if (j == subByteArray.Length)
				{
					result = i;
					break;
				}
			}
			return result;
		}
	}
}
