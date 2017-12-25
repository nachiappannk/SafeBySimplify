using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using NSubstitute;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    [TestFixture]
    public class SearchAndAddOperationViewModelTests
    {
        private SearchAndAddOperationViewModel _searchAndAddOperationViewModel;
        private ISafe _safe;
        private bool _isRecordCreationRequested = false;
        private ViewModelPropertyObserver<bool> _searchResultVisibilityObserver;
        private ViewModelPropertyObserver<ObservableCollection<RecordHeaderViewModel>> _searchResultPropertyObserver;
        private ViewModelPropertyObserver<bool> _searchProgressIndicatorObserver;

        [SetUp]
        public void SetUp()
        {
            _safe = Substitute.For<ISafe>();
            _searchAndAddOperationViewModel = new SearchAndAddOperationViewModel(_safe, (x) => { }, () =>
                {
                    _isRecordCreationRequested = true;
                });

            _searchResultVisibilityObserver = _searchAndAddOperationViewModel
                .GetPropertyObserver<bool>(nameof(_searchAndAddOperationViewModel.IsSearchResultVisible));

            _searchResultPropertyObserver = _searchAndAddOperationViewModel
                .GetPropertyObserver<ObservableCollection<RecordHeaderViewModel>>(
                    nameof(_searchAndAddOperationViewModel.SearchResults));

            _searchProgressIndicatorObserver = _searchAndAddOperationViewModel
                .GetPropertyObserver<bool>(nameof(_searchAndAddOperationViewModel.IsSearchInProgress));
        }

        [Test]
        public void When_add_command_is_made_then_new_record_creation_is_requested()
        {
            Assume.That(_searchAndAddOperationViewModel.AddCommand.CanExecute());
            _searchAndAddOperationViewModel.AddCommand.Execute();
            Assert.True(_isRecordCreationRequested);
        }

        public class Initially : SearchAndAddOperationViewModelTests
        {
            [Test]
            public void Search_result_is_not_visible()
            {
                Assert.AreEqual(false, _searchAndAddOperationViewModel.IsSearchResultVisible);
            }

            [Test]
            public void Search_text_is_empty()
            {
                Assert.AreEqual(string.Empty, _searchAndAddOperationViewModel.SearchText);
            }

            [Test]
            public void Search_in_progress_indicator_is_disabled()
            {
                Assert.False(_searchAndAddOperationViewModel.IsSearchInProgress);
            }
        }

        public class SearchTextEntered : SearchAndAddOperationViewModelTests
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

            public class Tests : SearchTextEntered
            {
                [Test]
                public void Seach_in_progress_indicator_is_enabled()
                {
                    Assert.True(_searchProgressIndicatorObserver.PropertyValue); 
                }
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