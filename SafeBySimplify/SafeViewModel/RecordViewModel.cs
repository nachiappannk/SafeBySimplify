using System.Collections.Generic;
using Prism.Commands;

namespace SafeViewModel
{
    public class RecordViewModel
    {
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public List<PasswordRecord> PasswordRecords { get; set; }
        public List<FileRecord> FileRecords { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand DiscardChangesCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
    }
}