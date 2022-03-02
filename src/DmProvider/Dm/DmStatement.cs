using System;
using System.Collections.Generic;
using System.Data;
using Dm.parser;

namespace Dm
{
	internal class DmStatement
	{
		private int m_Stmtid;

		private DmConnInstance m_ConnInst;

		private DmCsi m_Csi;

		internal DmMsg SendMsg = new DmMsg();

		internal DmMsg RecvMsg = new DmMsg();

		private DmInfo m_ResultInfo;

		private DmResultSetCache m_RsCache;

		internal DmDataReader m_CurrentRs;

		private string m_explain;

		private string m_CursorName;

		private bool hasPreparedInfo;

		private string m_org_sql;

		private bool m_new_col_desc;

		private byte m_cursorType = 1;

		private bool m_Closed;

		internal bool stmtSerial;

		private bool m_Switched;

		private DmCommand m_Cmd;

		private long m_CursorUpdateRow;

		private bool m_ResultFetchOver;

		private bool m_PreExeced;

		private bool m_PrimaryPrepared;

		private bool m_StandbyPrepared;

		private int m_CommandTimeOut = 30;

		internal int Stmtid
		{
			get
			{
				return m_Stmtid;
			}
			set
			{
				m_Stmtid = value;
			}
		}

		internal string PrimaryCursorName
		{
			get
			{
				return m_CursorName;
			}
			set
			{
				m_CursorName = value;
			}
		}

		internal string CursorName
		{
			get
			{
				return PrimaryCursorName;
			}
			set
			{
				PrimaryCursorName = value;
			}
		}

		public bool HasPreparedInfo
		{
			get
			{
				return hasPreparedInfo;
			}
			set
			{
				hasPreparedInfo = value;
			}
		}

		public string Org_sql
		{
			get
			{
				return m_org_sql;
			}
			set
			{
				m_org_sql = value;
			}
		}

		internal bool New_col_desc
		{
			get
			{
				return m_new_col_desc;
			}
			set
			{
				m_new_col_desc = value;
			}
		}

		public byte CursorType
		{
			get
			{
				return m_cursorType;
			}
			set
			{
				m_cursorType = value;
			}
		}

		public bool Switched
		{
			get
			{
				return m_Switched;
			}
			set
			{
				m_Switched = value;
			}
		}

		internal bool PreExecuted
		{
			get
			{
				return m_PreExeced;
			}
			set
			{
				m_PreExeced = value;
			}
		}

		internal bool ResultFectchOver
		{
			get
			{
				return m_ResultFetchOver;
			}
			set
			{
				m_ResultFetchOver = false;
			}
		}

		internal DmInfo ResultInfo
		{
			get
			{
				return m_ResultInfo;
			}
			set
			{
				m_ResultInfo = value;
			}
		}

		internal DmCommand Command
		{
			get
			{
				return m_Cmd;
			}
			set
			{
				m_Cmd = value;
			}
		}

		internal DmConnInstance ConnInst => m_ConnInst;

		internal int Handle
		{
			get
			{
				return Stmtid;
			}
			set
			{
				Stmtid = Handle;
			}
		}

		internal DmInfo DbInfo => m_ResultInfo;

		internal DmCsi Csi => m_Csi;

		internal int CommandTimeOut
		{
			get
			{
				return m_CommandTimeOut;
			}
			set
			{
				m_CommandTimeOut = value;
			}
		}

		internal bool PrimaryPrepared
		{
			get
			{
				return m_PrimaryPrepared;
			}
			set
			{
				m_PrimaryPrepared = value;
			}
		}

		internal bool StandbyPrepared
		{
			get
			{
				return m_StandbyPrepared;
			}
			set
			{
				m_StandbyPrepared = value;
			}
		}

		public bool Prepared
		{
			get
			{
				return PrimaryPrepared;
			}
			set
			{
				PrimaryPrepared = value;
			}
		}

		public DmResultSetCache RsCache
		{
			get
			{
				return m_RsCache;
			}
			set
			{
				m_RsCache = value;
			}
		}

		internal void SetCommandTime()
		{
			if (m_Cmd != null)
			{
				m_CommandTimeOut = m_Cmd.do_CommandTimeout;
			}
		}

		internal void SetCommandTime(int timeout)
		{
			m_CommandTimeOut = timeout;
		}

		private void InitializeState()
		{
			m_Csi = m_ConnInst.GetCsi();
			m_Closed = false;
			m_Switched = false;
			SetCommandTime();
			m_Csi.AllocStmtHandle(this, SendMsg, RecvMsg, ref m_new_col_desc);
			long num = Stmtid;
			if (Stmtid < 0)
			{
				num = 0xFFFFFFFFu & num;
			}
			PrimaryCursorName = "DM_CURSOR_" + num;
		}

