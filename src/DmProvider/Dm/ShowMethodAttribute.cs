using System;

namespace Dm
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class ShowMethodAttribute : Attribute
	{
		private string m_Describe;

		private string m_Syntax;

		private string m_Exception;

		private string m_Note;

		public string Describe => m_Describe;

		public string Syntax => m_Syntax;

		public string ExceptionInfo => m_Exception;

		public string Note => m_Note;

		public ShowMethodAttribute(string describe, string syntax, string exception, string note)
		{
			m_Describe = describe;
			m_Syntax = syntax;
			m_Exception = exception;
			m_Note = note;
		}
	}
}
