using Dm.filter.log;
using Dm.filter.reconnect;
using Dm.filter.rw;

namespace Dm.filter
{
	internal interface IFilterInfo
	{
		long ID { get; }

		FilterChain FilterChain { get; set; }

		LogInfo LogInfo { get; set; }

		RWInfo RWInfo { get; set; }

		RecoverInfo RecoverInfo { get; set; }
	}
}
