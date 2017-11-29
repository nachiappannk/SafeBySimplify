using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prism.Commands;
using SafeViewModel;

namespace SafeBySimplify
{
    /// <summary>
    /// Interaction logic for SafeUserControl.xaml
    /// </summary>
    public partial class OperationUserControl : UserControl
    {
        public OperationUserControl()
        {
            InitializeComponent();
            var x = new RecordViewModel();
            x.Name = "SBI";
            x.PasswordRecords = new List<PasswordRecord>()
            {
                new PasswordRecord() {Name = "ss", Value = "yy"}
            };
            x.FileRecords = new List<FileRecord>()
            {
                new FileRecord()
                {
                    Name = "Somve File",
                    DownloadCommand = new DelegateCommand(Delete),
                    UploadCommand = new DelegateCommand(Upload),
                    
                }
            };
        }

        public void Delete()
        {

        }

        public void Upload()
        {

        }
    }
}
