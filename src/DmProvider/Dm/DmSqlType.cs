using System;
using System.Data;

namespace Dm
{
	internal class DmSqlType
	{
		internal const string VERSION = "7.0.0.0";

		internal const string BUILD_TIME = "2011.01.10";

		internal const string DATABASE_PRODUCT_VERSION = "7.0.0.0";

		internal const int DATA_CHAR = 0;

		internal const int DATA_VARCHAR2 = 1;

		internal const int DATA_VARCHAR = 2;

		internal const int DATA_BIT = 3;

		internal const int DATA_TINYINT = 5;

		internal const int DATA_SMALLINT = 6;

		internal const int DATA_INT = 7;

		internal const int DATA_INT64 = 8;

		internal const int DATA_DEC = 9;

		internal const int DATA_FLOAT = 10;

		internal const int DATA_DOUBLE = 11;

		internal const int DATA_BLOB = 12;

		internal const int DATA_BOOLEAN = 13;

		internal const int DATA_DATE = 14;

		internal const int DATA_TIME = 15;

		internal const int DATA_DATETIME = 16;

		internal const int DATA_BINARY = 17;

		internal const int DATA_VARBINARY = 18;

		internal const int DATA_TEXT = 19;

		internal const int DATA_INTERVAL_YM = 20;

		internal const int DATA_INTERVAL_DT = 21;

		internal const int DATA_TIME_TZ = 22;

		internal const int DATA_DATETIME_TZ = 23;

		internal const int DATA_DEC_INT64 = 24;

		internal const int DATA_NULL = 25;

		internal const int DATA_ANY = 31;

		internal const int DATA_STAR_ALL = 32;

		internal const int DATA_STAR = 33;

		internal const int DATA_RECORD = 40;

		internal const int DATA_TYPE = 41;

		internal const int DATA_TYPE_REF = 42;

		internal const int DATA_UNKNOWN = 54;

		internal const int INTERVAL_YEAR = 0;

		internal const int INTERVAL_YEAR_TO_MONTH = 1;

		internal const int INTERVAL_MONTH = 2;

		internal const int INTERVAL_DAY = 3;

		internal const int INTERVAL_DAY_TO_HOUR = 4;

		internal const int INTERVAL_DAY_TO_MIMUTE = 5;

		internal const int INTERVAL_DAY_TO_SECOND = 6;

		internal const int INTERVAL_HOUR = 7;

		internal const int INTERVAL_HOUR_TO_MIMUTE = 8;

		internal const int INTERVAL_HOUR_TO_SECOND = 9;

		internal const int INTERVAL_MIMUTE = 10;

		internal const int INTERVAL_MIMUTE_TO_SECOND = 11;

		internal const int INTERVAL_SECOND = 12;

		internal const int ARRAY = 117;

		internal const int CLASS = 119;

		internal const int PCURSOR = 120;

		internal const int PLTYPE_RECORD = 121;

		internal const int SARRAY = 122;

		internal const int CURSOR_ORACLE = -10;

		internal const int MAX_STRING_LEN = 8188;

		internal const int LOCAL_TIME_ZONE_SCALE_MASK = 4096;

		internal const int BFILE_PREC = 512;

		internal const int BFILE_SCALE = 6;

		internal const int COMPLEX_SCALE = 5;

		internal const int CURRENCY_PREC = 19;

		internal const int CURRENCY_SCALE = 4;

		internal const int FLOAT_SCALE_MASK = 129;

		internal const int DATA_BIT_LEN = 1;

		internal const int DATA_TINYINT_LEN = 1;

		internal const int DATA_SMALLINT_LEN = 2;

		internal const int DATA_INT_LEN = 4;

		internal const int DATA_INT64_LEN = 8;

		internal const int DATA_FLOAT_LEN = 4;

		internal const int DATA_DOUBLE_LEN = 8;

		internal const int DATA_DATE_LEN = 3;

		internal const int DATA_TIME_LEN = 5;

		internal const int DATA_DATETIME_LEN = 8;

		internal const int DATA_INTERVAL_YM_LEN = 12;

		internal const int DATA_INTERVAL_DT_LEN = 24;

		internal const int DATA_DEC_INT64_LEN = 8;

		internal const int DATA_NULL_LNE = 4;

		internal static bool IsComplexType(int type, int scale)
		{
			if ((type != 12 || scale != 5) && type != 117 && type != 122 && type != 119)
			{
				return type == 121;
			}
			return true;
		}

		internal static bool isFloat(int type, int scale)
		{
			if (type == 9)
			{
				return scale == 129;
			}
			return false;
		}

