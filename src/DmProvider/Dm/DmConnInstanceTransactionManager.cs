using System.Collections;
using System.Transactions;

namespace Dm
{
	internal class DmConnInstanceTransactionManager
	{
		private static Hashtable _dmConnInstanceInUse = new Hashtable();

		public static DmConnInstance GetDmConnInstanceInTransaction(Transaction transaction)
		{
			lock (_dmConnInstanceInUse.SyncRoot)
			{
				return (DmConnInstance)_dmConnInstanceInUse[transaction.GetHashCode()];
			}
		}

		public static void SetDmConnInstanceInTransaction(DmConnInstance connInstance)
		{
			lock (_dmConnInstanceInUse.SyncRoot)
			{
				_dmConnInstanceInUse[connInstance.CurrentTransaction.BaseTransaction.GetHashCode()] = connInstance;
			}
		}

		public static void RemoveDmConnInstanceInTransaction(Transaction transaction)
		{
			lock (_dmConnInstanceInUse.SyncRoot)
			{
				_dmConnInstanceInUse.Remove(transaction.GetHashCode());
			}
		}
	}
}
