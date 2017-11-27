using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.Extensions;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;

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
        public void When_searched_then_a_search_query_is_made()
        {
            var searchText = "ss";
            _operationStepViewModel.SearchText = searchText;
            _safe.Received().GetRecordsAsync(searchText, Arg.Any<CancellationToken>());
        }

        [Test]
        public void When_search_text_is_changed_then_a_new_search_query_is_made_and_the_old_is_cancelled()
        {
            var searchText = "ss";
            var updatedSearchText = "sss";

            var initialSearchCompleted = false;
            var secondSearchCompleted = false;
            _safe.GetRecordsAsync(searchText, Arg.Any<CancellationToken>())
                .Returns(
                    x =>
                    {
                        return Task.Run(() =>
                        {
                            ((CancellationToken)x[1]).ThrowIfCancellationRequested();
                            Thread.Sleep(1000);
                            initialSearchCompleted = true;
                            return new List<RecordHeader>();
                        });
                    }
                );
            _safe.GetRecordsAsync(updatedSearchText, Arg.Any<CancellationToken>())
                .Returns(
                    x =>
                    {
                        return Task.Run(() =>
                        {
                            ((CancellationToken)x[1]).ThrowIfCancellationRequested();
                            Thread.Sleep(1000);
                            secondSearchCompleted = true;
                            return new List<RecordHeader>();
                        });
                    }
                );

            _operationStepViewModel.SearchText = searchText;
            _operationStepViewModel.SearchText = updatedSearchText;
            Thread.Sleep(5000);
            Assert.AreEqual(false, initialSearchCompleted);


        }

        [Test]
        public void IsOperationBarVisibile()
        {
            Assert.AreEqual(true,_operationStepViewModel.IsOperationsBarVisible);
        }
    }
}