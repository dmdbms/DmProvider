using System;

namespace NetTaste
{
	public class FatalError : Exception
	{
		public FatalError(string m)
			: base(m)
		{
		}
	}
}
