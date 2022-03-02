using System.Globalization;
using System.Resources;
using System.Threading;

namespace Dm
{
	internal class DmConst
	{
		internal static string PROP_KEY_EP_GROUP = "ep_group";

		internal static string PROP_KEY_SERVER = "server";

		internal static string PROP_KEYSYN_SERVER = "data source";

		internal static string PROP_KEY_USER = "user";

		internal static string PROP_KEYSYN_USER = "userid,user id,username,user name,uid";

		internal static string PROP_KEY_PASSWORD = "password";

		internal static string PROP_KEYSYN_PASSWORD = "pwd";

		internal static string PROP_KEY_PORT = "port";

		internal static string PROP_KEY_ENCODING = "encoding";

		internal static string PROP_KEYSYN_ENCODING = "sessEncode,sess_encode";

		internal static string PROP_KEY_ENLIST = "enlist";

		internal static string PROP_KEYSYN_ENLIST = "autoenlist";

		internal static string PROP_KEY_CONNECTION_TIMEOUT = "connect_timeout";

		internal static string PROP_KEYSYN_CONNECTION_TIMEOUT = "connect timeout,connectiontimeout,timeout,connectTimeout";

		internal static string PROP_KEY_COMMAND_TIMEOUT = "command_timeout";

		internal static string PROP_KEYSYN_COMMAND_TIMEOUT = "sessionTimeout,commandtimeout,session_timeout";

		internal static string PROP_KEY_STMT_POOL_SIZE = "stmt_pool_size";

		internal static string PROP_KEYSYN_STMT_POOL_SIZE = "poolsize";

		internal static string PROP_KEY_STMT_POOLING = "stmt_pooling";

		internal static string PROP_KEYSYN_STMT_POOLING = "stmtpooling";

		internal static string PROP_KEY_PSTMT_POOLING = "pstmt_pooling";

		internal static string PROP_KEYSYN_PSTMT_POOLING = "preparepooling";

		internal static string PROP_KEY_PSTMT_POOL_SIZE = "pstmt_pool_size";

		internal static string PROP_KEYSYN_PSTMT_POOL_SIZE = "preparepoolsize";

		internal static string PROP_KEY_CONN_POOLING = "conn_pooling";

		internal static string PROP_KEYSYN_CONN_POOLING = "connpooling";

		internal static string PROP_KEY_CONN_POOL_SIZE = "conn_pool_size";

		internal static string PROP_KEYSYN_CONN_POOL_SIZE = "connpoolsize";

		internal static string PROP_KEY_CONN_POOL_CHECK = "conn_pool_check";

		internal static string PROP_KEYSYN_CONN_POOL_CHECK = "connpoolcheck";

		internal static string PROP_KEY_ESCAPE_PROCESS = "escape_process";

		internal static string PROP_KEYSYN_ESCAPE_PROCESS = "escapeProcess";

		internal static string PROP_KEY_TIMEZONE = "time_zone";

		internal static string PROP_KEY_LANGUAGE = "language";

		internal static string PROP_KEY_KEYWORDS = "primary_key";

		internal static string PROP_KEYSYN_KEYWORDS = "keywords";

		internal static string PROP_KEY_LOGIN_MODE = "loginMode";

		internal static string PROP_KEYSYN_LOGIN_MODE = "login_mode";

		internal static string PROP_KEY_SCHEMA = "schema";

		internal static string PROP_KEY_APPNAME = "app_name";

		internal static string PROP_KEYSYN_APPNAME = "appname";

		internal static string PROP_KEY_DATABASE = "database";

		internal static string PROP_KEY_HOST = "host";

		internal static string PROP_KEY_OS = "os_name";

		internal static string PROP_KEYSYN_OS = "os";

		internal static string PROP_KEY_DM_SVC_PATH = "svc_conf_path";

		internal static string PROP_KEYSYN_DM_SVC_PATH = "dm_svc_conf";

		internal static string PROP_KEY_LOG_LEVEL = "loglevel";

		internal static string PROP_KEYSYN_LOG_LEVEL = "log_level";

		internal static string PROP_KEY_LOG_DIR = "logdir";

		internal static string PROP_KEYSYN_LOG_DIR = "log_dir";

		internal static string PROP_KEY_LOG_SIZE = "logsize";

		internal static string PROP_KEYSYN_LOG_SIZE = "log_size";

		internal static string PROP_KEY_RW_SEPARATE = "rw_separate";

		internal static string PROP_KEYSYN_RW_SEPARATE = "rwSeparate";

		internal static string PROP_KEY_RW_PERCENT = "rw_percent";

		internal static string PROP_KEYSYN_RW_PERCENT = "rwPercent";

		internal static string PROP_KEY_COMPRESS = "compress";

		internal static string PROP_KEYSYN_COMPRESS = "compress_msg";

		internal static string PROP_KEY_COMPRESS_ID = "compress_id";

		internal static string PROP_KEYSYN_COMPRESS_ID = "compressId";

		internal static string PROP_KEY_LOGIN_ENCRYPT = "login_encrypt";

		internal static string PROP_KEYSYN_LOGIN_ENCRYPT = "loginEncrypt, communication_encrypt";

		internal static string PROP_KEY_CIPHER_PATH = "cipher_path";

		internal static string PROP_KEYSYN_CIPHER_PATH = "cipherPath";

		internal static string PROP_KEY_DIRECT = "direct";

		internal static string PROP_KEY_ENABLE_RS_CACHE = "enRsCache";

		internal static string PROP_KEYSYN_ENABLE_RS_CACHE = "en_rs_cache,enable_rs_cache";

		internal static string PROP_KEY_RS_CACHE_SIZE = "rsCacheSize";

		internal static string PROP_KEYSYN_RS_CACHE_SIZE = "rs_cache_size";

