using System;

namespace Dm
{
	internal class DmMsg
	{
		private byte[] buffer;

		private int read;

		private int write;

		internal byte[] Buffer => buffer;

		internal int Read => read;

		internal int Write
		{
			set
			{
				write = value;
			}
		}

		internal DmMsg()
		{
			buffer = new byte[32640];
		}

		internal DmMsg(byte[] buffer)
		{
			this.buffer = buffer;
		}

		internal void Clear(int offset)
		{
			write = offset;
			read = 0;
			Array.Clear(buffer, 0, offset);
		}

		internal void ReSizeBuffer(int len)
		{
			if (write + len > buffer.Length)
			{
				byte[] destinationArray = new byte[buffer.Length * 2 + len];
				Array.Copy(buffer, 0, destinationArray, 0, write);
				buffer = destinationArray;
			}
		}

		internal byte GetByte(int offset)
		{
			return buffer[offset];
		}

		internal short GetShort(int offset)
		{
			int num = 0xFF & buffer[offset];
			offset++;
			int num2 = 0xFF & buffer[offset];
			num2 = num | (num2 << 8);
			return (short)(0xFFFF & num2);
		}

		internal int GetUB2(int offset)
		{
			int num = 0xFF & buffer[offset];
			offset++;
			int num2 = 0xFF & buffer[offset];
			return num | (num2 << 8);
		}

		internal int GetInt(int offset)
		{
			int num = 4;
			int num2 = offset + num;
			long num3 = 0xFF & buffer[--num2];
			num3 = (0xFF & buffer[--num2]) | (num3 << 8);
			num3 = (0xFF & buffer[--num2]) | (num3 << 8);
			num3 = (0xFF & buffer[--num2]) | (num3 << 8);
			return (int)(0xFFFFFFFFu & num3);
		}

		internal long GetLong(int offset)
		{
			long num = 0L;
			int num2 = offset + 8;
			for (int i = 0; i < 8; i++)
			{
				num = (0xFF & buffer[--num2]) | (num << 8);
			}
			return num;
		}

		internal byte[] GetBytes(int offset, int len)
		{
			return DmConvertion.GetBytes(buffer, offset, len);
		}

		internal string GetString(int offset, int byteLen, string charsetName)
		{
			return DmConvertion.GetString(buffer, offset, byteLen, charsetName);
		}

		internal byte ReadByte()
		{
			byte @byte = GetByte(read);
			read++;
			return @byte;
		}

		internal short ReadShort()
		{
			short @short = GetShort(read);
			read += 2;
			return @short;
		}

		internal int ReadInt()
		{
			int @int = GetInt(read);
			read += 4;
			return @int;
		}

		internal long ReadLong()
		{
			long @long = GetLong(read);
			read += 8;
			return @long;
		}

		internal byte[] ReadBytes(int len)
		{
			byte[] bytes = GetBytes(read, len);
			read += len;
			return bytes;
		}

		internal string ReadString(int byteLen, string charsetName)
		{
			string @string = GetString(read, byteLen, charsetName);
			read += byteLen;
			return @string;
		}

		internal string ReadStringWith1Length(string charsetName)
		{
			return ReadString(ReadByte(), charsetName);
		}

		internal string ReadStringWith2Length(string charsetName)
		{
			return ReadString(ReadShort(), charsetName);
		}

		internal string ReadStringWith4Length(string charsetName)
		{
			return ReadString(ReadInt(), charsetName);
		}

		internal void SkipRead(int len)
		{
			read += len;
		}

		internal void SkipWrite(int len)
		{
			ReSizeBuffer(len);
			write += len;
		}

		public void SetByte(byte val, int offset)
		{
			buffer[offset] = val;
		}

		public void SetShort(int val, int offset)
		{
			buffer[offset] = (byte)((uint)val & 0xFFu);
			offset++;
			val >>= 8;
			buffer[offset] = (byte)((uint)val & 0xFFu);
		}

