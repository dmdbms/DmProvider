using System;
using System.Numerics;

namespace Dm
{
	internal class DHKey
	{
		private BigInteger x;

		private BigInteger y;

		private DHGroup group;

		internal BigInteger X
		{
			get
			{
				return x;
			}
			set
			{
				x = value;
			}
		}

		internal BigInteger Y
		{
			get
			{
				return y;
			}
			set
			{
				y = value;
			}
		}

		internal DHGroup Group
		{
			get
			{
				return group;
			}
			set
			{
				group = value;
			}
		}

		internal DHKey()
		{
		}

		internal DHKey(byte[] s)
		{
			Array.Reverse(s);
			y = new BigInteger(s);
		}
	}
}
