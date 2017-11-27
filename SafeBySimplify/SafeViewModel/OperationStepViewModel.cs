using System;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class OperationStepViewModel : WorkFlowStepViewModel, IHasSafe
    {
        public event Action GoToEntryStepRequested;
        public OperationStepViewModel()
        {
            SignOutCommand = new DelegateCommand(() => { GoToEntryStepRequested?.Invoke();});
        }

        public ISafe Safe { get; set; }
        public DelegateCommand SignOutCommand { get; set; }
    }

    public interface IHasSafe
    {
        ISafe Safe { get; set; }    
    }
}