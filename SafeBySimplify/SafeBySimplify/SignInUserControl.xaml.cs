﻿using System;
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
    /// Interaction logic for SignInUserControl.xaml
    /// </summary>
    public partial class SignInUserControl : UserControl
    {
        public SignInUserControl()
        {
            InitializeComponent();
            PasswordBox.PasswordChanged += PasswordChanged;
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var signInViewModel = this.DataContext as SignInViewModel;
            if(signInViewModel == null)return;
            signInViewModel.SignInPassword = PasswordBox.Password;
        }
    }
}
