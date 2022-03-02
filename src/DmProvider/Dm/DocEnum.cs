using System.IO;

namespace Dm
{
	public class DocEnum : DmDocBase
	{
		private string m_EnumName;

		private string m_Syntax;

		private string m_Member;

		public string EnumName
		{
			set
			{
				m_EnumName = value;
			}
		}

		public string Syntax
		{
			set
			{
				m_Syntax = value;
			}
		}

		public string Member
		{
			set
			{
				m_Member = value;
			}
		}

		public void ToHtml()
		{
			FileStream fileStream = new FileStream("c:\\" + m_EnumName + ".html", FileMode.Create, FileAccess.ReadWrite);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.WriteLine("<html>");
			streamWriter.WriteLine("<head>");
			streamWriter.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
			streamWriter.WriteLine("<title>" + m_EnumName + "枚举信息</title>");
			streamWriter.WriteLine("</head>");
			streamWriter.WriteLine("<body>");
			streamWriter.WriteLine("<font color=\"#EA0000\" size=\"5\">" + m_EnumName + "</font><br>");
			streamWriter.WriteLine("<font size=\"4\">语法</font><br>");
			streamWriter.WriteLine(m_Syntax + "<br>");
			streamWriter.WriteLine("<font size=\"4\">成员</font><br>");
			streamWriter.WriteLine(m_Member + "<br>");
			streamWriter.Flush();
			streamWriter.Close();
			fileStream.Close();
		}
	}
}
