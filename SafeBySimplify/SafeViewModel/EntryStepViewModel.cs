using System;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class EntryStepViewModel : WorkFlowStepViewModel
    {
        
        public event Action GoToSettingsRequested;
        public event Action GoToOperationsRequested;

        public DelegateCommand GoToSettingsCommand;

        public SignUpViewModel SignUpViewModel { get; set; }

        private void OnSignUpCompletion()
        {
            GoToOperationsRequested?.Invoke();            
        }

        public EntryStepViewModel(ISafeProviderForNonExistingUser safeProviderForNonExistingUser, IHasSafe hasSafe)
        {
            GoToSettingsCommand = new DelegateCommand(() =>
            {
                GoToSettingsRequested?.Invoke();
            }); 

            SignUpViewModel = new SignUpViewModel(safeProviderForNonExistingUser, hasSafe, OnSignUpCompletion);
        }


        
    }
}