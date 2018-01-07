using System;
using System.Linq;
using System.Text;

namespace SafeModel.Standard
{
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
}
