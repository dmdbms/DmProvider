using System;
using System.Collections.Generic;

namespace Dm.Config
{
	internal class DmOptionHelper
	{
		internal delegate object SetDefaultDelegate();

		internal static SetDefaultDelegate localtimezone = () => Convert.ToInt16( TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalMinutes);

		internal static SetDefaultDelegate defaultsvc_conf = () => Environment.OSVersion.VersionString.Contains("Windows") ? (Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\dm_svc.conf") : "/etc/dm_svc.conf";

		internal static string serverDef = "localhost";

		internal static string userDef = "SYSDBA";

		internal static string passwordDef = "SYSDBA";

		internal static int portDef = 5236;

		internal static string encodingDef = "";

		internal static bool enlistDef = false;

		internal static int connectionTimeoutDef = 5000;

		internal static int commandTimeoutDef = 30;

		internal static int poolSizeDef = 100;

		internal static bool connPoolingDef = false;

		internal static bool stmtPoolingDef = true;

		internal static bool preparePoolingDef = false;

		internal static int preparePoolSizeDef = 100;

		internal static int connPoolSizeDef = 100;

		internal static bool connPoolCheckDef = false;

		internal static bool escapeProcessDef = true;

		internal static string keyWordsDef = null;

		internal static string schemaDef = "";

		internal static string appnameDef = "";

		internal static string osDef = "";

		internal static string initialCatalogDef = "";

		internal static string databaseDef = "";

		internal static string hostDef = "";

		internal static bool loginEncryptDef = false;

		internal static string cipherPathDef = "";

		internal static bool directDef = true;

		internal static bool enableRsCacheDef = false;

		internal static int rsCacheSizeDef = 10;

		internal static int rsRefreshFreqDef = 10;

		internal static bool rwSeparateDef = false;

		internal static int rwPercentDef = 25;

		internal static int compressDef = DmConst.MSG_COMPRESS_NO;

		internal static int compressIdDef = DmConst.MSG_CPR_FUN_ID_ZIP;

		internal static LoginModeFlag login_modedef = LoginModeFlag.normalfirst;

		internal static TraceFlag tracedef = TraceFlag.none;

		internal static SupportedLanguage languagedef = SupportedLanguage.cn;

		internal static int timezonedef = (short)localtimezone();

		internal static string dm_svc_confdef = defaultsvc_conf().ToString();

		internal static LogLevel logleveldef = LogLevel.OFF;

		internal static string logdirdef = Environment.CurrentDirectory;

		internal static int logFlushFreqdef = 5;

		internal static int logSizedef = 104857600;

		internal static int lobModeDef = 1;

		internal static bool autoCommitDef = true;

		internal static bool alwaysAllowCommitDef = true;

		internal static int batchTypeDef = 1;

		internal static int batchAllowMaxErrorsDef = 0;

		internal static bool batchContinueOnErrorDef = false;

		internal static bool batchNotOnCallDef = false;

		internal static int bufPrefetchDef = 0;

		internal static bool clobAsStringDef = false;

		internal static bool columnNameUpperCaseDef = false;

		internal static ColumnNameCase columnNameCaseDef = ColumnNameCase.OFF;

		internal static string databaseProductNameDef = "";

		internal static CompatibleMode compatibleModeDef = CompatibleMode.OFF;

		internal static bool ignoreCaseDef = true;

		internal static bool isBdtaRsDef = false;

		internal static int maxRowsDef = 0;

		internal static int socketTimeoutDef = 0;

		internal static string addressRemapDef = "";

		internal static string userRemapDef = "";

		internal static EpSelector epSelectorDef = EpSelector.WELL_DISTRIBUTE;

		internal static int switchTimesDef = 1;

		internal static int switchIntervalDef = 1000;

		internal static LoginStatus loginStatusDef = LoginStatus.OFF;

		internal static bool loginDscCtrlDef = false;

		internal static int rwStandbyRecoverTimeDef = 60000;

		internal static bool rwHADef = false;

		internal static bool rwAutoDistributeDef = true;

		internal static int rwFilterTypeDef = 2;

		internal static DoSwitch doSwitchDef = DoSwitch.OFF;

		internal static CLUSTER clusterDef = CLUSTER.NORMAL;

		internal static int dbAliveCheckFreqDef = 0;

		internal static int dbAliveCheckTimeoutDef = 10000;

		internal static DmOption GetOption(string keyword, List<DmOption> options)
		{
			if (keyword == null)
			{
				throw new ArgumentNullException(keyword + "is a null reference");
			}
			keyword = keyword.Trim();
			foreach (DmOption option in options)
			{
				if (option.HasKey(keyword))
				{
					return option;
				}
			}
			return null;
		}

		internal static void SetProperty(DmOption option, object value, Dictionary<string, object> property)
		{
			property[option.Keyword] = value;
			if (option.Synonym != null)
			{
				string[] synonym = option.Synonym;
				foreach (string key in synonym)
				{
					property[key] = value;
				}
			}
		}
	}
}
