using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dm.util
{
	internal static class StreamUtil
	{
		internal static string readString(TextReader reader, int len)
		{
			try
			{
				if (len == 0)
				{
					return "";
				}
				if (len > 0)
				{
					char[] array = new char[len];
					int num = reader.Read(array, 0, array.Length);
					return (num > 0) ? new string(array, 0, num) : "";
				}
				return readString(reader);
			}
			catch (Exception)
			{
				throw new SystemException("read error");
			}
		}

		internal static string readString(TextReader reader)
		{
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				char[] array = new char[10000];
				int num = 0;
				while ((num = reader.Read(array, 0, array.Length)) > 0)
				{
					stringBuilder.Append(new string(array, 0, num));
				}
				return stringBuilder.ToString();
			}
			catch (Exception)
			{
				throw new SystemException("read error");
			}
		}

		public static byte[] readBytes(Stream stream, int length)
		{
			try
			{
				if (length == 0)
				{
					return new byte[0];
				}
				if (length > 0)
				{
					byte[] array = new byte[length];
					int num = stream.Read(array, 0, array.Length);
					if (num < array.Length)
					{
						byte[] array2 = new byte[num];
						Array.Copy(array, 0, array2, 0, num);
						array = array2;
					}
					return array;
				}
				return readBytes(stream);
			}
			catch (Exception)
			{
				throw new SystemException("read error");
			}
		}

		public static byte[] readBytes(Stream stream)
		{
			try
			{
				int num = 10000;
				byte[] array = new byte[num];
				int num2 = 0;
				List<byte[]> list = new List<byte[]>();
				byte[] result = null;
				while ((num2 = stream.Read(array, 0, array.Length)) > 0)
				{
					if (num2 < array.Length)
					{
						result = new byte[list.Count * num + num2];
						for (int i = 0; i < list.Count; i++)
						{
							Array.Copy(list[i], 0, result, num * i, num);
						}
						Array.Copy(array, 0, result, num * list.Count, num2);
						return result;
					}
					list.Add(array);
					array = new byte[num];
				}
				return result;
			}
			catch (Exception)
			{
				throw new SystemException("read error");
			}
		}
	}
}
