using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Dm
{
	public class DocClass : DmDocBase
	{
		private string m_Name;

		private string m_Syntax;

		private string m_Note;

		private Dictionary<MethodInfo, List<string>> m_Method = new Dictionary<MethodInfo, List<string>>();

		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		public string Syntax
		{
			set
			{
				m_Syntax = value;
			}
		}

		public string Note
		{
			set
			{
				m_Note = value;
			}
		}

		public Dictionary<MethodInfo, List<string>> Method => m_Method;

		public void ToHtml()
		{
			FileStream fileStream = new FileStream("c:\\" + m_Name + ".html", FileMode.Create, FileAccess.ReadWrite);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.WriteLine("<html>");
			streamWriter.WriteLine("<head>");
			streamWriter.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
			streamWriter.WriteLine("<title>" + m_Name + "类信息</title>");
			streamWriter.WriteLine("</head>");
			streamWriter.WriteLine("<body>");
			streamWriter.WriteLine("<font color=\"#EA0000\" size=\"5\">" + m_Name + "</font><br>");
			streamWriter.WriteLine("<font size=\"4\">语法</font><br>");
			streamWriter.WriteLine(m_Syntax + "<br>");
			streamWriter.WriteLine("<font size=\"4\">备注</font><br>");
			streamWriter.WriteLine(m_Note + "<br>");
			streamWriter.Flush();
			streamWriter.WriteLine("<font size=\"4\">方法</font><br>");
			foreach (KeyValuePair<MethodInfo, List<string>> item in m_Method)
			{
				List<string> value = item.Value;
				string text = "file:///c:/" + value[0] + ".html";
				streamWriter.WriteLine("<a href=\"" + text + "\">" + item.Key.Name + "</A><br>");
				streamWriter.WriteLine(value[1] + "<br>");
				streamWriter.Flush();
			}
			streamWriter.Close();
			fileStream.Close();
		}
	}
}
