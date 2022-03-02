using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Dm.util
{
	internal static class StringUtil
	{
		public static readonly string EMPTY = string.Empty;

		public static readonly string LINE_SEPARATOR = Environment.NewLine;

		public const string LINUX_LINE_SEPARATOR = "\n";

		public const string INVISIBLE_USERNAME = "……";

		public const int yyyy_MM_dd_HH_mm_ss = 2;

		public static string trim(string str)
		{
			return str?.Trim();
		}

		public static string rightTrim(string str)
		{
			if (str == null)
			{
				return null;
			}
			return ("r" + str).Trim().Substring(1);
		}

		public static string trimToEmpty(string str)
		{
			if (str != null)
			{
				return str.Trim();
			}
			return EMPTY;
		}

		public static bool isEmpty(string str)
		{
			if (str != null)
			{
				return str.Length == 0;
			}
			return true;
		}

		public static bool isNotEmpty(string str)
		{
			if (str != null)
			{
				return str.Length > 0;
			}
			return false;
		}

		public static string substringBetween(string str, string tag)
		{
			return substringBetween(str, tag, tag);
		}

		public static string substringBetween(string str, string open, string close)
		{
			if (str == null || open == null || close == null)
			{
				return null;
			}
			int num = str.IndexOf(open, StringComparison.Ordinal);
			if (num != -1 && str.IndexOf(close, num + open.Length, StringComparison.Ordinal) != -1)
			{
				return str.Substring(num + open.Length, close.Length);
			}
			return null;
		}

		public static bool Equals(string str1, string str2)
		{
			return str1?.Equals(str2) ?? (str2 == null);
		}

		public static bool equalsIgnoreCase(string str1, string str2)
		{
			return str1?.Equals(str2, StringComparison.CurrentCultureIgnoreCase) ?? (str2 == null);
		}

		public static bool isDigit(string str)
		{
			if (isEmpty(str))
			{
				return false;
			}
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				if (!char.IsDigit(str[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static bool isNumerical(string str)
		{
			int num = 0;
			bool flag = true;
			str = trimToEmpty(str);
			if (isEmpty(str))
			{
				return false;
			}
			if (str.StartsWith("+", StringComparison.Ordinal) || str.StartsWith("-", StringComparison.Ordinal))
			{
				if (str.Length == 1)
				{
					return false;
				}
				num = 1;
			}
			for (int i = num; i < str.Length; i++)
			{
				if (!char.IsDigit(str[i]))
				{
					if (!(str[i] == '.' && flag))
					{
						return false;
					}
					flag = false;
				}
			}
			if (str.Length == num + 1 && !flag)
			{
				return false;
			}
			return true;
		}

		public static bool isInteger(string str)
		{
			try
			{
				Convert.ToInt32(str);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool isLong(string str)
		{
			try
			{
				Convert.ToInt64(str);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool isDouble(string str)
		{
			try
			{
				Convert.ToDouble(str);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static string join(object[] array)
		{
			return join(array, null);
		}

		public static string join(object[] array, string separator)
		{
			if (array == null)
			{
				return null;
			}
			int num = array.Length;
			StringBuilder stringBuilder = new StringBuilder((num != 0) ? ((((array[0] == null) ? 16 : array[0].ToString()!.Length) + 1) * num) : 0);
			for (int i = 0; i < num; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(separator);
				}
				if (array[i] != null)
				{
					stringBuilder.Append(array[i]);
				}
			}
			return stringBuilder.ToString();
		}

		public static string replace(string text, string repl, string with, int maximum)
		{
			int num = maximum;
			if (text == null || isEmpty(repl) || with == null || num == 0)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(text.Length);
			int num2 = 0;
			int num3 = 0;
			while ((num3 = text.IndexOf(repl, num2, StringComparison.Ordinal)) != -1)
			{
				stringBuilder.Append(text.Substring(num2, num3 - num2)).Append(with);
				num2 = num3 + repl.Length;
				if (--num == 0)
				{
					break;
				}
			}
			stringBuilder.Append(text.Substring(num2));
			return stringBuilder.ToString();
		}

		public static string replaceAll(string text, string regex, string replacement, bool caseSensitive)
		{
			if (!caseSensitive)
			{
				return Regex.Replace(text, regex, replacement, RegexOptions.IgnoreCase);
			}
			return Regex.Replace(text, regex, replacement);
		}

		public static bool isValidIP(string ip)
		{
			if ("localhost".Equals(ip, StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
			string pattern = "((1[0-9]{1,2}|2([0-4][0-9]|5[0-5])|[0-9\\*]{1,2})\\.(1[0-9]{1,2}|2([0-4][0-9]|5[0-5])|[0-9\\*]{1,2})\\.(1[0-9]{1,2}|2([0-4][0-9]|5[0-5])|[0-9\\*]{1,2})\\.([1][0-9]{1,2}|2([0-4][0-9]|5[0-5])|[0-9\\*]{1,2}))";
			Match match = Regex.Match(ip, pattern);
			if (match.Success)
			{
				return match.Length == ip.Length;
			}
			return false;
		}

		public static bool isValidMac(string mac)
		{
			string pattern = "([0-9A-Fa-f]{2})(-[0-9A-Fa-f]{2}){5}";
			return Regex.Match(mac, pattern).Success;
		}

		public static bool isValidEmail(string email)
		{
			string pattern = "^[A-Z0-9a-z._%+-]+@(([A-Z0-9a-z._]+\\.[A-Za-z]{2,})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))$";
			return Regex.Match(email, pattern).Success;
		}

		public static string bytesToHexString(sbyte[] bs)
		{
			return bytesToHexString(bs, pre: false);
		}

		public static string bytesToHexString(sbyte[] bs, bool pre)
		{
			if (bs == null)
			{
				return null;
			}
			if (bs.Length == 0)
			{
				return "";
			}
			string text = "0123456789ABCDEF";
			StringBuilder stringBuilder = new StringBuilder(bs.Length * 2);
			foreach (sbyte b in bs)
			{
				stringBuilder.Append(text[0xF & (b >> 4)]);
				stringBuilder.Append(text[0xF & b]);
			}
			if (pre)
			{
				return "0x" + stringBuilder.ToString();
			}
			return stringBuilder.ToString();
		}

		public static sbyte[] hexStringToBytes(string s)
		{
			string text = s;
			if (text == null)
			{
				return null;
			}
			sbyte[] result = new sbyte[0];
			bool flag = false;
			text = text.Trim();
			if (text.IndexOf("0x", StringComparison.Ordinal) == 0 || text.IndexOf("0X", StringComparison.Ordinal) == 0)
			{
				text = text.Substring(2, text.Length - 2);
			}
			if (text.Length == 0)
			{
				return result;
			}
			char[] array = text.ToCharArray();
			int num = array.Length;
			char[] array2;
			if (num % 2 == 0)
			{
				array2 = array;
			}
			else
			{
				num++;
				array2 = new char[num];
				array2[0] = '0';
				for (int i = 0; i < num - 1; i++)
				{
					array2[i + 1] = array[i];
				}
			}
			result = new sbyte[num / 2];
			int num2 = 0;
			for (int i = 0; i < array2.Length; i += 2)
			{
				sbyte b = convertHex(array2[i]);
				sbyte b2 = convertHex(array2[i + 1]);
				if (b == -1 || b2 == -1)
				{
					flag = true;
					break;
				}
				result[num2++] = (sbyte)(b * 16 + b2);
			}
			if (flag)
			{
				result = (sbyte[])(object)Encoding.UTF8.GetBytes(text.ToCharArray());
			}
			return result;
		}

		private static sbyte convertHex(char chr)
		{
			if (chr >= '0' && chr <= '9')
			{
				return (sbyte)(chr - 48);
			}
			if (chr >= 'a' && chr <= 'f')
			{
				return (sbyte)(chr - 97 + 10);
			}
			if (chr >= 'A' && chr <= 'F')
			{
				return (sbyte)(chr - 65 + 10);
			}
			return -1;
		}

		public static bool startWithIgnoreCase(string str1, string str2)
		{
			return str1?.ToUpper().StartsWith(str2.ToUpper(), StringComparison.Ordinal) ?? (str2 == null);
		}

		public static bool containsIgnoreCase(string str1, string str2)
		{
			return str1.ToUpper().Contains(str2.ToUpper());
		}

		public static string formatCharset(string s, string srcCharset, string desCharset)
		{
			if (srcCharset.Equals("cp850", StringComparison.CurrentCultureIgnoreCase))
			{
				byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(s.ToCharArray());
				return Encoding.GetEncoding(desCharset).GetString(bytes);
			}
			return s;
		}

		public static int ipToInt(string ip)
		{
			string[] array = ip.Split(".".ToCharArray(), StringSplitOptions.None);
			return (Convert.ToInt32(array[0]) << 24) + (Convert.ToInt32(array[1]) << 16) + (Convert.ToInt32(array[2]) << 8) + Convert.ToInt32(array[3]);
		}

		public static string intToIp(int ip)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append((ip >> 24) & 0xFF);
			stringBuilder.Append(".");
			stringBuilder.Append((ip >> 16) & 0xFF);
			stringBuilder.Append(".");
			stringBuilder.Append((ip >> 8) & 0xFF);
			stringBuilder.Append(".");
			stringBuilder.Append(ip & 0xFF);
			return stringBuilder.ToString();
		}

		public static string toPersent(double radio)
		{
			radio *= 100.0;
			string text = Convert.ToString(radio);
			if (text.Length > 6)
			{
				text = text.Substring(0, 6);
			}
			return text + "%";
		}

		public static string upperFirstChar(string str)
		{
			char c = str[0];
			if (c > '`' && c < '{')
			{
				char[] array = str.ToCharArray();
				array[0] = (char)(c - 32);
				str = new string(array);
			}
			return str;
		}

		public static string getAlignNumString(int num, int maxNum)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (num >= maxNum || num < 0)
			{
				return num.ToString();
			}
			while (maxNum >= 1)
			{
				maxNum /= 10;
				if (num >= 1)
				{
					stringBuilder.Append(num % 10);
					num /= 10;
				}
				else
				{
					stringBuilder.Append("0");
				}
			}
			char[] array = stringBuilder.ToString().ToCharArray();
			Array.Reverse(array);
			return new string(array);
		}

		public static bool isValidTimeZone(string timeZoneStr)
		{
			if (isEmpty(timeZoneStr))
			{
				return false;
			}
			if (timeZoneStr.Length != 6)
			{
				return false;
			}
			char c = timeZoneStr[0];
			if (c != '+' && c != '-')
			{
				return false;
			}
			if (timeZoneStr[3] != ':')
			{
				return false;
			}
			string s = timeZoneStr.Substring(1, 2);
			string s2 = timeZoneStr.Substring(4, 2);
			try
			{
				int num = int.Parse(s);
				if (num < 0 || num > 14)
				{
					return false;
				}
				int num2 = int.Parse(s2);
				if (num2 < 0 || num2 > 60)
				{
					return false;
				}
				int num3 = int.Parse(((c == '+') ? "" : "-") + (num * 60 + num2));
				return num3 <= 840 && num3 >= -779;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static string removeEndSeparatorOfPath(string path, bool isWin)
		{
			int length = path.Length;
			if (isWin && length == 3)
			{
				return path;
			}
			if (!isWin && length == 1)
			{
				return path;
			}
			while ((isWin && path[length - 1] == '\\') || path[length - 1] == '/')
			{
				path = path.Substring(0, path.Length - 1);
				length = path.Length;
			}
			return path;
		}

		public static string addEndSeparatorOfPath(string path, bool isWin)
		{
			int length = path.Length;
			if ((isWin && path[length - 1] == '\\') || path[length - 1] == '/')
			{
				return path;
			}
			path += (isWin ? "\\" : "/");
			return path;
		}

		public static string processDoubleQuoteOfName(string name)
		{
			return processQuoteOfName(name, "\"");
		}

		public static string processDoubleQuoteOfNameForLink(string name)
		{
			return "\"" + processDoubleQuoteOfName(name) + "\"";
		}

		public static string processSingleQuoteOfName(string name)
		{
			return processQuoteOfName(name, "'");
		}

		public static string processQuoteOfName(string name, string quote)
		{
			if (isEmpty(quote) || name == null)
			{
				return name;
			}
			string text = name;
			StringBuilder stringBuilder = new StringBuilder();
			int num = -1;
			int length = quote.Length;
			while ((num = text.IndexOf(quote, StringComparison.Ordinal)) != -1)
			{
				stringBuilder.Append(text.Substring(0, num + length)).Append(quote);
				text = text.Substring(num + length);
			}
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}

		public static string ToString(object obj)
		{
			return obj?.ToString();
		}

		public static bool checkPwdComplexity(string pwdStr)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			if (pwdStr == null || pwdStr.Length == 0)
			{
				return false;
			}
			char[] array = pwdStr.ToCharArray();
			foreach (char c in array)
			{
				switch (c)
				{
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					flag = true;
					break;
				case ' ':
				case '!':
				case '"':
				case '#':
				case '$':
				case '%':
				case '&':
				case '\'':
				case '(':
				case ')':
				case '*':
				case '+':
				case ',':
				case '-':
				case '.':
				case '/':
				case ':':
				case ';':
				case '<':
				case '=':
				case '>':
				case '?':
				case '@':
				case 'A':
				case 'B':
				case 'C':
				case 'D':
				case 'E':
				case 'F':
				case 'G':
				case 'H':
				case 'I':
				case 'J':
				case 'K':
				case 'L':
				case 'M':
				case 'N':
				case 'O':
				case 'P':
				case 'Q':
				case 'R':
				case 'S':
				case 'T':
				case 'U':
				case 'V':
				case 'W':
				case 'X':
				case 'Y':
				case 'Z':
				case '[':
				case '\\':
				case ']':
				case '^':
				case '_':
				case '`':
				case 'a':
				case 'b':
				case 'c':
				case 'd':
				case 'e':
				case 'f':
				case 'g':
				case 'h':
				case 'i':
				case 'j':
				case 'k':
				case 'l':
				case 'm':
				case 'n':
				case 'o':
				case 'p':
				case 'q':
				case 'r':
				case 's':
				case 't':
				case 'u':
				case 'v':
				case 'w':
				case 'x':
				case 'y':
				case 'z':
				case '{':
				case '|':
				case '}':
				case '~':
					if (c >= 'A' && c <= 'Z')
					{
						flag2 = true;
					}
					else if (c >= 'a' && c <= 'z')
					{
						flag3 = true;
					}
					else
					{
						flag4 = true;
					}
					break;
				}
				if (flag && flag2 && flag3 && flag4)
				{
					return true;
				}
			}
			return false;
		}

		private static int min(int one, int two, int three)
		{
			int num = one;
			if (two < num)
			{
				num = two;
			}
			if (three < num)
			{
				num = three;
			}
			return num;
		}

		public static string format2Table(IList<IDictionary> objList, string[] titles, string[] fields, int maxColLen, bool showAll)
		{
			if (objList == null || fields == null)
			{
				return "";
			}
			int num = 0;
			string text = null;
			int[] array = new int[fields.Length];
			for (int i = 0; i < fields.Length; i++)
			{
				array[i] = fields[i].Length;
			}
			foreach (IDictionary obj2 in objList)
			{
				for (int j = 0; j < fields.Length; j++)
				{
					object obj = obj2[fields[j]];
					text = ((obj != null) ? obj.ToString() : "null");
					num = text.Length;
					if (num > array[j])
					{
						array[j] = num;
					}
				}
			}
			for (int k = 0; k < fields.Length; k++)
			{
				if (maxColLen > 0 && array[k] > maxColLen)
				{
					array[k] = maxColLen;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (titles != null)
			{
				IDictionary dictionary = new Dictionary<string, string>(titles.Length);
				for (int l = 0; l < titles.Length; l++)
				{
					dictionary[fields[l]] = titles[l];
				}
				objList.Insert(0, dictionary);
			}
			sepLine(stringBuilder, array);
			foreach (IDictionary obj3 in objList)
			{
				formateLine(stringBuilder, obj3, fields, array, showAll);
				sepLine(stringBuilder, array);
			}
			return stringBuilder.ToString();
		}

		private static void formateLine(StringBuilder output, IDictionary obj, string[] fields, int[] colLens, bool showAll)
		{
			bool flag = false;
			int num = 0;
			string text = null;
			for (int i = 0; i < fields.Length; i++)
			{
				object obj2 = obj[fields[i]];
				if (obj2 == null)
				{
					text = "null";
				}
				else
				{
					text = obj2.ToString();
					text = text.Replace('\t', ' ');
					text = text.Replace('\n', ' ');
					text = text.Replace('\r', ' ');
				}
				num = text.Length;
				if (num <= colLens[i])
				{
					output.Append('|');
					output.Append(text);
					blanks(output, colLens[i] - num);
					if (showAll)
					{
						obj[fields[i]] = "";
					}
				}
				else
				{
					output.Append('|');
					if (showAll)
					{
						output.Append(text.Substring(0, colLens[i]));
						obj[fields[i]] = text.Substring(colLens[i]);
						flag = true;
					}
					else
					{
						output.Append(text.Substring(0, colLens[i] - 3)).Append("...");
					}
				}
			}
			output.Append('|');
			output.Append('\n');
			if (flag)
			{
				formateLine(output, obj, fields, colLens, showAll);
			}
		}

		private static void sepLine(StringBuilder output, int[] colLens)
		{
			output.Append('+');
			foreach (int num in colLens)
			{
				for (int j = 0; j < num; j++)
				{
					output.Append('-');
				}
				output.Append('+');
			}
			output.Append('\n');
		}

		private static void blanks(StringBuilder output, int count)
		{
			while (count > 0)
			{
				output.Append(' ');
				count--;
			}
		}

		public static void appendLine(StringBuilder sb)
		{
			sb.Append(LINE_SEPARATOR);
		}

		public static string replaceSpecialCharFromShell(string str)
		{
			if (isEmpty(str))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in str)
			{
				if (c == '$' || c == '"' || c == '\\')
				{
					stringBuilder.Append('\\');
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		public static long getSizeWithoutUnit(string spaceStr)
		{
			if (isLong(spaceStr))
			{
				return Convert.ToInt64(spaceStr);
			}
			if (spaceStr.Length <= 1)
			{
				return 0L;
			}
			string text = spaceStr.Substring(spaceStr.Length - 1);
			string s = spaceStr.Substring(0, spaceStr.Length - 1);
			if (text.Equals("K", StringComparison.CurrentCultureIgnoreCase))
			{
				return long.Parse(s) * 1024;
			}
			if (text.Equals("M", StringComparison.CurrentCultureIgnoreCase))
			{
				return long.Parse(s) * 1024 * 1024;
			}
			if (text.Equals("G", StringComparison.CurrentCultureIgnoreCase))
			{
				return long.Parse(s) * 1024 * 1024 * 1024;
			}
			if (text.Equals("T", StringComparison.CurrentCultureIgnoreCase))
			{
				return long.Parse(s) * 1024 * 1024 * 1024 * 1024;
			}
			return 0L;
		}

		public static string getSizeWithUnit(long size)
		{
			return getSizeWithUnit(size, "");
		}

		public static string getSizeWithUnit(long size, string unitStr)
		{
			string result = "";
			if (size / 1024 > 2)
			{
				size /= 1024;
				result = size + "K";
				if (unitStr.Equals("K", StringComparison.CurrentCultureIgnoreCase))
				{
					return result;
				}
			}
			if (size / 1024 > 2)
			{
				size /= 1024;
				result = size + "M";
				if (unitStr.Equals("M", StringComparison.CurrentCultureIgnoreCase))
				{
					return result;
				}
			}
			if (size / 1024 > 2)
			{
				size /= 1024;
				result = size + "G";
				if (unitStr.Equals("G", StringComparison.CurrentCultureIgnoreCase))
				{
					return result;
				}
			}
			if (size / 1024 > 2)
			{
				size /= 1024;
				result = size + "T";
				unitStr.Equals("T", StringComparison.CurrentCultureIgnoreCase);
				return result;
			}
			return result;
		}

		public static IList<string> lex(string str, bool ignoreLineSeparator, bool ignoreComment, bool ltWithQuote)
		{
			return lex(str, ignoreLineSeparator, ignoreComment, ltWithQuote, isSql: true);
		}

		public static IList<string> lex(string str, bool ignoreLineSeparator, bool ignoreComment, bool ltWithQuote, bool isSql)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			Stack<char?> stack = new Stack<char?>();
			IList<string> list = new List<string>();
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				char c = str[i];
				if (c != '\r' && c != '\n')
				{
					flag5 = false;
				}
				switch (c)
				{
				case '\t':
				case ' ':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
					}
					else
					{
						decodeWord(stack, list, emptyValid: false);
					}
					break;
				case '(':
				case ')':
				case '{':
				case '}':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					list.Add(c.ToString());
					break;
				case '.':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					if (i + 1 < length && str[i + 1] == '.')
					{
						if (i + 2 < length && str[i + 2] == '.')
						{
							list.Add("...");
							i += 2;
						}
						else
						{
							list.Add("..");
							i++;
						}
					}
					else
					{
						list.Add(c.ToString());
					}
					break;
				case '!':
				case '=':
				case '^':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					if (i + 1 < length && str[i + 1] == '=')
					{
						list.Add(c + "=");
						i++;
					}
					else
					{
						list.Add(c.ToString());
					}
					break;
				case '%':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					if (i + 1 < length && str[i + 1] == '=')
					{
						list.Add(c + "=");
						i++;
					}
					else if (i + 1 < length && str[i + 1] == '%')
					{
						list.Add("%%");
						i++;
					}
					else
					{
						list.Add(c.ToString());
					}
					break;
				case '|':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					if (i + 1 < length && str[i + 1] == '=')
					{
						list.Add(c + "=");
						i++;
					}
					else if (i + 1 < length && str[i + 1] == '|')
					{
						list.Add("||");
						i++;
					}
					else
					{
						list.Add(c.ToString());
					}
					break;
				case '&':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					if (i + 1 < length && str[i + 1] == '=')
					{
						list.Add(c + "=");
						i++;
					}
					else if (i + 1 < length && str[i + 1] == '&')
					{
						list.Add("&&");
						i++;
					}
					else
					{
						list.Add(c.ToString());
					}
					break;
				case '<':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					if (i + 1 < length && str[i + 1] == '=')
					{
						list.Add(c + "=");
						i++;
					}
					else if (i + 1 < length && str[i + 1] == '<')
					{
						if (i + 2 < length && str[i + 2] == '=')
						{
							list.Add("<<=");
							i += 2;
						}
						else
						{
							list.Add("<<");
							i++;
						}
					}
					else if (str[i + 1] == '>')
					{
						list.Add(c + ">");
						i++;
					}
					else
					{
						list.Add(c.ToString());
					}
					break;
				case '>':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					if (i + 1 < length && str[i + 1] == '=')
					{
						list.Add(c + "=");
						i++;
					}
					else if (i + 1 < length && str[i + 1] == '>')
					{
						if (i + 2 < length && str[i + 2] == '=')
						{
							list.Add(">>=");
							i += 2;
						}
						else
						{
							list.Add(">>");
							i++;
						}
					}
					else
					{
						list.Add(c.ToString());
					}
					break;
				case '+':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					if (i + 1 < length && str[i + 1] == '=')
					{
						list.Add(c + "=");
						i++;
					}
					else if (i + 1 < length && str[i + 1] == '+')
					{
						list.Add(c + "+");
						i++;
					}
					else
					{
						list.Add(c.ToString());
					}
					break;
				case '-':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					if (i + 1 < length && str[i + 1] == '=')
					{
						list.Add(c + "=");
						i++;
					}
					else if (i + 1 < length && str[i + 1] == '-')
					{
						if (isSql)
						{
							stack.Push(c);
							stack.Push('-');
							flag = true;
							i++;
						}
						else
						{
							list.Add(c + "-");
							i++;
						}
					}
					else if (i + 1 < length && str[i + 1] == '>')
					{
						list.Add(c + ">");
						i++;
					}
					else
					{
						list.Add(c.ToString());
					}
					break;
				case '*':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						if (flag2 && i + 1 < length && str[i + 1] == '/')
						{
							stack.Push('/');
							if (!ignoreComment)
							{
								decodeWord(stack, list, emptyValid: false);
							}
							else
							{
								stack.Clear();
							}
							flag2 = false;
							i++;
						}
					}
					else
					{
						decodeWord(stack, list, emptyValid: false);
						if (i + 1 < length && str[i + 1] == '=')
						{
							list.Add(c + "=");
							i++;
						}
						else
						{
							list.Add(c.ToString());
						}
					}
					break;
				case '/':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					if (i + 1 < length && str[i + 1] == '*')
					{
						stack.Push(c);
						stack.Push('*');
						flag2 = true;
						i++;
					}
					else if (i + 1 < length && str[i + 1] == '/')
					{
						stack.Push(c);
						stack.Push('/');
						flag = true;
						i++;
					}
					else if (i + 1 < length && str[i + 1] == '=')
					{
						list.Add(c + "=");
						i++;
					}
					else
					{
						list.Add(c.ToString());
					}
					break;
				case '\'':
					if (flag || flag2 || flag4)
					{
						stack.Push(c);
					}
					else if (flag3)
					{
						if (str[i - 1] == '\\')
						{
							stack.Push(c);
							break;
						}
						if (i + 1 < length && str[i + 1] == '\'')
						{
							stack.Push(c);
							i++;
							break;
						}
						if (ltWithQuote)
						{
							stack.Push(c);
						}
						decodeWord(stack, list, emptyValid: true);
						flag3 = false;
					}
					else
					{
						decodeWord(stack, list, emptyValid: false);
						if (ltWithQuote)
						{
							stack.Push(c);
						}
						flag3 = true;
					}
					break;
				case '"':
					if (flag || flag2 || flag3)
					{
						stack.Push(c);
					}
					else if (flag4)
					{
						if (str[i - 1] == '\\')
						{
							stack.Push(c);
						}
						else if (i + 1 < length && str[i + 1] == '"')
						{
							stack.Push(c);
							if (ltWithQuote)
							{
								stack.Push(c);
							}
							i++;
						}
						else
						{
							if (ltWithQuote)
							{
								stack.Push(c);
							}
							decodeWord(stack, list, emptyValid: true);
							flag4 = false;
						}
					}
					else
					{
						decodeWord(stack, list, emptyValid: false);
						if (ltWithQuote)
						{
							stack.Push(c);
						}
						flag4 = true;
					}
					break;
				case '$':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					if (i + 1 < length && str[i + 1] == '$')
					{
						list.Add("$$");
						i++;
					}
					else if (char.IsDigit(str[i + 1]))
					{
						stack.Push(c);
					}
					else
					{
						list.Add(c.ToString());
					}
					break;
				case '#':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					if (i + 1 < length && str[i + 1] == '#')
					{
						list.Add("##");
						i++;
					}
					else
					{
						stack.Push(c);
					}
					break;
				case '\\':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					list.Add(c.ToString());
					break;
				case ',':
				case ';':
				case '?':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					list.Add(c.ToString());
					break;
				case ':':
					if (flag || flag2 || flag3 || flag4)
					{
						stack.Push(c);
						break;
					}
					decodeWord(stack, list, emptyValid: false);
					if (i + 1 < length && str[i + 1] == '=')
					{
						if (isSql)
						{
							list.Add(":=");
							i++;
						}
						else
						{
							list.Add(c.ToString());
						}
					}
					else
					{
						list.Add(c.ToString());
					}
					break;
				case '\n':
				case '\r':
					if (flag2)
					{
						stack.Push(c);
						break;
					}
					if (flag && ignoreComment)
					{
						stack.Clear();
					}
					else
					{
						decodeWord(stack, list, emptyValid: false);
					}
					if (!ignoreLineSeparator && !flag5)
					{
						list.Add(LINE_SEPARATOR);
					}
					flag = false;
					flag5 = true;
					break;
				default:
					stack.Push(c);
					break;
				}
			}
			decodeWord(stack, list, emptyValid: false);
			return list;
		}

		private static void decodeWord(Stack<char?> chrStack, IList<string> wordList, bool emptyValid)
		{
			StringBuilder stringBuilder = new StringBuilder("");
			while (chrStack.Count > 0)
			{
				stringBuilder.Insert(0, chrStack.Pop());
			}
			if (emptyValid || stringBuilder.Length > 0)
			{
				wordList.Add(stringBuilder.ToString());
			}
		}

		public static string formatDir(string dir)
		{
			dir = trimToEmpty(dir);
			if (isNotEmpty(dir))
			{
				string text = new string(Path.DirectorySeparatorChar, 1);
				if (!dir.EndsWith(text, StringComparison.Ordinal))
				{
					dir += text;
				}
			}
			return dir;
		}

		public static string[] split(string s, string seperators)
		{
			if (s == null || seperators == null)
			{
				return null;
			}
			int[] array = new int[s.Length];
			int num = 0;
			int length = s.Length;
			int length2 = seperators.Length;
			for (int i = 0; i < length; i++)
			{
				if (i == 0 && length >= length2 && s.Substring(0, length2).Equals(seperators))
				{
					i += length2 - 1;
					continue;
				}
				for (int j = 0; j < length2; j++)
				{
					if (s[i] == seperators[j])
					{
						array[num] = i;
						num++;
						break;
					}
				}
			}
			string[] array2 = new string[num + 1];
			if (num == 0)
			{
				array2[0] = s;
				return array2;
			}
			array2[0] = s.Substring(0, array[0]);
			for (int k = 1; k < num; k++)
			{
				array2[k] = s.Substring(array[k - 1] + 1, array[k] - (array[k - 1] + 1));
			}
			array2[num] = s.Substring(array[num - 1] + 1, s.Length - (array[num - 1] + 1));
			return array2;
		}

		private static string getStrFromInt(int i)
		{
			if (i >= 10)
			{
				return i.ToString();
			}
			return "0" + i;
		}

		public static void append(StringBuilder sf, string[] objs)
		{
			append(sf, objs, null);
		}

		public static void append(StringBuilder sf, string[] objs, string separate)
		{
			if (objs == null || objs.Length == 0)
			{
				return;
			}
			if (isEmpty(separate))
			{
				for (int i = 0; i < objs.Length; i++)
				{
					sf.Append(objs[i]);
				}
				return;
			}
			sf.Append(objs[0]);
			for (int j = 1; j < objs.Length; j++)
			{
				sf.Append(separate);
				sf.Append(objs[j]);
			}
		}

		public static void append(StringBuilder sf, string str, int count)
		{
			append(sf, str, count, null);
		}

		public static void append(StringBuilder sf, string str, int count, string separate)
		{
			if (isNotEmpty(separate))
			{
				sf.Append(str);
				for (int i = 1; i < count; i++)
				{
					sf.Append(separate);
					sf.Append(str);
				}
			}
			else
			{
				for (int j = 0; j < count; j++)
				{
					sf.Append(str);
				}
			}
		}

		public static string formatTime()
		{
			return DateTime.Now.ToString();
		}

		public static string formatTime(string prefix, string suffix)
		{
			return prefix + formatTime() + suffix;
		}

		public static string subString(string src, string start, string to)
		{
			int num = ((start != null) ? src.IndexOf(start, StringComparison.Ordinal) : 0);
			int num2 = ((to == null) ? src.Length : src.IndexOf(to, StringComparison.Ordinal));
			if (num < 0 || num2 < 0 || num > num2)
			{
				return null;
			}
			if (start != null)
			{
				num += start.Length;
			}
			return src.Substring(num, num2 - num);
		}

		public static long murmurhash2_64(string text)
		{
			sbyte[] array = (sbyte[])(object)Encoding.UTF8.GetBytes(text.ToCharArray());
			int num = array.Length;
			long num2 = 0xE17A1465L ^ (num * -4132994306676758123L);
			int num3 = num / 8;
			for (int i = 0; i < num3; i++)
			{
				int num4 = i * 8;
				long num5 = (long)(((ulong)array[num4] & 0xFFuL) + (((ulong)array[num4 + 1] & 0xFFuL) << 8) + (((ulong)array[num4 + 2] & 0xFFuL) << 16) + (((ulong)array[num4 + 3] & 0xFFuL) << 24) + (((ulong)array[num4 + 4] & 0xFFuL) << 32) + (((ulong)array[num4 + 5] & 0xFFuL) << 40) + (((ulong)array[num4 + 6] & 0xFFuL) << 48) + (((ulong)array[num4 + 7] & 0xFFuL) << 56));
				num5 *= -4132994306676758123L;
				num5 ^= (long)((ulong)num5 >> 47);
				num5 *= -4132994306676758123L;
				num2 ^= num5;
				num2 *= -4132994306676758123L;
			}
			switch (num % 8)
			{
			case 7:
				num2 ^= (long)(array[(num & -8) + 6] & 0xFF) << 48;
				goto case 6;
			case 6:
				num2 ^= (long)(array[(num & -8) + 5] & 0xFF) << 40;
				goto case 5;
			case 5:
				num2 ^= (long)(array[(num & -8) + 4] & 0xFF) << 32;
				goto case 4;
			case 4:
				num2 ^= (long)(array[(num & -8) + 3] & 0xFF) << 24;
				goto case 3;
			case 3:
				num2 ^= (long)(array[(num & -8) + 2] & 0xFF) << 16;
				goto case 2;
			case 2:
				num2 ^= (long)(array[(num & -8) + 1] & 0xFF) << 8;
				goto case 1;
			case 1:
				num2 ^= array[num & -8] & 0xFF;
				num2 *= -4132994306676758123L;
				break;
			}
			num2 ^= (long)((ulong)num2 >> 47);
			num2 *= -4132994306676758123L;
			return num2 ^ (long)((ulong)num2 >> 47);
		}

		public static string getStackTrace(Exception ex)
		{
			return ex.StackTrace;
		}

		public static sbyte[] getBytes(string str, string encoding)
		{
			try
			{
				return (sbyte[])(object)Encoding.GetEncoding(encoding).GetBytes(str);
			}
			catch (Exception)
			{
				DmError.ThrowDmException(DmErrorDefinition.EC_CHAR_CODE_NOT_SUPPORTED);
				return new sbyte[0];
			}
		}

		public static void Main(string[] args)
		{
			Console.Write(lex("call \"(\"(@param_1, @param_2);", ignoreLineSeparator: true, ignoreComment: true, ltWithQuote: true));
		}
	}
}
