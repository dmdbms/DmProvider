using System;
using System.IO;

namespace NetTaste
{
	public class Errors
	{
		public int count;

		public TextWriter errorStream = Console.Out;

		public string errMsgFormat = "-- line {0} col {1}: {2}";

		public virtual void SynErr(int line, int col, int n)
		{
			string arg = n switch
			{
				0 => "EOF expected", 
				1 => "integer expected", 
				2 => "identifier expected", 
				3 => "binteger expected", 
				4 => "decimal expected", 
				5 => "real expected", 
				6 => "op expected", 
				7 => "one_vard expected", 
				8 => "one_varq expected", 
				9 => "one_var expected", 
				10 => "end_word expected", 
				11 => "??? expected", 
				_ => "error " + n, 
			};
			errorStream.WriteLine(errMsgFormat, line, col, arg);
			count++;
		}

		public virtual void SemErr(int line, int col, string s)
		{
			errorStream.WriteLine(errMsgFormat, line, col, s);
			count++;
		}

		public virtual void SemErr(string s)
		{
			errorStream.WriteLine(s);
			count++;
		}

		public virtual void Warning(int line, int col, string s)
		{
			errorStream.WriteLine(errMsgFormat, line, col, s);
		}

		public virtual void Warning(string s)
		{
			errorStream.WriteLine(s);
		}
	}
}
