using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace SafeModel
{
    public class Cryptor : ICryptor
    {

        public byte[] GetEncryptedBytes<T>(T content, string password)
        {
            var flatContent = JsonConvert.SerializeObject(content);
            AesCryptorWrapper cryptorWrapper = new AesCryptorWrapper(password);
            return cryptorWrapper.Encrypt(Encoding.UTF8.GetBytes(flatContent));
        }

        public T GetDecryptedContent<T>(byte[] encryptedBytes, string password)
        {
            AesCryptorWrapper cryptorWrapper = new AesCryptorWrapper(password);
            var decryptedBytes = cryptorWrapper.Decrypt(encryptedBytes);
            var searializedString = Encoding.UTF8.GetString(decryptedBytes);
            return JsonConvert.DeserializeObject<T>(searializedString);
        }
    }

    public class AesCryptorWrapper
    {
        private string _key;
        public string RandomizingStringForSalt { get; set; }
        public string RandomizingStringForMain { get; set; }
        public int Iteration { get; set; }

        public AesCryptorWrapper(string key)
        {
            _key = key;
            RandomizingStringForSalt = "NashSecurity";
            RandomizingStringForMain = "SuchSeeth";
            Iteration = 111;
        }

        public byte[] Encrypt(byte[] inputBytes)
        {
            return Crypt(AesCryptor.Encrypt, inputBytes);
        }

        public byte[] Decrypt(byte[] encryptedPassword)
        {
            return Crypt(AesCryptor.Decrypt, encryptedPassword);
        }

        private byte[] Crypt(Func<byte[], byte[], byte[], int, byte[]> cryptingFunction, byte[] bytes)
        {
            var saltBytes = GetBytes(RandomizingStringForSalt, _key);
            var mainBytes = GetBytes(RandomizingStringForMain, _key);
            return cryptingFunction(bytes, mainBytes, saltBytes, Iteration);
        }

        private static byte[] GetBytes(string s1, string s2)
        {
            byte[][] byte2DArray = { Encoding.ASCII.GetBytes(s1), Encoding.ASCII.GetBytes(s2) };
            var resultByteLength = byte2DArray.Sum(a => a.Length);
            var resultBytes = new byte[resultByteLength];

            int sizeOfBiggestInputArray = byte2DArray.Max(a => a.Length);
            var numberOfInputArrays = byte2DArray.Length;
            int currentIndex = 0;
            for (int i = 0; i < sizeOfBiggestInputArray; i++)
            {
                for (var j = 0; j < numberOfInputArrays; j++)
                {
                    if (i < byte2DArray[j].Length)
                        resultBytes[currentIndex++] = byte2DArray[j][i];
                }
            }
            return resultBytes;
        }
    }

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