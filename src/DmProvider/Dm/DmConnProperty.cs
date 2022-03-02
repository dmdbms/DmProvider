using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dm.Config;

namespace Dm
{
	internal class DmConnProperty
	{
		private byte m_MppType;

		private byte m_NewLobFlag = 1;

		private bool? m_AutoCommit;

		private int m_MaxRowSize;

		private int m_DDLAutoCommit;

		private bool m_CaseSensitive = true;

		private byte m_backslashEsc;

		private int m_FailedAttempts;

		private string m_LastLoginIP;

		private string m_LastLoginTime;

		private int m_LoginWarningID;

		private int m_GracetimeRemainder;

		private string m_guid;

		private string m_CurSchema;

		private IsolationLevel m_ISOlvl = IsolationLevel.ReadCommitted;

		private int m_encrypt;

		private int m_serials;

		private short m_DbTimeZone;

		private int m_sessClt = 3;

		private byte m_accessMode;

		private int m_MaxSession;

		private byte m_c2p = 1;

		private bool m_commited;

		private string m_serverVersion;

		private int m_verNum;

		private bool m_DbrwSeparate;

		private string m_standbyIp;

		private int m_standbyPort = 5236;

		private int m_standbyNum;

		private long _sessId = -1L;

		private bool rwStandby;

		private int svrMode;

		private int svrStat;

		private bool dscControl;

		internal string oracleDateFormat = "YYYY-MM-DD";

		internal string oracleTimestampFormat = "YYYY-MM-DD HH24:MI:SS.FF6";

		internal string oracleTimestampTZFormat = "YYYY-MM-DD HH24:MI:SS.FF6 TZH:TZM";

		internal string oracleTimeFormat = "HH24:MI:SS.FF6";

		internal string oracleTimeTZFormat = "HH24:MI:SS.FF6 TZH:TZM";

		private string serverEncoding = "gb18030";

		internal int nlsDateLang;

		internal int msgVersion = 2;

		internal byte[] serverPubKey;

		internal int encryptType = -1;

		internal int hashType = -1;

		internal bool encryptPwd;

		internal bool encryptMsg;

		private DmSvcConfig config;

		private DmConnectionStringBuilder builder;

		private Dictionary<string, object> property;

		internal static Dictionary<string, Encoding> encodingMap;

		internal string ConnectionString
		{
			get
			{
				return builder.ConnectionString;
			}
			set
			{
				builder.ConnectionString = value;
				foreach (string key2 in builder.setProperty.Keys)
				{
					property[key2] = builder.setProperty[key2];
				}
				config = new DmSvcConfig(builder.property[DmConst.PROP_KEY_DM_SVC_PATH].ToString());
				string key = builder.property[DmConst.PROP_KEY_SERVER].ToString();
				if (!config.propertyDictionary.ContainsKey(key))
				{
					key = "defaultproperty";
				}
				foreach (string key3 in config.propertyDictionary[key].Keys)
				{
					if (!property.ContainsKey(key3))
					{
						property[key3] = config.propertyDictionary[key][key3];
					}
				}
				ServName = property[DmConst.PROP_KEY_SERVER]?.ToString() + ":" + property[DmConst.PROP_KEY_PORT];
				string[] array = property[DmConst.PROP_KEY_ADDRESS_REMAP].ToString()!.Split(new char[1] { '&' }, StringSplitOptions.RemoveEmptyEntries);
				string[] array2 = null;
				string[] array3;
				if (array != null && array.Length != 0)
				{
					array3 = array;
					for (int i = 0; i < array3.Length; i++)
					{
						array2 = array3[i].Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
						if (array2 != null && array2.Length == 2 && array2[0].Equals(Server + ":" + Port))
						{
							if (!array2[1].Contains(":"))
							{
								throw new ArgumentException("format: address_remap=192.168.1.24:5236,192.168.1.23:5236&192.168.1.22:5236,192.168.1.20:5236");
							}
							string[] array4 = array2[1].Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
							if (!int.TryParse(array4[1], out var result))
							{
								throw new ArgumentException("format: address_remap=192.168.1.24:5236,192.168.1.23:5236&192.168.1.22:5236,192.168.1.20:5236");
							}
							Server = array4[0];
							Port = result;
							break;
						}
					}
				}
				array = property[DmConst.PROP_KEY_USER_REMAP].ToString()!.Split(new char[1] { '&' }, StringSplitOptions.RemoveEmptyEntries);
				if (array == null || array.Length == 0)
				{
					return;
				}
				array3 = array;
				for (int i = 0; i < array3.Length; i++)
				{
					array2 = array3[i].Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					if (array2 != null && array2.Length == 2 && array2[0].Equals(User))
					{
						User = array2[1];
						break;
					}
				}
				AdjustProperty();
			}
		}

