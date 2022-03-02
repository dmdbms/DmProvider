using System;
using System.IO;

namespace Dm.parser
{
	public class Lexer
	{
		public const int YYEOF = -1;

		private const int ZZ_BUFFERSIZE = 16384;

		public const int YYINITIAL = 0;

		public const int xc = 2;

		public const int xq = 4;

		public const int xdq = 6;

		public const int xsb = 8;

		public const int xbin = 10;

		public const int xhex = 12;

		public const int xhint = 14;

		public const int xq2 = 16;

		public const int xq2_2 = 18;

		private static readonly int[] ZZ_LEXSTATE = new int[20]
		{
			0, 0, 1, 1, 2, 2, 3, 3, 4, 4,
			5, 5, 6, 6, 4, 4, 7, 7, 8, 8
		};

		private const string ZZ_CMAP_PACKED = "\t\0\u0001\u0016\u0001\u0015\u0001\u0018\u0001\u0016\u0001\u0015\u0012\0\u0001\u0016\u0001\u000f\u0001\u0002\u0002\n\u0002\u000f\u0001\u0001\u0002\u000f\u0001\u0004\u0001\u0013\u0001\u000f\u0001\u0017\u0001\u000e\u0001\u0003\u0001\u0010\t\v\u0001\f\u0001\u000f\u0001\u000f\u0001\r\u0003\u000f\u0001\u0011\u0001\b\u0001\u0011\u0001\u0014\u0001\u0012\u0001\u0014\u0002\n\u0001\u001c\u0002\n\u0001\u001b\u0001\n\u0001\u0019\u0001\u001e\u0001\n\u0001\a\u0001\n\u0001\u001d\u0001\u001f\u0001\u001a\u0002\n\u0001\t\u0002\n\u0001\u0005\u0001\0\u0001\u0006\u0001\u000f\u0001\n\u0001\0\u0001\u0011\u0001\b\u0001\u0011\u0001\u0014\u0001\u0012\u0001\u0014\u0002\n\u0001\u001c\u0002\n\u0001\u001b\u0001\n\u0001\u0019\u0001\u001e\u0001\n\u0001\a\u0001\n\u0001\u001d\u0001\u001f\u0001\u001a\u0002\n\u0001\t\u0002\n\u0001\u000f\u0001\u000f\u0002\u000f\u0001\0\u0005\n\u0001\nz\nἨ\0\u0001\u0018\u0001\u0018\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\udfe6\0";

		private static readonly char[] ZZ_CMAP = zzUnpackCMap("\t\0\u0001\u0016\u0001\u0015\u0001\u0018\u0001\u0016\u0001\u0015\u0012\0\u0001\u0016\u0001\u000f\u0001\u0002\u0002\n\u0002\u000f\u0001\u0001\u0002\u000f\u0001\u0004\u0001\u0013\u0001\u000f\u0001\u0017\u0001\u000e\u0001\u0003\u0001\u0010\t\v\u0001\f\u0001\u000f\u0001\u000f\u0001\r\u0003\u000f\u0001\u0011\u0001\b\u0001\u0011\u0001\u0014\u0001\u0012\u0001\u0014\u0002\n\u0001\u001c\u0002\n\u0001\u001b\u0001\n\u0001\u0019\u0001\u001e\u0001\n\u0001\a\u0001\n\u0001\u001d\u0001\u001f\u0001\u001a\u0002\n\u0001\t\u0002\n\u0001\u0005\u0001\0\u0001\u0006\u0001\u000f\u0001\n\u0001\0\u0001\u0011\u0001\b\u0001\u0011\u0001\u0014\u0001\u0012\u0001\u0014\u0002\n\u0001\u001c\u0002\n\u0001\u001b\u0001\n\u0001\u0019\u0001\u001e\u0001\n\u0001\a\u0001\n\u0001\u001d\u0001\u001f\u0001\u001a\u0002\n\u0001\t\u0002\n\u0001\u000f\u0001\u000f\u0002\u000f\u0001\0\u0005\n\u0001\nz\nἨ\0\u0001\u0018\u0001\u0018\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\uffff\0\udfe6\0");

		private static readonly int[] ZZ_ACTION = zzUnpackAction();

		private const string ZZ_ACTION_PACKED_0 = "\t\0\u0001\u0001\u0001\u0002\u0001\u0003\u0002\u0004\u0004\u0005\u0001\u0006\u0002\u0004\u0001\u0006\u0001\a\u0001\u0004\u0002\u0005\u0001\b\u0002\t\u0001\n\u0001\v\u0001\f\u0001\r\u0001\u000e\u0001\u000f\u0001\u0010\u0001\u0011\u0001\u0012\u0001\u0013\u0001\u0014\u0001\u0015\u0001\u0016\u0001\a\u0001\u0017\u0001\0\u0001\u0018\u0001\u0019\u0001\u001a\u0001\0\u0001\u001b\u0001\u001c\u0001\u001d\u0001\u001a\u0001\u001e\u0001\0\u0003\u0005\u0001\u001f\u0001 \u0001\0\u0001!\u0002\0\u0001\"\u0004\0\u0001#\u0001$\u0001\u001b\u0001\0\u0001%\u0002\u0005\u0003\0\u0001&\u0001'\u0001(\u0001)\u0010\0\u0001*\u0001\0\u0001+\u0001*\u0001+";

