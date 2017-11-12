using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SafeUiLib
{
    
    public class HintingTextProvidingObject : DependencyObject
    {
        public static string GetHintText(DependencyObject obj)
        {
            return (string)obj.GetValue(HintTextProperty);
        }

        public static void SetHintText(DependencyObject obj, string value)
        {
            obj.SetValue(HintTextProperty, value);
        }

        public static readonly DependencyProperty HintTextProperty =
            DependencyProperty.RegisterAttached("HintText", typeof(string), typeof(HintingTextProvidingObject), new UIPropertyMetadata(string.Empty));

    }
}
