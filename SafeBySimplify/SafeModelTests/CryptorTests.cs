using NUnit.Framework;
using SafeModel.Standard;

namespace SafeModelTests
{
    [TestFixture]
    public class CryptorTests
    {
        [Test]
        public void Encrypting_and_decrypting_gives_the_same_output()
        {
            var input = "sksjadflksj;flakjs;lka;lkjsf;lakjs;f";
            var password = "password";

            Cryptor cryptor = new Cryptor();
            var encryptedBytes = cryptor.GetEncryptedBytes(input, password);
            var result = cryptor.GetDecryptedContent<string>(encryptedBytes, password);
            Assert.AreEqual(input, result);

        }
    }
}