		private static readonly int[] ZZ_ROWMAP = zzUnpackRowMap();

		private const string ZZ_ROWMAP_PACKED_0 = "\0\0\0 \0@\0`\0\u0080\0\u00a0\0À\0à\0Ā\0\u0080\0\u0080\0\u0080\0Ġ\0\u0080\0ŀ\0Š\0ƀ\0Ơ\0ǀ\0Ǡ\0Ȁ\0Ƞ\0\u0080\0ɀ\0ɠ\0ʀ\0ʠ\0ˀ\0ˠ\0\u0300\0\u0320\0\u0340\0\u0360\0\u0380\0Π\0π\0Ϡ\0Ѐ\0\u0080\0\u0080\0\u0080\0\u0080\0Р\0\u0080\0р\0\u0080\0\u0080\0Ѡ\0Ҁ\0\u0080\0\u0080\0\u0080\0Ҡ\0\u0080\0Ӏ\0Ӡ\0Ԁ\0Ԡ\0\u0080\0\u0080\0ˠ\0\u0080\0Հ\0\u0560\0\u0080\0ր\0Π\0\u05a0\0Ϡ\0\u0080\0\u0080\0׀\0׀\0Ӏ\0נ\0\u0600\0ؠ\0ـ\0٠\0\u0080\0\u0080\0\u0080\0Ơ\0ڀ\0ڠ\0ۀ\0\u06e0\0܀\0ܠ\0\u0740\0ݠ\0ހ\0ޠ\0߀\0ߠ\0ࠀ\0\u0820\0ࡀ\0\u0860\0\u0080\0\u0880\0\u0080\0\u06e0\0ܠ";

		private static readonly int[] ZZ_TRANS = zzUnpackTrans();

		private const string ZZ_TRANS_PACKED_0 = "\u0001\n\u0001\v\u0001\f\u0001\r\u0003\u000e\u0001\u000f\u0001\u0010\u0001\u0011\u0001\u0012\u0001\u0013\u0001\u0014\u0001\u000e\u0001\u0015\u0001\u000e\u0001\u0016\u0002\u0012\u0001\u000e\u0001\u0012\u0002\u0017\u0001\u0018\u0001\0\u0001\u0019\u0002\u0012\u0001\u001a\u0003\u0012\u0003\u001b\u0001\u001c\u0001\u001d\u001b\u001b\u0001\u001e\u0001\u001f\u001e\u001e\u0002 \u0001!\u001d  \0\u0001\"\u0001#\u001e\"\u0001$\u0001%\u001e$\u0006&\u0001'\u0019&\u0001(\u0001)\u0004(\u0001*\u0019(\u0003\0\u0001+\u0001,\u001c\0\u0001-\u0005\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\a\u0012\u0001\0\u0001.\u0005\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\a\u0012\u0001\0\u0001/\u0005\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\a\u0012\a\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\a\u0012\v\0\u0001\u0013\u0002\0\u00010\u0001\0\u0001\u0013\u0001\0\u00011\u0001\0\u00012\u0018\0\u00013\u0016\0\u00014\u0006\0\u00015\u0002\0\u00016\u0001\0\u00015\u0018\0\u00017\u0001\0\u0001\u0013\u0002\0\u00010\u0001\0\u0001\u0013\u0001\0\u00011\u0001\0\u00012\"\0\u0001+\u000f\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\u0001\u0012\u00018\u0003\u0012\u00019\u0001\u0012\a\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\u0004\u0012\u0001:\u0002\u0012\u0003\u001b\u0002\0\u001b\u001b\u0004\0\u0001;\u001e\0\u0001<\u0001=\u001b\0\u0001\u001e\u0001\0\u001e\u001e\u0001\0\u0001>\u0013\0\u0001?\u0001@\t\0\u0002 \u0001\0\u001d \u0002\0\u0001A\u001d\0\u0001\"\u0001\0\u001e\"\u0015\0\u0001B\u0001C\t\0\u0001$\u0001\0\u001e$\u0015\0\u0001D\u0001E\t\0\u0006&\u0001\0\u0019&\u0015+\u0001\0\n+\u0005\0\u0001F%\0\u00015\u0002\0\u0001G\u0001\0\u00015\u0001\0\u00011\u0001\0\u00012\u0016\0\u0001H\u0004\0\u0001H\u0002\0\u0001I\u0003\0\u0001I\u0013\0\u00015\u0004\0\u00015\u0001\0\u00011\u0001\0\u00012\u0013\0\u0001J\u0002\0\u0001J\u0004\0\u0003J\u0001\0\u0001J\u0012\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\u0002\u0012\u0001K\u0004\u0012\a\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\u0006\u0012\u0001L\u0003\0\u0001M\u0003\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0002N\u0001O\u0001\0\a\u0012\u0001\0\u0001P\u0013\0\u0002?\u001e\0\u0001?\u0001@\n\0\u0001Q\u0013\0\u0002B\n\0\u0001R\u0013\0\u0002D\u0014\0\u0001H\u0004\0\u0001H\u0016\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\u0002\u0012\u0001S\u0004\u0012\u0003\0\u0001T\u0003\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0002U\u0001V\u0001\0\a\u0012\u0003\0\u0001W\u001f\0\u0001M\u0011\0\u0002N\u0001O\u0001\0\u0001X\u001d\0\u0001W\v\0\u0001Y\u001f\0\u0001T\u0011\0\u0002U\u0001V\u0001\0\u0001Z\u001d\0\u0001Y\b\0\u0015W\u0001N\u0003W\u0001[\u0006W\u001a\0\u0001\\\u0005\0\u0015Y\u0001U\u0003Y\u0001]\u0006Y\u001a\0\u0001^\u0005\0\u0015W\u0001N\u0003W\u0001[\u0001_\u0005W\u001b\0\u0001`\u0004\0\u0015Y\u0001U\u0003Y\u0001]\u0001a\u0005Y\u001b\0\u0001b\u0004\0\u0015W\u0001N\u0003W\u0001[\u0001W\u0001c\u0004W\u001b\0\u0001d\u0004\0\u0015Y\u0001U\u0003Y\u0001]\u0001Y\u0001e\u0004Y\u001b\0\u0001f\u0004\0\u0015W\u0001N\u0003W\u0001[\u0001W\u0001g\u0004W\u0015Y\u0001U\u0003Y\u0001]\u0001Y\u0001h\u0004Y";

