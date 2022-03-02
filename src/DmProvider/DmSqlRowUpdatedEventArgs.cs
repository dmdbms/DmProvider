using System.Data;
using System.Data.Common;

public class DmSqlRowUpdatedEventArgs : RowUpdatedEventArgs
{
	public DmSqlRowUpdatedEventArgs(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		: base(dataRow, command, statementType, tableMapping)
	{
	}
}