		public void SetUB2(int val, int offset)
		{
			buffer[offset] = (byte)((uint)val & 0xFFu);
			offset++;
			val >>= 8;
			buffer[offset] = (byte)((uint)val & 0xFFu);
		}

		public void SetInt(int val, int offset)
		{
			buffer[offset++] = (byte)((uint)val & 0xFFu);
			val >>= 8;
			buffer[offset++] = (byte)((uint)val & 0xFFu);
			val >>= 8;
			buffer[offset++] = (byte)((uint)val & 0xFFu);
			val >>= 8;
			buffer[offset++] = (byte)((uint)val & 0xFFu);
			val >>= 8;
		}

		public void SetLong(long val, int offset)
		{
			int num = 8;
			int num2 = offset;
			while (num-- > 0)
			{
				buffer[num2++] = (byte)(val & 0xFF);
				val >>= 8;
			}
		}

		public void SetBytes(int offset, byte[] src, int len)
		{
			SetBytes(offset, src, 0, len);
		}

		public void SetBytes(int offset, byte[] src, int src_offset, int len)
		{
			DmConvertion.SetBytes(buffer, offset, src, src_offset, len);
		}

		internal void WriteByte(byte val)
		{
			int num = 1;
			ReSizeBuffer(num);
			SetByte(val, write);
			write += num;
		}

		internal void WriteShort(short val)
		{
			int num = 2;
			ReSizeBuffer(num);
			SetShort(val, write);
			write += num;
		}

		internal void WriteInt(int val)
		{
			int num = 4;
			ReSizeBuffer(num);
			SetInt(val, write);
			write += num;
		}

		internal void WriteLong(long val)
		{
			int num = 8;
			ReSizeBuffer(num);
			SetLong(val, write);
			write += num;
		}

		internal void WriteBytes(byte[] src, int srcOffset, int len)
		{
			ReSizeBuffer(len);
			SetBytes(write, src, srcOffset, len);
			write += len;
		}

		internal void WriteBytes(byte[] src, int len)
		{
			WriteBytes(src, 0, len);
		}

		internal void WriteBytes(byte[] src)
		{
			WriteBytes(src, 0, src.Length);
		}

		internal void WriteBytesWith4Length(byte[] src)
		{
			WriteInt(src.Length);
			WriteBytes(src);
		}

		internal void WriteBytesWith2Length(byte[] src)
		{
			WriteShort((short)src.Length);
			WriteBytes(src);
		}

		internal void WriteStringWith2Length(string str, string serverEncoding)
		{
			WriteBytesWith2Length(DmConvertion.GetBytes(str, serverEncoding));
		}

		internal void WriteStringWithNTS(string str, string serverEncoding)
		{
			byte[] bytesWithNTS = DmConvertion.GetBytesWithNTS(str, serverEncoding);
			WriteBytes(bytesWithNTS);
		}

		public byte CalculateCRC()
		{
			byte b = buffer[0];
			byte b2 = 19;
			byte b3 = buffer[1];
			for (byte b4 = 1; b4 < b2; b4 = (byte)(b4 + 1))
			{
				b3 = buffer[b4];
				b = (byte)(b ^ b3);
			}
			return b;
		}

		public bool CheckCRC()
		{
			return true;
		}

		public short GetParamType(int offset)
		{
			return GetShort(offset + 22);
		}

		public bool GetNullable(int offset)
		{
			if (GetInt(offset + 12) == 0)
			{
				return false;
			}
			return true;
		}

		public short GetItemFlag(int offset)
		{
			return GetShort(offset + 16);
		}

		public short GetNameLen(int offset)
		{
			return GetShort(offset + 24);
		}

		public short GetTypeNameLen(int offset)
		{
			return GetShort(offset + 26);
		}

		public short GetTableLen(int offset)
		{
			return GetShort(offset + 28);
		}

		public short GetSchemaLen(int offset)
		{
			return GetShort(offset + 30);
		}