		public static int getFloatPrec(int type, int prec)
		{
			return (int)Math.Round((double)prec * 0.30103) + 1;
		}

		public static int getFloatScale(int type, int scale)
		{
			return scale & -130;
		}

		internal DmSqlType()
		{
		}

		internal static int TypeNameToCType(string typeName)
		{
			int result = 0;
			typeName = typeName.ToUpper();
			if (typeName.Equals("CHAR"))
			{
				result = 0;
			}
			else if (typeName.Equals("CHARACTER"))
			{
				result = 0;
			}
			else if (typeName.Equals("VARCHAR"))
			{
				result = 2;
			}
			else if (typeName.Equals("VARCHAR2"))
			{
				result = 2;
			}
			else if (typeName.Equals("NUMERIC"))
			{
				result = 9;
			}
			else if (typeName.Equals("DECIMAL"))
			{
				result = 9;
			}
			else if (typeName.Equals("DEC"))
			{
				result = 9;
			}
			else if (typeName.Equals("NUMBER"))
			{
				result = 9;
			}
			else if (typeName.Equals("BYTE"))
			{
				result = 5;
			}
			else if (typeName.Equals("TINYINT"))
			{
				result = 5;
			}
			else if (typeName.Equals("SMALLINT"))
			{
				result = 6;
			}
			else if (typeName.Equals("BIGINT"))
			{
				result = 8;
			}
			else if (typeName.Equals("INT"))
			{
				result = 7;
			}
			else if (typeName.Equals("INTEGER"))
			{
				result = 7;
			}
			else if (typeName.Equals("FLOAT"))
			{
				result = 11;
			}
			else if (typeName.Equals("REAL"))
			{
				result = 10;
			}
			else if (typeName.Equals("DOUBLE"))
			{
				result = 11;
			}
			else if (typeName.Equals("DOUBLE PRECISION"))
			{
				result = 11;
			}
			else if (typeName.Equals("DATE"))
			{
				result = 14;
			}
			else if (typeName.Equals("TIME"))
			{
				result = 15;
			}
			else if (typeName.Equals("TIME WITH TIME ZONE"))
			{
				result = 22;
			}
			else if (typeName.Equals("DATETIME"))
			{
				result = 16;
			}
			else if (typeName.Equals("TIMESTAMP"))
			{
				result = 16;
			}
			else if (typeName.Equals("TIMESTAMP WITH TIME ZONE"))
			{
				result = 23;
			}
			else if (typeName.Equals("TEXT"))
			{
				result = 19;
			}
			else if (typeName.Equals("CLOB"))
			{
				result = 19;
			}
			else if (typeName.Equals("SOUND"))
			{
				result = 12;
			}
			else if (typeName.Equals("IMAGE"))
			{
				result = 12;
			}
			else if (typeName.Equals("BINARY"))
			{
				result = 17;
			}
			else if (typeName.Equals("VARBINARY"))
			{
				result = 18;
			}
			else if (typeName.Equals("BLOB"))
			{
				result = 12;
			}
			else if (typeName.Equals("BIT"))
			{
				result = 3;
			}
			else if (typeName.Equals("MONEY"))
			{
				result = 9;
			}
			else if (typeName.Equals("BOOL"))
			{
				result = 3;
			}
			else if (typeName.Equals("BOOLEAN"))
			{
				result = 3;
			}
			else if (typeName.Equals("LONGVARCHAR"))
			{
				result = 19;
			}
			else if (typeName.Equals("LONGVARBINARY"))
			{
				result = 12;
			}
			else if (typeName.Equals("CURSOR"))
			{
				result = 120;
			}
			else
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_DATA_CONVERTION_ERROR);
			}
			return result;
		}

		internal static Type CTypeToSystemType(int cType, int prec)
		{
			switch (cType)
			{
			case 0:
			case 1:
			case 2:
			case 54:
				return typeof(string);
			case 3:
				return typeof(bool);
			case 5:
				return typeof(sbyte);
			case 6:
				return typeof(short);
			case 7:
				return typeof(int);
			case 8:
				return typeof(long);
			case 9:
			case 24:
				return typeof(decimal);
			case 10:
				return typeof(float);
			case 11:
				return typeof(double);
			case 12:
				return typeof(byte[]);
			case 14:
				return typeof(DateTime);
			case 15:
				return typeof(DateTime);
			case 16:
				return typeof(DateTime);
			case 17:
				return typeof(byte[]);
			case 18:
				return typeof(byte[]);
			case 19:
				return typeof(string);
			case 20:
			case 21:
				return typeof(object);
			default:
				return typeof(object);
			}
		}

		internal static Type CTypeToSystemTypeInner(int cType)
		{
			switch (cType)
			{
			case 0:
				return typeof(string);
			case 1:
				return typeof(string);
			case 2:
				return typeof(string);
			case 3:
				return typeof(bool);
			case 5:
				return typeof(sbyte);
			case 6:
				return typeof(short);
			case 7:
				return typeof(int);
			case 8:
				return typeof(long);
			case 9:
			case 24:
				return typeof(decimal);
			case 10:
				return typeof(float);
			case 11:
				return typeof(double);
			case 12:
				return typeof(byte[]);
			case 14:
				return typeof(DateTime);
			case 15:
				return typeof(DateTime);
			case 22:
				return typeof(DateTimeOffset);
			case 16:
				return typeof(DateTime);
			case 23:
				return typeof(DateTimeOffset);
			case 17:
				return typeof(byte[]);
			case 18:
				return typeof(byte[]);
			case 19:
				return typeof(string);
			case 21:
				return typeof(DmIntervalDT);
			case 20:
				return typeof(DmIntervalYM);
			case 54:
				return typeof(string);
			default:
				return typeof(object);
			}
		}

		internal static DbType DmSqlTypeToDbType(DmDbType sqlType)
		{
			switch (sqlType)
			{
			case DmDbType.Binary:
				return DbType.Binary;
			case DmDbType.Bit:
				return DbType.Boolean;
			case DmDbType.Blob:
				return DbType.Binary;
			case DmDbType.Byte:
				return DbType.Byte;
			case DmDbType.Char:
				return DbType.StringFixedLength;
			case DmDbType.Clob:
				return DbType.String;
			case DmDbType.Date:
				return DbType.Date;
			case DmDbType.DateTime:
				return DbType.DateTime;
			case DmDbType.DateTimeOffset:
				return DbType.DateTimeOffset;
			case DmDbType.TimeOffset:
				return DbType.DateTimeOffset;
			case DmDbType.Decimal:
				return DbType.Decimal;
			case DmDbType.Double:
				return DbType.Double;
			case DmDbType.Float:
				return DbType.Single;
			case DmDbType.Int16:
				return DbType.Int16;
			case DmDbType.Int32:
				return DbType.Int32;
			case DmDbType.Int64:
				return DbType.Int64;
			case DmDbType.IntervalDayToSecond:
			case DmDbType.IntervalYearToMonth:
				return DbType.Object;
			case DmDbType.SByte:
				return DbType.SByte;
			case DmDbType.Text:
				return DbType.String;
			case DmDbType.Time:
				return DbType.Time;
			case DmDbType.UInt16:
				return DbType.UInt16;
			case DmDbType.UInt32:
				return DbType.UInt32;
			case DmDbType.UInt64:
				return DbType.UInt64;
			case DmDbType.VarBinary:
				return DbType.Binary;
			case DmDbType.VarChar:
				return DbType.String;
			case DmDbType.Cursor:
				return DbType.Object;
			case DmDbType.XDEC:
				return DbType.VarNumeric;
			case DmDbType.ARRAY:
				return DbType.Object;
			default:
				return DbType.Object;
			}
		}

		internal static DmDbType DbTypeToDmSqlType(DbType dbType)
		{
			switch (dbType)
			{
			case DbType.AnsiString:
				return DmDbType.VarChar;
			case DbType.AnsiStringFixedLength:
				return DmDbType.Char;
			case DbType.Binary:
				return DmDbType.Binary;
			case DbType.Boolean:
				return DmDbType.Bit;
			case DbType.Byte:
				return DmDbType.Byte;
			case DbType.Currency:
				return DmDbType.Decimal;
			case DbType.Date:
				return DmDbType.Date;
			case DbType.DateTime:
			case DbType.DateTime2:
				return DmDbType.DateTime;
			case DbType.DateTimeOffset:
				return DmDbType.DateTimeOffset;
			case DbType.Decimal:
				return DmDbType.Decimal;
			case DbType.Double:
				return DmDbType.Double;
			case DbType.Guid:
				return DmDbType.VarChar;
			case DbType.Int16:
				return DmDbType.Int16;
			case DbType.Int32:
				return DmDbType.Int32;
			case DbType.Int64:
				return DmDbType.Int64;
			case DbType.Object:
				return DmDbType.Blob;
			case DbType.SByte:
				return DmDbType.SByte;
			case DbType.Single:
				return DmDbType.Float;
			case DbType.String:
				return DmDbType.VarChar;
			case DbType.StringFixedLength:
				return DmDbType.Char;
			case DbType.Time:
				return DmDbType.Time;
			case DbType.UInt16:
				return DmDbType.UInt16;
			case DbType.UInt32:
				return DmDbType.UInt32;
			case DbType.UInt64:
				return DmDbType.UInt64;
			case DbType.VarNumeric:
				return DmDbType.XDEC;
			default:
				return DmDbType.VarChar;
			}
		}

		internal static Type DbTypeToType(DbType dbType)
		{
			switch (dbType)
			{
			case DbType.AnsiString:
				return typeof(string);
			case DbType.AnsiStringFixedLength:
				return typeof(string);
			case DbType.Binary:
				return typeof(sbyte[]);
			case DbType.Boolean:
				return typeof(bool);
			case DbType.Byte:
				return typeof(byte);
			case DbType.Currency:
				return typeof(decimal);
			case DbType.Date:
				return typeof(DateTime);
			case DbType.DateTime:
			case DbType.DateTime2:
				return typeof(DateTime);
			case DbType.DateTimeOffset:
				return typeof(DateTimeOffset);
			case DbType.Decimal:
				return typeof(decimal);
			case DbType.Double:
				return typeof(double);
			case DbType.Guid:
				return typeof(Guid);
			case DbType.Int16:
				return typeof(short);
			case DbType.Int32:
				return typeof(int);
			case DbType.Int64:
				return typeof(long);
			case DbType.Object:
				return typeof(object);
			case DbType.SByte:
				return typeof(sbyte);
			case DbType.Single:
				return typeof(float);
			case DbType.String:
				return typeof(string);
			case DbType.StringFixedLength:
				return typeof(string);
			case DbType.Time:
				return typeof(TimeSpan);
			case DbType.UInt16:
				return typeof(ushort);
			case DbType.UInt32:
				return typeof(uint);
			case DbType.UInt64:
				return typeof(ulong);
			case DbType.VarNumeric:
				return typeof(DmXDec);
			default:
				return typeof(string);
			}
		}

		internal static DbType DbTypeFromObject(object x)
		{
			DbType dbType = DbType.Object;
			if (x == null)
			{
				return DbType.Object;
			}
			if (x is DBNull)
			{
				return DbType.Object;
			}
			if (x is byte)
			{
				return DbType.Byte;
			}
			if (x is sbyte)
			{
				return DbType.SByte;
			}
			if (x is string)
			{
				return DbType.String;
			}
			if (x is char)
			{
				return DbType.StringFixedLength;
			}
			if (x is char[])
			{
				return DbType.StringFixedLength;
			}
			if (x is decimal)
			{
				return DbType.Decimal;
			}
			if (x is short)
			{
				return DbType.Int16;
			}
			if (x is ushort)
			{
				return DbType.UInt16;
			}
			if (x is int)
			{
				return DbType.Int32;
			}
			if (x is uint)
			{
				return DbType.UInt32;
			}
			if (x is long)
			{
				return DbType.Int64;
			}
			if (x is ulong)
			{
				return DbType.UInt64;
			}
			if (x is float)
			{
				return DbType.Single;
			}
			if (x is double)
			{
				return DbType.Double;
			}
			if (x is sbyte[])
			{
				return DbType.Binary;
			}
			if (x is byte[])
			{
				return DbType.Binary;
			}
			if (x is DateTime)
			{
				return DbType.DateTime;
			}
			if (x is DateTimeOffset)
			{
				return DbType.DateTimeOffset;
			}
			if (x is bool)
			{
				return DbType.Boolean;
			}
			if (x is DmTime)
			{
				return DbType.DateTime;
			}
			if (x is DmIntervalYM)
			{
				return DbType.Int32;
			}
			if (x is DmIntervalDT)
			{
				return DbType.Object;
			}
			if (x is TimeSpan)
			{
				return DbType.Time;
			}
			if (x is Guid)
			{
				return DbType.Guid;
			}
			throw new SystemException("Value is of unknown data type");
		}

		internal static DmDbType CTypeToDmDbType(int cType)
		{
			switch (cType)
			{
			case 0:
				return DmDbType.Char;
			case 1:
				return DmDbType.Char;
			case 2:
				return DmDbType.VarChar;
			case 3:
				return DmDbType.Bit;
			case 5:
				return DmDbType.SByte;
			case 6:
				return DmDbType.Int16;
			case 7:
				return DmDbType.Int32;
			case 8:
				return DmDbType.Int64;
			case 9:
			case 24:
				return DmDbType.Decimal;
			case 10:
				return DmDbType.Float;
			case 11:
				return DmDbType.Double;
			case 12:
				return DmDbType.Blob;
			case 14:
				return DmDbType.Date;
			case 15:
				return DmDbType.Time;
			case 16:
				return DmDbType.DateTime;
			case 17:
				return DmDbType.Binary;
			case 18:
				return DmDbType.VarBinary;
			case 19:
				return DmDbType.Text;
			case 21:
				return DmDbType.IntervalDayToSecond;
			case 20:
				return DmDbType.IntervalYearToMonth;
			case 120:
				return DmDbType.Cursor;
			case 54:
				return DmDbType.VarChar;
			case 117:
				return DmDbType.ARRAY;
			default:
				return DmDbType.VarChar;
			}
		}

		internal static int GetSizeByCType(int cType, int cPrec)
		{
			int num = 0;
			switch (cType)
			{
			case 0:
				if (cPrec != 0)
				{
					return cPrec;
				}
				return 8188;
			case 1:
				if (cPrec != 0)
				{
					return cPrec;
				}
				return 8188;
			case 2:
				if (cPrec != 0)
				{
					return cPrec;
				}
				return 8188;
			case 3:
				return 1;
			case 5:
				return 3;
			case 6:
				return 5;
			case 7:
				return 10;
			case 8:
				return 19;
			case 9:
			case 24:
				return cPrec;
			case 10:
				return 24;
			case 11:
				return 53;
			case 12:
				return cPrec;
			case 14:
				return 10;
			case 15:
				return 15;
			case 16:
				return 26;
			case 17:
				return cPrec;
			case 18:
				return cPrec;
			case 19:
				return cPrec;
			case 20:
			case 21:
				return cPrec;
			case 54:
				return cPrec;
			default:
				return cPrec;
			}
		}

		internal static string IntervalDTtypeToName(int scale)
		{
			string result = "";
			int num = (scale & 0xFF00) >> 8;
			int num2 = (scale & 0xF0) >> 4;
			int num3 = scale & 0xF;
			switch (num)
			{
			case 0:
				result = "INTERVAL YEAR(" + num2 + ")";
				break;
			case 2:
				result = "INTERVAL MONTH(" + num2 + ")";
				break;
			case 3:
				result = "INTERVAL DAY(" + num2 + ")";
				break;
			case 7:
				result = "INTERVAL HOUR(" + num2 + ")";
				break;
			case 10:
				result = "INTERVAL MINUTE(" + num2 + ")";
				break;
			case 12:
				result = "INTERVAL SECOND(" + num2 + "," + num3 + ")";
				break;
			case 1:
				result = "INTERVAL YEAR(" + num2 + ") TO MONTH";
				break;
			case 4:
				result = "INTERVAL DAY(" + num2 + ") TO HOUR";
				break;
			case 5:
				result = "INTERVAL DAY(" + num2 + ") TO MINUTE";
				break;
			case 6:
				result = "INTERVAL DAY(" + num2 + ") TO SECOND(" + num3 + ")";
				break;
			case 8:
				result = "INTERVAL HOUR(" + num2 + ") TO MINUTE";
				break;
			case 9:
				result = "INTERVAL HOUR(" + num2 + ") TO SECOND(" + num3 + ")";
				break;
			case 11:
				result = "INTERVAL MINUTE(" + num2 + ") TO SECOND(" + num3 + ")";
				break;
			}
			return result;
		}

		internal static bool DtypeIsFixedLow(int dtype)
		{
			switch (dtype)
			{
			case 0:
				return true;
			case 1:
			case 2:
				return false;
			case 3:
				return true;
			case 7:
				return true;
			case 8:
				return true;
			case 6:
				return true;
			case 5:
				return true;
			case 9:
			case 24:
				return true;
			case 10:
				return true;
			case 11:
				return true;
			case 12:
				return false;
			case 19:
				return false;
			case 14:
				return true;
			case 15:
				return true;
			case 16:
				return true;
			case 20:
				return true;
			case 21:
				return true;
			case 17:
				return true;
			case 18:
				return false;
			case 25:
				return true;
			default:
				return false;
			}
		}

		internal static int DtypeGetInternalLenLow(int dtype, int len)
		{
			return dtype switch
			{
				7 => 4, 
				0 => len, 
				9 => (2 + len + 1) / 2, 
				11 => 8, 
				6 => 2, 
				8 => 8, 
				5 => 1, 
				10 => 4, 
				14 => 3, 
				15 => 5, 
				16 => 8, 
				17 => len, 
				3 => 1, 
				20 => 12, 
				21 => 24, 
				25 => 4, 
				24 => 8, 
				_ => 0, 
			};
		}
	}
}
