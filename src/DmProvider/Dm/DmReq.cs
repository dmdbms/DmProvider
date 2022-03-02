using System;
using System.Collections.Generic;
using System.Data;
using Dm.Config;
using Dm.parser;

namespace Dm
{
	internal class DmReq
	{
		private DmReq()
		{
		}

		public static void AllocStmt(DmMsg msgIn, short reqCmd, int stmtId)
		{
			msgIn.Clear(64);
			msgIn.ReqSetCmd(reqCmd);
			msgIn.ReqSetStmtid(stmtId);
			msgIn.ReqAllocStmtSetNewColDescFlag(1);
			msgIn.ReqSetLen();
		}

		public static void Simple(DmMsg msgIn, short reqCmd, int stmtId)
		{
			msgIn.Clear(64);
			msgIn.ReqSetCmd(reqCmd);
			msgIn.ReqSetStmtid(stmtId);
			msgIn.ReqSetLen();
		}

		public static void StartupServer(DmMsg msgIn, DmConnProperty connProperty, DmCommTcpip commTcpip)
		{
			msgIn.Clear(64);
			msgIn.ReqSetCmd(200);
			msgIn.ReqSetStmtid(0);
			msgIn.ReqStartupSetEncryptType(0);
			msgIn.ReqStartupSetCmprsMsg(connProperty.Compress);
			msgIn.ReqStartupSetGenKeyPairFlag((byte)(connProperty.LoginEncrypt ? 1 : 0));
			msgIn.ReqStartupSetCommEncFlag((byte)(connProperty.LoginEncrypt ? 2 : 0));
			msgIn.ReqStartupSetRsBdtaFlag((byte)(connProperty.IsBdtaRs ? 2 : 0));
			msgIn.ReqStartupSetProtocolVersion(connProperty.msgVersion);
			msgIn.WriteBytesWith4Length(DmConvertion.GetBytes("7.0.0.0", connProperty.ServerEncoding));
			msgIn.WriteByte(0);
			if (connProperty.LoginEncrypt)
			{
				msgIn.WriteBytesWith4Length(commTcpip.GetClientPubKey());
			}
			msgIn.ReqSetLen();
		}

		public static void Login(DmMsg msgIn, DmConnProperty connProperty, DmCommTcpip commTcpip)
		{
			int num = -1;
			msgIn.Clear(64);
			msgIn.ReqSetCmd(1);
			msgIn.ReqSetStmtid(0);
			msgIn.ReqLoginSetEnv(connProperty.SessClt);
			num = ((connProperty.IsolationLevel == IsolationLevel.ReadCommitted) ? 1 : ((connProperty.IsolationLevel != IsolationLevel.Serializable) ? (-1) : 3));
			msgIn.ReqLoginSetIsoLevel(num);
			msgIn.ReqLoginSetLanguage(connProperty.Language);
			msgIn.ReqLoginSetReadOnly(connProperty.AccessMode);
			msgIn.ReqLoginSetTimeZone(connProperty.TimeZone);
			msgIn.ReqLoginSetSessTimeout(connProperty.CommandTimeout);
			msgIn.ReqLoginSetLanguage(connProperty.MppType);
			byte standbyflag = Convert.ToByte(connProperty.RwSeparate ? 1 : 0);
			msgIn.ReqLoginSetStandby(standbyflag);
			msgIn.ReqLoginSetNewLobFlag(connProperty.NewLobFlag);
			byte[] array = DmConvertion.GetBytes(connProperty.User, connProperty.ServerEncoding);
			byte[] array2 = DmConvertion.GetBytes(connProperty.Pwd, connProperty.ServerEncoding);
			if (connProperty.encryptPwd && commTcpip.cipher != null)
			{
				array = commTcpip.cipher.Encrypt(array, genDigest: false);
				array2 = commTcpip.cipher.Encrypt(array2, genDigest: false);
			}
			msgIn.WriteBytesWith4Length(array);
			msgIn.WriteBytesWith4Length(array2);
			msgIn.WriteBytesWith4Length(DmConvertion.GetBytes(connProperty.AppName, connProperty.ServerEncoding));
			msgIn.WriteBytesWith4Length(DmConvertion.GetBytes(connProperty.OS, connProperty.ServerEncoding));
			msgIn.WriteBytesWith4Length(DmConvertion.GetBytes(connProperty.Host, connProperty.ServerEncoding));
			if (connProperty.RWStandby)
			{
				msgIn.WriteByte(1);
			}
			else
			{
				msgIn.WriteByte(0);
			}
			msgIn.ReqSetLen();
		}

