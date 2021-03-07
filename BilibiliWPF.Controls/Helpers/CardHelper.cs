using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BiliWpf.Controls.Helpers
{
    public class CardHelper
    {
        #region
        public static readonly DependencyProperty CardTitleProperty
            = DependencyProperty.RegisterAttached("CardTitle", typeof(string), typeof(CardHelper), new PropertyMetadata(""));

        public static string GetCardTitle(DependencyObject element) => (string)element.GetValue(CardTitleProperty);
        public static void SetCardTitle(DependencyObject element, string value) => element.SetValue(CardTitleProperty, value);
        #endregion

        #region
        public static readonly DependencyProperty CountProperty
            = DependencyProperty.RegisterAttached("Count", typeof(string), typeof(CardHelper), new PropertyMetadata(""));

        public static string GetCount(DependencyObject element) => (string)element.GetValue(CountProperty);
        public static void SetCount(DependencyObject element, string value) => element.SetValue(CountProperty, value);
        #endregion
    }
}
