using System.Collections.Generic;

namespace SafeModel.Standard
{
    public interface ISafe : IFileSafe
    {
        List<RecordHeader> GetRecordHeaders(string searchText);
        Record GetRecord(string recordId);
        string WorkingDirectory { get; set; }
        string UserName { get; set; }

        void ReorganizeFiles(string recordId);
        void UpsertRecord(Record record);

        void DeleteRecord(string recordId);
    }
}
