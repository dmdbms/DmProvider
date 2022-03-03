using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Text;

namespace Dm
{
	internal class DmSchema
	{
		private DmConnection m_Conn;

		public const string MetaCollection = "METADATACOLLECTIONS";

		public const string DataSourceInfo = "DATASOURCEINFORMATION";

		public const string Databases = "DATABASES";

		public DmSchema(DmConnection conn)
		{
			m_Conn = conn;
		}

		public DataTable GetSchema(string collection, string[] restrictionValues)
		{
			if (m_Conn.do_State != ConnectionState.Open)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_CONNECTION_CLOSED);
			}
			collection = collection.ToUpper(DmConst.invariantCulture);
			string[] restrictions = CleanRestrictions(restrictionValues);
			return GetSchemaInner(collection, restrictions);
		}

		private string dup_chr_QUOTATION_MARK(string srcsource)
		{
			if (srcsource.Equals(string.Empty))
			{
				return srcsource;
			}
			srcsource = srcsource.Replace("'", "''");
			return srcsource;
		}

		public virtual DataTable GetTables(string[] restrictions)
		{
			DataTable dataTable = new DataTable("Tables");
			dataTable.Columns.Add("TABLE_CATALOG", typeof(string));
			dataTable.Columns.Add("TABLE_SCHEMA", typeof(string));
			dataTable.Columns.Add("TABLE_NAME", typeof(string));
			dataTable.Columns.Add("TABLE_TYPE", typeof(string));
			dataTable.Columns.Add("FILLFACTOR", typeof(int));
			dataTable.Columns.Add("SPACE_LIMIT", typeof(int));
			dataTable.Columns.Add("ROW_COUNT", typeof(ulong));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(DmConst.invariantCulture, "SELECT SF_GET_SCHEMA_NAME_BY_ID(SCHID), NAME, CASE SUBTYPE$ WHEN 'UTAB' THEN 'UTAB' WHEN 'VIEW' THEN 'VIEW' WHEN 'STAB' THEN 'STAB' WHEN 'SYNOM' THEN 'SYNONYM' END AS TABLE_TYPE, INFO1, INFO2, INFO3, INFO4 FROM SYS.SYSOBJECTS WHERE SUBTYPE$ IN ('UTAB','STAB','VIEW') ");
			if (restrictions != null && restrictions.Length >= 1 && restrictions[0] != null)
			{
				string value = $" AND SF_GET_SCHEMA_NAME_BY_ID(SCHID) LIKE '{dup_chr_QUOTATION_MARK(restrictions[0])}'";
				stringBuilder.Append(value);
			}
			if (restrictions != null && restrictions.Length >= 2 && restrictions[1] != null)
			{
				string value2 = $" AND NAME LIKE '{dup_chr_QUOTATION_MARK(restrictions[1])}'";
				stringBuilder.Append(value2);
			}
			if (restrictions != null && restrictions.Length >= 3 && restrictions[2] != null)
			{
				string value3 = $" AND SUBTYPE$ = '{dup_chr_QUOTATION_MARK(restrictions[2])}'";
				stringBuilder.Append(value3);
				if (restrictions[2].Equals("UTAB"))
				{
					string value4 = $" AND INFO3&0x100000!=0x100000 AND INFO3&0x200000!=0x200000 AND INFO3 & 0x003F not in (0x0A, 0x20) AND NAME not like 'CTI$%$_'  AND NAME not like '%$AUX' AND PID = -1";
					stringBuilder.Append(value4);
				}
			}
			DmCommand dmCommand = new DmCommand(stringBuilder.ToString(), m_Conn);
			using (DmDataReader dmDataReader = dmCommand.do_ExecuteDbDataReader(CommandBehavior.Default))
			{
				object obj = null;
				while (dmDataReader.do_Read())
				{
					DataRow dataRow = dataTable.NewRow();
					dataRow["TABLE_CATALOG"] = null;
					dataRow["TABLE_SCHEMA"] = dmDataReader.do_GetString(0);
					dataRow["TABLE_NAME"] = dmDataReader.do_GetString(1);
					dataRow["TABLE_TYPE"] = dmDataReader.do_GetString(2);
					obj = dmDataReader.do_GetInt32(3);
					if (obj == DBNull.Value)
					{
						dataRow["FILLFACTOR"] = DBNull.Value;
					}
					else
					{
						dataRow["FILLFACTOR"] = Convert.ToInt32(obj);
					}
					obj = dmDataReader.do_GetValue(4);
					if (obj == DBNull.Value)
					{
						dataRow["SPACE_LIMIT"] = DBNull.Value;
					}
					else
					{
						dataRow["SPACE_LIMIT"] = Convert.ToInt32(obj);
					}
					obj = dmDataReader.do_GetValue(5);
					if (obj == DBNull.Value)
					{
						dataRow["ROW_COUNT"] = DBNull.Value;
					}
					else
					{
						dataRow["ROW_COUNT"] = Convert.ToInt64(obj);
					}
					dataTable.Rows.Add(dataRow);
				}
			}
			dmCommand.Dispose();
			return dataTable;
		}

		public virtual DataTable GetProcedures(string[] restrictions)
		{
			DataTable dataTable = new DataTable("Procedures");
			dataTable.Columns.Add("PROCEDURE_SCHEMA", typeof(string));
			dataTable.Columns.Add("PROCEDURE_NAME", typeof(string));
			dataTable.Columns.Add("PROCEDURE_TYPE", typeof(string));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(DmConst.invariantCulture, "SELECT  SF_GET_SCHEMA_NAME_BY_ID(SCHID), NAME, CASE INFO1 & 0x01 WHEN 1 THEN 'PROCEDURE' ELSE 'FUNCTION' END  FROM SYS.SYSOBJECTS WHERE SUBTYPE$='PROC' ");
			if (restrictions != null && restrictions.Length >= 1 && restrictions[0] != null)
			{
				string value = $" AND SF_GET_SCHEMA_NAME_BY_ID(SCHID) LIKE '{dup_chr_QUOTATION_MARK(restrictions[0])}'";
				stringBuilder.Append(value);
			}
			if (restrictions != null && restrictions.Length >= 2 && restrictions[1] != null)
			{
				string value2 = $" AND NAME LIKE '{dup_chr_QUOTATION_MARK(restrictions[1])}'";
				stringBuilder.Append(value2);
			}
			if (restrictions != null && restrictions.Length >= 3 && restrictions[2] != null)
			{
				if (restrictions[2].EndsWith("PROCEDURE", ignoreCase: true, null))
				{
					string value3 = string.Format(" AND INFO1&0x01 = 1 AND INFO1&0x04 = 0", restrictions[2]);
					stringBuilder.Append(value3);
				}
				else if (restrictions[2].EndsWith("FUNCTION", ignoreCase: true, null))
				{
					string value3 = string.Format(" AND INFO1&0x01 = 0 AND INFO1&0x04 = 0", restrictions[2]);
					stringBuilder.Append(value3);
				}
			}
			DmCommand dmCommand = new DmCommand(stringBuilder.ToString(), m_Conn);
			using (DmDataReader dmDataReader = dmCommand.do_ExecuteDbDataReader(CommandBehavior.Default))
			{
				while (dmDataReader.do_Read())
				{
					DataRow dataRow = dataTable.NewRow();
					dataRow["PROCEDURE_SCHEMA"] = dmDataReader.do_GetString(0);
					dataRow["PROCEDURE_NAME"] = dmDataReader.do_GetString(1);
					dataRow["PROCEDURE_TYPE"] = dmDataReader.do_GetString(2);
					dataTable.Rows.Add(dataRow);
				}
			}
			dmCommand.Dispose();
			return dataTable;
		}

		public virtual DataTable GetParameters(string[] restrictions)
		{
			DataTable dataTable = new DataTable("Parameters");
			dataTable.Columns.Add("PROCEDURE_SCHEMA", typeof(string));
			dataTable.Columns.Add("PROCEDURE_NAME", typeof(string));
			dataTable.Columns.Add("PARAMETER_NAME", typeof(string));
			dataTable.Columns.Add("PARAMETER_ID", typeof(int));
			dataTable.Columns.Add("PARAMETER_TYPE", typeof(string));
			dataTable.Columns.Add("PARAMETER_SIZE", typeof(int));
			dataTable.Columns.Add("PARAMETER_SCALE", typeof(int));
			dataTable.Columns.Add("DEFAULT_VALUE", typeof(string));
			dataTable.Columns.Add("NULLABLE", typeof(string));
			dataTable.Columns.Add("PARAMETER_INOUT", typeof(short));
			string parameterRestriction = null;
			if (restrictions != null && restrictions.Length == 3)
			{
				parameterRestriction = restrictions[2];
				restrictions[2] = null;
			}
			foreach (DataRow row in GetProcedures(restrictions).Rows)
			{
				LoadProcedureParameters(dataTable, row["PROCEDURE_SCHEMA"].ToString(), row["PROCEDURE_NAME"].ToString(), parameterRestriction);
			}
			return dataTable;
		}

		private void LoadProcedureParameters(DataTable dt, string schema, string procedureName, string parameterRestriction)
		{
			string text = $"SELECT NAME, COLID, TYPE$, INFO1, CASE SF_GET_COLUMN_SIZE(TYPE$, CAST (LENGTH$ AS INT), CAST (SCALE AS INT)) WHEN -2 THEN NULL ELSE SF_GET_COLUMN_SIZE(TYPE$, CAST (LENGTH$ AS INT), CAST (SCALE AS INT)) END AS COLUMN_SIZE, SCALE, NULLABLE$, DEFVAL FROM SYS.SYSCOLUMNS WHERE  ID = (SELECT ID FROM SYS.SYSOBJECTS WHERE NAME = '{procedureName}' AND SUBTYPE$ IN ('PROC') AND SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE NAME = '{schema}' AND TYPE$='SCH'))";
			if (parameterRestriction != null)
			{
				string text2 = $" AND NAME LIKE '{parameterRestriction}'";
				text += text2;
			}
			DmCommand dmCommand = new DmCommand(text, m_Conn);
			using (DmDataReader dmDataReader = dmCommand.do_ExecuteDbDataReader(CommandBehavior.Default))
			{
				while (dmDataReader.do_Read())
				{
					DataRow dataRow = dt.NewRow();
					dataRow["PROCEDURE_SCHEMA"] = schema;
					dataRow["PROCEDURE_NAME"] = procedureName;
					dataRow["PARAMETER_NAME"] = dmDataReader.do_GetString(0);
					dataRow["PARAMETER_ID"] = dmDataReader.do_GetInt16(1);
					dataRow["PARAMETER_TYPE"] = dmDataReader.do_GetString(2);
					dataRow["PARAMETER_INOUT"] = dmDataReader.do_GetInt16(3);
					dataRow["PARAMETER_SIZE"] = dmDataReader.do_GetInt32(4);
					dataRow["PARAMETER_SCALE"] = dmDataReader.do_GetInt16(5);
					dataRow["NULLABLE"] = dmDataReader.do_GetString(6);
					dataRow["DEFAULT_VALUE"] = dmDataReader.do_GetString(7);
					dt.Rows.Add(dataRow);
				}
			}
			dmCommand.Dispose();
		}

		protected virtual DataTable GetCollections()
		{
			object[][] data = new object[11][]
			{
				new object[3] { "MetaDataCollections", 0, 0 },
				new object[3] { "DataSourceInformation", 0, 0 },
				new object[3] { "DataTypes", 0, 0 },
				new object[3] { "Restrictions", 0, 0 },
				new object[3] { "ReservedWords", 0, 0 },
				new object[3] { "Tables", 3, 2 },
				new object[3] { "Columns", 3, 3 },
				new object[3] { "Users", 1, 1 },
				new object[3] { "Procedures", 3, 2 },
				new object[3] { "Indexes", 3, 3 },
				new object[3] { "PrimaryKeys", 2, 2 }
			};
			DataTable obj = new DataTable("MetaDataCollections")
			{
				Columns = 
				{
					new DataColumn("CollectionName", typeof(string)),
					new DataColumn("NumberOfRestrictions", typeof(int)),
					new DataColumn("NumberOfIdentifierParts", typeof(int))
				}
			};
			FillTable(obj, data);
			return obj;
		}

		private DataTable GetDataSourceInformation()
		{
			DataTable obj = new DataTable("DataSourceInformation")
			{
				Columns = 
				{
					{
						"CompositeIdentifierSeparatorPattern",
						typeof(string)
					},
					{
						"DataSourceProductName",
						typeof(string)
					},
					{
						"DataSourceProductVersion",
						typeof(string)
					},
					{
						"DataSourceProductVersionNormalized",
						typeof(string)
					},
					{
						"GroupByBehavior",
						typeof(GroupByBehavior)
					},
					{
						"IdentifierPattern",
						typeof(string)
					},
					{
						"IdentifierCase",
						typeof(IdentifierCase)
					},
					{
						"OrderByColumnsInSelect",
						typeof(bool)
					},
					{
						"ParameterMarkerFormat",
						typeof(string)
					},
					{
						"ParameterMarkerPattern",
						typeof(string)
					},
					{
						"ParameterNameMaxLength",
						typeof(int)
					},
					{
						"ParameterNamePattern",
						typeof(string)
					},
					{
						"QuotedIdentifierPattern",
						typeof(string)
					},
					{
						"QuotedIdentifierCase",
						typeof(IdentifierCase)
					},
					{
						"StatementSeparatorPattern",
						typeof(string)
					},
					{
						"StringLiteralPattern",
						typeof(string)
					},
					{
						"SupportedJoinOperators",
						typeof(SupportedJoinOperators)
					}
				}
			};
			DataRow dataRow = obj.NewRow();
			dataRow["CompositeIdentifierSeparatorPattern"] = "\\.";
			dataRow["DataSourceProductName"] = "DM";
			dataRow["DataSourceProductVersion"] = "";
			dataRow["DataSourceProductVersionNormalized"] = "";
			dataRow["GroupByBehavior"] = GroupByBehavior.Unrelated;
			dataRow["IdentifierPattern"] = "(^\\`\\p{Lo}\\p{Lu}\\p{Ll}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Nd}@$#_]*$)|(^\\`[^\\`\\0]|\\`\\`+\\`$)|(^\\\" + [^\\\"\\0]|\\\"\\\"+\\\"$)";
			if (m_Conn.ConnProperty.CaseSensitive)
			{
				dataRow["IdentifierCase"] = IdentifierCase.Sensitive;
			}
			else
			{
				dataRow["IdentifierCase"] = IdentifierCase.Insensitive;
			}
			dataRow["OrderByColumnsInSelect"] = false;
			dataRow["ParameterMarkerFormat"] = "{0}";
			dataRow["ParameterMarkerPattern"] = "(@[A-Za-z0-9_$#]*)";
			dataRow["ParameterNameMaxLength"] = 128;
			dataRow["ParameterNamePattern"] = "^[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\\uff3f_@#\\$]*(?=\\s+|$)";
			dataRow["QuotedIdentifierPattern"] = "(([^\\`]|\\`\\`)*)";
			dataRow["QuotedIdentifierCase"] = IdentifierCase.Sensitive;
			dataRow["StatementSeparatorPattern"] = ";";
			dataRow["StringLiteralPattern"] = "'(([^']|'')*)'";
			dataRow["SupportedJoinOperators"] = 15;
			obj.Rows.Add(dataRow);
			return obj;
		}

		private static void DataTypesGenCols(DataTable dt)
		{
			dt.Columns.Add(new DataColumn("TypeName", typeof(string)));
			dt.Columns.Add(new DataColumn("ProviderDbType", typeof(int)));
			dt.Columns.Add(new DataColumn("ColumnSize", typeof(long)));
			dt.Columns.Add(new DataColumn("CreateFormat", typeof(string)));
			dt.Columns.Add(new DataColumn("CreateParameters", typeof(string)));
			dt.Columns.Add(new DataColumn("DataType", typeof(string)));
			dt.Columns.Add(new DataColumn("IsAutoincrementable", typeof(bool)));
			dt.Columns.Add(new DataColumn("IsBestMatch", typeof(bool)));
			dt.Columns.Add(new DataColumn("IsCaseSensitive", typeof(bool)));
			dt.Columns.Add(new DataColumn("IsFixedLength", typeof(bool)));
			dt.Columns.Add(new DataColumn("IsFixedPrecisionScale", typeof(bool)));
			dt.Columns.Add(new DataColumn("IsLong", typeof(bool)));
			dt.Columns.Add(new DataColumn("IsNullable", typeof(bool)));
			dt.Columns.Add(new DataColumn("IsSearchable", typeof(bool)));
			dt.Columns.Add(new DataColumn("IsSearchableWithLike", typeof(bool)));
			dt.Columns.Add(new DataColumn("IsUnsigned", typeof(bool)));
			dt.Columns.Add(new DataColumn("MaximumScale", typeof(short)));
			dt.Columns.Add(new DataColumn("MinimumScale", typeof(short)));
			dt.Columns.Add(new DataColumn("IsConcurrencyType", typeof(bool)));
			dt.Columns.Add(new DataColumn("IsLiteralsSupported", typeof(bool)));
			dt.Columns.Add(new DataColumn("LiteralPrefix", typeof(string)));
			dt.Columns.Add(new DataColumn("LiteralSuffix", typeof(string)));
			dt.Columns.Add(new DataColumn("NativeDataType", typeof(string)));
		}

		private static void DataTypesAddRow(DataTable dt, string TypeName, int ProviderDbType, long ColumnSize, string CreateFormat, string CreateParameters, string DataType, bool IsAutoincrementable, bool IsBestMatch, bool IsCaseSensitive, bool IsFixedLength, bool IsFixedPrecisionScale, bool IsLong, bool IsNullable, bool IsSearchable, bool IsSearchableWithLike, object IsUnsigned, object MaximumScale, object MinimumScale, bool IsConcurrencyType, object IsLiteralsSupported, string LiteralPrefix, string LiteralSuffix, string NativeDataType)
		{
			DataRow dataRow = dt.NewRow();
			dataRow["TypeName"] = TypeName;
			dataRow["ProviderDbType"] = ProviderDbType;
			dataRow["ColumnSize"] = ColumnSize;
			dataRow["CreateFormat"] = CreateFormat;
			dataRow["CreateParameters"] = CreateParameters;
			dataRow["DataType"] = DataType;
			dataRow["IsAutoincrementable"] = IsAutoincrementable;
			dataRow["IsBestMatch"] = IsBestMatch;
			dataRow["IsCaseSensitive"] = IsCaseSensitive;
			dataRow["IsFixedLength"] = IsFixedLength;
			dataRow["IsFixedPrecisionScale"] = IsFixedPrecisionScale;
			dataRow["IsLong"] = IsLong;
			dataRow["IsNullable"] = IsNullable;
			dataRow["IsSearchable"] = IsSearchable;
			dataRow["IsSearchableWithLike"] = IsSearchableWithLike;
			dataRow["IsUnsigned"] = IsUnsigned;
			dataRow["MaximumScale"] = MaximumScale;
			dataRow["MinimumScale"] = MinimumScale;
			dataRow["IsConcurrencyType"] = IsConcurrencyType;
			dataRow["IsLiteralsSupported"] = IsLiteralsSupported;
			dataRow["LiteralPrefix"] = LiteralPrefix;
			dataRow["LiteralSuffix"] = LiteralSuffix;
			dataRow["NativeDataType"] = NativeDataType;
			dt.Rows.Add(dataRow);
		}

		private static DataTable GetDataTypes()
		{
			DataTable dataTable = new DataTable("DataTypes");
			DataTypesGenCols(dataTable);
			DataTypesAddRow(dataTable, "smallint", 12, 5L, "smallint", null, "System.Int16", IsAutoincrementable: true, IsBestMatch: true, IsCaseSensitive: false, IsFixedLength: true, IsFixedPrecisionScale: true, IsLong: false, IsNullable: true, IsSearchable: true, IsSearchableWithLike: false, false, DBNull.Value, DBNull.Value, IsConcurrencyType: false, DBNull.Value, null, null, "smallint");
			DataTypesAddRow(dataTable, "int", 13, 10L, "int", null, "System.Int32", IsAutoincrementable: true, IsBestMatch: true, IsCaseSensitive: false, IsFixedLength: true, IsFixedPrecisionScale: true, IsLong: false, IsNullable: true, IsSearchable: true, IsSearchableWithLike: false, false, DBNull.Value, DBNull.Value, IsConcurrencyType: false, DBNull.Value, null, null, "int");
			DataTypesAddRow(dataTable, "real", 11, 7L, "real", null, "System.Single", IsAutoincrementable: false, IsBestMatch: true, IsCaseSensitive: false, IsFixedLength: true, IsFixedPrecisionScale: false, IsLong: false, IsNullable: true, IsSearchable: true, IsSearchableWithLike: false, false, DBNull.Value, DBNull.Value, IsConcurrencyType: false, DBNull.Value, null, null, "real");
			DataTypesAddRow(dataTable, "double", 10, 53L, "double", null, "System.Double", IsAutoincrementable: false, IsBestMatch: true, IsCaseSensitive: false, IsFixedLength: true, IsFixedPrecisionScale: false, IsLong: false, IsNullable: true, IsSearchable: true, IsSearchableWithLike: false, false, DBNull.Value, DBNull.Value, IsConcurrencyType: false, DBNull.Value, null, null, "double");
			DataTypesAddRow(dataTable, "bit", 2, 1L, "bit", null, "System.Boolean", IsAutoincrementable: false, IsBestMatch: false, IsCaseSensitive: false, IsFixedLength: true, IsFixedPrecisionScale: false, IsLong: false, IsNullable: true, IsSearchable: true, IsSearchableWithLike: false, DBNull.Value, DBNull.Value, DBNull.Value, IsConcurrencyType: false, DBNull.Value, null, null, "bit");
			DataTypesAddRow(dataTable, "tinyint", 17, 3L, "tinyint", null, "System.SByte", IsAutoincrementable: true, IsBestMatch: true, IsCaseSensitive: false, IsFixedLength: true, IsFixedPrecisionScale: true, IsLong: false, IsNullable: true, IsSearchable: true, IsSearchableWithLike: false, false, DBNull.Value, DBNull.Value, IsConcurrencyType: false, DBNull.Value, null, null, "tinyint");
			DataTypesAddRow(dataTable, "bigint", 14, 19L, "bigint", null, "System.Int64", IsAutoincrementable: true, IsBestMatch: true, IsCaseSensitive: false, IsFixedLength: true, IsFixedPrecisionScale: true, IsLong: false, IsNullable: true, IsSearchable: true, IsSearchableWithLike: false, false, DBNull.Value, DBNull.Value, IsConcurrencyType: false, DBNull.Value, null, null, "bigint");
			DataTypesAddRow(dataTable, "varbinary", 23, 8000L, "varbinary({0})", "max length", "System.Byte[]", IsAutoincrementable: false, IsBestMatch: true, IsCaseSensitive: false, IsFixedLength: false, IsFixedPrecisionScale: false, IsLong: false, IsNullable: true, IsSearchable: true, IsSearchableWithLike: false, DBNull.Value, DBNull.Value, DBNull.Value, IsConcurrencyType: false, DBNull.Value, "0x", null, "varbinary");
			DataTypesAddRow(dataTable, "binary", 1, 8000L, "binary({0})", "length", "System.Byte[]", IsAutoincrementable: false, IsBestMatch: true, IsCaseSensitive: false, IsFixedLength: true, IsFixedPrecisionScale: false, IsLong: false, IsNullable: true, IsSearchable: true, IsSearchableWithLike: false, DBNull.Value, DBNull.Value, DBNull.Value, IsConcurrencyType: false, DBNull.Value, "0x", null, "binary");
			DataTypesAddRow(dataTable, "image", 0, 2147483647L, "image", null, "System.Byte[]", IsAutoincrementable: false, IsBestMatch: true, IsCaseSensitive: false, IsFixedLength: false, IsFixedPrecisionScale: false, IsLong: true, IsNullable: true, IsSearchable: false, IsSearchableWithLike: false, DBNull.Value, DBNull.Value, DBNull.Value, IsConcurrencyType: false, DBNull.Value, "0x", null, "image");
			DataTypesAddRow(dataTable, "char", 4, 8000L, "char", "length", "System.String", IsAutoincrementable: false, IsBestMatch: true, IsCaseSensitive: false, IsFixedLength: true, IsFixedPrecisionScale: false, IsLong: false, IsNullable: true, IsSearchable: true, IsSearchableWithLike: true, DBNull.Value, DBNull.Value, DBNull.Value, IsConcurrencyType: false, DBNull.Value, "'", "'", "char");
			DataTypesAddRow(dataTable, "varchar", 24, 8000L, "varchar({0})", "max length", "System.String", IsAutoincrementable: false, IsBestMatch: true, IsCaseSensitive: false, IsFixedLength: true, IsFixedPrecisionScale: false, IsLong: false, IsNullable: true, IsSearchable: true, IsSearchableWithLike: true, DBNull.Value, DBNull.Value, DBNull.Value, IsConcurrencyType: false, DBNull.Value, "'", "'", "varchar");
			DataTypesAddRow(dataTable, "text", 5, 2147483647L, "text", null, "System.String", IsAutoincrementable: false, IsBestMatch: false, IsCaseSensitive: false, IsFixedLength: false, IsFixedPrecisionScale: false, IsLong: true, IsNullable: true, IsSearchable: false, IsSearchableWithLike: false, DBNull.Value, DBNull.Value, DBNull.Value, IsConcurrencyType: false, DBNull.Value, "'", "'", "text");
			DataTypesAddRow(dataTable, "decimal", 8, 38L, "decimal({0}, {1})", "precision,scale", "System.Decimal", IsAutoincrementable: true, IsBestMatch: true, IsCaseSensitive: false, IsFixedLength: true, IsFixedPrecisionScale: false, IsLong: false, IsNullable: true, IsSearchable: true, IsSearchableWithLike: false, false, 38, 0, IsConcurrencyType: false, DBNull.Value, "'", "'", "decimal");
			DataTypesAddRow(dataTable, "datetime", 7, 23L, "datetime", null, "System.DateTime", IsAutoincrementable: false, IsBestMatch: true, IsCaseSensitive: false, IsFixedLength: true, IsFixedPrecisionScale: false, IsLong: false, IsNullable: true, IsSearchable: true, IsSearchableWithLike: true, DBNull.Value, DBNull.Value, DBNull.Value, IsConcurrencyType: false, DBNull.Value, null, null, "datetime");
			return dataTable;
		}

		protected virtual DataTable GetRestrictions()
		{
			object[][] data = new object[16][]
			{
				new object[4] { "Users", "Name", "", 0 },
				new object[4] { "Databases", "Name", "", 0 },
				new object[4] { "Tables", "Schema", "", 0 },
				new object[4] { "Tables", "Table", "", 1 },
				new object[4] { "Tables", "TableType", "", 2 },
				new object[4] { "Procedures", "Schema", "", 0 },
				new object[4] { "Procedures", "Procedure", "", 1 },
				new object[4] { "Procedures", "ProcedureType", "", 2 },
				new object[4] { "Columns", "Schema", "", 0 },
				new object[4] { "Columns", "Table", "", 1 },
				new object[4] { "Columns", "Column", "", 2 },
				new object[4] { "Indexes", "Schema", "", 0 },
				new object[4] { "Indexes", "Table", "", 1 },
				new object[4] { "Indexes", "Name", "", 2 },
				new object[4] { "PrimaryKeys", "Schema", "", 0 },
				new object[4] { "PrimaryKeys", "Table", "", 1 }
			};
			DataTable obj = new DataTable("Restrictions")
			{
				Columns = 
				{
					new DataColumn("CollectionName", typeof(string)),
					new DataColumn("RestrictionName", typeof(string)),
					new DataColumn("RestrictionDefault", typeof(string)),
					new DataColumn("RestrictionNumber", typeof(int))
				}
			};
			FillTable(obj, data);
			return obj;
		}

		private DataTable GetReservedWords()
		{
			DataTable dataTable = new DataTable("ReservedWords");
			dataTable.Columns.Add(new DataColumn(DbMetaDataColumnNames.ReservedWord, typeof(string)));
			Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Dm.ReservedWords.txt");
			StreamReader streamReader = new StreamReader(manifestResourceStream);
			for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
			{
				string[] array = text.Split(new char[1] { ' ' });
				foreach (string value in array)
				{
					if (!string.IsNullOrEmpty(value))
					{
						DataRow dataRow = dataTable.NewRow();
						dataRow[0] = value;
						dataTable.Rows.Add(dataRow);
					}
				}
			}
			streamReader.Close();
			manifestResourceStream.Close();
			DmCommand dmCommand = new DmCommand("SELECT KEYWORD FROM V$RESERVED_WORDS", m_Conn);
			using (DmDataReader dmDataReader = dmCommand.do_ExecuteDbDataReader(CommandBehavior.Default))
			{
				while (dmDataReader.do_Read())
				{
					DataRow dataRow2 = dataTable.NewRow();
					dataRow2[DbMetaDataColumnNames.ReservedWord] = dmDataReader.do_this(0);
					dataTable.Rows.Add(dataRow2);
				}
			}
			dmCommand.Dispose();
			return dataTable;
		}

		public virtual DataTable GetSchemas(string[] restrictions)
		{
			StringBuilder stringBuilder = new StringBuilder("select NAME from sysobjects where type$='SCH'");
			if (restrictions != null && restrictions.Length >= 1 && restrictions[0] != null)
			{
				stringBuilder.AppendFormat(DmConst.invariantCulture, " and name LIKE '{0}'", dup_chr_QUOTATION_MARK(restrictions[0]));
			}
			DmDataAdapter dmDataAdapter = new DmDataAdapter(stringBuilder.ToString(), m_Conn);
			DataTable dataTable = new DataTable();
			dmDataAdapter.Fill(dataTable);
			dataTable.TableName = "Schemas";
			return dataTable;
		}

		public virtual DataTable GetForeignKeys(string[] restrictions)
		{
			DataTable dataTable = new DataTable("ForeignKeys");
			dataTable.Columns.Add("TABLE_SCHEMA", typeof(string));
			dataTable.Columns.Add("TABLE_NAME", typeof(string));
			dataTable.Columns.Add("COLUMN_NAME", typeof(string));
			dataTable.Columns.Add("FOREIGN_KEY_CONSTRAINT_NAME", typeof(string));
			string fkname = null;
			if (restrictions != null && restrictions.Length == 3)
			{
				fkname = restrictions[2];
				restrictions[2] = null;
			}
			foreach (DataRow row in GetTables(restrictions).Rows)
			{
				LoadTableForeignkeys(dataTable, row["TABLE_SCHEMA"].ToString(), row["TABLE_NAME"].ToString(), fkname);
			}
			return dataTable;
		}

		public void LoadTableForeignkeys(DataTable dt, string schname, string tabname, string fkname)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT SCH.NAME  AS TABLE_SCHEMA,TAB.NAME  AS TABLE_SCHEMA,COLS.NAME AS COLUMN_NAME ,CONS.NAME AS FOREIGN_KEY_CONSTRAINT_NAME FROM SYS.SYSINDEXES INDS,(SELECT OBJ.NAME, CON.ID, CON.TYPE$  , CON.TABLEID, CON.COLID, CON.INDEXID FROM SYS.SYSCONS    AS CON, SYS.SYSOBJECTS AS OBJ WHERE OBJ.SUBTYPE$= 'CONS' AND OBJ.ID = CON.ID");
			if (!string.IsNullOrEmpty(fkname))
			{
				stringBuilder.AppendFormat(" AND NAME = {0}", dup_chr_QUOTATION_MARK(fkname));
			}
			stringBuilder.AppendFormat(")CONS,SYS.SYSCOLUMNS COLS,(SELECT NAME, ID, SCHID FROM SYS.SYSOBJECTS WHERE SUBTYPE$= 'UTAB' AND NAME = '{0}')TAB,(SELECT ID, NAME FROM SYS.SYSOBJECTS WHERE NAME = '{1}' AND TYPE$= 'SCH')SCH,(SELECT ID, NAME FROM SYS.SYSOBJECTS WHERE SUBTYPE$ = 'INDEX')OBJ_INDS WHERE TAB.SCHID = SCH.ID AND CONS.TYPE$= 'F' AND CONS.INDEXID = INDS.ID AND INDS.ID = OBJ_INDS.ID AND TAB.ID = COLS.ID AND CONS.TABLEID = TAB.ID AND SF_COL_IS_IDX_KEY(INDS.KEYNUM, INDS.KEYINFO, COLS.COLID)= 1", dup_chr_QUOTATION_MARK(tabname), dup_chr_QUOTATION_MARK(schname));
			DmCommand dmCommand = new DmCommand(stringBuilder.ToString(), m_Conn);
			using (DmDataReader dmDataReader = dmCommand.do_ExecuteDbDataReader(CommandBehavior.Default))
			{
				while (dmDataReader.do_Read())
				{
					DataRow dataRow = dt.NewRow();
					dataRow["TABLE_SCHEMA"] = schname;
					dataRow["TABLE_NAME"] = tabname;
					dataRow["COLUMN_NAME"] = dmDataReader.do_this(2);
					dataRow["FOREIGN_KEY_CONSTRAINT_NAME"] = dmDataReader.do_this(3);
					dt.Rows.Add();
				}
			}
			dmCommand.Dispose();
		}

		public virtual DataTable GetUsers(string[] restrictions)
		{
			StringBuilder stringBuilder = new StringBuilder("SELECT name as USERNAME FROM sys.sysobjects where SUBTYPE$='USER'");
			if (restrictions != null && restrictions.Length >= 1 && restrictions[0] != null)
			{
				stringBuilder.AppendFormat(DmConst.invariantCulture, " and name LIKE '{0}'", dup_chr_QUOTATION_MARK(restrictions[0]));
			}
			DmDataAdapter dmDataAdapter = new DmDataAdapter(stringBuilder.ToString(), m_Conn);
			DataTable dataTable = new DataTable();
			dmDataAdapter.Fill(dataTable);
			dataTable.TableName = "Users";
			return dataTable;
		}

		public virtual DataTable GetColumns(string[] restrictions)
		{
			DataTable dataTable = new DataTable("Columns");
			dataTable.Columns.Add("TABLE_SCHEMA", typeof(string));
			dataTable.Columns.Add("TABLE_NAME", typeof(string));
			dataTable.Columns.Add("COLUMN_NAME", typeof(string));
			dataTable.Columns.Add("COLUMN_ID", typeof(int));
			dataTable.Columns.Add("COLUMN_TYPE", typeof(string));
			dataTable.Columns.Add("COLUMN_SIZE", typeof(int));
			dataTable.Columns.Add("COLUMN_SCALE", typeof(int));
			dataTable.Columns.Add("NULLABLE", typeof(string));
			dataTable.Columns.Add("DEFAULT_VALUE", typeof(string));
			string columnRestriction = null;
			if (restrictions != null && restrictions.Length == 3)
			{
				columnRestriction = restrictions[2];
				restrictions[2] = null;
			}
			foreach (DataRow row in GetTables(restrictions).Rows)
			{
				LoadTableColumns(dataTable, row["TABLE_SCHEMA"].ToString(), row["TABLE_NAME"].ToString(), columnRestriction);
			}
			return dataTable;
		}

		private void LoadTableColumns(DataTable dt, string schema, string tableName, string columnRestriction)
		{
			string text = $"SELECT NAME, COLID,  CASE INSTR(TYPE$,'CLASS',1,1) WHEN 0 THEN TYPE$ ELSE SF_GET_CLASS_NAME(TYPE$) END AS COLUMN_TYPE, CASE SF_GET_COLUMN_SIZE(TYPE$, CAST (LENGTH$ AS INT), CAST (SCALE AS INT)) WHEN -2 THEN NULL ELSE SF_GET_COLUMN_SIZE(TYPE$, CAST (LENGTH$ AS INT), CAST (SCALE AS INT)) END AS COLUMN_SIZE, SCALE, NULLABLE$, DEFVAL FROM SYS.SYSCOLUMNS WHERE  ID = (SELECT ID FROM SYS.SYSOBJECTS WHERE NAME = '{tableName}' AND SUBTYPE$ IN ('UTAB','STAB','VIEW') AND SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE NAME = '{schema}' AND TYPE$='SCH'))";
			if (columnRestriction != null)
			{
				string text2 = $" AND NAME LIKE '{columnRestriction}'";
				text += text2;
			}
			DmCommand dmCommand = new DmCommand(text, m_Conn);
			try
			{
				using DmDataReader dmDataReader = dmCommand.do_ExecuteDbDataReader(CommandBehavior.Default);
				while (dmDataReader.do_Read())
				{
					DataRow dataRow = dt.NewRow();
					dataRow["TABLE_SCHEMA"] = schema;
					dataRow["TABLE_NAME"] = tableName;
					dataRow["COLUMN_NAME"] = dmDataReader.do_GetString(0);
					dataRow["COLUMN_ID"] = dmDataReader.do_GetInt16(1);
					dataRow["COLUMN_TYPE"] = dmDataReader.do_GetString(2);
					dataRow["COLUMN_SIZE"] = dmDataReader.do_GetInt32(3);
					dataRow["COLUMN_SCALE"] = dmDataReader.do_GetInt16(4);
					dataRow["NULLABLE"] = dmDataReader.do_GetString(5);
					dataRow["DEFAULT_VALUE"] = dmDataReader.do_GetString(6);
					dt.Rows.Add(dataRow);
				}
			}
			finally
			{
				dmCommand?.Dispose();
			}
		}

		public virtual DataTable GetIndexes(string[] restrictions)
		{
			DataTable dataTable = new DataTable("Indexes");
			dataTable.Columns.Add("INDEX_SCHEMA", typeof(string));
			dataTable.Columns.Add("INDEX_NAME", typeof(string));
			dataTable.Columns.Add("TABLE_NAME", typeof(string));
			dataTable.Columns.Add("UNIQUE", typeof(string));
			dataTable.Columns.Add("INDEX_TYPE", typeof(string));
			string[] array = new string[Math.Max((restrictions == null) ? 3 : restrictions.Length, 3)];
			restrictions?.CopyTo(array, 0);
			array[2] = "UTAB";
			foreach (DataRow row in GetTables(array).Rows)
			{
				string text = string.Format("SELECT OBJ_INDS.NAME, INDS.ISUNIQUE, CASE WHEN INDS.XTYPE & 0X00001000 = 0X00001000 THEN 'BITMAP' WHEN INDS.XTYPE & 0X00000001 = 0X00000000 THEN 'CLUSTER' WHEN INDS.XTYPE & 0X11111111 = 0X00000001 THEN 'DEFAULT' ELSE 'UNKNOW' END INDEX_TYPE  FROM SYS.SYSINDEXES INDS, (SELECT ID, NAME, PID FROM SYS.SYSOBJECTS WHERE SUBTYPE$='INDEX') OBJ_INDS  WHERE INDS.ID=OBJ_INDS.ID AND OBJ_INDS.PID = (SELECT ID FROM SYS.SYSOBJECTS WHERE SUBTYPE$ = 'UTAB' AND NAME = '{0}' AND SCHID =  (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$='SCH' AND NAME = '{1}'))", row["TABLE_NAME"].ToString(), row["TABLE_SCHEMA"].ToString());
				if (restrictions != null && restrictions.Length >= 3 && restrictions[2] != null)
				{
					string text2 = $" AND OBJ_INDS.NAME LIKE '{dup_chr_QUOTATION_MARK(restrictions[2])}'";
					text += text2;
				}
				DmCommand dmCommand = new DmCommand(text, m_Conn);
				using (DmDataReader dmDataReader = dmCommand.do_ExecuteDbDataReader(CommandBehavior.Default))
				{
					while (dmDataReader.do_Read())
					{
						DataRow dataRow2 = dataTable.NewRow();
						dataRow2["INDEX_SCHEMA"] = row["TABLE_SCHEMA"].ToString();
						dataRow2["INDEX_NAME"] = dmDataReader.do_GetString(0);
						dataRow2["TABLE_NAME"] = row["TABLE_NAME"].ToString();
						dataRow2["UNIQUE"] = dmDataReader.do_GetString(1);
						dataRow2["INDEX_TYPE"] = dmDataReader.do_GetString(2);
						dataTable.Rows.Add(dataRow2);
					}
				}
				dmCommand.Dispose();
			}
			return dataTable;
		}

		public virtual DataTable GetPrimaryKeys(string[] restrictions)
		{
			if (restrictions == null)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_SCHEMA_RESTRICTIONS);
			}
			if (restrictions.Length < 2)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_SCHEMA_RESTRICTIONS);
			}
			if (restrictions[0] == null || restrictions[1] == null)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_SCHEMA_RESTRICTIONS);
			}
			DataTable dataTable = new DataTable("PrimaryKeys");
			dataTable.Columns.Add("TABLE_SCHEMA", typeof(string));
			dataTable.Columns.Add("TABLE_NAME", typeof(string));
			dataTable.Columns.Add("COLUMN_NAME", typeof(string));
			dataTable.Columns.Add("KEY_SEQUENCE", typeof(int));
			dataTable.Columns.Add("PK_NAME", typeof(string));
			DmCommand dmCommand = new DmCommand($"SELECT COLS.NAME AS COLUMN_NAME, SF_GET_INDEX_KEY_SEQ(INDS.KEYNUM, INDS.KEYINFO, COLS.COLID) AS KEY_SEQ, CONS.NAME AS PK_NAME  FROM SYS.SYSINDEXES INDS,  (SELECT OBJ.NAME, CON.ID, CON.TYPE$, CON.TABLEID, CON.COLID, CON.INDEXID FROM SYS.SYSCONS AS CON, SYS.SYSOBJECTS AS OBJ WHERE OBJ.SUBTYPE$='CONS' AND OBJ.ID=CON.ID) CONS,  SYS.SYSCOLUMNS COLS,  (SELECT NAME, ID FROM SYS.SYSOBJECTS WHERE SUBTYPE$='UTAB' AND NAME = '{dup_chr_QUOTATION_MARK(restrictions[1])}' AND SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE NAME = '{dup_chr_QUOTATION_MARK(restrictions[0])}' AND TYPE$='SCH')) TAB,  (SELECT ID, NAME FROM SYS.SYSOBJECTS WHERE SUBTYPE$ = 'INDEX') OBJ_INDS  WHERE CONS.TYPE$='P' AND CONS.INDEXID=INDS.ID AND INDS.ID=OBJ_INDS.ID AND TAB.ID=COLS.ID AND CONS.TABLEID=TAB.ID  AND SF_COL_IS_IDX_KEY(INDS.KEYNUM, INDS.KEYINFO,COLS.COLID)=1 ", m_Conn);
			using (DmDataReader dmDataReader = dmCommand.do_ExecuteDbDataReader(CommandBehavior.Default))
			{
				while (dmDataReader.do_Read())
				{
					DataRow dataRow = dataTable.NewRow();
					dataRow["TABLE_SCHEMA"] = restrictions[0].ToString();
					dataRow["TABLE_NAME"] = restrictions[1].ToString();
					dataRow["COLUMN_NAME"] = dmDataReader.do_GetString(0);
					dataRow["KEY_SEQUENCE"] = dmDataReader.do_GetInt16(1);
					dataRow["PK_NAME"] = dmDataReader.do_GetString(2);
					dataTable.Rows.Add(dataRow);
				}
			}
			dmCommand.Dispose();
			return dataTable;
		}

		public virtual DataTable GetIndexColumns(string[] restrictions)
		{
			if (restrictions == null)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_INDEXCOL_RESTRICTIONS);
			}
			if (restrictions.Length < 3)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_INDEXCOL_RESTRICTIONS);
			}
			if (restrictions[0] == null || restrictions[1] == null || restrictions[2] == null)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_INVALID_INDEXCOL_RESTRICTIONS);
			}
			DataTable dataTable = new DataTable("IndexColumns");
			dataTable.Columns.Add("COLUMN_NAME", typeof(string));
			dataTable.Columns.Add("SORT_ORDER", typeof(string));
			DmCommand dmCommand = new DmCommand(string.Format("DECLARE KI      VARBINARY(816); CURR_P  INT; KEYNUM  INT; TABLEID BIGINT; COLID   SMALLINT; SORT    VARCHAR(10); COLNAME VARCHAR(128); CUR2     CURSOR; CUR_STR2 VARCHAR(8188); CUR     CURSOR; CUR_STR VARCHAR(8188); BEGIN CUR_STR = 'SELECT INDS.KEYNUM, INDS.KEYINFO, OBJ_INDS.PID FROM SYS.SYSINDEXES INDS, (SELECT ID, NAME, PID FROM SYS.SYSOBJECTS WHERE SUBTYPE$=''INDEX'') OBJ_INDS  WHERE INDS.ID=OBJ_INDS.ID AND OBJ_INDS.PID = (SELECT ID FROM SYS.SYSOBJECTS WHERE SUBTYPE$ = ''UTAB'' AND NAME = ''{1}'' AND SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$=''SCH'' AND NAME = ''{0}'')) AND OBJ_INDS.NAME = ''{2}'';'; OPEN CUR FOR CUR_STR; FETCH CUR INTO KEYNUM, KI, TABLEID; CURR_P = 0; PRINT CURR_P; EXECUTE IMMEDIATE 'CREATE TABLE ##DMGET_INDEXCOLUMNS_3169(COLNAME VARCHAR(128), SORTORDER CHAR(1));'; FOR I IN 1..KEYNUM LOOP \tCOLID = SF_BIN_GET_SMALLINT(KI, CURR_P);    CURR_P =  CURR_P + 2;        SORT = SF_BIN_GET_CHAR(KI, CURR_P);    CURR_P =  CURR_P + 1;        CUR_STR2 = 'SELECT NAME FROM SYS.SYSCOLUMNS WHERE ID = '||TABLEID||' AND COLID = '||COLID||';';        OPEN CUR2 FOR CUR_STR2;    FETCH CUR2 INTO COLNAME;    CLOSE CUR2;        EXECUTE IMMEDIATE 'INSERT INTO ##DMGET_INDEXCOLUMNS_3169 VALUES('''||COLNAME||''', '''||SORT||''');';    END LOOP; CLOSE CUR; EXECUTE IMMEDIATE 'SELECT * FROM ##DMGET_INDEXCOLUMNS_3169;'; EXECUTE IMMEDIATE 'DROP TABLE ##DMGET_INDEXCOLUMNS_3169;'; END;", dup_chr_QUOTATION_MARK(restrictions[0]), dup_chr_QUOTATION_MARK(restrictions[1]), dup_chr_QUOTATION_MARK(restrictions[2])), m_Conn);
			using (DmDataReader dmDataReader = dmCommand.do_ExecuteDbDataReader(CommandBehavior.Default))
			{
				while (dmDataReader.do_Read())
				{
					DataRow dataRow = dataTable.NewRow();
					dataRow["COLUMN_NAME"] = dmDataReader.do_GetString(0);
					dataRow["SORT_ORDER"] = dmDataReader.do_GetString(1);
					dataTable.Rows.Add(dataRow);
				}
			}
			dmCommand.Dispose();
			return dataTable;
		}

		private DataTable GetSchemaInner(string collection, string[] restrictions)
		{
			if (restrictions != null && restrictions[0] == null)
			{
				restrictions[0] = m_Conn.Schema;
			}
			switch (collection)
			{
			case "METADATACOLLECTIONS":
				return GetCollections();
			case "DATASOURCEINFORMATION":
				return GetDataSourceInformation();
			case "DATATYPES":
				return GetDataTypes();
			case "RESTRICTIONS":
				return GetRestrictions();
			case "RESERVEDWORDS":
				return GetReservedWords();
			case "USERS":
				return GetUsers(restrictions);
			case "TABLES":
				return GetTables(restrictions);
			case "PROCEDURES":
				return GetProcedures(restrictions);
			case "PARAMETERS":
				return GetParameters(restrictions);
			case "COLUMNS":
				return GetColumns(restrictions);
			case "INDEXES":
				return GetIndexes(restrictions);
			case "PRIMARYKEYS":
				return GetPrimaryKeys(restrictions);
			case "INDEXCOLUMNS":
				return GetIndexColumns(restrictions);
			case "DATABASES":
			case "SCHEMAS":
				return GetSchemas(restrictions);
			case "FOREIGNKEYS":
				return GetForeignKeys(restrictions);
			default:
				return null;
			}
		}

		private static string[] CleanRestrictions(string[] restrictionValues)
		{
			string[] array = null;
			if (restrictionValues != null)
			{
				array = (string[])restrictionValues.Clone();
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (text != null)
					{
						array[i] = text.Trim('`');
					}
				}
			}
			return array;
		}

		private static string GetString(DmDataReader reader, int index)
		{
			if (reader.do_IsDBNull(index))
			{
				return null;
			}
			return reader.GetString(index);
		}

		private static void FillTable(DataTable dt, object[][] data)
		{
			foreach (object[] array in data)
			{
				DataRow dataRow = dt.NewRow();
				for (int j = 0; j < array.Length; j++)
				{
					dataRow[j] = array[j];
				}
				dt.Rows.Add(dataRow);
			}
		}

		private static bool IsEscape(string str)
		{
			if (str.IndexOf('%') != -1 || str.IndexOf('_') != -1)
			{
				return true;
			}
			return false;
		}
	}
}
