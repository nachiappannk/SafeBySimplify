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

namespace SafeBySimplify
{
    /// <summary>
    /// Interaction logic for SignUpUserControl.xaml
    /// </summary>
    public partial class SignUpUserControl : UserControl
    {
        public SignUpUserControl()
        {
            InitializeComponent();
            this.PasswordBox.PasswordChanged += PasswordChanged;
            this.ConfirmPasswordBox.PasswordChanged += ConfirmPasswordChanged;
        }

        

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var signUpViewModel = this.DataContext as SignUpViewModel;
            if (signUpViewModel == null) return;
            signUpViewModel.SignUpPassword = this.PasswordBox.Password;
        }

        private void ConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            var signUpViewModel = this.DataContext as SignUpViewModel;
            if (signUpViewModel == null) return;
            signUpViewModel.SignUpConfirmPassword = this.ConfirmPasswordBox.Password;
        }
    }
}
