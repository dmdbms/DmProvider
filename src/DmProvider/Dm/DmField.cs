namespace Dm
{
	internal class DmField
	{
		private string m_SchName;

		private byte[] m_SchNameForSet;

		private string m_TabName;

		private byte[] m_TabNameForSet;

		private string m_Name;

		private byte[] m_NameForSet;

		private string m_TypeName;

		private byte[] m_TypeNameForSet;

		private byte m_TypeFlag;

		private int m_CType;

		private short m_InOutType = 3;

		private int m_Precision;

		private int m_Size;

		private int m_Scale;

		private bool m_Nullable;

		private bool m_Identity;

		private bool m_IsLob;

		private short m_Dbid;

		private int m_Schid;

		private int m_Tabid;

		private short m_Colid;

		private string m_BaseCatalog;

		private byte[] m_BaseCataLogForSet;

		private string m_BaseSchema;

		private byte[] m_BaseSchemaForSet;

		private string m_BaseTable;

		private byte[] m_BaseTableForSet;

		private string m_BaseColumn;

		private byte[] m_BaseColumnForSet;

		private string m_ServerEncoding;

		private TypeDescriptor typeDescriptor;

		internal TypeDescriptor TypeDesc
		{
			get
			{
				return typeDescriptor;
			}
			set
			{
				typeDescriptor = value;
			}
		}

		public DmField(DmConnInstance conn)
		{
			m_ServerEncoding = conn.ConnProperty.ServerEncoding;
		}

		public DmField(string ServerEncoding)
		{
			m_ServerEncoding = ServerEncoding;
		}

		public void SetCType(int type)
		{
			m_CType = type;
		}

		public void SetTypeFlag(byte typeFlag)
		{
			m_TypeFlag = typeFlag;
		}

		public void SetPrecision(int precision)
		{
			m_Precision = precision;
		}

		public void SetSize(int size)
		{
			m_Size = size;
		}

		public void SetScale(int colnumscale)
		{
			m_Scale = colnumscale;
		}

		public void SetInOutType(short type)
		{
			m_InOutType = type;
		}

		public void SetNullable(bool nullable)
		{
			m_Nullable = nullable;
		}

		public void SetIdentity(bool identity)
		{
			m_Identity = identity;
		}

		public void SetIsLob(bool isLob)
		{
			m_IsLob = isLob;
		}

		public void SetDbID(short dbid)
		{
			m_Dbid = dbid;
		}

		public void SetSchemaID(int schid)
		{
			m_Schid = schid;
		}

		public void SetTableID(int tabid)
		{
			m_Tabid = tabid;
		}

		public void SetColID(short colid)
		{
			m_Colid = colid;
		}

		public string GetName()
		{
			if (m_Name == null)
			{
				if (m_NameForSet != null && m_NameForSet.Length != 0)
				{
					m_Name = DmConvertion.GetString(m_NameForSet, 0, m_NameForSet.Length, m_ServerEncoding);
				}
				else
				{
					m_Name = "";
				}
			}
			return m_Name;
		}

		public int GetCType()
		{
			return m_CType;
		}

		public byte GetTypeFlag()
		{
			return m_TypeFlag;
		}

		public int GetPrecision()
		{
			return m_Precision;
		}

		public int GetSize()
		{
			return m_Size;
		}

		public int GetScale()
		{
			return m_Scale;
		}

		public short GetInOutType()
		{
			return m_InOutType;
		}

		public bool GetNullable()
		{
			return m_Nullable;
		}

		public bool GetIdentity()
		{
			return m_Identity;
		}

		public bool GetIsLob()
		{
			return m_IsLob;
		}

		public short GetDbID()
		{
			return m_Dbid;
		}

		public string GetBaseCatalog()
		{
			if (m_BaseCatalog == null)
			{
				if (m_BaseCataLogForSet != null && m_BaseCataLogForSet.Length != 0)
				{
					m_BaseCatalog = DmConvertion.GetString(m_BaseCataLogForSet, 0, m_BaseCataLogForSet.Length, m_ServerEncoding);
				}
				else
				{
					m_BaseCatalog = "";
				}
			}
			return m_BaseCatalog;
		}

		public int GetSchemaID()
		{
			return m_Schid;
		}

		public string GetBaseSchema()
		{
			if (m_BaseSchema == null)
			{
				if (m_BaseSchemaForSet != null && m_BaseSchemaForSet.Length != 0)
				{
					m_BaseSchema = DmConvertion.GetString(m_BaseSchemaForSet, 0, m_BaseSchemaForSet.Length, m_ServerEncoding);
				}
				else
				{
					m_BaseSchema = "";
				}
			}
			return m_BaseSchema;
		}

		public int GetTableID()
		{
			return m_Tabid;
		}

		public string GetBaseTable()
		{
			if (m_BaseTable == null)
			{
				if (m_BaseTableForSet != null && m_BaseTableForSet.Length != 0)
				{
					m_BaseTable = DmConvertion.GetString(m_BaseTableForSet, 0, m_BaseTableForSet.Length, m_ServerEncoding);
				}
				else
				{
					m_BaseTable = "";
				}
			}
			return m_BaseTable;
		}

		public short GetColID()
		{
			return m_Colid;
		}

		public string GetBaseColumn()
		{
			if (m_BaseColumn == null)
			{
				if (m_BaseColumnForSet != null && m_BaseColumnForSet.Length != 0)
				{
					m_BaseColumn = DmConvertion.GetString(m_BaseColumnForSet, 0, m_BaseColumnForSet.Length, m_ServerEncoding);
				}
				else
				{
					m_BaseColumn = "";
				}
			}
			return m_BaseColumn;
		}

		public void SetTypeName(DmMsg msg, int len)
		{
			if (len != 0)
			{
				m_TypeNameForSet = msg.ReadBytes(len);
			}
			else
			{
				m_TypeNameForSet = new byte[0];
			}
		}

		public void SetBaseColumn(DmMsg msg, int len)
		{
			if (len != 0)
			{
				m_BaseColumnForSet = msg.ReadBytes(len);
			}
			else
			{
				m_BaseColumnForSet = new byte[0];
			}
		}

		public void SetName(DmMsg msg, int len)
		{
			if (len != 0)
			{
				m_NameForSet = msg.ReadBytes(len);
			}
			else
			{
				m_NameForSet = new byte[0];
			}
		}

		public void SetSchema(DmMsg msg, int len)
		{
			if (len != 0)
			{
				m_SchNameForSet = msg.ReadBytes(len);
			}
			else
			{
				m_SchNameForSet = new byte[0];
			}
		}

		public void SetTable(DmMsg msg, int len)
		{
			if (len != 0)
			{
				m_TabNameForSet = msg.ReadBytes(len);
			}
			else
			{
				m_TabNameForSet = new byte[0];
			}
		}

		public string GetSchema()
		{
			if (m_SchName == null)
			{
				if (m_SchNameForSet != null && m_SchNameForSet.Length != 0)
				{
					m_SchName = DmConvertion.GetString(m_SchNameForSet, 0, m_SchNameForSet.Length, m_ServerEncoding);
				}
				else
				{
					m_SchName = "";
				}
			}
			return m_SchName;
		}

		public string GetTable()
		{
			if (m_TabName == null)
			{
				if (m_TabNameForSet != null && m_TabNameForSet.Length != 0)
				{
					m_TabName = DmConvertion.GetString(m_TabNameForSet, 0, m_TabNameForSet.Length, m_ServerEncoding);
				}
				else
				{
					m_TabName = "";
				}
			}
			return m_TabName;
		}

		public string GetTypeName()
		{
			if (m_TypeName == null)
			{
				if (m_TypeNameForSet != null && m_TypeNameForSet.Length != 0)
				{
					m_TypeName = DmConvertion.GetString(m_TypeNameForSet, 0, m_TypeNameForSet.Length, m_ServerEncoding);
				}
				else
				{
					m_TypeName = "";
				}
			}
			return m_TypeName;
		}
	}
}
