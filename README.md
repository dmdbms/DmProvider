## 提示：此代码仓库不是由达梦公司维护，如果有Pull request，欢迎提交。如果需要技术支持，请联系达梦公司。

# DmProvider [![NuGet Version](http://img.shields.io/nuget/v/dmdbms.DmProvider.svg?style=flat)](https://www.nuget.org/packages/dmdbms.DmProvider/)

* DmProvider is an ADO.NET data provider for DaMeng(DM).
* 适用于达梦数据库的ADO.NET数据提供程序

## How to Use - 使用方法
```csharp
// set these values correctly for your database server
// 根据实际情况设置参数的值
var builder = new DmConnectionStringBuilder
{
    Server = "your-server",
    Port = 5236,
    Schema = "database-name",
    User = "database-user",
    Password = "P@ssw0rd!"
};

// open a connection
// 打开连接
using var connection = new DmConnection(builder.ConnectionString);
connection.Open();

// create a DB command and set the SQL statement with parameters
// 创建一个数据库命令对象并设置参数
using var command = connection.CreateCommand();
command.CommandText = @"SELECT * FROM orders WHERE order_id = @OrderId;";
command.Parameters.AddWithValue("@OrderId", orderId);

// execute the command and read the results
// 执行SQL命令并读取结果
using var reader = command.ExecuteReader();
while (reader.Read())
{
	var id = reader.GetInt32("order_id");
	var date = reader.GetDateTime("order_date");
	// ...
}
```

## Main Types - 主要类型

The main types provided by this library are:
此程序库提供了这些主要类型:

* `DmConnection` (implementation of `DbConnection`)
* `DmCommand` (implementation of `DbCommand`)
* `DmDataReader` (implementation of `DbDataReader`)
* `DmBulkCopy`
* `DmConnectionStringBuilder`
* `DmConnectorFactory`
* `DmDataAdapter` (implementation of `DbDataAdapter`)
* `DmException`
* `DmTransaction` (implementation of `DbTransaction`)

## Related Packages - 相关包

* Entity Framework Core: [dmdbms.Microsoft.EntityFrameworkCore.Dm](https://www.nuget.org/packages/dmdbms.Microsoft.EntityFrameworkCore.Dm/)
