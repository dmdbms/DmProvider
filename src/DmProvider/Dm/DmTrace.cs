using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Dm
{
	public class DmTrace
	{
		private static readonly string ClassName = "DmTrace";

		private static TraceLevel level = TraceLevel.None;

		private static bool to_file = true;

		private static string path = "ProviderTrace.txt";

		private static string pathBase = "ProviderTrace";

		private static object fileused = new object();

		public static bool To_file
		{
			get
			{
				return to_file;
			}
			set
			{
				to_file = value;
			}
		}

		public static TraceLevel Level
		{
			get
			{
				TracePropertyGet(TraceLevel.Debug, ClassName, "Level");
				return level;
			}
			set
			{
				TracePropertySet(TraceLevel.Debug, ClassName, "Level");
				level = value;
			}
		}

		private DmTrace()
		{
		}

		protected static void WriteIntoFile(byte[] info)
		{
			lock (fileused)
			{
				if (!File.Exists(path))
				{
					using (File.Create(path))
					{
					}
				}
				using FileStream fileStream2 = File.Open(path, FileMode.Append);
				fileStream2.Write(info, 0, info.Length);
			}
		}

		protected static void WriteIntoFile(byte[] info, int thd)
		{
			string text = pathBase + thd + ".txt";
			lock (fileused)
			{
				if (!File.Exists(text))
				{
					using (File.Create(text))
					{
					}
				}
				using FileStream fileStream2 = File.Open(text, FileMode.Append);
				fileStream2.Write(info, 0, info.Length);
			}
		}

		internal static void TracePropertySet(TraceLevel lev, string ClassName, string PropertyName)
		{
			if (lev <= level)
			{
				string text = ClassName + ":" + PropertyName + "\tproperty set. " + DateTime.Now.ToLocalTime().ToString() + "\n";
				if (to_file)
				{
					WriteIntoFile(DmConvertion.GetBytes(text, null));
				}
				else
				{
					Console.WriteLine(text);
				}
			}
		}

		internal static void TracePropertyGet(TraceLevel lev, string ClassName, string PropertyName)
		{
			if (lev <= level)
			{
				string text = ClassName + ":" + PropertyName + "\tproperty get. " + DateTime.Now.ToLocalTime().ToString() + "\n";
				if (to_file)
				{
					WriteIntoFile(DmConvertion.GetBytes(text, null));
				}
				else
				{
					Console.WriteLine(text);
				}
			}
		}

		internal static void TraceMethodEnter(TraceLevel lev, string ClassName, string MethodName)
		{
			if (lev <= level)
			{
				string text = ClassName + ":" + MethodName + "\tmethod enter. " + DateTime.Now.ToLocalTime().ToString() + "\n";
				if (to_file)
				{
					WriteIntoFile(DmConvertion.GetBytes(text, null));
				}
				else
				{
					Console.WriteLine(text);
				}
			}
		}

		public static void TracePrint(string str)
		{
			if (level >= TraceLevel.Trace)
			{
				int hashCode = Thread.CurrentThread.GetHashCode();
				byte[] bytes = DmConvertion.GetBytes("[" + hashCode + "] " + DateTime.Now.ToLocalTime().ToString() + " " + str + "\n", null);
				if (level == TraceLevel.Thread)
				{
					WriteIntoFile(bytes, hashCode);
				}
				else
				{
					WriteIntoFile(bytes);
				}
			}
		}

		public static void TracePrintStack(string usrString)
		{
			if (level < TraceLevel.Trace)
			{
				return;
			}
			StackFrame[] frames = new StackTrace().GetFrames();
			string text = "[" + Thread.CurrentThread.GetHashCode() + "]";
			if (usrString != null)
			{
				usrString = "[" + usrString + "]";
			}
			for (int i = 0; i < frames.Length; i++)
			{
				MethodBase method = frames[i].GetMethod();
				string text2 = string.Format("{3}{4}[CALL STACK][{0}]: {1}.{2}\n", i, (method.DeclaringType == null || method.DeclaringType!.Name == null) ? "null" : method.DeclaringType!.FullName, method.Name, text, usrString);
				if (to_file)
				{
					WriteIntoFile(DmConvertion.GetBytes(text2, null));
				}
				else
				{
					Console.WriteLine(text2);
				}
			}
		}

		public static void TracePrintStack()
		{
			TracePrintStack(null);
		}
	}
}
