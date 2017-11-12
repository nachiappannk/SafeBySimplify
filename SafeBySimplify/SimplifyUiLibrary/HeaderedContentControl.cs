using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SimplifyUiLibrary
{
    public class HeaderedContentControl : ContentControl
    {
        public static readonly DependencyProperty HeadingProperty = DependencyProperty.Register(
            "Heading", typeof(string), typeof(HeaderedContentControl), new PropertyMetadata(default(string)));

        public string Heading
        {
            get { return (string) GetValue(HeadingProperty); }
            set { SetValue(HeadingProperty, value); }
        }
    }
}
