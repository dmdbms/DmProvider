using System;
using System.IO;
using System.Text;
using Dm.util;

namespace Dm.net.buffer
{
	public class ByteArrayBuffer : Buffer
	{
		public class Node
		{
			public sbyte[] bytes;

			public int offset;

			public int size;

			public object info;

			public Node prev;

			public Node next;

			internal Node(int capacity)
			{
				bytes = new sbyte[capacity];
				offset = 0;
				size = 0;
				prev = null;
				next = null;
				info = null;
			}

			internal Node(sbyte[] bytes, int offset, int length)
			{
				this.bytes = bytes;
				this.offset = offset;
				size = offset + length;
				prev = null;
				next = null;
				info = null;
			}

			public override string ToString()
			{
				return "capacity=" + bytes.Length + " offset=" + offset + " size=" + size + " info=" + info;
			}

			internal virtual void clear()
			{
				offset = 0;
				size = 0;
			}

			internal virtual int leave(bool write)
			{
				if (!write)
				{
					return size - offset;
				}
				return bytes.Length - size;
			}

			internal virtual void flip()
			{
				offset = 0;
			}

			internal virtual int fill(int len)
			{
				int num = leave(write: true);
				if (len > num)
				{
					len = num;
				}
				Arrays.Fill(bytes, offset, offset + len, (sbyte)0);
				offset += len;
				size += len;
				return len;
			}

			internal virtual int skip(int len)
			{
				int num = leave(write: false);
				if (len > num)
				{
					len = num;
				}
				offset += len;
				return len;
			}

			internal virtual int write(sbyte[] srcBytes, int srcOffset, int len)
			{
				int num = leave(write: true);
				if (len > num)
				{
					len = num;
				}
				Array.Copy(srcBytes, srcOffset, bytes, offset, len);
				offset += len;
				size += len;
				return len;
			}

			internal virtual int set(int offset, sbyte[] srcBytes, int srcOffset, int len)
			{
				int num = size - offset;
				if (len > num)
				{
					len = num;
				}
				Array.Copy(srcBytes, srcOffset, bytes, offset, len);
				return len;
			}

			internal virtual int read(sbyte[] destBytes, int destOffset, int len)
			{
				int num = leave(write: false);
				if (len > num)
				{
					len = num;
				}
				Array.Copy(bytes, offset, destBytes, destOffset, len);
				offset += len;
				return len;
			}

			internal virtual int get(int offset, sbyte[] destBytes, int destOffset, int len)
			{
				int num = size - offset;
				if (len > num)
				{
					len = num;
				}
				Array.Copy(bytes, offset, destBytes, destOffset, len);
				return len;
			}

			internal virtual int load(Stream @is, int len)
			{
				int num = leave(write: true);
				if (len > num)
				{
					len = num;
				}
				int i;
				int num2;
				for (i = 0; i < len; i += num2)
				{
					num2 = @is.Read((byte[])(object)bytes, offset + i, len - i);
					if (num2 == 0)
					{
						throw new IOException("已读完");
					}
				}
				offset += i;
				size += i;
				return i;
			}

			internal virtual int flush(Stream @out, int off, int len)
			{
				int num = size - off;
				len = ((len > num) ? num : len);
				@out.Write((byte[])(object)bytes, off, len);
				return len;
			}
		}

		public Node firstNode;

		public Node lastNode;

		public Node currentNode;

		public int count;

		internal ByteArrayBuffer(int capacity)
		{
			currentNode = (lastNode = (firstNode = new Node(capacity)));
			count = 1;
		}

		internal ByteArrayBuffer(sbyte[] bytes, int offset, int length)
		{
			currentNode = (lastNode = (firstNode = new Node(bytes, offset, length)));
			count = 1;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("[" + count + "]: ");
			for (Node next = firstNode; next != null; next = next.next)
			{
				if (next != firstNode)
				{
					stringBuilder.Append(" <-> ");
				}
				if (next == currentNode)
				{
					stringBuilder.Append("*");
				}
				stringBuilder.Append("{");
				stringBuilder.Append(next);
				stringBuilder.Append("}");
			}
			return stringBuilder.ToString();
		}

		public override void clear()
		{
			for (Node next = firstNode; next != null; next = next.next)
			{
				next.clear();
			}
			currentNode = firstNode;
		}

