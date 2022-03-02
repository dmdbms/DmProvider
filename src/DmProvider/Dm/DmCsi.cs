using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Dm.filter.log;
using Dm.parser;
using Dm.util;

namespace Dm
{
	internal class DmCsi
	{
		private ILogger LOG = (Logger)LogFactory.getLog(typeof(DmCsi));

		private DmConnInstance m_ConnInstance;

		private volatile bool m_CsiSerial;

		private int m_socketTimeout;

		private volatile bool m_Closed;

		private object m_lock = new object();

		private bool local;

		private DmConnProperty m_connProperty;

		private DmCommTcpip m_CommTcpip;

		public DmCommTcpip CommTcpip
		{
			get
			{
				return m_CommTcpip;
			}
			set
			{
				m_CommTcpip = value;
			}
		}

		public DmConnProperty ConnProperty
		{
			get
			{
				return m_connProperty;
			}
			set
			{
				m_connProperty = value;
			}
		}

		public bool CsiSerial
		{
			get
			{
				return m_CsiSerial;
			}
			set
			{
				m_CsiSerial = value;
			}
		}

		internal bool is_CommdTcpipClosed()
		{
			if (CommTcpip == null)
			{
				return true;
			}
			return CommTcpip.is_CLosed();
		}

		internal void CheckConnected()
		{
			if (is_CommdTcpipClosed())
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_COMMUNITION_ERROR);
			}
		}

		internal DmCsi(DmMsg SendMsg, DmMsg RecvMsg, DmConnInstance conn)
		{
			m_Closed = true;
			m_ConnInstance = conn;
			ConnProperty = conn.ConnProperty;
			GetServerAndPort(ConnProperty.Server);
			CommTcpip = new DmCommTcpip(ConnProperty.Server, ConnProperty.Port, ConnProperty.ConnectionTimeout);
			StartupServer(SendMsg, RecvMsg);
			if (ConnProperty.Compress > 0)
			{
				local = DriverUtil.isLocalHost(ConnProperty.Server);
			}
			if (ConnProperty.encryptMsg || ConnProperty.encryptPwd)
			{
				byte[] tmpSessionKey = MsgSecurity.ComputeSessionKey(CommTcpip.GetClientPrivKey(), ConnProperty.serverPubKey);
				if (ConnProperty.encryptType == -1)
				{
					ConnProperty.encryptType = 132;
				}
				if (ConnProperty.hashType == -1)
				{
					ConnProperty.hashType = 4352;
				}
				CommTcpip.GenMsgCiphers(ConnProperty.encryptType, tmpSessionKey, ConnProperty.CipherPath, ConnProperty.hashType);
			}
			Login(SendMsg, RecvMsg);
			m_Closed = false;
			m_socketTimeout = conn.ConnProperty.SocketTimeout;
		}

		private DmMsg Communicate(DmMsg SendMsg, DmMsg RecvMsg, int socketTimeout)
		{
			lock (m_lock)
			{
				try
				{
					CheckConnected();
					LOG.Info(ConnProperty.RWStandby ? ("accessStandby Cmd:" + SendMsg.ReqGetCmd()) : ("access Cmd:" + SendMsg.ReqGetCmd()));
					CommTcpip.Send(SendMsg, socketTimeout, ConnProperty.CrcBody, ConnProperty.encryptMsg);
					CommTcpip.Recv(RecvMsg, socketTimeout, ConnProperty.CrcBody, ConnProperty.encryptMsg);
					return RecvMsg;
				}
				catch (SocketException ex)
				{
					Close();
					if (((ExternalException)(object)ex).ErrorCode == 10060)
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_COMMAND_TIME_OUT);
						return RecvMsg;
					}
					DmError.ThrowDmException(DmErrorDefinition.ECNET_COMMUNITION_ERROR);
					return RecvMsg;
				}
			}
		}

		internal void StartupServer(DmMsg SendMsg, DmMsg RecvMsg)
		{
			DmReq.StartupServer(SendMsg, ConnProperty, CommTcpip);
			Communicate(SendMsg, RecvMsg, ConnProperty.ConnectionTimeout);
			DmResp.StartupServer(RecvMsg, ConnProperty);
		}

		public DmMsg Login(DmMsg SendMsg, DmMsg RecvMsg)
		{
			DmReq.Login(SendMsg, ConnProperty, CommTcpip);
			Communicate(SendMsg, RecvMsg, ConnProperty.ConnectionTimeout);
			DmResp.Login(RecvMsg, ConnProperty);
			return RecvMsg;
		}

		internal void GetServerAndPort(string svr)
		{
			if (svr.Trim().StartsWith("["))
			{
				int num = svr.IndexOf("[");
				int num2 = svr.IndexOf("]");
				ConnProperty.Server = svr.Substring(num + 1, num2 - 1).Trim();
				int num3 = svr.Substring(num2).IndexOf(":");
				if (num3 != -1)
				{
					ConnProperty.Port = Convert.ToInt32(svr.Substring(num2).Substring(num3 + 1).Trim());
				}
			}
			else
			{
				int num4 = svr.IndexOf(":");
				if (num4 != -1)
				{
					ConnProperty.Server = svr.Substring(0, num4).Trim();
					ConnProperty.Port = Convert.ToInt32(svr.Substring(num4 + 1, svr.Length - num4 - 1).Trim());
				}
			}
		}

		public void AllocStmtHandle(DmStatement stmt, DmMsg SendMsg, DmMsg RecvMsg, ref bool m_new_col_desc)
		{
			DmReq.AllocStmt(SendMsg, 3, 0);
			Communicate(SendMsg, RecvMsg, m_socketTimeout);
			stmt.Stmtid = DmResp.AllocStmt(RecvMsg, ConnProperty, ref m_new_col_desc);
		}

		public void FreeHandle(DmMsg SendMsg, DmMsg RecvMsg, DmStatement stmt)
		{
			if (!m_Closed)
			{
				DmReq.FreeStmt(SendMsg, stmt.Stmtid);
				Communicate(SendMsg, RecvMsg, stmt.ConnInst.ConnProperty.SocketTimeout);
				DmResp.Simple(RecvMsg, ConnProperty);
			}
		}

		public void PrepareSql(DmMsg SendMsg, DmMsg RecvMsg, DmStatement stmt, string sql, bool direct, int checkFlag)
		{
			DmReq.PrepareSql(SendMsg, stmt.Handle, ConnProperty, sql, direct, checkFlag, stmt.CursorType, stmt.CommandTimeOut);
			Communicate(SendMsg, RecvMsg, stmt.ConnInst.ConnProperty.SocketTimeout);
			if (direct)
			{
				DmResp.Execute(RecvMsg, stmt, ConnProperty);
				stmt.Prepared = false;
			}
			else
			{
				DmResp.Prepare(RecvMsg, stmt);
				stmt.Prepared = true;
				stmt.HasPreparedInfo = true;
			}
		}

		public void PrepareSql(DmMsg SendMsg, DmMsg RecvMsg, DmStatement stmt, string sql, bool direct, int checkFlag, bool fromStandby)
		{
			DmReq.PrepareSql(SendMsg, stmt.Handle, ConnProperty, sql, direct, checkFlag, stmt.CursorType, stmt.CommandTimeOut);
			Communicate(SendMsg, RecvMsg, stmt.ConnInst.ConnProperty.SocketTimeout);
			if (direct)
			{
				DmResp.Execute(RecvMsg, stmt, ConnProperty);
				stmt.Prepared = false;
			}
			else
			{
				DmResp.Prepare(RecvMsg, stmt);
				stmt.Prepared = true;
				stmt.HasPreparedInfo = true;
			}
		}

		public void ExecutePrepared(DmStatement stmt, DmInfo des)
		{
			DmMsg sendMsg = stmt.SendMsg;
			DmMsg recvMsg = stmt.RecvMsg;
			DmReq.Execute(sendMsg, stmt, des, ConnProperty);
			Communicate(sendMsg, recvMsg, stmt.ConnInst.ConnProperty.SocketTimeout);
			DmResp.Execute(recvMsg, stmt, ConnProperty);
			stmt.Prepared = false;
		}

		public void PreExec(int paramTotal, DmStatement stmt, DmParameterInternal[] inParas)
		{
			DmMsg dmMsg = new DmMsg();
			DmMsg dmMsg2 = new DmMsg();
			DmReq.PreExec(paramTotal, dmMsg, stmt.Handle, inParas);
			Communicate(dmMsg, dmMsg2, stmt.ConnInst.ConnProperty.SocketTimeout);
			DmResp.Simple(dmMsg2, ConnProperty);
		}

		public bool Fetch(DmStatement stmt, DmResultSetCache rsCache, short resId, long curPos, long fetchNum)
		{
			DmMsg sendMsg = stmt.SendMsg;
			DmMsg recvMsg = stmt.RecvMsg;
			DmReq.Fetch(sendMsg, stmt.Handle, curPos, resId, fetchNum, stmt.ConnInst.ConnProperty.BufPrefetch);
			Communicate(sendMsg, recvMsg, stmt.ConnInst.ConnProperty.SocketTimeout);
			return DmResp.Fetch(recvMsg, curPos, rsCache);
		}

		public void Commit(DmMsg SendMsg, DmMsg RecvMsg)
		{
			DmReq.Simple(SendMsg, 8, 0);
			Communicate(SendMsg, RecvMsg, m_socketTimeout);
			DmResp.Simple(RecvMsg, ConnProperty);
		}

		public void Rollback(DmMsg SendMsg, DmMsg RecvMsg)
		{
			DmReq.Simple(SendMsg, 9, 0);
			Communicate(SendMsg, RecvMsg, m_socketTimeout);
			DmResp.Simple(RecvMsg, ConnProperty);
		}

		public void CloseHandle(DmStatement stmt)
		{
			DmReq.CloseCursor(stmt.SendMsg, stmt.Stmtid);
			Communicate(stmt.SendMsg, stmt.RecvMsg, stmt.ConnInst.ConnProperty.SocketTimeout);
			DmResp.Simple(stmt.RecvMsg, ConnProperty);
		}

		public void SetCursorName(DmMsg SendMsg, DmMsg RecvMsg, DmStatement stmt, string cursorName)
		{
			DmReq.SetCursorName(SendMsg, stmt, cursorName, ConnProperty);
			Communicate(stmt.SendMsg, stmt.RecvMsg, stmt.ConnInst.ConnProperty.SocketTimeout);
			DmResp.Simple(stmt.RecvMsg, ConnProperty);
		}

		public void PutData(DmMsg SendMsg, DmMsg RecvMsg, DmStatement stmt, short paramNum, byte[] data, int data_len, int clob_len)
		{
			DmReq.PutData(SendMsg, stmt.Handle, paramNum, data, data_len, ConnProperty, clob_len);
			Communicate(SendMsg, RecvMsg, stmt.ConnInst.ConnProperty.SocketTimeout);
			DmResp.Simple(RecvMsg, ConnProperty);
		}

		public int GetLobLen(DmLob lob)
		{
			DmMsg recvMsg = lob.Stmt.RecvMsg;
			DmMsg sendMsg = lob.Stmt.SendMsg;
			DmReq.GetLobLen(sendMsg, lob);
			Communicate(sendMsg, recvMsg, lob.Stmt.ConnInst.ConnProperty.SocketTimeout);
			return DmResp.GetLobLen(recvMsg, ConnProperty);
		}

		public int SetLobData(DmLob lob, byte flag, int start_pos, byte[] buf, int off, int len, byte firstOrLast)
		{
			DmMsg sendMsg = lob.Stmt.SendMsg;
			DmMsg recvMsg = lob.Stmt.RecvMsg;
			DmReq.SetLobData(sendMsg, lob, flag, start_pos, buf, off, len, firstOrLast);
			return DmResp.SetLobData(Communicate(sendMsg, recvMsg, lob.Stmt.ConnInst.ConnProperty.SocketTimeout), lob, ConnProperty);
		}

		public void LobTrunc(DmLob lob, byte flag, int len)
		{
			DmMsg sendMsg = lob.Stmt.SendMsg;
			DmMsg recvMsg = lob.Stmt.RecvMsg;
			DmReq.LobTrunc(sendMsg, lob, flag, len);
			Communicate(sendMsg, recvMsg, lob.Stmt.ConnInst.ConnProperty.SocketTimeout);
			DmResp.LobTrunc(recvMsg, lob, ConnProperty);
		}

		public byte[] GetLobData(DmLob lob, int start_pos, int length)
		{
			DmMsg sendMsg = lob.Stmt.SendMsg;
			DmMsg recvMsg = lob.Stmt.RecvMsg;
			DmReq.GetLobData(sendMsg, lob, start_pos, length);
			Communicate(sendMsg, recvMsg, lob.Stmt.ConnInst.ConnProperty.SocketTimeout);
			return DmResp.GetLobData(recvMsg, lob, ConnProperty);
		}

		public void MoreResult(DmStatement stmt, short resId)
		{
			DmMsg dmMsg = new DmMsg();
			DmMsg dmMsg2 = new DmMsg();
			DmReq.GetMoreResult(dmMsg, stmt.Handle, resId);
			Communicate(dmMsg, dmMsg2, stmt.ConnInst.ConnProperty.SocketTimeout);
			DmResp.GetMoreResult(dmMsg2, stmt, ConnProperty);
		}

		public long[] TableTs(DmResultSetCache rsCache)
		{
			DmMsg dmMsg = new DmMsg();
			DmMsg dmMsg2 = new DmMsg();
			DmReq.TableTs(dmMsg, rsCache.ids);
			Communicate(dmMsg, dmMsg2, m_socketTimeout);
			return DmResp.GetTabelTs(dmMsg2, ConnProperty);
		}

		private void GetResultSetData(DmMsg m_Msg, DmStatement stmt, DmMsg msg, int colNum, long rowNum, int offset)
		{
			int num = msg.ResExecuteGetFetchedRows();
			int num2 = msg.ResExecuteGetRsCacheOffset();
			if (num > 0)
			{
				int num3 = msg.ResGetLen();
				short num4 = msg.ResExecuteGetRetType();
				bool isRsBdta = false;
				short rsBdtaRowidCol = -1;
				if (num4 == 160 || num4 == 162)
				{
					isRsBdta = msg.ResExecuteGetRsBdtaFlag() == 2;
					rsBdtaRowidCol = msg.ResExecuteGetRsRowIdColIndex();
				}
				else
				{
					msg.ResExecuteGetRowid();
				}
				int len = num3 + 64 - offset;
				if (stmt.RsCache == null)
				{
					stmt.RsCache = new DmResultSetCache(stmt, colNum, num);
				}
				else
				{
					stmt.RsCache.SetCols(colNum);
					stmt.RsCache.TotalRows = rowNum;
				}
				byte[] bytes = msg.GetBytes(offset, len);
				stmt.RsCache.FillRows(0L, num, bytes, isRsBdta, rsBdtaRowidCol);
				if (stmt.ConnInst.ConnProperty.EnRsCache && num2 > 0 && rowNum == num)
				{
					RsKey key = new RsKey(stmt.ConnInst.ConnProperty.Guid, stmt.ConnInst.ConnProperty.CurrentSchema, stmt.Org_sql, stmt.Command.do_DbParameterCollection.Count, stmt.Command.do_DbParameterCollection);
					DmConnection.rsLRUCache.Add(key, stmt.RsCache);
				}
			}
			if (num2 > 0)
			{
				msg.SkipRead(num2 - msg.Read);
				short num5 = msg.ReadShort();
				int[] array = new int[num5];
				long[] array2 = new long[num5];
				for (int i = 0; i < num5; i++)
				{
					array[i] = msg.ReadInt();
					array2[i] = msg.ReadLong();
				}
				stmt.RsCache.ids = array;
				stmt.RsCache.tss = array2;
				stmt.RsCache.lastCheckDt = DateTime.Now;
			}
		}

		public DmInfo GetMoreResult(DmStatement stmt, DmInfo des, short resId)
		{
			DmMsg dmMsg = new DmMsg();
			DmMsg dmMsg2 = new DmMsg();
			DmReq.GetMoreResult(dmMsg, stmt.Handle, resId);
			Communicate(dmMsg, dmMsg2, stmt.ConnInst.ConnProperty.SocketTimeout);
			return GetDescAndResultSet(stmt.SendMsg, stmt, dmMsg2, des, flag: true);
		}

		private DmInfo GetDescAndResultSet(DmMsg m_Msg, DmStatement stmt, DmMsg recv, DmInfo des, bool flag)
		{
			if (!recv.CheckCRC())
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_MSG_CHECK_ERROR);
			}
			int num = recv.ResGetSqlcode();
			if (num < 0)
			{
				ThrowServerException(recv);
			}
			int num2 = recv.ResExecuteGetColNum();
			long num3 = recv.ResExecuteGetRowNum();
			int num4 = recv.ResExecuteGetRetType();
			des.SetRetStmtType(num4);
			if (ProcessRetCmd(num4, recv, stmt))
			{
				return des;
			}
			if (num4 == 159 || num4 == 158 || num4 == 157 || num4 == 160 || num4 == 162)
			{
				des.SetRowCount(num3);
			}
			else
			{
				des.SetRowCount(-1L);
			}
			switch (num4)
			{
			case 157:
			case 158:
			case 159:
				des.SetRecordsAffected(num3);
				break;
			case 160:
				des.SetRecordsAffected(-1L);
				break;
			}
			des.SetUpdatable((recv.ResExecuteGetRsUpdatable() != 0) ? true : false);
			long rowId = recv.ResExecuteGetRowid();
			des.SetRowId(rowId);
			if (!flag)
			{
				int @int = recv.GetInt(39);
				if (@int > 0)
				{
					string printMsg = recv.ReadString(@int, m_ConnInstance.ConnProperty.ServerEncoding);
					des.SetPrintMsg(printMsg);
				}
			}
			if (!flag && des.GetOutParamCount() > 0)
			{
				int num5 = recv.ResExecuteGetParamNum();
				if (num5 != des.GetOutParamCount())
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_COMMUNITION_ERROR);
				}
				int[] array = new int[num5];
				int parameterCount = des.GetParameterCount();
				int num6 = 0;
				for (int i = 0; i < parameterCount; i++)
				{
					if (des.GetParamsInfo()[i].GetInOutType() != 0)
					{
						array[num6] = i;
						num6++;
					}
				}
				DmParameterInternal[] paramsInfo = des.GetParamsInfo();
				byte[] array2 = null;
				for (int j = 0; j < num5; j++)
				{
					bool flag2 = false;
					int num7 = array[j];
					int num8 = recv.ReadShort();
					if (num8 == -2)
					{
						num8 = 0;
						flag2 = true;
					}
					array2 = recv.ReadBytes(num8);
					paramsInfo[num7].ClearOutParam();
					paramsInfo[num7].SetOutValue(array2);
					paramsInfo[num7].SetOutDataBound(outDataBound: true);
					if (flag2)
					{
						paramsInfo[num7].SetOutNull();
					}
					DmParameter obj = (DmParameter)stmt.Command.Parameters[num7];
					DmGetValue dmGetValue = new DmGetValue(m_ConnInstance.ConnProperty.ServerEncoding, stmt, m_ConnInstance.ConnProperty.NewLobFlag);
					obj.do_Value = dmGetValue.GetObject(0, array2, paramsInfo[num7].GetCType(), paramsInfo[num7].GetPrecision(), paramsInfo[num7].GetScale());
					obj.do_Value = DmSysTypeConvertion.TypeConvertion(obj);
				}
			}
			if ((num4 == 160 || (num4 == 162 && num2 > 0)) && num != 111)
			{
				DmResp.GetColsInfo(recv, num2, des, stmt.ConnInst, stmt.New_col_desc);
				GetResultSetData(stmt.SendMsg, stmt, recv, num2, num3, recv.Read);
			}
			else
			{
				des.SetHasResultSet(hasResultSet: false);
				if (num == 111)
				{
					des.SetRowCount(-1L);
				}
			}
			return des;
		}

		public bool IsClosed()
		{
			return m_Closed;
		}

		internal void CloseForDbAliveCheck()
		{
			if (!m_Closed)
			{
				if (CommTcpip != null)
				{
					CommTcpip.Close();
				}
				m_ConnInstance = null;
				m_Closed = true;
			}
		}

		public void Close()
		{
			if (m_Closed)
			{
				return;
			}
			lock (m_lock)
			{
				if (CommTcpip != null)
				{
					CommTcpip.Close();
				}
			}
			m_ConnInstance = null;
			m_Closed = true;
		}

		public void PutClobData(DmStatement stmt, int paramIndex, DmParameterInternal inPara)
		{
			int num = 8000;
			int num2 = 0;
			DmMsg sendMsg = new DmMsg();
			DmMsg recvMsg = new DmMsg();
			byte[] array = new byte[32000];
			string @string = DmConvertion.GetString(inPara.GetParamValue()[0].m_InValue, 0, inPara.GetParamValue()[0].GetStreamLen(), ConnProperty.ServerEncoding);
			int num3 = @string.Length;
			while (num3 > 0)
			{
				if (num3 < num)
				{
					array = DmConvertion.GetBytes(@string.Substring(num2, num3), ConnProperty.ServerEncoding);
					PutData(sendMsg, recvMsg, stmt, (short)paramIndex, array, array.Length, num3);
					return;
				}
				array = DmConvertion.GetBytes(@string.Substring(num2, num), ConnProperty.ServerEncoding);
				PutData(sendMsg, recvMsg, stmt, (short)paramIndex, array, array.Length, num);
				num3 -= num;
				num2 += num;
			}
			PutData(sendMsg, recvMsg, stmt, (short)paramIndex, array, array.Length, 0);
		}

		public void PutBlobData(DmStatement stmt, int paramIndex, DmParameterInternal inPara)
		{
			int num = 32000;
			int num2 = 0;
			DmMsg sendMsg = new DmMsg();
			DmMsg recvMsg = new DmMsg();
			byte[] val = new byte[num];
			int bytes;
			for (bytes = inPara.GetBytes(ref val, 0, num2, num, 0); bytes > 0; bytes = inPara.GetBytes(ref val, 0, num2, num, 0))
			{
				if (bytes < num)
				{
					PutData(sendMsg, recvMsg, stmt, (short)paramIndex, val, bytes, int.MinValue);
					return;
				}
				PutData(sendMsg, recvMsg, stmt, (short)paramIndex, val, num, int.MinValue);
				num2 += num;
			}
			PutData(sendMsg, recvMsg, stmt, (short)paramIndex, val, bytes, int.MinValue);
		}

		public string ClobGetSubString(DmLob lob, byte flag, int pos, int len)
		{
			string text = null;
			byte[] lobData = GetLobData(lob, pos, len);
			try
			{
				if (lobData == null)
				{
					return null;
				}
				return DmConvertion.GetString(lobData, 0, lobData.Length, m_ConnInstance.ConnProperty.ServerEncoding);
			}
			catch (Exception)
			{
				return null;
			}
		}

		private bool ProcessRetCmd(int retCmd, DmMsg recvMsg, DmStatement stmt)
		{
			int num = 64;
			switch (retCmd)
			{
			case 166:
			{
				short short2 = recvMsg.GetShort(num);
				num += 2;
				num++;
				IsolationLevel isolationLevel = IsolationLevel.ReadCommitted;
				stmt.ConnInst.SetTrxISO(short2 switch
				{
					1 => IsolationLevel.ReadCommitted, 
					3 => IsolationLevel.Serializable, 
					0 => IsolationLevel.ReadUncommitted, 
					_ => IsolationLevel.Unspecified, 
				});
				stmt.Csi.CsiSerial = true;
				return true;
			}
			case 150:
			{
				short @short = recvMsg.GetShort(num);
				num += 2;
				num++;
				if (stmt.Command != null)
				{
					stmt.Command.SetStmtSerial(@short);
				}
				if (stmt.ConnInst.Transaction != null)
				{
					stmt.ConnInst.Transaction.SetStmtSerial(@short);
				}
				return true;
			}
			case 152:
			{
				int @int = recvMsg.GetInt(num);
				num += 4;
				stmt.ConnInst.ConnProperty.Database = recvMsg.GetString(num, @int, m_ConnInstance.ConnProperty.ServerEncoding);
				num += @int;
				num += 4;
				return true;
			}
			case 153:
				num += 4;
				return true;
			case 165:
				num += 2;
				return true;
			default:
				return false;
			}
		}

		private void ThrowServerException(DmMsg msg)
		{
			DmError dmError = new DmError();
			dmError.State = msg.ResGetSqlcode();
			msg.GetErrorInfo(dmError, m_ConnInstance.ConnProperty.ServerEncoding);
			DmError.ThrowDmException(dmError);
		}

		public int BlobTextTruncate(DmLob lob, byte flag, int length)
		{
			DmMsg dmMsg = new DmMsg();
			DmMsg dmMsg2 = new DmMsg();
			dmMsg.ReqSetStmtid(0);
			dmMsg.ReqSetCmd(31);
			dmMsg.WriteByte(flag);
			dmMsg.WriteLong(lob.m_lobid);
			dmMsg.WriteShort(lob.m_data_grpid);
			dmMsg.WriteShort(lob.m_data_fileid);
			dmMsg.WriteInt(lob.m_data_pageno);
			dmMsg.WriteInt(lob.m_tabid);
			dmMsg.WriteShort(lob.m_colid);
			dmMsg.WriteLong(lob.m_Rowid);
			dmMsg.WriteInt(length);
			if (ConnProperty.NewLobFlag == 1)
			{
				dmMsg.WriteShort(lob.m_rec_grpid);
				dmMsg.WriteShort(lob.m_rec_fileid);
				dmMsg.WriteInt(lob.m_rec_pageno);
			}
			dmMsg.ReqSetLen();
			Communicate(dmMsg, dmMsg2, m_socketTimeout);
			if (!dmMsg2.CheckCRC())
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_MSG_CHECK_ERROR);
			}
			if (dmMsg2.ResGetSqlcode() < 0)
			{
				ThrowServerException(dmMsg2);
			}
			int result = dmMsg2.ReadInt();
			long blobid = dmMsg2.ReadLong();
			lob.setBlobid(blobid);
			DmConvertion.SetShort(lob.cur_fileid, 0, lob.m_data_fileid);
			DmConvertion.SetInt(lob.cur_pageno, 0, lob.m_data_pageno);
			DmConvertion.SetInt(lob.m_curOff, 0, 0);
			return result;
		}

		public byte[] LobGetBytes(DmLob lob, byte flag, int start_pos, int length)
		{
			int num = 0;
			DmMsg dmMsg = new DmMsg();
			DmMsg dmMsg2 = new DmMsg();
			dmMsg.ReqSetStmtid(0);
			dmMsg.ReqSetCmd(32);
			dmMsg.WriteByte(flag);
			dmMsg.WriteInt(lob.m_tabid);
			dmMsg.WriteShort(lob.m_colid);
			dmMsg.WriteLong(lob.m_lobid);
			dmMsg.WriteShort(lob.m_data_grpid);
			dmMsg.WriteShort(lob.m_data_fileid);
			dmMsg.WriteInt(lob.m_data_pageno);
			dmMsg.WriteBytes(lob.cur_fileid, 0, 2);
			dmMsg.WriteBytes(lob.cur_pageno, 0, 4);
			dmMsg.WriteBytes(lob.m_curOff, 0, 4);
			dmMsg.WriteInt(start_pos);
			dmMsg.WriteInt(length);
			if (ConnProperty.NewLobFlag == 1)
			{
				dmMsg.WriteLong(lob.m_Rowid);
				dmMsg.WriteShort(lob.m_rec_grpid);
				dmMsg.WriteShort(lob.m_rec_fileid);
				dmMsg.WriteInt(lob.m_rec_pageno);
			}
			dmMsg.ReqSetLen();
			Communicate(dmMsg, dmMsg2, m_socketTimeout);
			if (!dmMsg2.CheckCRC())
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_MSG_CHECK_ERROR);
			}
			if (dmMsg2.ResGetSqlcode() < 0)
			{
				ThrowServerException(dmMsg2);
			}
			dmMsg2.ReadByte();
			num = dmMsg2.ReadInt();
			if (num <= 0)
			{
				return null;
			}
			lob.setCurFileid(dmMsg2.ReadBytes(2));
			lob.setCurPageno(dmMsg2.ReadBytes(4));
			lob.setCurOff(dmMsg2.ReadBytes(4));
			byte[] result = dmMsg2.ReadBytes(num);
			dmMsg = null;
			return result;
		}

		public int LobSetBytes(DmLob lob, byte flag, int start_pos, byte[] buf, int off, int len, byte firstOrLast)
		{
			DmMsg dmMsg = new DmMsg();
			DmMsg dmMsg2 = new DmMsg();
			dmMsg.ReqSetStmtid(0);
			dmMsg.ReqSetCmd(30);
			dmMsg.WriteByte(flag);
			dmMsg.WriteByte(firstOrLast);
			dmMsg.WriteLong(lob.m_lobid);
			dmMsg.WriteShort(lob.m_data_grpid);
			dmMsg.WriteShort(lob.m_data_fileid);
			dmMsg.WriteInt(lob.m_data_pageno);
			dmMsg.WriteBytes(lob.cur_fileid, 0, 2);
			dmMsg.WriteBytes(lob.cur_pageno, 0, 4);
			dmMsg.WriteBytes(lob.m_curOff, 0, 4);
			dmMsg.WriteInt(lob.m_tabid);
			dmMsg.WriteShort(lob.m_colid);
			dmMsg.WriteLong(lob.m_Rowid);
			dmMsg.WriteInt(start_pos);
			dmMsg.WriteInt(len);
			dmMsg.WriteBytes(buf, off, len);
			if (ConnProperty.NewLobFlag == 1)
			{
				dmMsg.WriteShort(lob.m_rec_grpid);
				dmMsg.WriteShort(lob.m_rec_fileid);
				dmMsg.WriteInt(lob.m_rec_pageno);
			}
			dmMsg.ReqSetLen();
			Communicate(dmMsg, dmMsg2, m_socketTimeout);
			if (!dmMsg2.CheckCRC())
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_MSG_CHECK_ERROR);
			}
			if (dmMsg2.ResGetSqlcode() < 0)
			{
				ThrowServerException(dmMsg2);
			}
			dmMsg2.ReadInt();
			long blobid = dmMsg2.ReadLong();
			lob.setBlobid(blobid);
			lob.setGroupid(dmMsg2.ReadShort());
			lob.setFileid(dmMsg2.ReadShort());
			lob.setPageno(dmMsg2.ReadInt());
			lob.setCurFileid(dmMsg2.ReadBytes(2));
			lob.setCurPageno(dmMsg2.ReadBytes(4));
			lob.setCurOff(dmMsg2.ReadBytes(4));
			dmMsg = null;
			return len;
		}

		public int GetLobLength(DmLob lob, byte flag)
		{
			DmMsg dmMsg = new DmMsg();
			DmMsg dmMsg2 = new DmMsg();
			dmMsg.ReqSetStmtid(0);
			dmMsg.ReqSetCmd(29);
			dmMsg.WriteByte(flag);
			dmMsg.WriteLong(lob.m_lobid);
			dmMsg.WriteShort(lob.m_data_grpid);
			dmMsg.WriteShort(lob.m_data_fileid);
			dmMsg.WriteInt(lob.m_data_pageno);
			if (ConnProperty.NewLobFlag == 1)
			{
				dmMsg.WriteInt(lob.m_tabid);
				dmMsg.WriteShort(lob.m_colid);
				dmMsg.WriteLong(lob.m_Rowid);
				dmMsg.WriteShort(lob.m_rec_grpid);
				dmMsg.WriteShort(lob.m_rec_fileid);
				dmMsg.WriteInt(lob.m_rec_pageno);
			}
			dmMsg.ReqSetLen();
			Communicate(dmMsg, dmMsg2, m_socketTimeout);
			if (!dmMsg2.CheckCRC())
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_MSG_CHECK_ERROR);
			}
			if (dmMsg2.ResGetSqlcode() < 0)
			{
				ThrowServerException(dmMsg2);
			}
			return dmMsg2.ReadInt();
		}

		public void ExecDirectOpt(DmStatement stmt, List<SQLProcessor.Parameter> optParamList)
		{
			DmMsg sendMsg = stmt.SendMsg;
			DmMsg recvMsg = stmt.RecvMsg;
			DmReq.ExecDirectOpt(sendMsg, stmt, optParamList, ConnProperty);
			Communicate(sendMsg, recvMsg, stmt.ConnInst.ConnProperty.SocketTimeout);
			DmResp.Execute(recvMsg, stmt, ConnProperty);
			stmt.Prepared = false;
		}

		public void SetSessIso(DmMsg SendMsg, DmMsg RecvMsg, int iso)
		{
			if (iso == 3)
			{
				CsiSerial = true;
			}
			DmReq.SetSessIso(SendMsg, iso);
			Communicate(SendMsg, RecvMsg, m_socketTimeout);
			DmResp.Simple(RecvMsg, ConnProperty);
		}
	}
}
