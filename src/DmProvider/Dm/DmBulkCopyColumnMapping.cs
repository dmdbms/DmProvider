using System;

namespace Dm
{
	public sealed class DmBulkCopyColumnMapping
	{
		internal int m_sourceColumnOrdinal = -1;

		internal string m_sourceColumnName;

		internal int m_destColumnOrdinal = -1;

		internal string m_destColumnName;

		internal int m_internalSourceColumnOrdinal = -1;

		internal int m_internalDestColumnOrdinal = -1;

		public string DestinationColumn
		{
			get
			{
				if (m_destColumnName != null)
				{
					return m_destColumnName;
				}
				return string.Empty;
			}
			set
			{
				m_destColumnOrdinal = -1;
				m_destColumnName = value;
			}
		}

		public int DestinationOrdinal
		{
			get
			{
				return m_destColumnOrdinal;
			}
			set
			{
				if (value < 0)
				{
					throw new IndexOutOfRangeException(value.ToString());
				}
				m_destColumnName = null;
				m_destColumnOrdinal = (m_internalDestColumnOrdinal = value);
			}
		}

		public string SourceColumn
		{
			get
			{
				if (m_sourceColumnName != null)
				{
					return m_sourceColumnName;
				}
				return string.Empty;
			}
			set
			{
				m_sourceColumnOrdinal = -1;
				m_sourceColumnName = value;
			}
		}

		public int SourceOrdinal
		{
			get
			{
				return m_sourceColumnOrdinal;
			}
			set
			{
				if (value < 0)
				{
					throw new IndexOutOfRangeException(value.ToString());
				}
				m_sourceColumnName = null;
				m_sourceColumnOrdinal = (m_internalSourceColumnOrdinal = value);
			}
		}

		public DmBulkCopyColumnMapping()
		{
		}

		public DmBulkCopyColumnMapping(int sourceColumnOrdinal, int destColumnOrdinal)
		{
			SourceOrdinal = (m_internalSourceColumnOrdinal = sourceColumnOrdinal);
			DestinationOrdinal = (m_internalDestColumnOrdinal = destColumnOrdinal);
		}

		public DmBulkCopyColumnMapping(int sourceColumnOrdinal, string destColumnName)
		{
			SourceOrdinal = (m_internalSourceColumnOrdinal = sourceColumnOrdinal);
			DestinationColumn = destColumnName;
		}

		public DmBulkCopyColumnMapping(string sourceColumnName, int destColumnOrdinal)
		{
			SourceColumn = sourceColumnName;
			DestinationOrdinal = (m_internalDestColumnOrdinal = destColumnOrdinal);
		}

		public DmBulkCopyColumnMapping(string sourceColumnName, string destColumnName)
		{
			SourceColumn = sourceColumnName;
			DestinationColumn = destColumnName;
		}
	}
}
