using System.IO;

namespace Dm
{
	public class DocMethod : DmDocBase
	{
		private string m_MethodName;

		private string m_Syntax;

		private string m_Exception;

		private string m_Note;

		public string MethodName
		{
			set
			{
				m_MethodName = value;
			}
		}

		public string Syntax
		{
			set
			{
				m_Syntax = value;
			}
		}

		public string ExceptionInfo
		{
			set
			{
				m_Exception = value;
			}
		}

		public string Note
		{
			set
			{
				m_Note = value;
			}
		}

		public void ToHtml()
		{
			FileStream fileStream = new FileStream("c:\\" + m_MethodName + ".html", FileMode.Create, FileAccess.ReadWrite);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.WriteLine("<html>");
			streamWriter.WriteLine("<head>");
			streamWriter.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
			streamWriter.WriteLine("<title>" + m_MethodName.Substring(0, m_MethodName.Length - 1) + "方法信息</title>");
			streamWriter.WriteLine("</head>");
			streamWriter.WriteLine("<body>");
			streamWriter.WriteLine("<font color=\"#EA0000\" size=\"5\">" + m_MethodName.Substring(0, m_MethodName.Length - 1) + "</font><br>");
			streamWriter.WriteLine("<font size=\"4\">语法</font><br>");
			streamWriter.WriteLine(m_Syntax + "<br>");
			streamWriter.WriteLine("<font size=\"4\">异常</font><br>");
			streamWriter.WriteLine(m_Exception + "<br>");
			streamWriter.WriteLine("<font size=\"4\">备注</font><br>");
			streamWriter.WriteLine(m_Note + "<br>");
			streamWriter.Flush();
			streamWriter.Close();
			fileStream.Close();
		}
	}
}
