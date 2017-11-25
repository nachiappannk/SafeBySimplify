using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SafeModel;

namespace SafeModelTests
{
    [TestFixture]
    public class SafeProviderTests
    {
        private ISettingGateway _settingGateway;
        private string _initialWorkingDirectory = "C:\\SomeDirectory";
        private ISafeProvider _safeProvider;
        IAccountGateway _accountGateway;

        [SetUp]
        public void SetUp()
        {
            _settingGateway = new SettingGatewayForTests();
            _settingGateway.SetWorkingDirectory(_initialWorkingDirectory);
            var safeProvider = new SafeProvider();
            _accountGateway = Substitute.For<IAccountGateway>();
            safeProvider.SettingGateway = _settingGateway;
            safeProvider.AccountGateway = _accountGateway;
            _safeProvider = safeProvider;
        }

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

        [TestCase("",false, SafeProvider.UserNameCanNotBeEmptyErrorMessage)]
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
            _accountGateway.IsUsernameCreatable(userName).Returns(false);
            var isValid = _safeProvider.IsUserNameValidForNonExistingUser(userName, out errorMessage);
            Assert.AreEqual(false, isValid);
            Assert.AreEqual(SafeProvider.UserNameProbablyExistsErrorMessage, errorMessage);
        }

        [TestCase("SomeUserName")]
        [TestCase("someUserName")]
        [TestCase("someUser124Name")]
        [TestCase("121someUser124Name")]
        public void ValidUserNameForCreationCase(string username)
        {
            var errorMessage = string.Empty;
            _accountGateway.IsUsernameCreatable(username).Returns(true);
            var isValid = _safeProvider.IsUserNameValidForNonExistingUser(username, out errorMessage);
            Assert.AreEqual(true, isValid);
            Assert.AreEqual(string.Empty, errorMessage);
        }


    }
}
