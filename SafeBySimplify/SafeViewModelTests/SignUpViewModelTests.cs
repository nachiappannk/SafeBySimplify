using System;
using NSubstitute;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    [TestFixture]
    public partial class SignUpViewModelTests
    {
        protected const string ValidUserName = "SomeUserName";
        protected const string ValidPassword = "Password";


        protected SignUpViewModel _signUpViewModel;
        protected CommandObserver _commandObserver;
        protected ViewModelPropertyObserver<string> _errorMessagePropertyObserver;

        protected ISafeProviderForNonExistingUser _safeProviderForNonExistingUser;

        [SetUp]
        public void SetUp()
        {
            _safeProviderForNonExistingUser = CreateSafeProviderForNonExistingUser();
            _signUpViewModel = new SignUpViewModel(_safeProviderForNonExistingUser, (safe, n) => { });
            _commandObserver = _signUpViewModel.SignUpCommand.GetDelegateCommandObserver();
            _errorMessagePropertyObserver =
                _signUpViewModel.GetPropertyObserver<string>(nameof(_signUpViewModel.SignUpErrorMessage));
        }

        private static ISafeProviderForNonExistingUser CreateSafeProviderForNonExistingUser()
        {
            var safeProviderForNonExistingUser = Substitute.For<ISafeProviderForNonExistingUser>();

            string value = "";
            safeProviderForNonExistingUser.IsUserNameValidForNonExistingUser(ValidUserName, out value)
                .Returns(x =>
                {
                    x[1] = String.Empty;
                    return true;
                });

            safeProviderForNonExistingUser.IsPasswordValidForNonExistingUser(ValidPassword, out value)
                .Returns(x =>
                {
                    x[1] = String.Empty;
                    return true;
                });
            return safeProviderForNonExistingUser;
        }
    }
}