		public static void FreeStmt(DmMsg msgIn, int stmtId)
		{
			msgIn.Clear(64);
			msgIn.ReqSetCmd(4);
			msgIn.ReqSetStmtid(stmtId);
			msgIn.ReqSetLen();
		}

		public static void PrepareSql(DmMsg msgIn, int handle, DmConnProperty connProperty, string sql, bool direct, int checkFlag, byte cursorType, int execTimeout)
		{
			msgIn.Clear(64);
			msgIn.ReqSetStmtid(handle);
			msgIn.ReqSetCmd(5);
			if (connProperty.AutoCommit)
			{
				msgIn.ReqPrepareSetAutoCmt(1);
			}
			else
			{
				msgIn.ReqPrepareSetAutoCmt(0);
			}
			if (direct)
			{
				msgIn.ReqPrepareSetExecDirect(1);
			}
			else
			{
				msgIn.ReqPrepareSetExecDirect(0);
			}
			msgIn.ReqPrepareSetCheckType((byte)((connProperty.CompatibleMode != CompatibleMode.ORACLE) ? ((byte)checkFlag) : 0));
			msgIn.ReqPrepareSetCurForwardOnly(cursorType);
			msgIn.ReqPrepareSetSqlType((short)((connProperty.CompatibleMode != CompatibleMode.ORACLE) ? 25 : 0));
			msgIn.ReqPrepareSetMaxRows((connProperty.MaxRows <= 0 || connProperty.EnRsCache) ? long.MaxValue : connProperty.MaxRows);
			msgIn.ReqPrepareSetRsBdtaFlag((byte)(connProperty.IsBdtaRs ? 2 : 0));
			msgIn.ReqPrepareSetRsBdtaLen(0);
			msgIn.ReqPrepareSetExecTimeout(execTimeout);
			if (connProperty.ResveredList != null)
			{
				string str = DmStringUtil.ReplaceReservedWords(sql, connProperty.ResveredList);
				msgIn.WriteStringWithNTS(str, connProperty.ServerEncoding);
			}
			else
			{
				msgIn.WriteStringWithNTS(sql, connProperty.ServerEncoding);
			}
			msgIn.ReqSetLen();
		}

		private static void FillParamInfo(int paramTotal, DmMsg msgIn, DmParameterInternal[] inParas)
		{
			DmParameterInternal dmParameterInternal = null;
			for (int i = 0; i < paramTotal; i++)
			{
				dmParameterInternal = inParas[i];
				int num;
				int num2;
				int num3;
				if (dmParameterInternal.GetTypeFlag() == 1)
				{
					num = dmParameterInternal.GetCType();
					num2 = dmParameterInternal.GetPrecision();
					num3 = dmParameterInternal.GetScale();
				}
				else if (dmParameterInternal.GetInDataBound())
				{
					num = dmParameterInternal.GetSqlType();
					num2 = dmParameterInternal.GetPrec();
					num3 = dmParameterInternal.GetBindScale();
				}
				else if (dmParameterInternal.GetTypeFlag() == 2)
				{
					num = dmParameterInternal.GetCType();
					num2 = dmParameterInternal.GetPrecision();
					num3 = dmParameterInternal.GetScale();
				}
				else
				{
					num = 2;
					num2 = 8188;
					num3 = 0;
				}
				if (num == 120)
				{
					msgIn.WriteByte(2);
				}
				else
				{
					msgIn.WriteByte((byte)dmParameterInternal.GetInOutType());
				}
				msgIn.WriteInt(num);
				TypeDescriptor typeDesc = inParas[i].TypeDesc;
				switch (num)
				{
				case 117:
				case 122:
					num2 = TypeDescriptor.GetPackArraySize(typeDesc);
					break;
				case 121:
					num2 = TypeDescriptor.GetPackRecordSize(typeDesc);
					break;
				case 119:
					num2 = TypeDescriptor.GetPackClassSize(typeDesc);
					break;
				case 12:
					if (DmSqlType.IsComplexType(num, num3))
					{
						num2 = typeDesc.GetObjId();
						if (num2 == 4)
						{
							num2 = typeDesc.GetOuterId();
						}
					}
					break;
				}
				msgIn.WriteInt(num2);
				msgIn.WriteInt(num3);
				switch (num)
				{
				case 117:
				case 122:
					TypeDescriptor.PackArray(typeDesc, msgIn);
					break;
				case 121:
					TypeDescriptor.PackRecord(typeDesc, msgIn);
					break;
				case 119:
					TypeDescriptor.PackClass(typeDesc, msgIn);
					break;
				}
			}
		}

