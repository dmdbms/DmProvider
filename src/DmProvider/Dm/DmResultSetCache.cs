using System;

namespace Dm
{
	internal class DmResultSetCache
	{
		private const long CURSOR_BEFORE_ANY = -1L;

		private const long CURSOR_AFTER_ANY = -2L;

		private DmStatement m_Statement;

		private long m_TotalRows = long.MaxValue;

		private int m_colNum;

		private long m_RowsetPos = -1L;

		private long m_Start;

		private long m_RowsNum;

		private int m_Off;

		private byte[] m_Rows;

		internal bool isRsBdta;

		internal short rsBdtaRowidCol;

		private bool rsBdtaDataRowNext;

		private int[] rsBdtaDataRowOffNext;

		private int[] rsBdtaDataRowOffCurrent;

		internal int[] ids;

		internal long[] tss;

		internal DateTime lastCheckDt;

		internal long RowsNum => m_RowsNum;

		internal long RowsetPos
		{
			get
			{
				return m_RowsetPos;
			}
			set
			{
				m_RowsetPos = value;
			}
		}

		internal long TotalRows
		{
			get
			{
				return m_TotalRows;
			}
			set
			{
				m_TotalRows = value;
			}
		}

		internal int ColNum
		{
			get
			{
				return m_colNum;
			}
			set
			{
				m_colNum = value;
			}
		}

		internal DmStatement Statement
		{
			get
			{
				return m_Statement;
			}
			set
			{
				m_Statement = value;
			}
		}

		internal int BytesCount
		{
			get
			{
				if (m_Rows == null)
				{
					return 0;
				}
				return m_Rows.Length;
			}
		}

