using System;

namespace Dm
{
	[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
	public class ShowEnumAttribute : Attribute
	{
		private string m_Describe;

		private string m_Syntax;

		private string m_Member;

		public string Describe => m_Describe;

		public string Syntax => m_Syntax;

		public string Member => m_Member;

		public ShowEnumAttribute(string describe, string syntax, string member)
		{
			m_Describe = describe;
			m_Syntax = syntax;
			m_Member = member;
		}
	}
}
