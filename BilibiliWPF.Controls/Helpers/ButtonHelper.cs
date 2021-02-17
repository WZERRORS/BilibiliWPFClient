using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BiliWpf.Controls.Helpers
{
    public class ButtonHelper
    {
        private static readonly StreamGeometry DefaultCaptionSymbol = null;
        private static readonly string DefaultFontIconPath = null;
        private static readonly SolidColorBrush DefaultHoverColor = new SolidColorBrush(Color.FromArgb(25, 0, 0, 0));

        #region AttachedProperty : CornerRadiusProperty
        /// <summary>
        /// Controls the corner radius of the surrounding box.
        /// </summary>
        public static readonly DependencyProperty CaptionSymbolProperty
            = DependencyProperty.RegisterAttached("CaptionSymbol", typeof(StreamGeometry), typeof(ButtonHelper), new PropertyMetadata(DefaultCaptionSymbol));

        public static StreamGeometry GetCaptionSymbol(DependencyObject element) => (StreamGeometry)element.GetValue(CaptionSymbolProperty);
        public static void SetCaptionSymbol(DependencyObject element, StreamGeometry value) => element.SetValue(CaptionSymbolProperty, value);
        #endregion

        #region
        public static readonly DependencyProperty MouseHoverColorProperty
            = DependencyProperty.RegisterAttached("MouseHoverColor", typeof(SolidColorBrush), typeof(ButtonHelper), new PropertyMetadata(DefaultHoverColor));

        public static SolidColorBrush GetMouseHoverColor(DependencyObject element) => (SolidColorBrush)element.GetValue(MouseHoverColorProperty);
        public static void SetMouseHoverColor(DependencyObject element, SolidColorBrush value) => element.SetValue(MouseHoverColorProperty, value);
        #endregion

        #region
        public static readonly DependencyProperty IconPathProperty
            = DependencyProperty.RegisterAttached("IconPath", typeof(string), typeof(ButtonHelper), new PropertyMetadata(DefaultFontIconPath));

        public static string GetIconPath(DependencyObject element) => (string)element.GetValue(IconPathProperty);
        public static void SetIconPath(DependencyObject element, string value) => element.SetValue(IconPathProperty, value);
        #endregion
    }
}
