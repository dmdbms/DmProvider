using System;
using System.Collections.Generic;

namespace Dm.filter.rw
{
	internal class RWCounter
	{
		private static object obj = new object();

		private static Dictionary<string, RWCounter> rwMap = new Dictionary<string, RWCounter>();

		private long NTRX_PRIMARY;

		private long NTRX_TOTAL;

		private double primaryPercent;

		private double standbyPercent;

		private Dictionary<string, long> standbyNTrxMap = new Dictionary<string, long>();

		private Dictionary<string, int> standbyIdMap = new Dictionary<string, int>();

		private int standbyCount;

		private Random random = new Random();

		private int[] flag;

		private int[] increments;

		private RWCounter(int primaryPercent, int standbyCount)
		{
			reset(primaryPercent, standbyCount);
		}

		private void reset(int primaryPercent, int standbyCount)
		{
			lock (obj)
			{
				NTRX_PRIMARY = 0L;
				NTRX_TOTAL = 0L;
				this.standbyCount = standbyCount;
				increments = new int[standbyCount + 1];
				increments[0] = primaryPercent * standbyCount;
				for (int i = 1; i < increments.Length; i++)
				{
					increments[i] = 100 - primaryPercent;
				}
				increments = divis(increments);
				flag = new int[increments.Length];
				Array.Copy(increments, flag, increments.Length);
				if (standbyCount > 0)
				{
					this.primaryPercent = (double)primaryPercent / 100.0;
					standbyPercent = (double)(100 - primaryPercent) / 100.0 / (double)standbyCount;
				}
				else
				{
					this.primaryPercent = 1.0;
					standbyPercent = 0.0;
				}
			}
		}

		public static RWCounter getInstance(DmConnection connection)
		{
			string key = connection.ConnProperty.Server + "_" + connection.ConnProperty.Port + "_" + connection.ConnProperty.RwPercent;
			RWCounter rWCounter = null;
			lock (obj)
			{
				if (!rwMap.ContainsKey(key))
				{
					rWCounter = new RWCounter(connection.ConnProperty.RwPercent, connection.ConnProperty.StandbyNum);
					rwMap[key] = rWCounter;
					return rWCounter;
				}
				rWCounter = rwMap[key];
				if (rWCounter.standbyCount != connection.ConnProperty.StandbyNum)
				{
					rWCounter.reset(connection.ConnProperty.RwPercent, connection.ConnProperty.StandbyNum);
					return rWCounter;
				}
				return rWCounter;
			}
		}

		public RWSite countPrimary()
		{
			lock (obj)
			{
				adjustNtrx();
				incrementPrimaryNtrx();
				return RWSite.PRIMARY;
			}
		}

		public RWSite count(RWSite dest, DmConnection standby)
		{
			lock (obj)
			{
				adjustNtrx();
				switch (dest)
				{
				case RWSite.ANY:
					if (primaryPercent == 1.0 || (flag[0] > flag[getStandbyId(standby)] && flag[0] > sum(flag, 1, flag.Length)))
					{
						incrementPrimaryNtrx();
						dest = RWSite.PRIMARY;
					}
					else
					{
						incrementStandbyNtrx(standby);
						dest = RWSite.STANDBY;
					}
					break;
				case RWSite.STANDBY:
					incrementStandbyNtrx(standby);
					break;
				case RWSite.PRIMARY:
					incrementPrimaryNtrx();
					break;
				default:
					throw new InvalidOperationException("Invalid RWSite!");
				}
				return dest;
			}
		}

		private void adjustNtrx()
		{
			lock (obj)
			{
				if (NTRX_TOTAL >= long.MaxValue)
				{
					long num = long.MaxValue;
					IEnumerator<long> enumerator = standbyNTrxMap.Values.GetEnumerator();
					while (enumerator.MoveNext())
					{
						if (enumerator.Current < num)
						{
							num = enumerator.Current;
						}
					}
					num = ((num < NTRX_PRIMARY) ? num : NTRX_PRIMARY);
					NTRX_PRIMARY /= num;
					NTRX_TOTAL /= num;
					IEnumerator<KeyValuePair<string, long>> enumerator2 = standbyNTrxMap.GetEnumerator();
					while (enumerator2.MoveNext())
					{
						KeyValuePair<string, long> current = enumerator2.Current;
						standbyNTrxMap[current.Key] = current.Value / num;
					}
				}
				if (flag[0] <= 0 && sum(flag, 1, flag.Length) <= 0)
				{
					sum(flag, increments);
				}
			}
		}

		private void incrementPrimaryNtrx()
		{
			NTRX_PRIMARY++;
			flag[0]--;
			NTRX_TOTAL++;
		}

		private long getStandbyNtrx(DmConnection standby)
		{
			lock (obj)
			{
				string key = standby.ConnProperty.Server + ":" + standby.ConnProperty.Port;
				if (standbyNTrxMap.ContainsKey(key))
				{
					return standbyNTrxMap[key];
				}
				return 0L;
			}
		}

		private int getStandbyId(DmConnection standby)
		{
			lock (obj)
			{
				string key = standby.ConnProperty.Server + ":" + standby.ConnProperty.Port;
				if (!standbyIdMap.ContainsKey(key))
				{
					int num = standbyIdMap.Count + 1;
					standbyIdMap[key] = num;
					return num;
				}
				return standbyIdMap[key];
			}
		}

		private void incrementStandbyNtrx(DmConnection standby)
		{
			lock (obj)
			{
				string key = standby.ConnProperty.Server + ":" + standby.ConnProperty.Port;
				if (!standbyNTrxMap.ContainsKey(key))
				{
					standbyNTrxMap[key] = 1L;
				}
				else
				{
					standbyNTrxMap[key] += 1;
				}
				int num;
				if (!standbyIdMap.ContainsKey(key))
				{
					num = standbyIdMap.Count + 1;
					standbyIdMap[key] = num;
				}
				else
				{
					num = standbyIdMap[key];
				}
				flag[num]--;
				NTRX_TOTAL++;
			}
		}

		public int Random(int rowCount)
		{
			return random.Next(rowCount);
		}

		public override string ToString()
		{
			return "PERCENT(P/S) : " + primaryPercent + "/" + standbyPercent + "\nNTRX_PRIMARY : " + NTRX_PRIMARY + "\nNTRX_TOTAL : " + NTRX_TOTAL + "\nNTRX_STANDBY : " + standbyNTrxMap.ToString();
		}

		private int[] divis(int[] nums)
		{
			int num = nums[0];
			for (int i = 1; i < nums.Length; i++)
			{
				if (nums[i] < num)
				{
					num = nums[i];
				}
			}
			int num2 = 0;
			for (num2 = num; num2 > 0; num2--)
			{
				int num3 = 0;
				for (int j = 0; j < nums.Length && nums[j] % num2 == 0; j++)
				{
					num3++;
				}
				if (num3 == nums.Length)
				{
					break;
				}
			}
			if (num2 > 1)
			{
				for (int k = 0; k < nums.Length; k++)
				{
					nums[k] /= num2;
				}
			}
			return nums;
		}

		private long sum(int[] vals, int startOff, int endOff)
		{
			int num = 0;
			for (int i = startOff; i < endOff; i++)
			{
				num += vals[i];
			}
			return num;
		}

		private int[] sum(int[] vals, int[] increments)
		{
			for (int i = 0; i < vals.Length; i++)
			{
				vals[i] += increments[i];
			}
			return vals;
		}
	}
}