		internal static string PROP_KEY_RS_REFRESH_FREQ = "rsRefreshFreq";

		internal static string PROP_KEYSYN_RS_REFRESH_FREQ = "rs_refresh_freq";

		internal static string PROP_KEY_LOB_MODE = "lobMode";

		internal static string PROP_KEYSYN_LOB_MODE = "lob_mode";

		internal static string PROP_KEY_AUTO_COMMIT = "autoCommit";

		internal static string PROP_KEYSYN_AUTO_COMMIT = "auto_commit";

		internal static string PROP_KEY_ALWAYS_ALLOW_COMMIT = "alwaysAllowCommit";

		internal static string PROP_KEYSYN_ALWAYS_ALLOW_COMMIT = "always_allow_commit";

		internal static string PROP_KEY_BATCH_TYPE = "batchType";

		internal static string PROP_KEYSYN_BATCH_TYPE = "batch_type";

		internal static string PROP_KEY_BATCH_CONTINUE_ON_ERROR = "batchContinueOnError";

		internal static string PROP_KEYSYN_BATCH_CONTINUE_ON_ERROR = "continueBatchOnError,batch_continue_on_error,continue_batch_on_error";

		internal static string PROP_KEY_BATCH_NOT_ON_CALL = "batchNotOnCall";

		internal static string PROP_KEYSYN_BATCH_NOT_ON_CALL = "callBatchNot,call_batch_not,batch_not_on_call";

		internal static string PROP_KEY_BATCH_ALLOW_MAX_ERRORS = "batchAllowMaxErrors";

		internal static string PROP_KEYSYN_BATCH_ALLOW_MAX_ERRORS = "BATCH_ALLOW_MAX_ERRORS";

		internal static string PROP_KEY_BUF_PREFETCH = "bufPrefetch";

		internal static string PROP_KEYSYN_BUF_PREFETCH = "buf_prefetch";

		internal static string PROP_KEY_CLOB_AS_STRING = "clobAsString";

		internal static string PROP_KEYSYN_CLOB_AS_STRING = "clob_as_string";

		internal static string PROP_KEY_COLUMN_NAME_UPPERCASE = "columnNameUpperCase";

		internal static string PROP_KEYSYN_COLUMN_NAME_UPPERCASE = "column_name_upper_Case";

		internal static string PROP_KEY_COLUMN_NAME_CASE = "columnNameCase";

		internal static string PROP_KEYSYN_COLUMN_NAME_CASE = "COLUMN_NAME_CASE";

		internal static string PROP_KEY_DATABASE_PRODUCT_NAME = "databaseProductName";

		internal static string PROP_KEYSYN_DATABASE_PRODUCT_NAME = "database_product_name";

		internal static string PROP_KEY_COMPATIBLE_MODE = "compatibleMode";

		internal static string PROP_KEYSYN_COMPATIBLE_MODE = "COMPATIBLE_MODE";

		internal static string PROP_KEY_IGNORE_CASE = "ignoreCase";

		internal static string PROP_KEYSYN_IGNORE_CASE = "IGNORE_CASE";

		internal static string PROP_KEY_IS_BDTA_RS = "isBdtaRS";

		internal static string PROP_KEYSYN_IS_BDTA_RS = "IS_BDTA_RS";

		internal static string PROP_KEY_MAX_ROWS = "maxRows";

		internal static string PROP_KEYSYN_MAX_ROWS = "MAX_ROWS";

		internal static string PROP_KEY_SOCKET_TIMEOUT = "scoketTimeout";

		internal static string PROP_KEYSYN_SOCKET_TIMEOUT = "socket_timeout";

		internal static string PROP_KEY_ADDRESS_REMAP = "addressRemap";

		internal static string PROP_KEYSYN_ADDRESS_REMAP = "ADDRESS_REMAP";

		internal static string PROP_KEY_USER_REMAP = "userRemap";

		internal static string PROP_KEYSYN_USER_REMAP = "user_remap";

		internal static string PROP_KEY_EP_SELECTOR = "epSelector";

		internal static string PROP_KEYSYN_EP_SELECTOR = "epSelection,EP_SELECTION,EP_SELECTOR";

		internal static string PROP_KEY_SWITCH_TIMES = "switchTimes";

		internal static string PROP_KEYSYN_SWITCH_TIMES = "SWITCH_TIME,SWITCH_TIMES";

		internal static string PROP_KEY_SWITCH_INTERVAL = "switchInterval";

		internal static string PROP_KEYSYN_SWITCH_INTERVAL = "SWITCH_INTERVAL";

		internal static string PROP_KEY_LOGIN_STATUS = "loginStatus";

		internal static string PROP_KEYSYN_LOGIN_STATUS = "LOGIN_STATUS";

		internal static string PROP_KEY_LOGIN_DSC_CTRL = "loginDscCtrl";

		internal static string PROP_KEYSYN_LOGIN_DSC_CTRL = "LOGIN_DSC_CTRL";

		internal static string PROP_KEY_RW_STANDBY_RECOVER_TIME = "rwStandbyRecoverTime";

		internal static string PROP_KEYSYN_RW_STANDBY_RECOVER_TIME = "RW_STANDBY_RECOVER_TIME";

		internal static string PROP_KEY_RW_HA = "rwHA";

		internal static string PROP_KEYSYN_RW_HA = "RW_HA";

		internal static string PROP_KEY_RW_AUTO_DISTRIBUTE = "rwAutoDistribute";

		internal static string PROP_KEYSYN_RW_AUTO_DISTRIBUTE = "rw_Auto_Distribute";

		internal static string PROP_KEY_RW_FILTER_TYPE = "rwFilterType";

