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

using System.Collections.ObjectModel;
using NSubstitute;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;
using SafeViewModelTests.TestTools;

namespace SafeViewModelTests
{
    [TestFixture]
    public partial class SearchAndAddOperationViewModelTests
    {
        private SearchAndAddOperationViewModel _searchAndAddOperationViewModel;
        private ISafe _safe;
        private bool _isRecordCreationRequested = false;
        private string _openedRecordId;
        private ViewModelPropertyObserver<bool> _searchResultVisibilityObserver;
        private ViewModelPropertyObserver<ObservableCollection<RecordHeaderViewModel>> _searchResultPropertyObserver;
        private ViewModelPropertyObserver<bool> _searchProgressIndicatorObserver;
        private ViewModelPropertyObserver<bool> _searchResultEmptyPropertyObserver;

        [SetUp]
        public void SetUp()
        {
            _safe = Substitute.For<ISafe>();
            _searchAndAddOperationViewModel = new SearchAndAddOperationViewModel(_safe, (x) => { _openedRecordId = x; }, () =>
                {
                    _isRecordCreationRequested = true;
                });

            _searchResultVisibilityObserver = _searchAndAddOperationViewModel
                .GetPropertyObserver<bool>(nameof(_searchAndAddOperationViewModel.IsSearchResultVisible));

            _searchResultEmptyPropertyObserver = _searchAndAddOperationViewModel
                .GetPropertyObserver<bool>(nameof(_searchAndAddOperationViewModel.IsSearchResultEmpty));

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
    }
}