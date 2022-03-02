using System;

namespace Dm
{
	internal class DmExecDirectOp
	{
		private uint type;

		private string sql_buf;

		private int sql_len;

		private short param_num;

		private byte[] values;

		private int val_len;

		private byte[] types;

		private int types_len;

		public short GetParameterCount()
		{
			return param_num;
		}

		public string GetSql()
		{
			return sql_buf;
		}

		public byte[] GetTypes()
		{
			return types;
		}

		public int GetTypeLen()
		{
			return types_len;
		}

		public byte[] GetValues()
		{
			return values;
		}

		public int GetValueLen()
		{
			return val_len;
		}

		public int GetExecOpt(string sql, DmStatement stmt)
		{
			int num = 0;
			int result = 0;
			int num2 = 0;
			try
			{
				if (DmClientLexCall.clex_for_provider_init(out var clexProvider) != 0)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OPT_FAIL);
				}
				if (DmClientLexCall.clex_for_provider(clexProvider, sql) != 0)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_OPT_FAIL);
				}
				num = DmClientLexCall.clex_for_provider_get_first_node(clexProvider, out var data, out var flag);
				int num3 = 1;
				while (num != -1)
				{
					if (num3 == 1)
					{
						if (data.ToUpper().CompareTo("INSERT") == 0)
						{
							type = 1u;
						}
						else if (data.ToUpper().CompareTo("UPDATE") == 0)
						{
							type = 2u;
						}
						else if (data.ToUpper().CompareTo("SELECT") == 0)
						{
							type = 3u;
						}
						else
						{
							if (data.ToUpper().CompareTo("DELETE") != 0)
							{
								return -1;
							}
							type = 4u;
						}
					}
					bool flag2 = true;
					switch (num2)
					{
					case 0:
						if (data.ToUpper().CompareTo("ORDER") == 0)
						{
							num2 = 1;
						}
						break;
					case 1:
						if (data.ToUpper().CompareTo("BY") == 0)
						{
							num2 = 2;
							flag2 = false;
						}
						else
						{
							num2 = 0;
						}
						break;
					}
					type = (uint)((int)flag & -16777216) >> 24;
					int word_len = (ushort)(int)flag & 0xFFFFFF;
					if (!flag2 && num2 == 2)
					{
						type = 0u;
					}
					if (type != 0 && type != 2)
					{
						if (ExecDirectOptAddVal(type, data, stmt) != 0)
						{
							return -1;
						}
					}
					else if (data.CompareTo("'") == 0)
					{
						num = DmClientLexCall.clex_for_provider_get_next_node(clexProvider, out data, out flag);
						if (data.CompareTo("'") == 0)
						{
							string word = "''";
							ExecDirectOptAddWord(word, 2);
						}
						else
						{
							type = (uint)((int)flag & -16777216) >> 24;
							word_len = (ushort)(int)flag & 0xFFFFFF;
							if (ExecDirectOptAddVal(type, data, stmt) != 0)
							{
								return -1;
							}
							num = DmClientLexCall.clex_for_provider_get_next_node(clexProvider, out data, out flag);
						}
					}
					else
					{
						ExecDirectOptAddWord(data, word_len);
					}
					num3++;
					num = DmClientLexCall.clex_for_provider_get_next_node(clexProvider, out data, out flag);
					if (param_num >= 600)
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_OPT_FAIL);
						break;
					}
				}
				DmClientLexCall.clex_for_provider_deinit(clexProvider);
				return result;
			}
			catch (DllNotFoundException)
			{
				return -1;
			}
			catch (Exception ex2)
			{
				Console.WriteLine(ex2.StackTrace);
				return result;
			}
		}

		private void ExecDirectOptAddWord(string word, int word_len)
		{
			int num = ((word_len != 0) ? word_len : word.Length);
			if (sql_len != 0)
			{
				sql_buf = sql_buf.Insert(sql_len, " ");
				sql_len++;
			}
			sql_buf = sql_buf.Insert(sql_len, word);
			sql_len += num;
		}

		private int ExecDirectOptAddVal(uint type, string word, DmStatement stmt)
		{
			int num = 0;
			byte[] array = new byte[100];
			int num2 = 54;
			byte[] sourceArray = new byte[4] { 0, 0, 0, 0 };
			Array.Copy(sourceArray, 0, GetTypes(), types_len, 1);
			types_len++;
			switch (type)
			{
			case 1u:
			{
				long num3 = Convert.ToInt64(word);
				if (num3 < int.MinValue || num3 > int.MaxValue)
				{
					num2 = 8;
					num = 8;
					Array.Copy(DmConvertion.LongToByteArray(num3), 0, array, 0, num);
				}
				else
				{
					num2 = 7;
					num = 4;
					Array.Copy(DmConvertion.IntToByteArray((int)num3), 0, array, 0, num);
				}
				break;
			}
			case 3u:
				num2 = 11;
				num = 8;
				Array.Copy(DmConvertion.DoubleToByteArray(Convert.ToDouble(word)), 0, array, 0, num);
				break;
			case 4u:
				try
				{
					array = new DmXDec().StrToDec(word, 0, 0, dmxdec_direct: false);
				}
				catch (DllNotFoundException)
				{
					return -1;
				}
				break;
			case 5u:
				num2 = 2;
				array = DmConvertion.GetBytes(word, stmt.ConnInst.ConnProperty.ServerEncoding);
				num = word.Length;
				break;
			default:
				return -1;
			case 2u:
				break;
			}
			Array.Copy(DmConvertion.IntToByteArray(num2), 0, types, types_len, 4);
			types_len += 4;
			Array.Copy(DmConvertion.IntToByteArray(num), 0, types, types_len, 4);
			types_len += 4;
			Array.Copy(sourceArray, 0, types, types_len, 4);
			types_len += 4;
			Array.Copy(DmConvertion.ShortToByteArray(num), 0, GetValues(), val_len, 2);
			val_len += 2;
			Array.Copy(array, 0, GetValues(), val_len, num);
			val_len += num;
			string word2 = "?";
			ExecDirectOptAddWord(word2, 1);
			param_num++;
			return 0;
		}

		public void InitExecDirectOpt()
		{
			type = 0u;
			param_num = 0;
			sql_buf = "";
			sql_len = 0;
			val_len = 0;
			types_len = 0;
			values = new byte[65536];
			types = new byte[8192];
		}

		public void DeinitExecDirectOpt()
		{
			sql_buf = null;
			values = null;
			types = null;
		}
	}
}