		internal static string PROP_KEYSYN_RW_FILTER_TYPE = "RW_FILTER_TYPE";

		internal static string PROP_KEY_DO_SWITCH = "doSwitch";

		internal static string PROP_KEYSYN_DO_SWITCH = "do_Switch,autoReconnect,auto_Reconnect";

		internal static string PROP_KEY_CLUSTER = "cluster";

		internal static string PROP_KEY_DB_ALIVE_CHECK_FREQ = "dbAliveCheckFreq";

		internal static string PROP_KEYSYN_DB_ALIVE_CHECK_FREQ = "DB_ALIVE_CHECK_FREQ";

		internal static string PROP_KEY_DB_ALIVE_CHECK_TIMEOUT = "dbAliveCheckTimeout";

		internal static string PROP_KEYSYN_DB_ALIVE_CHECK_TIMEOUT = "DB_ALIVE_CHECK_TIMEOUT";

		internal const int SERVER_MODE_NORMAL = 0;

		internal const int SERVER_MODE_PRIMARY = 1;

		internal const int SERVER_MODE_STANDBY = 2;

		internal const int SERVER_STATUS_MOUNT = 3;

		internal const int SERVER_STATUS_OPEN = 4;

		internal const int SERVER_STATUS_SUSPEND = 5;

		internal static int MSG_COMPRESS_THRESHOLD = 8192;

		internal static int MSG_COMPRESS_NO = 0;

		internal static int MSG_COMPRESS_SIMPLE = 1;

		internal static int MSG_COMPRESS_AUTO = 2;

		internal static int MSG_CPR_FUN_ID_ZIP = 0;

		internal static int MSG_CPR_FUN_ID_SNAPPY = 1;

		internal static int MSG_CPR_FUN_ID_NONE = -1;

		public const int version_exec2 = 117506688;

		public const int PROVIDER_ERROR = -1;

		public const int PROVIDER_SUCCESS = 0;

		public const int TRUE = 1;

		public const int FALSE = 0;

		public const int CONNECTION_DEFAULT_TIMEOUT = 15;

		public const int SESS_NETP = 3;

		public const int LOGIN_MPP_GLOBAL = 0;

		public const int LOGIN_MPP_LOCAL = 1;

		public const int LOGIN_NEW_LOB_FLAG = 1;

		public const int XDEC_STORE_LEN = 22;

		public const int PLTYPE_CLASS_PREC_MAGIC = 5;

		public const int EXEC_DIRECT_SQL_TYPE_UNDEF = 0;

		public const int EXEC_DIRECT_SQL_TYPE_INSERT = 1;

		public const int EXEC_DIRECT_SQL_TYPE_UPDATE = 2;

		public const int EXEC_DIRECT_SQL_TYPE_SELECT = 3;

		public const int EXEC_DIRECT_SQL_TYPE_DELETE = 4;

		public const int EXEC_DIRECT_OPT_MAX_PARAM = 600;

		public const int DMTIME_FUN_DATE_YEAR = 1;

		public const int DMTIME_FUN_DATE_MONTH = 2;

		public const int DMTIME_FUN_DATE_DAY = 3;

		public const int DMTIME_FUN_DATE_HOUR = 4;

		public const int DMTIME_FUN_DATE_MIN = 5;

		public const int DMTIME_FUN_DATE_SEC = 6;

		public const int DMTIME_FUN_DATE_MSEC = 7;

		public const int DMTIME_FUN_DATE_QUARTER = 8;

		public const int DMTIME_FUN_DATE_DAYOFY = 9;

		public const int DMTIME_FUN_DATE_WEEK = 10;

		public const int DMTIME_FUN_DATE_WEEKDAY = 11;

		public const int DMTIME_FUN_TZ_HOUR = 12;

		public const int DMTIME_FUN_TZ_MIN = 13;

		public const int DMTIME_FUN_DATE_NULL = 0;

		public const int CLEX_TYPE_NORMAL = 0;

		public const int CLEX_TYPE_INT = 1;

		public const int CLEX_TYPE_BINT = 2;

		public const int CLEX_TYPE_REAL = 3;

		public const int CLEX_TYPE_DECIMAL = 4;

		public const int CLEX_TYPE_CHAR = 5;

		public const int EXEC_DIRECT_SQL_BUF_INC_SZ = 8192;

		public const int EXEC_DIRECT_SQL_BUF_SZ = 65536;

		public const int CHAR_DEFAULT = 8188;

		public const int DEFAULTPORT = 5236;

		public const int NAMELEN = 128;

		public const int DM_PAGE_SIZE = 4096;

		public const int TUPLE_BUF_LEN = 1048576;

		public const int MSG_MAX_LEN = 32640;

		public const int NET_PACKET_SIZE = 8192;

		public const int MAX_METADATA_LEN = 32320;

		public const int MAX_STMT_NUM = 2000;

		public const int MAX_CONNECT_NUM = 256;

		public const int DES_MSG_MAX_ERR_LEN = 512;

		public const int MAXCOLUMNINORDERBY = 64;

		public const int MAXCOLUMNSINGROUPBY = 64;

		public const int DM_MAX_BLOB_LEN_PER_MSG = 32000;

		public const int FLD_TEXT_INROW_MAX_SIZE = 900;

		public const int MAX_PARAM_DATA_LEN = 65535;

		public const int VERSION = 2;

		public const int CMD_LOGIN = 1;

		public const int CMD_LOGOUT = 2;

		public const int CMD_STMT_ALLOCATE = 3;

		public const int CMD_STMT_FREE = 4;

		public const int CMD_PREPARE = 5;

		public const int CMD_EXECUTE = 6;

		public const int CMD_FETCH = 7;

		public const int CMD_COMMIT = 8;

		public const int CMD_ROLLBACK = 9;