		private const int ZZ_UNKNOWN_ERROR = 0;

		private const int ZZ_NO_MATCH = 1;

		private const int ZZ_PUSHBACK_2BIG = 2;

		private static readonly string[] ZZ_ERROR_MSG = new string[3] { "Unknown internal scanner error", "Error: could not match input", "Error: pushback value was too large" };

		private static readonly int[] ZZ_ATTRIBUTE = zzUnpackAttribute();

		private const string ZZ_ATTRIBUTE_PACKED_0 = "\u0004\0\u0001\b\u0004\0\u0003\t\u0001\u0001\u0001\t\b\u0001\u0001\t\u000f\u0001\u0004\t\u0001\u0001\u0001\t\u0001\0\u0002\t\u0001\u0001\u0001\0\u0003\t\u0001\u0001\u0001\t\u0001\0\u0003\u0001\u0002\t\u0001\0\u0001\t\u0002\0\u0001\t\u0004\0\u0002\t\u0001\u0001\u0001\0\u0003\u0001\u0003\0\u0003\t\u0001\u0001\u0010\0\u0001\t\u0001\0\u0001\t\u0002\u0001";

		private TextReader zzReader;

		private int zzState;

		private int zzLexicalState;

		private char[] zzBuffer = new char[16384];

		private int zzMarkedPos;

		private int zzCurrentPos;

		private int zzStartRead;

		private int zzEndRead;

		private int yyline;

		private int yychar;

		private int yycolumn;

		private bool zzAtBOL = true;

		private bool zzAtEOF;

		private bool zzEOFDone;

		private int zzFinalHighSurrogate;

		private string ltstr;

		private bool debug_Renamed;

		public static readonly char MIN_HIGH_SURROGATE = '\ud800';

		public static readonly char MAX_HIGH_SURROGATE = '\udbff';

		public static readonly char MIN_LOW_SURROGATE = '\udc00';

		public static readonly char MAX_LOW_SURROGATE = '\udfff';

		public static readonly int MIN_SUPPLEMENTARY_CODE_POINT = 65536;

		private static int[] zzUnpackAction()
		{
			int[] result = new int[104];
			int offset = 0;
			offset = zzUnpackAction("\t\0\u0001\u0001\u0001\u0002\u0001\u0003\u0002\u0004\u0004\u0005\u0001\u0006\u0002\u0004\u0001\u0006\u0001\a\u0001\u0004\u0002\u0005\u0001\b\u0002\t\u0001\n\u0001\v\u0001\f\u0001\r\u0001\u000e\u0001\u000f\u0001\u0010\u0001\u0011\u0001\u0012\u0001\u0013\u0001\u0014\u0001\u0015\u0001\u0016\u0001\a\u0001\u0017\u0001\0\u0001\u0018\u0001\u0019\u0001\u001a\u0001\0\u0001\u001b\u0001\u001c\u0001\u001d\u0001\u001a\u0001\u001e\u0001\0\u0003\u0005\u0001\u001f\u0001 \u0001\0\u0001!\u0002\0\u0001\"\u0004\0\u0001#\u0001$\u0001\u001b\u0001\0\u0001%\u0002\u0005\u0003\0\u0001&\u0001'\u0001(\u0001)\u0010\0\u0001*\u0001\0\u0001+\u0001*\u0001+", offset, result);
			return result;
		}

