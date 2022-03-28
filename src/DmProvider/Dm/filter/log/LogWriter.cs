using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Dm.Config;
using Dm.util;

namespace Dm.filter.log
{
	internal class LogWriter
	{
		private class LoggerHolder
		{
			internal static readonly LogWriter instance = new LogWriter();
		}

		private const int BATCH_SIZE = 100;

		private const string FILE_PREFIX = "DmProvider";

		private BlockingQueue<byte[]> _flushQueue = new BlockingQueue<byte[]>();

		private Thread _baseThread;

		private FileInfo _logFile;

		private BufferedStream _output;

		private string _fileDir;

		private int _curFileLength;

		internal static LogWriter Instance => LoggerHolder.instance;

		private string Now => DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss.fff", DateTimeFormatInfo.CurrentInfo);

		private LogWriter()
		{
			_fileDir = DmSvcConfig.logDir;
			_baseThread = new Thread(new ThreadStart(Run));
			_baseThread.Name = "DmProvider-LogFlusher";
			_baseThread.IsBackground = true;
			_baseThread.Start();
		}

		private void Run()
		{
			List<byte[]> list = null;
			while (true)
			{
				try
				{
					list = _flushQueue.Dequeue(100);
					if (list != null && list.Count > 0)
					{
						Flush(list);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.StackTrace);
				}
			}
		}

		private void Flush(List<byte[]> list)
		{
			if (_logFile == null)
			{
				_output = CreateNewFile();
				_curFileLength = 0;
			}
			else if (_curFileLength > DmSvcConfig.logSize)
			{
				CloseCurrentFile();
				_output = CreateNewFile();
				_curFileLength = 0;
			}
			try
			{
				foreach (byte[] item in list)
				{
					_output.Write(item, 0, item.Length);
					_curFileLength += item.Length;
				}
				_output.Flush();
			}
			catch (IOException)
			{
			}
		}

		private BufferedStream CreateNewFile()
		{
			try
			{
				string text = "DmProvider_" + Now + ".log";
				if (StringUtil.isNotEmpty(_fileDir) && StringUtil.isNotEmpty(text))
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(_fileDir);
					if (!directoryInfo.Exists)
					{
						directoryInfo.Create();
					}
					_logFile = new FileInfo(_fileDir + text);
					if (!_logFile.Exists)
					{
						return new BufferedStream(new FileStream(_logFile.FullName, FileMode.CreateNew), DmSvcConfig.logSize);
					}
					return new BufferedStream(new FileStream(_logFile.FullName, FileMode.Append), DmSvcConfig.logSize);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
			return null;
		}

		private void CloseCurrentFile()
		{
			if (_output == null)
			{
				return;
			}
			try
			{
				_output.Close();
			}
			catch (IOException)
			{
			}
			finally
			{
				_output = null;
			}
		}

		internal void WriteLine(string msg)
		{
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(StringUtil.trimToEmpty(msg) + StringUtil.LINE_SEPARATOR);
				_flushQueue.Enqueue(bytes);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
		}
	}
}
