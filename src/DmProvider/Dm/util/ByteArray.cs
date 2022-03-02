using System;
using System.Collections.Generic;
using System.IO;
using Dm.net.buffer;

namespace Dm.util
{
	internal class ByteArray
	{
		internal class Element
		{
			private readonly ByteArray outerInstance;

			internal sbyte[] bytes;

			internal int start;

			internal int length;

			public Element(ByteArray outerInstance, sbyte[] bytes, int offset, int len)
			{
				this.outerInstance = outerInstance;
				this.bytes = bytes;
				start = offset;
				length = len;
			}

			public virtual int writeBytes(Dm.net.buffer.Buffer buffer, int len)
			{
				len = ((length > len) ? len : length);
				buffer.writeBytes(bytes, start, len);
				start += len;
				length -= len;
				return len;
			}

			public virtual int writeBytes(Stream @out, int len)
			{
				len = ((length > len) ? len : length);
				@out.Write((byte[])(object)bytes, start, len);
				start += len;
				length -= len;
				return len;
			}

			public virtual int writeBytes(sbyte[] buffer, int offset, int len)
			{
				len = ((length > len) ? len : length);
				Array.Copy(bytes, start, buffer, offset, len);
				start += len;
				length -= len;
				return len;
			}

			public virtual sbyte getByte(int offset)
			{
				return bytes[start + offset];
			}
		}

		private LinkedList<Element> byteArrayList = new LinkedList<Element>();

		private Element current;

		private int length_Renamed;

		public virtual int length()
		{
			return length_Renamed;
		}

		public virtual void putBytes(sbyte[] bytes, int offset, int len)
		{
			if (len != 0)
			{
				Element value = new Element(this, bytes, offset, len);
				if (current == null)
				{
					current = value;
				}
				else
				{
					byteArrayList.AddLast(value);
				}
				length_Renamed += len;
			}
		}

		public virtual int writeBytes(Dm.net.buffer.Buffer buffer, int len)
		{
			int num = 0;
			int num2 = 0;
			while (num < len && current != null)
			{
				num2 = current.writeBytes(buffer, len - num);
				if (current.length == 0)
				{
					next();
				}
				num += num2;
				length_Renamed -= num2;
			}
			return num;
		}

		public virtual int writeBytes(Stream @out, int len)
		{
			int num = 0;
			int num2 = 0;
			while (num < len && current != null)
			{
				num2 = current.writeBytes(@out, len - num);
				if (current.length == 0)
				{
					next();
				}
				num += num2;
				length_Renamed -= num2;
			}
			return num;
		}

		public virtual int writeBytes(sbyte[] buffer, int offset, int len)
		{
			int num = 0;
			int num2 = 0;
			while (num < len && current != null)
			{
				num2 = current.writeBytes(buffer, offset, len - num);
				if (current.length == 0)
				{
					next();
				}
				num += num2;
				length_Renamed -= num2;
				offset += num2;
			}
			return num;
		}

		public virtual sbyte getByte(int offset)
		{
			int num = offset;
			Element value = current;
			while (num > 0 && value != null)
			{
				if (value.length != 0)
				{
					if (num <= value.length - 1)
					{
						break;
					}
					num -= value.length;
					value = byteArrayList.First!.Value;
				}
			}
			return value.getByte(num);
		}

		public virtual void append(ByteArray buffer)
		{
			if (buffer.length() != 0)
			{
				Element element = null;
				while ((element = buffer.current) != null)
				{
					addElement(element);
					buffer.next();
				}
				buffer.length_Renamed = 0;
			}
		}

		private void addElement(Element e)
		{
			if (e.length != 0)
			{
				if (current == null)
				{
					current = e;
				}
				else
				{
					byteArrayList.AddLast(e);
				}
				length_Renamed += e.length;
			}
		}

		private void next()
		{
			current = byteArrayList.First!.Value;
		}

		public virtual sbyte[] toBytes()
		{
			sbyte[] array = new sbyte[length_Renamed];
			Element value = current;
			int num = 0;
			int num2 = array.Length;
			int num3 = 0;
			while (value != null)
			{
				if (value.length > 0)
				{
					num3 = ((num2 > value.length) ? value.length : num2);
					Array.Copy(value.bytes, value.start, array, num, num3);
					num += num3;
					num2 -= num3;
				}
				value = byteArrayList.First!.Value;
			}
			return array;
		}
	}
}
