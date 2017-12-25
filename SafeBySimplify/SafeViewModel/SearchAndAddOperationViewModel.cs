/*
MIT License

Copyright(c) 2017 Nachiappan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/


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

        private bool _isSearchResultVisible;
        public bool IsSearchResultVisible
        {
            get { return _isSearchResultVisible; }
            set
            {
                if (_isSearchResultVisible == value) return;
                _isSearchResultVisible = value;
                FirePropertyChanged();
            }
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
        public TaskHolder TaskHolder { get; set; }

        public DelegateCommand AddCommand { get; set; }


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


        private bool _isSearchInProgress;

        public bool IsSearchInProgress
        {
            get { return _isSearchInProgress; }
            set
            {
                if (_isSearchInProgress == value) return;
                _isSearchInProgress = value;
                FirePropertyChanged();
            }
        }

        public SearchAndAddOperationViewModel(ISafe safe, Action<string> recordSelectionAction, Action addNewRecordAction)
        {
            _safe = safe;
            _recordSelectionAction = recordSelectionAction;
            IsSearchResultVisible = false;
            AddCommand = new DelegateCommand(addNewRecordAction);
            IsSearchInProgress = false;
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
    }
}