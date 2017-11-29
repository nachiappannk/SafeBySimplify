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
            SelectedOperation = new EmptyOperationViewModel(null);
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
                    (headers.Select(x => new RecordHeaderViewModel(x, () =>
                {

                    SelectedOperation = new RecordAlteringOperationViewModel(x);
                })));
                IsSearchResultVisible = true;
            }
            catch (Exception e)
            {

            }
        }

        public bool IsOperationsBarVisible { get; set; }

        private ObservableCollection<RecordHeaderViewModel> _searchResult;

        public ObservableCollection<RecordHeaderViewModel> SearchResults
        {
            get { return _searchResult; }
            set
            {
                if (_searchResult != value)
                {
                    _searchResult = value;
                    FirePropertyChanged();
                }
            }
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

        private bool _isSearchResultVisible;

        public bool IsSearchResultVisible
        {
            get { return _isSearchResultVisible; }
            set
            {
                if (_isSearchResultVisible != value)
                {
                    _isSearchResultVisible = value;
                    FirePropertyChanged();
                }
            }
        }
    }
}