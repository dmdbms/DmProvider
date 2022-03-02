using System;

namespace NetTaste
{
	public class SymbolTable
	{
		private const int words = 0;

		private const int identifier = 1;

		public Obj topScope;

		public SymbolTable(Parser parser)
		{
			Obj obj = new Obj();
			topScope = null;
			obj.name = "";
			obj.kind = 0;
			obj.locals = null;
			obj.nextAdr = 0;
			obj.next = topScope;
			topScope = obj;
		}

		public Obj NewObj(string name, int kind)
		{
			Obj obj = new Obj();
			obj.kind = kind;
			obj.name = name;
			Obj obj2 = topScope.locals;
			Obj obj3 = null;
			while (obj2 != null)
			{
				obj3 = obj2;
				obj2 = obj2.next;
			}
			if (obj3 == null)
			{
				topScope.locals = obj;
			}
			else
			{
				obj3.next = obj;
			}
			return obj;
		}

		public bool Replace(string name)
		{
			bool result = false;
			for (Obj next = topScope; next != null; next = next.next)
			{
				for (Obj obj = next.locals; obj != null; obj = obj.next)
				{
					if (obj.kind == 0 && obj.name == name)
					{
						obj.name = "\"" + name.ToUpper() + "\"";
						result = true;
					}
				}
			}
			return result;
		}

		public void Print()
		{
			for (Obj next = topScope; next != null; next = next.next)
			{
				for (Obj obj = next.locals; obj != null; obj = obj.next)
				{
					Console.Write(obj.name + " ");
				}
			}
			Console.Write("\n");
		}

		public string GetString()
		{
			string text = null;
			for (Obj next = topScope; next != null; next = next.next)
			{
				for (Obj obj = next.locals; obj != null; obj = obj.next)
				{
					text = text + obj.name + " ";
				}
			}
			return text;
		}
	}
}
