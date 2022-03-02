using System;
using System.Data;

namespace Dm
{
	internal class DmSqlTypeInfo
	{
		internal int _cType;

		public DmSqlTypeInfo(int cType, string Name, DmDbType DmDbType, DbType DbType, Type Type)
		{
			_cType = cType;
		}
	}
}
