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

using SafeModel;

namespace SafeViewModel
{
    public class WorkFlowViewModel : NotifiesPropertyChanged
    {
        private readonly ISafeProvider _safeProvider;

        private WorkFlowStepViewModel _currentStep;
        public WorkFlowStepViewModel CurrentStep
        {
            get { return _currentStep; }
            set
            {
                if (_currentStep != value)
                {
                    _currentStep = value;
                    _currentStep?.OnEntry();
                    FirePropertyChanged();
                }
            }
        }

        public WorkFlowViewModel(ISafeProvider safeProvider)
        {
            _safeProvider = safeProvider;
            GoToEntryStep();
        }

        private void GoToOperationStep(ISafe safe, string userName)
        {
            CurrentStep = new OperationStepViewModel(userName, safe, GoToEntryStep);
        }

        private void GoToSettingStep()
        {
            CurrentStep = new SettingsStepViewModel(_safeProvider, GoToEntryStep);
        }

        private void GoToEntryStep()
        {
            CurrentStep = new EntryStepViewModel(_safeProvider, GoToSettingStep, GoToOperationStep);
        }
    }
}