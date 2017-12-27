using System;
using System.Collections.Generic;
using System.Linq;

namespace SafeModel
{
    public class Searcher : ISearcher
    {
        public List<RecordHeader> Search(List<RecordHeader> inputRecordHeaders, string searchString)
        {
            var searchTokens = searchString.Split(' ');
            var weightedRecordHeaders = inputRecordHeaders.Select(x =>
                new WeightedRecordHeader {Weight = WeighRecordHeader(x, searchTokens), RecordHeader = x}).ToList();
            var orderedRecordHeaders = weightedRecordHeaders.OrderByDescending(x => x.Weight);
            return orderedRecordHeaders.Select(x => x.RecordHeader).ToList();
        }

        private int WeighRecordHeader(RecordHeader recordHeader, string[] searchTokens)
        {
            var title = recordHeader.Name;
            var titleTokens = title.Split(' ').ToList();
            int weight = 0;
            foreach (var searchToken in searchTokens)
            {
                if (!string.IsNullOrWhiteSpace(searchToken))
                    weight = weight + WeighSearchTokenWeight(titleTokens, searchToken);
            }
            return weight;
        }

        int WeighSearchTokenWeight(List<string> tokens, string searchToken)
        {
            if (tokens.Contains(searchToken)) return 10;
            if (tokens.Any(x => x.StartsWith(searchToken))) return 5;
            if (tokens.Any(x => x.Contains(searchToken))) return 2;
            return 0;
        }
    }


    public class WeightedRecordHeader
    {
        public int Weight { get; set; }
        public RecordHeader RecordHeader { get; set; }
    }
}