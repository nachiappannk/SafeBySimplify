using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class FileRecordViewModel
    {
        private readonly IFileSafe _safe;
        

        public FileRecordViewModel(ObservableCollection<FileRecordViewModel> fileRecordViewModels, 
            IFileSafe safe, string recordId, string fileId)
        {
            _safe = safe;
            RecordId = recordId;
            FileRecordId = fileId;
            DeleteCommand = new DelegateCommand(() => fileRecordViewModels.Remove(this));
        }

        public string Description { get; set; }
        public string Name { get; set; }
        public string Extention { get; set; }
        public string FileRecordId { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public string RecordId { get; set; }

        public void DownloadFileAs(string uri)
        {
            _safe.RetreiveFile(RecordId, FileRecordId, uri);
        }
    }
}