		public const int CMD_GET_SQLSATE = 10;

		public const int CMD_CANCLE = 11;

		public const int CMD_POSITION = 12;

		public const int CMD_EXECUTE2 = 13;

		public const int CMD_PUT_DATA = 14;

		public const int CMD_GET_DATA = 15;

		public const int CMD_CREATE_BLOB = 16;

		public const int CMD_CLOSE_STMT = 17;

		public const int CMD_TIME_OUT = 18;

		public const int CMD_CURSOR_PREPARE = 19;

		public const int CMD_CURSOR_EXECUTE = 20;

		public const int CMD_EXPLAIN = 21;

		public const int CMD_CURSOR_SET_NAME = 27;

		public const int CMD_GET_LOB_LEN = 29;

		public const int CMD_SET_LOB_DATA = 30;

		public const int CMD_LOB_TRUNCATE = 31;

		public const int CMD_GET_LOB_DATA = 32;

		public const int CMD_MORE_RESULT = 44;

		public const int CMD_PRE_EXEC = 90;

		public const int CMD_EXEC_DIRECT = 91;

		public const int CMD_SESS_ISO = 52;

		public const int CMD_TABLE_TS = 71;

		public const int CMD_STARTUP = 200;

		public const int RET_BASE = 127;

		public const int RET_DDL_CDB = 128;

		public const int RET_DDL_CTAB = 129;

		public const int RET_DDL_DTAB = 130;

		public const int RET_DDL_CVIEW = 131;

		public const int RET_DDL_DVIEW = 132;

		public const int RET_DDL_CIND = 133;

		public const int RET_DDL_DIND = 134;

		public const int RET_DDL_CUSR = 135;

		public const int RET_DDL_DUSR = 136;

		public const int RET_DDL_CROL = 137;

		public const int RET_DDL_DROL = 138;

		public const int RET_DDL_DROP = 139;

		public const int RET_DDL_ALTDB = 140;

		public const int RET_DDL_ALTUSR = 141;

		public const int RET_DDL_CFUNC = 142;

		public const int RET_DDL_CPROC = 143;

		public const int RET_DDL_GRANT = 144;

		public const int RET_DDL_REVOKE = 145;

		public const int RET_DDL_ALTTAB = 146;

		public const int RET_COMMIT = 147;

		public const int RET_ROLLBACK = 148;

		public const int RET_EXPLAIN = 149;

		public const int RET_SET_TRAN = 150;

		public const int RET_SAVEPNT = 151;

		public const int RET_SET_CURDB = 152;

		public const int RET_SET_CURSCH = 153;

		public const int RET_LOCK_TAB = 154;

		public const int RET_DDL_AUDIT = 155;

		public const int RET_DML_INSERT = 157;

		public const int RET_DML_DELETE = 158;

		public const int RET_DML_UPDATE = 159;

		public const int RET_DML_SELECT = 160;

		public const int RET_DML_SELECT_INTO = 161;

		public const int RET_DML_CALL = 162;

		public const int RET_LOGIN = 163;

		public const int RET_DML_MERGE = 164;

		public const int RET_SET_TIME_ZONE = 165;

		public const int RET_SET_SESS_TRAN = 166;

		public const int RET_FETCH_NOT_OVER = 167;

		public const int RET_EXEC_PROC = 178;

		public const int RET_SIMPLE = 187;

		public const int RET_DDL_CSCHEMA = 188;

		public const int RET_PLAN_ERRCODE = 189;

		public const int RET_DDL_ALTTRIG = 190;

		public const int RET_DDL_DROP_TABLE = 191;

		public const int RET_DDL_DROP_INDEX = 192;

		public const int RET_DDL_DROP_VIEW = 193;

		public const int RET_DML_TRCT = 194;

		public const int RET_SET_IDENTINS = 195;

		public const int RET_DDL_CSEQ = 196;

		public const int RET_DML_CURSOR_DECLARE = 197;

		public const int RET_DML_CURSOR_OPEN = 198;

		public const int RET_DML_CURSOR_CLOSE = 199;

		public const int RET_DML_CURSOR_UPDATE = 200;

		public const int RET_DML_CURSOR_DELETE = 201;

		public const int RET_DML_CURSOR_FETCH = 202;

		public const int RET_DML_CURSOR_SEEK = 203;

		public const int RET_DDL_CLGN = 204;

		public const int RET_DDL_ALTLGN = 205;

		public const int RET_DDL_CCONIND = 206;

		public const int RET_DDL_DCONIND = 207;

		public const int RET_DDL_ALTCONIND = 208;

		public const int RET_DDL_CLNK = 209;

		public const int RET_SYNC_DATA = 210;

		public const int RET_SYNC_COMMIT = 211;

		public const int RET_SYNC_ROLLBACK = 212;

		public const int RET_SYNC_RECV_COMMIT = 213;

		public const int RET_SYNC_RECV_ROLLBACK = 214;

		public const int RET_DDL_CPLY = 215;

		public const int RET_DDL_ALTPLY = 216;

		public const int RET_DDL_ALTUSRPLY = 217;

		public const int RET_DDL_ALTTABPLY = 218;

		public const int RET_DDL_CRULE = 219;

		public const int RET_DDL_COPTR = 220;

		public const int RET_DDL_CALT = 221;

		public const int RET_DDL_CJOB = 222;

		public const int RET_DDL_ALTOPTR = 223;

		public const int RET_DDL_ALTALT = 224;

		public const int RET_DDL_ALTJOB = 225;

		public const int RET_DDL_BAKDB = 226;

		public const int RET_DDL_RESDB = 227;

		public const int RET_STARTUP = 228;

		public const int RET_XA_RECV = 229;

		public const int RET_SYNC_SUSP_TRX = 230;

