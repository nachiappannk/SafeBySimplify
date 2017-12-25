using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class RecordAlteringOperationViewModel : SingleOperationViewModel
    {
        private readonly ISafe _safe;
        private readonly string _recordId;
        private readonly Action _reloadAction;
        private readonly Action _closeAction;
        public RecordViewModel Record { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand DiscardCommand { get; set; }
        public DelegateCommand GoToSearchCommand { get; set; }

        public RecordAlteringOperationViewModel(ISafe safe, string recordId, IFileIdGenerator fileIdGenerator, Action reloadAction, Action closeAction)
        {
            _safe = safe;
            _recordId = recordId;
            _reloadAction = reloadAction;
            _closeAction = closeAction;
            var record = safe.GetRecord(recordId);
            Record = new RecordViewModel(record, safe, fileIdGenerator);
            SaveCommand = new DelegateCommand(Save);
            DeleteCommand = new DelegateCommand(Delete);
            DiscardCommand = new DelegateCommand(Discard);
            GoToSearchCommand = new DelegateCommand(GoToSearch);
        }

        private void GoToSearch()
        {
            _safe.ReoganizeFiles(_recordId);
            _closeAction.Invoke();

        }

        private void Discard()
        {
            _safe.ReoganizeFiles(_recordId);
            _reloadAction.Invoke();
        }

        private void Delete()
        {
            _safe.DeleteRecord(_recordId);
            _safe.ReoganizeFiles(_recordId);
            _closeAction.Invoke();
        }

        private void Save()
        {
            _safe.UpsertRecord(Record.GetRecord());
            _safe.ReoganizeFiles(_recordId);
            _reloadAction.Invoke();
        }
    }
}