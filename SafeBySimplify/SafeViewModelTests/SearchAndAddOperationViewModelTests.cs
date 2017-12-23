using System.Collections.Generic;
using System.Threading;
using NSubstitute;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;

namespace SafeViewModelTests
{
    [TestFixture]
    public class SearchAndAddOperationViewModelTests
    {
        private SearchAndAddOperationViewModel _searchAndAddOperationViewModel;
        private ISafe _safe;

        private string _searchText = "ss";

        private List<RecordHeader> _searchResults = new List<RecordHeader>()
        {
            new RecordHeader() {Name = "1", Tags = new List<string>()},
            new RecordHeader() {Name = "2", Tags = new List<string>()}
        };

        private int _timeTakenForSearching = 100;

        [SetUp]
        public void SetUp()
        {
            _safe = NSubstitute.Substitute.For<ISafe>();
            _searchAndAddOperationViewModel = new SearchAndAddOperationViewModel(_safe, (x) => { }, () => { });
        }

        public class Tests : SearchAndAddOperationViewModelTests
        {
            [Test]
            public void Search_result_is_initially_not_available()
            {
                Assert.AreEqual(false, _searchAndAddOperationViewModel.IsSearchResultVisible);
            }
        }

        public class SearchTextEnteredAndSearchResultsAreCorrect : SearchAndAddOperationViewModelTests
        {
            [SetUp]
            public void SearchTextEnteredAndSearchResultsAreCorrectSetUp()
            {
                _safe.GetRecordHeaders(_searchText).Returns(x =>
                {
                    Thread.Sleep(_timeTakenForSearching);
                    return _searchResults;
                });

                _searchAndAddOperationViewModel.SearchText = _searchText;
                _searchAndAddOperationViewModel.TaskHolder.WaitOnHoldingTask();
                //Assume.That(_searchResultVisibilityObserver.PropertyValue, "The search results are invisible");
                //var actualHeaders = _searchResultPropertyObserver.PropertyValue.Select(x => x.RecordHeader).ToList();
                //Assume.That(simpleSearchTextAndResult1.SearchResults.Count == actualHeaders.Count,
                //    "The search results are wrong (count)");
                //var isAllSearchResultsListed =
                //    actualHeaders.All(x => simpleSearchTextAndResult1.SearchResults.Contains(x));
                //Assume.That(isAllSearchResultsListed, "The search results are wrong");
            }

        }

       
    }
}