		public const int RET_SYNC_HEUR_COMMIT = 231;

		public const int RET_SYNC_HEUR_ROLLBACK = 232;

		public const int RET_DDL_CPKG = 233;

		public const int RET_DDL_CPKG_BODY = 234;

		public const int RET_DDL_CTYPE = 235;

		public const int RET_DDL_CTYPE_BODY = 236;

		public const int RET_DDL_CSYNONYM = 237;

		public const int RET_DDL_CCRY = 238;

		public const int RET_DDL_ALTCRY = 239;

		public const int RET_DDL_CTS = 240;

		public const int RET_DDL_ALTTS = 241;

		public const int RET_DDL_COMMENT = 242;

		public const int RET_FLDR_CLR = 243;

		public const int RET_DDL_ALTSESS = 244;

		public const int RET_DDL_CDMN = 245;

		public const int RET_DDL_CCHARSET = 246;

		public const int RET_DDL_CLLT = 247;

		public const int RET_DDL_ALTIND = 248;

		public const int RET_DDL_CCONTEXT = 249;

		public const int RET_DDL_STATON = 250;

		public const int RET_DDL_ALTSESS_DATEFMT = 251;

		public const int RET_DDL_ALTSESS_TIMEFMT = 252;

		public const int RET_DDL_ALTSESS_DTFMT = 253;

		public const int RET_DDL_ALTSESS_DTTZFMT = 254;

		public const int RET_DDL_ALTSESS_TIMETZFMT = 255;

		public const int RET_DDL_ALTSESS_DATE_LANG = 256;

		public const int USINT_UNDEFINED = -1;

		public const int BYTE_SIZE = 1;

		public const int USINT_SIZE = 2;

		public const int ULINT_SIZE = 4;

		public const int DMBOOL_SIZE = 4;

		public const int DDWORD_SIZE = 8;

		public const int TIME_SIZE = 12;

		public const int LINT64_SIZE = 8;

		public const int DEC_SIZE = 64;

		public const int IYM_SIZE = 12;

		public const int IDT_SIZE = 24;

		public const int DATA_BLOB_LEN = 18;

		public const int DATA_TEXT_LEN = 18;

		public const long RESULTSET_ROW_NUMBER_UNKNONW = long.MaxValue;

		public const int SESS3_CLT_UNKNOWN = -1;

		public const int SESS3_CLT_ODBC = 0;

		public const int SESS3_CLT_SQL3 = 1;

		public const int SESS3_CLT_JDBC = 0;

		public const int MSG_LOGIN_USERNANE = 0;

		public const int MSG_LOGIN_PSWD = 129;

		public const int MSG_LOGIN_FLAG = 258;

		public const int MSG_LOGIN_SIZE = 262;

		public const int MSG_POSITION_SIZE = 8;

		public const int MSG_DSE_ITEM_TYPE_DTYPE = 0;

		public const int MSG_DSE_ITEM_TYPE_PREC = 4;

		public const int MSG_DSE_ITEM_TYPE_SCALE = 8;

		public const int MSG_DES_ITEM_NULLABLE = 12;

		public const int MSG_DES_ITEM_FLAGS = 16;

		public const int MSG_DES_ITEM_UNNAMED = 18;

		public const int MSG_DES_ITEM_IO_TYPE = 22;

		public const int MSG_DES_ITEM_NAME_LEN = 24;

		public const int MSG_DES_ITEM_TYPE_NAME_LEN = 26;

		public const int MSG_DES_ITEM_TABLE_NAME_LEN = 28;

		public const int MSG_DES_ITEM_SCHEMA_NAME_LEN = 30;

		public const int MSG_DES_ITEM_FIX_SIZE = 32;

		public const int REC3_FLD_LEN = 0;

		public const int REC3_FLD_FLAG = 2;

		public const int REC3_FLD_N_FIELDS = 3;

		public const int REC3_EXTRA_SIZE = 5;

		public const int REC3_DELETE_MASK = 128;

		public const int REC3_ROWID = 7;

		public const int MSG_PARA_TAG = 1;

		public const int MSG_OVER = 1;

		public const int MSG_BEGIN = 2;

		public const int DATA_NULL_LEN = -2;

		public const int DM_PARA_IO_IN = 0;

		public const int DM_PARA_IO_OUT = 1;

		public const int DM_PARA_IO_INOUT = 2;

		public const int MSG_SELBUF_COLNUM = 4;

		public const int MSG_SELBUF_SIZE = 8;

		public const int REC3_HEADER_SIZE = 7;

		public const int REC3_1BYTE_HIGHEST = 128;

		public const int REC3_HIGHEST_OFFSET = 32768;

		public const int REC3_SQL_NULL = -2;

		public const int REC3_1BYTE_LEN_MASK = 64;

		public const int DM_FETCH_TYPE_CUR_SET_END = 0;

		public const int DM_FETCH_TYPE_CUR_SET_START = 1;

		public const int DM_FETCH_TYPE_CUR_SET_MID = 2;

		public const int NORMAL_TYPE = 0;

		public const int INPUT_STREAM_TYPE = 1;

		public const int READER_TYPE = 2;

		public const int BLOB_TYPE = 3;

		public const int CLOB_TYPE = 4;

		public const int IS_BLOB = 0;

		public const int IS_CLOB = 1;

		public const int IS_STRING = 2;

		public const int IS_BYTE = 3;

		public const int LOCK_TABLE_X = 0;

		public const int LOCK_TABLE_S = 1;

		public const int ISO_LEVEL_READ_UNCOMMITTED = 0;

		public const int ISO_LEVEL_READ_COMMITTED = 1;

		public const int ISO_LEVEL_SERIALIZABLE = 3;

		public const byte FLD_TEXT_IN_ROW = 1;