		private static int zzUnpackAction(string packed, int offset, int[] result)
		{
			int num = 0;
			int result2 = offset;
			int length = packed.Length;
			while (num < length)
			{
				int num2 = packed[num++];
				int num3 = packed[num++];
				do
				{
					result[result2++] = num3;
				}
				while (--num2 > 0);
			}
			return result2;
		}

		private static int[] zzUnpackRowMap()
		{
			int[] result = new int[104];
			int offset = 0;
			offset = zzUnpackRowMap("\0\0\0 \0@\0`\0\u0080\0\u00a0\0À\0à\0Ā\0\u0080\0\u0080\0\u0080\0Ġ\0\u0080\0ŀ\0Š\0ƀ\0Ơ\0ǀ\0Ǡ\0Ȁ\0Ƞ\0\u0080\0ɀ\0ɠ\0ʀ\0ʠ\0ˀ\0ˠ\0\u0300\0\u0320\0\u0340\0\u0360\0\u0380\0Π\0π\0Ϡ\0Ѐ\0\u0080\0\u0080\0\u0080\0\u0080\0Р\0\u0080\0р\0\u0080\0\u0080\0Ѡ\0Ҁ\0\u0080\0\u0080\0\u0080\0Ҡ\0\u0080\0Ӏ\0Ӡ\0Ԁ\0Ԡ\0\u0080\0\u0080\0ˠ\0\u0080\0Հ\0\u0560\0\u0080\0ր\0Π\0\u05a0\0Ϡ\0\u0080\0\u0080\0׀\0׀\0Ӏ\0נ\0\u0600\0ؠ\0ـ\0٠\0\u0080\0\u0080\0\u0080\0Ơ\0ڀ\0ڠ\0ۀ\0\u06e0\0܀\0ܠ\0\u0740\0ݠ\0ހ\0ޠ\0߀\0ߠ\0ࠀ\0\u0820\0ࡀ\0\u0860\0\u0080\0\u0880\0\u0080\0\u06e0\0ܠ", offset, result);
			return result;
		}

		private static int zzUnpackRowMap(string packed, int offset, int[] result)
		{
			int num = 0;
			int result2 = offset;
			int length = packed.Length;
			while (num < length)
			{
				int num2 = (int)((uint)packed[num++] << 16);
				result[result2++] = num2 | packed[num++];
			}
			return result2;
		}

