using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Threading;
using Dm.Config;
using Dm.filter;
using Dm.filter.log;
using Dm.filter.reconnect;
using Dm.filter.rw;

namespace Dm
{
	public class DmDataReader : DbDataReader, IFilterInfo
	{
		internal long id = -1L;

		internal static long idGenerator = 0L;

		private static readonly string ClassName = "DmDataReader";

		private DmConnInstance m_Conn;

		private DmResultSetCache m_RsCache;

		internal DmStatement m_Statement;

		private DmInfo m_DbInfo;

		private DmColumn[] m_ColInfo;

		private DmGetValue m_GetVal;

		internal long m_RowCount;

		private CommandBehavior m_Behavior;

		private int m_is_single_row;

		internal long m_StartRow;

		protected long m_CurrentRow = -1L;

		private long m_RecordsAffected;

		protected bool m_IsClosed;

		private bool is_SequentialAccess;

		private int m_SequentialSeq = -1;

		private long m_StreamPos;

		private bool skipCol = true;

		private ArrayList m_Clobs = new ArrayList();

		public long ID
		{
			get
			{
				if (id < 0)
				{
					id = Interlocked.Increment(ref idGenerator);
				}
				return id;
			}
		}

		public FilterChain FilterChain { get; set; }

		public LogInfo LogInfo { get; set; }

		public RWInfo RWInfo { get; set; }

		public RecoverInfo RecoverInfo { get; set; }

		internal long RowCount => m_RowCount;

		internal int do_Depth => 0;

		internal bool do_HasRows
		{
			get
			{
				if (m_RowCount > 0)
				{
					return true;
				}
				return false;
			}
		}

		internal int do_VisibleFieldCount => base.VisibleFieldCount;

		internal int do_RecordsAffected
		{
			get
			{
				if (m_DbInfo != null && m_DbInfo.GetHasResultSet())
				{
					return -1;
				}
				return (int)m_RecordsAffected;
			}
		}

		internal bool do_IsClosed => m_IsClosed;

		internal int do_FieldCount => m_DbInfo.GetColumnCount();

		public override object this[int number]
		{
			get
			{
				if (FilterChain == null)
				{
					return do_this(number);
				}
				return FilterChain.reset().getThis(this, number);
			}
		}

		public override object this[string name]
		{
			get
			{
				if (FilterChain == null)
				{
					return do_this(name);
				}
				return FilterChain.reset().getThis(this, name);
			}
		}

		public override int Depth
		{
			get
			{
				if (FilterChain == null)
				{
					return do_Depth;
				}
				return FilterChain.reset().getDepth(this);
			}
		}

		public override bool HasRows
		{
			get
			{
				if (FilterChain == null)
				{
					return do_HasRows;
				}
				return FilterChain.reset().getHasRows(this);
			}
		}

		public override int VisibleFieldCount
		{
			get
			{
				if (FilterChain == null)
				{
					return do_VisibleFieldCount;
				}
				return FilterChain.reset().getVisibleFieldCount(this);
			}
		}

		public override int RecordsAffected
		{
			get
			{
				if (FilterChain == null)
				{
					return do_RecordsAffected;
				}
				return FilterChain.reset().getRecordsAffected(this);
			}
		}

		public override bool IsClosed
		{
			get
			{
				if (FilterChain == null)
				{
					return do_IsClosed;
				}
				return FilterChain.reset().getIsClosed(this);
			}
		}

		public override int FieldCount
		{
			get
			{
				if (FilterChain == null)
				{
					return do_FieldCount;
				}
				return FilterChain.reset().getFieldCount(this);
			}
		}

		internal DmDataReader(DmResultSetCache cache, DmInfo info, CommandBehavior behavior)
		{
			FilterChain.createFilterChain(this);
			m_RsCache = cache;
			m_DbInfo = info;
			m_ColInfo = info.GetColumnsInfo();
			m_RowCount = m_DbInfo.GetRowCount();
			m_RecordsAffected = m_DbInfo.GetRecordsAffected();
			m_Statement = m_RsCache.Statement;
			m_Conn = m_Statement.ConnInst;
			m_GetVal = new DmGetValue(m_Conn.ConnProperty.ServerEncoding, m_Statement, m_Conn.ConnProperty.NewLobFlag);
			m_Behavior = behavior;
			if ((Convert.ToByte(m_Behavior) & 0x3F) == Convert.ToByte(CommandBehavior.SequentialAccess))
			{
				is_SequentialAccess = true;
			}
		}

		internal DmDataReader(DmInfo info, CommandBehavior behavior, DmStatement stmt)
		{
			FilterChain.createFilterChain(this);
			m_RsCache = null;
			m_DbInfo = info;
			m_ColInfo = info.GetColumnsInfo();
			m_RowCount = m_DbInfo.GetRowCount();
			m_RecordsAffected = m_DbInfo.GetRecordsAffected();
			m_Statement = stmt;
			m_Conn = m_Statement.ConnInst;
			m_GetVal = new DmGetValue(m_Conn.ConnProperty.ServerEncoding, m_Statement, m_Conn.ConnProperty.NewLobFlag);
			m_Behavior = behavior;
			if ((Convert.ToByte(m_Behavior) & 0x3F) == Convert.ToByte(CommandBehavior.SequentialAccess))
			{
				is_SequentialAccess = true;
			}
		}

		internal object do_this(int number)
		{
			return do_GetValue(number);
		}