		private void CheckColID(short columnNo)
		{
			if (columnNo < 0 || columnNo > m_colNum)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_COL_NUMBER);
			}
		}

		public DmResultSetCache(DmStatement stmt, int colNum, long rowCount)
		{
			m_Statement = stmt;
			m_colNum = colNum;
			m_TotalRows = rowCount;
		}

		public void SetCols(int colNum)
		{
			Reset();
			m_colNum = colNum;
		}

		public void GetBytes(short columnNo, ref byte[] val_buf)
		{
			if (m_Rows == null || m_RowsetPos == -2 || m_RowsetPos == -1)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_READ_NO_DATA);
			}
			if (isRsBdta)
			{
				int num = 0;
				int @int = DmConvertion.GetInt(m_Rows, num);
				num += 4;
				int @short = DmConvertion.GetShort(m_Rows, num);
				num += 2;
				num += 4;
				num += 4;
				num++;
				m_RowsNum = @int;
				short dtype = ((@short <= ColNum || columnNo < rsBdtaRowidCol) ? DmConvertion.GetShort(m_Rows, num + 2 * columnNo) : DmConvertion.GetShort(m_Rows, num + 2 * (columnNo + 1)));
				num += 2 * @short;
				int num2 = ((@short <= ColNum || columnNo < rsBdtaRowidCol) ? DmConvertion.GetInt(m_Rows, num + 4 * columnNo) : DmConvertion.GetInt(m_Rows, num + 4 * (columnNo + 1)));
				num = num2;
				int num3 = ((DmConvertion.GetInt(m_Rows, num) == 1) ? 1 : 0);
				num += 4;
				if (rsBdtaDataRowOffNext == null)
				{
					rsBdtaDataRowOffNext = new int[ColNum];
				}
				if (rsBdtaDataRowOffCurrent == null)
				{
					rsBdtaDataRowOffCurrent = new int[ColNum];
				}
				if (num3 == 0)
				{
					bool flag;
					if (rsBdtaDataRowNext)
					{
						flag = DmConvertion.GetByte(m_Rows, num + rsBdtaDataRowOffNext[columnNo]) == 0;
						rsBdtaDataRowOffNext[columnNo]++;
					}
					else
					{
						flag = DmConvertion.GetByte(m_Rows, num + rsBdtaDataRowOffCurrent[columnNo]) == 0;
					}
					if (flag)
					{
						val_buf = null;
						return;
					}
				}
				int num4 = getBdtaDataLength(dtype);
				int num5 = 0;
				switch (num4)
				{
				case -2:
					if (rsBdtaDataRowNext)
					{
						num5 = DmConvertion.GetInt(m_Rows, num + rsBdtaDataRowOffNext[columnNo]);
						rsBdtaDataRowOffNext[columnNo] += 4;
						num4 = DmConvertion.GetInt(m_Rows, num + rsBdtaDataRowOffNext[columnNo]);
						rsBdtaDataRowOffNext[columnNo] += 4;
					}
					else
					{
						num5 = DmConvertion.GetInt(m_Rows, num + rsBdtaDataRowOffCurrent[columnNo]);
						num4 = DmConvertion.GetInt(m_Rows, num + rsBdtaDataRowOffCurrent[columnNo]);
					}
					break;
				case -3:
					if (rsBdtaDataRowNext)
					{
						num4 = DmConvertion.GetInt(m_Rows, num + rsBdtaDataRowOffNext[columnNo]);
						rsBdtaDataRowOffNext[columnNo] += 4;
					}
					else
					{
						num4 = DmConvertion.GetInt(m_Rows, num + rsBdtaDataRowOffCurrent[columnNo]);
					}
					break;
				}
				val_buf = new byte[num4 + num5];
				if (rsBdtaDataRowNext)
				{
					Array.Copy(m_Rows, num + rsBdtaDataRowOffNext[columnNo], val_buf, 0, num4);
					rsBdtaDataRowOffNext[columnNo] += num4;
				}
				else
				{
					Array.Copy(m_Rows, num + rsBdtaDataRowOffCurrent[columnNo], val_buf, 0, num4);
				}
				if (num5 == 0)
				{
					rsBdtaDataRowOffCurrent[columnNo] = rsBdtaDataRowOffNext[columnNo];
					rsBdtaDataRowNext = false;
					return;
				}
				for (int i = num4; i < val_buf.Length; i++)
				{
					val_buf[i] = 32;
				}
				if (rsBdtaDataRowNext)
				{
					rsBdtaDataRowOffNext[columnNo] += num5;
				}
				rsBdtaDataRowOffCurrent[columnNo] = rsBdtaDataRowOffNext[columnNo];
				rsBdtaDataRowNext = false;
			}
			else
			{
				byte[] array = new byte[2];
				int num6 = RecGetNthFldAddr(m_Rows, m_Off, columnNo, array);
				short short2 = DmConvertion.GetShort(array, 0);
				if (short2 == -2)
				{
					val_buf = null;
					return;
				}
				val_buf = new byte[short2];
				Array.Copy(m_Rows, m_Off + num6, val_buf, 0, short2);
			}
		}

		public byte[] GetBytes(short columnNo)
		{
			if (m_Rows == null || m_RowsetPos == -2 || m_RowsetPos == -1)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_READ_NO_DATA);
			}
			if (isRsBdta)
			{
				int num = 0;
				int @int = DmConvertion.GetInt(m_Rows, num);
				num += 4;
				int @short = DmConvertion.GetShort(m_Rows, num);
				num += 2;
				num += 4;
				num += 4;
				num++;
				m_RowsNum = @int;
				short dtype = ((@short <= ColNum || columnNo < rsBdtaRowidCol) ? DmConvertion.GetShort(m_Rows, num + 2 * columnNo) : DmConvertion.GetShort(m_Rows, num + 2 * (columnNo + 1)));
				num += 2 * @short;
				int num2 = ((@short <= ColNum || columnNo < rsBdtaRowidCol) ? DmConvertion.GetInt(m_Rows, num + 4 * columnNo) : DmConvertion.GetInt(m_Rows, num + 4 * (columnNo + 1)));
				num = num2;
				int num3 = ((DmConvertion.GetInt(m_Rows, num) == 1) ? 1 : 0);
				num += 4;
				if (rsBdtaDataRowOffNext == null)
				{
					rsBdtaDataRowOffNext = new int[ColNum];
				}
				if (rsBdtaDataRowOffCurrent == null)
				{
					rsBdtaDataRowOffCurrent = new int[ColNum];
				}
				if (num3 == 0)
				{
					bool flag;
					if (rsBdtaDataRowNext)
					{
						flag = DmConvertion.GetByte(m_Rows, num + rsBdtaDataRowOffNext[columnNo]) == 0;
						rsBdtaDataRowOffNext[columnNo]++;
					}
					else
					{
						flag = DmConvertion.GetByte(m_Rows, num + rsBdtaDataRowOffCurrent[columnNo]) == 0;
					}
					if (flag)
					{
						return null;
					}
				}
				int num4 = getBdtaDataLength(dtype);
				int num5 = 0;
				switch (num4)
				{
				case -2:
					if (rsBdtaDataRowNext)
					{
						num5 = DmConvertion.GetInt(m_Rows, num + rsBdtaDataRowOffNext[columnNo]);
						rsBdtaDataRowOffNext[columnNo] += 4;
						num4 = DmConvertion.GetInt(m_Rows, num + rsBdtaDataRowOffNext[columnNo]);
						rsBdtaDataRowOffNext[columnNo] += 4;
					}
					else
					{
						num5 = DmConvertion.GetInt(m_Rows, num + rsBdtaDataRowOffCurrent[columnNo]);
						num4 = DmConvertion.GetInt(m_Rows, num + rsBdtaDataRowOffCurrent[columnNo]);
					}
					break;
				case -3:
					if (rsBdtaDataRowNext)
					{
						num4 = DmConvertion.GetInt(m_Rows, num + rsBdtaDataRowOffNext[columnNo]);
						rsBdtaDataRowOffNext[columnNo] += 4;
					}
					else
					{
						num4 = DmConvertion.GetInt(m_Rows, num + rsBdtaDataRowOffCurrent[columnNo]);
					}
					break;
				}
				byte[] array = new byte[num4 + num5];
				if (rsBdtaDataRowNext)
				{
					Array.Copy(m_Rows, num + rsBdtaDataRowOffNext[columnNo], array, 0, num4);
					rsBdtaDataRowOffNext[columnNo] += num4;
				}
				else
				{
					Array.Copy(m_Rows, num + rsBdtaDataRowOffCurrent[columnNo], array, 0, num4);
				}
				if (num5 == 0)
				{
					rsBdtaDataRowOffCurrent[columnNo] = rsBdtaDataRowOffNext[columnNo];
					rsBdtaDataRowNext = false;
					return array;
				}
				for (int i = num4; i < array.Length; i++)
				{
					array[i] = 32;
				}
				if (rsBdtaDataRowNext)
				{
					rsBdtaDataRowOffNext[columnNo] += num5;
				}
				rsBdtaDataRowOffCurrent[columnNo] = rsBdtaDataRowOffNext[columnNo];
				rsBdtaDataRowNext = false;
				return array;
			}
			byte[] array2 = new byte[2];
			int num6 = RecGetNthFldAddr(m_Rows, m_Off, columnNo, array2);
			short short2 = DmConvertion.GetShort(array2, 0);
			if (short2 == -2)
			{
				return null;
			}
			byte[] array3 = new byte[short2];
			Array.Copy(m_Rows, m_Off + num6, array3, 0, short2);
			return array3;
		}

		private int getBdtaDataLength(int dtype)
		{
			int num = 0;
			switch (dtype)
			{
			case 3:
			case 5:
			case 6:
			case 7:
			case 13:
			case 25:
				return 4;
			case 8:
				return 8;
			case 0:
			case 1:
			case 2:
			case 12:
			case 17:
			case 18:
			case 19:
				return -2;
			case 9:
				return -3;
			case 10:
				return 4;
			case 11:
				return 8;
			case 14:
			case 15:
			case 16:
			case 22:
			case 23:
				return 12;
			case 20:
				return 12;
			case 21:
				return 24;
			default:
				throw new SystemException("Value is of unknown data type");
			}
		}

		private int RecGetNthFldAddr(byte[] rec, int off, short n, byte[] alen)
		{
			int num = 10 + n * 2;
			short @short = DmConvertion.GetShort(rec, off + num);
			int result = @short + 2;
			alen[0] = rec[off + @short];
			alen[1] = rec[off + @short + 1];
			return result;
		}

		private short RecGetLen(byte[] rec, int off)
		{
			return (short)(DmConvertion.GetShort(rec, off) & 0x7FFF);
		}

		public void Reset()
		{
			m_TotalRows = 0L;
			m_colNum = 0;
			m_RowsetPos = -1L;
			m_Start = 0L;
			m_RowsNum = 0L;
			m_Off = 0;
			m_Rows = null;
		}

		private void ClearLastFetch()
		{
		}

		internal bool FetchNext()
		{
			ClearLastFetch();
			if (m_TotalRows == 0L)
			{
				m_RowsetPos = -1L;
				return false;
			}
			if (m_RowsetPos == -2)
			{
				return false;
			}
			long num = ((m_RowsetPos != -1) ? (m_RowsetPos + 1) : 1);
			if (num > m_TotalRows)
			{
				m_RowsetPos = -2L;
				return false;
			}
			if (num > m_Start + m_RowsNum)
			{
				if (m_Statement.Csi.Fetch(m_Statement, this, 0, num - 1, m_TotalRows - (num - 1)))
				{
					m_RowsetPos = -2L;
					return false;
				}
			}
			else if (m_RowsetPos != -1 && !isRsBdta)
			{
				int num2 = RecGetLen(m_Rows, m_Off);
				m_Off += num2;
			}
			if (isRsBdta)
			{
				rsBdtaDataRowNext = true;
			}
			m_RowsetPos = num;
			return true;
		}

		internal bool FetchPrevious()
		{
			ClearLastFetch();
			if (m_TotalRows == 0L)
			{
				m_RowsetPos = -1L;
				return false;
			}
			if (m_RowsetPos == -1 || m_RowsetPos == 1)
			{
				return false;
			}
			long num = ((m_RowsetPos != -2) ? (m_RowsetPos - 1) : m_TotalRows);
			if (num < m_Start)
			{
				if (m_Statement.Csi.Fetch(m_Statement, this, 0, num - 1, long.MaxValue))
				{
					m_RowsetPos = -1L;
					return false;
				}
			}
			else if (m_RowsetPos != -1)
			{
				m_Off = 0;
				for (long num2 = m_Start; num2 < num - 1; num2++)
				{
					int num3 = RecGetLen(m_Rows, m_Off);
					m_Off += num3;
				}
			}
			m_RowsetPos = num;
			return true;
		}

		internal bool FetchAbsolute(long npos)
		{
			ClearLastFetch();
			if (m_TotalRows == 0L || npos == 0L)
			{
				m_RowsetPos = -1L;
				return false;
			}
			if (m_TotalRows > Math.Abs(npos))
			{
				if (m_TotalRows == long.MaxValue)
				{
					m_Statement.Csi.Fetch(m_Statement, this, 0, long.MaxValue, 1L);
				}
				if (m_TotalRows > Math.Abs(npos))
				{
					m_RowsetPos = -1L;
					return false;
				}
			}
			if (npos < 0)
			{
				npos = m_RowsNum + npos + 1;
			}
			if (npos < m_Start || npos > m_Start + m_RowsNum)
			{
				if (m_Statement.Csi.Fetch(m_Statement, this, 0, npos - 1, long.MaxValue))
				{
					m_RowsetPos = -1L;
					return false;
				}
			}
			else if (m_RowsetPos != -1)
			{
				m_Off = 0;
				for (long num = m_Start; num < npos - 1; num++)
				{
					int num2 = RecGetLen(m_Rows, m_Off);
					m_Off += num2;
				}
			}
			m_RowsetPos = npos;
			return true;
		}

		internal bool FetchRelative(long offset)
		{
			ClearLastFetch();
			if (m_RowsetPos == -1 && offset < 0)
			{
				return false;
			}
			if (m_RowsetPos == 1 && offset < 0)
			{
				return false;
			}
			if (m_RowsetPos == -2 && offset > 0)
			{
				return false;
			}
			if (m_RowsetPos + offset > m_TotalRows)
			{
				return false;
			}
			if (m_RowsetPos > 1 && m_RowsetPos + offset < 1 && Math.Abs(offset) > 1)
			{
				return false;
			}
			long npos = ((m_RowsetPos == -1 && offset > 0) ? offset : ((m_RowsetPos != -2 || offset >= 0) ? (offset + m_RowsetPos) : (-offset)));
			return FetchAbsolute(npos);
		}

		internal bool FetchLast()
		{
			ClearLastFetch();
			if (m_TotalRows == long.MaxValue)
			{
				m_Statement.Csi.Fetch(m_Statement, this, 0, long.MaxValue, 1L);
			}
			return FetchAbsolute(m_TotalRows);
		}

		internal bool FetchFirst()
		{
			ClearLastFetch();
			return FetchAbsolute(1L);
		}

		public void FillRows(long rowPos, int rowNum, byte[] rowsBuf, bool isRsBdta, short rsBdtaRowidCol)
		{
			m_Start = rowPos;
			m_RowsNum = rowNum;
			m_Rows = rowsBuf;
			m_Off = 0;
			this.isRsBdta = isRsBdta;
			this.rsBdtaRowidCol = rsBdtaRowidCol;
			rsBdtaDataRowOffNext = null;
			rsBdtaDataRowOffCurrent = null;
		}

		public long GetRecRowid()
		{
			if (m_Rows == null)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_NOT_ALLOW_NULL);
			}
			return DmConvertion.GetLong(m_Rows, m_Off + 2);
		}

		internal long CursorUpdateRow()
		{
			if (m_RowsetPos == -1 || m_RowsetPos == -2)
			{
				return m_RowsetPos;
			}
			return m_RowsetPos - 1;
		}
	}
}