		private static int[] zzUnpackTrans()
		{
			int[] result = new int[2208];
			int offset = 0;
			offset = zzUnpackTrans("\u0001\n\u0001\v\u0001\f\u0001\r\u0003\u000e\u0001\u000f\u0001\u0010\u0001\u0011\u0001\u0012\u0001\u0013\u0001\u0014\u0001\u000e\u0001\u0015\u0001\u000e\u0001\u0016\u0002\u0012\u0001\u000e\u0001\u0012\u0002\u0017\u0001\u0018\u0001\0\u0001\u0019\u0002\u0012\u0001\u001a\u0003\u0012\u0003\u001b\u0001\u001c\u0001\u001d\u001b\u001b\u0001\u001e\u0001\u001f\u001e\u001e\u0002 \u0001!\u001d  \0\u0001\"\u0001#\u001e\"\u0001$\u0001%\u001e$\u0006&\u0001'\u0019&\u0001(\u0001)\u0004(\u0001*\u0019(\u0003\0\u0001+\u0001,\u001c\0\u0001-\u0005\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\a\u0012\u0001\0\u0001.\u0005\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\a\u0012\u0001\0\u0001/\u0005\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\a\u0012\a\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\a\u0012\v\0\u0001\u0013\u0002\0\u00010\u0001\0\u0001\u0013\u0001\0\u00011\u0001\0\u00012\u0018\0\u00013\u0016\0\u00014\u0006\0\u00015\u0002\0\u00016\u0001\0\u00015\u0018\0\u00017\u0001\0\u0001\u0013\u0002\0\u00010\u0001\0\u0001\u0013\u0001\0\u00011\u0001\0\u00012\"\0\u0001+\u000f\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\u0001\u0012\u00018\u0003\u0012\u00019\u0001\u0012\a\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\u0004\u0012\u0001:\u0002\u0012\u0003\u001b\u0002\0\u001b\u001b\u0004\0\u0001;\u001e\0\u0001<\u0001=\u001b\0\u0001\u001e\u0001\0\u001e\u001e\u0001\0\u0001>\u0013\0\u0001?\u0001@\t\0\u0002 \u0001\0\u001d \u0002\0\u0001A\u001d\0\u0001\"\u0001\0\u001e\"\u0015\0\u0001B\u0001C\t\0\u0001$\u0001\0\u001e$\u0015\0\u0001D\u0001E\t\0\u0006&\u0001\0\u0019&\u0015+\u0001\0\n+\u0005\0\u0001F%\0\u00015\u0002\0\u0001G\u0001\0\u00015\u0001\0\u00011\u0001\0\u00012\u0016\0\u0001H\u0004\0\u0001H\u0002\0\u0001I\u0003\0\u0001I\u0013\0\u00015\u0004\0\u00015\u0001\0\u00011\u0001\0\u00012\u0013\0\u0001J\u0002\0\u0001J\u0004\0\u0003J\u0001\0\u0001J\u0012\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\u0002\u0012\u0001K\u0004\u0012\a\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\u0006\u0012\u0001L\u0003\0\u0001M\u0003\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0002N\u0001O\u0001\0\a\u0012\u0001\0\u0001P\u0013\0\u0002?\u001e\0\u0001?\u0001@\n\0\u0001Q\u0013\0\u0002B\n\0\u0001R\u0013\0\u0002D\u0014\0\u0001H\u0004\0\u0001H\u0016\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0004\0\u0002\u0012\u0001S\u0004\u0012\u0003\0\u0001T\u0003\0\u0005\u0012\u0004\0\u0003\u0012\u0001\0\u0001\u0012\u0002U\u0001V\u0001\0\a\u0012\u0003\0\u0001W\u001f\0\u0001M\u0011\0\u0002N\u0001O\u0001\0\u0001X\u001d\0\u0001W\v\0\u0001Y\u001f\0\u0001T\u0011\0\u0002U\u0001V\u0001\0\u0001Z\u001d\0\u0001Y\b\0\u0015W\u0001N\u0003W\u0001[\u0006W\u001a\0\u0001\\\u0005\0\u0015Y\u0001U\u0003Y\u0001]\u0006Y\u001a\0\u0001^\u0005\0\u0015W\u0001N\u0003W\u0001[\u0001_\u0005W\u001b\0\u0001`\u0004\0\u0015Y\u0001U\u0003Y\u0001]\u0001a\u0005Y\u001b\0\u0001b\u0004\0\u0015W\u0001N\u0003W\u0001[\u0001W\u0001c\u0004W\u001b\0\u0001d\u0004\0\u0015Y\u0001U\u0003Y\u0001]\u0001Y\u0001e\u0004Y\u001b\0\u0001f\u0004\0\u0015W\u0001N\u0003W\u0001[\u0001W\u0001g\u0004W\u0015Y\u0001U\u0003Y\u0001]\u0001Y\u0001h\u0004Y", offset, result);
			return result;
		}

		private static int zzUnpackTrans(string packed, int offset, int[] result)
		{
			int num = 0;
			int result2 = offset;
			int length = packed.Length;
			while (num < length)
			{
				int num2 = packed[num++];
				int num3 = packed[num++];
				num3--;
				do
				{
					result[result2++] = num3;
				}
				while (--num2 > 0);
			}
			return result2;
		}

		private static int[] zzUnpackAttribute()
		{
			int[] result = new int[104];
			int offset = 0;
			offset = zzUnpackAttribute("\u0004\0\u0001\b\u0004\0\u0003\t\u0001\u0001\u0001\t\b\u0001\u0001\t\u000f\u0001\u0004\t\u0001\u0001\u0001\t\u0001\0\u0002\t\u0001\u0001\u0001\0\u0003\t\u0001\u0001\u0001\t\u0001\0\u0003\u0001\u0002\t\u0001\0\u0001\t\u0002\0\u0001\t\u0004\0\u0002\t\u0001\u0001\u0001\0\u0003\u0001\u0003\0\u0003\t\u0001\u0001\u0010\0\u0001\t\u0001\0\u0001\t\u0002\u0001", offset, result);
			return result;
		}

		private static int zzUnpackAttribute(string packed, int offset, int[] result)
		{
			int num = 0;
			int result2 = offset;
			int length = packed.Length;
			while (num < length)
			{
				int num2 = packed[num++];
				int num3 = packed[num++];
				do
				{
					result[result2++] = num3;
				}
				while (--num2 > 0);
			}
			return result2;
		}

		private void debug(string info)
		{
			_ = debug_Renamed;
		}

