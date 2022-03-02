namespace Dm
{
	internal interface Cipher
	{
		byte[] Encrypt(byte[] plaintext, bool genDigest);

		byte[] Decrypt(byte[] ciphertext, bool checkDigest);
	}
}