		public override void truncate(int offset)
		{
			Node node = locate(offset);
			offset = ((int?)node.info).Value;
			node.offset = offset;
			node.size = offset;
			currentNode = node;
			for (node = node.next; node != null; node = node.next)
			{
				node.clear();
			}
		}

		public override int length()
		{
			int num = 0;
			for (Node next = firstNode; next != null; next = next.next)
			{
				num += next.size;
			}
			return num;
		}

		public override void flip()
		{
			for (Node next = firstNode; next != null; next = next.next)
			{
				next.flip();
			}
			currentNode = firstNode;
		}

		public override void fill(int len)
		{
			while (len > 0)
			{
				len -= currentNode.fill(len);
				if (len != 0)
				{
					currentNode = currentNode.next;
					if (currentNode == null)
					{
						extend(len);
					}
					continue;
				}
				break;
			}
		}

		public override void skip(int len)
		{
			while (len > 0)
			{
				len -= currentNode.skip(len);
				if (len != 0)
				{
					currentNode = currentNode.next;
					if (currentNode == null)
					{
						throw new Exception("buffer index out of range");
					}
					continue;
				}
				break;
			}
		}

		public override long offset()
		{
			int num = 0;
			for (Node next = firstNode; next != null; next = next.next)
			{
				num += next.offset;
				if (next == currentNode)
				{
					break;
				}
			}
			return num;
		}

		public virtual Node locate(int offset)
		{
			Node next = firstNode;
			while (next != null && offset >= next.size)
			{
				offset -= next.size;
				next = next.next;
			}
			if (next == null)
			{
				throw new Exception("buffer index out of range");
			}
			next.info = offset;
			return next;
		}

		private void extend(int len)
		{
			int num = 2 * lastNode.bytes.Length;
			len = ((len > num) ? len : num);
			currentNode = new Node(len);
			currentNode.prev = lastNode;
			lastNode.next = currentNode;
			lastNode = currentNode;
			count++;
		}

		private int write(long val, int len)
		{
			int result = 0;
			while (len > 0)
			{
				int num = currentNode.leave(write: true);
				if (len <= num)
				{
					for (int i = 0; i < len; i++)
					{
						currentNode.bytes[currentNode.offset++] = (sbyte)(val >> result++ * 8);
						currentNode.size++;
					}
					break;
				}
				for (int j = 0; j < num; j++)
				{
					currentNode.bytes[currentNode.offset++] = (sbyte)(val >> result++ * 8);
					currentNode.size++;
				}
				len -= num;
				currentNode = currentNode.next;
				if (currentNode == null)
				{
					extend(len);
				}
			}
			return result;
		}

		public override int writeByte(sbyte b)
		{
			if (currentNode.leave(write: true) >= 1)
			{
				ByteUtil.setByte(currentNode.bytes, currentNode.offset, b);
				currentNode.offset++;
				currentNode.size++;
				return 1;
			}
			return write(b, 1);
		}

		public override int writeShort(short s)
		{
			if (currentNode.leave(write: true) >= 2)
			{
				ByteUtil.setShort(currentNode.bytes, currentNode.offset, s);
				currentNode.size += 2;
				currentNode.offset += 2;
				return 2;
			}
			return write(s, 2);
		}

		public override int writeInt(int i)
		{
			if (currentNode.leave(write: true) >= 4)
			{
				ByteUtil.setInt(currentNode.bytes, currentNode.offset, i);
				currentNode.size += 4;
				currentNode.offset += 4;
				return 4;
			}
			return write(i, 4);
		}

		public override int writeUB1(int i)
		{
			if (currentNode.leave(write: true) >= 1)
			{
				ByteUtil.setUB1(currentNode.bytes, currentNode.offset, i);
				currentNode.offset++;
				currentNode.size++;
				return 1;
			}
			return write(i, 1);
		}

		public override int writeUB2(int i)
		{
			if (currentNode.leave(write: true) >= 2)
			{
				ByteUtil.setUB2(currentNode.bytes, currentNode.offset, i);
				currentNode.size += 2;
				currentNode.offset += 2;
				return 2;
			}
			return write(i, 2);
		}

