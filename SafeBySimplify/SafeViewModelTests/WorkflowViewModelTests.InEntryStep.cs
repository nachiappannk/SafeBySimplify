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

using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;

namespace SafeViewModelTests
{
    public partial class WorkflowViewModelTests
    {
        public class InEntryStep : WorkflowViewModelTests
        {
            [SetUp]
            public void InEntryStepSetUp()
            {
                Assume.That(typeof(EntryStepViewModel) == _workFlowViewModel.CurrentStep.GetType());
            }
            
            [Test]
            public void When_go_to_settings_command_is_executed_then_app_moves_to_settings()
            {
                MoveFromEntryStepToSettingStep();
                Assert.AreEqual(typeof(SettingsStepViewModel), _currentStepProperyObserver.PropertyValue.GetType());
                Assert.AreNotEqual(0, _currentStepProperyObserver.NumberOfTimesPropertyChanged);
            }

            [Test]
            public void When_signed_up_then_app_moves_to_operation_step_with_correct_username()
            {
                const string validUserName = "SomeUserName";
                const string validPassword = "SomePassword";

                var safe = Substitute.For<ISafe>();
                _safeProvider.CreateSafeForNonExistingUser(validUserName, validPassword, validPassword).Returns(safe);

                _safeProvider.StubUserNameValidity(validUserName, true, string.Empty);
                _safeProvider.StubPasswordNameValidity(validPassword, true, string.Empty);

                MoveFromEntryStepToOperationStepBySigningUp(validUserName, validPassword);
                Assert.AreEqual(typeof(OperationStepViewModel), _currentStepProperyObserver.PropertyValue.GetType());
                var operationStepViewModel = _currentStepProperyObserver.PropertyValue as OperationStepViewModel;
                Assert.AreEqual(validUserName, operationStepViewModel.UserName);
                // ReSharper disable once PossibleNullReferenceException
                Assert.AreEqual(safe, operationStepViewModel.Safe);
            }


            [Test]
            public void When_signed_in_with_correct_detail_then_operation_step_is_initialized_and_app_moves_operation_step_with_correct_username()
            {
                const string validPassword = "SomePassword";
                string validUserName = _validUserNames.ElementAt(0);

                var safe = Substitute.For<ISafe>();
                _safeProvider.StubCreateSafeForExistingUser(validUserName, validPassword, safe);

                MoveFromEntryStepToOperationStepBySigningIn(validUserName, validPassword);
                Assert.AreEqual(typeof(OperationStepViewModel), _currentStepProperyObserver.PropertyValue.GetType());
                var operationStepViewModel = _currentStepProperyObserver.PropertyValue as OperationStepViewModel;
                Assert.AreEqual(validUserName, operationStepViewModel.UserName);
                // ReSharper disable once PossibleNullReferenceException
                Assert.AreEqual(safe, operationStepViewModel.Safe);
            }

        }
    }
}