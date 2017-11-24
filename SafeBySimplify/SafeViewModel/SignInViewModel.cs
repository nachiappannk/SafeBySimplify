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

        public SignInViewModel(ISafeProviderForExistingUser safeProviderForExistingUser)
        {
            _safeProviderForExistingUser = safeProviderForExistingUser;

            AvailableUserNames = _safeProviderForExistingUser.GetUserNames().ToList();
            IsEnabled = AvailableUserNames.Count != 0;
            if(IsEnabled) _signInUserName = AvailableUserNames.ElementAt(0);
            _signInPassword = string.Empty;
            SignInCommand = new DelegateCommand(Login, CanLogIn);
        }

        private void Login()
        {
            ISafe safe;
            if (!_safeProviderForExistingUser.TryCreateSafeForExistingUser(SignInUserName, SignInPassword, out safe))
            {
                ErrorMessage = WrongPasswordErrorMessage;
            }
        }

        private bool CanLogIn()
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