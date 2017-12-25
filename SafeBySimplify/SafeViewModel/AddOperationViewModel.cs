﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            Record = new RecordViewModel(safe, recordIdGenerator, fileIdGenerator);

            Record.Id = recordIdGenerator.GetRecordId();

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
                var record = new Record();
                record.Header = new RecordHeader();
                record.Header.Name = Record.Name;
                record.Header.Id = Record.Id;
                record.Header.Tags = Record.Tags;
                record.PasswordRecords = new List<PasswordRecord>();

                var passwordRecords = Record.PasswordRecords.ToList();
                var last = passwordRecords.Last();
                passwordRecords.Remove(last);

                foreach (var passwordRecord in passwordRecords)
                {
                    record.PasswordRecords.Add(new PasswordRecord() {Name = passwordRecord.Name, Value = passwordRecord.Value});
                }
                record.FileRecords = new List<FileRecord>();
                foreach (var fileRecord in Record.FileRecords)
                {
                    record.FileRecords.Add(new FileRecord()
                    {
                        Name = fileRecord.Name,
                        Extention = fileRecord.Extention,
                        FileId = fileRecord.FileRecordId,
                    });
                }


                safe.UpsertRecord(record);
                saveAction.Invoke(Record.Id);
            },() => CanExecuteSaveCommand );
        }

        
    }
}