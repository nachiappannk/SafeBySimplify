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
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class SignInViewModel : NotifiesPropertyChanged
    {
        public const string WrongPasswordErrorMessage = "Wrong Password";

        private readonly ISafeProviderForExistingUser _safeProviderForExistingUser;
        private readonly Action<ISafe, string> _signUpCompletionCallback;

        public DelegateCommand SignInCommand { get; set; }
        public List<string> AvailableUserNames { get; set; }
        public bool IsEnabled { get; set; }

        private string _signInUserName;
        public string SignInUserName
        {
            get { return _signInUserName; }
            set
            {
                if (_signInUserName != value)
                {
                    _signInUserName = value;
                    ErrorMessage = string.Empty;
                    SignInPassword = String.Empty;
                }
            }
        }

        private string _signInPassword = String.Empty;
        public string SignInPassword
        {
            get { return _signInPassword; }
            set
            {
                if (_signInPassword != value)
                {
                    _signInPassword = value;
                    ErrorMessage = string.Empty;
                    SignInCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    FirePropertyChanged();
                }
            }
        }

        public SignInViewModel(ISafeProviderForExistingUser safeProviderForExistingUser,
            Action<ISafe, string> signUpCompletionCallback)
        {
            _safeProviderForExistingUser = safeProviderForExistingUser;
            _signUpCompletionCallback = signUpCompletionCallback;

            AvailableUserNames = _safeProviderForExistingUser.GetUserNames().ToList();
            IsEnabled = AvailableUserNames.Count != 0;
            if(IsEnabled) _signInUserName = AvailableUserNames.ElementAt(0);
            SignInCommand = new DelegateCommand(SignIn, CanSignIn);
        }

        private void SignIn()
        {
            ISafe safe;
            if (!_safeProviderForExistingUser.TryCreateSafeForExistingUser(SignInUserName, SignInPassword, out safe))
            {
                ErrorMessage = WrongPasswordErrorMessage;
            }
            else
            {
                _signUpCompletionCallback.Invoke(safe, SignInUserName);
            }
        }

        private bool CanSignIn()
        {
            return SignInPassword.Length != 0;
        }
    }
}