		public override int writeUB3(int i)
		{
			if (currentNode.leave(write: true) >= 3)
			{
				ByteUtil.setUB3(currentNode.bytes, currentNode.offset, i);
				currentNode.size += 3;
				currentNode.offset += 3;
				return 3;
			}
			return write(i, 3);
		}

		public override int writeUB4(long l)
		{
			if (currentNode.leave(write: true) >= 4)
			{
				ByteUtil.setUB4(currentNode.bytes, currentNode.offset, l);
				currentNode.size += 4;
				currentNode.offset += 4;
				return 4;
			}
			return write(l, 4);
		}

		public override int writeLong(long l)
		{
			if (currentNode.leave(write: true) >= 8)
			{
				ByteUtil.setLong(currentNode.bytes, currentNode.offset, l);
				currentNode.size += 8;
				currentNode.offset += 8;
				return 8;
			}
			return write(l, 8);
		}

		public override int writeFloat(float f)
		{
			return writeBytes((sbyte[])(object)BitConverter.GetBytes(f));
		}

		public override int writeDouble(double d)
		{
			return writeLong(BitConverter.DoubleToInt64Bits(d));
		}

		public override int writeBytes(sbyte[] srcBytes)
		{
			return writeBytes(srcBytes, 0, srcBytes.Length);
		}

		public override int writeBytes(sbyte[] srcBytes, int srcOffset, int len)
		{
			int num = 0;
			while (len > 0)
			{
				int num2 = currentNode.write(srcBytes, srcOffset, len);
				len -= num2;
				num += num2;
				srcOffset += num2;
				if (len == 0)
				{
					break;
				}
				currentNode = currentNode.next;
				if (currentNode == null)
				{
					extend(len);
				}
			}
			return num;
		}

		public override int writeBytesWithLength(sbyte[] srcBytes, int srcOffset, int len)
		{
			return writeInt(len) + writeBytes(srcBytes, srcOffset, len);
		}

		public override int writeBytesWithLength2(sbyte[] srcBytes, int srcOffset, int len)
		{
			return writeUB2(len) + writeBytes(srcBytes, srcOffset, len);
		}

		public override int writeStringWithLength(string str, string encoding)
		{
			sbyte[] bytes = ByteUtil.getBytes(str, encoding);
			return writeBytesWithLength(bytes, 0, bytes.Length);
		}

		public override int writeStringWithLength2(string str, string encoding)
		{
			sbyte[] bytes = ByteUtil.getBytes(str, encoding);
			return writeBytesWithLength2(bytes, 0, bytes.Length);
		}

		public override int writeStringWithNTS(string str, string encoding)
		{
			sbyte[] bytes = ByteUtil.getBytes(str, encoding);
			return writeBytes(bytes, 0, bytes.Length) + writeByte(0);
		}

		private long read(int len)
		{
			long num = 0L;
			int num2 = 0;
			while (len > 0)
			{
				int num3 = currentNode.leave(write: false);
				if (len <= num3)
				{
					for (int i = 0; i < len; i++)
					{
						num |= (long)(currentNode.bytes[currentNode.offset++] & 0xFF) << num2++ * 8;
					}
					break;
				}
				for (int j = 0; j < num3; j++)
				{
					num |= (long)(currentNode.bytes[currentNode.offset++] & 0xFF) << num2++ * 8;
				}
				len -= num3;
				currentNode = currentNode.next;
				if (currentNode == null)
				{
					throw new Exception("buffer index out of range");
				}
			}
			return num;
		}

		public override sbyte readByte()
		{
			if (currentNode.leave(write: false) >= 1)
			{
				sbyte @byte = ByteUtil.getByte(currentNode.bytes, currentNode.offset);
				currentNode.offset++;
				return @byte;
			}
			return (sbyte)read(1);
		}

		public override short readShort()
		{
			if (currentNode.leave(write: false) >= 2)
			{
				short @short = ByteUtil.getShort(currentNode.bytes, currentNode.offset);
				currentNode.offset += 2;
				return @short;
			}
			return (short)read(2);
		}

		public override int readInt()
		{
			if (currentNode.leave(write: false) >= 4)
			{
				int @int = ByteUtil.getInt(currentNode.bytes, currentNode.offset);
				currentNode.offset += 4;
				return @int;
			}
			return (int)read(4);
		}

