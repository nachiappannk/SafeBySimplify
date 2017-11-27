using System.Windows;
using SafeBySimplify.BootStrapper;

namespace SafeBySimplify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = ViewModelFactory.CreateWorkFlowViewModelWithTestSetting();
            //this.DataContext = ViewModelFactory.CreateWorkFlowViewModel();
        }
    }
}
