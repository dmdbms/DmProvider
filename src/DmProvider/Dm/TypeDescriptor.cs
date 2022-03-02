using System;
using System.Data;
using Dm.util;

namespace Dm
{
	internal class TypeDescriptor
	{
		internal const int OBJ_BLOB_MAGIC = 78111999;

		internal const int CLTN_TYPE_IND_TABLE = 3;

		internal const int CLTN_TYPE_NST_TABLE = 2;

		internal const int CLTN_TYPE_VARRAY = 1;

		internal DmField column;

		internal SQLName m_sqlName;

		internal int m_objId = -1;

		internal int m_objVersion = -1;

		internal int m_outerId;

		internal int m_outerVer;

		internal int m_subId;

		internal int m_cltnType;

		internal int m_maxCnt;

		internal int m_length;

		internal int m_size;

		internal DmConnection m_conn;

		internal string m_serverEncoding;

		internal TypeDescriptor m_arrObj;

		internal TypeDescriptor[] m_fieldsObj;

		internal byte[] m_descBuf;

		internal TypeDescriptor(string fulName, DmConnection conn)
		{
			m_sqlName = new SQLName(fulName);
			m_conn = conn;
			column = new DmField(conn.GetConnInstance());
		}

		internal TypeDescriptor(DmConnection conn)
		{
			m_sqlName = new SQLName(conn);
			m_conn = conn;
			column = new DmField(conn.GetConnInstance());
		}

		internal void ParseDescByName()
		{
			string sql = "BEGIN :p1 = SF_DESCRIBE_TYPE(:p2); END;";
			DmParameter dmParameter = new DmParameter();
			dmParameter.do_ParameterName = "p1";
			dmParameter.do_Direction = ParameterDirection.Output;
			dmParameter.DmSqlType = DmDbType.Blob;
			DmParameter dmParameter2 = new DmParameter();
			dmParameter2.do_ParameterName = "p2";
			dmParameter2.do_Value = m_sqlName.m_fulName;
			DriverUtil.executeNonQuery(m_conn, sql, new DmParameter[2] { dmParameter, dmParameter2 });
			byte[] buffer = (byte[])dmParameter.do_Value;
			m_serverEncoding = m_conn.ConnProperty.ServerEncoding;
			Unpack(new DmMsg(buffer));
		}

		internal string GetFulName()
		{
			return m_sqlName.GetFulName();
		}

		internal int GetDType()
		{
			return column.GetCType();
		}

		public int GetPrec()
		{
			return column.GetPrecision();
		}

		public int GetScale()
		{
			return column.GetScale();
		}

		public string GetServerEncoding()
		{
			if (m_serverEncoding != null)
			{
				return m_serverEncoding;
			}
			return m_conn.ConnProperty.ServerEncoding;
		}

		public int GetObjId()
		{
			return m_objId;
		}

		public int GetStaticArrayLength()
		{
			return m_length;
		}

		public int GetStructMemSize()
		{
			return m_size;
		}

		public int GetOuterId()
		{
			return m_outerId;
		}

		public int GetCltnType()
		{
			return m_cltnType;
		}

		public int GetMaxCnt()
		{
			return m_maxCnt;
		}

		private static int GetPackSize(TypeDescriptor typeDesc)
		{
			int num = 0;
			switch (typeDesc.column.GetCType())
			{
			case 117:
			case 122:
				return GetPackArraySize(typeDesc);
			case 119:
				return GetPackClassSize(typeDesc);
			case 121:
				return GetPackRecordSize(typeDesc);
			default:
				num += 4;
				num += 4;
				return num + 4;
			}
		}

		private static void Pack(TypeDescriptor typeDesc, DmMsg msg)
		{
			switch (typeDesc.column.GetCType())
			{
			case 117:
			case 122:
				PackArray(typeDesc, msg);
				return;
			case 119:
				PackClass(typeDesc, msg);
				return;
			case 121:
				PackRecord(typeDesc, msg);
				return;
			}
			msg.WriteInt(typeDesc.column.GetCType());
			msg.WriteInt(typeDesc.column.GetPrecision());
			msg.WriteInt(typeDesc.column.GetScale());
		}

