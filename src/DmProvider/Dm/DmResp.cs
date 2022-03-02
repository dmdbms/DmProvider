using System;
using System.Data;
using Dm.util;

namespace Dm
{
	internal class DmResp
	{
		public static void Simple(DmMsg msg, DmConnProperty connProperty)
		{
			if (msg.ResGetSqlcode() < 0)
			{
				ThrowServerException(msg, connProperty.ServerEncoding, connProperty.RWStandby);
			}
		}

		public static void StartupServer(DmMsg msg, DmConnProperty connProperty)
		{
			DmError err = new DmError();
			if (msg.ResGetSqlcode() < 0)
			{
				ThrowServerException(msg, connProperty.ServerEncoding, connProperty.RWStandby);
			}
			connProperty.Encrypt = msg.ResStartupGetEncryptType();
			connProperty.Serials = msg.ResStartupGetSerial();
			switch (msg.ResStartupGetEncoding())
			{
			case 1:
				connProperty.ServerEncoding = "UTF-8";
				break;
			case 0:
				connProperty.ServerEncoding = "GB18030";
				break;
			case 2:
				connProperty.ServerEncoding = "euc-kr";
				break;
			}
			connProperty.Compress = msg.ResStartupGetCmprsMsg();
			int num = msg.ResStartupGetGenKeyPairFlag();
			byte num2 = msg.ResStartupGetCommEncFlag();
			connProperty.CaseSensitive = msg.ResStartupGetScFlag() == 1;
			connProperty.IsBdtaRs = msg.ResStartupGetRsBdtaFlag() == 2;
			connProperty.CrcBody = msg.ResStartupGetCrcFlag() == 1;
			connProperty.msgVersion = msg.ResStartupGetProtocolVersion();
			msg.GetErrorInfo(err, connProperty.ServerEncoding);
			string text = msg.ReadStringWith4Length(connProperty.ServerEncoding);
			text = text.Insert(text.Length, "-");
			int num3 = ProviderVersion2Num(text);
			if (num3 < 117440512)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_SVR_VERSION_WRONG);
			}
			connProperty.ServerVersion = text;
			connProperty.VerNum = num3;
			if (num2 > 0)
			{
				connProperty.encryptType = msg.ReadInt();
			}
			if (num > 0)
			{
				if (connProperty.encryptType == -1)
				{
					connProperty.encryptPwd = true;
				}
				else
				{
					connProperty.encryptMsg = true;
				}
				connProperty.serverPubKey = msg.ReadBytes(msg.ReadInt());
			}
			if (num2 == 2)
			{
				connProperty.hashType = msg.ReadInt();
			}
		}

		public static void Login(DmMsg msg, DmConnProperty connProperty)
		{
			DmError err = new DmError();
			if (msg.ResGetSqlcode() < 0)
			{
				ThrowServerException(msg, connProperty.ServerEncoding, connProperty.RWStandby);
			}
			connProperty.MaxRowSize = msg.ResLoginGetMaxDataLen();
			connProperty.MaxSession = msg.ResLoginGetMaxSession();
			connProperty.DDLAutoCommit = msg.ResLoginGetDdlAutoCommit();
			switch (msg.ResLoginGetIsoLevel())
			{
			case 1:
				connProperty.IsolationLevel = IsolationLevel.ReadCommitted;
				break;
			case 3:
				connProperty.IsolationLevel = IsolationLevel.Serializable;
				break;
			case 0:
				connProperty.IsolationLevel = IsolationLevel.ReadUncommitted;
				break;
			default:
				connProperty.IsolationLevel = IsolationLevel.Unspecified;
				break;
			}
			connProperty.CaseSensitive = msg.ResLoginGetStrCaseSensitive() == 1;
			connProperty.BackslashEsc = msg.ResLoginGetBackSlash();
			connProperty.C2p = msg.ResLoginGetC2p();
			connProperty.DbTimeZone = msg.ResLoginGetDbTimeZone();
			connProperty.DbrwSeparate = ((msg.ResLoginGetStandbyFlag() == 1) ? true : false);
			connProperty.NewLobFlag = msg.ResLoginGetNewLobFlag();
			if (connProperty.BufPrefetch == 0)
			{
				connProperty.BufPrefetch = msg.ResLoginGetFetchPackSize();
			}
			connProperty.SvrMode = msg.ResLoginGetSvrMode();
			connProperty.SvrStat = msg.ResLoginGetSvrStat();
			connProperty.DscControl = msg.ResLoginGetDscControl() == 1;
			msg.GetErrorInfo(err, connProperty.ServerEncoding);
			connProperty.Database = msg.ReadStringWith4Length(connProperty.ServerEncoding);
			int num = msg.ReadInt();
			if (num == 0 && connProperty.msgVersion > 0)
			{
				connProperty.CurrentSchema = connProperty.User.ToUpper();
			}
			else
			{
				connProperty.CurrentSchema = msg.ReadString(num, connProperty.ServerEncoding);
			}
			string text2 = (connProperty.LastLoginIP = msg.ReadStringWith4Length(connProperty.ServerEncoding));
			string text4 = (connProperty.LastLoginTime = msg.ReadStringWith4Length(connProperty.ServerEncoding));
			connProperty.FailedAttempts = msg.ReadInt();
			connProperty.LoginWarningID = msg.ReadInt();
			connProperty.GracetimeRemainder = msg.ReadInt();
			connProperty.Guid = msg.ReadStringWith4Length(connProperty.ServerEncoding);
			msg.SkipRead(msg.ReadInt());
			if (connProperty.DbrwSeparate)
			{
				connProperty.StandbyIp = msg.ReadStringWith4Length(connProperty.ServerEncoding);
				connProperty.StandbyPort = msg.ReadInt();
				connProperty.StandbyNum = msg.ReadShort();
			}
			if (msg.Read < msg.ReqGetLen() + 64)
			{
				connProperty.SessId = msg.ReadLong();
			}
			if (msg.Read < msg.ResGetLen() + 64 && msg.ReadByte() == 1)
			{
				connProperty.oracleDateFormat = "DD-MON-YY";
				connProperty.oracleTimeFormat = "HH12.MI.SS.FF6 AM";
				connProperty.oracleTimestampFormat = "DD-MON-YY HH12.MI.SS.FF6 AM";
				connProperty.oracleTimestampTZFormat = "DD-MON-YY HH12.MI.SS.FF6 AM +TZH:TZM";
				connProperty.oracleTimeTZFormat = "HH12.MI.SS.FF6 AM +TZH:TZM";
				connProperty.nlsDateLang = 0;
			}
			if (msg.Read < msg.ResGetLen() + 64)
			{
				string text5 = msg.ReadStringWith1Length(connProperty.ServerEncoding);
				if (StringUtil.isNotEmpty(text5))
				{
					connProperty.oracleDateFormat = text5;
				}
				text5 = msg.ReadStringWith1Length(connProperty.ServerEncoding);
				if (StringUtil.isNotEmpty(text5))
				{
					connProperty.oracleTimeFormat = text5;
				}
				text5 = msg.ReadStringWith1Length(connProperty.ServerEncoding);
				if (StringUtil.isNotEmpty(text5))
				{
					connProperty.oracleTimestampFormat = text5;
				}
				text5 = msg.ReadStringWith1Length(connProperty.ServerEncoding);
				if (StringUtil.isNotEmpty(text5))
				{
					connProperty.oracleTimestampTZFormat = text5;
				}
				text5 = msg.ReadStringWith1Length(connProperty.ServerEncoding);
				if (StringUtil.isNotEmpty(text5))
				{
					connProperty.oracleTimeTZFormat = text5;
				}
			}
		}

		public static int AllocStmt(DmMsg msg, DmConnProperty connProperty, ref bool m_new_col_desc)
		{
			if (msg.ResGetSqlcode() < 0)
			{
				ThrowServerException(msg, connProperty.ServerEncoding, connProperty.RWStandby);
			}
			m_new_col_desc = ((msg.ReqAllocStmtGetNewColDescFlag() == 1) ? true : false);
			return msg.ResGetStmtid();
		}

		private static void GetParamsInfo(DmMsg msg, int paraNum, ref DmInfo info)
		{
			if (paraNum == 0)
			{
				return;
			}
			int read = msg.Read;
			DmParameterInternal[] ParamsInfo = null;
			info.GetParamsInfo(out ParamsInfo);
			for (int i = 0; i < paraNum; i++)
			{
				read = msg.Read;
				short paramType = msg.GetParamType(read);
				int num = msg.GetdType(read);
				int prec = msg.GetPrec(read);
				int num2 = msg.GetScale(read);
				if (num == 12 && num2 == 5)
				{
					throw new InvalidOperationException("UNSUPPORT USERDEFINED TYPE");
				}
				bool nullable = msg.GetNullable(read);
				short itemFlag = msg.GetItemFlag(read);
				byte typeFlag = (byte)((((uint)itemFlag & 8u) != 0) ? 2 : ((num != 54) ? 1 : 0));
				ParamsInfo[i].SetIsLob((itemFlag & 2) != 0);
				short nameLen = msg.GetNameLen(read);
				short typeNameLen = msg.GetTypeNameLen(read);
				short tableLen = msg.GetTableLen(read);
				short schemaLen = msg.GetSchemaLen(read);
				msg.SkipRead(32);
				ParamsInfo[i].SetName(msg, nameLen);
				ParamsInfo[i].SetTypeName(msg, typeNameLen);
				ParamsInfo[i].SetTable(msg, tableLen);
				ParamsInfo[i].SetSchema(msg, schemaLen);
				if (num != 54)
				{
					ParamsInfo[i].SetCType(num);
				}
				else
				{
					ParamsInfo[i].SetCType(2);
				}
				if (num == 12 || num == 19)
				{
					num2 = 0;
				}
				if (num == 54 && prec == 0)
				{
					ParamsInfo[i].SetPrecision(8188);
				}
				else
				{
					ParamsInfo[i].SetPrecision(prec);
				}
				ParamsInfo[i].SetSize(DmSqlType.GetSizeByCType(num, prec));
				ParamsInfo[i].SetScale(num2);
				ParamsInfo[i].SetNullable(nullable);
				ParamsInfo[i].SetTypeFlag(typeFlag);
				ParamsInfo[i].SetInOutType(paramType);
				if (ParamsInfo[i].GetIsLob())
				{
					ParamsInfo[i].SetTableID(msg.ReadInt());
					ParamsInfo[i].SetColID(msg.ReadShort());
				}
			}
			for (int j = 0; j < paraNum; j++)
			{
				if (DmSqlType.IsComplexType(ParamsInfo[j].GetCType(), ParamsInfo[j].GetScale()))
				{
					TypeDescriptor typeDescriptor = new TypeDescriptor(info.ConnInstance.Conn);
					typeDescriptor.Unpack(msg);
					ParamsInfo[j].TypeDesc = typeDescriptor;
				}
			}
		}

		internal static void GetColsInfo(DmMsg msg, int colNum, DmInfo info, DmConnInstance conn, bool read_base_col_name)
		{
			if (colNum == 0)
			{
				return;
			}
			int read = msg.Read;
			DmColumn[] array = new DmColumn[colNum];
			for (int i = 0; i < colNum; i++)
			{
				array[i] = new DmColumn(conn);
				read = msg.Read;
				int num = msg.GetdType(read);
				int prec = msg.GetPrec(read);
				int scale = msg.GetScale(read);
				bool nullable = msg.GetNullable(read);
				short itemFlag = msg.GetItemFlag(read);
				short paramType = msg.GetParamType(read);
				short nameLen = msg.GetNameLen(read);
				short typeNameLen = msg.GetTypeNameLen(read);
				short tableLen = msg.GetTableLen(read);
				short schemaLen = msg.GetSchemaLen(read);
				msg.SkipRead(32);
				if (num == 12 && scale == 5)
				{
					throw new InvalidOperationException("UNSUPPORT USERDEFINED TYPE");
				}
				bool identity = (((itemFlag & 1) == 1) ? true : false);
				bool flag = (((itemFlag & 2) == 2) ? true : false);
				array[i].SetName(msg, nameLen);
				array[i].SetTypeName(msg, typeNameLen);
				array[i].SetTable(msg, tableLen);
				array[i].SetSchema(msg, schemaLen);
				if (read_base_col_name)
				{
					short num2 = 0;
					num2 = msg.ReadShort();
					if (num2 > 0)
					{
						array[i].SetBaseColumn(msg, num2);
					}
				}
				array[i].SetCType(num);
				array[i].SetPrecision(prec);
				array[i].SetSize(DmSqlType.GetSizeByCType(num, prec));
				array[i].SetScale(scale);
				array[i].SetNullable(nullable);
				array[i].SetIdentity(identity);
				array[i].SetIsLob(flag);
				array[i].SetInOutType(paramType);
				if (flag)
				{
					array[i].SetTableID(msg.ReadInt());
					array[i].SetColID(msg.ReadShort());
				}
			}
			for (int j = 0; j < colNum; j++)
			{
				if (DmSqlType.IsComplexType(array[j].GetCType(), array[j].GetScale()))
				{
					TypeDescriptor typeDescriptor = new TypeDescriptor(info.ConnInstance.Conn);
					typeDescriptor.Unpack(msg);
					array[j].TypeDesc = typeDescriptor;
				}
			}
			info.SetColumnsInfo(array);
			info.SetHasResultSet(hasResultSet: true);
		}

		public static void Prepare(DmMsg msg, DmStatement stmt)
		{
			if (msg.ResGetSqlcode() < 0)
			{
				ThrowServerException(msg, stmt.ConnInst.ConnProperty.ServerEncoding, stmt.ConnInst.ConnProperty.RWStandby);
			}
			DmInfo info = new DmInfo(stmt.ConnInst);
			int num = msg.ResGetStmtid();
			int num2 = msg.ResPrepareGetParamNum();
			int num3 = msg.ResPrepareGetColNum();
			int num4 = msg.ResPrepareGetRetType();
			info.SetRetStmtType(num4);
			if (num4 == 200 || num4 == 201)
			{
				foreach (DmStatement stmt2 in stmt.ConnInst.Stmts)
				{
					if (stmt2.Handle == num)
					{
						stmt.RsCache = stmt2.RsCache;
						break;
					}
				}
				return;
			}
			if (num2 > 0)
			{
				info.SetParaNum(num2);
				GetParamsInfo(msg, num2, ref info);
			}
			else
			{
				info.SetParaNum(0);
			}
			if (num3 > 0)
			{
				GetColsInfo(msg, num3, info, stmt.ConnInst, stmt.New_col_desc);
			}
			stmt.ResultInfo = info;
		}

		private static void GetParamData(DmMsg msg, DmStatement stmt)
		{
			int num = 0;
			object obj = null;
			byte[] array = null;
			DmGetValue dmGetValue = new DmGetValue(stmt.ConnInst.ConnProperty.ServerEncoding, stmt, stmt.ConnInst.ConnProperty.NewLobFlag);
			DmParameterInternal[] ParamsInfo = null;
			DmParameterInternal dmParameterInternal = null;
			DmParameter dmParameter = null;
			stmt.DbInfo.GetParamsInfo(out ParamsInfo);
			for (int i = 0; i < stmt.DbInfo.GetParameterCount(); i++)
			{
				dmParameterInternal = ParamsInfo[i];
				int sqlType = dmParameterInternal.GetSqlType();
				int scale = dmParameterInternal.GetScale();
				if (sqlType == 12 && scale == 5)
				{
					throw new InvalidOperationException("UNSUPPORT USERDEFINED TYPE");
				}
				dmParameter = ((!dmParameterInternal.GetName().Equals(string.Empty)) ? ((DmParameter)stmt.Command.Parameters[dmParameterInternal.GetName()]) : ((DmParameter)stmt.Command.Parameters[i]));
				if (dmParameter.do_Direction == ParameterDirection.Input)
				{
					continue;
				}
				num = msg.ReadShort();
				if (num > 0)
				{
					array = msg.ReadBytes(num);
				}
				else if (num == -2 && dmParameter.DmSqlType != DmDbType.Cursor)
				{
					dmParameter.do_Value = DBNull.Value;
					continue;
				}
				switch (dmParameter.DmSqlType)
				{
				case DmDbType.Cursor:
					if (dmParameterInternal.GetCType() == 120)
					{
						obj = dmParameterInternal;
						if (dmParameter.do_Direction == ParameterDirection.ReturnValue)
						{
							stmt.Csi.MoreResult(stmt.Command.RetRefCursorStmt, 1);
						}
						else
						{
							stmt.Csi.MoreResult(dmParameter.refCursorStmt, 1);
						}
						num = 0;
					}
					break;
				case DmDbType.Blob:
				case DmDbType.Binary:
				case DmDbType.VarBinary:
					obj = dmGetValue.GetBytes(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.Bit:
					obj = dmGetValue.GetBoolean(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.Char:
				case DmDbType.Clob:
				case DmDbType.Text:
				case DmDbType.VarChar:
					obj = dmGetValue.GetString(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.Time:
					obj = dmGetValue.GetTime(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.Date:
					obj = dmGetValue.GetDate(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.DateTime:
					obj = dmGetValue.GetTimestamp(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.DateTimeOffset:
					obj = dmGetValue.GetTimestampTZ(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.TimeOffset:
					obj = dmGetValue.GetTimeTZ(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.Decimal:
					obj = dmGetValue.GetBigDecimal(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.XDEC:
					obj = dmGetValue.GetDmDecimal(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.Double:
					obj = dmGetValue.GetDouble(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.Float:
					obj = dmGetValue.GetFloat(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.IntervalDayToSecond:
					obj = dmGetValue.GetINTERVALDT(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.IntervalYearToMonth:
					obj = dmGetValue.GetINTERVALYM(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.Int16:
					obj = dmGetValue.GetShort(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.Int32:
					obj = dmGetValue.GetInt(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.Int64:
					obj = dmGetValue.GetLong(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.SByte:
					obj = dmGetValue.GetSByte(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.Byte:
					obj = dmGetValue.GetByte(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.UInt16:
					obj = dmGetValue.GetUshort(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.UInt32:
					obj = dmGetValue.GetUint(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.UInt64:
					obj = dmGetValue.GetUlong(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				case DmDbType.ARRAY:
					obj = dmGetValue.ToComplexType(array, dmParameterInternal, stmt.ConnInst.Conn);
					if (obj is DmArray)
					{
						TypeData[] arrData = ((DmArray)obj).m_arrData;
						if (arrData == null || arrData.Length == 0)
						{
							obj = null;
							break;
						}
						object[] array2 = new object[arrData.Length];
						for (int j = 0; j < arrData.Length; j++)
						{
							array2[j] = arrData[j].m_dumyData;
						}
						obj = array2;
					}
					else if (obj is DmStruct)
					{
						throw new NotSupportedException("GetParamData");
					}
					break;
				default:
					array = msg.ReadBytes(num);
					obj = dmGetValue.GetObject(1, array, dmParameterInternal.GetCType(), dmParameterInternal.GetPrecision(), dmParameterInternal.GetScale());
					break;
				}
				dmParameter.do_Value = obj;
			}
		}

		public static void Execute(DmMsg msg, DmStatement stmt, DmConnProperty connProperty)
		{
			int num = msg.ResGetSqlcode();
			if (num < 0)
			{
				ThrowServerException(msg, connProperty.ServerEncoding, connProperty.RWStandby);
			}
			DmInfo dmInfo = new DmInfo(stmt.ConnInst);
			dmInfo.SetParaNum(stmt.ResultInfo.GetParameterCount());
			dmInfo.SetParamsInfo(stmt.ResultInfo.GetParamsInfo());
			int num2 = msg.ResGetStmtid();
			int num3 = msg.ResGetLen();
			short num4 = msg.ResExecuteGetRetType();
			short num5 = msg.ResExecuteGetColNum();
			long num6 = msg.ResExecuteGetRowNum();
			int num7 = msg.ResExecuteGetParamNum();
			byte num8 = msg.ResExecuteGetRsUpdatable();
			int num9 = msg.ResExecuteGetFetchedRows();
			int num10 = msg.ResExecuteGetPrintOff();
			long num11 = 0L;
			bool isRsBdta = false;
			dmInfo.Execid = msg.ResExecuteGetExecid();
			int num12 = msg.ResExecuteGetRsCacheOffset();
			msg.ResExecuteGetTransStatus();
			short rsBdtaRowidCol = -1;
			if (num4 == 160 || num4 == 162)
			{
				num11 = 0L;
				isRsBdta = msg.ResExecuteGetRsBdtaFlag() == 2;
				rsBdtaRowidCol = msg.ResExecuteGetRsRowIdColIndex();
			}
			else
			{
				num11 = msg.ResExecuteGetRowid();
			}
			dmInfo.SetRetStmtType(num4);
			dmInfo.SetOutParamNum(num7);
			if (num4 == 159 || num4 == 158 || num4 == 157 || num4 == 160 || num4 == 162)
			{
				dmInfo.SetRowCount(num6);
			}
			else
			{
				dmInfo.SetRowCount(-1L);
			}
			switch (num4)
			{
			case 157:
			case 158:
			case 159:
				dmInfo.SetRecordsAffected(num6);
				break;
			case 160:
				dmInfo.SetRecordsAffected(-1L);
				break;
			}
			if (num8 == 1)
			{
				dmInfo.SetUpdatable(val: true);
			}
			else
			{
				dmInfo.SetUpdatable(val: false);
			}
			if (num10 > 0)
			{
				string printMsg = msg.ReadString(num10, stmt.ConnInst.ConnProperty.ServerEncoding);
				dmInfo.SetPrintMsg(printMsg);
			}
			if (num7 > 0)
			{
				GetParamData(msg, stmt);
			}
			int num13;
			switch (num4)
			{
			case 150:
			case 166:
			{
				short num15 = msg.ReadShort();
				msg.SkipRead(1);
				if (num4 == 166)
				{
					IsolationLevel trxISO = IsolationLevel.ReadCommitted;
					switch (num15)
					{
					case 1:
						trxISO = IsolationLevel.ReadCommitted;
						break;
					case 3:
						trxISO = IsolationLevel.Serializable;
						break;
					case 0:
						trxISO = IsolationLevel.ReadUncommitted;
						break;
					}
					stmt.ConnInst.SetTrxISO(trxISO);
					stmt.Csi.CsiSerial = true;
				}
				else
				{
					if (stmt.Command != null)
					{
						stmt.Command.SetStmtSerial(num15);
					}
					if (stmt.ConnInst.Transaction != null)
					{
						stmt.ConnInst.Transaction.SetStmtSerial(num15);
					}
				}
				break;
			}
			case 165:
				connProperty.TimeZone = msg.ReadShort();
				break;
			case 147:
				connProperty.Commited = true;
				break;
			case 148:
				connProperty.Commited = true;
				stmt.ConnInst.RestAllStmt();
				break;
			case 200:
			case 201:
				if (num != 107)
				{
					break;
				}
				foreach (DmStatement stmt2 in stmt.ConnInst.Stmts)
				{
					if (stmt2.Handle == num2)
					{
						stmt.RsCache = stmt2.RsCache;
						break;
					}
				}
				stmt.Csi.ExecutePrepared(stmt, stmt.RsCache.Statement.ResultInfo);
				break;
			case 153:
			{
				int byteLen = msg.ReadInt();
				string text2 = (connProperty.CurrentSchema = msg.ReadString(byteLen, connProperty.ServerEncoding));
				break;
			}
			case 160:
				dmInfo.SetHasResultSet(hasResultSet: true);
				goto IL_03e6;
			case 162:
				if (num5 > 0 || num9 > 0)
				{
					dmInfo.SetHasResultSet(hasResultSet: true);
				}
				goto IL_03e6;
			case 157:
			case 159:
				dmInfo.SetRowId(num11);
				break;
			case 149:
				stmt.SetExplain(msg.ReadStringWith4Length(connProperty.ServerEncoding));
				break;
			case 251:
				connProperty.oracleDateFormat = msg.ReadStringWith1Length(connProperty.ServerEncoding);
				break;
			case 253:
				connProperty.oracleTimestampFormat = msg.ReadStringWith1Length(connProperty.ServerEncoding);
				break;
			case 254:
				connProperty.oracleTimestampTZFormat = msg.ReadStringWith1Length(connProperty.ServerEncoding);
				break;
			case 252:
				connProperty.oracleTimeFormat = msg.ReadStringWith1Length(connProperty.ServerEncoding);
				break;
			case 255:
				connProperty.oracleTimeTZFormat = msg.ReadStringWith1Length(connProperty.ServerEncoding);
				break;
			case 256:
				{
					connProperty.nlsDateLang = msg.ReadByte();
					break;
				}
				IL_03e6:
				if (num5 != 0)
				{
					GetColsInfo(msg, num5, dmInfo, stmt.ConnInst, stmt.New_col_desc);
					num13 = dmInfo.GetColumnsInfo().Length;
				}
				else if (stmt.ResultInfo != null)
				{
					DmColumn[] columnsInfo = stmt.ResultInfo.GetColumnsInfo();
					dmInfo.SetColumnsInfo(columnsInfo);
					num13 = ((columnsInfo != null) ? columnsInfo.Length : 0);
				}
				else
				{
					num13 = 0;
				}
				if (stmt.RsCache == null)
				{
					stmt.RsCache = new DmResultSetCache(stmt, num13, dmInfo.GetRowCount());
				}
				else
				{
					stmt.RsCache.SetCols(num13);
					stmt.RsCache.TotalRows = dmInfo.GetRowCount();
				}
				if (num9 > 0)
				{
					int len = num3 + 64 - msg.Read;
					byte[] rowsBuf = msg.ReadBytes(len);
					stmt.RsCache.FillRows(0L, num9, rowsBuf, isRsBdta, rsBdtaRowidCol);
					if (stmt.ConnInst.ConnProperty.EnRsCache && num12 > 0 && dmInfo.GetRowCount() == num9)
					{
						RsKey key = new RsKey(stmt.ConnInst.ConnProperty.Guid, stmt.ConnInst.ConnProperty.CurrentSchema, stmt.Org_sql, stmt.Command.do_DbParameterCollection.Count, stmt.Command.do_DbParameterCollection);
						DmConnection.rsLRUCache.Add(key, stmt.RsCache);
					}
				}
				if (num12 > 0)
				{
					msg.SkipRead(num12 - msg.Read);
					short num14 = msg.ReadShort();
					int[] array = new int[num14];
					long[] array2 = new long[num14];
					for (int i = 0; i < num14; i++)
					{
						array[i] = msg.ReadInt();
						array2[i] = msg.ReadLong();
					}
					stmt.RsCache.ids = array;
					stmt.RsCache.tss = array2;
					stmt.RsCache.lastCheckDt = DateTime.Now;
				}
				break;
			}
			stmt.ResultInfo = dmInfo;
		}

		public static bool Fetch(DmMsg msg, long rowPos, DmResultSetCache rsCache)
		{
			bool result = false;
			int num = msg.ResGetSqlcode();
			if (num < 0 && num != -7036)
			{
				ThrowServerException(msg, rsCache.Statement.ConnInst.ConnProperty.ServerEncoding, rsCache.Statement.ConnInst.ConnProperty.RWStandby);
			}
			long totalRows = msg.ResFetchGetRowCount();
			int rowNum = msg.ResFetchGetRetCount();
			byte[] rowsBuf = null;
			if (num == -7036)
			{
				result = true;
			}
			else
			{
				rowsBuf = msg.ReadBytes(msg.ResGetLen());
			}
			rsCache.TotalRows = totalRows;
			rsCache.FillRows(rowPos, rowNum, rowsBuf, rsCache.isRsBdta, rsCache.rsBdtaRowidCol);
			return result;
		}

		public static byte[] GetData(DmMsg msg, DmLob lob, DmConnInstance conn)
		{
			if (msg.ResGetSqlcode() < 0)
			{
				ThrowServerException(msg, conn.ConnProperty.ServerEncoding, conn.ConnProperty.RWStandby);
			}
			msg.SkipRead(1);
			int len = msg.ReadInt();
			msg.SkipRead(2);
			msg.SkipRead(4);
			msg.SkipRead(2);
			return msg.ReadBytes(len);
		}

		public static int GetLobLen(DmMsg msg, DmConnProperty connProperty)
		{
			if (msg.ResGetSqlcode() < 0)
			{
				ThrowServerException(msg, connProperty.ServerEncoding, connProperty.RWStandby);
			}
			return msg.ReadInt();
		}

		public static int SetLobData(DmMsg recv, DmLob lob, DmConnProperty connProperty)
		{
			if (!recv.CheckCRC())
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_MSG_CHECK_ERROR);
			}
			if (recv.ResGetSqlcode() < 0)
			{
				ThrowServerException(recv, connProperty.ServerEncoding, connProperty.RWStandby);
			}
			int result = recv.ReadInt();
			long blobid = recv.ReadLong();
			lob.setBlobid(blobid);
			lob.setGroupid(recv.ReadShort());
			lob.setFileid(recv.ReadShort());
			lob.setPageno(recv.ReadInt());
			lob.setCurFileid(recv.ReadBytes(2));
			lob.setCurPageno(recv.ReadBytes(4));
			lob.setCurOff(recv.ReadBytes(4));
			return result;
		}

		public static void LobTrunc(DmMsg msg, DmLob lob, DmConnProperty connProperty)
		{
			if (!msg.CheckCRC())
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_MSG_CHECK_ERROR);
			}
			if (msg.ResGetSqlcode() < 0)
			{
				ThrowServerException(msg, connProperty.ServerEncoding, connProperty.RWStandby);
			}
			msg.SkipRead(4);
			long blobid = msg.ReadLong();
			lob.setBlobid(blobid);
			DmConvertion.SetShort(lob.cur_fileid, 0, lob.m_data_fileid);
			DmConvertion.SetInt(lob.cur_pageno, 0, lob.m_data_pageno);
			DmConvertion.SetInt(lob.m_curOff, 0, 0);
		}

		public static byte[] GetLobData(DmMsg recv, DmLob lob, DmConnProperty connProperty)
		{
			if (recv.ResGetSqlcode() < 0)
			{
				ThrowServerException(recv, connProperty.ServerEncoding, connProperty.RWStandby);
			}
			bool isFree = false;
			if (recv.ReadByte() != 0)
			{
				isFree = true;
			}
			lob.setIsFree(isFree);
			int num = recv.ReadInt();
			if (num <= 0)
			{
				return null;
			}
			lob.setCurFileid(recv.ReadBytes(2));
			lob.setCurPageno(recv.ReadBytes(4));
			lob.setCurOff(recv.ReadBytes(4));
			return recv.ReadBytes(num);
		}

		public static void GetMoreResult(DmMsg msg, DmStatement stmt, DmConnProperty connProperty)
		{
			Execute(msg, stmt, connProperty);
		}

		public static long[] GetTabelTs(DmMsg msg, DmConnProperty connProperty)
		{
			if (msg.ResGetSqlcode() < 0)
			{
				ThrowServerException(msg, connProperty.ServerEncoding, connProperty.RWStandby);
			}
			int num = msg.ResTableTsGetIdNum();
			if (num <= 0)
			{
				return null;
			}
			long[] array = new long[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = msg.ReadLong();
			}
			return array;
		}

		private static int ProviderVersion2Num(string version)
		{
			int num = 0;
			char[] array = version.ToCharArray();
			int i = 0;
			if (array[i] == 'V')
			{
				i++;
			}
			int num2 = version.IndexOf('.');
			if (num2 == -1)
			{
				return 0;
			}
			int num3 = 0;
			for (; i < num2; i++)
			{
				if (array[i] < '0' || array[i] > '9')
				{
					return 0;
				}
				num3 = num3 * 10 + (array[i] - 48);
				if (num3 > 99)
				{
					return 0;
				}
			}
			i++;
			num = num3 << 24;
			version = version.Substring(num2 + 1, version.Length - num2 - 1);
			int num4 = version.IndexOf('.');
			if (num4 == -1)
			{
				return 0;
			}
			num2++;
			num2 += num4;
			num3 = 0;
			for (; i < num2; i++)
			{
				if (array[i] < '0' || array[i] > '9')
				{
					return 0;
				}
				num3 = num3 * 10 + (array[i] - 48);
				if (num3 > 99)
				{
					return 0;
				}
			}
			i++;
			num += num3 << 16;
			version = version.Substring(num4 + 1, version.Length - num4 - 1);
			num4 = version.IndexOf('.');
			if (num4 == -1)
			{
				return 0;
			}
			num2++;
			num2 += num4;
			num3 = 0;
			for (; i < num2; i++)
			{
				if (array[i] < '0' || array[i] > '9')
				{
					return 0;
				}
				num3 = num3 * 10 + (array[i] - 48);
				if (num3 > 99)
				{
					return 0;
				}
			}
			i++;
			num += num3 << 8;
			version = version.Substring(num4 + 1, version.Length - num4 - 1);
			num4 = version.IndexOf('-');
			if (num4 == -1)
			{
				return 0;
			}
			num2++;
			num2 += num4;
			num3 = 0;
			for (; i < num2; i++)
			{
				if (array[i] < '0' || array[i] > '9')
				{
					return 0;
				}
				num3 = num3 * 10 + (array[i] - 48);
				if (num3 > 999)
				{
					return 0;
				}
			}
			i++;
			return num + (num3 & 0xFF);
		}

		private static int Version2Num(string version)
		{
			return ProviderVersion2Num(version);
		}

		private static void ThrowServerException(DmMsg msg, string charSet, bool rwStandby)
		{
			DmError dmError = new DmError();
			dmError.State = msg.ResGetSqlcode();
			msg.GetErrorInfo(dmError, charSet);
			if (rwStandby)
			{
				dmError.Message = "[S]" + dmError.Message;
			}
			DmError.ThrowDmException(dmError);
		}
	}
}
