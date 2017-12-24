using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class FileRecordViewModel
    {
        private readonly ObservableCollection<FileRecordViewModel> _fileRecordViewModels;
        private readonly ISafe _safe;
        private readonly string _recordId;

        public FileRecordViewModel(ObservableCollection<FileRecordViewModel> fileRecordViewModels, 
            ISafe safe, string recordId)
        {
            _fileRecordViewModels = fileRecordViewModels;
            _safe = safe;
            _recordId = recordId;
            DeleteCommand = new DelegateCommand(() => fileRecordViewModels.Remove(this));
        }

        public string Name { get; set; }
        public string Extention { get; set; }
        public string FileRecordId { get; set; }
        public DelegateCommand DeleteCommand { get; set; }

        public void DownloadFileAs(string uri)
        {
            _safe.RetreiveFile(_recordId, FileRecordId, uri);
        }
    }
}