		public DmStatement(DmConnInstance conn, DmCommand cmd)
		{
			m_ResultInfo = new DmInfo(conn);
			m_ConnInst = conn;
			m_Cmd = cmd;
			InitializeState();
			conn.AddStmt(this);
			m_CommandTimeOut = cmd.do_CommandTimeout;
			SetCommandTime();
			if (cmd != null)
			{
				stmtSerial = cmd.GetStmtSerial();
			}
			else
			{
				stmtSerial = ((m_ConnInst.ConnProperty.IsolationLevel == IsolationLevel.Serializable) ? true : false);
			}
		}

		internal void SetExplain(string explain)
		{
			m_explain = explain;
		}

		internal string GetExplain()
		{
			return m_explain;
		}

		internal void GetDataReader(CommandBehavior behavior)
		{
			if (m_CurrentRs != null)
			{
				m_CurrentRs.do_Close();
			}
			if (Command != null)
			{
				if (Command.RetRefCursorStmt != null)
				{
					m_CurrentRs = new DmDataReader(Command.RetRefCursorStmt.m_RsCache, Command.RetRefCursorStmt.m_ResultInfo, behavior);
					m_CurrentRs.m_StartRow = 0L;
					return;
				}
				if (Command.RefCursorStmtArr != null && Command.RefCursorStmtArr.Count > 0)
				{
					DmStatement dmStatement = (DmStatement)Command.RefCursorStmtArr[Command.RefCursorStmtArr_cur];
					Command.IncRefCur();
					if (dmStatement != null && dmStatement.m_RsCache != null && dmStatement.m_ResultInfo != null)
					{
						m_CurrentRs = new DmDataReader(dmStatement.m_RsCache, dmStatement.m_ResultInfo, behavior);
						m_CurrentRs.m_StartRow = 0L;
						return;
					}
				}
			}
			if (!m_ResultInfo.GetHasResultSet())
			{
				m_CurrentRs = new DmDataReader(m_ResultInfo, behavior, this);
				m_CurrentRs.m_StartRow = 0L;
				return;
			}
			if (m_RsCache == null)
			{
				m_RsCache = new DmResultSetCache(this, m_ResultInfo.GetColumnsInfo().Length, m_ResultInfo.GetRowCount());
			}
			m_CurrentRs = new DmDataReader(m_RsCache, m_ResultInfo, behavior);
			m_CurrentRs.m_StartRow = 0L;
		}

		public DmDataReader ExecuteQuery(string sql, CommandBehavior behavior)
		{
			PreExecuteCheck();
			Org_sql = sql;
			if (ConnInst.ConnProperty.EscapeProcess)
			{
				try
				{
					Org_sql = SQLProcessor.escape(sql, ConnInst.ConnProperty.ResveredList);
				}
				catch (Exception)
				{
				}
			}
			if (ConnInst.ConnProperty.C2p == 1)
			{
				List<SQLProcessor.Parameter> list = new List<SQLProcessor.Parameter>();
				try
				{
					Org_sql = SQLProcessor.execOpt(Org_sql, list, ConnInst.ConnProperty.ServerEncoding);
				}
				catch (Exception)
				{
				}
				if (list.Count > 0)
				{
					try
					{
						m_Csi.ExecDirectOpt(this, list);
					}
					catch (Exception)
					{
						Org_sql = sql;
						SetCommandTime();
						m_Csi.PrepareSql(SendMsg, RecvMsg, this, sql, direct: true, 0);
					}
				}
				else
				{
					Org_sql = sql;
					SetCommandTime();
					m_Csi.PrepareSql(SendMsg, RecvMsg, this, sql, direct: true, 0);
				}
			}
			else
			{
				SetCommandTime();
				m_Csi.PrepareSql(SendMsg, RecvMsg, this, sql, direct: true, 0);
			}
			GetDataReader(behavior);
			return m_CurrentRs;
		}

		public int ExecuteUpdate(string sql)
		{
			PreExecuteCheck();
			Org_sql = sql;
			if (ConnInst.ConnProperty.EscapeProcess)
			{
				try
				{
					Org_sql = SQLProcessor.escape(sql, ConnInst.ConnProperty.ResveredList);
				}
				catch (Exception)
				{
				}
			}
			if (ConnInst.ConnProperty.C2p == 1)
			{
				List<SQLProcessor.Parameter> list = new List<SQLProcessor.Parameter>();
				try
				{
					Org_sql = SQLProcessor.execOpt(Org_sql, list, ConnInst.ConnProperty.ServerEncoding);
				}
				catch (Exception)
				{
				}
				if (list.Count > 0)
				{
					try
					{
						m_Csi.ExecDirectOpt(this, list);
					}
					catch (Exception)
					{
						Org_sql = sql;
						SetCommandTime();
						m_Csi.PrepareSql(SendMsg, RecvMsg, this, sql, direct: true, 0);
					}
				}
				else
				{
					Org_sql = sql;
					SetCommandTime();
					m_Csi.PrepareSql(SendMsg, RecvMsg, this, sql, direct: true, 0);
				}
			}
			else
			{
				SetCommandTime();
				m_Csi.PrepareSql(SendMsg, RecvMsg, this, sql, direct: true, 0);
			}
			if (m_ResultInfo.GetHasResultSet())
			{
				return -1;
			}
			return (int)m_ResultInfo.GetRowCount();
		}

