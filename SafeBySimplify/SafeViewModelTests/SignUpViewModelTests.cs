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

        protected SignUpViewModel SignUpViewModel;
        protected CommandObserver CommandObserver;
        protected ViewModelPropertyObserver<string> ErrorMessagePropertyObserver;

        protected ISafeProviderForNonExistingUser SafeProviderForNonExistingUser;

        [SetUp]
        public void SetUp()
        {
            SafeProviderForNonExistingUser = CreateSafeProviderForNonExistingUser();
            SignUpViewModel = new SignUpViewModel(SafeProviderForNonExistingUser, (safe, n) => { });
            CommandObserver = SignUpViewModel.SignUpCommand.GetDelegateCommandObserver();
            ErrorMessagePropertyObserver =
                SignUpViewModel.GetPropertyObserver<string>(nameof(SignUpViewModel.SignUpErrorMessage));
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