using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace SafeViewModel
{
    public class PasswordRecordViewModel
    {
        private readonly Action _changedAction;
        private string _name;
        private string _value;

        public PasswordRecordViewModel(Action changedAction, 
            Action<PasswordRecordViewModel> removeAction, 
            Predicate<PasswordRecordViewModel> isRemovePossible)
        {
            RemoveCommand = new DelegateCommand(() => removeAction.Invoke(this), () => isRemovePossible.Invoke(this));
            _changedAction = changedAction;
            _name = string.Empty;
            _value = string.Empty;
        }

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
