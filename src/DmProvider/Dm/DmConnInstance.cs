using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Dm.Config;
using Dm.filter.reconnect;
using Dm.filter.rw;
using Dm.util;

namespace Dm
{
	internal class DmConnInstance
	{
		private DmTransaction m_Tran;

		private DmCsi m_Csi;

		private DmConnection m_Conn;

		private List<DmStatement> m_Stmts = new List<DmStatement>();

		private DmMsg m_SendMsg = new DmMsg();

		private DmMsg m_RecvMsg = new DmMsg();

		private DmConnProperty m_ConnPro;

		private Queue stmt_queue = new Queue();

		private LRUCache<string, DmStatement> pstmtCache;

		private DmPromotableTransaction currentTransaction;

		private object _aliveCheckLockObj = new object();

		private bool _aliveCheck = true;

		internal RWInfo RWInfo { get; set; }

		internal RecoverInfo RecoverInfo { get; set; }

		internal bool AliveCheck
		{
			get
			{
				lock (_aliveCheckLockObj)
				{
					return _aliveCheck;
				}
			}
			set
			{
				lock (_aliveCheckLockObj)
				{
					_aliveCheck = value;
				}
			}
		}

		public DmConnProperty ConnProperty => m_ConnPro;

		public DmTransaction Transaction
		{
			get
			{
				return m_Tran;
			}
			set
			{
				m_Tran = value;
			}
		}

		public DmConnection Conn
		{
			get
			{
				return m_Conn;
			}
			set
			{
				m_Conn = value;
			}
		}

		public List<DmStatement> Stmts => m_Stmts;

		internal DmPromotableTransaction CurrentTransaction
		{
			get
			{
				return currentTransaction;
			}
			set
			{
				currentTransaction = value;
			}
		}

		internal bool ReUsedStmt(DmStatement stmt)
		{
			bool result = true;
			if (m_ConnPro.PreparePooling && stmt.HasPreparedInfo)
			{
				stmt = pstmtCache.Add(stmt.Org_sql, stmt);
				result = false;
			}
			if (stmt != null && stmt_queue.Count < m_ConnPro.PoolSize)
			{
				if (!stmt_queue.Contains(stmt))
				{
					stmt.HasPreparedInfo = false;
					stmt_queue.Enqueue(stmt);
				}
				result = false;
			}
			return result;
		}

		public DmConnInstance(DmConnection conn)
		{
			try
			{
				m_Conn = conn;
				m_ConnPro = conn.ConnProperty;
				if (m_ConnPro.PreparePooling)
				{
					pstmtCache = new LRUCache<string, DmStatement>(m_ConnPro.PreparePoolSize);
				}
				m_Csi = new DmCsi(m_SendMsg, m_RecvMsg, this);
				DBAliveCheckThread.CheckThread.AddConnInstance(this);
			}
			catch (Exception ex)
			{
				Conn.SetState(ConnectionState.Closed);
				throw ex;
			}
		}

		internal DmCsi GetCsi()
		{
			return m_Csi;
		}

		public bool GetAutoCommit()
		{
			return m_ConnPro.AutoCommit;
		}

		internal DmStatement GetStmtFromPool(DmCommand cmd)
		{
			DmStatement dmStatement = null;
			if (m_ConnPro.PreparePooling && cmd != null && cmd.do_CommandText != null && !cmd.do_CommandText.Trim().Equals(""))
			{
				dmStatement = pstmtCache.FindValue(cmd.GetCommandText());
			}
			if (m_ConnPro.StmtPooling && dmStatement == null && stmt_queue.Count > 0)
			{
				dmStatement = stmt_queue.Dequeue() as DmStatement;
			}
			if (dmStatement != null)
			{
				dmStatement.Command = cmd;
				AddStmt(dmStatement);
				return dmStatement;
			}
			return new DmStatement(this, cmd);
		}

		public void SetAutoCommit(bool autoCommit)
		{
			ConnProperty.AutoCommit = autoCommit;
		}

