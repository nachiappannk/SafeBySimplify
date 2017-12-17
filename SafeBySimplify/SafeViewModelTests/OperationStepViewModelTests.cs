using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    [TestFixture]
    public class OperationStepViewModelTests
    {
        private OperationStepViewModel _operationStepViewModel;
        private MockedSafe _safe;
        private ViewModelPropertyObserver<ObservableCollection<RecordHeaderViewModel>> _searchResultPropertyObserver;
        private ViewModelPropertyObserver<bool> _searchResultVisibilityObserver;
        private ViewModelPropertyObserver<SingleOperationViewModel> _selectedOperationPropertyObserver;
        private ViewModelPropertyObserver<string> _searchTextPropertyObserver;
        private ViewModelPropertyObserver<bool> _operationChangingPossibilityObserver;

        [TearDown]
        public void TestDown()
        {
            Assert.False(_safe.SearchedTexts.Contains(string.Empty));
        }


        public class MockedSafe : ISafe
        {
            Dictionary<string, List<RecordHeader>> _resultDictionary = new Dictionary<string, List<RecordHeader>>();
            Dictionary<string, int> _searchTimeDictionary = new Dictionary<string, int>();

            public void MockGetRecordsAsync(string searchText, List<RecordHeader> result, int searchTime)
            {
                _resultDictionary.Add(searchText, result);
                _searchTimeDictionary.Add(searchText, searchTime);
            }
            
            public List<RecordHeader> GetRecordHeaders(string searchText)
            {
                SearchedTexts.Add(searchText);
                var millisecondsTimeout = _searchTimeDictionary[searchText];
                //for (int i = 0; i < millisecondsTimeout; i++)
                //{
                //    for (int j = 0; j < 100000; j++)
                //    {

                //    }
                //}
                Thread.Sleep(millisecondsTimeout);
                return _resultDictionary[searchText];
            }

            public List<string> SearchedTexts => new List<string>();
        }


        [SetUp]
        public void SetUp()
        {
            _safe = new MockedSafe();

            _operationStepViewModel = new OperationStepViewModel(_safe, () => { });

            _searchResultPropertyObserver = _operationStepViewModel
                .GetPropertyObserver<ObservableCollection<RecordHeaderViewModel>>
                (nameof(_operationStepViewModel.SearchResults));

            _searchResultVisibilityObserver = _operationStepViewModel
                .GetPropertyObserver<bool>(nameof(_operationStepViewModel.IsSearchResultVisible));

            _selectedOperationPropertyObserver = _operationStepViewModel
                .GetPropertyObserver<SingleOperationViewModel>
                (nameof(_operationStepViewModel.SelectedOperation));


            _searchTextPropertyObserver = _operationStepViewModel.GetPropertyObserver<string>
                (nameof(_operationStepViewModel.SearchText));

            _operationChangingPossibilityObserver = _operationStepViewModel
                .GetPropertyObserver<bool>
                (nameof(_operationStepViewModel.IsOperationsChangingPossible));
        }

        public class Tests : OperationStepViewModelTests
        {
            [Test]
            public void Search_result_is_initially_not_available()
            {
                Assert.AreEqual(false, _operationStepViewModel.IsSearchResultVisible);
            }

            [Test]
            public void Selected_operation_is_initially_empty_operation()
            {
                Assert.AreEqual(typeof(EmptyOperationViewModel), _operationStepViewModel.SelectedOperation.GetType());
            }

            [Test]
            public void Initialy_selected_operation_modification_is_possible()
            {
                Assert.AreEqual(true, _operationStepViewModel.IsOperationsChangingPossible);
            }
        }


 


        public class SearchTextAndSearchResult
        {
            public string SearchText { get; set; }
            public List<RecordHeader> SearchResults { get; set; }

            public int TimeTakenForSearching { get; set; }
        }


        private static SearchTextAndSearchResult simpleSearchTextAndResult1 = new SearchTextAndSearchResult
        {
            SearchText = "ss",
            SearchResults = new List<RecordHeader>()
            {
                new RecordHeader() {Name = "1", Tags = new List<string>()},
                new RecordHeader() {Name = "2", Tags = new List<string>()}
            },
            TimeTakenForSearching = 100,
        };

        


        private void SetSearchText(string searchText)
        {
            _operationStepViewModel.SearchText = searchText;
        }


        public class SearchTextEnteredAndSearchResultsAreCorrect : OperationStepViewModelTests
        {
            [SetUp]
            public void SearchTextEnteredAndSearchResultsAreCorrectSetUp()
            {
                _safe.MockGetRecordsAsync(simpleSearchTextAndResult1.SearchText, 
                    simpleSearchTextAndResult1.SearchResults, simpleSearchTextAndResult1.TimeTakenForSearching);
                SetSearchText(simpleSearchTextAndResult1.SearchText);
                _operationStepViewModel.TaskHolder.WaitOnHoldingTask();
                Assume.That(_searchResultVisibilityObserver.PropertyValue, "The search results are invisible");
                var actualHeaders = _searchResultPropertyObserver.PropertyValue.Select(x => x.RecordHeader).ToList();
                Assume.That(simpleSearchTextAndResult1.SearchResults.Count == actualHeaders.Count, "The search results are wrong (count)");
                var isAllSearchResultsListed =  actualHeaders.All(x => simpleSearchTextAndResult1.SearchResults.Contains(x));
                Assume.That(isAllSearchResultsListed,"The search results are wrong");
            }

            



            [Test]
            public void When_search_result_is_selected_then_search_result_is_displayed_in_detail_and_search_text_is_reset()
            {
                _operationStepViewModel.SearchResults.ElementAt(1).SelectCommand.Execute();

                Assert.AreEqual(typeof(RecordAlteringOperationViewModel), _selectedOperationPropertyObserver.PropertyValue.GetType());
                Assert.AreEqual(1, _selectedOperationPropertyObserver.NumberOfTimesPropertyChanged);


                Assert.False(_searchResultVisibilityObserver.PropertyValue);
                Assert.True(_searchResultVisibilityObserver.NumberOfTimesPropertyChanged >= 0);

                Assert.AreEqual(string.Empty, _searchTextPropertyObserver.PropertyValue);

            }

            [Test]
            public void When_search_is_result_is_available_and_higlight_command_is_made_then_search_results_are_removed_and_search_text_is_removed()
            {
                Assume.That(_operationStepViewModel.SelectedOperation.HighlightCommand.CanExecute());
                _operationStepViewModel.SelectedOperation.HighlightCommand.Execute();
                Assert.False(_searchResultVisibilityObserver.PropertyValue);
                Assert.AreEqual(false, _operationStepViewModel.IsSearchResultVisible);
                Assert.AreEqual(string.Empty, _operationStepViewModel.SearchText);
            }


            [Test]
            public void When_search_is_made_and_add_command_is_made_then_selected_operation_is_add_operation_and_search_result_is_invisible_and_search_text_is_false()
            {
                Assume.That(_operationStepViewModel.AddCommand.CanExecute());
                _operationStepViewModel.AddCommand.Execute();
                Assert.AreEqual(typeof(AddOperationViewModel), _selectedOperationPropertyObserver.PropertyValue.GetType());
                Assert.False(_searchResultVisibilityObserver.PropertyValue);
                Assert.AreEqual(string.Empty, _searchTextPropertyObserver.PropertyValue);
            }
        }


        public class InitiatedAddingARecord : OperationStepViewModelTests
        {
            private AddOperationViewModel _addOperationViewModel;

            [SetUp]
            public void InitiatedAddingARecordSetUp()
            {
                Assume.That(_operationStepViewModel.AddCommand.CanExecute());
                _operationStepViewModel.AddCommand.Execute();
                Assume.That(typeof(AddOperationViewModel) == _selectedOperationPropertyObserver.PropertyValue.GetType());
                _addOperationViewModel = _operationStepViewModel.SelectedOperation as AddOperationViewModel;
                Assume.That(_addOperationViewModel != null);
            }

            [Test]
            public void When_record_is_discarded_then_selected_operation_is_empty_operation_and_operation_changing_is_possible()
            {
                Assert.True(_addOperationViewModel.DiscardCommand.CanExecute());
                _addOperationViewModel.DiscardCommand.Execute();
                Assert.AreEqual(typeof(EmptyOperationViewModel), _selectedOperationPropertyObserver.PropertyValue.GetType());
                Assert.True(_operationChangingPossibilityObserver.PropertyValue);
            }

            [Test]
            public void When_add_command_is_made_and_saved_then_selected_operation_is_modification_operation_and_operation_changing_is_possible()
            {
                Assert.True(_addOperationViewModel.SaveCommand.CanExecute());
                _addOperationViewModel.SaveCommand.Execute();
                Assert.AreEqual(typeof(RecordAlteringOperationViewModel), _selectedOperationPropertyObserver.PropertyValue.GetType());
                Assert.True(_operationChangingPossibilityObserver.PropertyValue);
            }




            [Test]
            public void When_add_command_is_made_then_operation_changing_is_disabled()
            {
                Assert.NotZero(_operationChangingPossibilityObserver.NumberOfTimesPropertyChanged);
                Assert.False(_operationChangingPossibilityObserver.PropertyValue);
            }


        }



        //Clicking of the SelectedResult should clear Search
        //Search Result should be closable
        //Search Result should disable Search
        //Add new result



        

    }
}