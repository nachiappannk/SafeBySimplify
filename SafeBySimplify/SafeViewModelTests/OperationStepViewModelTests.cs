using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Extensions;
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

        [SetUp]
        public void SetUp()
        {
            _safe = Substitute.For<ISafe>();
            _operationStepViewModel = new OperationStepViewModel(_safe, () => { });
        }

        [Test]
        public void Selected_result_is_initially_empty_operation_and_search_result_is_invisible()
        {
            Assert.AreEqual(typeof(EmptyOperationViewModel),_operationStepViewModel.SelectedOperation.GetType());
            Assert.AreEqual(false, _operationStepViewModel.IsSearchResultVisible);
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
            MockGetRecordAsync(_safe, searchText, 100, () => { semaphore.Release(); }, headers );
            var resultObserver = _operationStepViewModel.GetPropertyObserver<ObservableCollection<RecordHeaderViewModel>>(
                nameof(_operationStepViewModel.SearchResults));
            var resultVisibilityObserver = _operationStepViewModel
                .GetPropertyObserver<bool>(nameof(_operationStepViewModel.IsSearchResultVisible));
            _operationStepViewModel.SearchText = searchText;
            semaphore.WaitOne(10000);
            await Task.Run(() => { Thread.Sleep(100);});
            var actualHeaders = resultObserver.PropertyValue.Select(x => x.RecordHeader).ToList();
            CollectionAssert.AreEqual(headers, actualHeaders);
            Assert.AreEqual(true, resultVisibilityObserver.PropertyValue);
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
            MockGetRecordAsync(_safe, searchText, 100, () => { semaphore.Release(); }, headers);
            _operationStepViewModel.SearchText = searchText;
            semaphore.WaitOne(10000);
            await Task.Run(() => { Thread.Sleep(100); });
            Assume.That(_operationStepViewModel.SearchResults.Count == headers.Count);
            var selectedResultObserver = _operationStepViewModel.GetPropertyObserver<SingleOperationViewModel>
                (nameof(_operationStepViewModel.SelectedOperation));

            var searchResultVisibilityObserver = _operationStepViewModel.GetPropertyObserver<bool>
                (nameof(_operationStepViewModel.IsSearchResultVisible));

            var searchTextPropertyObserver = _operationStepViewModel.GetPropertyObserver<string>
                (nameof(_operationStepViewModel.SearchText));

            _operationStepViewModel.SearchResults.ElementAt(1).SelectCommand.Execute();
            Assert.AreEqual(typeof(RecordAlteringOperationViewModel),selectedResultObserver.PropertyValue.GetType());
            Assert.AreEqual(1,selectedResultObserver.NumberOfTimesPropertyChanged);


            Assert.False(searchResultVisibilityObserver.PropertyValue);
            Assert.True(searchResultVisibilityObserver.NumberOfTimesPropertyChanged >= 0);

            Assert.AreEqual(string.Empty, searchTextPropertyObserver.PropertyValue);

        }

        [Test]
        public async Task When_search_is_result_is_available_and_higlight_command_is_made_then_search_results_are_removed_and_search_text_is_removed()
        {
            Semaphore semaphore = new Semaphore(0, 1);
            var searchText = "ss";
            var headers = new List<RecordHeader>()
            {
                new RecordHeader() {Name = "1", Tags = new List<string>()},
                new RecordHeader() {Name = "2", Tags = new List<string>()}
            };
            MockGetRecordAsync(_safe, searchText, 100, () => { semaphore.Release(); }, headers);
            _operationStepViewModel.SearchText = searchText;
            semaphore.WaitOne(10000);
            await Task.Run(() => { Thread.Sleep(100); });
            Assume.That(_operationStepViewModel.SearchResults.Count == headers.Count);
            Assume.That(_operationStepViewModel.SelectedOperation.HighlightCommand.CanExecute());
            _operationStepViewModel.SelectedOperation.HighlightCommand.Execute();
            Assert.AreEqual(false,_operationStepViewModel.IsSearchResultVisible);
        }



        //Clicking of the SelectedResult should clear Search
        //Search Result should be closable
        //Search Result should disable Search
        //Add new result

        [Test]
        public void When_search_text_is_changed_then_a_new_search_query_is_made_and_the_old_is_cancelled()
        {
            var searchText = "ss";
            var updatedSearchText = "sss";

            var initialSearchCompleted = false;
            var secondSearchCompleted = false;

            var recordHeaders = new List<RecordHeader>();
            Semaphore semaphore = new Semaphore(0, 1);
            MockGetRecordAsync(_safe, searchText, 1000, () => { initialSearchCompleted = true; }, recordHeaders);
            MockGetRecordAsync(_safe, updatedSearchText, 1000, () =>
            {
                secondSearchCompleted = true;
                semaphore.Release();
            }, new List<RecordHeader>());
            

            _operationStepViewModel.SearchText = searchText;
            _operationStepViewModel.SearchText = updatedSearchText;
            semaphore.WaitOne(10000);
            Assert.AreEqual(false, initialSearchCompleted);
            Assert.AreEqual(true, secondSearchCompleted);


        }

        private void MockGetRecordAsync(ISafe safe, string searchText, int millisecondsTimeout, Action onTaskCompletionCallBack, List<RecordHeader> recordHeaders)
        {
            safe.GetRecordsAsync(searchText, Arg.Any<CancellationToken>())
                .Returns(
                    x =>
                    {
                        return Task.Run(() =>
                        {
                            ((CancellationToken)x[1]).ThrowIfCancellationRequested();
                            Thread.Sleep(millisecondsTimeout);
                            onTaskCompletionCallBack.Invoke();
                            return recordHeaders;
                        });
                    }
                );
        }

        [Test]
        public void IsOperationBarVisibile()
        {
            Assert.AreEqual(true,_operationStepViewModel.IsOperationsBarVisible);
        }
    }
}