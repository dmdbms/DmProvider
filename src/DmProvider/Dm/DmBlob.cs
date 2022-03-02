using System;
using System.IO;

namespace Dm
{
	public class DmBlob
	{
		private DmLob base_lob;

		private bool fetchAll;

		private byte[] m_BlobBytes;

		internal DmBlob(DmStatement stmt, byte[] val, short colIndex, bool fetchAll)
		{
			base_lob = new DmLob(stmt, val, colIndex);
			base_lob.InitLobInfo(stmt);
			base_lob.flag = 0;
			if (fetchAll)
			{
				FetchAllData();
				this.fetchAll = fetchAll;
			}
		}

		internal void SetBLobRowid(long rowid)
		{
			base_lob.m_Rowid = rowid;
		}

		public void FetchAllData()
		{
			m_BlobBytes = GetBytes(0L, Length());
		}

		public int Length()
		{
			if (fetchAll)
			{
				if (m_BlobBytes == null)
				{
					return 0;
				}
				return m_BlobBytes.Length;
			}
			return base_lob.Length();
		}

		private byte[] GetBytesOffRow(long pos, int length)
		{
			if (fetchAll)
			{
				if (m_BlobBytes == null)
				{
					return null;
				}
				int num = m_BlobBytes.Length - (int)pos;
				if (num < 0)
				{
					return null;
				}
				if (length > num)
				{
					byte[] array = new byte[num];
					Array.Copy(m_BlobBytes, pos, array, 0L, num);
					return array;
				}
				byte[] array2 = new byte[length];
				Array.Copy(m_BlobBytes, pos, array2, 0L, length);
				return array2;
			}
			byte[] array3 = null;
			byte[] array4 = new byte[length];
			int num2 = ((length <= 32000) ? length : 32000);
			int num3 = (int)pos;
			int num4 = 0;
			while (true)
			{
				base_lob.Stmt.SetCommandTime();
				array3 = base_lob.Stmt.ConnInst.GetCsi().GetLobData(base_lob, num3, num2);
				if (array3 == null)
				{
					break;
				}
				Array.Copy(array3, 0, array4, num4, array3.Length);
				num3 += array3.Length;
				num4 += array3.Length;
				num2 = length - num4;
				if (num2 <= 0)
				{
					break;
				}
				if (num2 > 32000)
				{
					num2 = 32000;
				}
			}
			if (num4 == length)
			{
				return array4;
			}
			byte[] array5 = new byte[num4];
			Array.Copy(array4, 0, array5, 0, num4);
			return array5;
		}

		public int SetBytes(long pos, ref byte[] bytes, int offset, int len)
		{
			if (pos < 0 || len < 0 || offset < 0)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_LENGTH_OR_OFFSET);
			}
			if (!base_lob.fromRowSetFlag && !base_lob.updatAble)
			{
				DmError.ThrowDmException(DmErrorDefinition.EC_UPDATE_READONLY_CURSOR);
			}
			if (offset + len > bytes.Length)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_LENGTH_OR_OFFSET);
			}
			if (fetchAll)
			{
				if (m_BlobBytes == null)
				{
					return 0;
				}
				int num = m_BlobBytes.Length - (int)pos;
				if (num <= 0)
				{
					return 0;
				}
				if (len > num)
				{
					byte[] array = new byte[m_BlobBytes.Length];
					Array.Copy(m_BlobBytes, 0L, array, 0L, pos);
					Array.Copy(bytes, offset, array, pos, num);
					m_BlobBytes = array;
					return num;
				}
				byte[] array2 = new byte[m_BlobBytes.Length];
				Array.Copy(m_BlobBytes, 0L, array2, 0L, pos);
				Array.Copy(bytes, offset, array2, pos, len);
				Array.Copy(m_BlobBytes, pos + len, array2, pos + len, m_BlobBytes.Length - (int)(pos + len));
				m_BlobBytes = array2;
				return len;
			}
			int num2 = (int)pos;
			int num3 = offset;
			int num4 = ((len <= 32000) ? len : 32000);
			int num5 = len / 32000 + 1;
			int num6 = 0;
			byte b = 0;
			for (int i = 0; i < num5; i++)
			{
				b = (byte)((i == 0 && i == num5 - 1) ? 3 : ((i == 0) ? 1 : ((i == num5 - 1) ? 2 : 0)));
				base_lob.Stmt.SetCommandTime();
				int num7 = base_lob.Stmt.ConnInst.GetCsi().SetLobData(base_lob, base_lob.flag, num2, bytes, num3, num4, b);
				if (num7 <= 0)
				{
					return num6;
				}
				num6 += num7;
				num2 += num4;
				num3 += num4;
				num4 = ((i != num5 - 2) ? 32000 : (len - (i + 1) * 32000));
			}
			if (base_lob.m_data_grpid == -1)
			{
				SetBlobInRow(pos, ref bytes, offset, len);
			}
			else
			{
				base_lob.m_Value[0] = 2;
			}
			return num6;
		}

		public byte[] GetBytes(long pos, int length)
		{
			byte[] valueInRow = base_lob.GetValueInRow();
			if (pos < 0 || length < 0)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_LENGTH_OR_OFFSET);
			}
			if (valueInRow != null)
			{
				byte[] array;
				if (pos > valueInRow.Length)
				{
					array = null;
				}
				else
				{
					int num = (int)pos;
					array = new byte[valueInRow.Length - num];
					Array.Copy(valueInRow, num, array, 0, valueInRow.Length - num);
				}
				return array;
			}
			return GetBytesOffRow(pos, length);
		}

		public void truncate(long len)
		{
			if (len < 0)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_LENGTH_OR_OFFSET);
			}
			if (fetchAll)
			{
				if (m_BlobBytes != null && len <= m_BlobBytes.Length)
				{
					byte[] array = new byte[len];
					Array.Copy(m_BlobBytes, 0L, array, 0L, len);
					m_BlobBytes = array;
				}
			}
			else if (len <= base_lob.getLobLen(0))
			{
				base_lob.Stmt.SetCommandTime();
				base_lob.Stmt.ConnInst.GetCsi().LobTrunc(base_lob, 0, (int)len);
				if (base_lob.m_data_grpid == -1)
				{
					byte[] array2 = new byte[(int)(13 + len)];
					Array.Copy(base_lob.m_Value, 0, array2, 0, array2.Length);
					DmConvertion.SetInt(array2, 9, (int)len);
					base_lob.m_Value = array2;
				}
			}
		}

		public Stream GetStream()
		{
			return new DmBLobStream(this);
		}

		private void SetBlobInRow(long pos, ref byte[] bytes, int offset, int len)
		{
			int lobLen = base_lob.getLobLen(0);
			base_lob.m_Value[0] = 1;
			if (pos > lobLen + 1)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_LENGTH_OR_OFFSET);
			}
			int num = ((!base_lob.new_lob_flag) ? 13 : 43);
			if (pos + len <= lobLen)
			{
				Array.Copy(bytes, offset, base_lob.m_Value, (int)(num + pos), len);
				return;
			}
			lobLen = (int)(pos + len);
			DmConvertion.SetInt(base_lob.m_Value, 9, lobLen);
			byte[] array = new byte[num + lobLen];
			Array.Copy(base_lob.m_Value, 0, array, 0, base_lob.m_Value.Length);
			base_lob.m_Value = array;
			Array.Copy(bytes, offset, base_lob.m_Value, (int)(num + pos), len);
		}
	}
}
