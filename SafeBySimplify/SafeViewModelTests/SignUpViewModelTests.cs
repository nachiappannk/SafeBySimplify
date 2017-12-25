/*
MIT License

Copyright(c) 2017 Nachiappan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

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
            SignUpViewModel = new SignUpViewModel(SafeProviderForNonExistingUser, (safe, userName) => { });
            CommandObserver = SignUpViewModel.SignUpCommand.GetDelegateCommandObserver();
            ErrorMessagePropertyObserver =
                SignUpViewModel.GetPropertyObserver<string>(nameof(SignUpViewModel.SignUpErrorMessage));
        }

        private static ISafeProviderForNonExistingUser CreateSafeProviderForNonExistingUser()
        {
            var safeProviderForNonExistingUser = Substitute.For<ISafeProviderForNonExistingUser>();

            // ReSharper disable once RedundantAssignment
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