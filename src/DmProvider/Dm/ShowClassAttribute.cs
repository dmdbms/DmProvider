using System;

namespace Dm
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	internal class ShowClassAttribute : Attribute
	{
		private string m_Describe;

		private string m_Syntax;

		private string m_Note;

		public string Describe => m_Describe;

		public string Syntax => m_Syntax;

		public string Note => m_Note;

		public ShowClassAttribute(string describe, string syntax, string note)
		{
			m_Describe = describe;
			m_Syntax = syntax;
			m_Note = note;
		}
	}
}
