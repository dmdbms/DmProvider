using System;

namespace Dm.filter.log
{
	internal interface ILogger
	{
		bool ErrorEnabled { get; }

		bool SqlEnabled { get; }

		bool InfoEnabled { get; }

		void Error(string message);

		void Error(string message, Exception t);

		void Sql(string message);

		void Info(string message);

		void Info(object source, string method, string info);

		void Info(object source, string method, params object[] @params);
	}
}
