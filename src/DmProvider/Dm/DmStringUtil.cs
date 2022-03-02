using System.IO;
using NetTaste;

namespace Dm
{
	public class DmStringUtil
	{
		public static string GetEscObjName(string name)
		{
			string text = "";
			if (name == null || name.IndexOf("\"") == -1)
			{
				return name;
			}
			int startIndex = 0;
			while (true)
			{
				int num = name.IndexOf("\"", startIndex);
				if (num == -1)
				{
					break;
				}
				text += name.Substring(startIndex, num + 1);
				text += "\"";
				startIndex = num + 1;
			}
			return text + name.Substring(startIndex);
		}

		public static string GetEscStringName(string name)
		{
			string text = "";
			if (name == null || name.IndexOf("'") == -1)
			{
				return name;
			}
			int startIndex = 0;
			while (true)
			{
				int num = name.IndexOf("'", startIndex);
				if (num == -1)
				{
					break;
				}
				text += name.Substring(startIndex, num + 1);
				text += "'";
				startIndex = num + 1;
			}
			return text + name.Substring(startIndex);
		}

		public static string ReplaceReservedWords(string src, string[] ResveredList)
		{
			if (ResveredList == null)
			{
				return src;
			}
			int num = ResveredList.Length;
			bool flag = false;
			if (num == 0)
			{
				return src;
			}
			if (src.Length > 0)
			{
				Parser parser = new Parser(new Scanner(new MemoryStream(DmConvertion.GetBytesWithNTS(src, null))));
				parser.tab = new SymbolTable(parser);
				parser.Parse();
				if (parser.errors.count == 0)
				{
					for (int i = 0; i < num; i++)
					{
						if (parser.tab.Replace(ResveredList[i]))
						{
							flag = true;
						}
					}
					if (flag)
					{
						return parser.tab.GetString();
					}
					return src;
				}
				return src;
			}
			return src;
		}
	}
}
