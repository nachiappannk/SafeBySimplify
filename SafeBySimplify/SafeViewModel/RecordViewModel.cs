using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using SafeModel;

namespace SafeViewModel
{
    public class RecordViewModel : NotifiesPropertyChanged
    {
        private readonly IFileSafe _fileSafe;
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
                    IsRecordModified = true;
                }
            }
        }

        public string Id { get; set; }

        public string Tags
        {
            get { return _tags; }
            set
            {
                IsRecordModified = true;
                _tags = value; 
                
            }
        }

        public ObservableCollection<PasswordRecordViewModel> PasswordRecords { get; set; }
        public ObservableCollection<FileRecordViewModel> FileRecords { get; set; }

        public bool IsRecordModified
        {
            get { return _isRecordModified; }
            set
            {
                
                _isRecordModified = value;
                RecordChanged?.Invoke();
            }
        }

        public event Action RecordChanged;

        private bool _initialized = false;
        private string _tags;
        private bool _isRecordModified;

        public RecordViewModel(Record record, IFileSafe fileSafe, IFileIdGenerator fileIdGenerator)
        {
            _fileSafe = fileSafe;
            _fileIdGenerator = fileIdGenerator;
            Id = record.Header.Id;
            Name = record.Header.Name;
            Tags = record.Header.Tags;
            PasswordRecords = new ObservableCollection<PasswordRecordViewModel>();
            FileRecords = new ObservableCollection<FileRecordViewModel>();

            PasswordRecords.CollectionChanged += (sender, args) => { IsRecordModified = true; };
            FileRecords.CollectionChanged += (sender, args) => { IsRecordModified = true; };

            foreach (var passwordRecord in record.PasswordRecords)
            {
                AddNewPasswordRecord(passwordRecord.Name, passwordRecord.Value);
            }

            foreach (var fileRecord in record.FileRecords)
            {
                var fileRecordViewModel = new FileRecordViewModel(FileRecords, _fileSafe, fileRecord.AssociatedRecordId,
                    fileRecord.FileId, () => { IsRecordModified = true; })
                {
                    Name = fileRecord.Name,
                    Description = fileRecord.Description,
                    Extention = fileRecord.Extention
                };
                FileRecords.Add(fileRecordViewModel);
            }

            _initialized = true;
            AddNewPasswordRecord(string.Empty, string.Empty);
            IsRecordModified = false;
        }

        public void AddFileRecord(string fileUri)
        {
            IsRecordModified = false;
            var fileRecordViewModel =
                new FileRecordViewModel(FileRecords, _fileSafe, Id, _fileIdGenerator.GetFileId(),() => { IsRecordModified = true; })
                {
                    Name = Path.GetFileNameWithoutExtension(fileUri),
                    Extention = Path.GetExtension(fileUri)?.Replace(".", ""),
                    Description = string.Empty,
                };
            FileRecords.Add(fileRecordViewModel);
            _fileSafe.StoreFile(Id, _fileIdGenerator.GetFileId(), fileUri);
        }

        private void AddNewPasswordRecord(string name, string value)
        {
            var passwordRecordViewModel =
                new PasswordRecordViewModel(OnPasswordRecordModification, PasswordRecords)
                {
                    Name = name,
                    Value = value
                };
            PasswordRecords.Add(passwordRecordViewModel);
        }

        private void OnPasswordRecordModification()
        {
            IsRecordModified = true;
            if(!_initialized) return;
            var lastRecord = PasswordRecords.Last();
            if ((string.Empty != lastRecord.Name) || (string.Empty != lastRecord.Value))
            {
                AddNewPasswordRecord(string.Empty, string.Empty);
            }
        }

        public Record GetRecord()
        {
            var record = new Record
            {
                Header = new RecordHeader(),
                FileRecords = new List<FileRecord>(),
                PasswordRecords = new List<PasswordRecord>()
            };
            record.Header.Name = Name;
            record.Header.Id = Id;
            record.Header.Tags = Tags;
            var fileRecords = FileRecords.Select(x => new FileRecord()
            {
                Name = x.Name,
                Description = x.Description,
                Extention = x.Extention,
                FileId = x.FileRecordId,
                AssociatedRecordId = x.RecordId,
            }).ToList();
            record.FileRecords.AddRange(fileRecords);

            var passwordRecordViewModels = PasswordRecords.ToList();
            passwordRecordViewModels.RemoveAt(PasswordRecords.Count - 1);
            var passwordRecords = passwordRecordViewModels.Select(x => new PasswordRecord()
                {Name = x.Name, Value = x.Value}).ToList();
            record.PasswordRecords.AddRange(passwordRecords);
            return record;
        }
    }
}