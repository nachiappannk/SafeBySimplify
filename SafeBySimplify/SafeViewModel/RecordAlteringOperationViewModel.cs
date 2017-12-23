using System.Collections.ObjectModel;
using SafeModel;

namespace SafeViewModel
{
    public class RecordAlteringOperationViewModel : SingleOperationViewModel
    {
        private readonly ISafe _safe;

        public RecordAlteringOperationViewModel(ISafe safe, string recordId)
        {
            _safe = safe;
            Record = new RecordViewModel();
            var record = safe.GetRecord(recordId);
            Record.Name = record.Header.Name;
            Record.Tags = record.Header.Tags;
            Record.PasswordRecords = new ObservableCollection<PasswordRecordViewModel>();
            var passwordRecordViewModel = new PasswordRecordViewModel(() => { }, (x) => { }, (x) => { return true; });
        }

        public RecordViewModel Record { get; set; }
    }
}