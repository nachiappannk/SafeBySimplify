using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
                }
            }
        }

        public string Id { get; set; }
        public string Tags { get; set; }
        public ObservableCollection<PasswordRecordViewModel> PasswordRecords { get; set; }
        public ObservableCollection<FileRecordViewModel> FileRecords { get; set; }

        private bool _initialized = false;

        public RecordViewModel(Record record, IFileSafe fileSafe, IFileIdGenerator fileIdGenerator)
        {
            _fileSafe = fileSafe;
            _fileIdGenerator = fileIdGenerator;
            Id = record.Header.Id;
            Name = record.Header.Name;
            Tags = record.Header.Tags;
            PasswordRecords = new ObservableCollection<PasswordRecordViewModel>();
            FileRecords = new ObservableCollection<FileRecordViewModel>();

            foreach (var passwordRecord in record.PasswordRecords)
            {
                AddNewPasswordRecord(passwordRecord.Name, passwordRecord.Value);
            }

            foreach (var fileRecord in record.FileRecords)
            {
                var fileRecordViewModel = new FileRecordViewModel(FileRecords, _fileSafe, fileRecord.AssociatedRecordId,
                    fileRecord.FileId)
                {
                    Name = fileRecord.Name,
                    Description = fileRecord.Description,
                    Extention = fileRecord.Extention
                };
                FileRecords.Add(fileRecordViewModel);
            }

            _initialized = true;
            AddNewPasswordRecord(string.Empty, string.Empty);
        }

        public void AddFileRecord(string fileUri)
        {
            var fileRecordViewModel =
                new FileRecordViewModel(FileRecords, _fileSafe, Id, _fileIdGenerator.GetFileId())
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
                new PasswordRecordViewModel(InsertEmptyRecordIfNecessary, PasswordRecords)
                {
                    Name = name,
                    Value = value
                };
            PasswordRecords.Add(passwordRecordViewModel);
        }

        private void InsertEmptyRecordIfNecessary()
        {
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