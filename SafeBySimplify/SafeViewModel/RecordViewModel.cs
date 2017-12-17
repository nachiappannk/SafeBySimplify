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
        public ObservableCollection<PasswordRecord> PasswordRecords { get; set; }
        public List<FileRecord> FileRecords { get; set; }

        public RecordViewModel()
        {
            PasswordRecords = new ObservableCollection<PasswordRecord>();
            AddNewEmptyPasswordRecord();
        }

        private void AddNewEmptyPasswordRecord()
        {
            PasswordRecords.Add(new PasswordRecord(InsertEmptyRecordIfNecessary, (r) => PasswordRecords.Remove(r)));
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