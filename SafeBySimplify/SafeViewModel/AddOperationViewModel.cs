using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class AddOperationViewModel : SingleOperationViewModel
    {

        public AddOperationViewModel(Action discardAction, Action<string> saveAction)
        {
            Record = new RecordViewModel
            {
                Name = string.Empty,
                Tags = new List<string>(),
                FileRecords = new List<FileRecord>(),
            };

            Record.PropertyChanged += (sender, args) =>
            {
                CanExecuteSaveCommand = !string.IsNullOrWhiteSpace(Record.Name);
            };


            DiscardCommand = new DelegateCommand(discardAction);
            SaveCommand = new DelegateCommand(() =>
            {
                saveAction.Invoke(null);
            },() => CanExecuteSaveCommand );
        }

        private bool _canExecuteSaveCommand;
        private bool CanExecuteSaveCommand
        {
            get { return _canExecuteSaveCommand; }
            set
            {
                if(_canExecuteSaveCommand == value) return;
                _canExecuteSaveCommand = value;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public RecordViewModel Record { get; set; }
        public DelegateCommand DiscardCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
    }
}