		public int GetdType(int offset)
		{
			return GetInt(offset);
		}

		public int GetPrec(int offset)
		{
			return GetInt(offset + 4);
		}

		public int GetScale(int offset)
		{
			return GetInt(offset + 8);
		}

		public void SetParamDataLength(int len, int offset)
		{
			SetShort(len, offset);
		}

		public void SetParamData(int offset, byte[] data)
		{
			SetBytes(offset, data, 0, data.Length);
		}

		public void GetErrorInfo(DmError err, string charsetName)
		{
			int num = ReadInt();
			if (num > 0)
			{
				err.Schema = ReadString(num, charsetName);
			}
			num = ReadInt();
			if (num > 0)
			{
				err.Table = ReadString(num, charsetName);
			}
			num = ReadInt();
			if (num > 0)
			{
				err.Col = ReadString(num, charsetName);
			}
			num = ReadInt();
			if (num > 0)
			{
				err.Message = ReadString(num, charsetName);
			}
		}

		public void ReqSetStmtid(int stmtid)
		{
			SetInt(stmtid, 0);
		}

		public void ReqSetCmd(short cmd)
		{
			SetShort(cmd, 4);
		}

		public short ReqGetCmd()
		{
			return GetShort(4);
		}

		public void ReqSetLen()
		{
			SetInt(write - 64, 6);
		}

		public int ReqGetLen()
		{
			return GetInt(6);
		}

		public void ReqSetCrc(byte crc)
		{
			SetByte(crc, 19);
		}

		public int ResGetStmtid()
		{
			return GetInt(0);
		}

		public short ResGetRet()
		{
			return GetShort(4);
		}

		public int ResGetLen()
		{
			return GetInt(6);
		}

		public void ResSetLen(int len)
		{
			SetInt(len, 6);
		}

		public int ResGetSqlcode()
		{
			return GetInt(10);
		}

		public int ResGetSvrMode()
		{
			return GetShort(14);
		}

		public byte ResGetCrc()
		{
			return GetByte(19);
		}

		public void ReqStartupSetEncryptType(int encrypt_type)
		{
			SetInt(encrypt_type, 20);
		}

		public void ReqStartupSetCmprsMsg(int cmprs_msg)
		{
			SetInt(cmprs_msg, 24);
		}

		public void ReqStartupSetGenKeyPairFlag(byte genKeyPairFlag)
		{
			SetByte(genKeyPairFlag, 28);
		}

		public void ReqStartupSetCommEncFlag(byte commEncFlag)
		{
			SetByte(commEncFlag, 29);
		}

		public void ReqStartupSetRsBdtaFlag(byte rsBdtaFlag)
		{
			SetByte(rsBdtaFlag, 30);
		}

		public void ReqStartupSetProtocolVersion(int version)
		{
			SetShort(version, 35);
		}

		public int ResStartupGetEncryptType()
		{
			return GetInt(20);
		}

		public int ResStartupGetSerial()
		{
			return GetInt(24);
		}

		public int ResStartupGetEncoding()
		{
			return GetInt(28);
		}

		public int ResStartupGetCmprsMsg()
		{
			return GetInt(32);
		}

		public int ResStartupGetScFlag()
		{
			return GetInt(36);
		}

		public byte ResStartupGetGenKeyPairFlag()
		{
			return GetByte(40);
		}

		public byte ResStartupGetCommEncFlag()
		{
			return GetByte(41);
		}

		public byte ResStartupGetRsBdtaFlag()
		{
			return GetByte(42);
		}

		public byte ResStartupGetCrcFlag()
		{
			return GetByte(45);
		}

		public short ResStartupGetProtocolVersion()
		{
			return GetShort(48);
		}

		public void ReqLoginSetEnv(int env)
		{
			SetInt(env, 20);
		}

		public void ReqLoginSetIsoLevel(int isoLevel)
		{
			SetInt(isoLevel, 24);
		}

		public void ReqLoginSetLanguage(int language)
		{
			SetInt(language, 28);
		}

