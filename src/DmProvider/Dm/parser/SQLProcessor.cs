using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Dm.util;

namespace Dm.parser
{
	public class SQLProcessor
	{
		public class Parameter
		{
			public byte[] bytes;

			public int ioType;

			public int type;

			public int prec;

			public int scale;

			public Parameter(byte[] bytes, int type, int prec)
			{
				this.bytes = bytes;
				this.type = type;
				this.prec = prec;
			}

			public override string ToString()
			{
				if (bytes == null)
				{
					return null;
				}
				return bytes.ToString();
			}
		}

		public static IList<LVal> lex(string sql)
		{
			Lexer lexer = new Lexer(new StringReader(sql), debug: false);
			int num = 0;
			LVal lVal = null;
			IList<LVal> list = new List<LVal>(32);
			while ((lVal = lexer.yylex()) != null)
			{
				lVal.position = num++;
				list.Add(lVal);
			}
			return list;
		}

		public static IList<LVal> lexSkipWhitespace(string sql, int n)
		{
			Lexer lexer = new Lexer(new StringReader(sql), debug: false);
			int num = 0;
			LVal lVal = null;
			IList<LVal> list = new List<LVal>(32);
			while ((lVal = lexer.yylex()) != null && n > 0)
			{
				lVal.position = num++;
				if (lVal.type != LVal.Type.WHITESPACE_OR_COMMENT)
				{
					list.Add(lVal);
					n--;
				}
			}
			return list;
		}

