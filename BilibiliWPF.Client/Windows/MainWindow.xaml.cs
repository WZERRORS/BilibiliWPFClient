using BiliWpf.Client.UserControls;
using BiliWpf.Controls;
using BiliWpf.Services;
using ModernWpf;
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

namespace BiliWpf.Client.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Grid contentRoot;
        private Border backGround;
        private WindowCaption windowCaption;
        private PageRoot navigationRoot;

        private WindowAccentCompositor compositor;

        public MainWindow()
        {
            InitializeComponent();

            compositor = new WindowAccentCompositor(this);
            SetCompositorColor();

            //ThemeManager.ActualApplicationThemeChanged = (manager, obj) =>
            //{
            //    SetCompositorColor();
            //};

            if(BiliClient.Account.Current == null)
            {
                (new LoginWindow()).Show();
            }
        }

        public void SetCompositorColor()
        {
            var newTheme = ThemeManager.Current.ActualApplicationTheme;
            if (newTheme == ApplicationTheme.Light)
                compositor.Color = Color.FromArgb(192, 255, 255, 255);
            else
                compositor.Color = Color.FromArgb(192, 0, 0, 0);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.contentRoot = GetTemplateChild("contentRoot") as Grid;
            this.backGround = GetTemplateChild("backGroundBorderElement") as Border;
            this.navigationRoot = GetTemplateChild("navigationRoot") as PageRoot;
            (this.windowCaption = navigationRoot.WindowCaption).SetTargetWindow(this);

            SizeChanged += (sender, args) =>
            {
                var isMaximized = WindowState == WindowState.Maximized;
                contentRoot.Margin = isMaximized
                    ? new Thickness(8)
                    : new Thickness(0);
                compositor.IsEnabled = isMaximized;
                backGround.Visibility = isMaximized ? Visibility.Hidden : Visibility.Visible;
            };
        }

        private void Window_ActualThemeChanged(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                SetCompositorColor();
                if (compositor.IsEnabled && WindowState == WindowState.Maximized)
                {
                        compositor.OnIsEnabledChanged(true);
                }
            }
            
            
        }
    }
}
