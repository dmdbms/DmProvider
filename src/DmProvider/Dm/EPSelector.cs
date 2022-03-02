using System.Collections.Generic;

namespace Dm
{
	internal abstract class EPSelector
	{
		protected EP[] dbs;

		internal EPSelector(List<EP> serverList)
		{
			dbs = serverList.ToArray();
		}

		public abstract EP[] sortDBList(bool firstTime);

		public abstract bool checkServerMode(bool lastTime, DmConnection conn);
	}
}
