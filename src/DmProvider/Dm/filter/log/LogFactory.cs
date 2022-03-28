using System;
using System.Collections.Generic;

namespace Dm.filter.log
{
	internal class LogFactory
	{
		private static Dictionary<object, Logger> instances = new Dictionary<object, Logger>();

		public static ILogger getLog(Type clazz)
		{
			Logger logger;
			if (instances.ContainsKey(clazz))
			{
				logger = instances[clazz];
				if (logger != null)
				{
					return logger;
				}
			}
			logger = new Logger(clazz.FullName);
			instances[clazz] = logger;
			return logger;
		}

		public static ILogger getLog(string name)
		{
			Logger logger;
			if (instances.ContainsKey(name))
			{
				logger = instances[name];
				if (logger != null)
				{
					return logger;
				}
			}
			logger = new Logger(name);
			instances[name] = logger;
			return logger;
		}

		public virtual void releaseAll()
		{
			instances.Clear();
		}
	}
}
