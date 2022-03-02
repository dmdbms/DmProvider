namespace Dm
{
	internal class RsKey
	{
		internal string dbGuid;

		internal string currentSchema;

		internal string sql;

		internal int paramCount;

		internal DmParameterCollection parameters;

		internal RsKey(string dbGuid, string currentSchema, string sql, int paramCount, DmParameterCollection parameters)
		{
			this.dbGuid = dbGuid;
			this.currentSchema = currentSchema;
			this.sql = sql;
			this.paramCount = paramCount;
			this.parameters = parameters;
		}

		public override int GetHashCode()
		{
			int num = 1;
			num = 31 * num + ((dbGuid != null) ? dbGuid.GetHashCode() : 0);
			num = 31 * num + ((currentSchema != null) ? currentSchema.GetHashCode() : 0);
			num = 31 * num + ((sql != null) ? sql.GetHashCode() : 0);
			if (parameters != null && parameters.Count > 0)
			{
				foreach (object parameter in parameters)
				{
					num = 31 * num + parameter.GetHashCode();
				}
				return num;
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (!(obj is RsKey))
			{
				return false;
			}
			RsKey rsKey = (RsKey)obj;
			if (dbGuid.Equals(rsKey.dbGuid) && currentSchema.Equals(rsKey.currentSchema) && sql.Equals(rsKey.sql) && paramCount == rsKey.paramCount)
			{
				return parameters.Equals(rsKey.parameters);
			}
			return false;
		}
	}
}
