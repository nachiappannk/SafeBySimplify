using System.Collections.Generic;

namespace SafeModel
{
    public class Searcher : ISearcher
    {
        public List<RecordHeader> Search(List<RecordHeader> inputRecordHeaders, string searchString)
        {
            return inputRecordHeaders;
        }
    }
}