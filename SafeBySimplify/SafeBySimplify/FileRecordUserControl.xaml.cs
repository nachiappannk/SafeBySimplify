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
using Microsoft.Win32;
using SafeViewModel;

namespace SafeBySimplify
{
    /// <summary>
    /// Interaction logic for FileRecordUserControl.xaml
    /// </summary>
    public partial class FileRecordUserControl : UserControl
    {
        public FileRecordUserControl()
        {
            InitializeComponent();
        }

        private void Download(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as FileRecordViewModel;
            if(viewModel == null) throw new Exception();

            var extention = viewModel.Extention;

            var filter = extention + "|*." + extention;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = filter;
            saveFileDialog.Title = "Save file";
            saveFileDialog.FileName = viewModel.Name;
            var result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                var resultFile = saveFileDialog.FileName;
                viewModel.DownloadFileAs(resultFile);
            }
        }
    }
}
