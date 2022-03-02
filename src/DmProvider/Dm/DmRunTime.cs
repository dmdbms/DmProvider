namespace Dm
{
	internal class DmRunTime
	{
		private static DmFileProperties m_Config = new DmFileProperties();

		public DmFileProperties fileProperties => m_Config;
	}
}