		internal string ServName { get; set; }

		internal string ServerActual { get; set; }

		internal int PortActual { get; set; }

		internal EPGroup EPGroup
		{
			get
			{
				if (!property.ContainsKey(DmConst.PROP_KEY_EP_GROUP) || property[DmConst.PROP_KEY_EP_GROUP] == null)
				{
					return new EPGroup(new List<EP>
					{
						new EP(Server, Port)
					});
				}
				return (EPGroup)property[DmConst.PROP_KEY_EP_GROUP];
			}
			set
			{
				property[DmConst.PROP_KEY_EP_GROUP] = null;
			}
		}

		internal string Server
		{
			get
			{
				return Convert.ToString(property[DmConst.PROP_KEY_SERVER]);
			}
			set
			{
				property[DmConst.PROP_KEY_SERVER] = value;
				builder.Server = value;
			}
		}

		internal string User
		{
			get
			{
				return Convert.ToString(property[DmConst.PROP_KEY_USER]);
			}
			set
			{
				property[DmConst.PROP_KEY_USER] = value;
			}
		}

		internal string Pwd
		{
			get
			{
				return Convert.ToString(property[DmConst.PROP_KEY_PASSWORD]);
			}
			set
			{
				property[DmConst.PROP_KEY_PASSWORD] = value;
			}
		}

		internal int Port
		{
			get
			{
				return Convert.ToInt32(property[DmConst.PROP_KEY_PORT]);
			}
			set
			{
				property[DmConst.PROP_KEY_PORT] = value;
				builder.Port = value;
			}
		}

		internal string ServerEncoding
		{
			get
			{
				string text = property[DmConst.PROP_KEY_ENCODING].ToString();
				if (!text.Equals(""))
				{
					return text;
				}
				return serverEncoding;
			}
			set
			{
				serverEncoding = value;
			}
		}

		internal bool Enlist => Convert.ToBoolean(property[DmConst.PROP_KEY_ENLIST]);

		internal int ConnectionTimeout => Convert.ToInt32(property[DmConst.PROP_KEY_CONNECTION_TIMEOUT]);

		internal int SocketTimeout => Convert.ToInt32(property[DmConst.PROP_KEY_SOCKET_TIMEOUT]);

		internal int CommandTimeout => Convert.ToInt32(property[DmConst.PROP_KEY_COMMAND_TIMEOUT]);

		internal int PoolSize => Convert.ToInt32(property[DmConst.PROP_KEY_STMT_POOL_SIZE]);

		internal bool ConnPooling
		{
			get
			{
				return Convert.ToBoolean(property[DmConst.PROP_KEY_CONN_POOLING]);
			}
			set
			{
				property[DmConst.PROP_KEY_CONN_POOLING] = value;
				builder.ConnPooling = value;
			}
		}

		internal int ConnPoolSize
		{
			get
			{
				return Convert.ToInt32(property[DmConst.PROP_KEY_CONN_POOL_SIZE]);
			}
			set
			{
				property[DmConst.PROP_KEY_CONN_POOL_SIZE] = value;
				builder.ConnPoolSize = value;
			}
		}

		internal bool ConnPoolCheck
		{
			get
			{
				return Convert.ToBoolean(property[DmConst.PROP_KEY_CONN_POOL_CHECK]);
			}
			set
			{
				property[DmConst.PROP_KEY_CONN_POOL_CHECK] = value;
				builder.ConnPoolCheck = value;
			}
		}

		internal bool EscapeProcess
		{
			get
			{
				return Convert.ToBoolean(property[DmConst.PROP_KEY_ESCAPE_PROCESS]);
			}
			set
			{
				property[DmConst.PROP_KEY_ESCAPE_PROCESS] = value;
				builder.EscapeProcess = value;
			}
		}

		internal bool StmtPooling => Convert.ToBoolean(property[DmConst.PROP_KEY_STMT_POOLING]);

		internal bool PreparePooling => Convert.ToBoolean(property[DmConst.PROP_KEY_PSTMT_POOLING]);

		internal int PreparePoolSize => Convert.ToInt32(property[DmConst.PROP_KEY_PSTMT_POOL_SIZE]);

		internal short TimeZone
		{
			get
			{
				return Convert.ToInt16(property[DmConst.PROP_KEY_TIMEZONE]);
			}
			set
			{
				property[DmConst.PROP_KEY_TIMEZONE] = value;
				builder.Time_Zone = value;
			}
		}