		public override long readLong()
		{
			if (currentNode.leave(write: false) >= 8)
			{
				long @long = ByteUtil.getLong(currentNode.bytes, currentNode.offset);
				currentNode.offset += 8;
				return @long;
			}
			return read(8);
		}

		public override float readFloat()
		{
			return BitConverter.ToSingle((byte[])(object)readBytes(4), 0);
		}

		public override double readDouble()
		{
			return BitConverter.Int64BitsToDouble(readLong());
		}

		public override int readUB1()
		{
			if (currentNode.leave(write: false) >= 1)
			{
				sbyte @byte = ByteUtil.getByte(currentNode.bytes, currentNode.offset);
				currentNode.offset++;
				return @byte;
			}
			return (int)read(1);
		}

		public override int readUB2()
		{
			if (currentNode.leave(write: false) >= 2)
			{
				int uB = ByteUtil.getUB2(currentNode.bytes, currentNode.offset);
				currentNode.offset += 2;
				return uB;
			}
			return (int)read(2);
		}

		public override int readUB3()
		{
			if (currentNode.leave(write: false) >= 3)
			{
				int uB = ByteUtil.getUB3(currentNode.bytes, currentNode.offset);
				currentNode.offset += 3;
				return uB;
			}
			return (int)read(3);
		}

		public override long readUB4()
		{
			if (currentNode.leave(write: false) >= 4)
			{
				long uB = ByteUtil.getUB4(currentNode.bytes, currentNode.offset);
				currentNode.offset += 4;
				return uB;
			}
			return read(4);
		}

		public override sbyte[] readBytes(int len)
		{
			sbyte[] array = new sbyte[len];
			readBytes(array, 0, len);
			return array;
		}

		public override void readBytes(sbyte[] destBytes, int off, int len)
		{
			int num = off;
			while (len > 0)
			{
				int num2 = currentNode.read(destBytes, num, len);
				len -= num2;
				num += num2;
				if (len != 0)
				{
					currentNode = currentNode.next;
					if (currentNode == null)
					{
						throw new Exception("buffer index out of range");
					}
					continue;
				}
				break;
			}
		}

		public override sbyte[] readBytesWithLength()
		{
			return readBytes(readInt());
		}

		public override sbyte[] readBytesWithLength2()
		{
			return readBytes(readUB2());
		}

		public override string readString(int len, string encoding)
		{
			sbyte[] array = readBytes(len);
			return ByteUtil.getString(array, 0, array.Length, encoding);
		}

		public override string readStringWithLength(string encoding)
		{
			sbyte[] array = readBytesWithLength();
			return ByteUtil.getString(array, 0, array.Length, encoding);
		}

		public override string readStringWithLength2(string encoding)
		{
			sbyte[] array = readBytesWithLength2();
			return ByteUtil.getString(array, 0, array.Length, encoding);
		}

		private int set(Node node, int offset, long val, int len)
		{
			int result = 0;
			while (len > 0)
			{
				int num = node.size - offset;
				if (len <= num)
				{
					for (int i = 0; i < len; i++)
					{
						node.bytes[offset++] = (sbyte)(val >> result++ * 8);
					}
					break;
				}
				for (int j = 0; j < num; j++)
				{
					node.bytes[offset++] = (sbyte)(val >> result++ * 8);
				}
				len -= num;
				offset = 0;
				node = node.next;
				if (node == null)
				{
					throw new Exception("buffer index out of range");
				}
			}
			return result;
		}

		public override int setByte(int offset, sbyte b)
		{
			Node node = locate(offset);
			offset = ((int?)node.info).Value;
			if (node.size - offset >= 1)
			{
				return ByteUtil.setByte(node.bytes, offset, b);
			}
			return set(node, offset, b, 1);
		}

		public override int setShort(int offset, short s)
		{
			Node node = locate(offset);
			offset = ((int?)node.info).Value;
			if (node.size - offset >= 2)
			{
				return ByteUtil.setShort(node.bytes, offset, s);
			}
			return set(node, offset, s, 2);
		}

		public override int setInt(int offset, int i)
		{
			Node node = locate(offset);
			offset = ((int?)node.info).Value;
			if (node.size - offset >= 4)
			{
				return ByteUtil.setInt(node.bytes, offset, i);
			}
			return set(node, offset, i, 4);
		}

