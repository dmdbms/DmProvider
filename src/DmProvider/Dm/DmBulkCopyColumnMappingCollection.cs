using System;
using System.Collections;

namespace Dm
{
	public sealed class DmBulkCopyColumnMappingCollection : CollectionBase
	{
		public DmBulkCopyColumnMapping this[int index] => (DmBulkCopyColumnMapping)base.List[index];

		internal DmBulkCopyColumnMappingCollection()
		{
		}

		public DmBulkCopyColumnMapping Add(DmBulkCopyColumnMapping bulkCopyMapping)
		{
			if ((bulkCopyMapping.DestinationColumn == null && bulkCopyMapping.DestinationOrdinal == -1) || (bulkCopyMapping.SourceColumn == null && bulkCopyMapping.SourceOrdinal == -1))
			{
				throw new InvalidOperationException("NonColumnMapping");
			}
			base.InnerList.Add(bulkCopyMapping);
			return bulkCopyMapping;
		}

		public DmBulkCopyColumnMapping Add(int sourceColumnIndex, int destColumnIndex)
		{
			DmBulkCopyColumnMapping bulkCopyMapping = new DmBulkCopyColumnMapping(sourceColumnIndex, destColumnIndex);
			return Add(bulkCopyMapping);
		}

		public DmBulkCopyColumnMapping Add(int sourceColumnIndex, string destColumnName)
		{
			DmBulkCopyColumnMapping bulkCopyMapping = new DmBulkCopyColumnMapping(sourceColumnIndex, destColumnName);
			return Add(bulkCopyMapping);
		}

		public DmBulkCopyColumnMapping Add(string sourceColumnName, int destColumnIndex)
		{
			DmBulkCopyColumnMapping bulkCopyMapping = new DmBulkCopyColumnMapping(sourceColumnName, destColumnIndex);
			return Add(bulkCopyMapping);
		}

		public DmBulkCopyColumnMapping Add(string sourceColumnName, string destColumnName)
		{
			DmBulkCopyColumnMapping bulkCopyMapping = new DmBulkCopyColumnMapping(sourceColumnName, destColumnName);
			return Add(bulkCopyMapping);
		}

		internal DmBulkCopyColumnMappingCollection CreateDefaultColumnMappingCollection(int fieldCount)
		{
			DmBulkCopyColumnMappingCollection dmBulkCopyColumnMappingCollection = new DmBulkCopyColumnMappingCollection();
			for (int i = 0; i < fieldCount; i++)
			{
				DmBulkCopyColumnMapping bulkCopyMapping = new DmBulkCopyColumnMapping(i, i);
				dmBulkCopyColumnMappingCollection.Add(bulkCopyMapping);
			}
			return dmBulkCopyColumnMappingCollection;
		}

		public new void Clear()
		{
			base.Clear();
		}

		public bool Contains(DmBulkCopyColumnMapping bulkCopyMapping)
		{
			return -1 != base.InnerList.IndexOf(bulkCopyMapping);
		}

		public void CopyTo(DmBulkCopyColumnMapping[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		public int IndexOf(DmBulkCopyColumnMapping value)
		{
			return base.InnerList.IndexOf(value);
		}

		public void Insert(int index, DmBulkCopyColumnMapping value)
		{
			base.InnerList.Insert(index, value);
		}

		public void Remove(DmBulkCopyColumnMapping value)
		{
			base.InnerList.Remove(value);
		}

		public new void RemoveAt(int index)
		{
			base.RemoveAt(index);
		}
	}
}
