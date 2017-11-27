using System;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class SignUpViewModel : NotifiesPropertyChanged
    {
        

        private readonly ISafeProviderForNonExistingUser _safeProviderForNonExistingUser;
        private readonly Action<ISafe> _signUpCompletionCallback;
        public const string PasswordMismatchingErrorMessage = "Password and Confirm Password are different";
        private bool _canSignUp = false;

        public SignUpViewModel(ISafeProviderForNonExistingUser safeProviderForNonExistingUser,
            Action<ISafe> signUpCompletionCallback)
        {
            _safeProviderForNonExistingUser = safeProviderForNonExistingUser;
            _signUpCompletionCallback = signUpCompletionCallback;


            SignUpCommand = new DelegateCommand(
                () =>
                {
                    var safe = _safeProviderForNonExistingUser
                    .CreateSafeForNonExistingUser(SignUpUserName, SignUpPassword, SignUpPassword);
                    _signUpCompletionCallback.Invoke(safe);
                }, 
                () => _canSignUp);
        }

        private void ComputeCanSignUp()
        {
            if (string.IsNullOrWhiteSpace(SignUpUserName)
                || string.IsNullOrWhiteSpace(SignUpPassword)
                || string.IsNullOrWhiteSpace(SignUpConfirmPassword))
            {
                SetUpSignUpCommandStateAndSignUpErrorMessage(false, string.Empty);
                return;
            }

            if (SignUpConfirmPassword != SignUpPassword)
            {
                SetUpSignUpCommandStateAndSignUpErrorMessage(false, PasswordMismatchingErrorMessage);
                return;
            }

            string errorMessage = string.Empty;
            if (!_safeProviderForNonExistingUser.IsUserNameValidForNonExistingUser(SignUpUserName, out errorMessage))
            {
                SetUpSignUpCommandStateAndSignUpErrorMessage(false, errorMessage);
                return;
            }
            errorMessage = string.Empty;
            if (!_safeProviderForNonExistingUser.IsPasswordValidForNonExistingUser(SignUpPassword, out errorMessage))
            {
                SetUpSignUpCommandStateAndSignUpErrorMessage(false, errorMessage);
                return;
            }
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