		internal static int GetPackArraySize(TypeDescriptor arrDesc)
		{
			int num = 0 + 4;
			string name = arrDesc.m_sqlName.m_name;
			int num2 = num + 2;
			string serverEncoding = arrDesc.GetServerEncoding();
			byte[] bytes = DmConvertion.GetBytes(name, serverEncoding);
			return num2 + bytes.Length + 4 + 4 + 4 + GetPackSize(arrDesc.m_arrObj);
		}

		internal static void PackArray(TypeDescriptor arrDesc, DmMsg msg)
		{
			msg.WriteInt(arrDesc.column.GetCType());
			msg.WriteStringWith2Length(arrDesc.m_sqlName.m_name, arrDesc.GetServerEncoding());
			msg.WriteInt(arrDesc.m_objId);
			msg.WriteInt(arrDesc.m_objVersion);
			msg.WriteInt(arrDesc.m_length);
			Pack(arrDesc.m_arrObj, msg);
		}

		internal static void PackRecord(TypeDescriptor strctDesc, DmMsg msg)
		{
			msg.WriteInt(strctDesc.column.GetCType());
			msg.WriteStringWith2Length(strctDesc.m_sqlName.m_name, strctDesc.GetServerEncoding());
			msg.WriteInt(strctDesc.m_objId);
			msg.WriteInt(strctDesc.m_objVersion);
			msg.WriteShort((short)strctDesc.m_size);
			for (int i = 0; i < strctDesc.m_size; i++)
			{
				Pack(strctDesc.m_fieldsObj[i], msg);
			}
		}

		internal static int GetPackRecordSize(TypeDescriptor strctDesc)
		{
			int num = 0;
			num += 4;
			string name = strctDesc.m_sqlName.m_name;
			num += 2;
			string serverEncoding = strctDesc.GetServerEncoding();
			byte[] bytes = DmConvertion.GetBytes(name, serverEncoding);
			num += bytes.Length;
			num += 4;
			num += 4;
			num += 2;
			for (int i = 0; i < strctDesc.m_size; i++)
			{
				num += GetPackSize(strctDesc.m_fieldsObj[i]);
			}
			return num;
		}

		internal static int GetPackClassSize(TypeDescriptor strctDesc)
		{
			int num = 0;
			num += 4;
			string name = strctDesc.m_sqlName.m_name;
			num += 2;
			string serverEncoding = strctDesc.GetServerEncoding();
			byte[] bytes = DmConvertion.GetBytes(name, serverEncoding);
			num += bytes.Length;
			num += 4;
			num += 4;
			if (strctDesc.m_objId == 4)
			{
				num += 4;
				num += 4;
				num += 2;
			}
			return num;
		}

		internal static void PackClass(TypeDescriptor strctDesc, DmMsg msg)
		{
			msg.WriteInt(strctDesc.column.GetCType());
			msg.WriteStringWith2Length(strctDesc.m_sqlName.m_name, strctDesc.GetServerEncoding());
			msg.WriteInt(strctDesc.m_objId);
			msg.WriteInt(strctDesc.m_objVersion);
			if (strctDesc.m_objId == 4)
			{
				msg.WriteInt(strctDesc.m_outerId);
				msg.WriteInt(strctDesc.m_outerVer);
				msg.WriteShort((short)strctDesc.m_subId);
			}
		}

		internal void Unpack(DmMsg msg)
		{
			column.SetCType(msg.ReadInt());
			switch (column.GetCType())
			{
			case 117:
			case 122:
				UnpackArray(msg);
				break;
			case 119:
				UnpackClass(msg);
				break;
			case 121:
				UnpackRecord(msg);
				break;
			default:
				column.SetPrecision(msg.ReadInt());
				column.SetScale(msg.ReadInt());
				break;
			}
		}

