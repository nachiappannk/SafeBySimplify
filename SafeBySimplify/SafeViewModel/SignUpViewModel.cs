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
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class SignUpViewModel : NotifiesPropertyChanged
    {
        private readonly ISafeProviderForNonExistingUser _safeProviderForNonExistingUser;
        private readonly Action<ISafe, string> _signUpCompletionCallback;
        public const string PasswordMismatchingErrorMessage = "Password and Confirm Password are different";
        private bool _canSignUp;

        private string _signUpErrorMessage = string.Empty;

        public string SignUpErrorMessage
        {
            get { return _signUpErrorMessage; }
            set
            {
                if (_signUpErrorMessage != value)
                {
                    _signUpErrorMessage = value;
                    FirePropertyChanged();
                }
            }
        }

        private string _signUpUserName;
        public string SignUpUserName
        {
            get { return _signUpUserName; }
            set
            {
                if (_signUpUserName != value)
                {
                    _signUpUserName = value;
                    ComputeCanSignUp();
                }
            }
        }


        private string _signUpPassword;

        public string SignUpPassword
        {
            get { return _signUpPassword; }
            set
            {
                if (_signUpPassword != value)
                {
                    _signUpPassword = value;
                    ComputeCanSignUp();
                }
            }
        }

        private string _signUpConfirmPassword;

        public string SignUpConfirmPassword
        {
            get { return _signUpConfirmPassword; }
            set
            {
                if (_signUpConfirmPassword != value)
                {
                    _signUpConfirmPassword = value;
                    ComputeCanSignUp();
                }
            }
        }

        public DelegateCommand SignUpCommand { get; set; }

        public SignUpViewModel(ISafeProviderForNonExistingUser safeProviderForNonExistingUser,
            Action<ISafe, string> signUpCompletionCallback)
        {
            _safeProviderForNonExistingUser = safeProviderForNonExistingUser;
            _signUpCompletionCallback = signUpCompletionCallback;
            SignUpCommand = new DelegateCommand( SignUp,() => _canSignUp);
        }

        private void SignUp()
        {
            var safe = _safeProviderForNonExistingUser
                .CreateSafeForNonExistingUser(SignUpUserName, SignUpPassword, SignUpPassword);
            _signUpCompletionCallback.Invoke(safe, SignUpUserName);
        }

        private void ComputeCanSignUp()
        {
            // ReSharper disable once RedundantAssignment
            string errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(SignUpUserName)) DisableSignUpWithNoError();
            else if(string.IsNullOrWhiteSpace(SignUpPassword)) DisableSignUpWithNoError();
            else if (string.IsNullOrWhiteSpace(SignUpConfirmPassword)) DisableSignUpWithNoError();
            else if (SignUpConfirmPassword != SignUpPassword) DisableSignUpWithError(PasswordMismatchingErrorMessage);
            else if (!_safeProviderForNonExistingUser.IsUserNameValidForNonExistingUser(SignUpUserName, out errorMessage))
                DisableSignUpWithError(errorMessage);
            else if (!_safeProviderForNonExistingUser.IsPasswordValidForNonExistingUser(SignUpPassword, out errorMessage))
                DisableSignUpWithError(errorMessage);
            else EnableSignUpWithNoError();
        }

        private void DisableSignUpWithNoError()
        {
            SetUpSignUpCommandStateAndSignUpErrorMessage(false, string.Empty);
        }

        private void DisableSignUpWithError(string errorMessage)
        {
            SetUpSignUpCommandStateAndSignUpErrorMessage(false, errorMessage);
        }

        private void EnableSignUpWithNoError()
        {
            SetUpSignUpCommandStateAndSignUpErrorMessage(true, string.Empty);
        }

        private void SetUpSignUpCommandStateAndSignUpErrorMessage(bool canSignUp, string errorMessage)
        {
            if (_canSignUp != canSignUp)
            {
                _canSignUp = canSignUp;
                SignUpCommand.RaiseCanExecuteChanged();
            }
            SignUpErrorMessage = errorMessage;
        }


    }
}