		public const byte FLD_TEXT_OFF_ROW = 2;

		public static ResourceManager res = new ResourceManager(typeof(DmErrorDefinition));

		public static CultureInfo ci = Thread.CurrentThread.CurrentCulture;

		public static CultureInfo invariantCulture = CultureInfo.InvariantCulture;

		public const int MSG_PREPARE_ANY = 0;

		public const int MSG_PREPARE_QUERY = 1;

		public const int MSG_PREPARE_UPDATE = 2;

		public const int MSG_PREPARE_CALL = 3;

		public const int MSG_PREPARE_SET_DB = 4;

		public const int MAX_ERR_LEN = 256;

		public const short MSG_HEAD_CMD_MSK = 4095;

		public const short MSG_ENCRYPT_NONE = 0;

		public const short MSG_ENCRYPT_SIMPLE = 4096;

		public const int NET_COMM_ENCRYPT_NONE_TYPE = 0;

		public const int NET_COMM_ENCRYPT_SIMPLE_TYPE = 1;

		public const int NET_COMM_ENCRYPT_SSL_TYPE = 2;

		public const int DH_KEY_LENGTH = 64;

		public const string SCH_TABLE_COLUMN_NAME = "ColumnName";

		public const string SCH_TABLE_COLUMN_ORDINALE = "ColumnOrdinal";

		public const string SCH_TABLE_COLUMN_SIZE = "ColumnSize";

		public const string SCH_TABLE_COLUMN_NUMERIC_PREC = "NumericPrecision";

		public const string SCH_TABLE_COLUMN_NUMERIC_SCALE = "NumericScale";

		public const string SCH_TABLE_COLUMN_ISUNIQUE = "IsUnique";

		public const string SCH_TABLE_COLUMN_ISKEYCOLUMN = "IsKey";

		public const string SCH_TABLE_COLUMN_BASESERVERNAME = "BaseServerName";

		public const string SCH_TABLE_COLUMN_BASECATALOGNAME = "BaseCatalogName";

		public const string SCH_TABLE_COLUMN_BASESCHEMANAME = "BaseSchemaName";

		public const string SCH_TABLE_COLUMN_BASETABLENAME = "BaseTableName";

		public const string SCH_TABLE_COLUMN_BASECOLUMNNAME = "BaseColumnName";

		public const string SCH_TABLE_COLUMN_DATATYPE = "DataType";

		public const string SCH_TABLE_COLUMN_ALLOWDBNULL = "AllowDBNull";

		public const string SCH_TABLE_COLUMN_PROVIDERTYPE = "ProviderType";

		public const string SCH_TABLE_COLUMN_ISALIASED = "IsAliased";

		public const string SCH_TABLE_COLUMN_ISEXPRESSION = "IsExpression";

		public const string SCH_TABLE_COLUMN_ISIDENTITY = "IsIdentity";

		public const string SCH_TABLE_COLUMN_ISAUTOINCREMENT = "IsAutoIncrement";

		public const string SCH_TABLE_COLUMN_ISROWVERSION = "IsRowVersion";

		public const string SCH_TABLE_COLUMN_ISHIDDEN = "IsHidden";

		public const string SCH_TABLE_COLUMN_ISLONG = "IsLong";

		public const string SCH_TABLE_COLUMN_ISREADONLY = "IsReadOnly";

		public const int MAX_LOB_LEN = int.MaxValue;

		public const byte MSG_PRE_RETURN_RSET = 49;

		public const long LINT64_MAX = long.MaxValue;

		public const int LINT_MAX = int.MaxValue;

		public const int BulkBatchSize = 100;

		public const int EC_SUCCESS = 0;

		public const int EC_EMPTY = 100;

		public const int EC_JUMP_STMT = 107;

		public const int EC_RESULT_SET_EMPTY = 111;

		public const int EC_SET_CURDB_SUCCESS = 200;

		public const int EC_SET_TRAN_SUCCESS = 201;

		public const int EC_INVALID_MSG = -6006;

		public const int EC_RN_EXCEED_ROWSET_SIZE = -7036;

		public const int EC_RN_STMT_TIMEOUT = -7049;

		public const short REC4_SQL_NULL = -2;

		public const byte PARAM_TYPE_UNKNOWN = 0;

		public const byte PARAM_TYPE_EXACT = 1;

		public const byte PARAM_TYPE_RECOMMEND = 2;

		public const int ITEM_FLAG_MASK_AUTO_INCREASEMENT = 1;

		public const int ITEM_FLAG_MASK_BLOB_DATA = 2;

		public const int ITEM_FLAG_MASK_READONLY = 4;

		public const int ITEM_FLAG_MASK_RECOMMEND = 8;

		public const int NBLOB_HDR_IN_ROW_FLAG = 0;

		public const int NBLOB_HDR_BLOBID = 1;

		public const int NBLOB_HDR_BLOB_LEN = 9;

		public const int NBLOB_HDR_GROUPID = 13;

		public const int NBLOB_HDR_OUTROW_FILEID = 15;

		public const int NBLOB_HDR_OUTROW_PAGENO = 17;

		public const int NBLOB_EX_HDR_TABLE_ID = 21;

		public const int NBLOB_EX_HDR_COL_ID = 25;

		public const int NBLOB_EX_HDR_ROW_ID = 27;

		public const int NBLOB_EX_HDR_FPA_GRPID = 35;

		public const int NBLOB_EX_HDR_FPA_FILEID = 37;

		public const int NBLOB_EX_HDR_FPA_PAGENO = 39;

		public const int NBLOB_EX_HDR_SIZE = 43;

		public const int NBLOB_OUTROW_HDR_SIZE = 21;

		public const int NBLOB_INROW_HDR_SIZE = 13;

