using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class SignInViewModel : NotifiesPropertyChanged
    {
        private readonly ISafeProviderForExistingUser _safeProviderForExistingUser;
        private readonly Action<ISafe, string> _signUpCompletionCallback;

        public SignInViewModel(ISafeProviderForExistingUser safeProviderForExistingUser,
            Action<ISafe, string> signUpCompletionCallback)
        {
            _safeProviderForExistingUser = safeProviderForExistingUser;
            _signUpCompletionCallback = signUpCompletionCallback;

            AvailableUserNames = _safeProviderForExistingUser.GetUserNames().ToList();
            IsEnabled = AvailableUserNames.Count != 0;
            if(IsEnabled) _signInUserName = AvailableUserNames.ElementAt(0);
            _signInPassword = string.Empty;
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

        private string _signInPassword;
        public const string WrongPasswordErrorMessage = "Wrong Password";

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
        public DelegateCommand SignInCommand { get; set; }
        public List<string> AvailableUserNames { get; set; }
        public bool IsEnabled { get; set; }

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
    }
}