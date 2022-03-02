using System;
using System.Data.Common;

namespace Dm
{
	[Serializable]
	public sealed class DmException : DbException
	{
		private DmErrorCollection m_ErrorCollection = new DmErrorCollection();

		public override string Message => m_ErrorCollection[0].Message;

		public int Number => m_ErrorCollection[0].State;

		public string Schema => m_ErrorCollection[0].Schema;

		public string Table => m_ErrorCollection[0].Table;

		public string Col => m_ErrorCollection[0].Col;

		internal DmException(DmError err)
			: base(err.Message)
		{
			m_ErrorCollection.Add(err);
			Data["Server Error Code"] = err.State;
		}

		internal DmException(string message, DmError err)
			: base(message, null)
		{
			m_ErrorCollection.Add(err);
			Data["Server Error Code"] = err.State;
		}
	}
}
