using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Commands;

namespace SafeViewModel
{
    public class FileRecordViewModel
    {
        private readonly ObservableCollection<FileRecordViewModel> _fileRecordViewModels;

        public FileRecordViewModel(ObservableCollection<FileRecordViewModel> fileRecordViewModels)
        {
            _fileRecordViewModels = fileRecordViewModels;
            DeleteCommand = new DelegateCommand(() => fileRecordViewModels.Remove(this));
        }

        public string Name { get; set; }
        public string Extention { get; set; }
        public DelegateCommand DeleteCommand { get; set; }

        public void DownloadFileAs(string uri)
        {

        }
    }
}