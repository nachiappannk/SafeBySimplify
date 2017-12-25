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
        public OperationStepViewModel(string userName, ISafe safe, Action goToEntryStepAction)
        {
            _idGenerator = new IdGenerator();
            Safe = safe;
            UserName = userName;
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
            SelectedOperation = new AddOperationViewModel(GoToSearchAndAddOperation, GoToAlteringOperation, _idGenerator, _idGenerator, Safe);
        }

        private void GoToSearchAndAddOperation()
        {
            SelectedOperation = new SearchAndAddOperationViewModel(Safe, GoToAlteringOperation, GoToAddOperation);
        }

        public ISafe Safe { get; set; }
        public DelegateCommand SignOutCommand { get; set; }

        private string _searchString;
        
        private void GoToAlteringOperation(string recordId)
        {
            SelectedOperation = new RecordAlteringOperationViewModel(
                Safe, recordId, _idGenerator, 
                ()=>GoToAlteringOperation(recordId),
                GoToSearchAndAddOperation);
        }

        private SingleOperationViewModel _selectedOperation;
        private IdGenerator _idGenerator;

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

        public string UserName { get; set; }


    }

    

}