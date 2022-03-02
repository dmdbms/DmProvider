using System;
using System.Collections.Generic;
using System.IO;

namespace Dm.Config
{
	internal class DmSvcConfig
	{
		internal static LogLevel logLevel;

		internal static string logDir;

		internal static int logSize;

		internal static int dbAliveCheckFreq;

		internal static int dbAliveCheckTimeout;

		private static List<DmOption> defaultOptions;

		public Dictionary<string, Dictionary<string, object>> propertyDictionary = new Dictionary<string, Dictionary<string, object>>(8, StringComparer.OrdinalIgnoreCase);

		private Dictionary<string, object> defaultProperty = new Dictionary<string, object>(64, StringComparer.OrdinalIgnoreCase);

		static DmSvcConfig()
		{
			logLevel = DmOptionHelper.logleveldef;
			logDir = DmOptionHelper.logdirdef;
			logSize = DmOptionHelper.logSizedef;
			dbAliveCheckFreq = DmOptionHelper.dbAliveCheckFreqDef;
			dbAliveCheckTimeout = DmOptionHelper.dbAliveCheckTimeoutDef;
			defaultOptions = new List<DmOption>();
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_EP_GROUP, typeof(EPGroup), null, null, null, 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_TIMEZONE, typeof(long), maxvalue: 840L, minvalue: -779L, defaultvalue: DmOptionHelper.timezonedef));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_LANGUAGE, defaultvalue: DmOptionHelper.languagedef, basetype: typeof(SupportedLanguage), syn: null, maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_COMPRESS, syn: DmConst.PROP_KEYSYN_COMPRESS, defaultvalue: DmOptionHelper.compressDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_COMPRESS_ID, syn: DmConst.PROP_KEYSYN_COMPRESS_ID, defaultvalue: DmOptionHelper.compressIdDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_LOGIN_ENCRYPT, syn: DmConst.PROP_KEYSYN_LOGIN_ENCRYPT, defaultvalue: DmOptionHelper.loginEncryptDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_CIPHER_PATH, syn: DmConst.PROP_KEYSYN_CIPHER_PATH, defaultvalue: DmOptionHelper.cipherPathDef, basetype: typeof(string), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_DIRECT, defaultvalue: DmOptionHelper.directDef, basetype: typeof(bool), syn: null, maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_KEYWORDS, syn: DmConst.PROP_KEYSYN_KEYWORDS, defaultvalue: DmOptionHelper.keyWordsDef, basetype: typeof(string), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_ENABLE_RS_CACHE, syn: DmConst.PROP_KEYSYN_ENABLE_RS_CACHE, defaultvalue: DmOptionHelper.enableRsCacheDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_RS_CACHE_SIZE, syn: DmConst.PROP_KEYSYN_RS_CACHE_SIZE, defaultvalue: DmOptionHelper.rsCacheSizeDef, basetype: typeof(long), maxvalue: 65535L, minvalue: 1L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_RS_REFRESH_FREQ, syn: DmConst.PROP_KEYSYN_RS_REFRESH_FREQ, defaultvalue: DmOptionHelper.rsRefreshFreqDef, basetype: typeof(long), maxvalue: 10000L, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_LOGIN_MODE, syn: DmConst.PROP_KEYSYN_LOGIN_MODE, defaultvalue: DmOptionHelper.login_modedef, basetype: typeof(LoginModeFlag), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_RW_SEPARATE, syn: DmConst.PROP_KEYSYN_RW_SEPARATE, defaultvalue: DmOptionHelper.rwSeparateDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_RW_PERCENT, syn: DmConst.PROP_KEYSYN_RW_PERCENT, defaultvalue: DmOptionHelper.rwPercentDef, basetype: typeof(long), maxvalue: 100L, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_LOG_LEVEL, syn: DmConst.PROP_KEYSYN_LOG_LEVEL, basetype: typeof(LogLevel), defaultvalue: DmOptionHelper.logleveldef, maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_LOG_DIR, syn: DmConst.PROP_KEYSYN_LOG_DIR, basetype: typeof(string), defaultvalue: DmOptionHelper.logdirdef, maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_LOG_SIZE, syn: DmConst.PROP_KEYSYN_LOG_SIZE, basetype: typeof(long), defaultvalue: DmOptionHelper.logSizedef, maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_SERVER, syn: DmConst.PROP_KEYSYN_SERVER, defaultvalue: DmOptionHelper.serverDef, basetype: typeof(string), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_USER, syn: DmConst.PROP_KEYSYN_USER, defaultvalue: DmOptionHelper.userDef, basetype: typeof(string), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_PASSWORD, syn: DmConst.PROP_KEYSYN_PASSWORD, defaultvalue: DmOptionHelper.passwordDef, basetype: typeof(string), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_PORT, defaultvalue: DmOptionHelper.portDef, basetype: typeof(long), syn: null, maxvalue: 65535L, minvalue: 1L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_ENCODING, syn: DmConst.PROP_KEYSYN_ENCODING, defaultvalue: DmOptionHelper.encodingDef, basetype: typeof(string), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_ENLIST, syn: DmConst.PROP_KEYSYN_ENLIST, defaultvalue: DmOptionHelper.enlistDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_CONNECTION_TIMEOUT, syn: DmConst.PROP_KEYSYN_CONNECTION_TIMEOUT, defaultvalue: DmOptionHelper.connectionTimeoutDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_COMMAND_TIMEOUT, syn: DmConst.PROP_KEYSYN_COMMAND_TIMEOUT, defaultvalue: DmOptionHelper.commandTimeoutDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_STMT_POOL_SIZE, syn: DmConst.PROP_KEYSYN_STMT_POOL_SIZE, defaultvalue: DmOptionHelper.poolSizeDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_STMT_POOLING, syn: DmConst.PROP_KEYSYN_STMT_POOLING, defaultvalue: DmOptionHelper.stmtPoolingDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_PSTMT_POOLING, syn: DmConst.PROP_KEYSYN_PSTMT_POOLING, defaultvalue: DmOptionHelper.preparePoolingDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_PSTMT_POOL_SIZE, syn: DmConst.PROP_KEYSYN_PSTMT_POOL_SIZE, defaultvalue: DmOptionHelper.preparePoolSizeDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_CONN_POOLING, syn: DmConst.PROP_KEYSYN_CONN_POOLING, defaultvalue: DmOptionHelper.connPoolingDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_CONN_POOL_SIZE, syn: DmConst.PROP_KEYSYN_CONN_POOL_SIZE, defaultvalue: DmOptionHelper.connPoolSizeDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_CONN_POOL_CHECK, syn: DmConst.PROP_KEYSYN_CONN_POOL_CHECK, defaultvalue: DmOptionHelper.connPoolCheckDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_ESCAPE_PROCESS, syn: DmConst.PROP_KEYSYN_ESCAPE_PROCESS, defaultvalue: DmOptionHelper.escapeProcessDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_SCHEMA, defaultvalue: DmOptionHelper.schemaDef, basetype: typeof(string), syn: null, maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_APPNAME, syn: DmConst.PROP_KEYSYN_APPNAME, defaultvalue: DmOptionHelper.appnameDef, basetype: typeof(string), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_HOST, defaultvalue: DmOptionHelper.hostDef, basetype: typeof(string), syn: null, maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_OS, syn: DmConst.PROP_KEYSYN_OS, defaultvalue: DmOptionHelper.osDef, basetype: typeof(string), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_LOB_MODE, syn: DmConst.PROP_KEYSYN_LOB_MODE, defaultvalue: DmOptionHelper.lobModeDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_AUTO_COMMIT, syn: DmConst.PROP_KEYSYN_AUTO_COMMIT, defaultvalue: DmOptionHelper.autoCommitDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_ALWAYS_ALLOW_COMMIT, syn: DmConst.PROP_KEYSYN_ALWAYS_ALLOW_COMMIT, defaultvalue: DmOptionHelper.alwaysAllowCommitDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_BATCH_TYPE, syn: DmConst.PROP_KEYSYN_BATCH_TYPE, defaultvalue: DmOptionHelper.batchTypeDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_BATCH_ALLOW_MAX_ERRORS, syn: DmConst.PROP_KEYSYN_BATCH_ALLOW_MAX_ERRORS, defaultvalue: DmOptionHelper.batchAllowMaxErrorsDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_BATCH_CONTINUE_ON_ERROR, syn: DmConst.PROP_KEYSYN_BATCH_CONTINUE_ON_ERROR, defaultvalue: DmOptionHelper.batchContinueOnErrorDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_BATCH_NOT_ON_CALL, syn: DmConst.PROP_KEYSYN_BATCH_NOT_ON_CALL, defaultvalue: DmOptionHelper.batchNotOnCallDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_BUF_PREFETCH, syn: DmConst.PROP_KEYSYN_BUF_PREFETCH, defaultvalue: DmOptionHelper.bufPrefetchDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_CLOB_AS_STRING, syn: DmConst.PROP_KEYSYN_CLOB_AS_STRING, defaultvalue: DmOptionHelper.clobAsStringDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_COLUMN_NAME_UPPERCASE, syn: DmConst.PROP_KEYSYN_COLUMN_NAME_UPPERCASE, defaultvalue: DmOptionHelper.columnNameUpperCaseDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_COLUMN_NAME_CASE, syn: DmConst.PROP_KEYSYN_COLUMN_NAME_CASE, defaultvalue: DmOptionHelper.columnNameCaseDef, basetype: typeof(ColumnNameCase), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_DATABASE_PRODUCT_NAME, syn: DmConst.PROP_KEYSYN_DATABASE_PRODUCT_NAME, defaultvalue: DmOptionHelper.databaseProductNameDef, basetype: typeof(string), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_COMPATIBLE_MODE, syn: DmConst.PROP_KEYSYN_COMPATIBLE_MODE, defaultvalue: DmOptionHelper.compatibleModeDef, basetype: typeof(CompatibleMode), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_IGNORE_CASE, syn: DmConst.PROP_KEYSYN_IGNORE_CASE, defaultvalue: DmOptionHelper.ignoreCaseDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_IS_BDTA_RS, syn: DmConst.PROP_KEYSYN_IS_BDTA_RS, defaultvalue: DmOptionHelper.isBdtaRsDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_MAX_ROWS, syn: DmConst.PROP_KEYSYN_MAX_ROWS, defaultvalue: DmOptionHelper.maxRowsDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_SOCKET_TIMEOUT, syn: DmConst.PROP_KEYSYN_SOCKET_TIMEOUT, defaultvalue: DmOptionHelper.socketTimeoutDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_ADDRESS_REMAP, syn: DmConst.PROP_KEYSYN_ADDRESS_REMAP, defaultvalue: DmOptionHelper.addressRemapDef, basetype: typeof(string), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_USER_REMAP, syn: DmConst.PROP_KEYSYN_USER_REMAP, defaultvalue: DmOptionHelper.userRemapDef, basetype: typeof(string), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_EP_SELECTOR, syn: DmConst.PROP_KEYSYN_EP_SELECTOR, defaultvalue: DmOptionHelper.epSelectorDef, basetype: typeof(EpSelector), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_SWITCH_TIMES, syn: DmConst.PROP_KEYSYN_SWITCH_TIMES, defaultvalue: DmOptionHelper.switchTimesDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_SWITCH_INTERVAL, syn: DmConst.PROP_KEYSYN_SWITCH_INTERVAL, defaultvalue: DmOptionHelper.switchIntervalDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_LOGIN_STATUS, syn: DmConst.PROP_KEYSYN_LOGIN_STATUS, defaultvalue: DmOptionHelper.loginStatusDef, basetype: typeof(LoginStatus), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_LOGIN_DSC_CTRL, syn: DmConst.PROP_KEYSYN_LOGIN_DSC_CTRL, defaultvalue: DmOptionHelper.loginDscCtrlDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_RW_STANDBY_RECOVER_TIME, syn: DmConst.PROP_KEYSYN_RW_STANDBY_RECOVER_TIME, defaultvalue: DmOptionHelper.rwStandbyRecoverTimeDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_RW_HA, syn: DmConst.PROP_KEYSYN_RW_HA, defaultvalue: DmOptionHelper.rwHADef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_RW_AUTO_DISTRIBUTE, syn: DmConst.PROP_KEYSYN_RW_AUTO_DISTRIBUTE, defaultvalue: DmOptionHelper.rwAutoDistributeDef, basetype: typeof(bool), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_RW_FILTER_TYPE, syn: DmConst.PROP_KEYSYN_RW_FILTER_TYPE, defaultvalue: DmOptionHelper.rwFilterTypeDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_DO_SWITCH, syn: DmConst.PROP_KEYSYN_DO_SWITCH, defaultvalue: DmOptionHelper.doSwitchDef, basetype: typeof(DoSwitch), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_CLUSTER, defaultvalue: DmOptionHelper.clusterDef, basetype: typeof(CLUSTER), syn: null, maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_DB_ALIVE_CHECK_FREQ, syn: DmConst.PROP_KEYSYN_DB_ALIVE_CHECK_FREQ, defaultvalue: DmOptionHelper.dbAliveCheckFreqDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
			defaultOptions.Add(new DmOption(DmConst.PROP_KEY_DB_ALIVE_CHECK_TIMEOUT, syn: DmConst.PROP_KEYSYN_DB_ALIVE_CHECK_TIMEOUT, defaultvalue: DmOptionHelper.dbAliveCheckTimeoutDef, basetype: typeof(long), maxvalue: null, minvalue: 0L));
		}

		internal DmSvcConfig(string filename)
		{
			foreach (DmOption defaultOption in defaultOptions)
			{
				DmOptionHelper.SetProperty(defaultOption, defaultOption.Defaultvalue, defaultProperty);
			}
			propertyDictionary.Add("defaultproperty", defaultProperty);
			ParseConfigFile(filename);
		}

		private void UpdateOptionsDictionary(string key, object value, Dictionary<string, object> property)
		{
			DmOption option = DmOptionHelper.GetOption(key, defaultOptions);
			if (option != null)
			{
				object obj = option.ValidateValue(value);
				DmOptionHelper.SetProperty(option, obj, property);
				if (key.Equals(DmConst.PROP_KEY_LOG_LEVEL, StringComparison.OrdinalIgnoreCase) || key.Equals(DmConst.PROP_KEYSYN_LOG_LEVEL, StringComparison.OrdinalIgnoreCase))
				{
					logLevel = (LogLevel)obj;
				}
				else if (key.Equals(DmConst.PROP_KEY_LOG_DIR, StringComparison.OrdinalIgnoreCase) || key.Equals(DmConst.PROP_KEYSYN_LOG_DIR, StringComparison.OrdinalIgnoreCase))
				{
					logDir = Convert.ToString(obj);
				}
				else if (key.Equals(DmConst.PROP_KEY_LOG_SIZE, StringComparison.OrdinalIgnoreCase) || key.Equals(DmConst.PROP_KEYSYN_LOG_SIZE, StringComparison.OrdinalIgnoreCase))
				{
					logSize = Convert.ToInt32(obj);
				}
				else if (key.Equals(DmConst.PROP_KEY_DB_ALIVE_CHECK_FREQ, StringComparison.OrdinalIgnoreCase) || key.Equals(DmConst.PROP_KEYSYN_DB_ALIVE_CHECK_FREQ, StringComparison.OrdinalIgnoreCase))
				{
					dbAliveCheckFreq = Convert.ToInt32(obj);
				}
				else if (key.Equals(DmConst.PROP_KEY_DB_ALIVE_CHECK_TIMEOUT, StringComparison.OrdinalIgnoreCase) || key.Equals(DmConst.PROP_KEYSYN_DB_ALIVE_CHECK_TIMEOUT, StringComparison.OrdinalIgnoreCase))
				{
					dbAliveCheckTimeout = Convert.ToInt32(obj);
				}
			}
		}

		private void ParseConfigFile(string filename)
		{
			try
			{
				using StreamReader streamReader = new StreamReader(filename);
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					if (text.Contains("#"))
					{
						text = text.Substring(0, text.IndexOf("#"));
					}
					string[] array = text.Split('=');
					if (array.Length != 2)
					{
						if (array.Length != 1 || !array[0].Trim().StartsWith("[") || !array[0].Trim().EndsWith("]"))
						{
							continue;
						}
						array[0] = array[0].Trim();
						int length = array[0].Length;
						string key = array[0].Substring(1, length - 2);
						while ((text = streamReader.ReadLine()) != null)
						{
							if (text.Contains("#"))
							{
								text = text.Substring(0, text.IndexOf("#"));
							}
							string[] array2 = text.Split('=');
							if (array2.Length != 2)
							{
								if (array2.Length == 1 && array2[0].Trim().StartsWith("[") && array2[0].Trim().EndsWith("]"))
								{
									array2[0] = array2[0].Trim();
									_ = array2[0].Length;
									key = array2[0].Substring(1, length - 2);
								}
								continue;
							}
							array2[0] = array2[0].Trim();
							array2[1] = array2[1].Trim();
							string[] array3 = Parse(array2[1]);
							if (propertyDictionary.ContainsKey(key))
							{
								UpdateOptionsDictionary(array2[0], array3[0], propertyDictionary[key]);
							}
						}
						continue;
					}
					array[0] = array[0].Trim();
					array[1] = array[1].Trim();
					string[] array4 = Parse(array[1]);
					if (array4 == null || string.IsNullOrEmpty(array4[0].Trim()))
					{
						continue;
					}
					if (array4.Length >= 1 && array4[0].Contains("."))
					{
						propertyDictionary.Add(array[0], new Dictionary<string, object>(defaultProperty, StringComparer.OrdinalIgnoreCase));
						UpdateOptionsDictionary(DmConst.PROP_KEY_EP_GROUP, array4, propertyDictionary[array[0]]);
						continue;
					}
					foreach (string key2 in propertyDictionary.Keys)
					{
						UpdateOptionsDictionary(array[0], array4[0], propertyDictionary[key2]);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private string[] Parse(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return null;
			}
			s = s.Trim();
			int num = s.IndexOf('(');
			int num2 = s.IndexOf(')');
			if (num != -1 && num2 != -1)
			{
				s = s.Substring(num + 1, num2 - num - 1);
				return s.Split(',');
			}
			return null;
		}
	}
}
