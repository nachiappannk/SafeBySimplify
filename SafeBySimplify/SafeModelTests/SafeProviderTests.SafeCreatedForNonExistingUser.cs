using NSubstitute;
using NUnit.Framework;
using SafeModel.Standard;

namespace SafeModelTests
{
    public partial class SafeProviderTests
    {
        public class SafeCreatedForNonExistingUser : SafeProviderTests
        {
            private readonly string _userName = "userName";
            private readonly string _masterpassword = "password";
            private readonly string _password = "password";
            private readonly byte[] _masterPassBytes = new byte[] { 23, 3, 4, 23, 41, 2, 34, 1, 2, 34, };
            private readonly string _verifyingWord = "SafeBySimplify";
            private readonly byte[] _verifyingWordEncryptedBytes = new byte[] { 23, 43, 2, 3, 4, 1, 4 };
            private ISafe _safe;

            [SetUp]
            public void SafeCreatedForNonExistingUserSetUp()
            {
                _cryptor.GetEncryptedBytes(_masterpassword, _password).Returns(_masterPassBytes);
                _cryptor.GetEncryptedBytes(_verifyingWord, _password).Returns(_verifyingWordEncryptedBytes);
                _safe = _safeProvider.CreateSafeForNonExistingUser(_userName, _masterpassword, _password);
            }

            [Test]
            public void When_new_user_is_created_then_user_records_are_written_to_the_gateway()
            {
                _accountGateway.Received(1)
                    .WriteUserRecord(_initialWorkingDirectory, _userName, Arg.Is<Account>(x => 
                    x.VeryifyingWordEncryptedBytes == _verifyingWordEncryptedBytes &&
                    x.VerifyingWord == _verifyingWord &&
                    x.MasterEncryptedPassBytes == _masterPassBytes));
            }

            [Test]
            public void When_safe_created_for_non_existing_user_then_working_directory_and_user_name_is_correctly_set()
            {
                Assert.AreEqual(_initialWorkingDirectory, _safe.WorkingDirectory);
                Assert.AreEqual(_userName, _safe.UserName);
            }
        }
    }
}