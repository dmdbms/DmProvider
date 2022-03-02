using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using Dm.filter;
using Dm.filter.log;
using Dm.filter.reconnect;
using Dm.filter.rw;

namespace Dm
{
	public class DmParameter : DbParameter, IDbDataParameter, IDataParameter, ICloneable, IFilterInfo
	{
		internal long id = -1L;

		internal static long idGenerator = 0L;

		private static readonly string ClassName = "DmParameter";

		private byte m_Prec;

		private byte m_Scale;

		private int m_Size;

		private string m_Name = string.Empty;

		private DbType m_DbType = DbType.String;

		private DmDbType m_DmSqlType = DmDbType.VarChar;

		private string m_DmSqlTypeName = string.Empty;

		private bool m_IsNullable;

		private string m_SourceCol = string.Empty;

		private string m_pre = string.Empty;

		private object m_value;

		private DataRowVersion m_DataRowVer = DataRowVersion.Current;

		private ParameterDirection m_Direct = ParameterDirection.Input;

		private bool m_SourceColumnNullMapping;

		private DmStatement m_refCursorStmt;

		private int m_EFParaKind;

		internal bool m_SetDbTypeFlag;

		internal bool m_SetSizeFlag;

		internal bool m_SetPrecFlag;

		internal bool m_SetScaleFlag;

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

		internal DbType do_DbType
		{
			get
			{
				return m_DbType;
			}
			set
			{
				m_DbType = value;
				m_DmSqlType = Dm.DmSqlType.DbTypeToDmSqlType(m_DbType);
				m_SetDbTypeFlag = true;
			}
		}

		internal ParameterDirection do_Direction
		{
			get
			{
				DmTrace.TracePropertyGet(TraceLevel.Debug, ClassName, "Direction");
				return m_Direct;
			}
			set
			{
				DmTrace.TracePropertySet(TraceLevel.Debug, ClassName, "Direction");
				CheckParameterDirection(value);
				m_Direct = value;
			}
		}

		internal bool do_IsNullable
		{
			get
			{
				DmTrace.TracePropertyGet(TraceLevel.Debug, ClassName, "IsNullable");
				return m_IsNullable;
			}
			set
			{
				DmTrace.TracePropertySet(TraceLevel.Debug, ClassName, "IsNullable");
				m_IsNullable = value;
			}
		}

		internal string do_ParameterName
		{
			get
			{
				DmTrace.TracePropertyGet(TraceLevel.Debug, ClassName, "ParameterName");
				return m_Name;
			}
			set
			{
				DmTrace.TracePropertySet(TraceLevel.Debug, ClassName, "ParameterName");
				string name = m_Name;
				GetParameterName(value);
				parameterCollection?.ChangeName(this, name, value);
				m_Name = value;
			}
		}

		internal int do_Size
		{
			get
			{
				DmTrace.TracePropertyGet(TraceLevel.Debug, ClassName, "Size");
				return m_Size;
			}
			set
			{
				DmTrace.TracePropertySet(TraceLevel.Debug, ClassName, "Size");
				m_Size = value;
				m_SetSizeFlag = true;
			}
		}

		internal string do_SourceColumn
		{
			get
			{
				DmTrace.TracePropertyGet(TraceLevel.Debug, ClassName, "SourceColumn");
				return m_SourceCol;
			}
			set
			{
				DmTrace.TracePropertySet(TraceLevel.Debug, ClassName, "SourceColumn");
				m_SourceCol = value;
			}
		}

		internal bool do_SourceColumnNullMapping
		{
			get
			{
				return m_SourceColumnNullMapping;
			}
			set
			{
				m_SourceColumnNullMapping = value;
			}
		}

		internal DataRowVersion do_SourceVersion
		{
			get
			{
				DmTrace.TracePropertyGet(TraceLevel.Debug, ClassName, "SourceVersion");
				return m_DataRowVer;
			}
			set
			{
				DmTrace.TracePropertySet(TraceLevel.Debug, ClassName, "SourceVersion");
				CheckSourceVersion(value);
				m_DataRowVer = value;
			}
		}

