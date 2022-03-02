using System;
using System.Numerics;
using System.Security.Cryptography;

namespace Dm
{
	internal sealed class MsgSecurity
	{
		internal const int DH_KEY_LENGTH = 64;

		internal const int WORK_MODE_MASK = 127;

		internal const int ECB_MODE = 1;

		internal const int CBC_MODE = 2;

		internal const int CFB_MODE = 4;

		internal const int OFB_MODE = 8;

		internal const int ALGO_MASK = 65408;

		internal const int DES = 128;

		internal const int DES3 = 256;

		internal const int AES128 = 512;

		internal const int AES192 = 1024;

		internal const int AES256 = 2048;

		internal const int RC4 = 4096;

		public const int MD5 = 4352;

		internal const int DES_CFB = 132;

		internal const int MD5_DIGEST_SIZE = 16;

		internal const int MIN_EXTERNAL_CIPHER_ID = 5000;

		private static byte[] DEFAULT_IV = new byte[32]
		{
			32, 33, 35, 36, 37, 38, 39, 40, 41, 42,
			43, 44, 45, 46, 47, 48, 49, 50, 51, 52,
			53, 54, 55, 56, 57, 58, 59, 60, 61, 62,
			63, 32
		};

		private static BigInteger p;

		private static BigInteger g;

		internal static DHKey getClientKeyPair()
		{
			byte[] array = new byte[65]
			{
				0, 192, 9, 216, 119, 186, 245, 250, 244, 22,
				183, 247, 120, 230, 17, 93, 203, 144, 214, 82,
				23, 220, 194, 240, 138, 157, 252, 181, 161, 146,
				197, 147, 235, 171, 2, 146, 146, 102, 184, 219,
				252, 32, 33, 3, 159, 219, 212, 183, 253, 226,
				185, 150, 224, 0, 8, 245, 122, 230, 239, 180,
				237, 63, 23, 182, 211
			};
			byte[] array2 = new byte[2] { 0, 5 };
			Array.Reverse(array);
			Array.Reverse(array2);
			p = new BigInteger(array);
			g = new BigInteger(array2);
			return new DHGroup(p, g).GenerateKeyPair();
		}

		internal static byte[] ComputeSessionKey(DHKey clientPrivKey, byte[] serverPubKey)
		{
			BigInteger value = bytes2Bn(serverPubKey);
			BigInteger x = clientPrivKey.X;
			return Bn2Bytes(BigInteger.ModPow(value, x, p), 0);
		}

		internal static BigInteger bytes2Bn(byte[] src)
		{
			if (src == null)
			{
				return default(BigInteger);
			}
			if (src[0] == 0)
			{
				return new BigInteger(src);
			}
			byte[] array = new byte[src.Length + 1];
			array[0] = 0;
			Array.Copy(src, 0, array, 1, src.Length);
			Array.Reverse(array);
			return new BigInteger(array);
		}

		internal static byte[] Bn2Bytes(BigInteger bn, int bnLen)
		{
			byte[] array = null;
			byte[] array2 = null;
			byte[] array3 = null;
			int num = 0;
			int num2 = 0;
			array = bn.ToByteArray();
			Array.Reverse(array);
			if (array[0] == 0)
			{
				num2 = array.Length - 1;
				array2 = new byte[num2];
				Array.Copy(array, 1, array2, 0, num2);
			}
			else
			{
				array2 = array;
				num2 = array2.Length;
			}
			num = ((bnLen != 0) ? (bnLen - num2) : 0);
			if (num > 0)
			{
				array3 = new byte[64];
				int num3 = 0;
				for (num3 = 0; num3 < num; num3++)
				{
					array3[num3] = 0;
				}
				Array.Copy(array2, 0, array3, num3, array2.Length);
			}
			else
			{
				array3 = array2;
			}
			return array3;
		}

		internal static ICryptoTransform newCipher(bool encrypt, SymmCipherDesc cipherDesc, byte[] sessionKey)
		{
			ICryptoTransform cryptoTransform = null;
			SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create(cipherDesc.getAlgorithmName());
			symmetricAlgorithm.Mode = cipherDesc.getWorkMode();
			symmetricAlgorithm.Padding = cipherDesc.getPaddingMode();
			if (CipherMode.ECB == cipherDesc.getWorkMode() || 4096 == cipherDesc.getAlgorithmType())
			{
				symmetricAlgorithm.GenerateIV();
				if (encrypt)
				{
					return symmetricAlgorithm.CreateEncryptor(sessionKey, symmetricAlgorithm.IV);
				}
				return symmetricAlgorithm.CreateDecryptor(sessionKey, symmetricAlgorithm.IV);
			}
			if (encrypt)
			{
				return symmetricAlgorithm.CreateEncryptor(sessionKey, getIV(cipherDesc.getIvLength()));
			}
			return symmetricAlgorithm.CreateDecryptor(sessionKey, getIV(cipherDesc.getIvLength()));
		}

		private static byte[] getIV(int ivLength)
		{
			if (-1 == ivLength)
			{
				return new byte[0];
			}
			byte[] array = new byte[ivLength];
			Array.Copy(DEFAULT_IV, 0, array, 0, ivLength);
			return array;
		}
	}
}
