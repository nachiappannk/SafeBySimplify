using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class OperationStepViewModel : WorkFlowStepViewModel
    {
        public OperationStepViewModel(ISafe safe, Action goToEntryStepAction)
        {
            Safe = safe;
            SignOutCommand = new DelegateCommand(() =>
            {
                goToEntryStepAction.Invoke();
                Safe = null;
            });
            //AddCommand = new DelegateCommand(GoToAddOperation);
            GoToSearchAndAddOperation();
        }

        private void GoToAddOperation()
        {
            SelectedOperation = new AddOperationViewModel(GoToSearchAndAddOperation, GoToAlteringOperation);
        }

        private void GoToSearchAndAddOperation()
        {
            SelectedOperation = new SearchAndAddOperationViewModel(Safe, GoToAlteringOperation, GoToAddOperation);
        }

        public ISafe Safe { get; set; }
        public DelegateCommand SignOutCommand { get; set; }

        private string _searchString;
        
        private void GoToAlteringOperation(RecordHeader x)
        {
            SelectedOperation = new RecordAlteringOperationViewModel(x);
        }

        private SingleOperationViewModel _selectedOperation;

        public SingleOperationViewModel SelectedOperation
        {
            get { return _selectedOperation; }
            set
            {
                if (_selectedOperation != value)
                {
                    _selectedOperation = value;
                    FirePropertyChanged();
                }
            }
        }

        
        //public DelegateCommand AddCommand { get; set; }
    }

    

}