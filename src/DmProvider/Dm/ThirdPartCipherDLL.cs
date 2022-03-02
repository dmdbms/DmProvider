using System;
using System.Runtime.InteropServices;

namespace Dm
{
	internal class ThirdPartCipherDLL : IDisposable
	{
		internal delegate int cipher_get_count();

		internal delegate long cipher_get_info(int seqno, ref int cipher_id, ref IntPtr cipher_name, ref byte type, ref int blk_size, ref int kh_size);

		internal delegate int cipher_encrypt_init(int inner_id, byte[] key, int key_size, out IntPtr encrypt_para);

		internal delegate int cipher_get_cipher_text_size(int inner_id, IntPtr cipher_para, int plain_text_size);

		internal delegate int cipher_encrypt(int inner_id, IntPtr encrypt_para, byte[] plain_text, int plain_text_size, byte[] cipher_text, int cipher_text_buf_size);

		internal delegate void cipher_cleanup(int inner_id, IntPtr cipher_para);

		internal delegate int cipher_decrypt_init(int inner_id, byte[] key, int key_size, out IntPtr decrypt_para);

		internal delegate int cipher_decrypt(int inner_id, IntPtr cipher_para, byte[] cipher_text, int cipher_text_size, byte[] plain_text, int plain_text_buf_size);

		internal delegate int cipher_hash_init(int inner_id, out IntPtr hash_para);

		internal delegate void cipher_hash_update(int inner_id, IntPtr hash_para, byte[] msg, int msg_size);

		internal delegate int cipher_hash_final(int inner_id, IntPtr hash_para, byte[] digest, int digest_buf_size);

		internal cipher_get_count getCount;

		internal cipher_get_info getInfo;

		internal cipher_encrypt_init encryptInit;

		internal cipher_get_cipher_text_size getCipherTextSize;

		internal cipher_encrypt encrypt;

		internal cipher_cleanup cleanup;

		internal cipher_decrypt_init decryptInit;

		internal cipher_decrypt decrypt;

		internal cipher_hash_init hashInit;

		internal cipher_hash_update hashUpdate;

		internal cipher_hash_final hashFinal;

		[DllImport("Kernel32.dll")]
		internal static extern IntPtr LoadLibrary(string cipherPath);

		[DllImport("Kernel32.dll")]
		internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("Kernel32.dll")]
		internal static extern IntPtr FreeLibrary(IntPtr hModule);

		internal Delegate GetAddress(IntPtr hModule, string procName, Type t)
		{
			IntPtr procAddress = GetProcAddress(hModule, procName);
			if (procAddress == IntPtr.Zero)
			{
				return null;
			}
			return Marshal.GetDelegateForFunctionPointer(procAddress, t);
		}

		internal ThirdPartCipherDLL(string cipherPath)
		{
			IntPtr intPtr = LoadLibrary(cipherPath);
			if (intPtr == IntPtr.Zero)
			{
				throw new SystemException("load thirdPart dll failed!");
			}
			getCount = (cipher_get_count)GetAddress(intPtr, "cipher_get_count", typeof(cipher_get_count));
			getInfo = (cipher_get_info)GetAddress(intPtr, "cipher_get_info", typeof(cipher_get_info));
			encryptInit = (cipher_encrypt_init)GetAddress(intPtr, "cipher_encrypt_init", typeof(cipher_encrypt_init));
			getCipherTextSize = (cipher_get_cipher_text_size)GetAddress(intPtr, "cipher_get_cipher_text_size", typeof(cipher_get_cipher_text_size));
			encrypt = (cipher_encrypt)GetAddress(intPtr, "cipher_encrypt", typeof(cipher_encrypt));
			cleanup = (cipher_cleanup)GetAddress(intPtr, "cipher_cleanup", typeof(cipher_cleanup));
			decryptInit = (cipher_decrypt_init)GetAddress(intPtr, "cipher_decrypt_init", typeof(cipher_decrypt_init));
			decrypt = (cipher_decrypt)GetAddress(intPtr, "cipher_decrypt", typeof(cipher_decrypt));
			hashInit = (cipher_hash_init)GetAddress(intPtr, "cipher_hash_init", typeof(cipher_hash_init));
			hashUpdate = (cipher_hash_update)GetAddress(intPtr, "cipher_hash_update", typeof(cipher_hash_update));
			hashFinal = (cipher_hash_final)GetAddress(intPtr, "cipher_hash_final", typeof(cipher_hash_final));
		}

		public void Dispose()
		{
		}
	}
}
