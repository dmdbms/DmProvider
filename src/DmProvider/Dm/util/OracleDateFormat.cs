using System;
using System.Collections.Generic;
using System.Text;

namespace Dm.util
{
	internal class OracleDateFormat
	{
		internal abstract class Element
		{
			internal bool hasfield;

			internal int commonParse(string str, int offset, int formatlen1, int formatlen2)
			{
				int? num = null;
				int? num2 = null;
				while (offset < str.Length)
				{
					if (char.IsWhiteSpace(str[offset]))
					{
						offset++;
						continue;
					}
					if (str[offset] == '+' || str[offset] == '-')
					{
						offset++;
					}
					break;
				}
				if (hasfield)
				{
					if (offset == 0 || !char.IsLetterOrDigit(str[offset - 1]))
					{
						num = offset - 1;
					}
					for (int i = 0; i < formatlen1 + 1; i++)
					{
						if (offset + i == str.Length || !char.IsLetterOrDigit(str[offset + i]))
						{
							num2 = offset + i;
							break;
						}
						if (i < formatlen1 && !char.IsDigit(str[offset + i]))
						{
							DmError.ThrowDmException(DmErrorDefinition.ECNET_DATEFORMAT_NOT_DIGIT_CHAR);
						}
					}
				}
				if (num.HasValue && num2.HasValue)
				{
					return num2.Value;
				}
				for (int j = 0; j < formatlen2; j++)
				{
					if (offset + j == str.Length)
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_DATEFORMAT_STR_NOT_MATCH);
					}
					else if (!char.IsDigit(str[offset + j]))
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_DATEFORMAT_NOT_DIGIT_CHAR);
					}
				}
				return offset + formatlen2;
			}

			internal abstract int Parse(string str, int offset, int[] dt);

			internal abstract string Format(int[] dt);
		}

		internal class SpecialElement : Element
		{
			internal char c;

			internal SpecialElement(char c)
			{
				this.c = c;
			}

			internal override int Parse(string str, int offset, int[] dt)
			{
				if (char.IsLetterOrDigit(str[offset]))
				{
					return offset;
				}
				return offset + 1;
			}

			internal override string Format(int[] dt)
			{
				return c.ToString();
			}
		}

		internal class YearElement : Element
		{
			internal int len = 4;

			internal override int Parse(string str, int offset, int[] dt)
			{
				int num = offset;
				offset = commonParse(str, offset, 4, len);
				string text = str.Substring(num, offset - num);
				text = ((text.Length > len) ? text.Substring(text.Length - len, len) : (new string('0', len - text.Length) + text));
				if (len != text.Length)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_DATEFORMAT_STR_NOT_MATCH);
				}
				text = DateTime.Today.Year.ToString().Substring(0, 4 - text.Length) + text;
				int num2 = Convert.ToInt32(text);
				if (num2 > 9999 || num2 < 1)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_YEAR);
				}
				dt[0] = num2;
				return offset;
			}

			internal override string Format(int[] dt)
			{
				return FormatInt(dt[0], len);
			}
		}

		internal class HH12Element : Element
		{
			internal override int Parse(string str, int offset, int[] dt)
			{
				int num = offset;
				offset = commonParse(str, offset, 2, 2);
				int num2 = Convert.ToInt32(str.Substring(num, offset - num));
				if (num2 > 12 || num2 < 1)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_HOUR12);
				}
				dt[3] = num2;
				return offset;
			}

			internal override string Format(int[] dt)
			{
				string text = null;
				int num = dt[3];
				if (num > 12 || num == 0)
				{
					return FormatInt(Math.Abs(num - 12), 2);
				}
				return FormatInt(num, 2);
			}
		}

		internal class HH24Element : Element
		{
			internal override int Parse(string str, int offset, int[] dt)
			{
				int num = offset;
				offset = commonParse(str, offset, 2, 2);
				int num2 = Convert.ToInt32(str.Substring(num, offset - num));
				if (num2 > 23 || num2 < 0)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_HOUR12);
				}
				dt[3] = num2;
				return offset;
			}

			internal override string Format(int[] dt)
			{
				return FormatInt(dt[3], 2);
			}
		}

		internal class MIElement : Element
		{
			internal override int Parse(string str, int offset, int[] dt)
			{
				int num = offset;
				offset = commonParse(str, offset, 2, 2);
				int num2 = Convert.ToInt32(str.Substring(num, offset - num));
				if (num2 > 59 || num2 < 0)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_MINUTE);
				}
				dt[4] = num2;
				return offset;
			}

			internal override string Format(int[] dt)
			{
				return FormatInt(dt[4], 2);
			}
		}

		internal class SSElement : Element
		{
			internal override int Parse(string str, int offset, int[] dt)
			{
				int num = offset;
				offset = commonParse(str, offset, 2, 2);
				int num2 = Convert.ToInt32(str.Substring(num, offset - num));
				if (num2 > 59 || num2 < 0)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_SECOND);
				}
				dt[5] = num2;
				return offset;
			}

			internal override string Format(int[] dt)
			{
				return FormatInt(dt[5], 2);
			}
		}

		internal class MMElement : Element
		{
			internal override int Parse(string str, int offset, int[] dt)
			{
				int num = offset;
				offset = commonParse(str, offset, 2, 2);
				int num2 = Convert.ToInt32(str.Substring(num, offset - num));
				if (num2 > 12 || num2 < 1)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_MONTH);
				}
				dt[1] = num2;
				return offset;
			}

			internal override string Format(int[] dt)
			{
				return FormatInt(dt[1], 2);
			}
		}

		internal class DDElement : Element
		{
			internal override int Parse(string str, int offset, int[] dt)
			{
				int num = offset;
				offset = commonParse(str, offset, 2, 2);
				int num2 = Convert.ToInt32(str.Substring(num, offset - num));
				if (num2 > 31 || num2 < 1)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DAY);
				}
				dt[2] = num2;
				return offset;
			}

			internal override string Format(int[] dt)
			{
				return FormatInt(dt[2], 2);
			}
		}

		internal class MonthElement : Element
		{
			internal bool upperCase;

			internal bool lowerCase;

			private string[] nameList = new string[13]
			{
				"", "January", "February", "March", "April", "May", "June", "July", "August", "September",
				"October", "November", "December"
			};

			private int datelang;

			public MonthElement(int datelang)
			{
				this.datelang = datelang;
			}

			internal override int Parse(string str, int offset, int[] dt)
			{
				if (datelang == 0)
				{
					int num = str.IndexOf("月", offset);
					if (num == -1)
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DATETIME_FORMAT);
					}
					while (offset < str.Length && char.IsWhiteSpace(str[offset]))
					{
						offset++;
					}
					int num2 = Convert.ToInt32(str.Substring(offset, num - offset));
					if (num2 > 12 || num2 < 1)
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DATETIME_FORMAT);
					}
					dt[1] = num2;
					return num + 1;
				}
				int num3 = 0;
				while (offset < str.Length && char.IsWhiteSpace(str[offset]))
				{
					offset++;
				}
				int num4 = offset;
				while (char.IsLetter(str[offset]))
				{
					offset++;
					if (offset == str.Length)
					{
						break;
					}
				}
				str = str.Substring(num4, offset - num4).Trim();
				for (int i = 1; i < nameList.Length; i++)
				{
					if (str.Equals(nameList[i], StringComparison.OrdinalIgnoreCase))
					{
						num3 = i;
						break;
					}
				}
				if (num3 == 0)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DATETIME_FORMAT);
				}
				dt[1] = num3;
				return offset;
			}

			internal override string Format(int[] dt)
			{
				int num = dt[1];
				if (datelang == 0)
				{
					return num + "月";
				}
				string text = nameList[num];
				if (!upperCase)
				{
					if (!lowerCase)
					{
						return text;
					}
					return text.ToLower();
				}
				return text.ToUpper();
			}
		}

		internal class MonElement : Element
		{
			internal bool upperCase;

			internal bool lowerCase;

			private string[] nameList = new string[13]
			{
				"", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep",
				"Oct", "Nov", "Dec"
			};

			private int datelang;

			public MonElement(int datelang)
			{
				this.datelang = datelang;
			}

			internal override int Parse(string str, int offset, int[] dt)
			{
				if (datelang == 0)
				{
					int num = str.IndexOf("月", offset);
					if (num == -1)
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_MONTH);
					}
					while (offset < str.Length && char.IsWhiteSpace(str[offset]))
					{
						offset++;
					}
					int num2 = Convert.ToInt32(str.Substring(offset, num - offset));
					if (num2 > 12 || num2 < 1)
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_MONTH);
					}
					dt[1] = num2;
					return num + 1;
				}
				int num3 = 0;
				while (offset < str.Length && char.IsWhiteSpace(str[offset]))
				{
					offset++;
				}
				if (str.Length - offset < 3)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_MONTH);
				}
				str = str.Substring(offset, 3);
				for (int i = 1; i < nameList.Length; i++)
				{
					if (str.Equals(nameList[i], StringComparison.OrdinalIgnoreCase))
					{
						num3 = i;
						break;
					}
				}
				if (num3 == 0)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_MONTH);
				}
				dt[1] = num3;
				return offset + 3;
			}

			internal override string Format(int[] dt)
			{
				int num = dt[1];
				if (datelang == 0)
				{
					return num + "月";
				}
				string text = nameList[num];
				if (!upperCase)
				{
					if (!lowerCase)
					{
						return text;
					}
					return text.ToLower();
				}
				return text.ToUpper();
			}
		}

		internal class FElement : Element
		{
			internal int len = 6;

			internal override int Parse(string str, int offset, int[] dt)
			{
				int num = offset;
				offset = commonParse(str, offset, len, len);
				for (int i = 0; i < offset - num; i++)
				{
					if (char.IsLetter(str[num + i]))
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_DATEFORMAT_NOT_DIGIT_CHAR);
					}
					else if (!char.IsLetterOrDigit(str[num + i]))
					{
						offset = num + i + 1;
						break;
					}
				}
				if (offset > str.Length)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_DATEFORMAT_STR_NOT_MATCH);
				}
				string text = str.Substring(num, offset - num);
				int num2 = Convert.ToInt32(text);
				num2 = (dt[6] = Convert.ToInt32((double)num2 * Math.Pow(10.0, len - text.Length)));
				return offset;
			}

			internal override string Format(int[] dt)
			{
				return FormatInt(dt[6], len);
			}
		}

		internal class TZHElement : Element
		{
			internal override int Parse(string str, int offset, int[] dt)
			{
				int num = offset;
				offset = commonParse(str, offset, 2, 2);
				int num2 = Convert.ToInt32(str.Substring(num, offset - num).Trim());
				if (num2 > 14 || num2 < -12)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_TZH);
				}
				dt[7] += ((num2 < 0) ? (num2 * -60) : (num2 * 60));
				dt[OFFSET_TIMEZONE_SIGN] ^= ((num2 < 0) ? 1 : 0);
				dt[OFFSET_TIMEZONE_DEFAULT] = 1;
				return offset;
			}

			internal override string Format(int[] dt)
			{
				int num = ((dt[7] != int.MinValue) ? (dt[7] / 60) : 0);
				if (num < 0)
				{
					return "-" + FormatInt(-num, 2);
				}
				return "+" + FormatInt(num, 2);
			}
		}

		internal class TZMElement : Element
		{
			internal override int Parse(string str, int offset, int[] dt)
			{
				int num = offset;
				offset = commonParse(str, offset, 2, 2);
				int num2 = Convert.ToInt32(str.Substring(num, offset - num).Trim());
				if (num2 > 59 || num2 < -59)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_TZM);
				}
				dt[7] += ((num2 < 0) ? (-num2) : num2);
				dt[OFFSET_TIMEZONE_SIGN] ^= ((num2 < 0) ? 1 : 0);
				dt[OFFSET_TIMEZONE_DEFAULT] = 1;
				return offset;
			}

			internal override string Format(int[] dt)
			{
				return FormatInt((dt[7] != int.MinValue) ? (Math.Abs(dt[7]) % 60) : 0, 2);
			}
		}

		internal class AMElement : Element
		{
			private int datelang;

			public AMElement(int datelang)
			{
				this.datelang = datelang;
			}

			internal override int Parse(string str, int offset, int[] dt)
			{
				while (offset < str.Length && char.IsWhiteSpace(str[offset]))
				{
					offset++;
				}
				if (datelang == 0)
				{
					string text = str.Substring(offset, Math.Min(2, str.Length - offset));
					if (text.Equals("下午", StringComparison.OrdinalIgnoreCase))
					{
						dt[OFFSET_PM] = 1;
					}
					else if (text.Equals("上午", StringComparison.OrdinalIgnoreCase))
					{
						dt[OFFSET_PM] = 0;
					}
					else
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_AM);
					}
					return offset + 2;
				}
				if (str.Substring(offset, Math.Min(2, str.Length - offset)).Equals("PM", StringComparison.OrdinalIgnoreCase))
				{
					dt[OFFSET_PM] = 1;
					return offset + 2;
				}
				if (str.Substring(offset, Math.Min(2, str.Length - offset)).Equals("AM", StringComparison.OrdinalIgnoreCase))
				{
					dt[OFFSET_PM] = 0;
					return offset + 2;
				}
				if (str.Substring(offset, Math.Min(4, str.Length - offset)).Equals("P.M.", StringComparison.OrdinalIgnoreCase))
				{
					dt[OFFSET_PM] = 1;
					return offset + 4;
				}
				if (str.Substring(offset, Math.Min(4, str.Length - offset)).Equals("A.M.", StringComparison.OrdinalIgnoreCase))
				{
					dt[OFFSET_PM] = 0;
					return offset + 4;
				}
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_AM);
				DmError.ThrowDmException(DmErrorDefinition.EC_NOT_SUPPORTED);
				return offset + 2;
			}

			internal override string Format(int[] dt)
			{
				int num = dt[3];
				if (datelang == 0)
				{
					if (num <= 12)
					{
						return "上午";
					}
					return "下午";
				}
				if (num <= 12)
				{
					return "AM";
				}
				return "PM";
			}
		}

		internal const int OFFSET_YEAR = 0;

		internal const int OFFSET_MONTH = 1;

		internal const int OFFSET_DAY = 2;

		internal const int OFFSET_HOUR = 3;

		internal const int OFFSET_MINUTE = 4;

		internal const int OFFSET_SECOND = 5;

		internal const int OFFSET_MILLISECOND = 6;

		internal const int OFFSET_TIMEZONE = 7;

		internal const int DT_LEN = 8;

		internal const int INVALID_VALUE = int.MinValue;

		internal const int LEN_MM = 2;

		internal const int LEN_DD = 2;

		internal const int LEN_YEAR = 4;

		internal const int LEN_HH = 2;

		internal const int LEN_MI = 2;

		internal const int LEN_SS = 2;

		internal const int LEN_FF = 6;

		internal const int LEN_AM = 2;

		internal const int LEN_TZH = 2;

		internal const int LEN_TZM = 2;

		internal const int LEN_SPECIAL = 1;

		internal int Count;

		internal YearElement Y = new YearElement();

		internal HH12Element HH12 = new HH12Element();

		internal HH24Element HH24 = new HH24Element();

		internal MIElement MI = new MIElement();

		internal SSElement SS = new SSElement();

		internal AMElement AM;

		internal MonthElement Month;

		internal MonElement Mon;

		internal MMElement MM = new MMElement();

		internal DDElement DD = new DDElement();

		internal TZHElement TZH = new TZHElement();

		internal TZMElement TZM = new TZMElement();

		internal FElement F = new FElement();

		public List<object> formatElementList = new List<object>();

		public static int OFFSET_TIMEZONE_SIGN = 8;

		public static int OFFSET_PM = 10;

		public static int OFFSET_TIMEZONE_DEFAULT = 9;

		public string pattern;

		public int datelang;

		public OracleDateFormat(int datelang)
		{
			this.datelang = datelang;
			AM = new AMElement(datelang);
			Month = new MonthElement(datelang);
			Mon = new MonElement(datelang);
		}

		internal int[] Parse(string str)
		{
			int[] dt = new int[11];
			int num = 0;
			str = str.Trim();
			if (str.Length < Count + Count - 1)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_DATEFORMAT_STR_TOO_SHORT);
			}
			for (int i = 0; i < formatElementList.Count; i++)
			{
				if (i + 1 < formatElementList.Count - 1)
				{
					SpecialElement specialElement = formatElementList[i] as SpecialElement;
					if (specialElement != null && formatElementList[i + 1] is FElement && specialElement.c != str[num])
					{
						i += 2;
						continue;
					}
				}
				num = ((Element)formatElementList[i]).Parse(str, num, dt);
				if (num == str.Length)
				{
					break;
				}
				if (i == formatElementList.Count - 1 && num < str.Length)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_DATEFORMAT_STR_NOT_MATCH);
				}
			}
			return AdjustDateTime(dt);
		}

		private int[] AdjustDateTime(int[] dt)
		{
			int[] array = new int[13]
			{
				0, 31, 28, 31, 30, 31, 30, 31, 31, 30,
				31, 30, 31
			};
			int num = dt[0];
			int num2 = dt[1];
			int num3 = dt[2];
			_ = dt[7];
			int num4 = (int)DateTimeOffset.Now.Offset.TotalMinutes;
			dt[0] = ((num == 0) ? DateTime.Now.Year : num);
			dt[1] = ((num2 == 0) ? DateTime.Now.Month : num2);
			dt[2] = ((num3 == 0) ? 1 : num3);
			if (dt[OFFSET_TIMEZONE_DEFAULT] == 0)
			{
				dt[7] = num4;
				dt[OFFSET_TIMEZONE_SIGN] = ((num4 < 0) ? 1 : 0);
			}
			if (dt[OFFSET_PM] == 1)
			{
				dt[3] = (dt[3] + 12) % 24;
			}
			if (DateTime.IsLeapYear(dt[0]))
			{
				array[2] = 29;
			}
			if (dt[2] > array[dt[1]] || dt[2] < 1)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DATETIME_FORMAT);
			}
			if (dt[7] > 840 || dt[7] < -779)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_TZM);
			}
			return dt;
		}

		private static OracleDateFormat GetFormat(int datelang)
		{
			return new OracleDateFormat(datelang);
		}

		internal static int[] Round(int[] dt, int scale)
		{
			_ = new int[7]
			{
				dt[6],
				dt[5],
				dt[4],
				dt[3],
				dt[2],
				dt[1],
				dt[0]
			};
			int num = dt[6];
			bool flag = false;
			for (int i = 0; i < 6 - scale; i++)
			{
				flag = false;
				if (num % 10 > 5)
				{
					flag = true;
				}
				num = num / 10 + (flag ? 1 : 0);
			}
			num = (int)(Math.Pow(10.0, 6 - scale) * (double)num);
			if (num > 999999)
			{
				dt[6] = 0;
				flag = true;
			}
			else
			{
				dt[6] = num;
				flag = false;
			}
			if (flag)
			{
				dt[5]++;
			}
			if (dt[5] > 59)
			{
				dt[5] = 0;
				flag = true;
			}
			else
			{
				flag = false;
			}
			if (flag)
			{
				dt[4]++;
			}
			if (dt[4] > 59)
			{
				dt[4] = 0;
				flag = true;
			}
			else
			{
				flag = false;
			}
			if (flag)
			{
				dt[3]++;
			}
			if (dt[3] > 23)
			{
				dt[3] = 0;
				flag = true;
			}
			else
			{
				flag = false;
			}
			if (flag)
			{
				dt[2]++;
			}
			int[] array = new int[13]
			{
				0, 31, 28, 31, 30, 31, 30, 31, 31, 30,
				31, 30, 31
			};
			if (dt[2] > array[dt[1]])
			{
				dt[2] = 1;
				flag = true;
			}
			else
			{
				flag = false;
			}
			if (flag)
			{
				dt[1]++;
			}
			if (dt[1] > 12)
			{
				dt[1] = 1;
				flag = true;
			}
			else
			{
				flag = false;
			}
			if (flag)
			{
				dt[0]++;
			}
			return dt;
		}

		public static int[] Parse(string str, string pattern, int datelang)
		{
			OracleDateFormat format = GetFormat(datelang);
			format.SetPattern(pattern);
			return format.Parse(str);
		}

		public static string Format(int[] dt, string pattern, int datelang)
		{
			OracleDateFormat format = GetFormat(datelang);
			format.SetPattern(pattern);
			return format.Format(dt);
		}

		internal string Format(int[] dt)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object formatElement in formatElementList)
			{
				if (formatElement is Element)
				{
					stringBuilder.Append(((Element)formatElement).Format(dt));
				}
				else
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DATETIME_FORMAT);
				}
			}
			return stringBuilder.ToString();
		}

		public void SetPattern(string pattern)
		{
			if ((pattern != null || this.pattern != null) && !pattern.Equals(this.pattern))
			{
				this.pattern = pattern;
				formatElementList.Clear();
				AnalysePattern(pattern);
			}
		}

		private List<object> AnalysePattern(string pattern)
		{
			pattern = pattern.Trim();
			char[] array = pattern.ToCharArray();
			int num = 0;
			int num2 = array.Length;
			while (num < num2)
			{
				char c = array[num];
				switch (c)
				{
				case 'H':
				case 'h':
					if (num + 4 <= num2 && new string(array, num, 4).Equals("HH24", StringComparison.OrdinalIgnoreCase))
					{
						formatElementList.Add(HH24);
						num += 4;
					}
					else if (num + 4 <= num2 && new string(array, num, 4).Equals("HH12", StringComparison.OrdinalIgnoreCase))
					{
						formatElementList.Add(HH12);
						num += 4;
					}
					else if (num + 2 <= num2 && new string(array, num, 2).Equals("HH", StringComparison.OrdinalIgnoreCase))
					{
						formatElementList.Add(HH12);
						num += 2;
					}
					else
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DATETIME_FORMAT);
					}
					break;
				case 'M':
				case 'm':
					if (num + 5 <= num2 && new string(array, num, 5).Equals("month", StringComparison.Ordinal))
					{
						Month.upperCase = false;
						Month.lowerCase = true;
						formatElementList.Add(Month);
						num += 5;
						Count++;
					}
					else if (num + 5 <= num2 && new string(array, num, 5).Equals("Month", StringComparison.Ordinal))
					{
						Month.upperCase = false;
						Month.lowerCase = false;
						formatElementList.Add(Month);
						num += 5;
						Count++;
					}
					else if (num + 5 <= num2 && new string(array, num, 5).Equals("MONTH", StringComparison.Ordinal))
					{
						Month.upperCase = true;
						Month.lowerCase = false;
						formatElementList.Add(Month);
						num += 5;
						Count++;
					}
					else if (num + 3 <= num2 && new string(array, num, 3).Equals("MON", StringComparison.Ordinal))
					{
						Mon.upperCase = true;
						Mon.lowerCase = false;
						formatElementList.Add(Mon);
						num += 3;
						Count++;
					}
					else if (num + 3 <= num2 && new string(array, num, 3).Equals("Mon", StringComparison.Ordinal))
					{
						Mon.upperCase = false;
						Mon.lowerCase = false;
						formatElementList.Add(Mon);
						num += 3;
						Count++;
					}
					else if (num + 3 <= num2 && new string(array, num, 3).Equals("mon", StringComparison.Ordinal))
					{
						Mon.upperCase = false;
						Mon.lowerCase = true;
						formatElementList.Add(Mon);
						num += 3;
						Count++;
					}
					else if (num + 2 <= num2 && new string(array, num, 2).Equals("MM", StringComparison.OrdinalIgnoreCase))
					{
						formatElementList.Add(MM);
						num += 2;
						Count++;
					}
					else if (num + 2 <= num2 && new string(array, num, 2).Equals("MI", StringComparison.OrdinalIgnoreCase))
					{
						formatElementList.Add(MI);
						num += 2;
					}
					else
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DATETIME_FORMAT);
					}
					break;
				case 'S':
				case 's':
					if (num + 2 <= num2 && new string(array, num, 2).Equals("SS", StringComparison.OrdinalIgnoreCase))
					{
						formatElementList.Add(SS);
						num += 2;
					}
					else
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DATETIME_FORMAT);
					}
					break;
				case 'F':
				case 'f':
					if (num + 3 <= num2 && new string(array, num, 3).Equals("FF1", StringComparison.OrdinalIgnoreCase))
					{
						F.len = 1;
						formatElementList.Add(F);
						num += 3;
					}
					else if (num + 3 <= num2 && new string(array, num, 3).Equals("FF2", StringComparison.OrdinalIgnoreCase))
					{
						F.len = 2;
						formatElementList.Add(F);
						num += 3;
					}
					else if (num + 3 <= num2 && new string(array, num, 3).Equals("FF3", StringComparison.OrdinalIgnoreCase))
					{
						F.len = 3;
						formatElementList.Add(F);
						num += 3;
					}
					else if (num + 3 <= num2 && new string(array, num, 3).Equals("FF4", StringComparison.OrdinalIgnoreCase))
					{
						F.len = 4;
						formatElementList.Add(F);
						num += 3;
					}
					else if (num + 3 <= num2 && new string(array, num, 3).Equals("FF5", StringComparison.OrdinalIgnoreCase))
					{
						F.len = 5;
						formatElementList.Add(F);
						num += 3;
					}
					else if (num + 3 <= num2 && new string(array, num, 3).Equals("FF6", StringComparison.OrdinalIgnoreCase))
					{
						F.len = 6;
						formatElementList.Add(F);
						num += 3;
					}
					else if (num + 2 <= num2 && new string(array, num, 2).Equals("FF", StringComparison.OrdinalIgnoreCase))
					{
						F.len = 6;
						formatElementList.Add(F);
						num += 2;
					}
					else
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DATETIME_FORMAT);
					}
					break;
				case 'A':
				case 'P':
				case 'a':
				case 'p':
					if (num + 4 <= num2 && new string(array, num, 4).Equals("A.M.", StringComparison.OrdinalIgnoreCase))
					{
						formatElementList.Add(AM);
						num += 4;
					}
					else if (num + 4 <= num2 && new string(array, num, 4).Equals("P.M.", StringComparison.OrdinalIgnoreCase))
					{
						formatElementList.Add(AM);
						num += 4;
					}
					else if (num + 2 <= num2 && new string(array, num, 2).Equals("AM", StringComparison.OrdinalIgnoreCase))
					{
						formatElementList.Add(AM);
						num += 2;
					}
					else if (num + 2 <= num2 && new string(array, num, 2).Equals("PM", StringComparison.OrdinalIgnoreCase))
					{
						formatElementList.Add(AM);
						num += 2;
					}
					else
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DATETIME_FORMAT);
					}
					break;
				case 'D':
				case 'd':
					if (num + 2 <= num2 && new string(array, num, 2).Equals("DD", StringComparison.OrdinalIgnoreCase))
					{
						formatElementList.Add(DD);
						num += 2;
					}
					else
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DATETIME_FORMAT);
					}
					Count++;
					break;
				case 'T':
				case 't':
					if (num + 3 <= num2 && new string(array, num, 3).Equals("TZH", StringComparison.OrdinalIgnoreCase))
					{
						formatElementList.Add(TZH);
						num += 3;
					}
					else if (num + 3 <= num2 && new string(array, num, 3).Equals("TZM", StringComparison.OrdinalIgnoreCase))
					{
						formatElementList.Add(TZM);
						num += 3;
					}
					else
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DATETIME_FORMAT);
					}
					break;
				case 'Y':
				case 'y':
					if (num + 4 <= num2 && new string(array, num, 4).Equals("YYYY", StringComparison.OrdinalIgnoreCase))
					{
						Y.len = 4;
						formatElementList.Add(Y);
						num += 4;
					}
					else if (num + 3 <= num2 && new string(array, num, 3).Equals("YYY", StringComparison.OrdinalIgnoreCase))
					{
						Y.len = 3;
						formatElementList.Add(Y);
						num += 3;
					}
					else if (num + 2 <= num2 && new string(array, num, 2).Equals("YY", StringComparison.OrdinalIgnoreCase))
					{
						Y.len = 2;
						formatElementList.Add(Y);
						num += 2;
					}
					else if (new string(array, num, 1).Equals("Y", StringComparison.OrdinalIgnoreCase))
					{
						Y.len = 1;
						formatElementList.Add(Y);
						num++;
					}
					else
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DATETIME_FORMAT);
					}
					Count++;
					break;
				default:
					if (!char.IsLetterOrDigit(c))
					{
						formatElementList.Add(new SpecialElement(c));
						num++;
					}
					else
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_DATETIME_FORMAT);
					}
					break;
				}
			}
			for (int num3 = formatElementList.Count - 1; num3 >= 0; num3--)
			{
				if (!(formatElementList[num3] is SpecialElement) && (num3 == 0 || formatElementList[num3 - 1] is SpecialElement) && (num3 + 1 == formatElementList.Count || formatElementList[num3 + 1] is SpecialElement))
				{
					((Element)formatElementList[num3]).hasfield = true;
				}
			}
			return formatElementList;
		}

		private static string FormatInt(int value, int len)
		{
			int num = (int)Math.Pow(10.0, len);
			if (value >= num)
			{
				value %= num;
			}
			value += num;
			return value.ToString().Substring(1);
		}

		private string FormatMilliSecond(int ms, int len)
		{
			string text = null;
			text = ((ms < 10) ? ("00000" + ms) : ((ms < 100) ? ("0000" + ms) : ((ms < 1000) ? ("000" + ms) : ((ms < 10000) ? ("00" + ms) : ((ms >= 100000) ? ms.ToString() : ("0" + ms))))));
			if (len < 6)
			{
				text = text.Substring(0, len);
			}
			return text;
		}
	}
}
