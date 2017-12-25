using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class FileRecordViewModel
    {
        private readonly IFileSafe _safe;
        private readonly Action _onChangeAction;
        private string _description;
        private string _name;


        public FileRecordViewModel(ObservableCollection<FileRecordViewModel> fileRecordViewModels, 
            IFileSafe safe, string recordId, string fileId, Action onChangeAction)
        {
            _safe = safe;
            _onChangeAction = onChangeAction;
            RecordId = recordId;
            FileRecordId = fileId;
            DeleteCommand = new DelegateCommand(() => fileRecordViewModels.Remove(this));
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                _onChangeAction.Invoke();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                _onChangeAction.Invoke();
            }
        }

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