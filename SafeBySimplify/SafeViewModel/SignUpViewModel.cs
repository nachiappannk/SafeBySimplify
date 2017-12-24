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
        private bool _canSignUp = false;

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
            string errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(SignUpUserName))
                SetUpSignUpCommandStateAndSignUpErrorMessage(false, string.Empty);
            else if(string.IsNullOrWhiteSpace(SignUpPassword))
                SetUpSignUpCommandStateAndSignUpErrorMessage(false, string.Empty);
            else if(string.IsNullOrWhiteSpace(SignUpConfirmPassword))
                SetUpSignUpCommandStateAndSignUpErrorMessage(false, string.Empty);
            else if (SignUpConfirmPassword != SignUpPassword)
                SetUpSignUpCommandStateAndSignUpErrorMessage(false, PasswordMismatchingErrorMessage);
            else if (!_safeProviderForNonExistingUser.IsUserNameValidForNonExistingUser(SignUpUserName, out errorMessage))
                SetUpSignUpCommandStateAndSignUpErrorMessage(false, errorMessage);
            else if (!_safeProviderForNonExistingUser.IsPasswordValidForNonExistingUser(SignUpPassword, out errorMessage))
                SetUpSignUpCommandStateAndSignUpErrorMessage(false, errorMessage);
            else
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

    }
}