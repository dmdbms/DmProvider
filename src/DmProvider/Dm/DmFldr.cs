using System;
using System.Data;

namespace Dm
{
	internal sealed class DmFldr
	{
		private DmConnProperty m_ConProps;

		private IntPtr m_Sinstance;

		private IntPtr m_Pinstance;

		public void SetAttrNecessary(IDataReader reader, DataTable dt, int count)
		{
			try
			{
				for (int i = 0; i < count; i++)
				{
					Type fieldTypeInner = ((DmDataReader)reader).GetFieldTypeInner(i);
					int typeCode = GetTypeCode(fieldTypeInner);
					int num = DmBulkCopy.GetLenFromType(fieldTypeInner);
					if (num == -1 || num == -2 || num == -3)
					{
						num = (int)dt.Rows[i][2];
					}
					DmFldrDllCall.FldrSetColMaxLen(m_Pinstance, i + 1, typeCode, num);
				}
				DmFldrDllCall.AllocPinstInfo(m_Pinstance);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		internal int GetTypeCode(Type type)
		{
			if (type.Equals(typeof(byte[])))
			{
				return 19;
			}
			if (type.Equals(typeof(bool)))
			{
				return 5;
			}
			return (int)Type.GetTypeCode(type);
		}

		public void SetAttrNecessary(DataTable dt, int[] len)
		{
			try
			{
				_ = dt.Columns[0];
				for (int i = 0; i < dt.Columns.Count; i++)
				{
					int typeCode = GetTypeCode(dt.Columns[i].DataType);
					DmFldrDllCall.FldrSetColMaxLenByName(m_Pinstance, dt.Columns[i].ColumnName, typeCode, len[i]);
				}
				DmFldrDllCall.AllocPinstInfo(m_Pinstance);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
			catch (Exception)
			{
				throw new DmException(new DmError
				{
					Message = "column not match!"
				});
			}
		}

		private int getCharset(string serverEncoding)
		{
			if (serverEncoding.Equals("UTF-8", StringComparison.OrdinalIgnoreCase))
			{
				return 1;
			}
			if (serverEncoding.Equals("GB18030", StringComparison.OrdinalIgnoreCase))
			{
				return 10;
			}
			return 0;
		}

		public void Initilize(DmConnProperty props, string desttable, DmBulkCopyOptions op, int rows, DataTable table)
		{
			try
			{
				m_ConProps = props;
				DmFldrDllCall.AllocSinst(out m_Sinstance);
				DmFldrDllCall.SetAttr(m_Sinstance, 4, Convert.ToInt32(m_ConProps.PortActual), 0);
				DmFldrDllCall.SetAttr(m_Sinstance, 30, int.MaxValue, 0);
				DmFldrDllCall.SetAttr(m_Sinstance, 37, (int)m_ConProps.MppType, 0);
				DmFldrDllCall.SetAttr(m_Sinstance, 19, getCharset(m_ConProps.ServerEncoding), 0);
				DmFldrDllCall.Initialize(m_Sinstance, 0, m_ConProps.ServerActual, m_ConProps.User, m_ConProps.Pwd, desttable);
				DmFldrDllCall.AllocPinst(m_Sinstance, out m_Pinstance, rows);
				DmFldrDllCall.CheckTable(m_Pinstance, table.Columns.Count);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetValue(int rowid, string colname, object value)
		{
			try
			{
				byte[] bytes = DmConvertion.GetBytes(value.ToString(), m_ConProps.ServerEncoding);
				DmFldrDllCall.SetValBytes(m_Sinstance, m_Pinstance, rowid, colname, bytes, bytes.Length);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetValue(int rowid, short colid, object value)
		{
			try
			{
				byte[] bytes = DmConvertion.GetBytes(value.ToString(), m_ConProps.ServerEncoding);
				DmFldrDllCall.SetValBytes(m_Sinstance, m_Pinstance, rowid, colid, bytes, bytes.Length);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetBytes(int rowid, short colid, byte[] value, int len)
		{
			try
			{
				DmFldrDllCall.SetValBytes(m_Sinstance, m_Pinstance, rowid, colid, value, len);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetBytes(int rowid, string colname, byte[] value, int len)
		{
			try
			{
				DmFldrDllCall.SetValBytes(m_Sinstance, m_Pinstance, rowid, colname, value, len);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetSingle(int rowid, short colid, float value)
		{
			try
			{
				DmFldrDllCall.SetValSingle(m_Sinstance, m_Pinstance, rowid, colid, value, 4);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetSingle(int rowid, string colname, float value)
		{
			try
			{
				DmFldrDllCall.SetValSingle(m_Sinstance, m_Pinstance, rowid, colname, value, 4);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetDouble(int rowid, short colid, double value)
		{
			try
			{
				DmFldrDllCall.SetValDouble(m_Sinstance, m_Pinstance, rowid, colid, value, 8);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetDouble(int rowid, string colname, double value)
		{
			try
			{
				DmFldrDllCall.SetValDouble(m_Sinstance, m_Pinstance, rowid, colname, value, 8);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetInt64(int rowid, short colid, long value)
		{
			try
			{
				DmFldrDllCall.SetValInt64(m_Sinstance, m_Pinstance, rowid, colid, value, 8);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetInt64(int rowid, string colname, long value)
		{
			try
			{
				DmFldrDllCall.SetValInt64(m_Sinstance, m_Pinstance, rowid, colname, value, 8);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetUInt64(int rowid, short colid, ulong value)
		{
			try
			{
				DmFldrDllCall.SetValUInt64(m_Sinstance, m_Pinstance, rowid, colid, value, 8);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetUInt64(int rowid, string colname, ulong value)
		{
			try
			{
				DmFldrDllCall.SetValUInt64(m_Sinstance, m_Pinstance, rowid, colname, value, 8);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetInt32(int rowid, short colid, int value)
		{
			try
			{
				DmFldrDllCall.SetValInt32(m_Sinstance, m_Pinstance, rowid, colid, value, 4);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetInt32(int rowid, string colname, int value)
		{
			try
			{
				DmFldrDllCall.SetValInt32(m_Sinstance, m_Pinstance, rowid, colname, value, 4);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetUInt32(int rowid, short colid, uint value)
		{
			try
			{
				DmFldrDllCall.SetValUInt32(m_Sinstance, m_Pinstance, rowid, colid, value, 4);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetUInt32(int rowid, string colname, uint value)
		{
			try
			{
				DmFldrDllCall.SetValUInt32(m_Sinstance, m_Pinstance, rowid, colname, value, 4);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetInt16(int rowid, short colid, short value)
		{
			try
			{
				DmFldrDllCall.SetValInt16(m_Sinstance, m_Pinstance, rowid, colid, value, 2);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetInt16(int rowid, string colname, short value)
		{
			try
			{
				DmFldrDllCall.SetValInt16(m_Sinstance, m_Pinstance, rowid, colname, value, 2);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetUInt16(int rowid, short colid, ushort value)
		{
			try
			{
				DmFldrDllCall.SetValUInt16(m_Sinstance, m_Pinstance, rowid, colid, value, 2);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetUInt16(int rowid, string colname, ushort value)
		{
			try
			{
				DmFldrDllCall.SetValUInt16(m_Sinstance, m_Pinstance, rowid, colname, value, 2);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetInt8(int rowid, short colid, sbyte value)
		{
			try
			{
				DmFldrDllCall.SetValInt8(m_Sinstance, m_Pinstance, rowid, colid, value, 1);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetInt8(int rowid, string colname, sbyte value)
		{
			try
			{
				DmFldrDllCall.SetValInt8(m_Sinstance, m_Pinstance, rowid, colname, value, 1);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetUInt8(int rowid, short colid, byte value)
		{
			try
			{
				DmFldrDllCall.SetValUInt8(m_Sinstance, m_Pinstance, rowid, colid, value, 1);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SetUInt8(int rowid, string colname, byte value)
		{
			try
			{
				DmFldrDllCall.SetValUInt8(m_Sinstance, m_Pinstance, rowid, colname, value, 1);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SendRow(int rows)
		{
			try
			{
				DmFldrDllCall.SendRows(m_Sinstance, rows);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void SendRow()
		{
			try
			{
				DmFldrDllCall.SendRows(m_Sinstance, 1);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void ResetBind()
		{
			try
			{
				DmFldrDllCall.ResetBind(m_Pinstance);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void Batch()
		{
			try
			{
				long rows = 0L;
				DmFldrDllCall.Batch(m_Sinstance, out rows);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void Finish()
		{
			try
			{
				DmFldrDllCall.Finish(m_Sinstance);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}

		public void UnInitilize()
		{
			try
			{
				DmFldrDllCall.UnInitilize(m_Sinstance);
				DmFldrDllCall.FreePinst(m_Pinstance);
				DmFldrDllCall.FreeSinst(m_Sinstance);
			}
			catch (DllNotFoundException)
			{
				throw new DmException(new DmError
				{
					Message = "The fastloading dll not loading!"
				});
			}
		}
	}
}
