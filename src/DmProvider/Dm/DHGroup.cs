using System;
using System.Numerics;
using System.Security.Cryptography;

namespace Dm
{
	internal class DHGroup
	{
		private BigInteger p;

		private BigInteger g;

		internal BigInteger P => p;

		internal BigInteger G => g;

		internal DHGroup(BigInteger p, BigInteger g)
		{
			this.p = p;
			this.g = g;
		}

		internal DHKey GenerateKeyPair()
		{
			RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
			DHKey dHKey = new DHKey();
			byte[] array = new byte[P.ToByteArray().Length - 1];
			rNGCryptoServiceProvider.GetNonZeroBytes(array);
			byte[] array2 = new byte[P.ToByteArray().Length];
			array2[^1] = 0;
			Array.Copy(array, 0, array2, 0, array.Length);
			dHKey.X = new BigInteger(array2);
			dHKey.Y = BigInteger.ModPow(G, dHKey.X, P);
			return dHKey;
		}

		internal DHKey ComputeKey(DHKey pubKey, DHKey privKey)
		{
			_ = P;
			_ = pubKey.Y;
			if (pubKey.Y.Sign <= 0 || pubKey.Y >= P)
			{
				throw new SystemException("DH parameter out of bounds");
			}
			_ = privKey.X;
			BigInteger y = BigInteger.ModPow(pubKey.Y, privKey.X, P);
			return new DHKey
			{
				Y = y,
				Group = this
			};
		}
	}
}
