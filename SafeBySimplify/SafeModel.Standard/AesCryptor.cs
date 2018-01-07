using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SafeModel.Standard
{
    public static class AesCryptor
    {
        private static readonly int KeySizeInBytes = 256 / 8;
        private static readonly int IvSizeInBytes = 128 / 8;

        static byte[] Crypt(Func<Aes, ICryptoTransform> operationSelector,
            byte[] content, byte[] key, byte[] initializationVector)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Aes aes = Aes.Create())
                {
                    Debug.Assert(aes != null, "aes != null");
                    aes.Mode = CipherMode.CBC;
                    aes.Key = key;
                    aes.IV = initializationVector;
                    using (var cryptoStream = new CryptoStream(memoryStream, operationSelector.Invoke(aes),
                        CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(content, 0, content.Length);
                        cryptoStream.Close();
                    }
                    return memoryStream.ToArray();
                }
            }
        }

        static byte[] Encrypt(byte[] content, byte[] key, byte[] initializationVector)
        {
            return Crypt(aes => aes.CreateEncryptor(), content, key, initializationVector);
        }

        static byte[] Decrypt(byte[] content, byte[] key, byte[] initializationVector)
        {
            return Crypt(aes => aes.CreateDecryptor(), content, key, initializationVector);
        }

        static byte[] GetKeyBytes(byte[] mainPassBytes, byte[] saltPassBytes, int iteration)
        {
            var rfc2898DeriveBytes = GetRfc2898DeriveBytes(mainPassBytes, saltPassBytes, iteration);
            return rfc2898DeriveBytes.GetBytes(KeySizeInBytes);
        }

        static byte[] GetIvBytes(byte[] mainPassBytes, byte[] saltPassBytes, int iteration)
        {
            var rfc2898DeriveBytes = GetRfc2898DeriveBytes(mainPassBytes, saltPassBytes, iteration);
            return rfc2898DeriveBytes.GetBytes(IvSizeInBytes);
        }

        private static Rfc2898DeriveBytes GetRfc2898DeriveBytes(byte[] mainPassBytes, byte[] saltPassBytes, int iteration)
        {
            var saltPassBytes1 = saltPassBytes.ToList();

            while (saltPassBytes1.Count < 8)
            {
                saltPassBytes1.Add(0);
            }
            return new Rfc2898DeriveBytes(mainPassBytes, saltPassBytes1.ToArray(), iteration);
        }

        public static byte[] Encrypt(byte[] content, byte[] mainPassBytes, byte[] saltPassBytes, int iteration)
        {
            return Encrypt(content, GetKeyBytes(mainPassBytes, saltPassBytes, iteration),
                GetIvBytes(mainPassBytes, saltPassBytes, iteration));
        }

        public static byte[] Decrypt(byte[] content, byte[] mainPassBytes, byte[] saltPassBytes, int iteration)
        {
            return Decrypt(content, GetKeyBytes(mainPassBytes, saltPassBytes, iteration),
                GetIvBytes(mainPassBytes, saltPassBytes, iteration));
        }

    }
}
