using System.Data;
using System.Data.Common;

public class DmSqlRowUpdatingEventArgs : RowUpdatingEventArgs
{
	public DmSqlRowUpdatingEventArgs(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		: base(dataRow, command, statementType, tableMapping)
	{
	}
}