		public virtual void yyerror(string msg)
		{
			string text = "(line: " + yyline + ", column: " + yycolumn + ", char: " + yychar + ")";
			if (string.IsNullOrEmpty(msg))
			{
				throw new Exception("syntex error" + text);
			}
			throw new Exception("syntex error" + text + ": " + msg);
		}

		public Lexer(TextReader @in, bool debug)
		{
			debug_Renamed = debug;
			zzReader = @in;
		}

		private static char[] zzUnpackCMap(string packed)
		{
			char[] array = new char[1114112];
			int num = 0;
			int num2 = 0;
			while (num < 208)
			{
				int num3 = packed[num++];
				char c = packed[num++];
				do
				{
					array[num2++] = c;
				}
				while (--num3 > 0);
			}
			return array;
		}

		private bool zzRefill()
		{
			if (zzStartRead > 0)
			{
				zzEndRead += zzFinalHighSurrogate;
				zzFinalHighSurrogate = 0;
				Array.Copy(zzBuffer, zzStartRead, zzBuffer, 0, zzEndRead - zzStartRead);
				zzEndRead -= zzStartRead;
				zzCurrentPos -= zzStartRead;
				zzMarkedPos -= zzStartRead;
				zzStartRead = 0;
			}
			if (zzCurrentPos >= zzBuffer.Length - zzFinalHighSurrogate)
			{
				char[] destinationArray = new char[zzBuffer.Length * 2];
				Array.Copy(zzBuffer, 0, destinationArray, 0, zzBuffer.Length);
				zzBuffer = destinationArray;
				zzEndRead += zzFinalHighSurrogate;
				zzFinalHighSurrogate = 0;
			}
			int num = zzBuffer.Length - zzEndRead;
			int num2 = zzReader.Read(zzBuffer, zzEndRead, num);
			if (num2 > 0)
			{
				zzEndRead += num2;
				if (num2 == num && char.IsHighSurrogate(zzBuffer[zzEndRead - 1]))
				{
					zzEndRead--;
					zzFinalHighSurrogate = 1;
				}
				return false;
			}
			return true;
		}

		public void yyclose()
		{
			zzAtEOF = true;
			zzEndRead = zzStartRead;
			if (zzReader != null)
			{
				zzReader.Close();
			}
		}

		public void yyreset(TextReader reader)
		{
			zzReader = reader;
			zzAtBOL = true;
			zzAtEOF = false;
			zzEOFDone = false;
			zzEndRead = (zzStartRead = 0);
			zzCurrentPos = (zzMarkedPos = 0);
			zzFinalHighSurrogate = 0;
			yyline = (yychar = (yycolumn = 0));
			zzLexicalState = 0;
			if (zzBuffer.Length > 16384)
			{
				zzBuffer = new char[16384];
			}
		}

		public int yystate()
		{
			return zzLexicalState;
		}

		public void yybegin(int newState)
		{
			zzLexicalState = newState;
		}

		public string yytext()
		{
			return new string(zzBuffer, zzStartRead, zzMarkedPos - zzStartRead);
		}

		public char yycharat(int pos)
		{
			return zzBuffer[zzStartRead + pos];
		}

		public int yylength()
		{
			return zzMarkedPos - zzStartRead;
		}

		private void zzScanError(int errorCode)
		{
			string message;
			try
			{
				message = ZZ_ERROR_MSG[errorCode];
			}
			catch (IndexOutOfRangeException)
			{
				message = ZZ_ERROR_MSG[0];
			}
			throw new Exception(message);
		}

		public virtual void yypushback(int number)
		{
			if (number > yylength())
			{
				zzScanError(2);
			}
			zzMarkedPos -= number;
		}

