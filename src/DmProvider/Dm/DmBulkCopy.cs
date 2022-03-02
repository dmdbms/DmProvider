using System;
using System.Data;

namespace Dm
{
	public class DmBulkCopy : IDisposable
	{
		private int m_BatchSize = -1;

		private string m_DestTable;

		private DmBulkCopyOptions m_CopyOpt;

		private int m_timeout = 30;

		private DmBulkCopyColumnMappingCollection m_columnMappings;

		private int m_notifyAfter;

		private DmFldr m_Fldr;

		private DmConnection m_Conn;

		private bool closed;

		public int BatchSize
		{
			get
			{
				if (m_BatchSize != -1)
				{
					return m_BatchSize;
				}
				return 100;
			}
			set
			{
				if ((long)value >= 0L && value <= int.MaxValue)
				{
					m_BatchSize = value;
				}
			}
		}

		public int BulkCopyTimeout
		{
			get
			{
				return m_timeout;
			}
			set
			{
				if ((long)value >= 0L && value <= int.MaxValue)
				{
					m_timeout = value;
				}
			}
		}

		public DmBulkCopyColumnMappingCollection ColumnMappings => m_columnMappings;

		public string DestinationTableName
		{
			get
			{
				return m_DestTable;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("DestinationTableName");
				}
				if (value.Length == 0)
				{
					throw new ArgumentOutOfRangeException("DestinationTableName");
				}
				m_DestTable = value;
			}
		}

