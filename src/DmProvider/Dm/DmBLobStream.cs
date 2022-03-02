using System;
using System.IO;

namespace Dm
{
	public class DmBLobStream : Stream
	{
		private DmBlob m_BLob;

		private bool m_CanRead = true;

		private bool m_CanSeek = true;

		private bool m_CanTimeOut = true;

		private bool m_CanWrite = true;

		private bool m_Closed;

		private long m_CurPos = 1L;

		private int m_ReadTimeout;

		private int m_WriteTimeout;

		private const int STREAM_CLOSED = -1;

		public override bool CanRead => m_CanRead;

		public override bool CanSeek => m_CanSeek;

		public override bool CanTimeout => m_CanTimeOut;

		public override bool CanWrite => m_CanWrite;

		public override long Length => m_BLob.Length();

		public override long Position
		{
			get
			{
				return m_CurPos;
			}
			set
			{
				m_CurPos = Position;
			}
		}

		public override int ReadTimeout
		{
			get
			{
				return m_ReadTimeout;
			}
			set
			{
				ReadTimeout = m_ReadTimeout;
			}
		}

		public override int WriteTimeout
		{
			get
			{
				return m_WriteTimeout;
			}
			set
			{
				m_WriteTimeout = WriteTimeout;
			}
		}

		public DmBLobStream(DmBlob bLob)
		{
			m_BLob = bLob;
		}

		public bool StreamCheck()
		{
			if (m_Closed)
			{
				return true;
			}
			return false;
		}

		public override void Close()
		{
			m_CurPos = 0L;
			m_Closed = true;
		}

		protected override void Dispose(bool disposing)
		{
		}

		public override void Flush()
		{
			DmError.ThrowUnsupportedException();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (StreamCheck())
			{
				return -1;
			}
			byte[] bytes = m_BLob.GetBytes(m_CurPos, count);
			m_CurPos += bytes.Length;
			Array.Copy(bytes, 0, buffer, offset, bytes.Length);
			return bytes.Length;
		}

		public override int ReadByte()
		{
			if (StreamCheck())
			{
				return -1;
			}
			byte[] bytes = m_BLob.GetBytes(m_CurPos, 1);
			if (bytes == null)
			{
				return -1;
			}
			m_CurPos += bytes.Length;
			return bytes[0];
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			if (StreamCheck())
			{
				return -1L;
			}
			long num = m_BLob.Length();
			long num2 = 0L;
			switch (origin)
			{
			case SeekOrigin.Begin:
				num2 = 1L;
				break;
			case SeekOrigin.Current:
				num2 = m_CurPos;
				break;
			case SeekOrigin.End:
				num2 = num;
				break;
			default:
				return -1L;
			}
			if (num2 + offset >= num)
			{
				m_CurPos = num - 1;
			}
			else if (num2 + offset < 0)
			{
				m_CurPos = 0L;
			}
			else
			{
				m_CurPos = num2 + offset;
			}
			return m_CurPos;
		}

		public override void SetLength(long value)
		{
			if (!StreamCheck())
			{
				m_BLob.truncate(value);
			}
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (m_Closed)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_RESULTSET_CLOSED);
			}
			if (offset + count > buffer.Length)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_LENGTH_OR_OFFSET);
				return;
			}
			m_BLob.SetBytes(m_CurPos, ref buffer, offset, count);
			m_CurPos += count;
		}
	}
}
