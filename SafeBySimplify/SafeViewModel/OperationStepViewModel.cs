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
            IsOperationsChangingPossible = true;
            SearchResults = new ObservableCollection<RecordHeaderViewModel>();
            GoToEmptyOperation();
            AddCommand = new DelegateCommand(GoToAddOperation);
        }

        private void GoToAddOperation()
        {
            IsOperationsChangingPossible = false;
            SelectedOperation = new AddOperationViewModel(GoToEmptyOperation, GoToAlteringOperation);
            IsSearchResultVisible = false;
            SetSearchTextWithoutSearching(string.Empty);

        }

    private void GoToEmptyOperation()
        {
            SelectedOperation = new EmptyOperationViewModel(HighLightSelectedOperation);
            IsOperationsChangingPossible = true;
        }

        private void HighLightSelectedOperation()
        {
            SetSearchTextWithoutSearching(string.Empty);
            IsSearchResultVisible = false;
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

        private void SetSearchTextWithoutSearching(string searchText)
        {
            if (_searchString != searchText)
            {
                _searchString = searchText;
                FirePropertyChanged(nameof(SearchText));
            }

        }

        private CancellationTokenSource _tokenSource;

        public int NumberOfOnGoingSearches
        {
            get { return _numberOfOnGoingSearches; }
            set
            {
                _numberOfOnGoingSearches = value;
                IsSearchInProgress = _numberOfOnGoingSearches != 0;
            }
        }

        public bool IsSearchInProgress { get; set; }

        private void SearchAndUpdateSearchResults(string value, CancellationTokenSource cancellationTokenSource)
        {
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            var headers = GetRecordHeaders(value);
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            SearchResults = new ObservableCollection<RecordHeaderViewModel>
                    (headers.Select(x => new RecordHeaderViewModel(x, () => { GoToAlteringOperation(x); })));
            IsSearchResultVisible = true;
        }

        private void OnSearchTextChanged(string value)
        {
            var taskHolder = new TaskHolder((cts) =>  SearchAndUpdateSearchResults(value, cts));
            TaskHolder?.Cancel();
            TaskHolder = taskHolder;
        }

        public TaskHolder TaskHolder { get; set; }

        private List<RecordHeader> GetRecordHeaders(string searchText)
        {
            return Safe.GetRecordHeaders(searchText);
        }


        private void GoToAlteringOperation(RecordHeader x)
        {
            SetSearchTextWithoutSearching(string.Empty);
            SelectedOperation = new RecordAlteringOperationViewModel(x);
            IsSearchResultVisible = false;
            IsOperationsChangingPossible = true;
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
        private int _numberOfOnGoingSearches;

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

    public class TaskHolder
    {
        public TaskHolder(Action<CancellationTokenSource> taskDelegate)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _runningTask = Task.Run(() =>
            {
                try
                {
                    taskDelegate.Invoke(_cancellationTokenSource);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }

        public Task _runningTask;

        private CancellationTokenSource _cancellationTokenSource;

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        public void WaitOnHoldingTask()
        {
            _runningTask.Wait();
        }

    }

}