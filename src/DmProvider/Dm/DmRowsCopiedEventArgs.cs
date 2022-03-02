using System;

namespace Dm
{
	public class DmRowsCopiedEventArgs : EventArgs
	{
		private long m_rowsCopied;

		private bool m_abort;

		public long RowsCopied => m_rowsCopied;

		public bool Abort
		{
			get
			{
				return m_abort;
			}
			set
			{
				m_abort = value;
			}
		}

		public DmRowsCopiedEventArgs(long rowsCopied)
		{
			m_rowsCopied = rowsCopied;
		}
	}
}
