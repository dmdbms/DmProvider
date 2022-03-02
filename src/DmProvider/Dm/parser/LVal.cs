namespace Dm.parser
{
	public class LVal
	{
		public enum Type
		{
			NORMAL,
			INT,
			DOUBLE,
			DECIMAL,
			STRING,
			HEX_INT,
			WHITESPACE_OR_COMMENT,
			NULL
		}

		public const int MAX_DEC_LEN = 38;

		public string value;

		public Type type;

		public int position;

		public LVal()
		{
			reset();
		}

		public LVal(string value, Type type)
		{
			this.value = value;
			this.type = type;
		}

		public virtual void reset()
		{
			value = "";
			type = Type.NORMAL;
		}

		public override string ToString()
		{
			return type.ToString() + ": " + value;
		}
	}
}