		internal object do_Value
		{
			get
			{
				DmTrace.TracePropertyGet(TraceLevel.Debug, ClassName, "Value");
				return m_value;
			}
			set
			{
				DmTrace.TracePropertySet(TraceLevel.Debug, ClassName, "Value");
				m_value = value;
				byte[] array = value as byte[];
				string text = value as string;
				if (array != null)
				{
					m_Size = array.Length;
					m_SetSizeFlag = true;
				}
				else if (text != null)
				{
					m_Size = text.Length;
					m_SetSizeFlag = true;
				}
				if (!m_SetDbTypeFlag && value != null)
				{
					SetDbTypeFromValue(value);
				}
			}
		}

		internal byte do_Precision
		{
			get
			{
				return m_Prec;
			}
			set
			{
				m_Prec = value;
				m_SetPrecFlag = true;
			}
		}

		internal byte do_Scale
		{
			get
			{
				return m_Scale;
			}
			set
			{
				m_Scale = value;
				m_SetScaleFlag = true;
			}
		}

		public override DbType DbType
		{
			get
			{
				if (FilterChain == null)
				{
					return do_DbType;
				}
				return FilterChain.reset().getDbType(this);
			}
			set
			{
				if (FilterChain == null)
				{
					do_DbType = value;
				}
				else
				{
					FilterChain.reset().setDbType(this, value);
				}
			}
		}

		public override ParameterDirection Direction
		{
			get
			{
				if (FilterChain == null)
				{
					return do_Direction;
				}
				return FilterChain.reset().getDirection(this);
			}
			set
			{
				if (FilterChain == null)
				{
					do_Direction = value;
				}
				else
				{
					FilterChain.reset().setDirection(this, value);
				}
			}
		}

		public override bool IsNullable
		{
			get
			{
				if (FilterChain == null)
				{
					return do_IsNullable;
				}
				return FilterChain.reset().getIsNullable(this);
			}
			set
			{
				if (FilterChain == null)
				{
					do_IsNullable = value;
				}
				else
				{
					FilterChain.reset().setIsNullable(this, value);
				}
			}
		}

		public override string ParameterName
		{
			get
			{
				if (FilterChain == null)
				{
					return do_ParameterName;
				}
				return FilterChain.reset().getParameterName(this);
			}
			set
			{
				if (FilterChain == null)
				{
					do_ParameterName = value;
				}
				else
				{
					FilterChain.reset().setParameterName(this, value);
				}
			}
		}

		public override int Size
		{
			get
			{
				if (FilterChain == null)
				{
					return do_Size;
				}
				return FilterChain.reset().getSize(this);
			}
			set
			{
				if (FilterChain == null)
				{
					do_Size = value;
				}
				else
				{
					FilterChain.reset().setSize(this, value);
				}
			}
		}

		public override string SourceColumn
		{
			get
			{
				if (FilterChain == null)
				{
					return do_SourceColumn;
				}
				return FilterChain.reset().getSourceColumn(this);
			}
			set
			{
				if (FilterChain == null)
				{
					do_SourceColumn = value;
				}
				else
				{
					FilterChain.reset().setSourceColumn(this, value);
				}
			}
		}

		public override bool SourceColumnNullMapping
		{
			get
			{
				if (FilterChain == null)
				{
					return do_SourceColumnNullMapping;
				}
				return FilterChain.reset().getSourceColumnNullMapping(this);
			}
			set
			{
				if (FilterChain == null)
				{
					do_SourceColumnNullMapping = value;
				}
				else
				{
					FilterChain.reset().setSourceColumnNullMapping(this, value);
				}
			}
		}

		public override DataRowVersion SourceVersion
		{
			get
			{
				if (FilterChain == null)
				{
					return do_SourceVersion;
				}
				return FilterChain.reset().getSourceVersion(this);
			}
			set
			{
				if (FilterChain == null)
				{
					do_SourceVersion = value;
				}
				else
				{
					FilterChain.reset().setSourceVersion(this, value);
				}
			}
		}

		public override object Value
		{
			get
			{
				if (FilterChain == null)
				{
					return do_Value;
				}
				return FilterChain.reset().getValue(this);
			}
			set
			{
				if (FilterChain == null)
				{
					do_Value = value;
				}
				else
				{
					FilterChain.reset().setValue(this, value);
				}
			}
		}

		public override byte Precision
		{
			get
			{
				if (FilterChain == null)
				{
					return do_Precision;
				}
				return FilterChain.reset().getPrecision(this);
			}
			set
			{
				if (FilterChain == null)
				{
					do_Precision = value;
				}
				else
				{
					FilterChain.reset().setPrecision(this, value);
				}
			}
		}

