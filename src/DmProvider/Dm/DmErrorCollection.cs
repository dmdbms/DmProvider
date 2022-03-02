using System;
using System.Collections;

namespace Dm
{
	public sealed class DmErrorCollection : ICollection, IEnumerable
	{
		private ArrayList errors = new ArrayList();

		public int Count => errors.Count;

		public DmError this[int index] => (DmError)errors[index];

		bool ICollection.IsSynchronized => false;

		object ICollection.SyncRoot => this;

		internal DmErrorCollection()
		{
		}

		internal void Add(DmError error)
		{
			errors.Add(error);
		}

		public void CopyTo(Array array, int index)
		{
			errors.CopyTo(array, index);
		}

		public void CopyTo(DmError[] array, int index)
		{
			errors.CopyTo(array, index);
		}

		public IEnumerator GetEnumerator()
		{
			return errors.GetEnumerator();
		}
	}
}
