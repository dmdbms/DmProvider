using System;
using System.IO;
using System.Text;

namespace Dm
{
	public class DmClob
	{
		private DmLob m_BaseLob;

		private string m_ServerEncoding;

		private string m_ClobStr = "";

		private int m_ByteOffset = 1;

		private int m_CharOffset = 1;

		private bool fetchAll;

		internal DmClob(DmStatement stmt, byte[] val, short colIndex, bool fetchAll)
		{
			m_BaseLob = new DmLob(stmt, val, colIndex);
			m_BaseLob.InitLobInfo(stmt);
			m_BaseLob.flag = 1;
			m_ServerEncoding = stmt.ConnInst.ConnProperty.ServerEncoding;
			if (m_BaseLob.IsValueInRow())
			{
				byte[] valueInRow = m_BaseLob.GetValueInRow();
				if (valueInRow == null)
				{
					m_ClobStr = null;
				}
				else
				{
					m_ClobStr = DmConvertion.GetString(valueInRow, 0, valueInRow.Length, m_ServerEncoding);
				}
			}
			else if (fetchAll)
			{
				FetchAllData();
				this.fetchAll = fetchAll;
			}
		}

		internal void SetBLobRowid(long rowid)
		{
			m_BaseLob.m_Rowid = rowid;
		}

		internal void SetCurFileid(byte[] fileid)
		{
			Array.Copy(fileid, 0, m_BaseLob.cur_fileid, 0, fileid.Length);
		}

		internal void SetCurPageno(byte[] pageno)
		{
			Array.Copy(pageno, 0, m_BaseLob.cur_pageno, 0, pageno.Length);
		}

		internal void SetCurOff(byte[] curoff)
		{
			Array.Copy(curoff, 0, m_BaseLob.m_curOff, 0, curoff.Length);
		}

		internal Stream GetStream()
		{
			int num = Length();
			MemoryStream memoryStream = new MemoryStream(num);
			memoryStream.Write(GetBytes(), 0, num);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		internal byte[] GetBytes()
		{
			string subString = GetSubString(0L, int.MaxValue);
			if (subString == null)
			{
				return null;
			}
			return Encoding.Default.GetBytes(subString);
		}

		public void FetchAllData()
		{
			m_ClobStr = GetString(0, Length());
		}

		public string GetString(int pos, int length)
		{
			return GetSubString(pos, length);
		}

		public int Length()
		{
			if (fetchAll)
			{
				return m_ClobStr.Length;
			}
			return m_BaseLob.Length();
		}

		public int SetString(long pos, string str, int offset, int len)
		{
			if (pos < 0 || offset < 0 || len < 0)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_LENGTH_OR_OFFSET);
			}
			if (fetchAll)
			{
				int num = m_ClobStr.Length - (int)pos;
				if (num <= 0)
				{
					return 0;
				}
				if (len > num)
				{
					m_ClobStr = m_ClobStr.Substring(0, (int)pos) + str.Substring(offset, num);
					return num;
				}
				m_ClobStr = m_ClobStr.Substring(0, (int)pos) + str.Substring(offset, len) + m_ClobStr.Substring((int)pos + len, m_ClobStr.Length - (int)(pos + len));
				return len;
			}
			int num2 = 0;
			int num3 = 8000;
			byte[] bytes = DmConvertion.GetBytes(str.Substring(offset, len), m_ServerEncoding);
			int num4 = (int)pos;
			int num5 = 0;
			int num6 = bytes.Length;
			int num7 = ((num6 <= num3) ? num6 : num3);
			int num8 = num6 / num3 + 1;
			num2 = 0;
			byte b = 0;
			for (int i = 0; i < num8; i++)
			{
				b = (byte)((i == 0 && i == num8 - 1) ? 3 : ((i == 0) ? 1 : ((i == num8 - 1) ? 2 : 0)));
				m_BaseLob.Stmt.SetCommandTime();
				int num9 = m_BaseLob.Stmt.ConnInst.GetCsi().SetLobData(m_BaseLob, 1, num4, bytes, num5, num7, b);
				if (num9 <= 0)
				{
					return num2;
				}
				num2 += num9;
				num4 += num9;
				num5 += num7;
				num7 = ((i != num8 - 2) ? num3 : (num6 - num5));
			}
			if (m_BaseLob.m_data_grpid == -1)
			{
				SetClobInRow(pos, str, offset, len);
				m_ClobStr = m_ClobStr.Substring(0, (int)pos) + str.Substring(offset, len);
			}
			else
			{
				m_BaseLob.m_Value[0] = 2;
				m_ClobStr = "";
			}
			return num2;
		}

