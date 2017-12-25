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

        public RecordViewModel(Record record, IFileSafe fileSafe, IFileIdGenerator fileIdGenerator)
        {
            _fileSafe = fileSafe;
            _fileIdGenerator = fileIdGenerator;
            Id = record.Header.Id;
            Name = record.Header.Name;
            Tags = record.Header.Tags;
            PasswordRecords = new ObservableCollection<PasswordRecordViewModel>();
            FileRecords = new ObservableCollection<FileRecordViewModel>();
            AddNewEmptyPasswordRecord();
        }

        public void AddFileRecord(string fileUri)
        {
            var fileRecordViewModel = new FileRecordViewModel(FileRecords, _fileSafe, Id);
            fileRecordViewModel.Name = Path.GetFileNameWithoutExtension(fileUri);
            fileRecordViewModel.Extention = Path.GetExtension(fileUri)?.Replace(".", "");
            FileRecords.Add(fileRecordViewModel);
            _fileSafe.StoreFile(Id, _fileIdGenerator.GetFileId(), fileUri);
        }

        private void AddNewEmptyPasswordRecord()
        {
            var passwordRecordViewModel = new PasswordRecordViewModel(InsertEmptyRecordIfNecessary,PasswordRecords);
            PasswordRecords.Add(passwordRecordViewModel);
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