using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace SafeViewModel
{
    public class PasswordRecordViewModel
    {
        private readonly Action _changedAction;
        private readonly ObservableCollection<PasswordRecordViewModel> _passwordRecordViewModels;

        public PasswordRecordViewModel(Action changedAction, ObservableCollection<PasswordRecordViewModel> passwordRecordViewModels)
        {
            _passwordRecordViewModels = passwordRecordViewModels;
            _passwordRecordViewModels.CollectionChanged += CollectionModified;
            RemoveCommand = new DelegateCommand(Remove, IsRemovePossible);
            _changedAction = changedAction;
        }

        private void CollectionModified(object sender, NotifyCollectionChangedEventArgs e)
        {
            RemoveCommand.RaiseCanExecuteChanged();
        }

        private void Remove()
        {
            _passwordRecordViewModels.CollectionChanged -= CollectionModified;
            _passwordRecordViewModels.Remove(this);
        }

        private bool IsRemovePossible()
        {
            var lastItem = _passwordRecordViewModels.Last();
            return this != lastItem;
        }

        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                _changedAction.Invoke();
            }
        }

        private string _value = string.Empty;
        public string Value
        {
            get { return _value; }
            set
            {
                if(_value == value) return;
                _value = value;
                _changedAction.Invoke();
            }
        }

        public DelegateCommand RemoveCommand { get; set; }
    }
}
