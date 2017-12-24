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

using NUnit.Framework;
using SafeViewModel;

namespace SafeViewModelTests
{
    public partial class WorkflowViewModelTests
    {
        public class InSettingStep : WorkflowViewModelTests
        {
            [SetUp]
            public void InSettingStepSetUp()
            {
                MoveFromEntryStepToSettingStep();
            }

            [Test]
            public void When_in_settings_and_ok_command_is_made_then_the_app_moves_to_entry()
            {
                _currentStepProperyObserver.ResetObserver();
                AssumeInSettingStepAndThenGoToEntryStep();
                Assert.AreEqual(typeof(EntryStepViewModel), _currentStepProperyObserver.PropertyValue.GetType());
                Assert.AreNotEqual(0, _currentStepProperyObserver.NumberOfTimesPropertyChanged);
            }
        }
    }
}