		public void CheckClosed()
		{
			if (m_Csi == null || m_Csi.IsClosed())
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_CONNECTION_CLOSED);
			}
		}

		public void Switched()
		{
			if (m_Tran != null)
			{
				m_Tran.Valid = false;
				m_Tran = null;
			}
			foreach (DmStatement stmt in m_Stmts)
			{
				stmt.Switched = true;
			}
		}

		private void Cleanup(bool keep_tcp)
		{
			if (m_Tran != null)
			{
				if (m_Tran.Valid)
				{
					m_Tran.Clear();
				}
				m_Tran = null;
			}
			if (m_Stmts != null)
			{
				if (keep_tcp)
				{
					CloseAllStmts(keep_tcp);
				}
				else
				{
					CloseAllStmts(keep_tcp);
					ClearAllStmts();
				}
			}
			if (!keep_tcp)
			{
				m_Csi = null;
				m_ConnPro = null;
			}
		}

		public void Close(bool keep_tcp)
		{
			if (m_Csi == null)
			{
				return;
			}
			try
			{
				if (!keep_tcp)
				{
					m_Csi.Close();
					AliveCheck = false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				Cleanup(keep_tcp);
				Conn.SetState(ConnectionState.Closed);
			}
		}

		private void SetTransactionIsolation(DmTransaction m_Tran, IsolationLevel level)
		{
			int num = 1;
			switch (level)
			{
			case IsolationLevel.Unspecified:
			case IsolationLevel.ReadCommitted:
				level = IsolationLevel.ReadCommitted;
				num = 1;
				break;
			case IsolationLevel.Serializable:
				num = 3;
				break;
			case IsolationLevel.ReadUncommitted:
				num = 0;
				break;
			case IsolationLevel.RepeatableRead:
				if (ConnProperty.CompatibleMode == CompatibleMode.MYSQL)
				{
					num = 1;
				}
				else
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_TRAN_ISOLATION);
				}
				break;
			default:
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_TRAN_ISOLATION);
				break;
			}
			if (level != m_ConnPro.IsolationLevel)
			{
				string text = "SET TRANSACTION ISOLATION LEVEL ";
				text = num switch
				{
					0 => text + "READ UNCOMMITTED;", 
					1 => text + "READ COMMITTED;", 
					_ => text + "SERIALIZABLE;", 
				};
				m_Tran.Stmt = GetStmtFromPool(new DmCommand());
				m_Csi.PrepareSql(m_Tran.Stmt.SendMsg, m_Tran.Stmt.RecvMsg, m_Tran.Stmt, text, direct: true, 0);
			}
		}

		public void SetTrxISO(IsolationLevel level)
		{
			ConnProperty.IsolationLevel = level;
		}

		public DmTransaction BeginTrx(IsolationLevel il)
		{
			if (m_Tran != null && m_Tran.Valid)
			{
				throw new InvalidOperationException("不支持并行事务");
			}
			m_Tran = new DmTransaction(this, il);
			SetTransactionIsolation(m_Tran, il);
			SetAutoCommit(autoCommit: false);
			return m_Tran;
		}

		public DmTransaction BeginTrx(IsolationLevel il, bool for_ef)
		{
			if (m_Tran != null && m_Tran.Valid)
			{
				throw new InvalidOperationException("不支持并行事务");
			}
			m_Tran = new DmTransaction(this, il);
			SetTransactionIsolation(m_Tran, il);
			SetAutoCommit(autoCommit: false);
			return m_Tran;
		}

		public void Commit(bool for_new_tran)
		{
			CheckClosed();
			if (GetAutoCommit())
			{
				if (!ConnProperty.AlwaysAllowAutoCommit)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_COMMIT_IN_AUTOCOMMIT_MODE);
				}
				return;
			}
			if (!for_new_tran)
			{
				ClearTrx();
			}
			m_Csi.Commit(m_SendMsg, m_RecvMsg);
			ConnProperty.ClearAutoCommit();
		}

		public void Rollback()
		{
			CheckClosed();
			if (GetAutoCommit())
			{
				if (!ConnProperty.AlwaysAllowAutoCommit)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_COMMIT_IN_AUTOCOMMIT_MODE);
				}
			}
			else
			{
				ClearTrx();
				m_Csi.Rollback(m_SendMsg, m_RecvMsg);
				ConnProperty.ClearAutoCommit();
			}
		}

		private void ClearTrx()
		{
			if (m_Tran != null)
			{
				if (m_Tran.Valid)
				{
					m_Tran.Clear();
				}
				m_Tran = null;
			}
		}

		public void AddStmt(DmStatement stmt)
		{
			lock (m_Stmts)
			{
				m_Stmts.Add(stmt);
			}
		}

		public void RemoveStmt(DmStatement stmt)
		{
			lock (m_Stmts)
			{
				if (!ConnProperty.StmtPooling && m_Stmts.Contains(stmt))
				{
					m_Stmts.Remove(stmt);
				}
			}
		}

		public void CloseAllStmts(bool keep_tcp)
		{
			if (m_Stmts == null)
			{
				return;
			}
			lock (m_Stmts)
			{
				foreach (DmStatement stmt in m_Stmts)
				{
					stmt.Close(remove_from_list: false, keep_tcp);
				}
				m_Stmts.Clear();
			}
		}

		public void ClearAllStmts()
		{
			if (m_Stmts == null)
			{
				return;
			}
			lock (m_Stmts)
			{
				foreach (DmStatement stmt in m_Stmts)
				{
					stmt.Clear();
				}
				m_Stmts.Clear();
			}
		}

		public void RestAllStmt()
		{
			lock (m_Stmts)
			{
				foreach (DmStatement stmt in m_Stmts)
				{
					if (stmt.RsCache != null)
					{
						stmt.RsCache.Reset();
					}
				}
			}
		}
	}
}
