namespace Dm
{
	internal class SQLStateRange
	{
		private int m_Low;

		private int m_High;

		private string m_SQLState;

		public static SQLStateRange[] ranges = new SQLStateRange[3]
		{
			new SQLStateRange(DmErrorDefinition.EC_STR_TRUNC_WARN, DmErrorDefinition.EC_RN_FIND_SEARCH_EMPTY, "01000"),
			new SQLStateRange(DmErrorDefinition.EC_NO_UNLOCK_LOGIN_PRIVILEGE, DmErrorDefinition.EC_NO_INS_PRIVILEGE, "42000"),
			new SQLStateRange(DmErrorDefinition.EC_RN_VIOLATE_NOT_NULL_CONSTAINT, DmErrorDefinition.EC_RN_DUP_KEY, "23000")
		};

		public int GetLow => m_Low;

		public int GetHigh => m_High;

		public string GetSQLState => m_SQLState;

		internal SQLStateRange(int i, int j, string s)
		{
			m_Low = i;
			m_High = j;
			m_SQLState = s;
		}
	}
}
