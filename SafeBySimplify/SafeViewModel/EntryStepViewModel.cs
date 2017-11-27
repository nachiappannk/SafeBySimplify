using System;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class EntryStepViewModel : WorkFlowStepViewModel
    {
        public DelegateCommand GoToSettingsCommand { get; set; }

        public SignUpViewModel SignUpViewModel { get; set; }

        public SignInViewModel SignInViewModel { get; set; }

        public EntryStepViewModel(
            ISafeProvider safeProvider, Action goToSettingStepAction, Action<ISafe> goToOperationStep)
        {
            GoToSettingsCommand = new DelegateCommand(goToSettingStepAction); 

            SignUpViewModel = new SignUpViewModel(safeProvider, goToOperationStep);
            SignInViewModel = new SignInViewModel(safeProvider, goToOperationStep);
        }


        
    }
}