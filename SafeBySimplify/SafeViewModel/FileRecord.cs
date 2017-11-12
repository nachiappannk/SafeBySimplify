using Prism.Commands;

namespace SafeViewModel
{
    public class FileRecord
    {
        public string Name { get; set; }
        public DelegateCommand DownloadCommand { get; set; }

        public DelegateCommand UploadCommand { get; set; }
    }
}