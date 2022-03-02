namespace Dm
{
	public class DmErrorDefinition
	{
		public static int EC_EMPTY = 100;

		public static int EC_STR_TRUNC_WARN = 101;

		public static int EC_NULL_IN_SFUN = 102;

		public static int EC_INVALID_TABLE_NAME_WARN = 103;

		public static int EC_EMPTY_DEL = 104;

		public static int EC_EMPTY_INS = 105;

		public static int EC_EMPTY_UPD = 106;

		public static int EC_JUMP_STMT = 107;

		public static int EC_REVOKE_NONE_WARN = 108;

		public static int EC_EMPTY_CHAR_CAST = 109;

		public static int EC_BUILD_NOT_COMPLETE = 110;

		public static int EC_RESULT_SET_EMPTY = 111;

		public static int EC_UTF8_CODE_NOT_INTEGRATED = 112;

		public static int EC_RS_CACHE_FULL = 113;

		public static int EC_PREC_TRUNC_WARN = 114;

		public static int EC_SET_CURDB_SUCCESS = 200;

		public static int EC_SET_TRAN_SUCCESS = 201;

		public static int EC_EXPLAIN = 202;

		public static int EC_RN_FIND_SEARCH_EMPTY = 203;

		public static int EC_SUCCESS = 0;

		public static int EC_FAIL = -1;

		public static int EC_USER_DEFINE_EXCEPT = -2;

		public static int EC_CREATE_MUTEX = -101;

		public static int EC_SERVER_ALREADY_RUN = -102;

		public static int EC_INVALID_USAGE = -103;

		public static int EC_INI_FILE_ERROR = -104;

		public static int EC_CTL_FILE_ERROR = -106;

		public static int EC_MEM_POOL_INIT = -107;

		public static int EC_BUF_POOL_INIT = -108;

		public static int EC_FAIL_OPEN_RLOG = -109;

		public static int EC_DB_VERSION_MISMATCH = -110;

		public static int EC_DB_CODE_MISMATCH = -111;

		public static int EC_LIC_FILE_ERROR = -112;

		public static int EC_LIC_EXPIRED = -113;

		public static int EC_DB_INIT_FAIL = -114;

		public static int EC_OUT_OF_SPACE = -510;

		public static int EC_OUT_OF_USEG_SPACE = -511;

		public static int EC_OUT_OF_TEMP_SPACE = -512;

		public static int EC_OUT_OF_MEMORY = -513;

		public static int EC_OUT_OF_TABLE_SPACE = -514;

		public static int EC_COMMENT_IN_PREPARE = -2001;

		public static int EC_NOT_PREPARED = -2002;

		public static int EC_STMT_TYPE_MISMATCH = -2003;

		public static int EC_NOT_SUPPORTED = -2004;

		public static int EC_PARSE_ERROR_EXPLAIN = -2005;

		public static int EC_PARSE_ERROR_ORG = -2006;

		public static int EC_PARSE_ERROR = -2007;

		public static int EC_PARSE_ERROR_TRACE = -2008;

		public static int EC_NOT_SUPPORTED_CASCADE = -2009;

		public static int EC_PRIMARY_NOT_SUPPORTED = -2010;

		public static int EC_CHAR_CODE_NOT_SUPPORTED = -2011;

		public static int EC_MPP_NOT_SUPPORTED = -2012;

		public static int EC_WITH_COUNT_NOT_SUPPORTED = -2013;

		public static int EC_STANDBY_NOT_SUPPORTED_HUGE = -2014;

		public static int EC_STANDBY_NOT_SUPPORTED = -2015;

		public static int EC_NOT_SUPPORTED_FOR_RLOG_APPEND = -2016;

		public static int EC_RAC_NOT_SUPPORTED_HUGE = -2017;

		public static int EC_STANDBY_IDU_NOT_SUPPORTED = -2018;

		public static int EC_RAC_NOT_SUPPORTED_SPACE_LIMIT = -2019;

		public static int EC_RAC_NOT_SUPPORTED = -2020;

		public static int EC_RAC_SLAVE_EP_NOT_SUPPORTED = -2021;

		public static int EC_INVALID_DB_NAME = -1000;

		public static int EC_SAME_DB_NAME = -1001;

		public static int EC_INVALID_LOGIN_NAME = -1002;

		public static int EC_INVALID_USER_NAME = -1003;

		public static int EC_INVALID_OS_USER = -1004;

		public static int EC_INVALID_ROLE_NAME = -1006;

		public static int EC_INVALID_SCHEMA_NAME = -1007;

		public static int EC_INVALID_TABLE_NAME = -1008;

		public static int EC_INVALID_VIEW_NAME = -1009;

		public static int EC_INVALID_TV_NAME = -1010;

		public static int EC_INVALID_TEMPORARY_TABLE_NAME = -1011;

		public static int EC_INVALID_INDEX_NAME = -1012;

		public static int EC_INVALID_CONS_NAME = -1013;

		public static int EC_INVALID_TRIG_NAME = -1014;

		public static int EC_PSE_REC_NAME_OUT_OF_LENGTH = -1015;

		public static int EC_INVALID_COL_NAME = -1016;

		public static int EC_AMBIGUOUS_COL_NAME = -1017;

		public static int EC_INVALID_ALIAS_NAME = -1018;

		public static int EC_NO_COL_NAME_FOR_TEMP_TABLE = -1019;

		public static int EC_DUP_COL_NAME = -1020;

		public static int EC_COLUMN_NOT_EXIST = -1021;

		public static int EC_COLUMN_ALREADY_EXIST = -1022;

		public static int EC_INVALID_SEQUENCE_NAME = -1023;

		public static int EC_INVALID_PROC_NAME = -1024;

		public static int EC_INVALID_FUNC_NAME = -1025;

		public static int EC_INVALID_VAR_NAME = -1026;

		public static int EC_INVALID_EXCEPT_NAME = -1027;

		public static int EC_DUP_EXCEPT_HANDLER = -1028;

		public static int EC_INVALID_SAVEPOINT_NAME = -1029;

		public static int EC_INVALID_LABEL = -1030;

		public static int EC_INVALID_CURSOR_NAME = -1031;

		public static int EC_OBJ_ALREADY_EXISTS = -1032;

		public static int EC_EXPLICT_DMBLOB_TV_NAME = -1033;

		public static int EC_TOO_MANY_QUALIFIES = -1034;

		public static int EC_INVALID_DB_OBJECT = -1035;

		public static int EC_INVALID_SERVER_NAME = -1036;

		public static int EC_INVALID_PORT = -1037;

		public static int EC_INVALID_COL_NAME_WITH_TV = -1038;

		public static int EC_INVALID_DBLINK_NAME = -1039;

		public static int EC_INVALID_CONNECT_STR = -1040;

		public static int EC_INVALID_PUBLIC_FLAG = -1041;

		public static int EC_LINK_OBJ_CHANGED = -1042;

		public static int EC_SAME_MASTER_SLAVE_SERVER = -1043;

		public static int EC_INVALID_DUP_TYPE = -1044;

		public static int EC_INVALID_POLICY_NAME = -1050;

		public static int EC_INVALID_LEVEL_ID = -1051;

		public static int EC_INVALID_LEVEL_NAME = -1052;

		public static int EC_INVALID_CATEGORY_NAME = -1053;

		public static int EC_TRY_TO_MODIFY_LABEL_COL = -1054;

		public static int EC_TRY_TO_DROP_LABEL_COL = -1055;

		public static int EC_POLICY_HAS_NO_LEVEL = -1056;

		public static int EC_LABEL_MISMATCH = -1057;

		public static int EC_CAN_NOT_APPLY_POLICY = -1058;

		public static int EC_ALTER_USER_ACTIONS = -1059;

		public static int EC_ACROSS_DB_ACTION = -1060;

		public static int EC_INVALID_SYSROLE_NAME = -1061;

		public static int EC_INVALID_RES_LIMIT_VALUE = -1062;

		public static int EC_INVALID_PASSWD_REUSE_COND = -1063;

		public static int EC_PASSWORD_EXPIRED_BUT_IN_GRACE_TIME = -1063;

		public static int EC_HAS_NOT_ALTER_ITEM = -1065;

		public static int EC_INVALID_RULE_NAME = -1066;

		public static int EC_INVALID_IP = -1067;

		public static int EC_INVALID_FREQUENCY = -1068;

		public static int EC_PASSWORD_CHECK_LOGIN_NAME = -1069;

		public static int EC_INVALID_PASSWORD_POLICY = -1070;

		public static int EC_POLICY_MISMATCH = -1071;

		public static int EC_INVALID_LIMIT_SPACE = -1072;

		public static int EC_INVALID_ENCRYPT_ALGORITHM = -1074;

		public static int EC_ENCRYPT_FUN_FORBID = -1075;

		public static int EC_DECRYPT_FUN_FORBID = -1076;

		public static int EC_TOO_MANY_CONTEXT_INDEX = -1077;

		public static int EC_READONLY_TRANS = -1100;

		public static int EC_SUSPEND_TRANS = -1101;

		public static int EC_TRY_TO_DROP_CURRENT_DB = -1150;

		public static int EC_TRY_TO_DROP_SYSTEM_DB = -1151;

		public static int EC_INVALID_DATAFILE_SIZE = -1152;

		public static int EC_DISABLE_SYS_DB = -1153;

		public static int EC_DB_OFFLINE = -1154;

		public static int EC_INVALID_DB_TYPE = -1155;

		public static int EC_SET_CURRENT_DB_ON_TMP = -1156;

		public static int EC_INVALID_PATHNAME = -1157;

		public static int EC_INVALID_DATA_FILE_NAME = -1158;

		public static int EC_INVALID_LOGFILE_NUM = -1159;

		public static int EC_INVALID_LOGFILE_TRUNCSIZE = -1160;

		public static int EC_INVALID_FILEGROUP_NAME = -1161;

		public static int EC_DB_ONLINE = -1162;

		public static int EC_FILE_GROUP_MISMATCH = -1163;

		public static int EC_UNAUTHORIZED_USER = -1200;

		public static int EC_INVALID_PASSWORD = -1201;

		public static int EC_PASSWORD_CHECK_COMBIN = -1202;

		public static int EC_ASSIGN_LOGIN_TO_TMP_DB = -1203;

		public static int EC_CREATE_USER_ON_TMP_DB = -1204;

		public static int EC_CREATE_AUDIT_USER_IN_OTHER_DB = -1205;

		public static int EC_ALTER_AUDTI_LOGIN = -1206;

		public static int EC_ALTER_NORMAL_LOGIN = -1207;

		public static int EC_ALTER_AUDTI_USER = -1208;

		public static int EC_ALTER_NORMAL_USER = -1209;

		public static int EC_USER_LOCKED = -1210;

		public static int EC_PASSWORD_EXPIRED = -1211;

		public static int EC_PASSWORD_CHECK_LEN = -1212;

		public static int EC_INVALID_DEFAULT_DB = -1213;

		public static int EC_CREATE_OTHER_DB_OBJ_IN_CRT_SCHEMA = -1214;

		public static int EC_TEMPORARY_TABLE_IN_SCHEMA = -1215;

		public static int EC_USER_HAS_RELATED_LOGIN = -1216;

		public static int EC_ALTER_TEMPORARY_TABLE = -1300;

		public static int EC_ALTER_SYS_TABLE = -1301;

		public static int EC_ALTER_TABLE_ACTIONS = -1302;

		public static int EC_ALTER_PART_TABLE = -1303;

		public static int EC_ALTER_BLOB_TABLE = -1304;

		public static int EC_DROP_SYS_TABLE = -1305;

		public static int EC_DROP_SYS_VIEW = -1306;

		public static int EC_DROP_OBJ_WITH_DEPENDS = -1307;

		public static int EC_DROP_PART_TABLE = -1308;

		public static int EC_USE_TEMP_TABLE_IN_OTHER_DB = -1309;

		public static int EC_USE_TEMP_TABLE_BY_OTHER_SCH = -1310;

		public static int EC_CREATE_DUP_ON_PART_TABLE = -1311;

		public static int EC_FAIL_CRT_TABLE_WITH_QRY = -1312;

		public static int EC_TABLE_BUSY = -1313;

		public static int EC_VIEW_ON_TEMPORARY_TABLE = -1314;

		public static int EC_VIEW_DEPEND_BEYOND_DB = -1315;

		public static int EC_VIEW_VALID = -1316;

		public static int EC_TRY_TO_INSERT_VIEW = -1317;

		public static int EC_TRY_TO_DELETE_VIEW = -1318;

		public static int EC_TRY_TO_UPDATE_VIEW = -1319;

		public static int EC_NOT_UPDATE_VIEW = -1320;

		public static int EC_TOO_MANY_COLS_IN_TABLE = -1321;

		public static int EC_TOO_MANY_IDENTITY = -1322;

		public static int EC_DROP_ONLY_COL = -1323;

		public static int EC_TOO_MANY_COLS = -1324;

		public static int EC_INVALID_COLUMN_LEVEL_CHECK = -1325;

		public static int EC_TRY_TO_DROP_MANY_COLS = -1326;

		public static int EC_FAIL_TO_BUILD_COL_WITH_BLOB = -1327;

		public static int EC_INVALID_RENAME = -1328;

		public static int EC_TRY_TO_DROP_DEPENDED_COLUMN = -1329;

		public static int EC_INVALID_COLNUM = -1330;

		public static int EC_INVALID_COLUMN_TYPE = -1331;

		public static int EC_TRY_TO_MODIFY_IDENT_COL = -1332;

		public static int EC_REC_OUT_OF_MAX_SIZE = -1333;

		public static int EC_COLUMNS_DEFINED_LEN_OVER_LENGTH = -1334;

		public static int EC_INVALID_PK_SPEC = -1335;

		public static int EC_INVALID_TAB_CONSTRAINT = -1336;

		public static int EC_INVALID_KEY_COL_NAME = -1337;

		public static int EC_ERR_IN_DEFAULT_CON = -1338;

		public static int EC_INVALID_DEFAULT_VALUE_ARG = -1339;

		public static int EC_REFERENCED_CONSTRAINT = -1340;

		public static int EC_CHK_CONS_IN_TEMP_TABLE = -1341;

		public static int EC_CHK_CONS_IN_PART_TABLE = -1342;

		public static int EC_REF_CONS_IN_PART_TABLE = -1343;

		public static int EC_REF_CONS_IN_TEMP_TABLE = -1344;

		public static int EC_REF_BEYOND_DB = -1345;

		public static int EC_ALTER_AUTO_CONS_FOR_PART_TABLE = -1346;

		public static int EC_TRIG_ON_TEMPORARY_TABLE = -1400;

		public static int EC_WHEN_COND_IN_TRIG_OF_STMT = -1401;

		public static int EC_OLD_ROW_REF_ASSGIN_VALUE = -1402;

		public static int EC_CHG_NEW_ROW_REF_IN_WRONG_TRG = -1403;

		public static int EC_INVALID_TRIGGER_ACTION = -1404;

		public static int EC_INVALID_REF_NAME_IN_TRIG = -1405;

		public static int EC_INVALID_PSEUDO_REC_IN_TRIG = -1406;

		public static int EC_REPLACE_TRIG_ERR = -1407;

		public static int EC_TRY_UPDATE_MUTATING_TABLE = -1408;

		public static int EC_CREATE_TRIG_ON_PART_TABLE = -1409;

		public static int EC_COMMIT_IN_TRIGGER_BODY = -1410;

		public static int EC_ROLLBACK_IN_TRIGGER_BODY = -1411;

		public static int EC_TOO_MANY_KEYS_IN_INDEX = -1450;

		public static int EC_INVALID_INDEX_COLUMN = -1451;

		public static int EC_NO_CONTEXT_INDEX = -1452;

		public static int EC_NO_CONTEXT_INDEX_ALTERED = -1453;

		public static int EC_INEFFICACY_INDEX_DEFINE = -1454;

		public static int EC_CREATE_INDEX_ON_BLOB_COL = -1455;

		public static int EC_TRY_TO_DROP_REFED_INDEX = -1456;

		public static int EC_INVALID_STORAGE_PARAM = -1457;

		public static int EC_INDEX_DEFINE_SIZE_EXCEED = -1458;

		public static int EC_TRY_TO_DROP_CLUSTER_INDEX = -1459;

		public static int EC_TRY_TO_DROP_REF_INDEX = -1460;

		public static int EC_TABLE_HAS_CLUTER_INDEX = -1461;

		public static int EC_CREATE_CLUSTER_INDEX_ON_TMP = -1462;

		public static int EC_CREATE_INDEX_ON_PART_TABLE = -1463;

		public static int EC_COLS_IN_DIFF_PART_TABLE = -1464;

		public static int EC_CREATE_INDEX_ON_BLOB_TABLE = -1465;

		public static int EC_DROP_INDEX_ON_BLOB_TABLE = -1466;

		public static int EC_TRY_TO_ALTER_SYS_PROC = -1500;

		public static int EC_ASSIGN_VALUE_TO_IN_ARG = -1501;

		public static int EC_DROP_SYS_FUNC = -1502;

		public static int EC_INVALID_FUNC_PARA = -1503;

		public static int EC_TOO_MANY_ARGS_IN_PF = -1504;

		public static int EC_INVALID_FUNCTION = -1505;

		public static int EC_INVALID_PARAM_DEFINE = -1506;

		public static int EC_TOO_MANY_ROWS_IN_FUNCTION = -1507;

		public static int EC_RETURN_VALUE_IN_PROC = -1508;

		public static int EC_RETURN_NO_VALUE = -1509;

		public static int EC_INVALID_EXCEPT_CODE = -1510;

		public static int EC_INVALID_OTHERS_EXCPT_HNDLR = -1511;

		public static int EC_DDL_IN_PLBLOCK = -1512;

		public static int EC_UNSUPPORTED_SQL_IN_PLBLOCK = -1513;

		public static int EC_DEFAULT_TOO_MANY_VARS = -1514;

		public static int EC_INVALID_EXTERN_FUNC_PATH = -1550;

		public static int EC_INVALID_EXTERN_FUNC_NAME = -1551;

		public static int EC_INVALID_SEQUENCE_PROPERTY = -1600;

		public static int EC_CRT_LINK_FAIL = -1650;

		public static int EC_LINK_LOGIN_FAIL = -1651;

		public static int EC_LINK_ALLOC_ENV_FAIL = -1652;

		public static int EC_LINK_ALLOC_CON_FAIL = -1653;

		public static int EC_LINK_ALLOC_STMT_FAIL = -1654;

		public static int EC_LINK_EXEC_SQL_ERROR = -1655;

		public static int EC_LINK_TRY_TO_UPDATE_VIEW = -1656;

		public static int EC_INVALID_RETURN_STMT = -1700;

		public static int EC_INVALID_EXPRESSION = -1701;

		public static int EC_INVALID_CONST_EXP = -1702;

		public static int EC_MUST_BE_QUERY_EXP = -1703;

		public static int EC_SUBQERY_EXISTS = -1704;

		public static int EC_INVALID_OP = -1705;

		public static int EC_COL_REF_NOT_EXISTS = -1706;

		public static int EC_INVALID_OP_IN_CURSOR = -1800;

		public static int EC_DELETE_READONLY_CURSOR = -1801;

		public static int EC_MAKE_RD_CURSOR_UPD = -1802;

		public static int EC_UPDATE_READONLY_CURSOR = -1803;

		public static int EC_INVALID_OPEN_CUR_VAR = -1804;

		public static int EC_TOO_MANY_COLS_IN_GROUP = -1805;

		public static int EC_TOO_MANY_COLS_IN_ORDER = -1806;

		public static int EC_TOO_MANY_COLS_IN_SELECT = -1807;

		public static int EC_TOO_MANY_TABLES_IN_SELECT = -1808;

		public static int EC_INVALID_ORDERBY = -1809;

		public static int EC_INVALID_SUB_EXPR = -1810;

		public static int EC_INVALID_SEL_ITEM = -1811;

		public static int EC_SET_FUN_EXISTS = -1812;

		public static int EC_TOO_MANY_SELECT_ITEMS = -1813;

		public static int EC_INVALID_SUBQUERY = -1814;

		public static int EC_TOO_MANY_TABLES = -1815;

		public static int EC_WITH_TIES_NO_ORDER_BY = -1816;

		public static int EC_INTO_WITH_ORDER_BY = -1817;

		public static int EC_LISTS_NOT_MATCH = -1818;

		public static int EC_INSERT_TABLE_WITH_UNION = -1819;

		public static int EC_NUM_MISMATCH = -1820;

		public static int EC_OBJECT_BUSY = -1821;

		public static int EC_SET_IDENTINS_NOT_EXIST = -1822;

		public static int EC_SELECT_WITH_INTO = -1823;

		public static int EC_INVALID_GROUP_BY_NO_COL = -1824;

		public static int EC_UPDATE_FROM_USELESS = -1825;

		public static int EC_INVALID_HAVING_ITEM = -1826;

		public static int EC_SCHEMA_IS_NOT_EMPTY = -1827;

		public static int EC_ORDER_ITEM_BEYOND_DISTINCT = -1828;

		public static int EC_INDENT_NOT_IN_LIST = -1829;

		public static int EC_INVALID_GROUP_BY_HAS_SFUN_OR_SUBQUERY = -1830;

		public static int EC_NO_INS_PRIVILEGE = -1900;

		public static int EC_NO_DEL_PRIVILEGE = -1901;

		public static int EC_NO_UPD_PRIVILEGE = -1902;

		public static int EC_NO_SEL_PRIVILEGE = -1903;

		public static int EC_NO_EXECUTE_PRIVILEGE = -1904;

		public static int EC_NO_REF_PRIVILEGE = -1905;

		public static int EC_NO_INS_COL_PRIVILEGE = -1906;

		public static int EC_NO_SEL_COL_PRIVILEGE = -1907;

		public static int EC_NO_ACC_PRIVILEGE = -1908;

		public static int EC_NO_CRT_DB_PRIVILEGE = -1910;

		public static int EC_NO_CRT_LOGIN_PRIVILEGE = -1911;

		public static int EC_NO_CRT_SCH_PRIVILEGE = -1912;

		public static int EC_NO_CRT_USER_PRIVILEGE = -1913;

		public static int EC_NO_CRT_ROLE_PRIVILEGE = -1914;

		public static int EC_NO_CRT_TAB_PRIVILEGE = -1915;

		public static int EC_NO_CRT_VIEW_PRIVILEGE = -1916;

		public static int EC_NO_CRT_PROC_PRIVILEGE = -1917;

		public static int EC_NO_CRT_FUNC_PRIVILEGE = -1918;

		public static int EC_NO_CRT_SEQ_PRIVILEGE = -1919;

		public static int EC_NO_CRT_INDEX_PRIVILEGE = -1920;

		public static int EC_NO_CRT_TRIG_PRIVILEGE = -1921;

		public static int EC_NO_CRT_PLY_PRIVILEGE = -1922;

		public static int EC_NO_CRT_RULE_PRIVILEGE = -1923;

		public static int EC_NO_CRT_DBLINK_PRIVILEGE = -1924;

		public static int EC_NO_CRT_DUP_PRIVILEGE = -1925;

		public static int EC_NO_CRT_ETRIG_PRIVILEGE = -1926;

		public static int EC_NO_CRT_ETRIG_FOR_OTHER_PRIVILEGE = -1927;

		public static int EC_NO_DRP_DB_PRIVILEGE = -1930;

		public static int EC_NO_DRP_LOGIN_PRIVILEGE = -1931;

		public static int EC_NO_DRP_SCH_PRIVILEGE = -1932;

		public static int EC_NO_DRP_USER_PRIVILEGE = -1933;

		public static int EC_NO_DRP_ROLE_PRIVILEGE = -1934;

		public static int EC_NO_DRP_TAB_PRIVILEGE = -1935;

		public static int EC_NO_DRP_VIEW_PRIVILEGE = -1936;

		public static int EC_NO_DRP_PROC_PRIVILEGE = -1937;

		public static int EC_NO_DRP_FUNC_PRIVILEGE = -1938;

		public static int EC_NO_DRP_SEQ_PRIVILEGE = -1939;

		public static int EC_NO_DRP_INDEX_PRIVILEGE = -1940;

		public static int EC_NO_DRP_TRIG_PRIVILEGE = -1941;

		public static int EC_NO_DRP_PLY_PRIVILEGE = -1942;

		public static int EC_NO_DRP_RULE_PRIVILEGE = -1943;

		public static int EC_NO_DRP_DBLINK_PRIVILEGE = -1944;

		public static int EC_NO_DRP_DUP_PRIVILEGE = -1945;

		public static int EC_NO_ALT_DB_PRIVILEGE = -1950;

		public static int EC_NO_ALT_LOGIN_PRIVILEGE = -1951;

		public static int EC_NO_ALT_USER_PRIVILEGE = -1952;

		public static int EC_NO_ALT_TAB_PRIVILEGE = -1953;

		public static int EC_NO_ALT_VIEW_PRIVILEGE = -1954;

		public static int EC_NO_ALT_PROC_PRIVILEGE = -1955;

		public static int EC_NO_ALT_FUNC_PRIVILEGE = -1956;

		public static int EC_NO_ALT_SEQ_PRIVILEGE = -1957;

		public static int EC_NO_ALT_INDEX_PRIVILEGE = -1958;

		public static int EC_NO_ALT_TRIG_PRIVILEGE = -1959;

		public static int EC_NO_ALT_POLICY_PRIVILEGE = -1960;

		public static int EC_NO_ALT_USRPLY_PRIVILEGE = -1961;

		public static int EC_NO_ALT_TABPLY_PRIVILEGE = -1962;

		public static int EC_NO_GNT_PRIVILEGE = -1970;

		public static int EC_NO_GNT_ALL_PRIVILEGE = -1971;

		public static int EC_GNT_TO_OBJ_OWNER = -1972;

		public static int EC_GNT_TO_SYS_UR = -1973;

		public static int EC_NO_RVK_PRIVILEGE = -1980;

		public static int EC_NO_RVK_ALL_PRIVILEGE = -1981;

		public static int EC_RVK_FROM_AUDIT = -1982;

		public static int EC_RVK_FROM_SELF = -1983;

		public static int EC_RVK_FROM_SYS_UR = -1984;

		public static int EC_RVK_FROM_OWNER = -1985;

		public static int EC_RVK_FROM_NULL = -1986;

		public static int EC_NO_ADT_PRIVILEGE = -1987;

		public static int EC_GRANT_ROLE_TO_SELF = -1988;

		public static int EC_LOOP_GRANT = -1989;

		public static int EC_INVALID_GRANT = -1990;

		public static int EC_INVALID_REVOKE = -1991;

		public static int EC_INVALID_PRIV_NAME = -1992;

		public static int EC_NO_SESS_INFO_PRIVILEGE = -1993;

		public static int EC_NO_TRACE_PRIVILEGE = -1994;

		public static int EC_NO_AGENT_PRIVILEGE = -1995;

		public static int EC_NO_CHECK_LOGIN_PRIVILEGE = -1996;

		public static int EC_NO_UNLOCK_LOGIN_PRIVILEGE = -1997;

		public static int EC_INVALID_AUDIT_TYPE = -2000;

		public static int EC_AUDIT_OFF = -2001;

		public static int EC_TABLE_COUNT_LIMIT = -2100;

		public static int EC_VIEW_COUNT_LIMIT = -2101;

		public static int EC_USER_COUNT_LIMIT = -2102;

		public static int EC_ROLE_COUNT_LIMIT = -2103;

		public static int EC_PROC_COUNT_LIMIT = -2104;

		public static int EC_SEQUENCE_COUNT_LIMIT = -2105;

		public static int EC_LOGIN_COUNT_LIMIT = -2106;

		public static int EC_FILE_COUNT_LIMIT = -2107;

		public static int EC_OBJ_COUNT_LIMIT = -2108;

		public static int EC_TOO_MANY_NESTED_LEVEL = -2109;

		public static int EC_DBLINK_COUNT_LIMIT = -2110;

		public static int EC_POLICY_COUNT_LIMIT = -2111;

		public static int EC_RULE_COUNT_LIMIT = -2112;

		public static int EC_OPERATOR_COUNT_LIMIT = -2113;

		public static int EC_ALERT_COUNT_LIMIT = -2114;

		public static int EC_JOB_COUNT_LIMIT = -2115;

		public static int EC_TOO_MANY_SESS = -1252;

		public static int EC_CATEGORY_COUNT_LIMIT = -1253;

		public static int EC_TABLE_POLICY_COUNT_LIMIT = -1254;

		public static int EC_DATA_LOSE_WARN = -2500;

		public static int EC_DATA_CNV_FAIL = -2501;

		public static int EC_DATA_OVERFLOW = -2502;

		public static int EC_DATA_DIV_ZERO = -2503;

		public static int EC_DATA_ILLEGAL_CHAR = -2504;

		public static int EC_DATATYPE_NOT_MATCH = -2505;

		public static int EC_INVALID_ESC_CHAR = -2506;

		public static int EC_INVALID_ESC_SEQ = -2507;

		public static int EC_STR_TRUNC = -2508;

		public static int EC_STR_TRIM = -2509;

		public static int EC_STR_SUBSTR = -2510;

		public static int EC_STR_CAST = -2511;

		public static int EC_DATETIME_OVERFLOW = -2512;

		public static int EC_CAST_LOST_IMFO = -2513;

		public static int EC_CAST_LOST_PREC = -2514;

		public static int EC_INVALID_INTERVAL = -2515;

		public static int EC_UNKNOWN_PARAM_DATATYPE = -2516;

		public static int EC_CMP_FAIL = -2517;

		public static int EC_INTERVAL_OVERFLOW = -2518;

		public static int EC_INVALID_DATETIME = -2519;

		public static int EC_INVALID_DATA_TYPE = -2520;

		public static int EC_INVALID_DATATYPE = EC_INVALID_DATA_TYPE;

		public static int EC_PREC_OUT_OF_LENGTH = -2522;

		public static int EC_DEC_OUT_OF_LENGTH = -2523;

		public static int EC_INVALID_DATE = -2524;

		public static int EC_STR_TOO_LONG = -2525;

		public static int EC_INTERVAL_YEAR_MONTH_LEADING_PREC_OVERFLOW = -2526;

		public static int EC_INTERVAL_DAY_SECOND_LEADING_PREC_OVERFLOW = -2527;

		public static int EC_RN_LOCK_WAIT = -3000;

		public static int EC_RN_WAIT_SUCCESS = EC_RN_LOCK_WAIT;

		public static int EC_RN_WAIT_RANGE = -3001;

		public static int EC_RN_LOCK_FAIL = -3002;

		public static int EC_RN_DEADLOCK = -3003;

		public static int EC_RN_SET_TRANS = -3050;

		public static int EC_RN_INVALID_SAVEPNT_NAME = -3051;

		public static int EC_RN_TRANS_ACTIVE = -3052;

		public static int EC_RN_OBJECT_BUSY = -3054;

		public static int EC_RN_PLAN_ALREADY_EXIST = -3055;

		public static int EC_RN_DUP_KEY = -3100;

		public static int EC_RN_INSERT_NULL_PK_VALUE = -3101;

		public static int EC_RN_VIOLATE_UNIQUE_CONSTRAINT = -3102;

		public static int EC_RN_WITH_CHECK_OPTION = -3103;

		public static int EC_RN_VIOLATE_CHECK_CONSTRAINT = -3104;

		public static int EC_RN_CHECK_CONSTRAINT = -3105;

		public static int EC_RN_REFERENCED_CONSTRAINT = -3106;

		public static int EC_RN_FK_REFERENCE_CONSTRAINT = -3107;

		public static int EC_RN_VIOLATE_PRI_KEY_CONSTRAINT = -3108;

		public static int EC_RN_VIOLATE_NOT_NULL_CONSTAINT = -3109;

		public static int EC_RN_NO_PARAM_VALUE = -3200;

		public static int EC_RN_INVALID_DATA = -3201;

		public static int EC_RN_REC_OUT_OF_MAX_SIZE = -3202;

		public static int EC_RN_INVALID_ARG_DATA = -3203;

		public static int EC_RN_NEED_MORE_PARAMS = -3204;

		public static int EC_RN_PARAM_NUM_MISMATCH = -3205;

		public static int EC_RN_ERROR_IN_USING_EXPR_LIST = -3206;

		public static int EC_RN_NUM_MISMATCH = -3207;

		public static int EC_PARAM_IO_TYPE_MISMATCH = -3208;

		public static int EC_RN_OBJ_NOT_EXISTS = -3300;

		public static int EC_RN_INVALID_DB_OBJECT = -3301;

		public static int EC_RN_DB_OFFLINE = -3302;

		public static int EC_RN_INVALID_STMT_ID = -3303;

		public static int EC_RN_INVALID_DATA_FILE_NAME = -3304;

		public static int EC_RN_INVALID_CONTROL_FILE = -3305;

		public static int EC_RN_INVALID_CURSOR_STATE = -3306;

		public static int EC_RN_INVALID_OP_IN_CURSOR = -3307;

		public static int EC_RN_TEMPORARY_TABLE_NOT_EXIST = -3308;

		public static int EC_RN_OBJECT_MODIFIED = -3309;

		public static int EC_RN_CURDB_NOT_SYSTEM = -3310;

		public static int EC_RN_DB_CORRUPT = -3311;

		public static int EC_RN_CTL_FILE_CREATE_FAIL = -3312;

		public static int EC_RN_LOG_FILE_CREATE_FAIL = -3313;

		public static int EC_RN_DTA_FILE_CREATE_FAIL = -3314;

		public static int EC_RN_INVALID_DB_NAME = -3315;

		public static int EC_RN_INVALID_LOGIN_NAME = -3316;

		public static int EC_RN_INVALID_USER_NAME = -3317;

		public static int EC_RN_INVALID_SCHEMA_NAME = -3318;

		public static int EC_RN_INVALID_TABLE_NAME = -3319;

		public static int EC_RN_INVALID_TV_NAME = -3320;

		public static int EC_RN_INVALID_INDEX_NAME = -3321;

		public static int EC_RN_INVALID_COL_NAME = -3322;

		public static int EC_RN_TABLE_NO_CLUSTER_PK = -3323;

		public static int EC_RN_TABLE_NO_DUPLICATE = -3324;

		public static int EC_RN_TABLE_SAME_DUPLICATE = -3325;

		public static int EC_RN_INVALID_CURSOR_STMT = -3326;

		public static int EC_RN_OBJ_ALREADY_EXISTS = -3327;

		public static int EC_RN_INVALID_VALUE_QUERY = -3400;

		public static int EC_RN_TOO_MANY_SEL_INTO_ROWS = -3401;

		public static int EC_RN_FAIL_TO_BUILD_COL_WITH_BLOB = -3402;

		public static int EC_RN_TOO_MANY_PATTERNS = -3403;

		public static int EC_RN_STMT_TIMEOUT = -3404;

		public static int EC_RN_INVALID_EXEC_STMT_IN_PLBLK = -3405;

		public static int EC_RN_CONTEXTINDEX_RUNTIME_ERROR = -3406;

		public static int EC_RN_DEL_NONE = -3407;

		public static int EC_RN_TRY_TO_MODIFY_IDENT_COL = -3408;

		public static int EC_RN_SET_IDENTINS_ALREADY_SET = -3409;

		public static int EC_RN_SET_IDENTINS_NOT_SET = -3410;

		public static int EC_RN_SET_IDENTINS_OTHER_SET = -3411;

		public static int EC_RN_TOO_MANY_NESTED_LEVEL = -3412;

		public static int EC_RN_FUNC_NOT_RET_VAL = -3413;

		public static int EC_RN_NOT_ENOUGH_CTX_STACK_SIZE = -3414;

		public static int EC_RN_NO_ACC_PRIVILEGE = -3415;

		public static int EC_BAK_BASE = -3500;

		public static int EC_BAK_UNKNOWN = -3501;

		public static int EC_BAK_NO_REDO = -3502;

		public static int EC_BAK_NO_ARCH = -3503;

		public static int EC_BAK_CTL_READ = -3504;

		public static int EC_BAK_BASE_INFO = -3505;

		public static int EC_BAK_FILE_LIST = -3506;

		public static int EC_BAK_FILE_CREATE = -3507;

		public static int EC_BAK_LOG_FILE = -3508;

		public static int EC_BAK_CTL_FILE = -3509;

		public static int EC_BAK_INFO_WRITE = -3510;

		public static int EC_BAK_SYS_STATUS = -3511;

		public static int EC_BAK_DATA_FILE = -3512;

		public static int EC_BAK_PARAMENT = -3513;

		public static int EC_BAK_DUP = -3514;

		public static int EC_BAK_FILE_EXISTS = -3515;

		public static int EC_BAK_DB_STATUS = -3516;

		public static int EC_BAK_BAK_LST_ADD = -3517;

		public static int EC_BAK_PATHNAME = -3518;

		public static int EC_BAK_NO_PRIVILEGE = -3519;

		public static int EC_BAK_BASE_DB_VERSION = -3520;

		public static int EC_BAK_DB_OFFLINE = -3521;

		public static int EC_BAK_INVALID_MAXSIZE = -3522;

		public static int EC_RES_UNKNOWN = -3551;

		public static int EC_RES_NO_DB = -3552;

		public static int EC_RES_CTL_FILE = -3553;

		public static int EC_RES_SYS_DB = -3554;

		public static int EC_RES_BASE_BACKUP = -3555;

		public static int EC_RES_BAK_FILE_LIST = -3556;

		public static int EC_RES_BAK_FILE = -3557;

		public static int EC_RES_DATA_FILE = -3558;

		public static int EC_RES_LOG_FILE = -3559;

		public static int EC_RES_BACKUP = -3561;

		public static int EC_RES_PAGE_SIZE = -3562;

		public static int EC_RES_EXTENT_SIZE = -3563;

		public static int EC_RES_CASE_SENSITIVE = -3564;

		public static int EC_RES_ROWID_SIZE = -3565;

		public static int EC_RES_LOG_PAGE_SIZE = -3566;

		public static int EC_RES_UNICODE_FLAG = -3567;

		public static int EC_RES_NO_PRIVILEGE = -3568;

		public static int EC_RES_DB_VERSION = -3569;

		public static int EC_RES_FILE_VER = -3570;

		public static int EC_RES_FILE_LOST = -3571;

		public static int EC_RES_ENABLE_POLICY = -3572;

		public static int EC_RES_SET_CTL_FILE_PATH = -3573;

		public static int EC_RES_IDENTICAL_FILE_ID = -3574;

		public static int EC_RES_INVALID_FILE_ID = -3575;

		public static int EC_RES_ARCH_PATH_NUM = -3576;

		public static int EC_RES_DATA_FILE_RW = -3577;

		public static int EC_RES_DEC_INT64_FLAG = -3578;

		public static int EC_RES_DB_NAME = -3579;

		public static int EC_SESS_INFO_NOT_ACTIVE = -3600;

		public static int EC_TRACE_ALREADY_EXISTS = -3601;

		public static int EC_INVALID_TRACE_EVT_NAME = -3602;

		public static int EC_TRACE_NOT_EXISTS = -3603;

		public static int EC_TRACE_FILE_ALREADY_EXISTS = -3604;

		public static int EC_OPERATOR_NOT_EXISTS = -3700;

		public static int EC_OPERATOR_ALREADY_EXISTS = -3701;

		public static int EC_OPERATOR_IS_USED = -3702;

		public static int EC_ALERT_NOT_EXISTS = -3720;

		public static int EC_ALERT_ALREADY_EXISTS = -3721;

		public static int EC_ALERT_IS_USED = -3722;

		public static int EC_ALERT_OPERATOR_NOT_EXISTS = -3723;

		public static int EC_ALERT_OPERATOR_ALREADY_EXISTS = -3724;

		public static int EC_JOB_NOT_EXISTS = -3740;

		public static int EC_JOB_ALREADY_EXISTS = -3741;

		public static int EC_JOB_STEP_NOT_EXISTS = -3742;

		public static int EC_JOB_STEP_ALREADY_EXISTS = -3743;

		public static int EC_JOB_SCHEDULE_NOT_EXISTS = -3744;

		public static int EC_JOB_SCHEDULE_ALREADY_EXISTS = -3745;

		public static int EC_JOB_ALERT_NOT_EXISTS = -3746;

		public static int EC_JOB_ALERT_ALREADY_EXISTS = -3747;

		public static int EC_EXCEDD_MAX_SESSION_LIMIT = -6001;

		public static int EC_EXCEED_MAX_SESSION_PER_USER = -6002;

		public static int EC_CONNECT_CAN_NOT_ESTABLISHED = -6003;

		public static int EC_SNET_FAIL = -6004;

		public static int EC_SYSASYNCTRXNUM_NO_SUCH_RECORD = -6005;

		public static int EC_INVALID_MSG = -6006;

		public static int EC_RECV_OOB = -6007;

		public static int EC_MSG_UNCOMPRESS_ERR = -6008;

		public static int EC_MSG_COMPRESS_ERR = -6009;

		public static int EC_CONNECT_LOST = -6010;

		public static int EC_HOST_IS_UNKNOWN = -6011;

		public static int EC_ACCESS_IS_DENYED = -6012;

		public static int EC_INVALID_LINK = -6013;

		public static int EC_SEND_MAIL_FAIL = -6014;

		public static int EC_AUTH_FAIL = -6015;

		public static int EC_INVALID_NET_ADDRESS = -6016;

		public static int EC_USER_NOT_EXIST = -6017;

		public static int EC_OUT_OF_MSG_BUFFER = -6018;

		public static int EC_SERVER_NOT_SECURITY = -6019;

		public static int EC_CLIENT_SCHEDUAL_ERR = -6020;

		public static int EC_MAL_LINK_LOST = -6021;

		public static int EC_MSG_LEN_TOO_LONG = -6022;

		public static int ECNET_COMMUNITION_ERROR = 6001;

		public static int ECNET_MSG_CHECK_ERROR = 6002;

		public static int ECNET_SQL_IS_EMPTY = 6003;

		public static int ECNET_OVER_FLOW = 6004;

		public static int ECNET_INVALID_TIME_INTERVAL = 6005;

		public static int ECNET_UNSUPPORTED_TYPE = 6006;

		public static int ECNET_DATA_CONVERTION_ERROR = 6007;

		public static int ECNET_READ_ONLY_CONNECTION = 6008;

		public static int ECNET_INVALID_SQL_TYPE = 6009;

		public static int ECNET_INVALID_SEQUENCE = 6010;

		public static int ECNET_INVALID_DB_NAME = 6011;

		public static int ECNET_INVALID_DIGITAL_FORMAT = 6012;

		public static int ECNET_INVALID_DATA_FORMAT = 6013;

		public static int ECNET_INVALID_TIME_FORMAT = 6014;

		public static int ECNET_INVALID_DATETIME_FORMAT = 6015;

		public static int ECNET_INVALID_COLUMN_TYPE = 6016;

		public static int ECNET_INVALID_COLUMN_NAME = 6017;

		public static int ECNET_INVALID_BIGDIGITAL_FORMAT = 6018;

		public static int ECNET_INVALID_RESULTSET_TYPE = 6019;

		public static int ECNET_INVALUID_ROW_NUMBER = 6020;

		public static int ECNET_EMPTY_RESULTSET = 6021;

		public static int ECNET_INVALID_CURSOR_MOVE_DIRECTION = 6022;

		public static int ECNET_FORWORD_ONLY_RESULTSET = 6023;

		public static int ECNET_NOT_ALLOW_NULL = 6024;

		public static int ECNET_INVALID_CATALOG = 6025;

		public static int ECNET_RESULTSET_NOT_IN_INSERT_STATUS = 6026;

		public static int ECNET_RESULTSET_IS_READ_ONLY = 6029;

		public static int ECNET_UNSUPPORED_INTERFACE = 6030;

		public static int ECNET_INVALID_SEQUENCE_NUMBER = 6032;

		public static int ECNET_INVALID_RETURN_VALUE = 6033;

		public static int ECNET_RESULTSET_CLOSED = 6034;

		public static int ECNET_STATEMENT_HANDLE_CLOSED = 6035;

		public static int ECNET_INVALID_PARAMETER_VALUE = 6036;

		public static int ECNET_INVALID_TRAN_ISOLATION = 6038;

		public static int ECNET_SAVEPOINT_IN_AUTOCOMMIT_MODE = 6039;

		public static int ECNET_ROLLBACK_TO_SAVEPOINT_IN_AUTOCOMMIT_MODE = 6040;

		public static int ECNET_RELEASE_SAVEPOINT_IN_AUTOCOMMIT_MODE = 6041;

		public static int ECNET_COMMIT_IN_AUTOCOMMIT_MODE = 6042;

		public static int ECNET_ROLLBACK_IN_AUTOCOMMIT_MODE = 6043;

		public static int ECNET_INVALID_INPUT_PARAMETER_VALUE = 6044;

		public static int ECNET_INVALID_OUTPUT_PARAMETER_VALUE = 6045;

		public static int ECNET_CANNOT_GET_SAVEPOINT_ID = 6046;

		public static int ECNET_CANNOT_GET_SAVEPOINT_NAME = 6047;

		public static int ECNET_UNKNOWN_PARAMETER_TYPE = 6048;

		public static int ECNET_INVALID_SCALE = 6049;

		public static int ECNET_INVALID_PARAMETER_NAME = 6050;

		public static int ECNET_INVALID_SAVEPOINT_NAME = 6051;

		public static int ECNET_PARAMETER_PREC_TOO_BIG = 6052;

		public static int ECNET_SAVEPOINT_RELEASED = 6053;

		public static int ECNET_UNBINDED_PARAMETER = 6054;

		public static int ECNET_INVALID_CURSOR_VALUE = 6055;

		public static int ECNET_INVALID_LENGTH_OR_OFFSET = 6057;

		public static int ECNET_CONNECTION_CLOSED = 6060;

		public static int ECNET_NEGOTIATE_FAIL = 6061;

		public static int ECNET_KERBEROS_FAIL = 6062;

		public static int ECNET_GET_FQDN_FAIL = 6063;

		public static int ECNET_LOB_LENGTH_ERROR = 6070;

		public static int ECNET_INVALID_COMMAND_TYPE = 6071;

		public static int ECNET_INVALID_CONNECT_PROPERTY = 6072;

		public static int ECNET_INVALID_SCHEMA_RESTRICTIONS = 6073;

		public static int ECNET_TRX_NOLONGER_USABLE = 6074;

		public static int ECNET_CONNCTION_NOT_OPENED = 6075;

		public static int ECNET_DO_NOT_SUPPORT_CATALOG = 6076;

		public static int ECNET_CRC_CHECK_FAIL_ = 6077;

		public static int ECNET_OPT_FAIL = 6079;

		public static int ECNET_SVR_VERSION_WRONG = 6080;

		public static int ECNET_NULL_VALUE = 6081;

		public static int ECNET_MUL_REFCURSOR = 6082;

		public static int ECNET_NOT_FUNCTION = 6083;

		public static int ECNET_INVALID_ENUM_VALUE = 6084;

		public static int ECNET_READ_NO_DATA = 6085;

		public static int ECNET_INVALID_COL_NUMBER = 6086;

		public static int ECNET_NAME_TOO_LONG = 6087;

		public static int ECNET_COMMAND_TIME_OUT = 6089;

		public static int ECNET_NO_COMMAND_TEXT = 6090;

		public static int ECNET_INVALID_INDEXCOL_RESTRICTIONS = 6091;

		public static int ECNET_DATAREADER_ALREADY_OPENED = 6092;

		public static int ECNET_CONNECTION_SWITCHED = 6093;

		public static int ECNET_CONNECTION_SWITCH_FAILED = 6094;

		public static int ECNET_STR_CUT = 6095;

		public static int ECNET_ONLY_DMPARAMETER = 6096;

		public static int ECNET_SEQUENTIALACCESS_ERROR = 6097;

		public static int ECNET_INVALID_DAY = 6098;

		public static int ECNET_INVALID_MONTH = 6099;

		public static int ECNET_INVALID_YEAR = 6100;

		public static int ECNET_INVALID_HOUR24 = 6101;

		public static int ECNET_INVALID_HOUR12 = 6102;

		public static int ECNET_INVALID_MINUTE = 6103;

		public static int ECNET_INVALID_SECOND = 6104;

		public static int ECNET_INVALID_MILLSECOND = 6105;

		public static int ECNET_INVALID_AM = 6106;

		public static int ECNET_INVALID_TZH = 6107;

		public static int ECNET_INVALID_TZM = 6108;

		public static int ECNET_INVALID_TZ = 6109;

		public static int ECNET_DATEFORMAT_STR_TOO_SHORT = 6110;

		public static int ECNET_DATEFORMAT_STR_NOT_MATCH = 6111;

		public static int ECNET_DATEFORMAT_NOT_DIGIT_CHAR = 6112;

		public static int ECNET_INVALID_PARAMETER_DmDbTYPE = 6113;

		public static int ECNET_INVALID_COMPLEX_TYPE_NAME = 6114;

		public static int ECNET_INVALID_SERVER_MODE = 6115;

		public static int ECNET_DATA_TOO_LONG = 6116;

		public static int ECNET_MSG_LEN_TOO_LONG = 6117;

		public static int ECNET_NOT_SUPPORT_ENCRYPT = 6118;

		public static int ECNET_ENCRYPT_FAIL = 6119;

		public static int ECNET_DECRYPT_FAIL = 6120;

		public static int ECNET_DIGEST_FAIL = 6121;

		public static int ECNET_CHECK_DIGEST_FAIL = 6122;

		public static int ERROR_MASTER_SLAVE_SWITCHED = 10000;

		public static int ERROR_FOR_LOG = 1000000;
	}
}