		public virtual LVal yylex()
		{
			int num = zzEndRead;
			char[] array = zzBuffer;
			char[] zZ_CMAP = ZZ_CMAP;
			int[] zZ_TRANS = ZZ_TRANS;
			int[] zZ_ROWMAP = ZZ_ROWMAP;
			int[] zZ_ATTRIBUTE = ZZ_ATTRIBUTE;
			while (true)
			{
				int num2 = zzMarkedPos;
				yychar += num2 - zzStartRead;
				bool flag = false;
				int num4;
				int i;
				for (i = zzStartRead; i < num2; i += num4)
				{
					int num3 = codePointAt(array, i, num2);
					num4 = charCount(num3);
					switch (num3)
					{
					case 11:
					case 12:
					case 133:
					case 8232:
					case 8233:
						yyline++;
						yycolumn = 0;
						flag = false;
						break;
					case 13:
						yyline++;
						yycolumn = 0;
						flag = true;
						break;
					case 10:
						if (flag)
						{
							flag = false;
							break;
						}
						yyline++;
						yycolumn = 0;
						break;
					default:
						flag = false;
						yycolumn += num4;
						break;
					}
				}
				if (flag)
				{
					bool flag2;
					if (num2 < num)
					{
						flag2 = array[num2] == '\n';
					}
					else if (zzAtEOF)
					{
						flag2 = false;
					}
					else
					{
						bool num5 = zzRefill();
						num = zzEndRead;
						num2 = zzMarkedPos;
						array = zzBuffer;
						flag2 = !num5 && array[num2] == '\n';
					}
					if (flag2)
					{
						yyline--;
					}
				}
				int num6 = -1;
				i = (zzCurrentPos = (zzStartRead = num2));
				zzState = ZZ_LEXSTATE[zzLexicalState];
				int num7 = zZ_ATTRIBUTE[zzState];
				if ((num7 & 1) == 1)
				{
					num6 = zzState;
				}
				int num8;
				while (true)
				{
					if (i < num)
					{
						num8 = codePointAt(array, i, num);
						i += charCount(num8);
					}
					else
					{
						if (zzAtEOF)
						{
							num8 = -1;
							break;
						}
						zzCurrentPos = i;
						zzMarkedPos = num2;
						bool num9 = zzRefill();
						i = zzCurrentPos;
						num2 = zzMarkedPos;
						array = zzBuffer;
						num = zzEndRead;
						if (num9)
						{
							num8 = -1;
							break;
						}
						num8 = codePointAt(array, i, num);
						i += charCount(num8);
					}
					int num10 = zZ_TRANS[zZ_ROWMAP[zzState] + zZ_CMAP[num8]];
					if (num10 == -1)
					{
						break;
					}
					zzState = num10;
					num7 = zZ_ATTRIBUTE[zzState];
					if ((num7 & 1) == 1)
					{
						num6 = zzState;
						num2 = i;
						if ((num7 & 8) == 8)
						{
							break;
						}
					}
				}
				zzMarkedPos = num2;
				if (num8 == -1 && zzStartRead == zzCurrentPos)
				{
					zzAtEOF = true;
					switch (zzLexicalState)
					{
					case 105:
					case 106:
					case 107:
					case 108:
					case 109:
					case 110:
					case 111:
						break;
					case 2:
						debug("<xc><<EOF>>");
						yybegin(0);
						yyerror("unterminated /* comment");
						break;
					case 4:
						debug("<xq><<EOF>>");
						yybegin(0);
						yyerror("unterminated quoted string");
						break;
					case 6:
						debug("<xdq><<EOF>>");
						yybegin(0);
						yyerror("unterminated quoted identifier");
						break;
					case 10:
						debug("<xbin><<EOF>>");
						yybegin(0);
						yyerror("unterminated binary string literal");
						break;
					case 12:
						debug("<xhex><<EOF>>");
						yybegin(0);
						yyerror("unterminated hexadecimal integer");
						break;
					case 16:
						yybegin(0);
						yyerror("unterminated q2 string");
						break;
					case 18:
						yybegin(0);
						yyerror("unterminated q2 string");
						break;
					default:
						return null;
					}
					continue;
				}
				switch ((num6 < 0) ? num6 : ZZ_ACTION[num6])
				{
				case 44:
				case 45:
				case 46:
				case 47:
				case 48:
				case 49:
				case 50:
				case 51:
				case 52:
				case 53:
				case 54:
				case 55:
				case 56:
				case 57:
				case 58:
				case 59:
				case 60:
				case 61:
				case 62:
				case 63:
				case 64:
				case 65:
				case 66:
				case 67:
				case 68:
				case 69:
				case 70:
				case 71:
				case 72:
				case 73:
				case 74:
				case 75:
				case 76:
				case 77:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 83:
				case 84:
				case 85:
				case 86:
					break;
				case 1:
					debug("{other}");
					return new LVal(yytext(), LVal.Type.NORMAL);
				case 2:
					debug("{xq_start}");
					yybegin(4);
					ltstr = "";
					break;
				case 3:
					debug("{xdq_start}");
					yybegin(6);
					ltstr = "";
					ltstr += yytext();
					break;
				case 4:
					debug("{self} | {op_chars}");
					return new LVal(yytext(), LVal.Type.NORMAL);
				case 5:
					debug("{identifier}");
					return new LVal(yytext(), LVal.Type.NORMAL);
				case 6:
					debug("{integer}");
					return new LVal(yytext(), LVal.Type.INT);
				case 7:
					debug("{whitespace} | {comment} | {c_line_comment}");
					return new LVal(yytext(), LVal.Type.WHITESPACE_OR_COMMENT);
				case 8:
					debug("<xc>{xc_inside}");
					ltstr += yytext();
					break;
				case 9:
					debug("<xc>[\\/] | <xc>[\\*]");
					ltstr += yytext();
					break;
				case 10:
					debug("<xq>{xq_inside}");
					ltstr += yytext();
					break;
				case 11:
					debug("<xq>{xq_stop}");
					yybegin(0);
					return new LVal(ltstr, LVal.Type.STRING);
				case 12:
					debug("<xdq>{xdq_inside}");
					ltstr += yytext();
					break;
				case 13:
					debug("<xdq>{xdq_stop}");
					yybegin(0);
					ltstr += yytext();
					return new LVal(ltstr, LVal.Type.NORMAL);
				case 14:
					debug("<xbin>{xbin_inside}");
					ltstr += yytext();
					break;
				case 15:
					debug("<xbin>{xbin_stop}");
					yybegin(0);
					ltstr += yytext();
					return new LVal(ltstr, LVal.Type.NORMAL);
				case 16:
					debug("<xhex>{xhex_inside}");
					ltstr += yytext();
					break;
				case 17:
					debug("<xhex>{xhex_stop}");
					yybegin(0);
					ltstr += yytext();
					return new LVal(ltstr, LVal.Type.NORMAL);
				case 18:
					ltstr += yytext();
					break;
				case 19:
					yybegin(18);
					break;
				case 20:
					ltstr += "]";
					ltstr += yytext();
					yybegin(16);
					break;
				case 21:
					yybegin(0);
					return new LVal(ltstr, LVal.Type.STRING);
				case 22:
					ltstr += "]";
					yybegin(18);
					break;
				case 23:
					debug("{xc_start}");
					yybegin(2);
					ltstr = yytext();
					break;
				case 24:
					debug("{xbin_start}");
					yybegin(10);
					ltstr = "";
					ltstr += yytext();
					break;
				case 25:
					debug("{xhex_start}");
					yybegin(12);
					ltstr = "";
					ltstr += yytext();
					break;
				case 26:
					debug("{decimal}");
					return new LVal(yytext(), LVal.Type.DECIMAL);
				case 27:
					debug("{real}");
					return new LVal(yytext(), LVal.Type.DOUBLE);
				case 28:
					debug("{assign}");
					return new LVal(yytext(), LVal.Type.NORMAL);
				case 29:
					debug("{selstar}");
					return new LVal(yytext(), LVal.Type.NORMAL);
				case 30:
					debug("{boundary}");
					return new LVal(yytext(), LVal.Type.NORMAL);
				case 31:
					debug("<xc>{xc_start}");
					ltstr += yytext();
					break;
				case 32:
					debug("<xc>{xc_stop}");
					yybegin(0);
					ltstr += yytext();
					return new LVal(ltstr, LVal.Type.WHITESPACE_OR_COMMENT);
				case 33:
					debug("<xq>{xq_double}");
					ltstr += "'";
					break;
				case 34:
					debug("<xdq>{xdq_double}");
					ltstr += yytext();
					break;
				case 35:
					yybegin(16);
					ltstr = "";
					break;
				case 36:
					debug("{integer_with_boundary}");
					return new LVal(yytext(), LVal.Type.NORMAL);
				case 37:
					debug("{hex_integer}");
					return new LVal(yytext(), LVal.Type.HEX_INT);
				case 38:
					debug("<xq>{xq_cat}");
					break;
				case 39:
					debug("<xbin>{xbin_cat}");
					break;
				case 40:
					debug("<xhex>{xhex_cat}");
					break;
				case 41:
					debug("{null}");
					return new LVal(null, LVal.Type.NULL);
				case 42:
					debug("{is_null}");
					return new LVal(yytext(), LVal.Type.NORMAL);
				case 43:
					debug("{not_null}");
					return new LVal(yytext(), LVal.Type.NORMAL);
				default:
					zzScanError(1);
					break;
				}
			}
		}

