using System;
using System.Collections;
using System.IO;

namespace NetTaste
{
	public class Scanner
	{
		private const char EOL = '\n';

		private const int eofSym = 0;

		private const int maxT = 11;

		private const int noSym = 11;

		public Buffer buffer;

		private Token t;

		private int ch;

		private int pos;

		private int charPos;

		private int col;

		private int line;

		private int oldEols;

		private static readonly Hashtable start;

		private Token tokens;

		private Token pt;

		private char[] tval = new char[128];

		private int tlen;

		static Scanner()
		{
			start = new Hashtable(128);
			for (int i = 49; i <= 57; i++)
			{
				start[i] = 16;
			}
			for (int j = 65; j <= 90; j++)
			{
				start[j] = 1;
			}
			for (int k = 97; k <= 122; k++)
			{
				start[k] = 1;
			}
			for (int l = 33; l <= 33; l++)
			{
				start[l] = 11;
			}
			for (int m = 35; m <= 38; m++)
			{
				start[m] = 11;
			}
			for (int n = 40; n <= 45; n++)
			{
				start[n] = 11;
			}
			for (int num = 47; num <= 47; num++)
			{
				start[num] = 11;
			}
			for (int num2 = 59; num2 <= 64; num2++)
			{
				start[num2] = 11;
			}
			for (int num3 = 94; num3 <= 96; num3++)
			{
				start[num3] = 11;
			}
			for (int num4 = 123; num4 <= 126; num4++)
			{
				start[num4] = 11;
			}
			for (int num5 = 34; num5 <= 34; num5++)
			{
				start[num5] = 12;
			}
			for (int num6 = 39; num6 <= 39; num6++)
			{
				start[num6] = 13;
			}
			for (int num7 = 58; num7 <= 58; num7++)
			{
				start[num7] = 17;
			}
			for (int num8 = 0; num8 <= 0; num8++)
			{
				start[num8] = 15;
			}
			start[48] = 18;
			start[46] = 19;
			start[65536] = -1;
		}

		public Scanner(string fileName)
		{
			try
			{
				Stream s = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				buffer = new Buffer(s, isUserStream: false);
				Init();
			}
			catch (IOException)
			{
				throw new FatalError("Cannot open file " + fileName);
			}
		}

		public Scanner(Stream s)
		{
			buffer = new Buffer(s, isUserStream: true);
			Init();
		}

		private void Init()
		{
			pos = -1;
			line = 1;
			col = 0;
			charPos = -1;
			oldEols = 0;
			NextCh();
			if (ch == 239)
			{
				NextCh();
				int num = ch;
				NextCh();
				int num2 = ch;
				if (num != 187 || num2 != 191)
				{
					throw new FatalError(string.Format("illegal byte order mark: EF {0,2:X} {1,2:X}", num, num2));
				}
				buffer = new UTF8Buffer(buffer);
				col = 0;
				charPos = -1;
				NextCh();
			}
			pt = (tokens = new Token());
		}

		private void NextCh()
		{
			if (oldEols > 0)
			{
				ch = 10;
				oldEols--;
				return;
			}
			pos = buffer.Pos;
			ch = buffer.Read();
			col++;
			charPos++;
			if (ch == 13 && buffer.Peek() != 10)
			{
				ch = 10;
			}
			if (ch == 10)
			{
				line++;
				col = 0;
			}
		}

		private void AddCh()
		{
			if (tlen >= tval.Length)
			{
				char[] destinationArray = new char[2 * tval.Length];
				Array.Copy(tval, 0, destinationArray, 0, tval.Length);
				tval = destinationArray;
			}
			if (ch != 65536)
			{
				tval[tlen++] = (char)ch;
				NextCh();
			}
		}

		private bool Comment0()
		{
			int num = 1;
			int num2 = pos;
			int num3 = line;
			int num4 = col;
			int num5 = charPos;
			NextCh();
			if (ch == 47)
			{
				NextCh();
				while (true)
				{
					if (ch == 10)
					{
						num--;
						if (num == 0)
						{
							oldEols = line - num3;
							NextCh();
							return true;
						}
						NextCh();
					}
					else
					{
						if (ch == 65536)
						{
							break;
						}
						NextCh();
					}
				}
				return false;
			}
			buffer.Pos = num2;
			NextCh();
			line = num3;
			col = num4;
			charPos = num5;
			return false;
		}

