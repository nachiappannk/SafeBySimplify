using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class SearchAndAddOperationViewModel : SingleOperationViewModel
    {
        private readonly ISafe _safe;
        private readonly Action<string> _recordSelectionAction;

        public SearchAndAddOperationViewModel(ISafe safe, Action<string> recordSelectionAction, Action addNewRecordAction)
        {
            _safe = safe;
            _recordSelectionAction = recordSelectionAction;
            IsSearchResultVisible = false;
            AddCommand = new DelegateCommand(addNewRecordAction);
            IsSearchInProgress = false;
        }

        private bool _isSearchResultVisible;
        public bool IsSearchResultVisible
        {
            get { return _isSearchResultVisible; }
            set
            {
                if(_isSearchResultVisible == value)return;
                _isSearchResultVisible = value;
                FirePropertyChanged();
            }
        }

        private void SearchAndUpdateSearchResults(string value, CancellationTokenSource cancellationTokenSource)
        {
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            var headers = GetRecordHeaders(value);
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            SearchResults = new ObservableCollection<RecordHeaderViewModel>
                (headers.Select(x => new RecordHeaderViewModel(x, () => { _recordSelectionAction(x.Id); })));
            IsSearchResultVisible = true;
            IsSearchInProgress = false;
        }

        private string _searchString = String.Empty;
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

        private List<RecordHeader> GetRecordHeaders(string searchText)
        {
            return _safe.GetRecordHeaders(searchText);
        }

        private void OnSearchTextChanged(string value)
        {
            TaskHolder?.Cancel();
            TaskHolder = null;
            if (string.IsNullOrWhiteSpace(value))
            {
                IsSearchResultVisible = false;
                IsSearchInProgress = false;
            }
            else
            {
                IsSearchInProgress = true;
                var taskHolder = new TaskHolder((cts) => SearchAndUpdateSearchResults(value, cts));
                TaskHolder = taskHolder;
            }
        }

        public TaskHolder TaskHolder { get; set; }


        private ObservableCollection<RecordHeaderViewModel> _searchResult;
        private bool _isSearchInProgress;

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

        public DelegateCommand AddCommand { get; set; }

        public bool IsSearchInProgress
        {
            get { return _isSearchInProgress; }
            set
            {
                if(_isSearchInProgress == value) return;
                _isSearchInProgress = value;
                FirePropertyChanged();
            }
        }
    }
}