		public static string escape(string sql, string[] keywords)
		{
			if ((keywords == null || keywords.Length == 0) && sql.IndexOf("{", StringComparison.Ordinal) == -1)
			{
				return sql;
			}
			IDictionary<string, object> dictionary = null;
			if (keywords != null && keywords.Length != 0)
			{
				dictionary = new Dictionary<string, object>();
				foreach (string text in keywords)
				{
					dictionary[text.ToUpper()] = null;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			Stack<bool?> stack = new Stack<bool?>();
			IList<LVal> list = lex(sql);
			for (int j = 0; j < list.Count; j++)
			{
				LVal lVal = list[j];
				if (lVal.type == LVal.Type.NORMAL)
				{
					if (lVal.value.Equals("{"))
					{
						LVal lVal2 = next(list, j + 1);
						if (lVal2 == null || lVal2.type != 0)
						{
							stack.Push(false);
							stringBuilder.Append(lVal.value);
						}
						else if (lVal2.value.Equals("escape", StringComparison.OrdinalIgnoreCase) || lVal2.value.Equals("call", StringComparison.OrdinalIgnoreCase))
						{
							stack.Push(true);
						}
						else if (lVal2.value.Equals("oj", StringComparison.OrdinalIgnoreCase))
						{
							stack.Push(true);
							lVal2.value = string.Empty;
							lVal2.type = LVal.Type.WHITESPACE_OR_COMMENT;
						}
						else if (lVal2.value.Equals("d", StringComparison.OrdinalIgnoreCase))
						{
							stack.Push(true);
							lVal2.value = "date";
						}
						else if (lVal2.value.Equals("t", StringComparison.OrdinalIgnoreCase))
						{
							stack.Push(true);
							lVal2.value = "time";
						}
						else if (lVal2.value.Equals("ts", StringComparison.OrdinalIgnoreCase))
						{
							stack.Push(true);
							lVal2.value = "datetime";
						}
						else if (lVal2.value.Equals("fn", StringComparison.OrdinalIgnoreCase))
						{
							stack.Push(true);
							lVal2.value = string.Empty;
							lVal2.type = LVal.Type.WHITESPACE_OR_COMMENT;
							LVal lVal3 = next(list, lVal2.position + 1);
							if (lVal3 != null && lVal3.type == LVal.Type.NORMAL && lVal3.value.Equals("database", StringComparison.OrdinalIgnoreCase))
							{
								lVal3.value = "cur_database";
							}
						}
						else if (lVal2.value.Equals("?", StringComparison.OrdinalIgnoreCase))
						{
							LVal lVal4 = next(list, lVal2.position + 1);
							if (lVal4 != null && lVal4.type == LVal.Type.NORMAL && lVal4.value.Equals("=", StringComparison.OrdinalIgnoreCase))
							{
								LVal lVal5 = next(list, lVal4.position + 1);
								if (lVal5 != null && lVal5.type == LVal.Type.NORMAL && lVal5.value.Equals("call", StringComparison.OrdinalIgnoreCase))
								{
									stack.Push(true);
									lVal5.value = string.Empty;
									lVal5.type = LVal.Type.WHITESPACE_OR_COMMENT;
								}
								else
								{
									stack.Push(false);
									stringBuilder.Append(lVal.value);
								}
							}
							else
							{
								stack.Push(false);
								stringBuilder.Append(lVal.value);
							}
						}
						else
						{
							stack.Push(false);
							stringBuilder.Append(lVal.value);
						}
					}
					else if (lVal.value.Equals("}", StringComparison.OrdinalIgnoreCase))
					{
						if (stack.Count <= 0 || !stack.Pop().HasValue)
						{
							stringBuilder.Append(lVal.value);
						}
					}
					else if (dictionary != null && dictionary.ContainsKey(lVal.value.ToUpper()))
					{
						stringBuilder.Append("\"" + StringUtil.processDoubleQuoteOfName(lVal.value.ToUpper()) + "\"");
					}
					else
					{
						stringBuilder.Append(lVal.value);
					}
				}
				else if (lVal.type == LVal.Type.STRING)
				{
					stringBuilder.Append("'" + StringUtil.processSingleQuoteOfName(lVal.value) + "'");
				}
				else
				{
					stringBuilder.Append(lVal.value);
				}
			}
			return stringBuilder.ToString();
		}

		private static LVal next(IList<LVal> lvalList, int start)
		{
			LVal lVal = null;
			int count = lvalList.Count;
			for (int i = start; i < count; i++)
			{
				lVal = lvalList[i];
				if (lVal.type != LVal.Type.WHITESPACE_OR_COMMENT)
				{
					break;
				}
			}
			return lVal;
		}

		public static string execOpt(string sql, IList<Parameter> paramList, string serverEncoding)
		{
			StringBuilder stringBuilder = new StringBuilder();
			IList<LVal> list = lex(sql);
			if (list == null || list.Count == 0)
			{
				return sql;
			}
			string value = list[0].value;
			if (!value.Equals("INSERT", StringComparison.CurrentCultureIgnoreCase) && !value.Equals("SELECT", StringComparison.CurrentCultureIgnoreCase) && !value.Equals("UPDATE", StringComparison.CurrentCultureIgnoreCase) && !value.Equals("DELETE", StringComparison.CurrentCultureIgnoreCase))
			{
				return sql;
			}
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				LVal lVal = list[i];
				switch (lVal.type)
				{
				case LVal.Type.NULL:
					stringBuilder.Append("?");
					paramList.Add(new Parameter(null, 25, 4));
					break;
				case LVal.Type.INT:
				{
					stringBuilder.Append("?");
					long num2 = Convert.ToInt64(lVal.value);
					if (num2 <= int.MaxValue && num2 >= int.MinValue)
					{
						paramList.Add(new Parameter(DmConvertion.IntToByteArray(num2), 7, 4));
					}
					else
					{
						paramList.Add(new Parameter(DmConvertion.LongToByteArray(num2), 8, 8));
					}
					break;
				}
				case LVal.Type.DOUBLE:
				{
					stringBuilder.Append("?");
					double d = Convert.ToDouble(lVal.value);
					paramList.Add(new Parameter(DmConvertion.DoubleToByteArray(d), 11, 8));
					break;
				}
				case LVal.Type.DECIMAL:
					stringBuilder.Append("?");
					paramList.Add(new Parameter(DmConvertion.DecimalToByteArray(decimal.Parse(lVal.value, DmConst.invariantCulture)), 9, 0));
					break;
				case LVal.Type.STRING:
					if (lVal.value.Length > 32767)
					{
						stringBuilder.Append("'" + StringUtil.processSingleQuoteOfName(lVal.value) + "'");
						break;
					}
					stringBuilder.Append("?");
					paramList.Add(new Parameter(DmConvertion.GetBytes(lVal.value, serverEncoding), 1, 8188));
					break;
				case LVal.Type.HEX_INT:
					stringBuilder.Append(lVal.value);
					break;
				default:
					stringBuilder.Append(lVal.value);
					break;
				}
				if (num > 0)
				{
					break;
				}
			}
			if (num > 0)
			{
				for (int j = num + 1; j < list.Count; j++)
				{
					stringBuilder.Append(list[j].value);
				}
			}
			return stringBuilder.ToString();
		}

		public static bool hasConst(string sql)
		{
			IList<LVal> list = lex(sql);
			if (list == null || list.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				LVal.Type type = list[i].type;
				if ((uint)(type - 1) <= 4u || type == LVal.Type.NULL)
				{
					return true;
				}
			}
			return false;
		}
	}
}
