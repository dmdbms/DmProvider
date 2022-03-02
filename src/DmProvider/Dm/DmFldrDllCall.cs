using System;
using System.Runtime.InteropServices;

namespace Dm
{
	internal class DmFldrDllCall
	{
		private const int FLDR_SUCCESS = 0;

		private static bool FLDR_SUCCEEDED(int ret)
		{
			if (ret != 0)
			{
				return false;
			}
			return true;
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_alloc(out IntPtr fsinst);

		public static void AllocSinst(out IntPtr fsinst)
		{
			if (!FLDR_SUCCEEDED(fldr_alloc(out fsinst)))
			{
				DmError.ThrowDmException(DmErrorDefinition.EC_FAIL);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_provider_alloc(IntPtr fsinst, out IntPtr fpinst, int Rows);

		public static void AllocPinst(IntPtr fsinst, out IntPtr fpinst, int Rows)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_provider_alloc(fsinst, out fpinst, Rows)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll", EntryPoint = "fldr_check_table")]
		public static extern int check_table(IntPtr fsinst, int cols);

		public static void CheckTable(IntPtr fsinst, int cols)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(check_table(fsinst, cols)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_provider_alloc_info(IntPtr fpinst);

		public static void AllocPinstInfo(IntPtr fpinst)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_provider_alloc_info(fpinst)))
			{
				GetError(fpinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_col_max_len(IntPtr fpinst, int colid, int type, int max_len);

		public static void FldrSetColMaxLen(IntPtr fpinst, int colid, int type, int max_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_col_max_len(fpinst, colid, type, max_len)))
			{
				GetError(fpinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_col_max_len_by_name(IntPtr fpinst, string col_name, int type, int max_len);

		public static void FldrSetColMaxLenByName(IntPtr fpinst, string col_name, int type, int max_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_col_max_len_by_name(fpinst, col_name, type, max_len)))
			{
				GetError(fpinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_free(IntPtr fsinst);

		public static void FreeSinst(IntPtr fsinst)
		{
			if (!FLDR_SUCCEEDED(fldr_free(fsinst)))
			{
				throw new Exception("释放实例失败");
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_provider_free(IntPtr fpinst);

		public static void FreePinst(IntPtr fpinst)
		{
			if (!FLDR_SUCCEEDED(fldr_provider_free(fpinst)))
			{
				throw new Exception("释放实例失败");
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_provider_reset_bind(IntPtr fpinst);

		public static void ResetBind(IntPtr fpinst)
		{
			if (!FLDR_SUCCEEDED(fldr_provider_reset_bind(fpinst)))
			{
				DmError.ThrowDmException(DmErrorDefinition.EC_FAIL);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_name(IntPtr fpinst, int rowid, string colname, string val, int val_len);

		public static void SetVal(IntPtr fsinst, IntPtr fpinst, int rowid, string colname, string val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_name(fpinst, rowid, colname, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_colid(IntPtr fpinst, int rowid, short colid, string val, int val_len);

		public static void SetVal(IntPtr fsinst, IntPtr fpinst, int rowid, short colid, string val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_colid(fpinst, rowid, colid, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_colid_bytes(IntPtr fpinst, int rowid, short colid, byte[] val, int val_len);

		public static void SetValBytes(IntPtr fsinst, IntPtr fpinst, int rowid, short colid, byte[] val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_colid_bytes(fpinst, rowid, colid, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_name_bytes(IntPtr fpinst, int rowid, string colname, byte[] val, int val_len);

		public static void SetValBytes(IntPtr fsinst, IntPtr fpinst, int rowid, string colname, byte[] val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_name_bytes(fpinst, rowid, colname, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_name_float(IntPtr fpinst, int rowid, string colname, float val, int val_len);

		public static void SetValSingle(IntPtr fsinst, IntPtr fpinst, int rowid, string colname, float val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_name_float(fpinst, rowid, colname, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_colid_float(IntPtr fpinst, int rowid, short colid, float val, int val_len);

		public static void SetValSingle(IntPtr fsinst, IntPtr fpinst, int rowid, short colid, float val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_colid_float(fpinst, rowid, colid, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_name_double(IntPtr fpinst, int rowid, string colname, double val, int val_len);

		public static void SetValDouble(IntPtr fsinst, IntPtr fpinst, int rowid, string colname, double val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_name_double(fpinst, rowid, colname, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_colid_double(IntPtr fpinst, int rowid, short colid, double val, int val_len);

		public static void SetValDouble(IntPtr fsinst, IntPtr fpinst, int rowid, short colid, double val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_colid_double(fpinst, rowid, colid, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_name_int64(IntPtr fpinst, int rowid, string colname, long val, int val_len);

		public static void SetValInt64(IntPtr fsinst, IntPtr fpinst, int rowid, string colname, long val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_name_int64(fpinst, rowid, colname, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_colid_int64(IntPtr fpinst, int rowid, short colid, long val, int val_len);

		public static void SetValInt64(IntPtr fsinst, IntPtr fpinst, int rowid, short colid, long val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_colid_int64(fpinst, rowid, colid, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_name_uint64(IntPtr fpinst, int rowid, string colname, ulong val, int val_len);

		public static void SetValUInt64(IntPtr fsinst, IntPtr fpinst, int rowid, string colname, ulong val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_name_uint64(fpinst, rowid, colname, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_colid_uint64(IntPtr fpinst, int rowid, short colid, ulong val, int val_len);

		public static void SetValUInt64(IntPtr fsinst, IntPtr fpinst, int rowid, short colid, ulong val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_colid_uint64(fpinst, rowid, colid, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_name_int32(IntPtr fpinst, int rowid, string colname, int val, int val_len);

		public static void SetValInt32(IntPtr fsinst, IntPtr fpinst, int rowid, string colname, int val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_name_int32(fpinst, rowid, colname, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_colid_int32(IntPtr fpinst, int rowid, short colid, int val, int val_len);

		public static void SetValInt32(IntPtr fsinst, IntPtr fpinst, int rowid, short colid, int val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_colid_int32(fpinst, rowid, colid, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_name_uint32(IntPtr fpinst, int rowid, string colname, uint val, int val_len);

		public static void SetValUInt32(IntPtr fsinst, IntPtr fpinst, int rowid, string colname, uint val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_name_uint32(fpinst, rowid, colname, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_colid_uint32(IntPtr fpinst, int rowid, short colid, uint val, int val_len);

		public static void SetValUInt32(IntPtr fsinst, IntPtr fpinst, int rowid, short colid, uint val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_colid_uint32(fpinst, rowid, colid, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_name_int16(IntPtr fpinst, int rowid, string colname, short val, int val_len);

		public static void SetValInt16(IntPtr fsinst, IntPtr fpinst, int rowid, string colname, short val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_name_int16(fpinst, rowid, colname, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_colid_int16(IntPtr fpinst, int rowid, short colid, short val, int val_len);

		public static void SetValInt16(IntPtr fsinst, IntPtr fpinst, int rowid, short colid, short val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_colid_int16(fpinst, rowid, colid, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_name_uint16(IntPtr fpinst, int rowid, string colname, ushort val, int val_len);

		public static void SetValUInt16(IntPtr fsinst, IntPtr fpinst, int rowid, string colname, ushort val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_name_uint16(fpinst, rowid, colname, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_colid_uint16(IntPtr fpinst, int rowid, short colid, ushort val, int val_len);

		public static void SetValUInt16(IntPtr fsinst, IntPtr fpinst, int rowid, short colid, ushort val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_colid_uint16(fpinst, rowid, colid, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_name_int8(IntPtr fpinst, int rowid, string colname, sbyte val, int val_len);

		public static void SetValInt8(IntPtr fsinst, IntPtr fpinst, int rowid, string colname, sbyte val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_name_int8(fpinst, rowid, colname, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_colid_int8(IntPtr fpinst, int rowid, short colid, sbyte val, int val_len);

		public static void SetValInt8(IntPtr fsinst, IntPtr fpinst, int rowid, short colid, sbyte val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_colid_int8(fpinst, rowid, colid, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_name_uint8(IntPtr fpinst, int rowid, string colname, byte val, int val_len);

		public static void SetValUInt8(IntPtr fsinst, IntPtr fpinst, int rowid, string colname, byte val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_name_uint8(fpinst, rowid, colname, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_set_value_by_colid_uint8(IntPtr fpinst, int rowid, short colid, byte val, int val_len);

		public static void SetValUInt8(IntPtr fsinst, IntPtr fpinst, int rowid, short colid, byte val, int val_len)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_set_value_by_colid_uint8(fpinst, rowid, colid, val, val_len)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll", EntryPoint = "fldr_set_attr")]
		public static extern int fldr_set_attr_1(IntPtr fsinst, int attr, IntPtr value, int length);

		[DllImport("dmfldr_dll.dll", EntryPoint = "fldr_set_attr")]
		public static extern int fldr_set_attr_2(IntPtr fsinst, int attr, string value, int length);

		public static void SetAttr(IntPtr fsinst, int attr, object value, int length)
		{
			int ret;
			switch (attr)
			{
			case 4:
			case 19:
			case 30:
			case 37:
				ret = fldr_set_attr_1(fsinst, attr, (IntPtr)(int)value, length);
				break;
			default:
				ret = fldr_set_attr_2(fsinst, attr, (string)value, length);
				break;
			}
			if (!FLDR_SUCCEEDED(ret))
			{
				DmError.ThrowDmException(DmErrorDefinition.EC_FAIL);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_initialize(IntPtr fsinst, int type, IntPtr conn, string server, string uid, string pwd, string tab);

		public static void Initialize(IntPtr fsinst, int type, string server, string uid, string pwd, string tab)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_initialize(fsinst, 0, (IntPtr)0, server, uid, pwd, tab)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_sendrows(IntPtr fsinst, int rows);

		public static void SendRows(IntPtr fsinst, int rows)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_sendrows(fsinst, rows)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_batch(IntPtr fsinst, out long rows);

		public static void Batch(IntPtr fsinst, out long rows)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_batch(fsinst, out rows)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_finish(IntPtr fsinst);

		public static void Finish(IntPtr fsinst)
		{
			int errcode = -1;
			string errmsg = "";
			if (!FLDR_SUCCEEDED(fldr_finish(fsinst)))
			{
				GetError(fsinst, out errcode, out errmsg);
				DmError.ThrowDmException(errmsg, errcode);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_uninitialize(IntPtr fsinst, int flag);

		public static void UnInitilize(IntPtr fsinst)
		{
			if (!FLDR_SUCCEEDED(fldr_uninitialize(fsinst, 0)))
			{
				DmError.ThrowDmException(DmErrorDefinition.EC_FAIL);
			}
		}

		[DllImport("dmfldr_dll.dll")]
		public static extern int fldr_provider_get_err(IntPtr fsinst, int nth_rec, ref int errcode, ref IntPtr errmsg);

		public static void GetError(IntPtr fsinst, out int errcode, out string errmsg)
		{
			errmsg = "";
			errcode = -1;
			IntPtr errmsg2 = Marshal.StringToBSTR(errmsg);
			fldr_provider_get_err(fsinst, 1, ref errcode, ref errmsg2);
			errmsg = Marshal.PtrToStringAnsi(errmsg2);
		}
	}
}