		private void UnpackArray(DmMsg msg)
		{
			m_sqlName.m_name = msg.ReadStringWith2Length(GetServerEncoding());
			m_sqlName.m_schId = msg.ReadInt();
			m_sqlName.m_packId = msg.ReadInt();
			m_objId = msg.ReadInt();
			m_objVersion = msg.ReadInt();
			m_length = msg.ReadInt();
			if (column.GetCType() == 117)
			{
				m_length = 0;
			}
			m_arrObj = new TypeDescriptor(m_conn);
			m_arrObj.Unpack(msg);
		}

		private void UnpackRecord(DmMsg msg)
		{
			m_sqlName.m_name = msg.ReadStringWith2Length(GetServerEncoding());
			m_sqlName.m_schId = msg.ReadInt();
			m_sqlName.m_packId = msg.ReadInt();
			m_objId = msg.ReadInt();
			m_objVersion = msg.ReadInt();
			m_size = msg.ReadShort();
			m_fieldsObj = new TypeDescriptor[m_size];
			for (int i = 0; i < m_size; i++)
			{
				m_fieldsObj[i] = new TypeDescriptor(m_conn);
				m_fieldsObj[i].Unpack(msg);
			}
		}

		private void UnpackClnt_nestTab(DmMsg msg)
		{
			m_maxCnt = msg.ReadInt();
			m_arrObj = new TypeDescriptor(m_conn);
			m_arrObj.Unpack(msg);
		}

		private void UnpackClnt(DmMsg msg)
		{
			m_outerId = msg.ReadInt();
			m_outerVer = msg.ReadInt();
			m_subId = msg.ReadShort();
			m_cltnType = msg.ReadShort();
			switch (m_cltnType)
			{
			case 3:
				throw new SystemException("not supported type");
			case 1:
			case 2:
				UnpackClnt_nestTab(msg);
				break;
			}
		}

		private void UnpackClass(DmMsg msg)
		{
			m_sqlName.m_name = msg.ReadStringWith2Length(GetServerEncoding());
			m_sqlName.m_schId = msg.ReadInt();
			m_sqlName.m_packId = msg.ReadInt();
			m_objId = msg.ReadInt();
			m_objVersion = msg.ReadInt();
			if (m_objId == 4)
			{
				UnpackClnt(msg);
				return;
			}
			m_size = msg.ReadShort();
			m_fieldsObj = new TypeDescriptor[m_size];
			for (int i = 0; i < m_size; i++)
			{
				m_fieldsObj[i] = new TypeDescriptor(m_conn);
				m_fieldsObj[i].Unpack(msg);
			}
		}

		internal int CalcChkDescLen_array(TypeDescriptor desc)
		{
			return 0 + 2 + 4 + CalcChkDescLen(desc.m_arrObj);
		}

		private int CalcChkDescLen_record(TypeDescriptor desc)
		{
			int num = 0;
			num += 2;
			num += 2;
			for (int i = 0; i < desc.m_size; i++)
			{
				num += CalcChkDescLen(desc.m_fieldsObj[i]);
			}
			return num;
		}

		internal int CalcChkDescLen_class_normal(TypeDescriptor desc)
		{
			int num = 0;
			num += 2;
			for (int i = 0; i < desc.m_size; i++)
			{
				num += CalcChkDescLen(desc.m_fieldsObj[i]);
			}
			return num;
		}

		private int CalcChkDescLen_class_cnlt(TypeDescriptor desc)
		{
			int num = 0;
			num += 2;
			num += 4;
			switch (desc.GetCltnType())
			{
			case 3:
				throw new SystemException("not supported type!");
			case 1:
			case 2:
				num += CalcChkDescLen(desc.m_arrObj);
				break;
			}
			return num;
		}

		private int CalcChkDescLen_class(TypeDescriptor desc)
		{
			int num = 0;
			num += 2;
			num++;
			if (desc.m_objId == 4)
			{
				return num + CalcChkDescLen_class_cnlt(desc);
			}
			return num + CalcChkDescLen_class_normal(desc);
		}

		private int CalcChkDescLen_buildin()
		{
			return 0 + 2 + 2 + 2;
		}