		public override byte Scale
		{
			get
			{
				if (FilterChain == null)
				{
					return do_Scale;
				}
				return FilterChain.reset().getScale(this);
			}
			set
			{
				if (FilterChain == null)
				{
					do_Scale = value;
				}
				else
				{
					FilterChain.reset().setScale(this, value);
				}
			}
		}

		internal DmParameterCollection parameterCollection { get; set; }

		internal string Pre
		{
			get
			{
				return m_pre;
			}
			set
			{
				m_pre = value;
			}
		}

		internal DmStatement refCursorStmt
		{
			get
			{
				return m_refCursorStmt;
			}
			set
			{
				m_refCursorStmt = value;
			}
		}

		public int EFParaKind
		{
			get
			{
				return m_EFParaKind;
			}
			set
			{
				m_EFParaKind = value;
			}
		}

		public DmDbType DmSqlType
		{
			get
			{
				DmTrace.TracePropertyGet(TraceLevel.Debug, ClassName, "DmSqlType");
				return m_DmSqlType;
			}
			set
			{
				DmTrace.TracePropertySet(TraceLevel.Debug, ClassName, "DmSqlType");
				CheckParameterDbType(value);
				m_DmSqlType = value;
				m_DbType = Dm.DmSqlType.DmSqlTypeToDbType(m_DmSqlType);
				m_SetDbTypeFlag = true;
			}
		}

		public string DmSqlTypeName
		{
			get
			{
				return m_DmSqlTypeName;
			}
			set
			{
				m_DmSqlTypeName = value;
			}
		}

		public DmParameter()
		{
			FilterChain.createFilterChain(this);
			Pre = string.Empty;
			m_SetDbTypeFlag = false;
			m_SetSizeFlag = false;
			m_SetPrecFlag = false;
			m_SetScaleFlag = false;
		}

		public DmParameter(string parameterName, DmDbType parameterType)
			: this(parameterName, parameterType, 0, string.Empty)
		{
			m_SetDbTypeFlag = true;
			m_SetSizeFlag = false;
			m_SetPrecFlag = false;
			m_SetScaleFlag = false;
		}

		public DmParameter(string parameterName, DmDbType parameterType, int size)
			: this(parameterName, parameterType, size, string.Empty)
		{
			m_SetDbTypeFlag = true;
			m_SetSizeFlag = true;
			m_SetPrecFlag = false;
			m_SetScaleFlag = false;
		}

		public DmParameter(string parameterName, DmDbType parameterType, int size, string sourceColumn)
		{
			FilterChain.createFilterChain(this);
			CheckParameterDbType(parameterType);
			do_ParameterName = parameterName;
			DmSqlType = parameterType;
			do_Size = size;
			do_SourceColumn = sourceColumn;
			m_SetDbTypeFlag = true;
			m_SetSizeFlag = true;
			m_SetPrecFlag = false;
			m_SetScaleFlag = false;
		}

