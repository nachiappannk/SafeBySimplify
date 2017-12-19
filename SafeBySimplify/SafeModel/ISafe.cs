using System.Collections.Generic;

namespace SafeModel
{
    public interface ISafe
    {
        List<RecordHeader> GetRecordHeaders(string searchText);
    }
}