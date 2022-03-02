namespace Dm
{
	internal class DmColumn : DmField
	{
		public DmColumn(DmConnInstance conn)
			: base(conn)
		{
		}

		public static int GetMaxTupleLen(DmColumn[] cols, int maxRowSize)
		{
			int num = 0;
			for (short num2 = 0; num2 < cols.Length; num2 = (short)(num2 + 1))
			{
				if (cols[num2].GetCType() == 12 || cols[num2].GetCType() == 19)
				{
					num = maxRowSize;
					break;
				}
				num += cols[num2].GetPrecision();
			}
			if (num > maxRowSize)
			{
				num = maxRowSize;
			}
			return num + (7 + 2 * cols.Length + 5);
		}
	}
}
