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

using System;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class SettingsStepViewModel : WorkFlowStepViewModel
    {

        private readonly IHasWorkingDirectory _hasWorkingDirectory;
        private readonly Action _completedAction;
        private string _savedValueOfWorkingDirectory;


        private bool _isInSavedState = true;
        private bool IsInSavedState
        {
            get { return _isInSavedState; }
            set
            {
                if (_isInSavedState != value)
                {
                    _isInSavedState = value;
                    OkCommand.RaiseCanExecuteChanged();
                    SaveCommand.RaiseCanExecuteChanged();
                    DiscardCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand SaveCommand { get; set; }

        public DelegateCommand DiscardCommand { get; set; }

        public DelegateCommand OkCommand { get; set; }


        private string _workingDirectory;
        public string WorkSpaceDirectory
        {
            get { return _workingDirectory; }
            set
            {
                if(value != _workingDirectory)
                { 
                    _workingDirectory = value;
                    FirePropertyChanged();
                }
                IsInSavedState = _savedValueOfWorkingDirectory == _workingDirectory;
            }
        }

        public SettingsStepViewModel(IHasWorkingDirectory hasWorkingDirectory, Action completedAction)
        {
            _hasWorkingDirectory = hasWorkingDirectory;
            _completedAction = completedAction;
            SaveCommand = new DelegateCommand(Save, () => !IsInSavedState);
            DiscardCommand = new DelegateCommand(Discard, () => !IsInSavedState);
            OkCommand = new DelegateCommand(Ok, ()=> IsInSavedState);
            GoToSavedState();
        }

        private void Discard()
        {
            WorkSpaceDirectory = _savedValueOfWorkingDirectory;
            IsInSavedState = true;
        }

        private void Save()
        {
            if (!IsInSavedState) _hasWorkingDirectory.WorkingDirectory = WorkSpaceDirectory;
            _savedValueOfWorkingDirectory = WorkSpaceDirectory;
            IsInSavedState = true;
        }

        private void Ok()
        {
            _completedAction.Invoke();
        }
        
        private void GoToSavedState()
        {
            _savedValueOfWorkingDirectory = _hasWorkingDirectory.WorkingDirectory;
            WorkSpaceDirectory = _savedValueOfWorkingDirectory;
            IsInSavedState = true;
        }
    }
}