		public void PrepareSql(string sql)
		{
			PreExecuteCheck();
			if (sql.Equals(""))
			{
				return;
			}
			SetCommandTime();
			Org_sql = sql;
			if (ConnInst.ConnProperty.EscapeProcess)
			{
				try
				{
					Org_sql = SQLProcessor.escape(sql, ConnInst.ConnProperty.ResveredList);
				}
				catch (Exception)
				{
				}
			}
			m_Csi.PrepareSql(SendMsg, RecvMsg, this, sql, direct: false, 0);
		}

		public DmDataReader ExecutePreparedQuery(CommandBehavior behavior)
		{
			PreExecuteCheck();
			CheckParameterBound();
			SetCommandTime();
			m_Csi.ExecutePrepared(this, m_ResultInfo);
			GetDataReader(behavior);
			return m_CurrentRs;
		}

		public int ExecutePreparedUpdate()
		{
			PreExecuteCheck();
			CheckParameterBound();
			SetCommandTime();
			m_Csi.ExecutePrepared(this, m_ResultInfo);
			if (m_ResultInfo.GetHasResultSet())
			{
				return -1;
			}
			return (int)m_ResultInfo.GetRowCount();
		}

		internal DmInfo GetInfo()
		{
			return m_ResultInfo;
		}

		private void CheckParameterBound()
		{
			DmParameterInternal[] ParamsInfo = null;
			m_ResultInfo.GetParamsInfo(out ParamsInfo);
			if (ParamsInfo == null)
			{
				return;
			}
			for (int i = 0; i < m_ResultInfo.GetParameterCount(); i++)
			{
				if (ParamsInfo[i].GetInOutType() != 1 && !ParamsInfo[i].GetInDataBound())
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_UNBINDED_PARAMETER);
				}
			}
		}

		public void Reset()
		{
			m_ResultInfo = null;
			m_RsCache = null;
			m_ResultFetchOver = false;
			m_CursorUpdateRow = 0L;
			m_PreExeced = false;
			m_Switched = false;
		}

		public void CloseCursor()
		{
			SetCommandTime();
			m_Csi.CloseHandle(this);
		}

		public void Clear()
		{
			if (m_Cmd != null)
			{
				m_Cmd.Statement = null;
				m_Cmd.RefCursorStmtArr.Clear();
				m_Cmd.RetRefCursorStmt = null;
			}
			m_ConnInst = null;
			m_Csi = null;
			m_CurrentRs = null;
			m_Closed = true;
			m_Switched = false;
			m_ResultInfo = null;
		}

		public bool IsClosed()
		{
			return m_Closed;
		}

		public void Close()
		{
			if (m_Closed)
			{
				return;
			}
			m_ConnInst.RemoveStmt(this);
			if (m_ConnInst != null && (m_ConnInst.ConnProperty.PreparePooling || m_ConnInst.ConnProperty.StmtPooling) && !m_ConnInst.ReUsedStmt(this))
			{
				return;
			}
			try
			{
				if (m_Cmd != null)
				{
					SetCommandTime();
				}
				m_Csi.FreeHandle(SendMsg, RecvMsg, this);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				Clear();
			}
		}

		public void Close(bool remove_from_list, bool keep_tcp)
		{
			if (m_Closed)
			{
				return;
			}
			if (keep_tcp && m_ConnInst != null && (m_ConnInst.ConnProperty.PreparePooling || m_ConnInst.ConnProperty.StmtPooling))
			{
				if (remove_from_list)
				{
					m_ConnInst.RemoveStmt(this);
				}
				if (!m_ConnInst.ReUsedStmt(this))
				{
					return;
				}
			}
			if (remove_from_list)
			{
				m_ConnInst.RemoveStmt(this);
			}
			try
			{
				if (m_Cmd != null)
				{
					SetCommandTime();
				}
				m_Csi.FreeHandle(SendMsg, RecvMsg, this);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				Clear();
			}
		}

		private void PreExecuteCheck()
		{
			PreExecuted = false;
			if (m_Closed)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_STATEMENT_HANDLE_CLOSED);
			}
			if (m_Switched)
			{
				Close();
				DmError.ThrowDmException("执行了主备切换，句柄无效", DmErrorDefinition.ERROR_MASTER_SLAVE_SWITCHED);
			}
		}

		internal void SetCursorName(string name)
		{
			if (name.Length == 0 || name.Equals(" ") || m_CursorName.Equals(name))
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_PARAMETER_VALUE);
			}
			SetCommandTime();
			m_Csi.SetCursorName(SendMsg, RecvMsg, this, name);
			PrimaryCursorName = name;
		}

		internal string GetCursorName()
		{
			return CursorName;
		}

		internal long GetCursorUpdateRow()
		{
			if (RsCache == null)
			{
				m_CursorUpdateRow = -1L;
			}
			else
			{
				m_CursorUpdateRow = RsCache.CursorUpdateRow();
			}
			return m_CursorUpdateRow;
		}
	}
}
