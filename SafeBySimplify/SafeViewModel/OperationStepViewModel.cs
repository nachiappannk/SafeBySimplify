using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            SearchResults = new ObservableCollection<RecordHeaderViewModel>();
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

        private async void OnSearchTextChanged(string value)
        {
            _tokenSource?.Cancel();
            _tokenSource = new CancellationTokenSource();
            try
            {
                var headers = await Safe.GetRecordsAsync(value, _tokenSource.Token);
                SearchResults = new ObservableCollection<RecordHeaderViewModel>
                    (headers.Select(x => new RecordHeaderViewModel(x)));
            }
            catch (Exception e)
            {

            }
        }

        public bool IsOperationsBarVisible { get; set; }
        public ObservableCollection<RecordHeaderViewModel> SearchResults { get; set; }
    }


}