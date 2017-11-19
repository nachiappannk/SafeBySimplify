using System;
using Prism.Commands;

namespace SafeViewModel
{
    public class EntryStepViewModel : WorkFlowStepViewModel
    {
        public const string PasswordMismatchingErrorMessage = "Password and Confirm Password are different";
        public event Action GoToSettingsRequested;

        public DelegateCommand GoToSettingsCommand;

        public EntryStepViewModel()
        {
            GoToSettingsCommand = new DelegateCommand(() =>
            {
                GoToSettingsRequested?.Invoke();
            });

            SignUpCommand = new DelegateCommand(() =>
                {

                }, () => _canSignUp);
        }

        private bool _canSignUp = false;

        private string _signUpUserName;

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

        public string SignUpUserName
        {
            get { return _signUpUserName; }
            set {
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
            set {
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
            set {
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