		private static void FillParamData(int paramTotal, DmMsg msgIn, DmParameterInternal[] inParas, DmStatement stmt, int index)
		{
			DmParameterInternal dmParameterInternal = null;
			byte[] InValue = null;
			for (int i = 0; i < paramTotal; i++)
			{
				dmParameterInternal = inParas[i];
				int num = ((dmParameterInternal.GetTypeFlag() == 1) ? dmParameterInternal.GetCType() : (dmParameterInternal.GetInDataBound() ? dmParameterInternal.GetCType() : ((dmParameterInternal.GetTypeFlag() != 2) ? dmParameterInternal.GetSqlType() : dmParameterInternal.GetCType())));
				if (dmParameterInternal.GetInOutType() == 1 && num != 120)
				{
					continue;
				}
				dmParameterInternal.GetInValue(ref InValue, index);
				int num2;
				if (dmParameterInternal.GetIsInDataNull(index))
				{
					num2 = -2;
					msgIn.WriteShort((short)num2);
					continue;
				}
				num2 = InValue.Length;
				if (num2 > 2048 && (num == 12 || num == 19))
				{
					num2 = 0;
					msgIn.WriteShort((short)num2);
					if (!stmt.PreExecuted)
					{
						stmt.Csi.PreExec(paramTotal, stmt, inParas);
						stmt.PreExecuted = true;
					}
					if (num == 12)
					{
						stmt.Csi.PutBlobData(stmt, i, dmParameterInternal);
					}
					else
					{
						stmt.Csi.PutClobData(stmt, i, dmParameterInternal);
					}
				}
				else if (num2 > 65535)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_DATA_TOO_LONG);
				}
				else
				{
					msgIn.WriteShort((short)num2);
					msgIn.WriteBytes(InValue);
				}
			}
		}

		public static void Execute(DmMsg msgIn, DmStatement stmt, DmInfo des, DmConnProperty connProperty)
		{
			DmParameterInternal[] ParamsInfo = null;
			des.GetParamsInfo(out ParamsInfo);
			int num = ((ParamsInfo != null) ? des.GetParameterCount() : 0);
			msgIn.Clear(64);
			msgIn.ReqSetStmtid(stmt.Handle);
			if (connProperty.VerNum > 117506688)
			{
				msgIn.ReqSetCmd(13);
			}
			else
			{
				msgIn.ReqSetCmd(6);
			}
			if (connProperty.AutoCommit)
			{
				msgIn.ReqExecuteSetAutoCmt(1);
			}
			else
			{
				msgIn.ReqExecuteSetAutoCmt(0);
			}
			msgIn.ReqExecuteSetParamNum(num);
			msgIn.ReqExecuteSetCurForwardOnly(stmt.CursorType);
			msgIn.ReqExecuteSetRowNum(ParamsInfo[0].GetParamValue().Count);
			msgIn.ReqExecuteSetCurPos(stmt.GetCursorUpdateRow());
			msgIn.ReqExecuteSetMaxRows((connProperty.MaxRows <= 0 || connProperty.EnRsCache) ? long.MaxValue : connProperty.MaxRows);
			msgIn.ReqExecuteIgnoreBatchError(connProperty.BatchContinueOnError);
			msgIn.ReqExecuteExecTimeout((stmt.CommandTimeOut == 0) ? (-1) : stmt.CommandTimeOut);
			msgIn.ReqExecuteBatchAllowMaxErrors(connProperty.BatchAllowMaxErrors);
			FillParamInfo(num, msgIn, ParamsInfo);
			for (int i = 0; i < ParamsInfo[0].GetParamValue().Count; i++)
			{
				FillParamData(num, msgIn, ParamsInfo, stmt, i);
			}
			msgIn.ReqSetLen();
		}

		public static void PreExec(int paramTotal, DmMsg msgIn, int stmtId, DmParameterInternal[] inParas)
		{
			msgIn.Clear(64);
			msgIn.ReqSetStmtid(stmtId);
			msgIn.ReqSetCmd(90);
			msgIn.ReqPreexecSetParamNum((short)paramTotal);
			FillParamInfo(paramTotal, msgIn, inParas);
			msgIn.ReqSetLen();
		}

		public static void Fetch(DmMsg msgIn, int stmtId, long curPos, short resId, long fetchNum, int fetchPackSize)
		{
			msgIn.Clear(64);
			msgIn.ReqSetCmd(7);
			msgIn.ReqSetStmtid(stmtId);
			msgIn.ReqFetchSetRowCount(fetchNum);
			msgIn.ReqFetchSetCurPos(curPos);
			msgIn.ReqFetchSetResId(resId);
			msgIn.ReqFetchMaxMsgSize(fetchPackSize);
			msgIn.ReqSetLen();
		}

		public static void CloseCursor(DmMsg msgIn, int stmtId)
		{
			msgIn.Clear(64);
			msgIn.ReqSetCmd(17);
			msgIn.ReqSetStmtid(stmtId);
			msgIn.ReqSetLen();
		}

		public static void SetCursorName(DmMsg msgIn, DmStatement stmt, string cursorName, DmConnProperty connProperty)
		{
			msgIn.Clear(64);
			msgIn.ReqSetCmd(27);
			msgIn.ReqSetStmtid(stmt.Handle);
			msgIn.WriteStringWithNTS(cursorName, connProperty.ServerEncoding);
			msgIn.WriteInt(1);
			msgIn.ReqSetLen();
		}

		public static void PutData(DmMsg msgIn, int stmtId, short paramIndex, byte[] data, int datalen, DmConnProperty connProperty, int clob_len)
		{
			msgIn.Clear(64);
			msgIn.ReqSetStmtid(stmtId);
			msgIn.ReqSetCmd(14);
			msgIn.ReqPutdataSetParaIndex(paramIndex);
			msgIn.WriteInt(datalen);
			if (connProperty.NewLobFlag == 1)
			{
				msgIn.WriteInt(-1);
			}
			msgIn.WriteBytes(data, datalen);
			msgIn.ReqSetLen();
		}

		public static void GetLobLen(DmMsg lobMsg, DmLob lob)
		{
			lobMsg.Clear(64);
			lobMsg.ReqSetStmtid(lob.Stmt.Handle);
			lobMsg.ReqSetCmd(29);
			lobMsg.WriteByte(lob.flag);
			lobMsg.WriteLong(lob.m_lobid);
			lobMsg.WriteShort(lob.m_data_grpid);
			lobMsg.WriteShort(lob.m_data_fileid);
			lobMsg.WriteInt(lob.m_data_pageno);
			if (lob.new_lob_flag)
			{
				lobMsg.WriteInt(lob.m_tabid);
				lobMsg.WriteShort(lob.m_colid);
				lobMsg.WriteLong(lob.m_Rowid);
				lobMsg.WriteShort(lob.m_rec_grpid);
				lobMsg.WriteShort(lob.m_rec_fileid);
				lobMsg.WriteInt(lob.m_rec_pageno);
			}
			lobMsg.ReqSetLen();
		}

		public static void SetLobData(DmMsg msg, DmLob lob, byte flag, int start_pos, byte[] buf, int off, int len, byte firstOrLast)
		{
			msg.Clear(64);
			msg.ReqSetStmtid(lob.Stmt.Handle);
			msg.ReqSetCmd(30);
			msg.WriteByte(flag);
			msg.WriteByte(firstOrLast);
			msg.WriteLong(lob.m_lobid);
			msg.WriteShort(lob.m_data_grpid);
			msg.WriteShort(lob.m_data_fileid);
			msg.WriteInt(lob.m_data_pageno);
			msg.WriteBytes(lob.cur_fileid, 0, 2);
			msg.WriteBytes(lob.cur_pageno, 0, 4);
			msg.WriteBytes(lob.m_curOff, 0, 4);
			msg.WriteInt(lob.m_tabid);
			msg.WriteShort(lob.m_colid);
			msg.WriteLong(lob.m_Rowid);
			msg.WriteInt(start_pos);
			msg.WriteInt(len);
			msg.WriteBytes(buf, off, len);
			if (lob.new_lob_flag)
			{
				msg.WriteShort(lob.m_rec_grpid);
				msg.WriteShort(lob.m_rec_fileid);
				msg.WriteInt(lob.m_rec_pageno);
			}
			msg.ReqSetLen();
		}

		public static void LobTrunc(DmMsg msg, DmLob lob, byte flag, int len)
		{
			msg.Clear(64);
			msg.ReqSetStmtid(lob.Stmt.Handle);
			msg.ReqSetCmd(31);
			msg.WriteByte(flag);
			msg.WriteLong(lob.m_lobid);
			msg.WriteShort(lob.m_data_grpid);
			msg.WriteShort(lob.m_data_fileid);
			msg.WriteInt(lob.m_data_pageno);
			msg.WriteInt(lob.m_tabid);
			msg.WriteShort(lob.m_colid);
			msg.WriteLong(lob.m_Rowid);
			msg.WriteInt(len);
			if (lob.new_lob_flag)
			{
				msg.WriteShort(lob.m_rec_grpid);
				msg.WriteShort(lob.m_rec_fileid);
				msg.WriteInt(lob.m_rec_pageno);
			}
			msg.ReqSetLen();
		}

		public static void GetLobData(DmMsg blobMsg, DmLob lob, int start_pos, int length)
		{
			blobMsg.Clear(64);
			blobMsg.ReqSetStmtid(lob.Stmt.Handle);
			blobMsg.ReqSetCmd(32);
			blobMsg.WriteByte(lob.flag);
			blobMsg.WriteInt(lob.m_tabid);
			blobMsg.WriteShort(lob.m_colid);
			blobMsg.WriteLong(lob.m_lobid);
			blobMsg.WriteShort(lob.m_data_grpid);
			blobMsg.WriteShort(lob.m_data_fileid);
			blobMsg.WriteInt(lob.m_data_pageno);
			blobMsg.WriteBytes(lob.cur_fileid, 0, 2);
			blobMsg.WriteBytes(lob.cur_pageno, 0, 4);
			blobMsg.WriteBytes(lob.m_curOff, 0, 4);
			blobMsg.WriteInt(start_pos);
			blobMsg.WriteInt(length);
			if (lob.new_lob_flag)
			{
				blobMsg.WriteLong(lob.m_Rowid);
				blobMsg.WriteShort(lob.m_rec_grpid);
				blobMsg.WriteShort(lob.m_rec_fileid);
				blobMsg.WriteInt(lob.m_rec_pageno);
			}
			blobMsg.ReqSetLen();
		}

		public static void GetMoreResult(DmMsg msgIn, int stmtId, short resId)
		{
			msgIn.Clear(64);
			msgIn.ReqSetStmtid(stmtId);
			msgIn.ReqSetCmd(44);
			msgIn.ReqMoreresSetResId(resId);
			msgIn.ReqSetLen();
		}

		public static void ExecDirectOpt(DmMsg msgIn, DmStatement stmt, List<SQLProcessor.Parameter> optParamList, DmConnProperty connProperty)
		{
			msgIn.Clear(64);
			msgIn.ReqSetStmtid(stmt.Handle);
			msgIn.ReqSetCmd(91);
			if (stmt.ConnInst.GetAutoCommit())
			{
				msgIn.ReqExecuteSetAutoCmt(1);
			}
			else
			{
				msgIn.ReqExecuteSetAutoCmt(0);
			}
			msgIn.ReqExecuteSetParamNum(optParamList.Count);
			msgIn.ReqExecuteSetCurForwardOnly(0);
			msgIn.ReqExecuteSetRowNum(1L);
			msgIn.ReqExecuteSetCurPos(stmt.GetCursorUpdateRow());
			msgIn.ReqExecuteSetMaxRows(long.MaxValue);
			msgIn.WriteStringWithNTS(stmt.Org_sql, connProperty.ServerEncoding);
			foreach (SQLProcessor.Parameter optParam in optParamList)
			{
				msgIn.WriteByte((byte)optParam.ioType);
				msgIn.WriteInt(optParam.type);
				msgIn.WriteInt(optParam.prec);
				msgIn.WriteInt(optParam.scale);
			}
			foreach (SQLProcessor.Parameter optParam2 in optParamList)
			{
				if (optParam2.bytes == null)
				{
					msgIn.WriteShort(-2);
					continue;
				}
				msgIn.WriteShort((short)optParam2.bytes.Length);
				msgIn.WriteBytes(optParam2.bytes);
			}
			msgIn.ReqSetLen();
		}

		public static void SetSessIso(DmMsg msgIn, int il)
		{
			msgIn.Clear(64);
			msgIn.ReqSetStmtid(0);
			msgIn.ReqSetCmd(52);
			msgIn.ReqSetSessIso(il);
			msgIn.ReqSetLen();
		}

		public static void TableTs(DmMsg msgIn, int[] ids)
		{
			msgIn.Clear(64);
			msgIn.ReqSetCmd(71);
			msgIn.ReqTableTsSetIdNum((short)ids.Length);
			foreach (int val in ids)
			{
				msgIn.WriteInt(val);
			}
			msgIn.ReqSetLen();
		}
	}
}
