using System;
using System.Security.Cryptography;

namespace Dm
{
	internal class SymmCipherDesc
	{
		private const string DEFAULT_PADDING_TYPE = "PKCS5Padding";

		private const string NO_PADDING_TYPE = "NoPadding";

		private int algorithmType = -1;

		private CipherMode cipherMode;

		private PaddingMode paddingMode;

		private string algorithmName;

		private int keyLength = -1;

		private int ivLength = -1;

		public SymmCipherDesc(int algorithmID)
		{
			algorithmType = algorithmID & 0xFF80;
			int num = algorithmID & 0x7F;
			switch (algorithmType)
			{
			case 512:
				algorithmName = "AES";
				keyLength = 16;
				ivLength = 16;
				break;
			case 1024:
				algorithmName = "AES";
				keyLength = 24;
				ivLength = 16;
				break;
			case 2048:
				algorithmName = "AES";
				keyLength = 32;
				ivLength = 16;
				break;
			case 128:
				algorithmName = "DES";
				keyLength = 8;
				ivLength = 8;
				break;
			case 256:
				algorithmName = "DESEDE";
				keyLength = 16;
				ivLength = 8;
				break;
			case 4096:
				keyLength = 16;
				algorithmName = "RC4";
				break;
			default:
				throw new Exception("invalid cipher type!");
			}
			if (4096 != algorithmType)
			{
				switch (num)
				{
				case 1:
					cipherMode = CipherMode.ECB;
					paddingMode = PaddingMode.PKCS7;
					break;
				case 2:
					cipherMode = CipherMode.CBC;
					paddingMode = PaddingMode.PKCS7;
					break;
				case 4:
					cipherMode = CipherMode.CFB;
					paddingMode = PaddingMode.None;
					break;
				case 8:
					cipherMode = CipherMode.OFB;
					paddingMode = PaddingMode.None;
					break;
				default:
					throw new Exception("invalid cipher type!");
				}
			}
		}

		internal string getAlgorithmName()
		{
			return algorithmName;
		}

		internal int getIvLength()
		{
			return ivLength;
		}

		internal int getKeyLength()
		{
			return keyLength;
		}

		internal CipherMode getWorkMode()
		{
			return cipherMode;
		}

		internal PaddingMode getPaddingMode()
		{
			return paddingMode;
		}

		internal int getAlgorithmType()
		{
			return algorithmType;
		}
	}
}
