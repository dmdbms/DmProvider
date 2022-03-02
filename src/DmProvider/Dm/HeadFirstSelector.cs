using System.Collections.Generic;

namespace Dm
{
	internal class HeadFirstSelector : WellDistributeSelector
	{
		public HeadFirstSelector(List<EP> serverList)
			: base(serverList)
		{
		}

		public override EP[] sortDBList(bool firstTime)
		{
			EP[] array = null;
			if (firstTime)
			{
				return dbs;
			}
			int num = dbs.Length;
			array = new EP[num];
			int num2 = 1;
			for (int i = 0; i < num; i++)
			{
				array[i] = dbs[(i + num2) % num];
			}
			sortByServerMode(array, firstTime);
			return array;
		}
	}
}
