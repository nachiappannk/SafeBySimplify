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
        private List<string> _searchedTexts = new List<string>();
        private ISafe _safe;
        private ViewModelPropertyObserver<SingleOperationViewModel> _selectedOperationPropertyObserver;

        [TearDown]
        public void TestDown()
        {
            Assert.False(_searchedTexts.Contains(string.Empty));
        }


        [SetUp]
        public void SetUp()
        {
            _safe = Substitute.For<ISafe>();

            _operationStepViewModel = new OperationStepViewModel(_safe, () => { });

            _selectedOperationPropertyObserver = _operationStepViewModel
                .GetPropertyObserver<SingleOperationViewModel>
                (nameof(_operationStepViewModel.SelectedOperation));



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
                new RecordHeader() {Name = "1", Tags = string.Empty},
                new RecordHeader() {Name = "2", Tags = string.Empty}
            },
            TimeTakenForSearching = 100,
        };



        public class SearchAndAddOperationIsTheSelectionOperation : OperationStepViewModelTests
        {
            private SearchAndAddOperationViewModel _serarchAndAddOperationViewModel;
            [SetUp]
            public void SearchAndAddOperationIsTheSelectionOPerationSetUp()
            {
                Assume.That(typeof(SearchAndAddOperationViewModel) == _operationStepViewModel.SelectedOperation.GetType());
                _serarchAndAddOperationViewModel =
                    _operationStepViewModel.SelectedOperation as SearchAndAddOperationViewModel;
            }

            public class SearchTextEnteredAndSearchResultsAreCorrect : SearchAndAddOperationIsTheSelectionOperation
            {
                private ViewModelPropertyObserver<bool> _searchResultVisibilityObserver;
                private ViewModelPropertyObserver<ObservableCollection<RecordHeaderViewModel>> _searchResultsPropertyObserver;

                [SetUp]
                public void SearchTextEnteredAndSearchResultsAreCorrectSetUp()
                {

                    _searchResultVisibilityObserver = _serarchAndAddOperationViewModel
                        .GetPropertyObserver<bool>(nameof(_serarchAndAddOperationViewModel.IsSearchResultVisible));

                    _searchResultsPropertyObserver = _serarchAndAddOperationViewModel
                        .GetPropertyObserver<ObservableCollection<RecordHeaderViewModel>>
                        (nameof(_serarchAndAddOperationViewModel.SearchResults));

                    _safe
                        .When(x => x.GetRecord(Arg.Any<string>()))
                        .Do(x => _searchedTexts.Add(x.ArgAt<string>(0)));

                    _safe.GetRecordHeaders(simpleSearchTextAndResult1.SearchText).Returns(x =>
                    {
                        Thread.Sleep(simpleSearchTextAndResult1.TimeTakenForSearching);
                        return simpleSearchTextAndResult1.SearchResults;
                    });

                    _serarchAndAddOperationViewModel.SearchText = simpleSearchTextAndResult1.SearchText;
                    _serarchAndAddOperationViewModel.TaskHolder.WaitOnHoldingTask();

                    Assume.That(_searchResultVisibilityObserver.PropertyValue, "The search results are invisible");
                    var actualHeaders = _searchResultsPropertyObserver.PropertyValue.Select(x => x.RecordHeader).ToList();
                    Assume.That(simpleSearchTextAndResult1.SearchResults.Count == actualHeaders.Count, "The search results are wrong (count)");
                    var isAllSearchResultsListed = actualHeaders.All(x => simpleSearchTextAndResult1.SearchResults.Contains(x));
                    Assume.That(isAllSearchResultsListed, "The search results are wrong");
                }

                [Test]
                public void When_search_result_is_selected_then_search_result_is_displayed_in_detail_and_search_text_is_reset()
                {
                    _serarchAndAddOperationViewModel.SearchResults.ElementAt(1).SelectCommand.Execute();

                    Assert.AreEqual(typeof(RecordAlteringOperationViewModel), _selectedOperationPropertyObserver.PropertyValue.GetType());
                    Assert.AreEqual(1, _selectedOperationPropertyObserver.NumberOfTimesPropertyChanged);
                    Assert.Inconclusive("The correct result is selected");


                }
                
                [Test]
                public void When_add_command_is_made_then_selected_operation_is_add_operation()
                {
                    Assume.That(_serarchAndAddOperationViewModel.AddCommand.CanExecute());
                    _serarchAndAddOperationViewModel.AddCommand.Execute();
                    Assert.AreEqual(typeof(AddOperationViewModel), _selectedOperationPropertyObserver.PropertyValue.GetType());
                }
            }

            public class InitiatedAddingARecord : SearchAndAddOperationIsTheSelectionOperation
            {
                private AddOperationViewModel _addOperationViewModel;

                [SetUp]
                public void InitiatedAddingARecordSetUp()
                {
                    Assume.That(_serarchAndAddOperationViewModel.AddCommand.CanExecute());
                    _serarchAndAddOperationViewModel.AddCommand.Execute();
                    Assume.That(typeof(AddOperationViewModel) == _selectedOperationPropertyObserver.PropertyValue.GetType());
                    _addOperationViewModel = _operationStepViewModel.SelectedOperation as AddOperationViewModel;
                    Assume.That(_addOperationViewModel != null);
                }

                [Test]
                public void When_record_is_discarded_then_selected_operation_is_search_and_add_operation()
                {
                    Assert.True(_addOperationViewModel.DiscardCommand.CanExecute());
                    _addOperationViewModel.DiscardCommand.Execute();
                    Assert.AreEqual(typeof(SearchAndAddOperationViewModel), _selectedOperationPropertyObserver.PropertyValue.GetType());
                }

                [Test]
                public void When_record_is_entered_and_saved_then_selected_operation_is_modification_operation()
                {
                    _addOperationViewModel.Record.Name = "SomeName";
                    Assert.True(_addOperationViewModel.SaveCommand.CanExecute());
                    _addOperationViewModel.SaveCommand.Execute();
                    Assert.AreEqual(typeof(RecordAlteringOperationViewModel), _selectedOperationPropertyObserver.PropertyValue.GetType());
                    Assert.Inconclusive("The Correct record is in the record altering operation view model");
                }
            }

        }





        




        //Clicking of the SelectedResult should clear Search
        //Search Result should be closable
        //Search Result should disable Search
        //Add new result





    }
}