		public DmParameter(string parameterName, DmDbType parameterType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
		{
			FilterChain.createFilterChain(this);
			CheckSourceVersion(sourceVersion);
			CheckParameterDirection(direction);
			CheckParameterDbType(parameterType);
			do_ParameterName = parameterName;
			DmSqlType = parameterType;
			do_Size = size;
			do_SourceColumn = sourceColumn;
			do_Direction = direction;
			do_IsNullable = isNullable;
			do_Precision = precision;
			do_Scale = scale;
			do_SourceVersion = sourceVersion;
			do_Value = value;
			if (m_value == null)
			{
				m_value = DBNull.Value;
			}
			m_SetDbTypeFlag = true;
			m_SetSizeFlag = true;
			m_SetPrecFlag = true;
			m_SetScaleFlag = true;
		}

		public DmParameter(string parameterName, DmDbType parameterType, int size, string sourceColumn, ParameterDirection direction, bool isNullable, byte precision, byte scale, DataRowVersion sourceVersion, object value)
		{
			FilterChain.createFilterChain(this);
			CheckSourceVersion(sourceVersion);
			CheckParameterDirection(direction);
			CheckParameterDbType(parameterType);
			do_ParameterName = parameterName;
			DmSqlType = parameterType;
			do_Size = size;
			do_SourceColumn = sourceColumn;
			do_Direction = direction;
			do_IsNullable = isNullable;
			do_Precision = precision;
			do_Scale = scale;
			do_SourceVersion = sourceVersion;
			do_Value = value;
			if (m_value == null)
			{
				m_value = DBNull.Value;
			}
			m_SetDbTypeFlag = true;
			m_SetSizeFlag = true;
			m_SetPrecFlag = true;
			m_SetScaleFlag = true;
		}

		public DmParameter(string parameterName, object value)
		{
			FilterChain.createFilterChain(this);
			do_ParameterName = parameterName;
			m_value = value;
			if (m_value == null || m_value == DBNull.Value)
			{
				m_value = DBNull.Value;
			}
			m_SetDbTypeFlag = false;
			m_SetSizeFlag = false;
			m_SetPrecFlag = false;
			m_SetScaleFlag = false;
		}

		internal void do_ResetDbType()
		{
			m_DbType = DbType.String;
		}

		public override void ResetDbType()
		{
			if (FilterChain == null)
			{
				do_ResetDbType();
			}
			else
			{
				FilterChain.reset().ResetDbType(this);
			}
		}

		private void SetDbTypeFromValue(object value)
		{
			switch (value.GetType().Name)
			{
			case "SByte":
				do_DbType = DbType.SByte;
				break;
			case "Byte":
				do_DbType = DbType.Byte;
				break;
			case "Int16":
				do_DbType = DbType.Int16;
				break;
			case "UInt16":
				do_DbType = DbType.UInt16;
				break;
			case "Int32":
				do_DbType = DbType.Int32;
				break;
			case "UInt32":
				do_DbType = DbType.UInt32;
				break;
			case "Int64":
				do_DbType = DbType.Int64;
				break;
			case "UInt64":
				do_DbType = DbType.UInt64;
				break;
			case "DateTime":
				do_DbType = DbType.DateTime;
				break;
			case "String":
				do_DbType = DbType.String;
				break;
			case "Single":
				do_DbType = DbType.Single;
				break;
			case "Double":
				do_DbType = DbType.Double;
				break;
			case "Decimal":
				do_DbType = DbType.Decimal;
				break;
			case "TimeSpan":
				DmSqlType = DmDbType.IntervalDayToSecond;
				break;
			case "Guid":
				do_DbType = DbType.Guid;
				break;
			case "Boolean":
				do_DbType = DbType.Boolean;
				break;
			case "DateTimeOffset":
				do_DbType = DbType.DateTimeOffset;
				break;
			default:
				do_DbType = DbType.Object;
				break;
			}
		}

		internal string GetParameterName(string name)
		{
			if (!string.IsNullOrEmpty(name) && (name[0] == '@' || name[0] == ':'))
			{
				Pre = ":";
			}
			return Pre + name;
		}

		private void CheckSourceVersion(DataRowVersion datarowversion)
		{
			if (datarowversion != DataRowVersion.Current && datarowversion != DataRowVersion.Default && datarowversion != DataRowVersion.Original && datarowversion != DataRowVersion.Proposed)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_ENUM_VALUE);
			}
		}

		private void CheckParameterDirection(ParameterDirection direction)
		{
			if (direction != ParameterDirection.Input && direction != ParameterDirection.InputOutput && direction != ParameterDirection.Output && direction != ParameterDirection.ReturnValue)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_ENUM_VALUE);
			}
		}

		private void CheckParameterDbType(DmDbType parameterType)
		{
			if (parameterType < DmDbType.Blob || parameterType > DmDbType.ARRAY)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_PARAMETER_DmDbTYPE);
			}
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		public DmParameter Clone()
		{
			return new DmParameter
			{
				m_Prec = m_Prec,
				m_Scale = m_Scale,
				m_Size = m_Size,
				m_DbType = m_DbType,
				m_DmSqlType = m_DmSqlType,
				m_Direct = m_Direct,
				m_IsNullable = m_IsNullable,
				m_Name = m_Name,
				Pre = Pre,
				m_SourceCol = m_SourceCol,
				m_DataRowVer = m_DataRowVer,
				m_value = m_value,
				m_SourceColumnNullMapping = m_SourceColumnNullMapping,
				m_refCursorStmt = m_refCursorStmt
			};
		}
	}
}
