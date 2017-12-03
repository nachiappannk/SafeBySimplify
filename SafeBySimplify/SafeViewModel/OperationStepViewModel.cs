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
            IsOperationsChangingPossible = true;
            SearchResults = new ObservableCollection<RecordHeaderViewModel>();
            GoToEmptyOperation();
            AddCommand = new DelegateCommand(
                () =>
                {
                    IsOperationsChangingPossible = false;
                    SelectedOperation = new AddOperationViewModel(GoToEmptyOperation);
                });
        }

        private void GoToEmptyOperation()
        {
            SelectedOperation = new EmptyOperationViewModel(() => { IsSearchResultVisible = false; });
            IsOperationsChangingPossible = true;
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
                    FirePropertyChanged();
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
                    SearchText = "";
                    SelectedOperation = new RecordAlteringOperationViewModel(x);
                    IsSearchResultVisible = false;
                })));
                IsSearchResultVisible = true;
            }
            catch (Exception e)
            {

            }
        }

        private bool _isOperationChangeingPossible;

        public bool IsOperationsChangingPossible
        {
            get { return _isOperationChangeingPossible; }
            set {
                if (_isOperationChangeingPossible != value)
                {
                    _isOperationChangeingPossible = value;
                    FirePropertyChanged();
                }
            }
        }

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

        public DelegateCommand AddCommand { get; set; }
    }
}