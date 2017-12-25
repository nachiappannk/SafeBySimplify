/*
MIT License

Copyright(c) 2017 Nachiappan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using NSubstitute;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;

namespace SafeViewModelTests
{
    public partial class SearchAndAddOperationViewModelTests
    {
        public partial class SearchTextEntered : SearchAndAddOperationViewModelTests
        {

            private string _searchText = "ss";

            private List<RecordHeader> _searchResults = new List<RecordHeader>()
            {
                new RecordHeader() {Name = "record name 1", Id = "some id", Tags = "tag11;tag12;tag13"},
                new RecordHeader() {Name = "record name 2", Id = "some other id", Tags = "tag21;tag22;tag23"}
            };

            private int _timeTakenForSearching = 600;

            [SetUp]
            public void SearchTextEnteredSetUp()
            {
                _safe.GetRecordHeaders(_searchText).Returns(x =>
                {
                    Thread.Sleep(_timeTakenForSearching);
                    return _searchResults;
                });
                _searchAndAddOperationViewModel.SearchText = _searchText;
            }

            [Test]
            public void When_serach_text_is_cleared_then_search_results_are_made_invisible()
            {
                _searchAndAddOperationViewModel.SearchText = String.Empty;
                Assert.False(_searchResultVisibilityObserver.PropertyValue);
            }

            [Test]
            public void When_serach_text_is_cleared_then_search_progress_indicator_is_disabled()
            {
                _searchAndAddOperationViewModel.SearchText = String.Empty;
                Assert.False(_searchProgressIndicatorObserver.PropertyValue);
            }

            public class SearchResultsAreAvailable : SearchTextEntered
            {

                private ObservableCollection<RecordHeaderViewModel> _recordHeaderViewModels;

                [SetUp]
                public void SearchTextEnteredAndNumberOfSearchResultsAreCorrectSetUp()
                {
                    _searchAndAddOperationViewModel.TaskHolder.WaitOnHoldingTask();
                    Assume.That(_searchResultVisibilityObserver.PropertyValue, "The search results are invisible");
                    _recordHeaderViewModels = _searchResultPropertyObserver.PropertyValue;
                }

                [Test]
                public void Seach_in_progress_indicator_is_disabled()
                {
                    Assert.False(_searchProgressIndicatorObserver.PropertyValue);
                }

                [Test]
                public void Search_result_has_the_correct_names_in_correct_order()
                {
                    var headerNames = _recordHeaderViewModels.Select(x => x.Name).ToList();
                    CollectionAssert.AreEqual(_searchResults.Select(x => x.Name), headerNames);
                }

                [Test]
                public void Search_result_has_the_correct_ids_in_correct_order()
                {
                    var ids = _recordHeaderViewModels.Select(x => x.Id).ToList();
                    CollectionAssert.AreEqual(_searchResults.Select(x => x.Id), ids);
                }

                [Test]
                public void Search_result_has_the_correct_tags_in_correct_order()
                {
                    var listOfTags = _recordHeaderViewModels.Select(x => x.Tags).ToList();
                    for (int i = 0; i < listOfTags.Count; i++)
                    {
                        var expectedTags = _searchResults.ElementAt(i).Tags.Split(';').ToList();
                        CollectionAssert.AreEqual(expectedTags, listOfTags.ElementAt(i));
                    }
                }   
            }
        }
    }
}