		internal string[] ResveredList => property[DmConst.PROP_KEY_KEYWORDS]?.ToString().Split(new char[1] { ',' });

		internal LoginModeFlag? LoginPrimary
		{
			get
			{
				return property[DmConst.PROP_KEY_LOGIN_MODE] as LoginModeFlag?;
			}
			set
			{
				property[DmConst.PROP_KEY_LOGIN_MODE] = value;
				builder.Login_Mode = value;
			}
		}

		internal EpSelector? EpSelector => property[DmConst.PROP_KEY_EP_SELECTOR] as EpSelector?;

		internal int SwitchTimes
		{
			get
			{
				return Convert.ToInt32(property[DmConst.PROP_KEY_SWITCH_TIMES]);
			}
			set
			{
				property[DmConst.PROP_KEY_SWITCH_TIMES] = value;
			}
		}

		internal int SwitchInterval => Convert.ToInt32(property[DmConst.PROP_KEY_SWITCH_INTERVAL]);

		internal LoginStatus? LoginStatus => property[DmConst.PROP_KEY_LOGIN_STATUS] as LoginStatus?;

		internal bool LoginDscCtrl => Convert.ToBoolean(property[DmConst.PROP_KEY_LOGIN_DSC_CTRL]);

		internal int RwStandbyRecoverTime => Convert.ToInt32(property[DmConst.PROP_KEY_RW_STANDBY_RECOVER_TIME]);

		internal bool RWHA => Convert.ToBoolean(property[DmConst.PROP_KEY_RW_HA]);

		internal DoSwitch? DoSwitch => property[DmConst.PROP_KEY_DO_SWITCH] as DoSwitch?;

		internal CLUSTER? Cluster => property[DmConst.PROP_KEY_CLUSTER] as CLUSTER?;

		internal string Schema
		{
			get
			{
				return Convert.ToString(property[DmConst.PROP_KEY_SCHEMA]);
			}
			set
			{
				property[DmConst.PROP_KEY_SCHEMA] = value;
				builder.Schema = value;
			}
		}

		internal string AppName => Convert.ToString(property[DmConst.PROP_KEY_APPNAME]);

		internal string Host => Convert.ToString(property[DmConst.PROP_KEY_HOST]);

		internal string OS => Convert.ToString(property[DmConst.PROP_KEY_OS]);

		internal string Database
		{
			get
			{
				return Convert.ToString(property[DmConst.PROP_KEY_DATABASE]);
			}
			set
			{
				property[DmConst.PROP_KEY_DATABASE] = value;
				builder.Database = value;
			}
		}

		internal int RwPercent => Convert.ToInt32(property[DmConst.PROP_KEY_RW_PERCENT]);

		internal bool RwSeparate
		{
			get
			{
				return Convert.ToBoolean(property[DmConst.PROP_KEY_RW_SEPARATE]);
			}
			set
			{
				property[DmConst.PROP_KEY_RW_SEPARATE] = value;
				if (Convert.ToBoolean(property[DmConst.PROP_KEY_RW_SEPARATE]))
				{
					property[DmConst.PROP_KEY_LOGIN_MODE] = LoginModeFlag.onlyprimary;
					builder.Login_Mode = LoginModeFlag.onlyprimary;
				}
			}
		}

		internal int StmtPoolSize => Convert.ToInt32(property[DmConst.PROP_KEY_STMT_POOL_SIZE]);

		internal int Language => Convert.ToInt32(property[DmConst.PROP_KEY_LANGUAGE]);

		internal int Compress
		{
			get
			{
				return Convert.ToInt32(property[DmConst.PROP_KEY_COMPRESS]);
			}
			set
			{
				property[DmConst.PROP_KEY_COMPRESS] = value;
			}
		}

		internal int CompressId => Convert.ToInt32(property[DmConst.PROP_KEY_COMPRESS_ID]);

		internal int LobMode => Convert.ToInt32(property[DmConst.PROP_KEY_LOB_MODE]);

		internal CompatibleMode CompatibleMode => (CompatibleMode)Convert.ToInt32(property[DmConst.PROP_KEY_COMPATIBLE_MODE]);

		internal bool IgnoreCase => Convert.ToBoolean(property[DmConst.PROP_KEY_IGNORE_CASE]);

		internal bool IsBdtaRs
		{
			get
			{
				return Convert.ToBoolean(property[DmConst.PROP_KEY_IS_BDTA_RS]);
			}
			set
			{
				property[DmConst.PROP_KEY_IS_BDTA_RS] = value;
			}
		}