		public int NotifyAfter
		{
			get
			{
				return m_notifyAfter;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("NotifyAfter");
				}
				m_notifyAfter = value;
			}
		}

		public event DmRowsCopiedEventHandler DmRowsCopied;

		public DmBulkCopy(DmConnection conn)
		{
			if (conn == null)
			{
				throw new InvalidOperationException();
			}
			m_columnMappings = new DmBulkCopyColumnMappingCollection();
			m_Conn = conn;
			m_Fldr = new DmFldr();
		}

		public DmBulkCopy(string connectionString)
			: this(new DmConnection(connectionString))
		{
		}

		public DmBulkCopy(string connectionString, DmBulkCopyOptions copyOptions)
			: this(connectionString)
		{
			m_CopyOpt = copyOptions;
		}

		public DmBulkCopy(DmConnection conn, DmBulkCopyOptions copyOptions, DmTransaction externalTran)
			: this(conn)
		{
			m_CopyOpt = copyOptions;
		}

		public void WriteToServer(DataRow[] rows)
		{
			DataTable dataTable = new DataTable();
			if (rows == null)
			{
				throw new ArgumentNullException("rows");
			}
			for (int i = 0; i < rows.Length; i++)
			{
				dataTable.Rows.Add(rows[i]);
			}
			WriteToServer(dataTable);
		}

		public void WriteToServer(DataTable table, DataRowState rowState)
		{
			DataTable dataTable = new DataTable();
			foreach (DataRow row in table.Rows)
			{
				if (row.RowState != DataRowState.Deleted && row.RowState == rowState)
				{
					dataTable.Rows.Add(row);
				}
			}
			WriteToServer(dataTable);
		}

		internal void SetValueInner(ref object obj, Type type, int nth_row, short dst_col, int src_col)
		{
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Single:
				m_Fldr.SetSingle(nth_row + 1, dst_col, (float)obj);
				return;
			case TypeCode.Double:
				m_Fldr.SetDouble(nth_row + 1, dst_col, (double)obj);
				return;
			case TypeCode.Int64:
			case TypeCode.UInt64:
				m_Fldr.SetInt64(nth_row + 1, dst_col, (long)obj);
				return;
			case TypeCode.Int32:
			case TypeCode.UInt32:
				m_Fldr.SetInt32(nth_row + 1, dst_col, (int)obj);
				return;
			case TypeCode.Int16:
			case TypeCode.UInt16:
				m_Fldr.SetInt16(nth_row + 1, dst_col, (short)obj);
				return;
			case TypeCode.SByte:
			case TypeCode.Byte:
				m_Fldr.SetUInt8(nth_row + 1, dst_col, (byte)obj);
				return;
			case TypeCode.Boolean:
				m_Fldr.SetInt8(nth_row + 1, dst_col, Convert.ToSByte(obj));
				return;
			}
			if (type.Equals(typeof(byte[])))
			{
				m_Fldr.SetBytes(nth_row + 1, dst_col, (byte[])obj, ((byte[])obj).Length);
			}
			else
			{
				m_Fldr.SetValue(nth_row + 1, dst_col, obj);
			}
		}

		internal void SetValueInner(ref object obj, Type type, int nth_row, string dst_col, int src_col)
		{
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Single:
				m_Fldr.SetSingle(nth_row + 1, dst_col, (float)obj);
				return;
			case TypeCode.Double:
				m_Fldr.SetDouble(nth_row + 1, dst_col, (double)obj);
				return;
			case TypeCode.Int64:
			case TypeCode.UInt64:
				m_Fldr.SetInt64(nth_row + 1, dst_col, (long)obj);
				return;
			case TypeCode.Int32:
			case TypeCode.UInt32:
				m_Fldr.SetInt32(nth_row + 1, dst_col, (int)obj);
				return;
			case TypeCode.Int16:
			case TypeCode.UInt16:
				m_Fldr.SetInt16(nth_row + 1, dst_col, (short)obj);
				return;
			case TypeCode.SByte:
			case TypeCode.Byte:
				m_Fldr.SetUInt8(nth_row + 1, dst_col, (byte)obj);
				return;
			case TypeCode.Boolean:
				m_Fldr.SetInt8(nth_row + 1, dst_col, Convert.ToSByte(obj));
				return;
			}
			if (type.Equals(typeof(byte[])))
			{
				m_Fldr.SetBytes(nth_row + 1, dst_col, (byte[])obj, ((byte[])obj).Length);
			}
			else
			{
				m_Fldr.SetValue(nth_row + 1, dst_col, obj);
			}
		}

		internal void SetValueInner(DataTable table, Type type, int batch_row, string dst_col, int src_col, int i)
		{
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Single:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetSingle(j + 1, dst_col, (float)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.Double:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetDouble(j + 1, dst_col, (double)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.Int64:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetInt64(j + 1, dst_col, (long)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.UInt64:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetUInt64(j + 1, dst_col, (ulong)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.Int32:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetInt32(j + 1, dst_col, (int)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.UInt32:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetUInt32(j + 1, dst_col, (uint)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.Int16:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetInt16(j + 1, dst_col, (short)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.UInt16:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetUInt16(j + 1, dst_col, (ushort)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.Byte:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetUInt8(j + 1, dst_col, (byte)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.SByte:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetInt8(j + 1, dst_col, (sbyte)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.Boolean:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetInt8(j + 1, dst_col, Convert.ToSByte(table.Rows[i + j][src_col]));
					}
				}
				return;
			}
			}
			if (type.Equals(typeof(byte[])))
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetBytes(j + 1, dst_col, (byte[])table.Rows[i + j][src_col], ((byte[])table.Rows[i + j][src_col]).Length);
					}
				}
				return;
			}
			for (int j = 0; j < batch_row; j++)
			{
				if (table.Rows[i + j][src_col] != DBNull.Value)
				{
					m_Fldr.SetValue(j + 1, dst_col, table.Rows[i + j][src_col]);
				}
			}
		}

		internal void SetValueInner(DataTable table, Type type, int batch_row, short dst_col, int src_col, int i)
		{
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Single:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetSingle(j + 1, dst_col, (float)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.Double:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetDouble(j + 1, dst_col, (double)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.Int64:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetInt64(j + 1, dst_col, (long)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.UInt64:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetUInt64(j + 1, dst_col, (ulong)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.Int32:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetInt32(j + 1, dst_col, (int)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.UInt32:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetUInt32(j + 1, dst_col, (uint)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.Int16:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetInt16(j + 1, dst_col, (short)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.UInt16:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetUInt16(j + 1, dst_col, (ushort)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.Byte:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetUInt8(j + 1, dst_col, (byte)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.SByte:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetInt8(j + 1, dst_col, (sbyte)table.Rows[i + j][src_col]);
					}
				}
				return;
			}
			case TypeCode.Boolean:
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetInt8(j + 1, dst_col, Convert.ToSByte(table.Rows[i + j][src_col]));
					}
				}
				return;
			}
			}
			if (type.Equals(typeof(byte[])))
			{
				for (int j = 0; j < batch_row; j++)
				{
					if (table.Rows[i + j][src_col] != DBNull.Value)
					{
						m_Fldr.SetBytes(j + 1, dst_col, (byte[])table.Rows[i + j][src_col], ((byte[])table.Rows[i + j][src_col]).Length);
					}
				}
				return;
			}
			for (int j = 0; j < batch_row; j++)
			{
				if (table.Rows[i + j][src_col] != DBNull.Value)
				{
					m_Fldr.SetValue(j + 1, dst_col, table.Rows[i + j][src_col]);
				}
			}
		}

		public void WriteToServer(DmDataReader reader)
		{
			int num = 0;
			int num2 = 0;
			if (m_DestTable == null)
			{
				throw new InvalidOperationException();
			}
			if (m_Fldr == null)
			{
				throw new InvalidOperationException();
			}
			int batchSize = BatchSize;
			DataTable dataTable = reader.do_GetSchemaTable();
			m_Fldr.Initilize(m_Conn.ConnProperty, m_DestTable, m_CopyOpt, batchSize, dataTable);
			m_Fldr.SetAttrNecessary(reader, dataTable, reader.do_FieldCount);
			if (m_columnMappings.Count == 0)
			{
				while (reader.do_Read())
				{
					for (int i = 0; i < reader.do_FieldCount; i++)
					{
						object obj = reader.do_GetValue(i);
						if (obj != DBNull.Value && obj != null)
						{
							SetValueInner(ref obj, reader.GetFieldTypeInner(i), num2, (short)(i + 1), i);
						}
					}
					num++;
					num2++;
					if (num2 == batchSize)
					{
						m_Fldr.SendRow(batchSize);
						num2 = 0;
					}
				}
				if (num2 != batchSize)
				{
					m_Fldr.SendRow(num2);
				}
			}
			else
			{
				short num3 = -1;
				int num4 = -1;
				while (reader.do_Read())
				{
					for (int i = 0; i < reader.do_FieldCount; i++)
					{
						object obj = reader.do_GetValue(i);
						if (obj == DBNull.Value || obj == null)
						{
							continue;
						}
						foreach (DmBulkCopyColumnMapping columnMapping in m_columnMappings)
						{
							if (columnMapping.SourceOrdinal == -1)
							{
								if (columnMapping.SourceColumn.Equals(dataTable.Rows[i][0]))
								{
									num4 = i;
									if (columnMapping.DestinationOrdinal == -1)
									{
										SetValueInner(ref obj, reader.GetFieldTypeInner(num4), num2, columnMapping.DestinationColumn, num4);
										break;
									}
									num3 = (short)(columnMapping.DestinationOrdinal + 1);
									SetValueInner(ref obj, reader.GetFieldTypeInner(num4), num2, num3, num4);
									break;
								}
								continue;
							}
							if (columnMapping.DestinationOrdinal == -1)
							{
								SetValueInner(ref obj, reader.GetFieldTypeInner(columnMapping.SourceOrdinal), num2, columnMapping.DestinationColumn, columnMapping.SourceOrdinal);
								break;
							}
							num3 = (short)(columnMapping.DestinationOrdinal + 1);
							SetValueInner(ref obj, reader.GetFieldTypeInner(columnMapping.SourceOrdinal), num2, num3, columnMapping.SourceOrdinal);
							break;
						}
					}
					num++;
					num2++;
					if (num2 == batchSize)
					{
						m_Fldr.SendRow(batchSize);
					}
				}
				if (num2 != batchSize)
				{
					m_Fldr.SendRow(num2);
				}
			}
			m_Fldr.Finish();
		}

		public static int GetLenFromType(Type type)
		{
			int result = 8188;
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Int16:
			case TypeCode.UInt16:
				result = 6;
				break;
			case TypeCode.Char:
				result = 2;
				break;
			case TypeCode.SByte:
			case TypeCode.Byte:
				result = 1;
				break;
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Single:
				result = 4;
				break;
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Double:
				result = 8;
				break;
			case TypeCode.String:
				result = -1;
				break;
			case TypeCode.Decimal:
			case TypeCode.DateTime:
				result = 40;
				break;
			case TypeCode.Empty:
			case TypeCode.DBNull:
				result = 0;
				break;
			case TypeCode.Boolean:
				result = 5;
				break;
			case TypeCode.Object:
				result = ((!type.Equals(typeof(DmXDec))) ? ((!type.Equals(typeof(DmIntervalDT))) ? ((!type.Equals(typeof(DmIntervalYM))) ? ((!type.Equals(typeof(byte[]))) ? (-3) : (-2)) : 50) : 50) : 40);
				break;
			}
			return result;
		}

		public int[] GetMaxColLen(DataTable table)
		{
			string serverEncoding = m_Conn.GetConnInstance().ConnProperty.ServerEncoding;
			if (table == null)
			{
				return null;
			}
			int count = table.Columns.Count;
			int count2 = table.Rows.Count;
			int[] array = new int[count];
			for (int i = 0; i < count; i++)
			{
				int num = GetLenFromType(table.Columns[i].DataType);
				switch (num)
				{
				case -3:
				case -1:
				{
					num = 0;
					for (int j = 0; j < count2; j++)
					{
						int num2 = ((table.Rows[j][i] != DBNull.Value && table.Rows[j][i] != null) ? DmConvertion.GetBytes((string)table.Rows[j][i], serverEncoding).Length : 0);
						if (num2 > num)
						{
							num = num2;
						}
					}
					break;
				}
				case -2:
				{
					num = 0;
					for (int j = 0; j < count2; j++)
					{
						int num2 = ((table.Rows[j][i] != DBNull.Value && table.Rows[j][i] != null) ? ((byte[])table.Rows[j][i]).Length : 0);
						if (num2 > num)
						{
							num = num2;
						}
					}
					break;
				}
				}
				array[i] = num;
			}
			return array;
		}

		public void WriteToServer(DataTable table)
		{
			if (m_DestTable == null)
			{
				throw new InvalidOperationException();
			}
			if (m_Fldr == null)
			{
				throw new InvalidOperationException();
			}
			int batchSize = BatchSize;
			m_Fldr.Initilize(m_Conn.ConnProperty, m_DestTable, m_CopyOpt, batchSize, table);
			m_Fldr.SetAttrNecessary(table, GetMaxColLen(table));
			if (m_columnMappings.Count == 0)
			{
				int num = table.Rows.Count;
				int num2 = 0;
				do
				{
					int num3 = ((batchSize <= num) ? batchSize : num);
					for (int i = 0; i < table.Columns.Count; i++)
					{
						SetValueInner(table, table.Columns[i].DataType, num3, (short)(i + 1), i, num2);
					}
					num2 += num3;
					m_Fldr.SendRow(num3);
					num -= num3;
				}
				while (num > 0);
			}
			else
			{
				int num4 = -1;
				short num5 = -1;
				int num = table.Rows.Count;
				int num2 = 0;
				do
				{
					int num3 = ((batchSize <= num) ? batchSize : num);
					for (int i = 0; i < table.Columns.Count; i++)
					{
						foreach (DmBulkCopyColumnMapping columnMapping in m_columnMappings)
						{
							if (columnMapping.SourceOrdinal == -1)
							{
								if (columnMapping.SourceColumn.Equals(table.Columns[i].ColumnName))
								{
									if (columnMapping.DestinationOrdinal == -1)
									{
										SetValueInner(table, table.Columns[i].DataType, num3, columnMapping.DestinationColumn, i, num2);
										break;
									}
									num5 = (short)(columnMapping.DestinationOrdinal + 1);
									SetValueInner(table, table.Columns[i].DataType, num3, num5, i, num2);
									break;
								}
								continue;
							}
							num4 = table.Columns.IndexOf(columnMapping.SourceColumn);
							if (columnMapping.DestinationOrdinal == -1)
							{
								SetValueInner(table, table.Columns[num4].DataType, num3, columnMapping.DestinationColumn, num4, num2);
								break;
							}
							num5 = (short)(columnMapping.DestinationOrdinal + 1);
							SetValueInner(table, table.Columns[num4].DataType, num3, num5, num4, num2);
							break;
						}
					}
					m_Fldr.SendRow(num3);
					num2 += num3;
					num -= num3;
				}
				while (num > 0);
			}
			m_Fldr.Finish();
		}

		public void Close()
		{
			m_Fldr.UnInitilize();
			closed = true;
		}

		private void Dispose(bool disposing)
		{
			if (!closed)
			{
				Close();
			}
		}

		void IDisposable.Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected void OnRowCopied(DmRowsCopiedEventArgs arg)
		{
			if (this.DmRowsCopied != null)
			{
				this.DmRowsCopied(this, arg);
			}
		}
	}
}
