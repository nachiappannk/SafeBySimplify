using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class RecordViewModel : NotifiesPropertyChanged
    {
        private readonly IFileSafe _fileSafe;
        private readonly IRecordIdGenerator _recordIdGenerator;
        private readonly IFileIdGenerator _fileIdGenerator;
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if(_name != value)
                { 
                    _name = value;
                    FirePropertyChanged();
                }
            }
        }

        public string Id { get; set; }

        public string Tags { get; set; }
        public ObservableCollection<PasswordRecordViewModel> PasswordRecords { get; set; }
        public ObservableCollection<FileRecordViewModel> FileRecords { get; set; }

        public void AddFileRecord(string fileUri)
        {
            var fileRecordViewModel = new FileRecordViewModel(FileRecords, _fileSafe, Id);
            fileRecordViewModel.Name = Path.GetFileNameWithoutExtension(fileUri);
            fileRecordViewModel.Extention = Path.GetExtension(fileUri).Replace(".","");
            FileRecords.Add(fileRecordViewModel);
            _fileSafe.StoreFile(Id, _fileIdGenerator.GetFileId(), fileUri);
        }

        public RecordViewModel(Record record, IFileSafe fileSafe, IFileIdGenerator fileIdGenerator)
        {
            _fileSafe = fileSafe;
            _fileIdGenerator = fileIdGenerator;
            Id = record.Header.Id;
            Name = String.Empty;
            Tags = string.Empty;
            PasswordRecords = new ObservableCollection<PasswordRecordViewModel>();
            FileRecords = new ObservableCollection<FileRecordViewModel>();
            AddNewEmptyPasswordRecord();
        }

        private void AddNewEmptyPasswordRecord()
        {
            PasswordRecords.Add(
                new PasswordRecordViewModel(InsertEmptyRecordIfNecessary, 
                (r) => PasswordRecords.Remove(r),
                (r) => PasswordRecords.Last() != r
                ));
            foreach (var passwordRecord in PasswordRecords)
            {
                passwordRecord.RemoveCommand.RaiseCanExecuteChanged();
            }
        }

        private void InsertEmptyRecordIfNecessary()
        {
            var lastRecord = PasswordRecords.Last();
            if ((string.Empty != lastRecord.Name) || (string.Empty != lastRecord.Value))
            {
                AddNewEmptyPasswordRecord();
            }

        }
    }
}