		internal bool CrcBody { get; set; }

		internal bool EnRsCache => Convert.ToBoolean(property[DmConst.PROP_KEY_ENABLE_RS_CACHE]);

		internal int RsCacheSize => Convert.ToInt32(property[DmConst.PROP_KEY_RS_CACHE_SIZE]);

		internal int RsRefreshFreq => Convert.ToInt32(property[DmConst.PROP_KEY_RS_REFRESH_FREQ]);

		internal int DbAliveCheckFreq => Convert.ToInt32(property[DmConst.PROP_KEY_DB_ALIVE_CHECK_FREQ]);

		internal int DbAliveCheckTimeout => Convert.ToInt32(property[DmConst.PROP_KEY_DB_ALIVE_CHECK_TIMEOUT]);

		internal int StandbyNum
		{
			get
			{
				return m_standbyNum;
			}
			set
			{
				m_standbyNum = value;
			}
		}

		internal string StandbyIp
		{
			get
			{
				return m_standbyIp;
			}
			set
			{
				m_standbyIp = value;
			}
		}

		internal int StandbyPort
		{
			get
			{
				return m_standbyPort;
			}
			set
			{
				m_standbyPort = value;
			}
		}

		internal long SessId
		{
			get
			{
				return _sessId;
			}
			set
			{
				_sessId = value;
			}
		}

		internal bool DbrwSeparate
		{
			get
			{
				return m_DbrwSeparate;
			}
			set
			{
				m_DbrwSeparate = value;
			}
		}

		internal byte MppType
		{
			get
			{
				return m_MppType;
			}
			set
			{
				m_MppType = value;
			}
		}

		internal byte NewLobFlag
		{
			get
			{
				return m_NewLobFlag;
			}
			set
			{
				m_NewLobFlag = value;
			}
		}

		internal int VerNum
		{
			get
			{
				return m_verNum;
			}
			set
			{
				m_verNum = value;
			}
		}

		internal string ServerVersion
		{
			get
			{
				return m_serverVersion;
			}
			set
			{
				m_serverVersion = value;
			}
		}

		internal int Serials
		{
			get
			{
				return m_serials;
			}
			set
			{
				m_serials = value;
			}
		}

		internal int Encrypt
		{
			get
			{
				return m_encrypt;
			}
			set
			{
				m_encrypt = value;
			}
		}

		internal bool Commited
		{
			get
			{
				return m_commited;
			}
			set
			{
				m_commited = value;
			}
		}

		internal byte C2p
		{
			get
			{
				return m_c2p;
			}
			set
			{
				m_c2p = value;
			}
		}

		internal byte BackslashEsc
		{
			get
			{
				return m_backslashEsc;
			}
			set
			{
				m_backslashEsc = value;
			}
		}

		internal int MaxSession
		{
			get
			{
				return m_MaxSession;
			}
			set
			{
				m_MaxSession = value;
			}
		}

		internal byte AccessMode
		{
			get
			{
				return m_accessMode;
			}
			set
			{
				m_accessMode = value;
			}
		}

		internal int SessClt => m_sessClt;

		internal IsolationLevel IsolationLevel
		{
			get
			{
				return m_ISOlvl;
			}
			set
			{
				m_ISOlvl = value;
			}
		}

		internal bool AutoCommit
		{
			get
			{
				if (!m_AutoCommit.HasValue)
				{
					m_AutoCommit = Convert.ToBoolean(property[DmConst.PROP_KEY_AUTO_COMMIT]);
				}
				return m_AutoCommit.Value;
			}
			set
			{
				m_AutoCommit = value;
			}
		}

		internal bool AlwaysAllowAutoCommit => Convert.ToBoolean(property[DmConst.PROP_KEY_ALWAYS_ALLOW_COMMIT]);

		internal int BatchType => Convert.ToInt32(property[DmConst.PROP_KEY_BATCH_TYPE]);

		internal int BatchAllowMaxErrors => Convert.ToInt32(property[DmConst.PROP_KEY_BATCH_ALLOW_MAX_ERRORS]);

		internal bool BatchContinueOnError => Convert.ToBoolean(property[DmConst.PROP_KEY_BATCH_CONTINUE_ON_ERROR]);

		internal bool BatchNotOnCall => Convert.ToBoolean(property[DmConst.PROP_KEY_BATCH_NOT_ON_CALL]);

		internal int BufPrefetch
		{
			get
			{
				return Convert.ToInt32(property[DmConst.PROP_KEY_BUF_PREFETCH]);
			}
			set
			{
				property[DmConst.PROP_KEY_BUF_PREFETCH] = value;
			}
		}

