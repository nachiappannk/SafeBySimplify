using System.Collections.Generic;

namespace SafeModel.Standard
{
    public interface ISearcher
    {
        List<RecordHeader> Search(List<RecordHeader> inputRecordHeaders, string searchString);
    }
}
