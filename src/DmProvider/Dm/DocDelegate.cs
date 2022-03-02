using System.IO;

namespace Dm
{
	public class DocDelegate : DmDocBase
	{
		private string m_DeleName;

		private string m_Syntax;

		private string m_Parameter;

		private string m_Note;

		public string DeleName
		{
			set
			{
				m_DeleName = value;
			}
		}

		public string Syntax
		{
			set
			{
				m_Syntax = value;
			}
		}

		public string Parameter
		{
			set
			{
				m_Parameter = value;
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
			FileStream fileStream = new FileStream("c:\\" + m_DeleName + ".html", FileMode.Create, FileAccess.ReadWrite);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.WriteLine("<html>");
			streamWriter.WriteLine("<head>");
			streamWriter.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
			streamWriter.WriteLine("<title>" + m_DeleName + "委托信息</title>");
			streamWriter.WriteLine("</head>");
			streamWriter.WriteLine("<body>");
			streamWriter.WriteLine("<font color=\"#EA0000\" size=\"5\">" + m_DeleName + "</font><br>");
			streamWriter.WriteLine("<font size=\"4\">语法</font><br>");
			streamWriter.WriteLine(m_Syntax + "<br>");
			streamWriter.WriteLine("<font size=\"4\">参数</font><br>");
			streamWriter.WriteLine(m_Parameter + "<br>");
			streamWriter.WriteLine("<font size=\"4\">备注</font><br>");
			streamWriter.WriteLine(m_Note + "<br>");
			streamWriter.Flush();
			streamWriter.Close();
			fileStream.Close();
		}
	}
}
