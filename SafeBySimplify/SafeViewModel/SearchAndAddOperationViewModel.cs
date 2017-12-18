﻿using System;
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
        private readonly Action<RecordHeader> _recordModifierSelectorAction;

        public SearchAndAddOperationViewModel(ISafe safe, Action<RecordHeader> recordModifierSelectorAction, Action addNewRecordAction)
        {
            _safe = safe;
            _recordModifierSelectorAction = recordModifierSelectorAction;
            IsSearchResultVisible = false;
            AddCommand = new DelegateCommand(addNewRecordAction);
        }

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
                (headers.Select(x => new RecordHeaderViewModel(x, () => { _recordModifierSelectorAction(x); })));
            IsSearchResultVisible = true;
        }

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

        private List<RecordHeader> GetRecordHeaders(string searchText)
        {
            return _safe.GetRecordHeaders(searchText);
        }

        private void OnSearchTextChanged(string value)
        {
            var taskHolder = new TaskHolder((cts) => SearchAndUpdateSearchResults(value, cts));
            TaskHolder?.Cancel();
            TaskHolder = taskHolder;
        }

        public TaskHolder TaskHolder { get; set; }


        private ObservableCollection<RecordHeaderViewModel> _searchResult;
        private bool _isSearchResultVisible;

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
    }
}