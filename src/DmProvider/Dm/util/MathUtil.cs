using System;

namespace Dm.util
{
	internal class MathUtil
	{
		internal static DateTime Round(DateTime dt, int scale)
		{
			long num = dt.Ticks;
			bool flag = false;
			for (int i = 0; i < 7 - scale; i++)
			{
				flag = false;
				if (num % 10 > 5)
				{
					flag = true;
				}
				num = num / 10 + (flag ? 1 : 0);
			}
			return new DateTime(num * (long)Math.Pow(10.0, 7 - scale));
		}
	}
}