		public const byte BYTE_FLAG = 0;

		public const byte CHAR_FLAG = 1;

		public const int MSG_HEAD_FIX_LEN = 64;

		public const int MSG_LIMIT_LEN = 536870912;

		public const int MSG_REQ_HEAD_RESV_LEN = 9;

		public const int MSG_REQ_HEAD_STMTID = 0;

		public const int MSG_REQ_HEAD_CMD = 4;

		public const int MSG_REQ_HEAD_LEN = 6;

		public const int MSG_REQ_HEAD_RESERVED = 10;

		public const int MSG_REQ_HEAD_CRC = 19;

		public const int MSG_REQ_HEAD_SIZE = 20;

		public const int MSG_RES_HEAD_RESV_LEN = 2;

		public const int MSG_RES_HEAD_STMTID = 0;

		public const int MSG_RES_HEAD_RET = 4;

		public const int MSG_RES_HEAD_LEN = 6;

		public const int MSG_RES_HEAD_SQLCODE = 10;

		public const int MSG_RES_HEAD_SVR_MODE = 14;

		public const int MSG_RES_HEAD_RESERVED = 16;

		public const int MSG_RES_HEAD_COMPRESS = 18;

		public const int MSG_RES_HEAD_CRC = 19;

		public const int MSG_RES_HEAD_SIZE = 20;

		public const int MSG_REQ_ALLOC_STMT_NEW_COL_DESC = 20;

		public const int MSG_RES_ALLOC_STMT_NEW_COL_DESC = 20;

		public const int MSG_REQ_STARTUP_ENCRYPT_TYPE = 20;

		public const int MSG_REQ_STARTUP_CMPRS_MSG = 24;

		public const int MSG_REQ_STARTUP_GEN_KEYPAIR_FLAG = 28;

		public const int MSG_REQ_STARTUP_COMM_ENC_FLAG = 29;

		public const int MSG_REQ_STARTUP_RS_BDTA_FLAG = 30;

		public const int MSG_REQ_STARTUP_MSGCPR_FUN_ID = 31;

		public const int MSG_REQ_STARTUP_FORCE_CERT_ENC = 32;

		public const int MSG_REQ_STARTUP_MARK_NETWORK = 33;

		public const int MSG_REQ_STARTUP_COMM_CRC_FLAG = 34;

		public const int MSG_REQ_STARTUP_PROTOCOL_VERSION = 35;

		public const int MSG_RES_STARTUP_ENCRYPT_TYPE = 20;

		public const int MSG_RES_STARTUP_SERIAL = 24;

		public const int MSG_RES_STARTUP_ENCODING = 28;

		public const int MSG_RES_STARTUP_CMPRS_MSG = 32;

		public const int MSG_RES_STARTUP_SC_FLAG = 36;

		public const int MSG_RES_STARTUP_GEN_KEYPAIR_FLAG = 40;

		public const int MSG_RES_STARTUP_COMM_ENC_FLAG = 41;

		public const int MSG_RES_STARTUP_RS_BDTA_FLAG = 42;

		public const int MSG_RES_STARTUP_MSGCPR_FUN_ID = 43;

		public const int MSG_RES_STARTUP_MARK_NETWORK = 44;

		public const int MSG_RES_STARTUP_COMM_CRC_FLAG = 45;

		public const int MSG_RES_STARTUP_NAME_BIND = 46;

		public const int MSG_RES_STARTUP_PROTOCOL_VERSION = 48;

		public const int MSG_REQ_LOGIN_ENV = 20;

		public const int MSG_REQ_LOGIN_ISO_LEVEL = 24;

		public const int MSG_REQ_LOGIN_LANGUAGE = 28;

		public const int MSG_REQ_LOGIN_READ_ONLY = 32;

		public const int MSG_REQ_LOGIN_TIME_ZONE = 33;

		public const int MSG_REQ_LOGIN_SESS_TIMEOUT = 35;

		public const int MSG_REQ_LOGIN_MPP_TYPE = 39;

		public const int MSG_REQ_LOGIN_REQ_STANDBY = 40;

		public const int MSG_REQ_LOGIN_NEW_LOB_FLAG = 41;

		public const int MSG_RES_LOGIN_MAX_DATA_LEN = 20;

		public const int MSG_RES_LOGIN_MAX_SESSION = 24;

		public const int MSG_RES_LOGIN_DDL_AUTO_CMT = 28;

		public const int MSG_RES_LOGIN_ISO_LEVEL = 29;

		public const int MSG_RES_LOGIN_STR_CASE_SENSITIVE = 33;

		public const int MSG_RES_LOGIN_BACK_SLASH = 34;

		public const int MSG_RES_LOGIN_SVR_MODE = 35;

		public const int MSG_RES_LOGIN_SVR_STAT = 37;

		public const int MSG_RES_LOGIN_C2P = 39;

		public const int MSG_RES_LOGIN_DbTimeZone = 40;

		public const int MSG_RES_LOGIN_RESP_STANDBY = 42;

		public const int MSG_RES_LOGIN_NEW_LOB_FLAG = 43;

		public const int MSG_RES_LOGIN_FETCH_PACK_SZ = 44;

		public const int MSG_RES_LOGIN_LIFETIME_REMAINDER = 48;

		public const int MSG_RES_LOGIN_DSC_CONTROL = 50;

		public const int MSG_REQ_PREPARE_AUTO_CMT = 20;

		public const int MSG_REQ_PREPARE_EXEC_DIRECT = 21;

		public const int MSG_REQ_PREPARE_PARAM_SEQU = 22;

		public const int MSG_REQ_PREPARE_CUR_FORWARD_ONLY = 23;

		public const int MSG_REQ_PREPARE_CHECK_TYPE = 24;

