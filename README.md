# DmProvider
[![NuGet Version](http://img.shields.io/nuget/v/dmdbms.DmProvider.svg?style=flat)](https://www.nuget.org/packages/dmdbms.DmProvider/)
DmProvider is an ADO.NET data provider for DaMeng(DM).
�����ڴ������ݿ��ADO.NET�����ṩ����

## How to Use
## ʹ�÷���

```csharp
// set these values correctly for your database server
// ����ʵ��������ò�����ֵ
var builder = new DmConnectionStringBuilder
{
    Server = "your-server",
    Port = 5236,
    Schema = "database-name",
    User = "database-user",
    Password = "P@ssw0rd!"
};

// open a connection
// ������
using var connection = new DmConnection(builder.ConnectionString);
connection.Open();

// create a DB command and set the SQL statement with parameters
// ����һ�����ݿ�����������ò���
using var command = connection.CreateCommand();
command.CommandText = @"SELECT * FROM orders WHERE order_id = @OrderId;";
command.Parameters.AddWithValue("@OrderId", orderId);

// execute the command and read the results
// ִ��SQL�����ȡ���
using var reader = command.ExecuteReader();
while (reader.Read())
{
	var id = reader.GetInt32("order_id");
	var date = reader.GetDateTime("order_date");
	// ...
}
```

## Main Types
## ��Ҫ����

The main types provided by this library are:
�˳�����ṩ����Щ��Ҫ����:

* `DmConnection` (implementation of `DbConnection`)
* `DmCommand` (implementation of `DbCommand`)
* `DmDataReader` (implementation of `DbDataReader`)
* `DmBulkCopy`
* `DmConnectionStringBuilder`
* `DmConnectorFactory`
* `DmDataAdapter` (implementation of `DbDataAdapter`)
* `DmException`
* `DmTransaction` (implementation of `DbTransaction`)

## Related Packages
## ��ذ�

* Entity Framework Core: [dmdbms.Microsoft.EntityFrameworkCore.Dm](https://www.nuget.org/packages/dmdbms.Microsoft.EntityFrameworkCore.Dm/)