		public override int setLong(int offset, long l)
		{
			Node node = locate(offset);
			offset = ((int?)node.info).Value;
			if (node.size - offset >= 1)
			{
				return ByteUtil.setLong(node.bytes, offset, l);
			}
			return set(node, offset, l, 8);
		}

		public override int setFloat(int offset, float f)
		{
			return setBytes(offset, (sbyte[])(object)BitConverter.GetBytes(f));
		}

		public override int setDouble(int offset, double d)
		{
			return setLong(offset, BitConverter.DoubleToInt64Bits(d));
		}

		public override int setUB1(int offset, int i)
		{
			return setByte(offset, (sbyte)i);
		}

		public override int setUB2(int offset, int i)
		{
			return setShort(offset, (short)i);
		}

		public override int setUB3(int offset, int i)
		{
			Node node = locate(offset);
			offset = ((int?)node.info).Value;
			if (node.size - offset >= 3)
			{
				return ByteUtil.setUB3(node.bytes, offset, i);
			}
			return 3;
		}

		public override int setUB4(int offset, long l)
		{
			return setInt(offset, (int)l);
		}

		public override int setBytes(int offset, sbyte[] srcBytes)
		{
			return setBytes(offset, srcBytes, 0, srcBytes.Length);
		}

		public override int setBytes(int offset, sbyte[] srcBytes, int srcOffset, int len)
		{
			Node node = locate(offset);
			offset = ((int?)node.info).Value;
			int num = 0;
			while (len > 0)
			{
				int num2 = node.set(offset, srcBytes, srcOffset, len);
				len -= num2;
				num += num2;
				offset = 0;
				srcOffset += num2;
				if (len == 0)
				{
					break;
				}
				node = node.next;
				if (node == null)
				{
					throw new Exception("buffer index out of range");
				}
			}
			return num;
		}

		public override int setBytesWithLength(int offset, sbyte[] srcBytes)
		{
			return setBytesWithLength(offset, srcBytes, 0, srcBytes.Length);
		}

		public override int setBytesWithLength(int offset, sbyte[] srcBytes, int srcOffset, int len)
		{
			offset += setInt(offset, len);
			return 4 + setBytes(offset, srcBytes, srcOffset, len);
		}

		public override int setBytesWithLength2(int offset, sbyte[] srcBytes)
		{
			return setBytesWithLength2(offset, srcBytes, 0, srcBytes.Length);
		}

		public override int setBytesWithLength2(int offset, sbyte[] srcBytes, int srcOffset, int len)
		{
			offset += setUB2(offset, len);
			return 2 + setBytes(offset, srcBytes, srcOffset, len);
		}

		public override int setStringWithLength(int offset, string str, string encoding)
		{
			sbyte[] bytes = ByteUtil.getBytes(str, encoding);
			return setBytesWithLength(offset, bytes, 0, bytes.Length);
		}

		public override int setStringWithLength2(int offset, string str, string encoding)
		{
			sbyte[] bytes = ByteUtil.getBytes(str, encoding);
			return setBytesWithLength2(offset, bytes, 0, bytes.Length);
		}

		public override int setStringWithNTS(int offset, string str, string encoding)
		{
			sbyte[] bytes = ByteUtil.getBytes(str, encoding);
			return setBytes(offset, bytes, 0, bytes.Length) + setByte(offset + bytes.Length, 0);
		}

		private long get(Node node, int offset, int len)
		{
			long num = 0L;
			int num2 = 0;
			while (len > 0)
			{
				int num3 = node.size - offset;
				if (len <= num3)
				{
					for (int i = 0; i < len; i++)
					{
						num |= (long)(node.bytes[offset++] & 0xFF) << num2++ * 8;
					}
					break;
				}
				for (int j = 0; j < num3; j++)
				{
					num |= (long)(node.bytes[offset++] & 0xFF) << num2++ * 8;
				}
				len -= num3;
				offset = 0;
				node = node.next;
				if (node == null)
				{
					throw new Exception("buffer index out of range");
				}
			}
			return num;
		}

		public override sbyte getByte(int offset)
		{
			Node node = locate(offset);
			offset = ((int?)node.info).Value;
			if (node.size - offset >= 1)
			{
				return ByteUtil.getByte(node.bytes, offset);
			}
			return (sbyte)get(node, offset, 1);
		}

