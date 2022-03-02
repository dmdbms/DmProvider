namespace NetTaste
{
	public class UTF8Buffer : Buffer
	{
		public UTF8Buffer(Buffer b)
			: base(b)
		{
		}

		public override int Read()
		{
			int num;
			do
			{
				num = base.Read();
			}
			while (num >= 128 && (num & 0xC0) != 192 && num != 65536);
			if (num >= 128 && num != 65536)
			{
				if ((num & 0xF0) == 240)
				{
					int num2 = num & 7;
					num = base.Read();
					int num3 = num & 0x3F;
					num = base.Read();
					int num4 = num & 0x3F;
					num = base.Read();
					int num5 = num & 0x3F;
					num = (((((num2 << 6) | num3) << 6) | num4) << 6) | num5;
				}
				else if ((num & 0xE0) == 224)
				{
					int num6 = num & 0xF;
					num = base.Read();
					int num7 = num & 0x3F;
					num = base.Read();
					int num8 = num & 0x3F;
					num = (((num6 << 6) | num7) << 6) | num8;
				}
				else if ((num & 0xC0) == 192)
				{
					int num9 = num & 0x1F;
					num = base.Read();
					int num10 = num & 0x3F;
					num = (num9 << 6) | num10;
				}
			}
			return num;
		}
	}
}
