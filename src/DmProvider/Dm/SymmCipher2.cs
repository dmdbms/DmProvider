using System;
using System.Runtime.InteropServices;

namespace Dm
{
	internal class SymmCipher2 : Cipher
	{
		private int cipherType;

		private byte[] sessionKey;

		private const int HASH_TYPE = 4352;

		private const int HASH_SIZE = 16;

		[DllImport("dmcyt.dll")]
		internal static extern int cyt_sys_init(IntPtr env);

		[DllImport("dmcyt.dll")]
		internal static extern int cyt_get_cipher_text_size(int id, int plain_text_size);

		[DllImport("dmcyt.dll")]
		internal static extern int cyt_do_encrypt(IntPtr env, int id, byte[] key, int key_size, byte[] plain_text, int plain_text_size, byte[] cipher_text, int cipher_buf_size);

		[DllImport("dmcyt.dll")]
		internal static extern int cyt_hash_gen_digest(int id, byte[] msg, int msg_size, byte[] digest_buf, int digest_buf_size);

		[DllImport("dmcyt.dll")]
		internal static extern int cyt_do_decrypt(IntPtr env, int id, byte[] key, int key_size, byte[] cipher_text, int cipher_text_size, byte[] plain_text, int plain_text_buf_size);

		public SymmCipher2(int cipherType, byte[] key)
		{
			this.cipherType = cipherType;
			int num = cipherType & 0xFF80;
			int num2 = 0;
			num2 = num switch
			{
				512 => 16, 
				1024 => 24, 
				2048 => 32, 
				128 => 8, 
				256 => 16, 
				4096 => 16, 
				_ => throw new Exception("invalid cipher type!"), 
			};
			if (256 == num)
			{
				sessionKey = new byte[num2 + 8];
				Array.Copy(key, 0, sessionKey, 0, num2);
				Array.Copy(key, 0, sessionKey, num2, 8);
			}
			else
			{
				sessionKey = new byte[num2];
				Array.Copy(key, 0, sessionKey, 0, num2);
			}
			cyt_sys_init(Marshal.StringToBSTR(null));
		}

		public byte[] Encrypt(byte[] src, bool genDigest)
		{
			try
			{
				byte[] array = new byte[cyt_get_cipher_text_size(cipherType, src.Length)];
				int num = cyt_do_encrypt(Marshal.StringToBSTR(null), cipherType, sessionKey, sessionKey.Length, src, src.Length, array, array.Length);
				byte[] array2 = null;
				if (num != array.Length)
				{
					array2 = new byte[num];
					Array.Copy(array, 0, array2, 0, num);
				}
				else
				{
					array2 = array;
				}
				if (!genDigest)
				{
					return array2;
				}
				byte[] array3 = genDigestText(src);
				byte[] array4 = new byte[array2.Length + array3.Length];
				Array.Copy(array2, 0, array4, 0, array2.Length);
				Array.Copy(array3, 0, array4, array2.Length, array3.Length);
				return array4;
			}
			catch (Exception)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_ENCRYPT_FAIL);
				return null;
			}
		}

		private byte[] genDigestText(byte[] text)
		{
			try
			{
				byte[] array = new byte[16];
				int num = cyt_hash_gen_digest(4352, text, text.Length, array, array.Length);
				byte[] array2 = null;
				if (num != array.Length)
				{
					array2 = new byte[num];
					Array.Copy(array, 0, array2, 0, num);
				}
				else
				{
					array2 = array;
				}
				return array2;
			}
			catch (Exception)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_DIGEST_FAIL);
				return null;
			}
		}

		public byte[] Decrypt(byte[] byteSource, bool checkDigest)
		{
			try
			{
				byte[] array = null;
				int num = 0;
				byte[] array2 = null;
				if (checkDigest)
				{
					byte[] array3 = new byte[16];
					Array.Copy(byteSource, byteSource.Length - 16, array3, 0, 16);
					byte[] array4 = new byte[byteSource.Length - 16];
					Array.Copy(byteSource, 0, array4, 0, array4.Length);
					array = new byte[array4.Length];
					num = cyt_do_decrypt(Marshal.StringToBSTR(null), cipherType, sessionKey, sessionKey.Length, array4, array4.Length, array, array.Length);
					if (num != array.Length)
					{
						array2 = new byte[num];
						Array.Copy(array, 0, array2, 0, array2.Length);
					}
					else
					{
						array2 = array;
					}
					byte[] array5 = genDigestText(array2);
					if (array5.Length != array3.Length)
					{
						DmError.ThrowDmException(DmErrorDefinition.ECNET_CHECK_DIGEST_FAIL);
					}
					for (int i = 0; i < array5.Length; i++)
					{
						if (array5[i] != array3[i])
						{
							DmError.ThrowDmException(DmErrorDefinition.ECNET_CHECK_DIGEST_FAIL);
						}
					}
				}
				else
				{
					array = new byte[byteSource.Length];
					num = cyt_do_decrypt(Marshal.StringToBSTR(null), cipherType, sessionKey, sessionKey.Length, byteSource, byteSource.Length, array, array.Length);
					if (num != array.Length)
					{
						array2 = new byte[num];
						Array.Copy(array, 0, array2, 0, array2.Length);
					}
					else
					{
						array2 = array;
					}
				}
				return array2;
			}
			catch (Exception)
			{
				DmError.ThrowDmException(DmErrorDefinition.ECNET_DECRYPT_FAIL);
				return null;
			}
		}
	}
}