		public void ReqLoginSetStandby(byte Standbyflag)
		{
			SetByte(Standbyflag, 40);
		}

		public void ReqLoginSetReadOnly(byte readOnly)
		{
			SetByte(readOnly, 32);
		}

		public void ReqLoginSetTimeZone(short timeZone)
		{
			SetShort(timeZone, 33);
		}

		public void ReqLoginSetSessTimeout(int sessTimeout)
		{
			SetInt(sessTimeout, 35);
		}

		public void ReqLoginSetLanguage(byte mpp_type)
		{
			SetByte(mpp_type, 39);
		}

		public void ReqLoginSetNewLobFlag(byte flag)
		{
			SetByte(flag, 41);
		}

		public int ResLoginGetMaxDataLen()
		{
			return GetInt(20);
		}

		public int ResLoginGetMaxSession()
		{
			return GetInt(24);
		}

		public byte ResLoginGetDdlAutoCommit()
		{
			return GetByte(28);
		}

		public int ResLoginGetIsoLevel()
		{
			return GetInt(29);
		}

		public byte ResLoginGetStrCaseSensitive()
		{
			return GetByte(33);
		}

		public byte ResLoginGetBackSlash()
		{
			return GetByte(34);
		}

		public byte ResLoginGetC2p()
		{
			return GetByte(39);
		}

		public short ResLoginGetDbTimeZone()
		{
			return GetShort(40);
		}

		public byte ResLoginGetStandbyFlag()
		{
			return GetByte(42);
		}

		public byte ResLoginGetNewLobFlag()
		{
			return GetByte(43);
		}

		public int ResLoginGetFetchPackSize()
		{
			return GetInt(44);
		}

		public byte ResLoginGetDscControl()
		{
			return GetByte(50);
		}

		public int ResLoginGetSvrStat()
		{
			return GetShort(37);
		}

		public int ResLoginGetSvrMode()
		{
			return GetShort(35);
		}

		public void ReqAllocStmtSetNewColDescFlag(byte flag)
		{
			SetByte(flag, 20);
		}

		public byte ReqAllocStmtGetNewColDescFlag()
		{
			return GetByte(20);
		}

		public void ReqPrepareSetAutoCmt(byte autoCmt)
		{
			SetByte(autoCmt, 20);
		}

		public void ReqPrepareSetExecDirect(byte execDirect)
		{
			SetByte(execDirect, 21);
		}

		public void ReqPrepareSetParamSequ(byte paramSequ)
		{
			SetByte(paramSequ, 22);
		}

		public void ReqPrepareSetCurForwardOnly(byte forwardOnly)
		{
			SetByte(forwardOnly, 23);
		}

		public void ReqPrepareSetCheckType(byte checkType)
		{
			SetByte(checkType, 24);
		}

		public void ReqPrepareSetSqlType(short sqlType)
		{
			SetShort(sqlType, 25);
		}

		public void ReqPrepareSetMaxRows(long rowNum)
		{
			SetLong(rowNum, 27);
		}

		public void ReqPrepareSetRsBdtaFlag(byte rsBdtaFlag)
		{
			SetByte(rsBdtaFlag, 35);
		}

		public void ReqPrepareSetRsBdtaLen(short rsBdtaLen)
		{
			SetShort(rsBdtaLen, 36);
		}

		public void ReqPrepareSetExecTimeout(int execTimeout)
		{
			SetInt(execTimeout, 41);
		}

		public short ResPrepareGetRetType()
		{
			return GetShort(20);
		}

		public int ResPrepareGetParamNum()
		{
			return GetUB2(22);
		}

		public short ResPrepareGetColNum()
		{
			return GetShort(24);
		}

		public int ResPrepareGetTraStatus()
		{
			return GetInt(26);
		}

		public void ReqExecuteSetAutoCmt(byte autoCmt)
		{
			SetByte(autoCmt, 20);
		}

