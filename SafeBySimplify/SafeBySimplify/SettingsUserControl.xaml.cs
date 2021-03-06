﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserControl = System.Windows.Controls.UserControl;

namespace SafeBySimplify
{
    /// <summary>
    /// Interaction logic for SettingsUserControl.xaml
    /// </summary>
    public partial class SettingsUserControl : UserControl
    {
        public SettingsUserControl()
        {
            InitializeComponent();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine();
            sb.AppendLine("If you have a feature request, feel free to mail it to nachiapan@gmail.com");
            sb.AppendLine("Alternatively you can register it at the below url");
            sb.AppendLine();
        }

        private void FolderSelectionButtonClicked(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Choose the working directory";
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK) FolderTextBox.Text = dialog.SelectedPath;
            }
        }
    }
}