		public override short getShort(int offset)
		{
			Node node = locate(offset);
			offset = ((int?)node.info).Value;
			if (node.size - offset >= 2)
			{
				return ByteUtil.getShort(node.bytes, offset);
			}
			return (short)get(node, offset, 2);
		}

		public override int getInt(int offset)
		{
			Node node = locate(offset);
			offset = ((int?)node.info).Value;
			if (node.size - offset >= 4)
			{
				return ByteUtil.getInt(node.bytes, offset);
			}
			return (sbyte)get(node, offset, 4);
		}

		public override long getLong(int offset)
		{
			Node node = locate(offset);
			offset = ((int?)node.info).Value;
			if (node.size - offset >= 8)
			{
				return ByteUtil.getLong(node.bytes, offset);
			}
			return get(node, offset, 8);
		}

		public override float getFloat(int offset)
		{
			return BitConverter.ToSingle((byte[])(object)getBytes(offset, 4), 0);
		}

		public override double getDouble(int offset)
		{
			return BitConverter.Int64BitsToDouble(getLong(offset));
		}

		public override int getUB1(int offset)
		{
			return getByte(offset) & 0xFF;
		}

		public override int getUB2(int offset)
		{
			return getShort(offset) & 0xFFFF;
		}

		public override int getUB3(int offset)
		{
			Node node = locate(offset);
			offset = ((int?)node.info).Value;
			if (node.size - offset >= 3)
			{
				return ByteUtil.getUB3(node.bytes, offset);
			}
			return (int)get(node, offset, 3);
		}

		public override long getUB4(int offset)
		{
			return getInt(offset) & 0xFFFFFFFFu;
		}

		public override sbyte[] getBytes(int offset, int len)
		{
			Node node = locate(offset);
			offset = ((int?)node.info).Value;
			int num = 0;
			sbyte[] array = new sbyte[len];
			while (len > 0)
			{
				int num2 = node.get(offset, array, num, len);
				len -= num2;
				if (len == 0)
				{
					break;
				}
				offset = 0;
				num += num2;
				node = node.next;
				if (node == null)
				{
					throw new Exception("buffer index out of range");
				}
			}
			return array;
		}

		public override sbyte[] getBytesWithLength(int offset)
		{
			return getBytes(offset + 4, getInt(offset));
		}

		public override sbyte[] getBytesWithLength2(int offset)
		{
			return getBytes(offset + 2, getUB2(offset));
		}

		public override string getStringWithLength(int offset, string encoding)
		{
			sbyte[] bytesWithLength = getBytesWithLength(offset);
			return ByteUtil.getString(bytesWithLength, 0, bytesWithLength.Length, encoding);
		}

		public override string getStringWithLength2(int offset, string encoding)
		{
			sbyte[] bytesWithLength = getBytesWithLength2(offset);
			return ByteUtil.getString(bytesWithLength, 0, bytesWithLength.Length, encoding);
		}

		public override string getStringWithNTS(int offset, string encoding)
		{
			int i = offset;
			for (int num = length(); i < num; i++)
			{
				if (getByte(i) == 0)
				{
					sbyte[] bytes = getBytes(offset, i - offset);
					return ByteUtil.getString(bytes, 0, bytes.Length, encoding);
				}
			}
			return "";
		}

		public override int load(Stream @is, int len)
		{
			int num = 0;
			while (len > 0)
			{
				int num2 = currentNode.load(@is, len);
				num += num2;
				len -= num2;
				if (len == 0)
				{
					break;
				}
				currentNode = currentNode.next;
				if (currentNode == null)
				{
					extend(len);
				}
			}
			return num;
		}

		public override void flush(Stream os)
		{
			Node next = firstNode;
			do
			{
				os.Write((byte[])(object)next.bytes, 0, next.size);
				next = next.next;
			}
			while (next != null);
			os.Flush();
		}

		public override void flush(Stream os, int off, int len)
		{
			Node next = firstNode;
			do
			{
				if (off > next.size)
				{
					off -= next.size;
					next = next.next;
				}
				else
				{
					len -= next.flush(os, off, len);
					off = 0;
					next = next.next;
				}
			}
			while (next != null && len > 0);
			os.Flush();
		}
	}
}
