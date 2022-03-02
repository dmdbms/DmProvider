using System;
using System.Data.Common;

namespace Dm
{
	public class DmClientFactory : DbProviderFactory, IServiceProvider
	{
		public static readonly DmClientFactory Instance = new DmClientFactory();

		public override bool CanCreateDataSourceEnumerator => true;

		private DmClientFactory()
		{
		}

		public override DbCommand CreateCommand()
		{
			return new DmCommand();
		}

		public override DbCommandBuilder CreateCommandBuilder()
		{
			return new DmCommandBuilder();
		}

		public override DbConnection CreateConnection()
		{
			return new DmConnection();
		}

		public override DbConnectionStringBuilder CreateConnectionStringBuilder()
		{
			return new DmConnectionStringBuilder();
		}

		public override DbDataAdapter CreateDataAdapter()
		{
			return new DmDataAdapter();
		}

		public override DbParameter CreateParameter()
		{
			return new DmParameter();
		}

		object IServiceProvider.GetService(Type serviceType)
		{
			return null;
		}
	}
}
