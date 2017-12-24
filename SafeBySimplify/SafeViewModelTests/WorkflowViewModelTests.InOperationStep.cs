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

using NSubstitute;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;

namespace SafeViewModelTests
{
    public partial class WorkflowViewModelTests
    {
        public class InOperationStep : WorkflowViewModelTests
        {
            private OperationStepViewModel _operationStepViewModel;

            [SetUp]
            public void InOperationStepSetUp()
            {
                const string validUserName = "SomeUserName";
                const string validPassword = "SomePassword";

                var safe = Substitute.For<ISafe>();
                _safeProvider.CreateSafeForNonExistingUser(validUserName, validPassword, validPassword).Returns(safe);

                _safeProvider.StubUserNameValidity(validUserName, true, string.Empty);
                _safeProvider.StubPasswordNameValidity(validPassword, true, string.Empty);
                MoveFromEntryStepToOperationStepBySigningUp(validUserName, validPassword);

                _operationStepViewModel = _workFlowViewModel.CurrentStep as OperationStepViewModel;

                Assume.That(_operationStepViewModel != null);
            }


            [Test]
            public void When_in_operation_screen_and_logged_out_then_app_returns_to_entry_screen()
            {
                Assert.AreEqual(true, _operationStepViewModel.SignOutCommand.CanExecute());
                _operationStepViewModel.SignOutCommand.Execute();
                Assert.IsNull(_operationStepViewModel.Safe);
                Assert.AreEqual(typeof(EntryStepViewModel), _currentStepProperyObserver.PropertyValue.GetType());

            }
        }
    }
}