		public void ReqExecuteSetParamNum(int paramNum)
		{
			SetUB2(paramNum, 21);
		}

		public void ReqExecuteSetCurForwardOnly(byte curForwardOnly)
		{
			SetByte(curForwardOnly, 23);
		}

		public void ReqExecuteSetRowNum(long rowNum)
		{
			SetLong(rowNum, 24);
		}

		public void ReqExecuteSetCurPos(long curPos)
		{
			SetLong(curPos, 32);
		}

		public void ReqExecuteSetMaxRows(long rowNum)
		{
			SetLong(rowNum, 40);
		}

		public void ReqExecuteIgnoreBatchError(bool batchContinueOnError)
		{
			SetByte((byte)(batchContinueOnError ? 1 : 0), 49);
		}

		public void ReqExecuteExecTimeout(int execTimeout)
		{
			SetInt(execTimeout, 52);
		}

		public void ReqExecuteBatchAllowMaxErrors(int batchAllowMaxErrors)
		{
			SetInt(batchAllowMaxErrors, 56);
		}

		public short ResExecuteGetRetType()
		{
			return GetShort(20);
		}

		public short ResExecuteGetColNum()
		{
			return GetShort(22);
		}

		public long ResExecuteGetRowNum()
		{
			return GetLong(24);
		}

		public int ResExecuteGetParamNum()
		{
			return GetUB2(32);
		}

		public byte ResExecuteGetRsUpdatable()
		{
			return GetByte(34);
		}

		public int ResExecuteGetFetchedRows()
		{
			return GetInt(35);
		}

		public int ResExecuteGetPrintOff()
		{
			return GetInt(39);
		}

		public long ResExecuteGetRowid()
		{
			return GetLong(43);
		}

		public byte ResExecuteGetRsBdtaFlag()
		{
			return GetByte(43);
		}

		public short ResExecuteGetRsRowIdColIndex()
		{
			return GetShort(44);
		}

		public int ResExecuteGetExecid()
		{
			return GetInt(51);
		}

		public int ResExecuteGetRsCacheOffset()
		{
			return GetInt(55);
		}

		public int ResExecuteGetTransStatus()
		{
			return GetInt(60);
		}

		public void ReqPreexecSetParamNum(short paramNum)
		{
			SetShort(paramNum, 20);
		}

		public void ReqPutdataSetParaIndex(short paraIndex)
		{
			SetShort(paraIndex, 20);
		}

		public void ReqFetchSetCurPos(long curPos)
		{
			SetLong(curPos, 20);
		}

		public void ReqFetchSetRowCount(long rowCount)
		{
			SetLong(rowCount, 28);
		}

		public void ReqFetchSetResId(short resId)
		{
			SetShort(resId, 36);
		}

		public void ReqFetchMaxMsgSize(int fetchPackSize)
		{
			SetInt(fetchPackSize, 38);
		}

		public long ResFetchGetRowCount()
		{
			return GetLong(20);
		}

		public int ResFetchGetRetCount()
		{
			return GetInt(28);
		}

		public void ReqMoreresSetResId(short resId)
		{
			SetShort(resId, 20);
		}

		public void ReqBcpsetSetIdentity(int identity)
		{
			SetInt(identity, 20);
		}

		public void ReqBcpsetSetChkcon(int chkcon)
		{
			SetInt(chkcon, 24);
		}

		public void ReqBcpsetSetGenLog(int genLog)
		{
			SetInt(genLog, 28);
		}

		public void ReqBcpsetSetIsOrder(int isOrder)
		{
			SetInt(isOrder, 32);
		}

		public void ReqBcpclrSetCmt(int withCmt)
		{
			SetInt(withCmt, 20);
		}

		public void ReqSetSessIso(int iso)
		{
			SetInt(iso, 20);
		}

		public void ReqTableTsSetIdNum(short num)
		{
			SetShort(num, 20);
		}

		public short ResTableTsGetIdNum()
		{
			return GetShort(20);
		}
	}
}
