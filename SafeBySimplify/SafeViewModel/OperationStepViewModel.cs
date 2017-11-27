using System;
using System.Threading;
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
            IsOperationsBarVisible = true;
        }

        public ISafe Safe { get; set; }
        public DelegateCommand SignOutCommand { get; set; }

        private string _searchString;

        public string SearchText
        {
            get { return _searchString; }
            set
            {
                if (_searchString != value)
                {
                    _searchString = value;
                    OnSearchTextChanged(value);
                }
            }
        }

        private CancellationTokenSource _tokenSource;

        private void OnSearchTextChanged(string value)
        {
            if (_tokenSource != null)
            {
                _tokenSource.Cancel();
            }
            _tokenSource = new CancellationTokenSource();
            try
            {
                Safe.GetRecordsAsync(value, _tokenSource.Token);
            }
            catch (Exception e)
            {

            }
        }

        public bool IsOperationsBarVisible { get; set; }
    }


}