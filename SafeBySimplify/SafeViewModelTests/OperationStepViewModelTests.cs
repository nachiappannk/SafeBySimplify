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
        public void Selected_result_is_initially_null()
        {
            Assert.Null(_operationStepViewModel.SelectedResult);
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
            _operationStepViewModel.SearchText = searchText;
            semaphore.WaitOne(10000);
            await Task.Run(() => { Thread.Sleep(100);});
            var actualHeaders = resultObserver.PropertyValue.Select(x => x.RecordHeader).ToList();
            CollectionAssert.AreEqual(headers, actualHeaders);
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
            var selectedResultObserver = _operationStepViewModel.GetPropertyObserver<ResultViewModel>
                (nameof(_operationStepViewModel.SelectedResult));
            _operationStepViewModel.SearchResults.ElementAt(1).SelectCommand.Execute();
            Assert.NotNull(selectedResultObserver.PropertyValue);
            Assert.AreEqual(1,selectedResultObserver.NumberOfTimesPropertyChanged);
        }



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