		public static bool isHighSurrogate(char ch)
		{
			if (ch >= MIN_HIGH_SURROGATE)
			{
				return ch <= MAX_HIGH_SURROGATE;
			}
			return false;
		}

		public static bool isLowSurrogate(char ch)
		{
			if (ch >= MIN_LOW_SURROGATE)
			{
				return ch <= MAX_LOW_SURROGATE;
			}
			return false;
		}

		public static int toCodePoint(char high, char low)
		{
			return (high - MIN_HIGH_SURROGATE << 10) + (low - MIN_LOW_SURROGATE) + MIN_SUPPLEMENTARY_CODE_POINT;
		}

		private static int codePointAtImpl(char[] a, int index, int limit)
		{
			char c = a[index++];
			if (isHighSurrogate(c) && index < limit)
			{
				char c2 = a[index];
				if (isLowSurrogate(c2))
				{
					return toCodePoint(c, c2);
				}
			}
			return c;
		}

		public static int codePointAt(char[] a, int index, int limit)
		{
			if (index >= limit || limit < 0 || limit > a.Length)
			{
				throw new Exception("index out of bounds");
			}
			return codePointAtImpl(a, index, limit);
		}

		public static int charCount(int codePoint)
		{
			if (codePoint < MIN_SUPPLEMENTARY_CODE_POINT)
			{
				return 1;
			}
			return 2;
		}
	}
}
