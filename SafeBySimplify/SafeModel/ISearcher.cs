using System.Collections.Generic;

namespace SafeModel
{
    public interface ISearcher
    {
        List<RecordHeader> Search(List<RecordHeader> inputRecordHeaders, string searchString);
    }
}