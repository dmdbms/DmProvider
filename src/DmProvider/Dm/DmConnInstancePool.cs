namespace Dm
{
	internal class DmConnInstancePool
	{
		private static int m_ConnectionNumber;

		public static int Number => m_ConnectionNumber;

		private DmConnInstancePool()
		{
		}

		public static DmConnInstance GetConnInstance(DmConnProperty property)
		{
			return null;
		}

		public static void DestroyAll()
		{
		}
	}
}