		private bool Comment1()
		{
			int num = 1;
			int num2 = pos;
			int num3 = line;
			int num4 = col;
			int num5 = charPos;
			NextCh();
			if (ch == 42)
			{
				NextCh();
				while (true)
				{
					if (ch == 42)
					{
						NextCh();
						if (ch == 47)
						{
							num--;
							if (num == 0)
							{
								oldEols = line - num3;
								NextCh();
								return true;
							}
							NextCh();
						}
					}
					else if (ch == 47)
					{
						NextCh();
						if (ch == 42)
						{
							num++;
							NextCh();
						}
					}
					else
					{
						if (ch == 65536)
						{
							break;
						}
						NextCh();
					}
				}
				return false;
			}
			buffer.Pos = num2;
			NextCh();
			line = num3;
			col = num4;
			charPos = num5;
			return false;
		}

		private void CheckLiteral()
		{
			_ = t.val;
		}

		private Token NextToken()
		{
			while (ch == 32 || (ch >= 9 && ch <= 10) || ch == 13)
			{
				NextCh();
			}
			if ((ch == 47 && Comment0()) || (ch == 47 && Comment1()))
			{
				return NextToken();
			}
			int num = 11;
			int num2 = pos;
			t = new Token();
			t.pos = pos;
			t.col = col;
			t.line = line;
			t.charPos = charPos;
			int num3 = (start.ContainsKey(ch) ? ((int)start[ch]) : 0);
			tlen = 0;
			AddCh();
			switch (num3)
			{
			case -1:
				t.kind = 0;
				break;
			case 0:
				if (num != 11)
				{
					tlen = num2 - t.pos;
					SetScannerBehindT();
				}
				t.kind = num;
				break;
			case 1:
				while (true)
				{
					num2 = pos;
					num = 2;
					if ((ch < 48 || ch > 57) && (ch < 65 || ch > 90) && (ch < 97 || ch > 122))
					{
						break;
					}
					AddCh();
				}
				t.kind = 2;
				break;
			case 2:
				if ((ch < 48 || ch > 57) && (ch < 65 || ch > 70) && (ch < 97 || ch > 102))
				{
					goto case 0;
				}
				AddCh();
				goto case 3;
			case 3:
				while (true)
				{
					num2 = pos;
					num = 3;
					if ((ch < 48 || ch > 57) && (ch < 65 || ch > 70) && (ch < 97 || ch > 102))
					{
						break;
					}
					AddCh();
				}
				t.kind = 3;
				break;
			case 4:
				if (ch != 43 && ch != 45)
				{
					goto case 0;
				}
				AddCh();
				goto case 5;
			case 5:
				if (ch < 48 || ch > 57)
				{
					goto case 0;
				}
				AddCh();
				goto case 6;
			case 6:
				while (true)
				{
					num2 = pos;
					num = 5;
					if (ch < 48 || ch > 57)
					{
						break;
					}
					AddCh();
				}
				t.kind = 5;
				break;
			case 7:
				if (ch != 68)
				{
					goto case 0;
				}
				AddCh();
				goto case 8;
			case 8:
				if (ch != 102)
				{
					goto case 0;
				}
				AddCh();
				goto case 9;
			case 9:
				if (ch != 70)
				{
					goto case 0;
				}
				AddCh();
				goto case 10;
			case 10:
				t.kind = 5;
				break;
			case 11:
				while (true)
				{
					num2 = pos;
					num = 6;
					if (ch != 33 && (ch < 35 || ch > 38) && (ch < 40 || ch > 47) && (ch < 58 || ch > 64) && (ch < 94 || ch > 96) && (ch < 123 || ch > 126))
					{
						break;
					}
					AddCh();
				}
				t.kind = 6;
				break;
			case 12:
				while (ch != 34)
				{
					if ((ch >= 32 && ch <= 33) || (ch >= 35 && ch <= 90) || (ch >= 94 && ch <= 126))
					{
						AddCh();
						continue;
					}
					goto case 0;
				}
				AddCh();
				goto case 20;
			case 13:
				while (ch != 39)
				{
					if ((ch >= 32 && ch <= 38) || (ch >= 40 && ch <= 90) || (ch >= 94 && ch <= 126))
					{
						AddCh();
						continue;
					}
					goto case 0;
				}
				AddCh();
				goto case 21;
			case 14:
				while (true)
				{
					num2 = pos;
					num = 9;
					if ((ch < 48 || ch > 57) && (ch < 65 || ch > 90) && (ch < 97 || ch > 122))
					{
						break;
					}
					AddCh();
				}
				t.kind = 9;
				break;
			case 15:
				t.kind = 10;
				break;
			case 16:
				while (true)
				{
					num2 = pos;
					num = 1;
					if (ch < 48 || ch > 57)
					{
						break;
					}
					AddCh();
				}
				if (ch == 46)
				{
					AddCh();
					goto case 22;
				}
				if (ch == 69 || ch == 101)
				{
					AddCh();
					goto case 4;
				}
				if (ch == 100)
				{
					AddCh();
					goto case 7;
				}
				t.kind = 1;
				break;
			case 17:
				num2 = pos;
				num = 6;
				if (ch == 33 || (ch >= 35 && ch <= 38) || (ch >= 40 && ch <= 47) || (ch >= 58 && ch <= 64) || (ch >= 94 && ch <= 96) || (ch >= 123 && ch <= 126))
				{
					AddCh();
					goto case 11;
				}
				if ((ch >= 65 && ch <= 90) || (ch >= 97 && ch <= 122))
				{
					AddCh();
					goto case 14;
				}
				t.kind = 6;
				break;
			case 18:
				num2 = pos;
				num = 1;
				if (ch >= 48 && ch <= 57)
				{
					AddCh();
					goto case 16;
				}
				if (ch == 88 || ch == 120)
				{
					AddCh();
					goto case 2;
				}
				if (ch == 46)
				{
					AddCh();
					goto case 22;
				}
				if (ch == 69 || ch == 101)
				{
					AddCh();
					goto case 4;
				}
				if (ch == 100)
				{
					AddCh();
					goto case 7;
				}
				t.kind = 1;
				break;
			case 19:
				num2 = pos;
				num = 6;
				if (ch >= 48 && ch <= 57)
				{
					AddCh();
					goto case 23;
				}
				if (ch == 33 || (ch >= 35 && ch <= 38) || (ch >= 40 && ch <= 47) || (ch >= 58 && ch <= 64) || (ch >= 94 && ch <= 96) || (ch >= 123 && ch <= 126))
				{
					AddCh();
					goto case 11;
				}
				t.kind = 6;
				break;
			case 20:
				num2 = pos;
				num = 7;
				if (ch == 34)
				{
					AddCh();
					goto case 12;
				}
				t.kind = 7;
				break;
			case 21:
				num2 = pos;
				num = 8;
				if (ch == 39)
				{
					AddCh();
					goto case 13;
				}
				t.kind = 8;
				break;
			case 22:
				num2 = pos;
				num = 4;
				if (ch >= 48 && ch <= 57)
				{
					AddCh();
					goto case 24;
				}
				if (ch == 69 || ch == 101)
				{
					AddCh();
					goto case 4;
				}
				if (ch == 100)
				{
					AddCh();
					goto case 7;
				}
				t.kind = 4;
				break;
			case 23:
				while (true)
				{
					num2 = pos;
					num = 4;
					if (ch < 48 || ch > 57)
					{
						break;
					}
					AddCh();
				}
				if (ch == 69 || ch == 101)
				{
					AddCh();
					goto case 4;
				}
				if (ch == 100)
				{
					AddCh();
					goto case 7;
				}
				t.kind = 4;
				break;
			case 24:
				while (true)
				{
					num2 = pos;
					num = 4;
					if (ch < 48 || ch > 57)
					{
						break;
					}
					AddCh();
				}
				if (ch == 69 || ch == 101)
				{
					AddCh();
					goto case 4;
				}
				if (ch == 100)
				{
					AddCh();
					goto case 7;
				}
				t.kind = 4;
				break;
			}
			t.val = new string(tval, 0, tlen);
			return t;
		}

		private void SetScannerBehindT()
		{
			buffer.Pos = t.pos;
			NextCh();
			line = t.line;
			col = t.col;
			charPos = t.charPos;
			for (int i = 0; i < tlen; i++)
			{
				NextCh();
			}
		}

		public Token Scan()
		{
			if (tokens.next == null)
			{
				return NextToken();
			}
			pt = (tokens = tokens.next);
			return tokens;
		}

		public Token Peek()
		{
			do
			{
				if (pt.next == null)
				{
					pt.next = NextToken();
				}
				pt = pt.next;
			}
			while (pt.kind > 11);
			return pt;
		}

		public void ResetPeek()
		{
			pt = tokens;
		}
	}
}
