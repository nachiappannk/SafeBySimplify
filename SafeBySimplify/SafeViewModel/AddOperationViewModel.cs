using System;
using System.Collections.Generic;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class AddOperationViewModel : SingleOperationViewModel
    {

        private bool _canExecuteSaveCommand;
        private bool CanExecuteSaveCommand
        {
            get { return _canExecuteSaveCommand; }
            set
            {
                if (_canExecuteSaveCommand == value) return;
                _canExecuteSaveCommand = value;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public RecordViewModel Record { get; set; }
        public DelegateCommand DiscardCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }


        public AddOperationViewModel(Action discardAction, Action<string> saveAction, IRecordIdGenerator recordIdGenerator, IFileIdGenerator fileIdGenerator, ISafe safe)
        {
            Record = new RecordViewModel(CreateEmptyRecord(recordIdGenerator.GetRecordId()), safe, fileIdGenerator);
            Record.PropertyChanged += (sender, args) =>
            {
                CanExecuteSaveCommand = !string.IsNullOrWhiteSpace(Record.Name);
            };

            DiscardCommand = new DelegateCommand
                (() =>
                    {
                        safe.ReoganizeFiles(Record.Id);
                        discardAction.Invoke();
                    }
                );

            SaveCommand = new DelegateCommand(() =>
            {
                var record = Record.GetRecord();
                safe.UpsertRecord(record);
                safe.ReoganizeFiles(Record.Id);
                saveAction.Invoke(Record.Id);
            },() => CanExecuteSaveCommand );
        }

        private Record CreateEmptyRecord(string recordId)
        {
            var record = new Record
            {
                Header = new RecordHeader(),
                FileRecords = new List<FileRecord>(),
                PasswordRecords = new List<PasswordRecord>()
            };
            record.Header.Id = recordId;
            record.Header.Name = string.Empty;
            record.Header.Tags = string.Empty;
            return record;
        }
    }
}