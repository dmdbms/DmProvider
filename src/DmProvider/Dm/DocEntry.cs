using System;
using System.Collections.Generic;
using System.IO;

namespace Dm
{
	public class DocEntry : DmDocBase
	{
		private string m_MainDesc = "";

		private Dictionary<Type, List<string>> m_Class = new Dictionary<Type, List<string>>();

		private Dictionary<Type, List<string>> m_Delegates = new Dictionary<Type, List<string>>();

		private Dictionary<Type, List<string>> m_Enum = new Dictionary<Type, List<string>>();

		public string MainDesc
		{
			set
			{
				m_MainDesc = value;
			}
		}

		public Dictionary<Type, List<string>> Class => m_Class;

		public Dictionary<Type, List<string>> Delegate => m_Delegates;

		public Dictionary<Type, List<string>> Enum => m_Enum;

		public void ToHtml()
		{
			FileStream fileStream = new FileStream("c:\\providerOutLine.html", FileMode.Create, FileAccess.ReadWrite);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.WriteLine("<html>");
			streamWriter.WriteLine("<head>");
			streamWriter.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
			streamWriter.WriteLine("<title>程序集信息</title>");
			streamWriter.WriteLine("</head>");
			streamWriter.WriteLine("<body>");
			streamWriter.WriteLine(m_MainDesc + "<br>");
			streamWriter.WriteLine("<font size=\"4\">类</font><br>");
			streamWriter.Flush();
			foreach (KeyValuePair<Type, List<string>> item in m_Class)
			{
				Type key = item.Key;
				List<string> value = item.Value;
				string text = "file:///c:/" + key.FullName + ".html";
				streamWriter.WriteLine("<a href=\"" + text + "\">" + key.FullName + "</A><br>");
				streamWriter.WriteLine(value[0] + "<br>");
				streamWriter.Flush();
			}
			streamWriter.WriteLine("<font size=\"4\">委托</font><br>");
			foreach (KeyValuePair<Type, List<string>> @delegate in m_Delegates)
			{
				Type key2 = @delegate.Key;
				List<string> value2 = @delegate.Value;
				string text2 = "file:///c:/" + key2.FullName + ".html";
				streamWriter.WriteLine("<a href=\"" + text2 + "\">" + key2.FullName + "</A><br>");
				streamWriter.WriteLine(value2[0] + "<br>");
				streamWriter.Flush();
			}
			streamWriter.WriteLine("<font size=\"4\">枚举</font><br>");
			foreach (KeyValuePair<Type, List<string>> item2 in m_Enum)
			{
				Type key3 = item2.Key;
				List<string> value3 = item2.Value;
				string text3 = "file:///c:/" + key3.FullName + ".html";
				streamWriter.WriteLine("<a href=\"" + text3 + "\">" + key3.FullName + "</A><br>");
				streamWriter.WriteLine(value3[0] + "<br>");
				streamWriter.Flush();
			}
			streamWriter.WriteLine("</body>");
			streamWriter.Flush();
			streamWriter.Close();
			fileStream.Close();
		}
	}
}