		public void Truncate(long len)
		{
			if (len < 0)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_LENGTH_OR_OFFSET);
			}
			if (fetchAll)
			{
				if (len > m_ClobStr.Length)
				{
					m_ClobStr = m_ClobStr.Substring(0, m_ClobStr.Length);
				}
				else
				{
					m_ClobStr = m_ClobStr.Substring(0, (int)len);
				}
				return;
			}
			m_BaseLob.Stmt.SetCommandTime();
			m_BaseLob.Stmt.ConnInst.GetCsi().LobTrunc(m_BaseLob, 1, (int)len);
			if (m_BaseLob.m_data_grpid == -1)
			{
				TruncateClobInRow(len);
			}
		}

		public string GetSubString(long pos, int length)
		{
			if (pos < 0 || length < 0)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_LENGTH_OR_OFFSET);
			}
			string text = null;
			int num = 32000;
			int num2 = 0;
			byte[] @byte = new byte[4];
			if (m_BaseLob.IsValueInRow() || fetchAll)
			{
				if (pos > m_ClobStr.Length)
				{
					return "";
				}
				if (m_ClobStr.Length < pos + length)
				{
					return m_ClobStr.Substring((int)pos);
				}
				return m_ClobStr.Substring((int)pos, length);
			}
			if (length > num)
			{
				int num3 = num;
			}
			else
			{
				int num3 = length;
			}
			string text2 = "";
			int num4 = (int)pos;
			int num5 = 0;
			while (true)
			{
				if (num4 == m_CharOffset)
				{
					num2 = m_ByteOffset;
				}
				m_BaseLob.Stmt.SetCommandTime();
				text = m_BaseLob.Stmt.ConnInst.GetCsi().ClobGetSubString(m_BaseLob, 1, num4, length);
				if (text == null)
				{
					break;
				}
				num2 = DmConvertion.FourByteToInt(@byte);
				if (num2 > 0)
				{
					m_CharOffset += text.Length;
					m_ByteOffset = num2;
				}
				else
				{
					m_CharOffset = 1;
					m_ByteOffset = 1;
				}
				text2 += text;
				num4 += text.Length;
				num5 += text.Length;
				int num3 = length - num5;
				if (num3 <= 0)
				{
					break;
				}
				if (num3 > num)
				{
					num3 = num;
				}
			}
			return text2;
		}

		private void TruncateClobInRow(long len)
		{
			byte[] bytes = DmConvertion.GetBytes(DmConvertion.GetString(m_BaseLob.m_Value, 13, m_BaseLob.getLobLen(1), m_ServerEncoding).Substring(0, (int)len), m_ServerEncoding);
			int num = ((!m_BaseLob.new_lob_flag) ? 13 : 43);
			byte[] array = new byte[bytes.Length + num];
			Array.Copy(m_BaseLob.m_Value, 0, array, 0, num);
			Array.Copy(bytes, 0, array, num, bytes.Length);
			DmConvertion.SetInt(array, 9, bytes.Length);
			m_BaseLob.m_Value = array;
		}

		private void SetClobInRow(long pos, string str, int offset, int len)
		{
			int lobLen = m_BaseLob.getLobLen(1);
			m_BaseLob.m_Value[0] = 1;
			string @string = DmConvertion.GetString(m_BaseLob.m_Value, 13, lobLen, m_ServerEncoding);
			string str2;
			if (pos + len <= @string.Length)
			{
				string text = @string.Substring(0, (int)pos);
				string text2 = @string.Substring((int)(pos + len));
				string text3 = str.Substring(offset, offset + len);
				str2 = text + text3 + text2;
			}
			else
			{
				string text4 = @string.Substring(0, (int)pos);
				string text5 = str.Substring(offset, offset + len);
				str2 = text4 + text5;
			}
			byte[] bytes = DmConvertion.GetBytes(str2, m_ServerEncoding);
			int num = ((!m_BaseLob.new_lob_flag) ? 13 : 43);
			byte[] array = new byte[bytes.Length + num];
			Array.Copy(m_BaseLob.m_Value, 0, array, 0, num);
			Array.Copy(bytes, 0, array, num, bytes.Length);
			DmConvertion.SetInt(array, 9, bytes.Length);
			m_BaseLob.m_Value = array;
		}
	}
}
