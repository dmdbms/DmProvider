namespace NetTaste
{
	public class Parser
	{
		public const int _EOF = 0;

		public const int _integer = 1;

		public const int _identifier = 2;

		public const int _binteger = 3;

		public const int _decimal = 4;

		public const int _real = 5;

		public const int _op = 6;

		public const int _one_vard = 7;

		public const int _one_varq = 8;

		public const int _one_var = 9;

		public const int _end_word = 10;

		public const int maxT = 11;

		private const bool T = true;

		private const bool x = false;

		private const int minErrDist = 2;

		public Scanner scanner;

		public Errors errors;

		public Token t;

		public Token la;

		private int errDist = 2;

		private const int words = 0;

		private const int identifier = 1;

		public SymbolTable tab;

		private static readonly bool[,] set = new bool[2, 13]
		{
			{
				true, false, false, false, false, false, false, false, false, false,
				false, false, false
			},
			{
				false, true, true, true, false, false, true, true, true, true,
				false, false, false
			}
		};

		public Parser(Scanner scanner)
		{
			this.scanner = scanner;
			errors = new Errors();
		}

		private void SynErr(int n)
		{
			if (errDist >= 2)
			{
				errors.SynErr(la.line, la.col, n);
			}
			errDist = 0;
		}

		public void SemErr(string msg)
		{
			if (errDist >= 2)
			{
				errors.SemErr(t.line, t.col, msg);
			}
			errDist = 0;
		}

		private void Get()
		{
			while (true)
			{
				t = la;
				la = scanner.Scan();
				if (la.kind <= 11)
				{
					break;
				}
				la = t;
			}
			errDist++;
		}

		private void Expect(int n)
		{
			if (la.kind == n)
			{
				Get();
			}
			else
			{
				SynErr(n);
			}
		}

		private bool StartOf(int s)
		{
			return set[s, la.kind];
		}

		private void ExpectWeak(int n, int follow)
		{
			if (la.kind == n)
			{
				Get();
				return;
			}
			SynErr(n);
			while (!StartOf(follow))
			{
				Get();
			}
		}

		private bool WeakSeparator(int n, int syFol, int repFol)
		{
			int kind = la.kind;
			if (kind == n)
			{
				Get();
				return true;
			}
			if (StartOf(repFol))
			{
				return false;
			}
			SynErr(n);
			while (!set[syFol, kind] && !set[repFol, kind] && !set[0, kind])
			{
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}

		private void NetTaste()
		{
			while (StartOf(1))
			{
				switch (la.kind)
				{
				case 2:
					Get();
					tab.NewObj(t.val, 0);
					break;
				case 6:
					Get();
					tab.NewObj(t.val, 1);
					break;
				case 9:
					Get();
					tab.NewObj(t.val, 1);
					break;
				case 7:
					Get();
					tab.NewObj(t.val, 1);
					break;
				case 8:
					Get();
					tab.NewObj(t.val, 1);
					break;
				case 1:
					Get();
					tab.NewObj(t.val, 1);
					break;
				case 3:
					Get();
					tab.NewObj(t.val, 1);
					break;
				}
			}
			Expect(10);
		}

		public void Parse()
		{
			la = new Token();
			la.val = "";
			Get();
			NetTaste();
			Expect(0);
		}
	}
}
