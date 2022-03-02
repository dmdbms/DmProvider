using System.Data;
using System.Threading;
using System.Transactions;

namespace Dm
{
	internal class DmTransactionScope
	{
		private DmConnection conn;

		private Transaction baseTransaction;

		private DmTransaction simpleTransaction;

		private int rollbackThreadId;

		public DmConnection Conn
		{
			get
			{
				return conn;
			}
			set
			{
				conn = value;
			}
		}

		public Transaction BaseTransaction
		{
			get
			{
				return baseTransaction;
			}
			set
			{
				baseTransaction = value;
			}
		}

		public DmTransaction SimpleTransaction
		{
			get
			{
				return simpleTransaction;
			}
			set
			{
				simpleTransaction = value;
			}
		}

		public int RollbackThreadId
		{
			get
			{
				return rollbackThreadId;
			}
			set
			{
				rollbackThreadId = value;
			}
		}

		public DmTransactionScope(DmConnection connection, Transaction transaction, DmTransaction simpleTransaction)
		{
			Conn = connection;
			BaseTransaction = transaction;
			SimpleTransaction = simpleTransaction;
		}

		public void Rollback(SinglePhaseEnlistment singlePhaseEnlistment)
		{
			DmConnInstance connInstance = Conn.GetConnInstance();
			lock (connInstance)
			{
				RollbackThreadId = Thread.CurrentThread.ManagedThreadId;
				SimpleTransaction.do_Rollback();
				singlePhaseEnlistment.Aborted();
				DmConnInstanceTransactionManager.RemoveDmConnInstanceInTransaction(BaseTransaction);
				connInstance.CurrentTransaction = null;
				if (Conn.do_State == ConnectionState.Closed)
				{
					conn.SetState(ConnectionState.Open);
					Conn.do_Close();
				}
				RollbackThreadId = 0;
			}
		}

		public void SinglePhaseCommit(SinglePhaseEnlistment singlePhaseEnlistment)
		{
			DmConnInstance connInstance = Conn.GetConnInstance();
			SimpleTransaction.do_Commit();
			singlePhaseEnlistment.Committed();
			DmConnInstanceTransactionManager.RemoveDmConnInstanceInTransaction(BaseTransaction);
			connInstance.CurrentTransaction = null;
			if (Conn.do_State == ConnectionState.Closed)
			{
				Conn.SetState(ConnectionState.Open);
				Conn.do_Close();
			}
		}
	}
}
