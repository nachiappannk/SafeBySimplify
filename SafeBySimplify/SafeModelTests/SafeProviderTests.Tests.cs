using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SafeModel;

namespace SafeModelTests
{
    public partial class SafeProviderTests
    {
        public class Tests : SafeProviderTests
        {
            [Test]
            public void The_current_working_directory_is_that_provided_by_settings_gateway()
            {
                Assert.AreEqual(_initialWorkingDirectory, _safeProvider.WorkingDirectory);
            }

            [Test]
            public void When_current_working_directory_is_updated_then_settings_gateway_is_updated()
            {
                var newDirectory = "C:\\NewDirectory";
                _safeProvider.WorkingDirectory = newDirectory;
                Assert.AreEqual(newDirectory, _settingGateway.GetWorkingDirectory());
            }

            [TestCase("", false, SafeProvider.UserNameCanNotBeEmptyErrorMessage)]
            [TestCase("abcd", false, SafeProvider.UserNameHasToBeMinimum8CharactersErrorMessage)]
            [TestCase("abcd1234@", false, SafeProvider.UserNameHasToAlphaNumericWithNoSpecialCharactersErrorMessage)]
            public void Username_invalidity_for_new_user(string userName, bool validity, string expectedErrorMessage)
            {
                var errorMessage = string.Empty;
                var isValid = _safeProvider.IsUserNameValidForNonExistingUser(userName, out errorMessage);
                Assert.AreEqual(validity, isValid);
                Assert.AreEqual(expectedErrorMessage, errorMessage);
            }

            [Test]
            public void When_user_gateway_says_a_username_can_not_be_created_then_the_user_name_can_not_be_created()
            {
                var errorMessage = string.Empty;
                var userName = "someUserName";
                _accountGateway.IsUsernameCreatable(_initialWorkingDirectory, userName).Returns(false);
                var isValid = _safeProvider.IsUserNameValidForNonExistingUser(userName, out errorMessage);
                Assert.AreEqual(false, isValid);
                Assert.AreEqual(SafeProvider.UserNameProbablyExistsErrorMessage, errorMessage);
            }

            [TestCase("SomeUserName")]
            [TestCase("someUserName")]
            [TestCase("someUser124Name")]
            [TestCase("121someUser124Name")]
            public void Username_validity_for_new_user(string username)
            {
                var errorMessage = string.Empty;
                _accountGateway.IsUsernameCreatable(_initialWorkingDirectory, username).Returns(true);
                var isValid = _safeProvider.IsUserNameValidForNonExistingUser(username, out errorMessage);
                Assert.AreEqual(true, isValid);
                Assert.AreEqual(string.Empty, errorMessage);
            }

            [TestCase("as21#@a", false, SafeProvider.PasswordTooShortErrorMessage)]
            [TestCase("as21#@as1", true, "")]
            public void PasswordValidityTests(string password, bool expctedValidity, string expectedErrorMessage)
            {
                var errorMessage = string.Empty;
                var isValid = _safeProvider.IsPasswordValidForNonExistingUser(password, out errorMessage);
                Assert.AreEqual(expctedValidity, isValid);
                Assert.AreEqual(expectedErrorMessage, errorMessage);
            }

            [TestCase("onasssdfaa", "aaaassdfa")]
            [TestCase()]
            public void ExistingUserNamesTest(params string[] userNamesAtGateway)
            {
                _accountGateway.GetUserNames(_initialWorkingDirectory).Returns(userNamesAtGateway.ToList());
                var userNames = _safeProvider.GetUserNames();
                CollectionAssert.AreEquivalent(userNamesAtGateway, userNames);
            }

            [Test]
            public void When_safe_creation_is_attempted_with_invalid_password_then_safe_creation_fails()
            {
                var userName = "SomeUserName";
                var someInvalidPassword = "somePassword";
                var verifyingWord = "SomeWord";
                var verifyingWordEncryptedBytesForInvalidPassword = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
                var verifyingWordEncryptedBytesForValidPassword = new byte[] { 0, 1, 2, 3, 4, 5, 6, };
                var masterPassEncryptedBytes = new byte[] { 4, 5, 6, 7, 8, 9 };

                var account = new Account()
                {
                    MasterEncryptedPassBytes = masterPassEncryptedBytes,
                    VerifyingWord = verifyingWord,
                    VeryifyingWordEncryptedBytes = verifyingWordEncryptedBytesForValidPassword,
                };

                _accountGateway.ReadUserAccount(userName).Returns(account);
                _cryptor.GetEncryptedBytes(verifyingWord, someInvalidPassword).Returns(verifyingWordEncryptedBytesForInvalidPassword);

                ISafe safe = null;
                var result = _safeProvider.TryCreateSafeForExistingUser(userName, someInvalidPassword, out safe);

                Assert.False(result);
                Assert.IsNull(safe);
            }


            [Test]
            public void When_safe_creation_is_attempted_with_valid_password_then_safe_creation_is_successful()
            {
                var userName = "SomeUserName";
                var password = "somePassword";
                var verifyingWord = "SomeWord";
                var verifyingWordEncryptedBytesForValidPassword = new byte[] { 0, 1, 2, 3, 4, 5, 6, };
                var masterPassEncryptedBytes = new byte[] { 4, 5, 6, 7, 8, 9 };

                var account = new Account()
                {
                    MasterEncryptedPassBytes = masterPassEncryptedBytes,
                    VerifyingWord = verifyingWord,
                    VeryifyingWordEncryptedBytes = verifyingWordEncryptedBytesForValidPassword,
                };

                _accountGateway.ReadUserAccount(userName).Returns(account);
                _cryptor.GetEncryptedBytes(verifyingWord, password).Returns(verifyingWordEncryptedBytesForValidPassword);

                ISafe safe = null;
                var result = _safeProvider.TryCreateSafeForExistingUser(userName, password, out safe);
                Assert.True(result);
                Assert.AreEqual(userName, safe.UserName);
                Assert.AreEqual(_initialWorkingDirectory, safe.WorkingDirectory);
            }
        }
    }
}