using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Dm
{
	internal class ThirdPartCipher : Cipher
	{
		internal int encryptType;

		internal int hashType;

		internal byte[] key;

		internal int hashSize;

		internal static ThirdPartCipherDLL thirdPartCipherDLL;

		internal ThirdPartCipher(int encryptType, byte[] key, string cipherPath, int hashType)
		{
			if (thirdPartCipherDLL == null)
			{
				thirdPartCipherDLL = new ThirdPartCipherDLL(cipherPath);
			}
			this.encryptType = encryptType;
			this.key = key;
			this.hashType = hashType;
			hashSize = ((this.hashType == 4352) ? 16 : getHashSize());
		}

		internal int getHashSize()
		{
			int num = thirdPartCipherDLL.getCount();
			int cipher_id = 0;
			int blk_size = 0;
			int kh_size = 0;
			byte type = 0;
			IntPtr cipher_name = Marshal.StringToBSTR("");
			for (int i = 1; i <= num; i++)
			{
				thirdPartCipherDLL.getInfo(i, ref cipher_id, ref cipher_name, ref type, ref blk_size, ref kh_size);
				if (cipher_id == encryptType)
				{
					return kh_size;
				}
			}
			return 16;
		}

		public byte[] Encrypt(byte[] plaintext, bool genDigest)
		{
			try
			{
				thirdPartCipherDLL.encryptInit(encryptType, key, key.Length, out var encrypt_para);
				int num = thirdPartCipherDLL.getCipherTextSize(encryptType, encrypt_para, plaintext.Length);
				byte[] array = new byte[num];
				int num2 = thirdPartCipherDLL.encrypt(encryptType, encrypt_para, plaintext, plaintext.Length, array, array.Length);
				byte[] array2 = null;
				if (num2 != num)
				{
					array2 = new byte[num2];
					Array.Copy(array, 0, array2, 0, array2.Length);
				}
				else
				{
					array2 = array;
				}
				thirdPartCipherDLL.cleanup(encryptType, encrypt_para);
				if (genDigest)
				{
					byte[] array3 = genMessageDigest(hashType, plaintext);
					array = new byte[array2.Length + array3.Length];
					Array.Copy(array2, 0, array, 0, array2.Length);
					Array.Copy(array3, 0, array, array2.Length, array3.Length);
				}
				return array;
			}
			catch (Exception)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_ENCRYPT_FAIL);
				return null;
			}
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
				thirdPartCipherDLL.hashInit(encryptType, out var hash_para);
				thirdPartCipherDLL.hashUpdate(encryptType, hash_para, msg_text, msg_text.Length);
				thirdPartCipherDLL.hashFinal(encryptType, hash_para, array, hashSize);
			}
			if (array == null || array.Length == 0 || (array.Length == 1 && array[0] == 0))
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_DIGEST_FAIL);
			}
			return array;
		}

		public byte[] Decrypt(byte[] ciphertext, bool checkDigest)
		{
			try
			{
				byte[] array = null;
				if (checkDigest)
				{
					byte[] array2 = new byte[hashSize];
					Array.Copy(ciphertext, ciphertext.Length - hashSize, array2, 0, hashSize);
					array = new byte[ciphertext.Length - hashSize];
					Array.Copy(ciphertext, 0, array, 0, array.Length);
					array = Decrypt(array);
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
				}
				else
				{
					array = Decrypt(ciphertext);
				}
				return array;
			}
			catch (Exception)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_DECRYPT_FAIL);
				return null;
			}
		}

		internal byte[] Decrypt(byte[] ciphertext)
		{
			thirdPartCipherDLL.decryptInit(encryptType, key, key.Length, out var decrypt_para);
			byte[] array = new byte[ciphertext.Length];
			int num = thirdPartCipherDLL.decrypt(encryptType, decrypt_para, ciphertext, ciphertext.Length, array, array.Length);
			byte[] array2 = new byte[num];
			Array.Copy(array, 0, array2, 0, num);
			thirdPartCipherDLL.cleanup(encryptType, decrypt_para);
			return array2;
		}
	}
}
