using System;
using System.IO;

namespace NetTaste
{
	public class Buffer
	{
		public const int EOF = 65536;

		private const int MIN_BUFFER_LENGTH = 1024;

		private const int MAX_BUFFER_LENGTH = 65536;

		private byte[] buf;

		private int bufStart;

		private int bufLen;

		private int fileLen;

		private int bufPos;

		private Stream stream;

		private bool isUserStream;

		public int Pos
		{
			get
			{
				return bufPos + bufStart;
			}
			set
			{
				if (value >= fileLen && stream != null && !stream.CanSeek)
				{
					while (value >= fileLen && ReadNextStreamChunk() > 0)
					{
					}
				}
				if (value < 0 || value > fileLen)
				{
					throw new FatalError("buffer out of bounds access, position: " + value);
				}
				if (value >= bufStart && value < bufStart + bufLen)
				{
					bufPos = value - bufStart;
				}
				else if (stream != null)
				{
					stream.Seek(value, SeekOrigin.Begin);
					bufLen = stream.Read(buf, 0, buf.Length);
					bufStart = value;
					bufPos = 0;
				}
				else
				{
					bufPos = fileLen - bufStart;
				}
			}
		}

		public Buffer(Stream s, bool isUserStream)
		{
			stream = s;
			this.isUserStream = isUserStream;
			if (stream.CanSeek)
			{
				fileLen = (int)stream.Length;
				bufLen = Math.Min(fileLen, 65536);
				bufStart = int.MaxValue;
			}
			else
			{
				fileLen = (bufLen = (bufStart = 0));
			}
			buf = new byte[(bufLen > 0) ? bufLen : 1024];
			if (fileLen > 0)
			{
				Pos = 0;
			}
			else
			{
				bufPos = 0;
			}
			if (bufLen == fileLen && stream.CanSeek)
			{
				Close();
			}
		}

		protected Buffer(Buffer b)
		{
			buf = b.buf;
			bufStart = b.bufStart;
			bufLen = b.bufLen;
			fileLen = b.fileLen;
			bufPos = b.bufPos;
			stream = b.stream;
			b.stream = null;
			isUserStream = b.isUserStream;
		}

		~Buffer()
		{
			Close();
		}

		protected void Close()
		{
			if (!isUserStream && stream != null)
			{
				stream.Close();
				stream = null;
			}
		}

		public virtual int Read()
		{
			if (bufPos < bufLen)
			{
				return buf[bufPos++];
			}
			if (Pos < fileLen)
			{
				Pos = Pos;
				return buf[bufPos++];
			}
			if (stream != null && !stream.CanSeek && ReadNextStreamChunk() > 0)
			{
				return buf[bufPos++];
			}
			return 65536;
		}

		public int Peek()
		{
			int pos = Pos;
			int result = Read();
			Pos = pos;
			return result;
		}

		public string GetString(int beg, int end)
		{
			int length = 0;
			char[] array = new char[end - beg];
			int pos = Pos;
			Pos = beg;
			while (Pos < end)
			{
				array[length++] = (char)Read();
			}
			Pos = pos;
			return new string(array, 0, length);
		}

		private int ReadNextStreamChunk()
		{
			int num = buf.Length - bufLen;
			if (num == 0)
			{
				byte[] destinationArray = new byte[bufLen * 2];
				Array.Copy(buf, destinationArray, bufLen);
				buf = destinationArray;
				num = bufLen;
			}
			int num2 = stream.Read(buf, bufLen, num);
			if (num2 > 0)
			{
				fileLen = (bufLen += num2);
				return num2;
			}
			return 0;
		}
	}
}
