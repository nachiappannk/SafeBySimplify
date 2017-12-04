using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
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
        private ISafe _safe;
        private ViewModelPropertyObserver<ObservableCollection<RecordHeaderViewModel>> _searchResultPropertyObserver;
        private ViewModelPropertyObserver<bool> _searchResultVisibilityObserver;
        private ViewModelPropertyObserver<SingleOperationViewModel> _selectedOperationPropertyObserver;
        private ViewModelPropertyObserver<string> _searchTextPropertyObserver;
        private ViewModelPropertyObserver<bool> _operationChangingPossibilityObserver;

        [SetUp]
        public void SetUp()
        {
            _safe = Substitute.For<ISafe>();
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

        [Test]
        public void Search_result_is_initially_not_available()
        {
            Assert.AreEqual(false, _operationStepViewModel.IsSearchResultVisible);

        }

        [Test]
        public void Selected_operation_is_initially_empty_operation()
        {
            Assert.AreEqual(typeof(EmptyOperationViewModel),_operationStepViewModel.SelectedOperation.GetType());
        }


        [Test]
        public async Task When_search_is_completed_the_search_results_are_updated()
        {
            Semaphore semaphore = new Semaphore(0,1);
            var searchText = "ss";
            var headers = new List<RecordHeader>()
            {
                new RecordHeader() {Name = "1", Tags = new List<string>()},
                new RecordHeader() {Name = "2", Tags = new List<string>()}
            };
            var asyncCompletionToken = MockGetRecordAsync(_safe, searchText, 100,  headers );
            SetSearchText(searchText);
            await asyncCompletionToken.WaitForTaskCompletion(10000);

            var actualHeaders = _searchResultPropertyObserver.PropertyValue.Select(x => x.RecordHeader).ToList();

            CollectionAssert.AreEqual(headers, actualHeaders);
            Assert.AreEqual(true, _searchResultVisibilityObserver.PropertyValue);
        }

        private void SetSearchText(string searchText)
        {
            _operationStepViewModel.SearchText = searchText;
        }

        [Test]
        public async Task When_search_is_result_is_selected_then_search_result_is_displayed_in_detail()
        {
            Semaphore semaphore = new Semaphore(0, 1);
            var searchText = "ss";
            var headers = new List<RecordHeader>()
            {
                new RecordHeader() {Name = "1", Tags = new List<string>()},
                new RecordHeader() {Name = "2", Tags = new List<string>()}
            };
            var asyncCompletionToken = MockGetRecordAsync(_safe, searchText, 100, headers);
            SetSearchText(searchText);
            
            await asyncCompletionToken.WaitForTaskCompletion(10000);

            Assume.That(_operationStepViewModel.SearchResults.Count == headers.Count);

            
            

            _operationStepViewModel.SearchResults.ElementAt(1).SelectCommand.Execute();

            Assert.AreEqual(typeof(RecordAlteringOperationViewModel),_selectedOperationPropertyObserver.PropertyValue.GetType());
            Assert.AreEqual(1,_selectedOperationPropertyObserver.NumberOfTimesPropertyChanged);


            Assert.False(_searchResultVisibilityObserver.PropertyValue);
            Assert.True(_searchResultVisibilityObserver.NumberOfTimesPropertyChanged >= 0);

            Assert.AreEqual(string.Empty, _searchTextPropertyObserver.PropertyValue);

        }

        [Test]
        public async Task When_search_is_result_is_available_and_higlight_command_is_made_then_search_results_are_removed_and_search_text_is_removed()
        {
            var searchText = "ss";
            var headers = new List<RecordHeader>()
            {
                new RecordHeader() {Name = "1", Tags = new List<string>()},
                new RecordHeader() {Name = "2", Tags = new List<string>()}
            };
            var searchText1 = String.Empty;
            var headers1 = new List<RecordHeader>()
            {
            };

            var asyncCompletionToken = MockGetRecordAsync(_safe, searchText, 100, headers);
            MockGetRecordAsync(_safe, searchText1, 0, headers1);
            _operationStepViewModel.SearchText = searchText;
            await asyncCompletionToken.WaitForTaskCompletion(10000);
            Assume.That(_operationStepViewModel.SearchResults.Count == headers.Count);
            Assume.That(_operationStepViewModel.SelectedOperation.HighlightCommand.CanExecute());
            _operationStepViewModel.SelectedOperation.HighlightCommand.Execute();
            Assert.False(_searchResultVisibilityObserver.PropertyValue);
            Assert.AreEqual(false,_operationStepViewModel.IsSearchResultVisible);
            Assert.AreEqual(string.Empty,_operationStepViewModel.SearchText);
        }


        [Test]
        public void When_add_command_is_made_then_selected_operation_is_add_operation()
        {
            Assume.That(_operationStepViewModel.AddCommand.CanExecute());
            _operationStepViewModel.AddCommand.Execute();
            Assert.AreEqual(typeof(AddOperationViewModel), _selectedOperationPropertyObserver.PropertyValue.GetType());
        }

        [Test]
        public async Task When_search_is_made_and_add_command_is_made_then_selected_operation_is_add_operation_and_search_result_is_invisible_and_search_text_is_false()
        {
            var searchText = "ss";
            var headers = new List<RecordHeader>()
            {
                new RecordHeader() {Name = "1", Tags = new List<string>()},
                new RecordHeader() {Name = "2", Tags = new List<string>()}
            };
            var asyncCompletionToken = MockGetRecordAsync(_safe, searchText, 100, headers);
            SetSearchText(searchText);
            await asyncCompletionToken.WaitForTaskCompletion(10000);

            Assume.That(_operationStepViewModel.AddCommand.CanExecute());
            _operationStepViewModel.AddCommand.Execute();
            Assert.AreEqual(typeof(AddOperationViewModel), _selectedOperationPropertyObserver.PropertyValue.GetType());
            Assert.False(_searchResultVisibilityObserver.PropertyValue);
            Assert.AreEqual(string.Empty,_searchTextPropertyObserver.PropertyValue);
        }

        [Test]
        public void When_add_command_is_made_and_discarded_then_selected_operation_is_empty_operation_and_operation_changing_is_possible()
        {
            Assume.That(_operationStepViewModel.AddCommand.CanExecute());
            _operationStepViewModel.AddCommand.Execute();
            var addOperationViewModel = _selectedOperationPropertyObserver.PropertyValue as AddOperationViewModel;
            Assume.That(addOperationViewModel != null);
            Assert.True(addOperationViewModel.DiscardCommand.CanExecute());
            addOperationViewModel.DiscardCommand.Execute();
            Assert.AreEqual(typeof(EmptyOperationViewModel),_selectedOperationPropertyObserver.PropertyValue.GetType());
            Assert.True(_operationChangingPossibilityObserver.PropertyValue);
        }

        [Test]
        public void When_add_command_is_made_and_saved_then_selected_operation_is_modification_operation_and_operation_changing_is_possible()
        {
            Assume.That(_operationStepViewModel.AddCommand.CanExecute());
            _operationStepViewModel.AddCommand.Execute();
            var addOperationViewModel = _selectedOperationPropertyObserver.PropertyValue as AddOperationViewModel;
            Assume.That(addOperationViewModel != null);
            Assert.True(addOperationViewModel.SaveCommand.CanExecute());
            addOperationViewModel.SaveCommand.Execute();
            Assert.AreEqual(typeof(RecordAlteringOperationViewModel), _selectedOperationPropertyObserver.PropertyValue.GetType());
            Assert.True(_operationChangingPossibilityObserver.PropertyValue);
        }




        [Test]
        public void When_add_command_is_made_then_operation_changing_is_disabled()
        {
            Assume.That(_operationStepViewModel.AddCommand.CanExecute());
            _operationStepViewModel.AddCommand.Execute();
            Assert.NotZero(_operationChangingPossibilityObserver.NumberOfTimesPropertyChanged);
            Assert.False(_operationChangingPossibilityObserver.PropertyValue);
        }

        //Clicking of the SelectedResult should clear Search
        //Search Result should be closable
        //Search Result should disable Search
        //Add new result

        [Test]
        public async Task When_search_text_is_changed_then_a_new_search_query_is_made_and_the_old_is_cancelled()
        {
            var searchText = "ss";
            var updatedSearchText = "sss";

            var asyncCompletionInitialToken = MockGetRecordAsync(_safe, searchText, 1000, new List<RecordHeader>());
            var asyncCompletionUpdatedToken = MockGetRecordAsync(_safe, updatedSearchText, 1000, new List<RecordHeader>());
            
            _operationStepViewModel.SearchText = searchText;
            _operationStepViewModel.SearchText = updatedSearchText;

            await asyncCompletionUpdatedToken.WaitForTaskCompletion(10000);
            Assert.False(asyncCompletionInitialToken.IsTaskCompleted);
        }

        private AsyncCompletionToken MockGetRecordAsync(ISafe safe, string searchText, int millisecondsTimeout, List<RecordHeader> recordHeaders)
        {
            var asyncCompeltionToken = new AsyncCompletionToken {Semaphore = new Semaphore(0, 1)};
            safe.GetRecordsAsync(searchText, Arg.Any<CancellationToken>())
                .Returns(
                    x =>
                    {
                        return Task.Run(() =>
                        {
                            ((CancellationToken)x[1]).ThrowIfCancellationRequested();
                            Thread.Sleep(millisecondsTimeout);
                            asyncCompeltionToken.IsTaskCompleted = true;
                            asyncCompeltionToken.Semaphore.Release();
                            return recordHeaders;
                        });
                    }
                );
            return asyncCompeltionToken;
        }

        public class AsyncCompletionToken
        {
            public Semaphore Semaphore { get; set; }

            public async Task WaitForTaskCompletion(int maxWaitTime)
            {
                var result = Semaphore.WaitOne(10000);
                await Task.Run(() => { Thread.Sleep(100); });
                if(!result) throw new Exception("The task was not completed");
            }

            public bool IsTaskCompleted { get; set; }
        }

        [Test]
        public void Operantion_modification_possibility()
        {
            Assert.AreEqual(true,_operationStepViewModel.IsOperationsChangingPossible);
        }
    }
}