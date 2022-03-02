using System;

namespace Dm
{
	public class DmDataTypeMetaData
	{
		public string GetClassName(int i)
		{
			string text = null;
			switch (i)
			{
			case 3:
				return "System.Boolean";
			case 5:
				return "System.Byte";
			case 6:
				return "System.Int16";
			case 7:
				return "System.Int32";
			case 8:
				return "System.Int64";
			case 9:
			case 24:
				return "System.Decimal";
			case 10:
				return "System.Single";
			case 11:
				return "System.Byte";
			case 0:
			case 1:
			case 2:
				return "System.String";
			case 17:
			case 18:
				return "System.Bytebyte[]";
			case 14:
			case 15:
			case 16:
				return "System.DateTime";
			case 22:
			case 23:
				return "System.DateTimeOffset";
			case 20:
			case 21:
				return "System.Object";
			case 54:
				return "System.String";
			default:
				return "System.Object";
			}
		}

		public string GetTypeName(int i, Type j, int scale)
		{
			string result = null;
			switch (i)
			{
			case 0:
			case 1:
				result = "CHAR";
				break;
			case 2:
				result = "VARCHAR";
				break;
			case 3:
				result = ((!(j == typeof(bool))) ? "BIT" : "BOOLEAN");
				break;
			case 5:
				result = "TINYINT";
				break;
			case 6:
				result = "SMALLINT";
				break;
			case 7:
				result = "INTEGER";
				break;
			case 8:
				result = "INT64";
				break;
			case 9:
			case 24:
				result = ((!(j == typeof(decimal))) ? "NUMERIC" : "DECIMAL");
				break;
			case 10:
				result = "REAL";
				break;
			case 11:
				result = ((!(j == typeof(double))) ? "FLOAT" : "DOUBLE");
				break;
			case 12:
				result = "BLOB";
				break;
			case 14:
				result = "DATE";
				break;
			case 15:
				result = "TIME";
				break;
			case 22:
				result = "TIME WITH TIME ZONE";
				break;
			case 16:
				result = "TIMESTAMP";
				break;
			case 23:
				result = "TIMESTAMP WITH TIME ZONE";
				break;
			case 17:
				result = "BINARY";
				break;
			case 18:
				result = "VARBINARY";
				break;
			case 19:
				result = ((!(j == typeof(string))) ? "LONGVARCHAR" : "CLOB");
				break;
			case 20:
			case 21:
				result = DmSqlType.IntervalDTtypeToName(scale);
				break;
			case 25:
				result = "VARCHAR";
				break;
			}
			return result;
		}

		public bool Signed(int i)
		{
			if ((uint)(i - 5) <= 6u)
			{
				return true;
			}
			return false;
		}
	}
}
