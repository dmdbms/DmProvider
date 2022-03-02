using System;

namespace Dm.filter.rw
{
	public class RWInfo
	{
		internal RWSite distribute;

		internal RWCounter rwCounter;

		internal DmConnection connStandby;

		internal DateTime tryRecoverTs;

		internal DmTransaction transStandby;

		internal DmCommand cmdStandby;

		internal DmCommand cmdCurrent;

		internal bool readOnly = true;

		public DmCommand getStmtStandby()
		{
			return cmdStandby;
		}

		public DmCommand getStmtCurrent()
		{
			return cmdCurrent;
		}

		public void cleanup()
		{
			distribute = RWSite.PRIMARY;
			rwCounter = null;
			connStandby = null;
			cmdStandby = null;
			cmdCurrent = null;
		}

		public RWInfo init()
		{
			distribute = RWSite.PRIMARY;
			readOnly = true;
			transStandby = null;
			cmdStandby = null;
			cmdCurrent = null;
			return this;
		}

		public RWSite toPrimary()
		{
			if (distribute != 0)
			{
				rwCounter.countPrimary();
			}
			distribute = RWSite.PRIMARY;
			return distribute;
		}

		public RWSite toAny()
		{
			distribute = rwCounter.count(RWSite.ANY, connStandby);
			return distribute;
		}
	}
}