		internal bool ClobAsString => Convert.ToBoolean(property[DmConst.PROP_KEY_CLOB_AS_STRING]);

		internal bool ColumnNameUpperCase => Convert.ToBoolean(property[DmConst.PROP_KEY_COLUMN_NAME_UPPERCASE]);

		internal ColumnNameCase ColumnNameCase => (ColumnNameCase)Convert.ToInt32(property[DmConst.PROP_KEY_COLUMN_NAME_CASE]);

		internal string DatabaseProductName => Convert.ToString(property[DmConst.PROP_KEY_DATABASE_PRODUCT_NAME]);

		internal long MaxRows => Convert.ToInt64(property[DmConst.PROP_KEY_MAX_ROWS]);

		internal bool LoginEncrypt => Convert.ToBoolean(property[DmConst.PROP_KEY_LOGIN_ENCRYPT]);

		internal string CipherPath => Convert.ToString(property[DmConst.PROP_KEY_CIPHER_PATH]);

		internal string CurrentSchema
		{
			get
			{
				return m_CurSchema;
			}
			set
			{
				m_CurSchema = value;
			}
		}

		internal short DbTimeZone
		{
			get
			{
				return m_DbTimeZone;
			}
			set
			{
				m_DbTimeZone = value;
			}
		}

		internal int MaxRowSize
		{
			get
			{
				return m_MaxRowSize;
			}
			set
			{
				m_MaxRowSize = value;
			}
		}

		internal int DDLAutoCommit
		{
			get
			{
				return m_DDLAutoCommit;
			}
			set
			{
				m_DDLAutoCommit = value;
			}
		}

		internal bool CaseSensitive
		{
			get
			{
				return m_CaseSensitive;
			}
			set
			{
				m_CaseSensitive = value;
			}
		}

		internal int FailedAttempts
		{
			get
			{
				return m_FailedAttempts;
			}
			set
			{
				m_FailedAttempts = value;
			}
		}

		internal string LastLoginIP
		{
			get
			{
				return m_LastLoginIP;
			}
			set
			{
				m_LastLoginIP = value;
			}
		}

		internal string LastLoginTime
		{
			get
			{
				return m_LastLoginTime;
			}
			set
			{
				m_LastLoginTime = value;
			}
		}

		internal int LoginWarningID
		{
			get
			{
				return m_LoginWarningID;
			}
			set
			{
				m_LoginWarningID = value;
			}
		}

		internal int GracetimeRemainder
		{
			get
			{
				return m_GracetimeRemainder;
			}
			set
			{
				m_GracetimeRemainder = value;
			}
		}

		internal string Guid
		{
			get
			{
				return m_guid;
			}
			set
			{
				m_guid = value;
			}
		}

		internal bool RWStandby
		{
			get
			{
				return rwStandby;
			}
			set
			{
				rwStandby = value;
			}
		}

		internal int SvrMode
		{
			get
			{
				return svrMode;
			}
			set
			{
				svrMode = value;
			}
		}

		internal int SvrStat
		{
			get
			{
				return svrStat;
			}
			set
			{
				svrStat = value;
			}
		}

		internal bool DscControl
		{
			get
			{
				return dscControl;
			}
			set
			{
				dscControl = value;
			}
		}

		private void AdjustProperty()
		{
			if (RwSeparate)
			{
				LoginPrimary = LoginModeFlag.onlyprimary;
				ConnPooling = false;
			}
		}

		static DmConnProperty()
		{
			encodingMap = new Dictionary<string, Encoding>();
			if (encodingMap.Count != 0)
			{
				return;
			}
			lock (encodingMap)
			{
				if (encodingMap.Count == 0)
				{
					Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
					encodingMap.Add("default", Encoding.Default);
					encodingMap.Add("utf-8", Encoding.GetEncoding("utf-8"));
					encodingMap.Add("UTF-8", Encoding.GetEncoding("UTF-8"));
					encodingMap.Add("gb18030", Encoding.GetEncoding("gb18030"));
					encodingMap.Add("GB18030", Encoding.GetEncoding("GB18030"));
				}
			}
		}

		internal DmConnProperty(string connectionstring)
			: this()
		{
			ConnectionString = connectionstring;
		}

		internal DmConnProperty()
		{
			builder = new DmConnectionStringBuilder();
			property = new Dictionary<string, object>(builder.setProperty, StringComparer.OrdinalIgnoreCase);
		}

		internal void ClearAutoCommit()
		{
			m_AutoCommit = null;
		}
	}
}