		internal object do_this(string name)
		{
			int number = do_GetOrdinal(name);
			return do_this(number);
		}

		internal void do_Close()
		{
			if (!m_IsClosed)
			{
				m_IsClosed = true;
				m_DbInfo = null;
				m_ColInfo = null;
				m_CurrentRow = -1L;
				if (m_RsCache != null)
				{
					m_RsCache = null;
				}
				m_Statement.Close();
				m_Statement = null;
				if ((Convert.ToByte(m_Behavior) & 0x3F) == Convert.ToByte(CommandBehavior.CloseConnection))
				{
					m_Conn.Conn.do_Close();
				}
			}
		}

		internal bool do_GetBoolean(int i)
		{
			object obj = do_GetValue(i);
			if (obj is bool)
			{
				return (bool)obj;
			}
			return Convert.ToBoolean(obj);
		}

		internal byte do_GetByte(int i)
		{
			byte[] value = null;
			CheckClose();
			GetByteArrayValue(i, ref value);
			int cType = m_ColInfo[i].GetCType();
			int precision = m_ColInfo[i].GetPrecision();
			int scale = m_ColInfo[i].GetScale();
			return m_GetVal.GetByte(i, value, cType, precision, scale);
		}

		internal long do_GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			CheckClose();
			skipCol = false;
			byte[] value = null;
			byte[] array = null;
			if (m_SequentialSeq == i && fieldOffset < m_StreamPos)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_SEQUENTIALACCESS_ERROR);
			}
			else if (m_SequentialSeq < i)
			{
				m_StreamPos = 0L;
			}
			GetByteArrayValue(i, ref value);
			int cType = m_ColInfo[i].GetCType();
			int precision = m_ColInfo[i].GetPrecision();
			int scale = m_ColInfo[i].GetScale();
			array = m_GetVal.GetBytes(i, value, cType, precision, scale);
			if (buffer == null)
			{
				skipCol = true;
				m_SequentialSeq++;
				return array.Length;
			}
			if (bufferoffset >= buffer.Length || bufferoffset < 0)
			{
				throw new IndexOutOfRangeException("Buffer index must be a valid index in buffer");
			}
			if (buffer.Length < bufferoffset + length)
			{
				throw new ArgumentException("Buffer is not large enough to hold the requested data");
			}
			if (fieldOffset < 0)
			{
				throw new IndexOutOfRangeException("Field offset must be a valid index in the field");
			}
			long num = length;
			if (array.Length - fieldOffset < length)
			{
				num = array.Length - fieldOffset;
			}
			Array.Copy(array, fieldOffset, buffer, bufferoffset, num);
			if (is_SequentialAccess)
			{
				m_StreamPos += num;
				skipCol = true;
			}
			return num;
		}

		internal char do_GetChar(int i)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetChar(int i)");
			CheckIndex(i);
			CheckClose();
			char[] array = new char[1];
			do_GetChars(i, 0L, array, 0, 1);
			return array[0];
		}

		internal long do_GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			string text = null;
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetChars(int i,long fieldoffset,char[] buffer,int bufferoffset,int length)");
			CheckClose();
			skipCol = false;
			if (m_SequentialSeq == i && fieldoffset < m_StreamPos)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_SEQUENTIALACCESS_ERROR);
			}
			else if (m_SequentialSeq < i)
			{
				m_StreamPos = 0L;
			}
			if (m_ColInfo[i].GetCType() == 19)
			{
				DmClob dmClob = (DmClob)m_Clobs[i];
				if (dmClob == null)
				{
					byte[] value = null;
					GetByteArrayValue(i, ref value);
					dmClob = new DmClob(m_Statement, value, (short)i, m_Statement.ConnInst.ConnProperty.LobMode == 2);
					m_Clobs[i] = dmClob;
				}
				text = dmClob.GetSubString(fieldoffset + 1, length);
				Array.Copy(text.ToCharArray(), 0, buffer, bufferoffset, length);
				return length;
			}
			byte[] value2 = null;
			GetByteArrayValue(i, ref value2);
			int cType = m_ColInfo[i].GetCType();
			int precision = m_ColInfo[i].GetPrecision();
			int scale = m_ColInfo[i].GetScale();
			if (value2 == null)
			{
				text = "";
			}
			text = m_GetVal.GetString(i, value2, cType, precision, scale);
			if (buffer == null)
			{
				skipCol = true;
				m_SequentialSeq++;
				return text.Length;
			}
			if (bufferoffset >= buffer.Length || bufferoffset < 0)
			{
				throw new IndexOutOfRangeException("Buffer index must be a valid index in buffer");
			}
			if (buffer.Length < bufferoffset + length)
			{
				throw new ArgumentException("Buffer is not large enough to hold the requested data");
			}
			if (fieldoffset < 0)
			{
				throw new IndexOutOfRangeException("Field offset must be a valid index in the field");
			}
			char[] array = text.ToCharArray();
			int num = length;
			if (buffer.Length - bufferoffset < length)
			{
				num = buffer.Length - bufferoffset;
			}
			else if (length > array.Length)
			{
				num = array.Length;
			}
			Array.Copy(array, fieldoffset, buffer, bufferoffset, num);
			if (is_SequentialAccess)
			{
				m_StreamPos += num;
				skipCol = true;
			}
			return num;
		}

		internal string do_GetDataTypeName(int i)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetDataTypeName(int i)");
			CheckClose();
			return m_ColInfo[i].GetTypeName();
		}

		internal DateTime do_GetDateTime(int i)
		{
			byte[] value = null;
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetDateTime(int i)");
			CheckClose();
			GetByteArrayValue(i, ref value);
			int cType = m_ColInfo[i].GetCType();
			int precision = m_ColInfo[i].GetPrecision();
			int scale = m_ColInfo[i].GetScale();
			return m_GetVal.GetTimestamp(i, value, cType, precision, scale);
		}

		internal decimal do_GetDecimal(int i)
		{
			byte[] value = null;
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetDecimal(int i)");
			CheckClose();
			GetByteArrayValue(i, ref value);
			int cType = m_ColInfo[i].GetCType();
			int precision = m_ColInfo[i].GetPrecision();
			int scale = m_ColInfo[i].GetScale();
			return m_GetVal.GetBigDecimal(i, value, cType, precision, scale);
		}

		internal double do_GetDouble(int i)
		{
			byte[] value = null;
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetDouble(int i)");
			CheckClose();
			GetByteArrayValue(i, ref value);
			int cType = m_ColInfo[i].GetCType();
			int precision = m_ColInfo[i].GetPrecision();
			int scale = m_ColInfo[i].GetScale();
			return m_GetVal.GetDouble(i, value, cType, precision, scale);
		}

		internal IEnumerator do_GetEnumerator()
		{
			if (m_Behavior == CommandBehavior.CloseConnection)
			{
				return new DbEnumerator(this, closeReader: true);
			}
			return new DbEnumerator(this, closeReader: false);
		}

		internal Type do_GetFieldType(int i)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetFieldType(int i)");
			CheckIndex(i);
			return DmSqlType.CTypeToSystemType(m_ColInfo[i].GetCType(), m_ColInfo[i].GetPrecision());
		}

		internal float do_GetFloat(int i)
		{
			byte[] value = null;
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetFloat(int i)");
			CheckClose();
			GetByteArrayValue(i, ref value);
			int cType = m_ColInfo[i].GetCType();
			int precision = m_ColInfo[i].GetPrecision();
			int scale = m_ColInfo[i].GetScale();
			return m_GetVal.GetFloat(i, value, cType, precision, scale);
		}

		internal Guid do_GetGuid(int i)
		{
			byte[] value = null;
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetGuid(int i)");
			CheckClose();
			GetByteArrayValue(i, ref value);
			int cType = m_ColInfo[i].GetCType();
			int precision = m_ColInfo[i].GetPrecision();
			int scale = m_ColInfo[i].GetScale();
			if (value == null)
			{
				return new Guid("");
			}
			if (cType == 2 || cType == 0 || cType == 1 || cType == 54)
			{
				return new Guid(m_GetVal.GetString(i, value, cType, precision, scale));
			}
			return new Guid("");
		}

		internal short do_GetInt16(int i)
		{
			byte[] value = null;
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetInt16(int i)");
			CheckClose();
			GetByteArrayValue(i, ref value);
			int cType = m_ColInfo[i].GetCType();
			int precision = m_ColInfo[i].GetPrecision();
			int scale = m_ColInfo[i].GetScale();
			return m_GetVal.GetShort(i, value, cType, precision, scale);
		}

		internal int do_GetInt32(int i)
		{
			byte[] value = null;
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetInt32(int i)");
			CheckClose();
			GetByteArrayValue(i, ref value);
			int cType = m_ColInfo[i].GetCType();
			int precision = m_ColInfo[i].GetPrecision();
			int scale = m_ColInfo[i].GetScale();
			return m_GetVal.GetInt(i, value, cType, precision, scale);
		}

		internal long do_GetInt64(int i)
		{
			byte[] value = null;
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetInt64(int i)");
			CheckClose();
			GetByteArrayValue(i, ref value);
			int cType = m_ColInfo[i].GetCType();
			int precision = m_ColInfo[i].GetPrecision();
			int scale = m_ColInfo[i].GetScale();
			return m_GetVal.GetLong(i, value, cType, precision, scale);
		}

		internal string do_GetName(int i)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetName(int i)");
			string name = m_ColInfo[i].GetName();
			if (m_Conn.ConnProperty.ColumnNameUpperCase)
			{
				return name.ToUpper();
			}
			if (m_Conn.ConnProperty.ColumnNameCase == ColumnNameCase.UPPER)
			{
				return name.ToUpper();
			}
			if (m_Conn.ConnProperty.ColumnNameCase == ColumnNameCase.LOWER)
			{
				return name.ToLower();
			}
			return name;
		}

		internal int do_GetOrdinal(string name)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetOrdinal(string name)");
			CheckClose();
			int i;
			for (i = 0; i < m_DbInfo.GetColumnCount() && !m_ColInfo[i].GetName().Equals(name); i++)
			{
			}
			if (i >= m_DbInfo.GetColumnCount())
			{
				for (int j = 0; j < m_DbInfo.GetColumnCount(); j++)
				{
					if ((m_ColInfo[j].GetTable() + "." + m_ColInfo[j].GetName()).Equals(name))
					{
						i = j;
						break;
					}
				}
			}
			if (i >= m_DbInfo.GetColumnCount())
			{
				for (int k = 0; k < m_DbInfo.GetColumnCount(); k++)
				{
					if ((m_ColInfo[k].GetSchema() + "." + m_ColInfo[k].GetTable() + "." + m_ColInfo[k].GetName()).ToUpper().Equals(name.ToUpper()))
					{
						i = k;
						break;
					}
				}
			}
			if (i >= m_DbInfo.GetColumnCount())
			{
				for (i = 0; i < m_DbInfo.GetColumnCount() && !m_ColInfo[i].GetName().ToUpper().Equals(name.ToUpper()); i++)
				{
				}
			}
			if (i >= m_DbInfo.GetColumnCount())
			{
				for (int l = 0; l < m_DbInfo.GetColumnCount(); l++)
				{
					if ((m_ColInfo[l].GetTable() + "." + m_ColInfo[l].GetName()).ToUpper().Equals(name.ToUpper()))
					{
						i = l;
						break;
					}
				}
			}
			if (i >= m_DbInfo.GetColumnCount())
			{
				for (int m = 0; m < m_DbInfo.GetColumnCount(); m++)
				{
					if ((m_ColInfo[m].GetSchema() + "." + m_ColInfo[m].GetTable() + "." + m_ColInfo[m].GetName()).ToUpper().Equals(name.ToUpper()))
					{
						i = m;
						break;
					}
				}
			}
			if (i >= m_DbInfo.GetColumnCount())
			{
				throw new IndexOutOfRangeException();
			}
			return i;
		}

		internal Type do_GetProviderSpecificFieldType(int ordinal)
		{
			return base.GetProviderSpecificFieldType(ordinal);
		}

		internal object do_GetProviderSpecificValue(int ordinal)
		{
			return base.GetProviderSpecificValue(ordinal);
		}

		internal int do_GetProviderSpecificValues(object[] values)
		{
			return base.GetProviderSpecificValues(values);
		}

		internal DataTable do_GetSchemaTable()
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetSchemaTable()");
			CheckClose();
			if (m_ColInfo == null)
			{
				return null;
			}
			DataTable dataTable = new DataTable("SchemaTable");
			BuildSchemaColumns(dataTable);
			FillSchemaTable(dataTable);
			return dataTable;
		}

		internal string do_GetString(int i)
		{
			byte[] value = null;
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetString(int i)");
			CheckClose();
			GetByteArrayValue(i, ref value);
			int cType = m_ColInfo[i].GetCType();
			int precision = m_ColInfo[i].GetPrecision();
			int scale = m_ColInfo[i].GetScale();
			if (value == null)
			{
				return "";
			}
			return m_GetVal.GetString(i, value, cType, precision, scale);
		}

		internal object do_GetValue(int i)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetValue(int i)");
			CheckIndex(i);
			CheckClose();
			byte[] value = null;
			GetByteArrayValue(i, ref value);
			int cType = m_ColInfo[i].GetCType();
			int precision = m_ColInfo[i].GetPrecision();
			int scale = m_ColInfo[i].GetScale();
			if (cType == 1)
			{
				int num = ((value != null) ? value.Length : 0);
				int precision2 = m_ColInfo[i].GetPrecision();
				if (num < precision2)
				{
					byte[] array = new byte[precision2];
					if (num != 0)
					{
						Array.Copy(value, 0, array, 0, num);
					}
					for (int j = num; j < precision2; j++)
					{
						array[j] = 32;
					}
					value = array;
				}
			}
			return m_GetVal.GetObject(i, value, cType, precision, scale);
		}

		internal int do_GetValues(object[] values)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetValues(object[] values)");
			CheckClose();
			if (values == null)
			{
				return 0;
			}
			int num = m_DbInfo.GetColumnCount();
			if (values.Length < num)
			{
				num = values.Length;
			}
			for (int i = 0; i < num; i++)
			{
				values[i] = do_GetValue(i);
			}
			return num;
		}

		internal bool do_IsDBNull(int i)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "IsDBNull(int i)");
			CheckClose();
			byte[] value = null;
			GetByteArrayValue(i, ref value);
			if (value == null)
			{
				return true;
			}
			return false;
		}

		internal bool do_NextResult()
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "NextResult()");
			if ((Convert.ToByte(m_Behavior) & 0x3F) == Convert.ToByte(CommandBehavior.SingleResult))
			{
				return false;
			}
			if (m_Statement == null)
			{
				return false;
			}
			if (m_Statement.Command == null)
			{
				return false;
			}
			DmStatement statement = m_Statement;
			if (statement.Command.RefCursorStmtArr != null && statement.Command.RefCursorStmtArr.Count > statement.Command.RefCursorStmtArr_cur)
			{
				statement = (DmStatement)statement.Command.RefCursorStmtArr[statement.Command.RefCursorStmtArr_cur];
				statement.Command.IncRefCur();
				m_DbInfo = statement.ResultInfo;
				m_ColInfo = m_DbInfo.GetColumnsInfo();
				m_RsCache = statement.RsCache;
				m_RowCount = m_DbInfo.GetRowCount();
				m_StartRow = 0L;
				m_CurrentRow = -1L;
				m_IsClosed = false;
			}
			else
			{
				m_DbInfo = statement.Csi.GetMoreResult(statement, m_DbInfo, 0);
				m_ColInfo = m_DbInfo.GetColumnsInfo();
				m_RsCache = statement.RsCache;
				m_RowCount = m_DbInfo.GetRowCount();
				m_StartRow = 0L;
				m_CurrentRow = -1L;
				m_IsClosed = false;
			}
			return m_DbInfo.GetHasResultSet();
		}

		internal bool do_Read()
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "Read()");
			CheckClose();
			ClearClobs();
			if ((Convert.ToByte(m_Behavior) & 0x3F) == Convert.ToByte(CommandBehavior.SequentialAccess))
			{
				m_SequentialSeq = -1;
			}
			if ((Convert.ToByte(m_Behavior) & 0x3F) == Convert.ToByte(CommandBehavior.SingleRow) && m_CurrentRow != -1)
			{
				m_is_single_row = 1;
				return false;
			}
			if ((Convert.ToByte(m_Behavior) & 0x3F) == Convert.ToByte(CommandBehavior.SchemaOnly))
			{
				m_RowCount = 0L;
				return false;
			}
			if (!do_HasRows || m_RsCache == null)
			{
				return false;
			}
			bool result = m_RsCache.FetchNext();
			m_CurrentRow = m_RsCache.RowsetPos;
			return result;
		}

		internal void do_Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		internal DmDataReader do_GetDbDataReader(int ordinal)
		{
			return (DmDataReader)base.GetDbDataReader(ordinal);
		}

		public override void Close()
		{
			if (FilterChain == null)
			{
				do_Close();
			}
			else
			{
				FilterChain.reset().Close(this);
			}
		}

		public override bool GetBoolean(int i)
		{
			if (FilterChain == null)
			{
				return do_GetBoolean(i);
			}
			return FilterChain.reset().GetBoolean(this, i);
		}

		public override byte GetByte(int i)
		{
			if (FilterChain == null)
			{
				return do_GetByte(i);
			}
			return FilterChain.reset().GetByte(this, i);
		}

		public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			if (FilterChain == null)
			{
				return do_GetBytes(i, fieldOffset, buffer, bufferoffset, length);
			}
			return FilterChain.reset().GetBytes(this, i, fieldOffset, buffer, bufferoffset, length);
		}

		public override char GetChar(int i)
		{
			if (FilterChain == null)
			{
				return do_GetChar(i);
			}
			return FilterChain.reset().GetChar(this, i);
		}

		public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			if (FilterChain == null)
			{
				return do_GetChars(i, fieldoffset, buffer, bufferoffset, length);
			}
			return FilterChain.reset().GetChars(this, i, fieldoffset, buffer, bufferoffset, length);
		}

		public override string GetDataTypeName(int i)
		{
			if (FilterChain == null)
			{
				return do_GetDataTypeName(i);
			}
			return FilterChain.reset().GetDataTypeName(this, i);
		}

		public override DateTime GetDateTime(int i)
		{
			if (FilterChain == null)
			{
				return do_GetDateTime(i);
			}
			return FilterChain.reset().GetDateTime(this, i);
		}

		public override decimal GetDecimal(int i)
		{
			if (FilterChain == null)
			{
				return do_GetDecimal(i);
			}
			return FilterChain.reset().GetDecimal(this, i);
		}

		public override double GetDouble(int i)
		{
			if (FilterChain == null)
			{
				return do_GetDouble(i);
			}
			return FilterChain.reset().GetDouble(this, i);
		}

		public override IEnumerator GetEnumerator()
		{
			if (FilterChain == null)
			{
				return do_GetEnumerator();
			}
			return FilterChain.reset().GetEnumerator(this);
		}

		public override Type GetFieldType(int i)
		{
			if (FilterChain == null)
			{
				return do_GetFieldType(i);
			}
			return FilterChain.reset().GetFieldType(this, i);
		}

		public override float GetFloat(int i)
		{
			if (FilterChain == null)
			{
				return do_GetFloat(i);
			}
			return FilterChain.reset().GetFloat(this, i);
		}

		public override Guid GetGuid(int i)
		{
			if (FilterChain == null)
			{
				return do_GetGuid(i);
			}
			return FilterChain.reset().GetGuid(this, i);
		}

		public override short GetInt16(int i)
		{
			if (FilterChain == null)
			{
				return do_GetInt16(i);
			}
			return FilterChain.reset().GetInt16(this, i);
		}

		public override int GetInt32(int i)
		{
			if (FilterChain == null)
			{
				return do_GetInt32(i);
			}
			return FilterChain.reset().GetInt32(this, i);
		}

		public override long GetInt64(int i)
		{
			if (FilterChain == null)
			{
				return do_GetInt64(i);
			}
			return FilterChain.reset().GetInt64(this, i);
		}

		public override string GetName(int i)
		{
			if (FilterChain == null)
			{
				return do_GetName(i);
			}
			return FilterChain.reset().GetName(this, i);
		}

		public override int GetOrdinal(string name)
		{
			if (FilterChain == null)
			{
				return do_GetOrdinal(name);
			}
			return FilterChain.reset().GetOrdinal(this, name);
		}

		public override Type GetProviderSpecificFieldType(int ordinal)
		{
			if (FilterChain == null)
			{
				return do_GetProviderSpecificFieldType(ordinal);
			}
			return FilterChain.reset().GetProviderSpecificFieldType(this, ordinal);
		}

		public override object GetProviderSpecificValue(int ordinal)
		{
			if (FilterChain == null)
			{
				return do_GetProviderSpecificValue(ordinal);
			}
			return FilterChain.reset().GetProviderSpecificValue(this, ordinal);
		}

		public override int GetProviderSpecificValues(object[] values)
		{
			if (FilterChain == null)
			{
				return do_GetProviderSpecificValues(values);
			}
			return FilterChain.reset().GetProviderSpecificValues(this, values);
		}

		public override DataTable GetSchemaTable()
		{
			if (FilterChain == null)
			{
				return do_GetSchemaTable();
			}
			return FilterChain.reset().GetSchemaTable(this);
		}

		public override string GetString(int i)
		{
			if (FilterChain == null)
			{
				return do_GetString(i);
			}
			return FilterChain.reset().GetString(this, i);
		}

		public override object GetValue(int i)
		{
			if (FilterChain == null)
			{
				return do_GetValue(i);
			}
			return FilterChain.reset().GetValue(this, i);
		}

		public override int GetValues(object[] values)
		{
			if (FilterChain == null)
			{
				return do_GetValues(values);
			}
			return FilterChain.reset().GetValues(this, values);
		}

		public override bool IsDBNull(int i)
		{
			if (FilterChain == null)
			{
				return do_IsDBNull(i);
			}
			return FilterChain.reset().IsDBNull(this, i);
		}

		public override bool NextResult()
		{
			if (FilterChain == null)
			{
				return do_NextResult();
			}
			return FilterChain.reset().NextResult(this);
		}

		public override bool Read()
		{
			if (FilterChain == null)
			{
				return do_Read();
			}
			return FilterChain.reset().Read(this);
		}

		protected override void Dispose(bool disposing)
		{
			if (FilterChain == null)
			{
				do_Dispose(disposing);
			}
			else
			{
				FilterChain.reset().Dispose(this, disposing);
			}
		}

		protected override DbDataReader GetDbDataReader(int ordinal)
		{
			if (FilterChain == null)
			{
				return do_GetDbDataReader(ordinal);
			}
			return FilterChain.reset().GetDbDataReader(this, ordinal);
		}

		public new void Dispose()
		{
			Close();
		}

		private void BuildSchemaColumns(DataTable schTbl)
		{
			schTbl.Columns.Add("ColumnName", typeof(string));
			schTbl.Columns.Add("ColumnOrdinal", typeof(int));
			schTbl.Columns.Add("ColumnSize", typeof(int));
			schTbl.Columns.Add("NumericPrecision", typeof(int));
			schTbl.Columns.Add("NumericScale", typeof(int));
			schTbl.Columns.Add("IsUnique", typeof(bool));
			schTbl.Columns.Add("IsKey", typeof(bool));
			schTbl.Columns.Add("BaseServerName", typeof(string));
			schTbl.Columns.Add("BaseCatalogName", typeof(string));
			schTbl.Columns.Add("BaseSchemaName", typeof(string));
			schTbl.Columns.Add("BaseTableName", typeof(string));
			schTbl.Columns.Add("BaseColumnName", typeof(string));
			schTbl.Columns.Add("DataType", typeof(Type));
			schTbl.Columns.Add("AllowDBNull", typeof(bool));
			schTbl.Columns.Add("ProviderType", typeof(int));
			schTbl.Columns.Add("IsAliased", typeof(bool));
			schTbl.Columns.Add("IsExpression", typeof(bool));
			schTbl.Columns.Add("IsIdentity", typeof(bool));
			schTbl.Columns.Add("IsAutoIncrement", typeof(bool));
			schTbl.Columns.Add("IsRowVersion", typeof(bool));
			schTbl.Columns.Add("IsHidden", typeof(bool));
			schTbl.Columns.Add("IsLong", typeof(bool));
			schTbl.Columns.Add("IsReadOnly", typeof(bool));
		}

		private void FillSchemaColumn(DataRow row, DmColumn dmCol, int i)
		{
			row["ColumnName"] = dmCol.GetName();
			row["ColumnOrdinal"] = i + 1;
			row["ColumnSize"] = dmCol.GetSize();
			row["NumericPrecision"] = dmCol.GetPrecision();
			row["NumericScale"] = dmCol.GetScale();
			row["IsUnique"] = false;
			row["IsKey"] = false;
			row["BaseServerName"] = "DM";
			row["BaseCatalogName"] = "";
			row["BaseSchemaName"] = dmCol.GetSchema();
			row["BaseTableName"] = dmCol.GetTable();
			row["BaseColumnName"] = dmCol.GetBaseColumn();
			row["DataType"] = DmSqlType.CTypeToSystemType(dmCol.GetCType(), dmCol.GetPrecision());
			row["AllowDBNull"] = dmCol.GetNullable();
			row["ProviderType"] = dmCol.GetCType();
			row["IsAliased"] = false;
			row["IsExpression"] = false;
			row["IsIdentity"] = dmCol.GetIdentity();
			row["IsAutoIncrement"] = dmCol.GetIdentity();
			row["IsRowVersion"] = false;
			row["IsHidden"] = false;
			row["IsLong"] = dmCol.GetIsLob();
			row["IsReadOnly"] = false;
		}

		private void FillSchemaTable(DataTable schTbl)
		{
			bool flag = true;
			string text = null;
			string text2 = null;
			for (int i = 0; i < m_ColInfo.Length; i++)
			{
				if (i == 0)
				{
					text = m_ColInfo[i].GetSchema();
					text2 = m_ColInfo[i].GetTable();
				}
				if (text == null || text2 == null)
				{
					flag = false;
				}
				else if (!text.Equals(m_ColInfo[i].GetSchema()) || !text2.Equals(m_ColInfo[i].GetTable()))
				{
					flag = false;
				}
				DataRow dataRow = schTbl.NewRow();
				FillSchemaColumn(dataRow, m_ColInfo[i], i);
				dataRow["IsReadOnly"] = !m_DbInfo.GetUpdatable();
				schTbl.Rows.Add(dataRow);
			}
			if (!flag)
			{
				return;
			}
			string text3 = null;
			DataRow dataRow2 = null;
			DmDataReader keyCols = GetKeyCols(text, text2);
			if (keyCols == null)
			{
				return;
			}
			while (keyCols.do_Read())
			{
				text3 = keyCols.GetString(0);
				for (int j = 0; j < schTbl.Rows.Count; j++)
				{
					dataRow2 = schTbl.Rows[j];
					if (Convert.ToString(dataRow2["BaseColumnName"])!.Equals(text3))
					{
						dataRow2["IsKey"] = true;
						if (keyCols.m_RowCount == 1)
						{
							dataRow2["IsUnique"] = true;
						}
						break;
					}
				}
			}
			keyCols.do_Close();
			keyCols = GetUniqueCols(text, text2);
			if (keyCols == null)
			{
				return;
			}
			while (keyCols.do_Read())
			{
				text3 = keyCols.GetString(0);
				for (int k = 0; k < schTbl.Rows.Count; k++)
				{
					dataRow2 = schTbl.Rows[k];
					if (Convert.ToString(dataRow2["BaseColumnName"])!.Equals(text3))
					{
						if (keyCols.m_RowCount == 1)
						{
							dataRow2["IsUnique"] = true;
						}
						break;
					}
				}
			}
			keyCols.do_Close();
		}

		public DmDataReader GetKeyCols(string schema, string table)
		{
			if (schema == null || table == null)
			{
				return null;
			}
			string escStringName = DmStringUtil.GetEscStringName(schema);
			string escStringName2 = DmStringUtil.GetEscStringName(table);
			string sql = "SELECT COLS.NAME FROM SYS.SYSINDEXES INDS, (SELECT OBJ.NAME, CON.ID, CON.TYPE$, CON.TABLEID, CON.COLID, CON.INDEXID FROM SYS.SYSCONS AS CON, SYS.SYSOBJECTS AS OBJ WHERE OBJ.SUBTYPE$='CONS' AND OBJ.ID=CON.ID) CONS, SYS.SYSCOLUMNS COLS, (SELECT NAME ,ID FROM SYS.SYSOBJECTS WHERE SUBTYPE$='UTAB' AND NAME = '" + escStringName2 + "' AND SCHID=(SELECT ID FROM SYS.SYSOBJECTS WHERE NAME = '" + escStringName + "' AND TYPE$='SCH')) TAB, (SELECT ID, NAME FROM SYS.SYSOBJECTS WHERE SUBTYPE$='INDEX')OBJ_INDS WHERE CONS.TYPE$='P' AND CONS.INDEXID=INDS.ID AND INDS.ID=OBJ_INDS.ID AND TAB.ID=COLS.ID AND CONS.TABLEID=TAB.ID AND SF_COL_IS_IDX_KEY(INDS.KEYNUM, INDS.KEYINFO,COLS.COLID)=1";
			DmDataReader result = null;
			try
			{
				result = m_Conn.GetStmtFromPool(new DmCommand()).ExecuteQuery(sql, CommandBehavior.Default);
				return result;
			}
			catch (Exception)
			{
				return result;
			}
		}

		public DmDataReader GetUniqueCols(string schema, string table)
		{
			if (schema == null || table == null)
			{
				return null;
			}
			string escStringName = DmStringUtil.GetEscStringName(schema);
			string escStringName2 = DmStringUtil.GetEscStringName(table);
			string sql = "SELECT COLS.NAME FROM SYS.SYSINDEXES INDS, (SELECT OBJ.NAME, CON.ID, CON.TYPE$, CON.TABLEID, CON.COLID, CON.INDEXID FROM SYS.SYSCONS AS CON, SYS.SYSOBJECTS AS OBJ WHERE OBJ.SUBTYPE$='CONS' AND OBJ.ID=CON.ID) CONS, SYS.SYSCOLUMNS COLS, (SELECT NAME ,ID FROM SYS.SYSOBJECTS WHERE SUBTYPE$='UTAB' AND NAME = '" + escStringName2 + "' AND SCHID=(SELECT ID FROM SYS.SYSOBJECTS WHERE NAME = '" + escStringName + "' AND TYPE$='SCH')) TAB, (SELECT ID, NAME FROM SYS.SYSOBJECTS WHERE SUBTYPE$='INDEX')OBJ_INDS WHERE CONS.TYPE$='U' AND CONS.INDEXID=INDS.ID AND INDS.ID=OBJ_INDS.ID AND TAB.ID=COLS.ID AND CONS.TABLEID=TAB.ID AND SF_COL_IS_IDX_KEY(INDS.KEYNUM, INDS.KEYINFO,COLS.COLID)=1";
			DmDataReader result = null;
			try
			{
				result = m_Conn.GetStmtFromPool(new DmCommand()).ExecuteQuery(sql, CommandBehavior.Default);
				return result;
			}
			catch (Exception)
			{
				return result;
			}
		}

		public bool Previous()
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "Previous()");
			CheckClose();
			ClearClobs();
			if (!do_HasRows)
			{
				return false;
			}
			bool result = m_RsCache.FetchPrevious();
			m_CurrentRow = m_RsCache.RowsetPos;
			return result;
		}

		public bool First()
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "First()");
			CheckClose();
			ClearClobs();
			if (!do_HasRows)
			{
				return false;
			}
			bool result = m_RsCache.FetchFirst();
			m_CurrentRow = m_RsCache.RowsetPos;
			return result;
		}

		public bool Last()
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "Last()");
			CheckClose();
			ClearClobs();
			if (!do_HasRows)
			{
				return false;
			}
			bool result = m_RsCache.FetchLast();
			m_CurrentRow = m_RsCache.RowsetPos;
			return result;
		}

		public bool Absolute(long pos)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "Absolute()");
			CheckClose();
			ClearClobs();
			if (!do_HasRows)
			{
				return false;
			}
			bool result = m_RsCache.FetchAbsolute(pos);
			m_CurrentRow = m_RsCache.RowsetPos;
			return result;
		}

		public bool Relative(long pos)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "Relative()");
			CheckClose();
			ClearClobs();
			if (!do_HasRows)
			{
				return false;
			}
			bool result = m_RsCache.FetchRelative(pos);
			m_CurrentRow = m_RsCache.RowsetPos;
			return result;
		}

		public new DbDataReader GetData(int i)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetData(int i)");
			CheckClose();
			DmError.ThrowUnsupportedException();
			return null;
		}

		public DateTimeOffset GetDateTimeOffset(int i)
		{
			byte[] value = null;
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetDateTime(int i)");
			CheckClose();
			GetByteArrayValue(i, ref value);
			int cType = m_ColInfo[i].GetCType();
			int precision = m_ColInfo[i].GetPrecision();
			int scale = m_ColInfo[i].GetScale();
			return m_GetVal.GetTimestampTZ(i, value, cType, precision, scale);
		}

		public DmXDec GetDmDecimal(int i)
		{
			byte[] value = null;
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetDecimal(int i)");
			CheckClose();
			GetByteArrayValue(i, ref value);
			int cType = m_ColInfo[i].GetCType();
			int precision = m_ColInfo[i].GetPrecision();
			int scale = m_ColInfo[i].GetScale();
			return m_GetVal.GetDmDecimal(i, value, cType, precision, scale);
		}

		internal Type GetFieldTypeInner(int i)
		{
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "GetFieldTypeInner(int i)");
			CheckIndex(i);
			return DmSqlType.CTypeToSystemTypeInner(m_ColInfo[i].GetCType());
		}

		public DmBlob GetBlob(short i)
		{
			CheckClose();
			DmBlob dmBlob = new DmBlob(m_Statement, GetByteArrayValue(i), i, m_Statement.ConnInst.ConnProperty.LobMode == 2);
			dmBlob.SetBLobRowid(m_RsCache.GetRecRowid());
			return dmBlob;
		}

		public DmClob GetClob(short i)
		{
			CheckClose();
			DmClob dmClob = new DmClob(m_Statement, GetByteArrayValue(i), i, m_Statement.ConnInst.ConnProperty.LobMode == 2);
			dmClob.SetBLobRowid(m_RsCache.GetRecRowid());
			return dmClob;
		}

		private void GetByteArrayValue(int columnIndex, ref byte[] value)
		{
			if (columnIndex >= m_RsCache.ColNum)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_SEQUENCE_NUMBER);
			}
			if (is_SequentialAccess && m_SequentialSeq > columnIndex)
			{
				skipCol = true;
				DmError.ThrowDmException(DmErrorDefinition.ECNET_SEQUENTIALACCESS_ERROR);
			}
			m_RsCache.GetBytes((short)columnIndex, ref value);
			if (is_SequentialAccess)
			{
				m_SequentialSeq = columnIndex;
				if (skipCol)
				{
					m_SequentialSeq++;
				}
			}
		}

		private byte[] GetByteArrayValue(int columnIndex)
		{
			if ((Convert.ToByte(m_Behavior) & 0x3F) == Convert.ToByte(CommandBehavior.SingleRow) && m_is_single_row == 1)
			{
				return null;
			}
			if (is_SequentialAccess && m_SequentialSeq > columnIndex)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_SEQUENTIALACCESS_ERROR);
			}
			byte[] bytes = m_RsCache.GetBytes((short)columnIndex);
			m_SequentialSeq = columnIndex + 1;
			if (m_ColInfo[columnIndex].GetCType() == 1)
			{
				int num = ((bytes != null) ? bytes.Length : 0);
				int precision = m_ColInfo[columnIndex].GetPrecision();
				if (num < precision)
				{
					byte[] array = new byte[precision];
					if (num != 0)
					{
						Array.Copy(bytes, 0, array, 0, num);
					}
					for (int i = num; i < precision; i++)
					{
						array[i] = 32;
					}
					return array;
				}
			}
			return bytes;
		}

		private void CheckIndex(int i)
		{
			if (i < 0 || i > m_DbInfo.GetColumnCount() - 1)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_SEQUENCE_NUMBER);
			}
		}

		private void CheckClose()
		{
			if (m_IsClosed || m_Statement == null || m_Statement.IsClosed())
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_RESULTSET_CLOSED);
			}
		}

		private void ClearClobs()
		{
			m_Clobs.Clear();
			for (int i = 0; i < m_DbInfo.GetColumnCount(); i++)
			{
				m_Clobs.Add(null);
			}
		}
	}
}
