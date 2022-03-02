using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Transactions;

namespace Dm
{
	internal sealed class DmPromotableTransaction : IPromotableSinglePhaseNotification, ITransactionPromoter
	{
		[ThreadStatic]
		private static Stack<DmTransactionScope> globalScopeStack;

		private DmConnection connection;

		private Transaction baseTransaction;

		private Stack<DmTransactionScope> scopeStack;

		public bool inCommitOrRollback;

		public DmConnection Connection
		{
			get
			{
				return connection;
			}
			set
			{
				connection = value;
			}
		}

		public Transaction BaseTransaction
		{
			get
			{
				if (scopeStack.Count > 0)
				{
					return scopeStack.Peek().BaseTransaction;
				}
				return null;
			}
		}

		public bool InRollback
		{
			get
			{
				if (scopeStack.Count > 0 && scopeStack.Peek().RollbackThreadId == Thread.CurrentThread.ManagedThreadId)
				{
					return true;
				}
				return false;
			}
		}

		internal Stack<DmTransactionScope> ScopeStack
		{
			get
			{
				return scopeStack;
			}
			set
			{
				scopeStack = value;
			}
		}

		public DmPromotableTransaction(DmConnection connection, Transaction transaction)
		{
			Connection = connection;
			baseTransaction = transaction;
		}

		void IPromotableSinglePhaseNotification.Initialize()
		{
			string name = Enum.GetName(typeof(System.Transactions.IsolationLevel), baseTransaction.IsolationLevel);
			System.Data.IsolationLevel isolationLevel = (System.Data.IsolationLevel)Enum.Parse(typeof(System.Data.IsolationLevel), name);
			DmTransaction simpleTransaction = Connection.do_BeginDbTransaction(isolationLevel);
			if (globalScopeStack == null)
			{
				globalScopeStack = new Stack<DmTransactionScope>();
			}
			scopeStack = globalScopeStack;
			scopeStack.Push(new DmTransactionScope(connection, baseTransaction, simpleTransaction));
		}

		byte[] ITransactionPromoter.Promote()
		{
			throw new NotSupportedException();
		}

		void IPromotableSinglePhaseNotification.Rollback(SinglePhaseEnlistment singlePhaseEnlistment)
		{
			inCommitOrRollback = true;
			scopeStack.Peek().Rollback(singlePhaseEnlistment);
			scopeStack.Pop();
		}

		void IPromotableSinglePhaseNotification.SinglePhaseCommit(SinglePhaseEnlistment singlePhaseEnlistment)
		{
			inCommitOrRollback = true;
			scopeStack.Pop().SinglePhaseCommit(singlePhaseEnlistment);
		}
	}
}
