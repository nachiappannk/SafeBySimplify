using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;

namespace SafeViewModel
{
    public class RecordViewModel : NotifiesPropertyChanged
    {
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

        public List<string> Tags { get; set; }
        public ObservableCollection<PasswordRecordViewModel> PasswordRecords { get; set; }
        public List<FileRecord> FileRecords { get; set; }

        public void AddFileRecord(string fileUri)
        {

        }

        public RecordViewModel()
        {
            PasswordRecords = new ObservableCollection<PasswordRecordViewModel>();
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