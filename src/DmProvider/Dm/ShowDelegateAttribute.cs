using System;

namespace Dm
{
	[AttributeUsage(AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	public class ShowDelegateAttribute : Attribute
	{
		private string m_Describe;

		private string m_Syntax;

		private string m_Parameter;

		private string m_Note;

		public string Describe => m_Describe;

		public string Syntax => m_Syntax;

		public string Parameter => m_Parameter;

		public string Note => m_Note;

		public ShowDelegateAttribute(string describe, string syntax, string paramter, string note)
		{
			m_Describe = describe;
			m_Syntax = syntax;
			m_Parameter = paramter;
			m_Note = note;
		}
	}
}
