using System.IO;

namespace Dm.net.buffer
{
	public abstract class Buffer
	{
		public static Buffer wrap(sbyte[] bytes)
		{
			return new ByteArrayBuffer(bytes, 0, bytes.Length);
		}

		public static Buffer allocateBytes(int capacity)
		{
			return new ByteArrayBuffer(capacity);
		}

		public abstract void clear();

		public abstract void truncate(int offset);

		public abstract int length();

		public abstract void flip();

		public abstract void fill(int len);

		public abstract void skip(int len);

		public abstract long offset();

		public abstract int writeByte(sbyte b);

		public abstract int writeShort(short s);

		public abstract int writeInt(int i);

		public abstract int writeUB1(int i);

		public abstract int writeUB2(int i);

		public abstract int writeUB3(int i);

		public abstract int writeUB4(long l);

		public abstract int writeLong(long l);

		public abstract int writeFloat(float f);

		public abstract int writeDouble(double d);

		public abstract int writeBytes(sbyte[] srcBytes);

		public abstract int writeBytes(sbyte[] srcBytes, int srcOffset, int len);

		public abstract int writeBytesWithLength(sbyte[] srcBytes, int srcOffset, int len);

		public abstract int writeBytesWithLength2(sbyte[] srcBytes, int srcOffset, int len);

		public abstract int writeStringWithLength(string str, string encoding);

		public abstract int writeStringWithLength2(string str, string encoding);

		public abstract int writeStringWithNTS(string str, string encoding);

		public abstract sbyte readByte();

		public abstract short readShort();

		public abstract int readInt();

		public abstract long readLong();

		public abstract float readFloat();

		public abstract double readDouble();

		public abstract int readUB1();

		public abstract int readUB2();

		public abstract int readUB3();

		public abstract long readUB4();

		public abstract sbyte[] readBytes(int len);

		public abstract void readBytes(sbyte[] destBytes, int off, int len);

		public abstract sbyte[] readBytesWithLength();

		public abstract sbyte[] readBytesWithLength2();

		public abstract string readString(int len, string encoding);

		public abstract string readStringWithLength(string encoding);

		public abstract string readStringWithLength2(string encoding);

		public abstract int setByte(int offset, sbyte b);

		public abstract int setShort(int offset, short s);

		public abstract int setInt(int offset, int i);

		public abstract int setLong(int offset, long l);

		public abstract int setFloat(int offset, float f);

		public abstract int setDouble(int offset, double d);

		public abstract int setUB1(int offset, int i);

		public abstract int setUB2(int offset, int i);

		public abstract int setUB3(int offset, int i);

		public abstract int setUB4(int offset, long l);

		public abstract int setBytes(int offset, sbyte[] srcBytes);

		public abstract int setBytes(int offset, sbyte[] srcBytes, int srcOffset, int len);

		public abstract int setBytesWithLength(int offset, sbyte[] srcBytes);

		public abstract int setBytesWithLength(int offset, sbyte[] srcBytes, int srcOffset, int len);

		public abstract int setBytesWithLength2(int offset, sbyte[] srcBytes);

		public abstract int setBytesWithLength2(int offset, sbyte[] srcBytes, int srcOffset, int len);

		public abstract int setStringWithLength(int offset, string str, string encoding);

		public abstract int setStringWithLength2(int offset, string str, string encoding);

		public abstract int setStringWithNTS(int offset, string str, string encoding);

		public abstract sbyte getByte(int offset);

		public abstract short getShort(int offset);

		public abstract int getInt(int offset);

		public abstract long getLong(int offset);

		public abstract float getFloat(int offset);

		public abstract double getDouble(int offset);

		public abstract int getUB1(int offset);

		public abstract int getUB2(int offset);

		public abstract int getUB3(int offset);

		public abstract long getUB4(int offset);

		public abstract sbyte[] getBytes(int offset, int len);

		public abstract sbyte[] getBytesWithLength(int offset);

		public abstract sbyte[] getBytesWithLength2(int offset);

		public abstract string getStringWithLength(int offset, string encoding);

		public abstract string getStringWithLength2(int offset, string encoding);

		public abstract string getStringWithNTS(int offset, string encoding);

		public abstract int load(Stream @is, int len);

		public abstract void flush(Stream os);

		public abstract void flush(Stream os, int off, int len);
	}
}