		public const int MSG_REQ_PREPARE_SQL_TYPE = 25;

		public const int MSG_REQ_PREPARE_MAX_ROW_NUM = 27;

		public const int MSG_REQ_PREPARE_RS_BDTA_FLAG = 35;

		public const int MSG_REQ_PREPARE_RS_BDTA_LEN = 36;

		public const int MSG_REQ_PREPARE_RET_ID_FLAG = 38;

		public const int MSG_REQ_PREPARE_DCP_PROBE_MODE = 39;

		public const int MSG_REQ_PREPARE_COLUMN_NEW_DESC = 40;

		public const int MSG_REQ_PREPARE_EXEC_TIMEOUT = 41;

		public const int MSG_RES_PREPARE_RET_TYPE = 20;

		public const int MSG_RES_PREPARE_PARAM_NUM = 22;

		public const int MSG_RES_PREPARE_COL_NUM = 24;

		public const int MSG_RES_PREPARE_TRA_STATUS = 26;

		public const int MSG_REQ_EXECUTE_AUTO_CMT = 20;

		public const int MSG_REQ_EXECUTE_PARAM_NUM = 21;

		public const int MSG_REQ_EXECUTE_CUR_FORWARD_ONLY = 23;

		public const int MSG_REQ_EXECUTE_ROW_NUM = 24;

		public const int MSG_REQ_EXECUTE_CUR_POS = 32;

		public const int MSG_REQ_EXECUTE_MAX_ROWS = 40;

		public const int MSG_REQ_EXECUTE_RET_ID_FLAG = 48;

		public const int MSG_REQ_EXECUTE_IGNORE_BATCH_ERROR = 49;

		public const int MSG_REQ_EXECUTE_DCP_PROBE_MODE = 50;

		public const int MSG_REQ_EXECUTE_COLUMN_NEW_DESC = 51;

		public const int MSG_REQ_EXECUTE_EXEC_TIMEOUT = 52;

		public const int MSG_REQ_EXECUTE_BATCH_MAX_ERRORS = 56;

		public const int MSG_REQ_EXECUTE_EXEC_INNER = 60;

		public const int MSG_RES_EXECUTE_RET_TYPE = 20;

		public const int MSG_RES_EXECUTE_COL_NUM = 22;

		public const int MSG_RES_EXECUTE_ROW_NUM = 24;

		public const int MSG_RES_EXECUTE_PARAM_NUM = 32;

		public const int MSG_RES_EXECUTE_RS_UPDATABLE = 34;

		public const int MSG_RES_EXECUTE_FETCHED_ROWS = 35;

		public const int MSG_RES_EXECUTE_PRINT_OFFSET = 39;

		public const int MSG_RES_EXECUTE_ROWID = 43;

		public const int MSG_RES_EXECUTE_RS_BDTA_FLAG = 43;

		public const int MSG_RES_EXECUTE_RS_ROWID_COLINDEX = 44;

		public const int MSG_RES_EXECUTE_EXECID = 51;

		public const int MSG_RES_RSCACHE_OFFSET = 55;

		public const int MSG_RES_EXECUTE_RET_FLAG = 59;

		public const int MSG_RES_EXECUTE_TRANS_STATUS = 60;

		public const int MSG_REQ_PREEXEC_PARAM_NUM = 20;

		public const int MSG_REQ_PUTDATA_PARA_INDEX = 20;

		public const int MSG_REQ_FETCH_CUR_POS = 20;

		public const int MSG_REQ_FETCH_ROW_COUNT = 28;

		public const int MSG_REQ_FETCH_RES_ID = 36;

		public const int MSG_REQ_FETCH_MAX_MSG_SZ = 38;

		public const int MSG_RES_FETCH_ROW_COUNT = 20;

		public const int MSG_RES_FETCH_RET_COUNT = 28;

		public const int MSG_REQ_MORERES_RES_ID = 20;

		public const int MSG_REQ_BCPSET_IDENTITY = 20;

		public const int MSG_REQ_BCPSET_CHK_CON = 24;

		public const int MSG_REQ_BCPSET_GEN_LOG = 28;

		public const int MSG_REQ_BCPSET_ISORDER = 32;

		public const int MSG_REQ_BCPCLR_WITH_CMT = 20;

		public const int MSG_REQ_SETSESSISO_ISO = 20;

		public const int MSG_REQ_TableTs_ID_COUNT = 20;

		public const int MSG_RES_TableTs_ID_COUNT = 20;

		public const int FLDR_TYPE_BIND = 1;

		public const int FLDR_ATTR_PORT = 4;

		public const int FLDR_ATTR_BAD_FILE = 17;

		public const int FLDR_ATTR_DATA_CHAR_SET = 19;

		public const int FLDR_ATTR_ERRORS_PERMIT = 30;

		public const int FLDR_ATTR_MPP_LOCAL_FLAG = 37;

		public const int FLDR_UNINITILIAZE_COMMIT = 0;

		public const int TRX4_NOT_START = 0;

		public const int TRX4_COMMITTED = 32;

		public const int TRX4_ROLLBACKED = 64;

		public const int TRX4_STATE_MASK = 4095;

		public const int PG_INVALID_CODE = 0;

		public const int PG_UTF8 = 1;

		public const int PG_GBK = 2;

		public const int PG_BIG5 = 3;

		public const int PG_ISO_8859_9 = 4;

		public const int PG_EUC_JP = 5;

		public const int PG_EUC_KR = 6;

		public const int PG_KOI8R = 7;

		public const int PG_ISO_8859_1 = 8;

		public const int PG_SQL_ASCII = 9;

		public const int PG_GB18030 = 10;

		public const int PG_ISO_8859_11 = 11;

		private DmConst()
		{
		}
	}
}
