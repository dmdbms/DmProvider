using System;
using System.Transactions;
using Dm.util;

namespace Dm
{
	internal class DmNotificationTransaction : IEnlistmentNotification
	{
		internal static Guid RMID = new Guid("11111111-1111-1111-1111-111111111111");

		internal static Dictionary2<string, long, DmConnInstance> TransactionMap = new Dictionary2<string, long, DmConnInstance>();

		private Transaction transaction;

		internal DmNotificationTransaction(Transaction transaction)
		{
			this.transaction = transaction;
		}

		public void Commit(Enlistment enlistment)
		{
			enlistment.Done();
		}

		public void InDoubt(Enlistment enlistment)
		{
			enlistment.Done();
		}

		public void Prepare(PreparingEnlistment preparingEnlistment)
		{
			preparingEnlistment.Prepared();
		}

		public void Rollback(Enlistment enlistment)
		{
			enlistment.Done();
		}
	}
}
