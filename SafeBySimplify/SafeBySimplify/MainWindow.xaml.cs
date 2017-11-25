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
using SafeViewModel;
using SafeModel;

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
            this.DataContext = new WorkFlowViewModel(new SafeProvider(null));
            //Test.DataContext = new SignUpViewModel(new SafeProvider(null), new HasSafe(), () => { });
            //Test.DataContext = new SignInViewModel(new SafeProvider(null), new HasSafe(), () => { });
        }

        public class HasSafe : IHasSafe
        {
            public ISafe Safe { get; set; }
        }

    }
}