		private int CalcChkDescLen(TypeDescriptor desc)
		{
			switch (desc.GetDType())
			{
			case 117:
			case 122:
				return CalcChkDescLen_array(desc);
			case 121:
				return CalcChkDescLen_record(desc);
			case 119:
				return CalcChkDescLen_class(desc);
			default:
				return CalcChkDescLen_buildin();
			}
		}

		private int MakeChkDesc_array(int offset, TypeDescriptor desc)
		{
			DmConvertion.SetShort(m_descBuf, offset, 117);
			offset += 2;
			DmConvertion.SetInt(m_descBuf, offset, desc.m_length);
			offset += 4;
			offset = MakeChkDesc(offset, desc.m_arrObj);
			return offset;
		}

		private int MakeChkDesc_record(int offset, TypeDescriptor desc)
		{
			DmConvertion.SetShort(m_descBuf, offset, 121);
			offset += 2;
			DmConvertion.SetShort(m_descBuf, offset, (short)desc.m_size);
			offset += 2;
			for (int i = 0; i < desc.m_size; i++)
			{
				offset = MakeChkDesc(offset, desc.m_fieldsObj[i]);
			}
			return offset;
		}

		private int MakeChkDesc_buildin(int offset, TypeDescriptor desc)
		{
			short num = (short)desc.GetDType();
			short val = 0;
			short val2 = 0;
			if (num != 12)
			{
				val = (short)desc.GetPrec();
				val2 = (short)desc.GetScale();
			}
			DmConvertion.SetShort(m_descBuf, offset, num);
			offset += 2;
			DmConvertion.SetShort(m_descBuf, offset, val);
			offset += 2;
			DmConvertion.SetShort(m_descBuf, offset, val2);
			offset += 2;
			return offset;
		}

		private int MakeChkDesc_class_normal(int offset, TypeDescriptor desc)
		{
			DmConvertion.SetShort(m_descBuf, offset, (short)desc.m_size);
			offset += 2;
			for (int i = 0; i < desc.m_size; i++)
			{
				offset = MakeChkDesc(offset, desc.m_fieldsObj[i]);
			}
			return offset;
		}

		private int MakeChkDesc_class_clnt(int offset, TypeDescriptor desc)
		{
			DmConvertion.SetShort(m_descBuf, offset, (short)desc.m_cltnType);
			offset += 2;
			DmConvertion.SetInt(m_descBuf, offset, desc.GetMaxCnt());
			offset += 4;
			switch (desc.m_cltnType)
			{
			case 3:
				throw new SystemException("not supported type");
			case 1:
			case 2:
				offset = MakeChkDesc(offset, desc.m_arrObj);
				break;
			}
			return offset;
		}

		private int MakeChkDesc_class(int offset, TypeDescriptor desc)
		{
			DmConvertion.SetShort(m_descBuf, offset, 119);
			offset += 2;
			bool flag = false;
			if (desc.m_objId == 4)
			{
				flag = true;
			}
			if (flag)
			{
				DmConvertion.SetByte(m_descBuf, offset, 1);
			}
			else
			{
				DmConvertion.SetByte(m_descBuf, offset, 0);
			}
			offset++;
			offset = ((!flag) ? MakeChkDesc_class_normal(offset, desc) : MakeChkDesc_class_clnt(offset, desc));
			return offset;
		}

		private int MakeChkDesc(int offset, TypeDescriptor subDesc)
		{
			switch (subDesc.GetDType())
			{
			case 117:
			case 122:
				offset = MakeChkDesc_array(offset, subDesc);
				break;
			case 121:
				offset = MakeChkDesc_record(offset, subDesc);
				break;
			case 119:
				offset = MakeChkDesc_class(offset, subDesc);
				break;
			default:
				offset = MakeChkDesc_buildin(offset, subDesc);
				break;
			}
			return offset;
		}

		internal byte[] GetClassDescChkInfo()
		{
			if (m_descBuf != null)
			{
				return m_descBuf;
			}
			int num = CalcChkDescLen(this);
			m_descBuf = new byte[num];
			MakeChkDesc(0, this);
			return m_descBuf;
		}
	}
}
