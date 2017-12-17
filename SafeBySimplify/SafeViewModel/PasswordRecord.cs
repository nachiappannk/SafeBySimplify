using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeViewModel
{
    public class PasswordRecord
    {
        private readonly Action _changedAction;
        private string _name;
        private string _value;

        public PasswordRecord(Action changedAction)
        {
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
    }
}
