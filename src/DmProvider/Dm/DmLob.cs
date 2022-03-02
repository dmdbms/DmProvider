using System;

namespace Dm
{
	internal class DmLob
	{
		public bool m_inRow;

		public int m_tabid;

		public short m_colid;

		public long m_Rowid;

		public short m_ColIndex;

		private int m_length;

		public long m_lobid = -1L;

		public short m_data_grpid;

		public short m_data_fileid;

		public int m_data_pageno;

		public short m_rec_grpid;

		public short m_rec_fileid;

		public int m_rec_pageno;

		public bool new_lob_flag = true;

		public byte[] m_Value;

		public byte[] cur_fileid = new byte[2];

		public byte[] cur_pageno = new byte[4];

		public byte[] m_curOff = new byte[4];

		public bool fromRowSetFlag;

		public bool isUpdated;

		public long offRowLen = -1L;

		public bool isFreed;

		public bool paramFlag;

		public bool updatAble = true;

		public byte flag;

		public DmStatement Stmt;

		public DmLob(DmStatement statement, byte[] val, short columnIndex)
		{
			Stmt = statement;
			m_ColIndex = columnIndex;
			m_Value = (byte[])val.Clone();
			new_lob_flag = ((statement.ConnInst.ConnProperty.NewLobFlag == 1) ? true : false);
			DmConvertion.SetShort(cur_fileid, 0, -1);
			DmConvertion.SetInt(cur_pageno, 0, -1);
		}

		public int getLobLen(byte flag)
		{
			if (IsValueInRow())
			{
				return DmConvertion.GetInt(m_Value, 9);
			}
			return Stmt.ConnInst.GetCsi().GetLobLen(this);
		}

		public void InitLobInfo(DmStatement stmt)
		{
			if (m_Value[0] == 2)
			{
				m_lobid = DmConvertion.GetLong(m_Value, 1);
				m_length = DmConvertion.GetInt(m_Value, 9);
				m_data_grpid = DmConvertion.GetShort(m_Value, 13);
				m_data_fileid = DmConvertion.GetShort(m_Value, 15);
				m_data_pageno = DmConvertion.GetInt(m_Value, 17);
				DmConvertion.SetShort(cur_fileid, 0, m_data_fileid);
				DmConvertion.SetInt(cur_pageno, 0, m_data_pageno);
				DmConvertion.SetInt(m_curOff, 0, 0);
			}
			else
			{
				m_lobid = DmConvertion.GetLong(m_Value, 1);
				m_length = DmConvertion.GetInt(m_Value, 9);
			}
			if (new_lob_flag)
			{
				m_tabid = DmConvertion.GetInt(m_Value, 21);
				m_colid = DmConvertion.GetShort(m_Value, 25);
				m_Rowid = DmConvertion.GetLong(m_Value, 27);
				m_rec_grpid = DmConvertion.GetShort(m_Value, 35);
				m_rec_fileid = DmConvertion.GetShort(m_Value, 37);
				m_rec_pageno = DmConvertion.GetInt(m_Value, 39);
			}
			else
			{
				m_tabid = stmt.DbInfo.GetColumnsInfo()[m_ColIndex].GetTableID();
				m_colid = stmt.DbInfo.GetColumnsInfo()[m_ColIndex].GetColID();
			}
		}

		public bool IsValueInRow()
		{
			if (DmConvertion.GetByte(m_Value, 0) == 2)
			{
				return false;
			}
			return true;
		}

		public int Length()
		{
			return m_length;
		}

		public byte[] GetValueInRow()
		{
			if (!IsValueInRow())
			{
				return null;
			}
			int @int = DmConvertion.GetInt(m_Value, 9);
			byte[] array = new byte[@int];
			if (new_lob_flag)
			{
				Array.Copy(m_Value, 43, array, 0, @int);
			}
			else
			{
				Array.Copy(m_Value, 13, array, 0, @int);
			}
			return array;
		}

		public void setBlobid(long blobid)
		{
			m_lobid = blobid;
		}

		public void setIsFree(bool isFreed)
		{
			this.isFreed = isFreed;
		}

		public void setGroupid(short groupid)
		{
			m_data_grpid = groupid;
		}

		public void setFileid(short fileid)
		{
			m_data_fileid = fileid;
		}

		public void setPageno(int pageno)
		{
			m_data_pageno = pageno;
		}

		public void setCurFileid(byte[] fileid)
		{
			Array.Copy(fileid, 0, cur_fileid, 0, fileid.Length);
		}

		public void setCurPageno(byte[] pageno)
		{
			Array.Copy(pageno, 0, cur_pageno, 0, pageno.Length);
		}

		public void setCurOff(byte[] curoff)
		{
			Array.Copy(curoff, 0, m_curOff, 0, curoff.Length);
		}
	}
}
