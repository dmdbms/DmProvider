using System;
using System.Security.Cryptography;

namespace Dm
{
	internal class SymmCipher : Cipher
	{
		private ICryptoTransform encryptCipher;

		private ICryptoTransform decryptCipher;

		private int hashType = 4352;

		private int hashSize = 16;

		public SymmCipher(int cipherType, byte[] key)
		{
			try
			{
				SymmCipherDesc symmCipherDesc = new SymmCipherDesc(cipherType);
				int keyLength = symmCipherDesc.getKeyLength();
				byte[] array;
				if (256 == symmCipherDesc.getAlgorithmType())
				{
					array = new byte[keyLength + 8];
					Array.Copy(key, 0, array, 0, keyLength);
					Array.Copy(key, 0, array, keyLength, 8);
				}
				else
				{
					array = new byte[keyLength];
					Array.Copy(key, 0, array, 0, keyLength);
				}
				encryptCipher = MsgSecurity.newCipher(encrypt: true, symmCipherDesc, array);
				decryptCipher = MsgSecurity.newCipher(encrypt: false, symmCipherDesc, array);
			}
			catch (Exception)
			{
				throw new Exception("cipher failed!");
			}
		}

		public byte[] Encrypt(byte[] src, bool genDigest)
		{
			byte[] array = null;
			try
			{
				array = encryptCipher.TransformFinalBlock(src, 0, src.Length);
			}
			catch (Exception)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_ENCRYPT_FAIL);
			}
			if (genDigest)
			{
				byte[] array2 = genMessageDigest(hashType, src);
				byte[] array3 = array;
				array = new byte[array.Length + array2.Length];
				Array.Copy(array3, 0, array, 0, array3.Length);
				Array.Copy(array2, 0, array, array3.Length, array2.Length);
			}
			return array;
		}

		private byte[] genMessageDigest(int algorithm, byte[] msg_text)
		{
			byte[] array = null;
			if (algorithm == 4352)
			{
				try
				{
					array = MD5.Create().ComputeHash(msg_text);
				}
				catch (Exception)
				{
					DmError.ThrowDmException(DmErrorDefinition.ECNET_DIGEST_FAIL);
				}
			}
			else
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_DIGEST_FAIL);
			}
			if (array == null || array.Length == 0 || (array.Length == 1 && array[0] == 0))
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_DIGEST_FAIL);
			}
			return array;
		}

		public byte[] Decrypt(byte[] byteSource, bool checkDigest)
		{
			byte[] array = null;
			try
			{
				if (checkDigest)
				{
					byte[] array2 = new byte[hashSize];
					Array.Copy(byteSource, byteSource.Length - hashSize, array2, 0, hashSize);
					array = new byte[byteSource.Length - hashSize];
					Array.Copy(byteSource, 0, array, 0, array.Length);
					array = decryptCipher.TransformFinalBlock(array, 0, array.Length);
					byte[] array3 = genMessageDigest(hashType, array);
					if (array3.Length != array2.Length)
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_CHECK_DIGEST_FAIL);
					}
					for (int i = 0; i < array3.Length; i++)
					{
						if (array3[i] != array2[i])
						{
							DmError.ThrowDmException(DmErrorDefinition.ECNET_CHECK_DIGEST_FAIL);
						}
					}
					return array;
				}
				array = decryptCipher.TransformFinalBlock(byteSource, 0, byteSource.Length);
				return array;
			}
			catch (Exception)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_DECRYPT_FAIL);
				return array;
			}
		}
	}
}
