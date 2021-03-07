using ModernWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace BiliWpf.Controls
{
    /// <summary>
    /// Interaction logic for WindowCaption.xaml
    /// </summary>
    public partial class WindowCaption : UserControl
    {
        public static Dictionary<Window, WindowCaption> Captions = new Dictionary<Window, WindowCaption>();

        private Window window;
        private bool _showBackground;
        public bool ShowBackground
        {
            get { return _showBackground; }
            set
            {
                if (value == _showBackground)
                    return;

                Back.BeginAnimation(OpacityProperty, new DoubleAnimation(
                    Back.Opacity,
                    value ? 1.0d : 0.0d,
                    new Duration(TimeSpan.FromMilliseconds(300))
                )
                { EasingFunction = new CubicEase() });

                _showBackground = value;
            }
        }

        public WindowCaption()
        {
            InitializeComponent();
            Background = new SolidColorBrush(Colors.Transparent);
        }

        private void DragArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if(window.WindowState == WindowState.Maximized)
                {
                    POINT point = GetMouse();
                    int mouseGoX = (int)(window.MinWidth * (point.X / SystemParameters.PrimaryScreenWidth));
                    int bWidth = point.X - mouseGoX;

                    SetMaximized(false);

                    window.Left = bWidth;
                    window.Top = 0;
                }
                window.DragMove();
            }
        }

        public void SetTargetWindow(Window window)
        {
            if(this.window != null)
            {
                return;
            }
            this.window = window;
            Captions.Add(window, this);

            // Use WindowProc as the callback method
            // to process all native window messages.
            //HwndSource.FromHwnd(handle)
            //.AddHook(MaximizedSizeFixWindowProc);

            ThemeManager.Current.ActualApplicationThemeChanged += delegate
            {
                SetColor();
            };
            SetColor();

            btnChromeMinimize.Click += delegate { window.WindowState = WindowState.Minimized; };
            btnChromeMaximize.Click += delegate 
            {
                SetMaximized(window.WindowState == WindowState.Normal);
            };
            btnChromeClose.Click += delegate { window.Close(); };

            btnChromeClose.MouseEnter += delegate 
            {
                btnChromeClose.Foreground.BeginAnimation(
                    SolidColorBrush.ColorProperty,
                    new ColorAnimation(
                        (Color)Application.Current.Resources["SystemBaseHighColor"],
                        (Color)Application.Current.Resources["SystemAltHighColor"],
                        new Duration(TimeSpan.FromMilliseconds(100))
                    )
                );
            };
            btnChromeClose.MouseLeave += delegate
            {
                btnChromeClose.Foreground.BeginAnimation(
                    SolidColorBrush.ColorProperty,
                    new ColorAnimation(
                        (Color)Application.Current.Resources["SystemAltHighColor"],
                        (Color)Application.Current.Resources["SystemBaseHighColor"],
                        new Duration(TimeSpan.FromMilliseconds(100))
                    )
                );
            };
        }

        public Window GetTargetWindow()
        {
            return window;
        }

        private void SetColor()
        {
            Color baseHighColor = (Color)Application.Current.Resources["SystemBaseHighColor"];
            btnChromeMinimize.Foreground = new SolidColorBrush(baseHighColor);
            btnChromeMaximize.Foreground = new SolidColorBrush(baseHighColor);
            btnChromeClose.Foreground = new SolidColorBrush(baseHighColor);
        }

        public void SetMaximized(bool isMaximized)
        {
            if(isMaximized)
            {
                window.WindowState = WindowState.Maximized;
                window.ResizeMode = ResizeMode.NoResize;
                window.BorderThickness = new Thickness(8);
            }
            else
            {
                window.WindowState = WindowState.Normal;
                window.ResizeMode = ResizeMode.CanResize;
                window.BorderThickness = new Thickness(0);
            }
        }

        public void SetMinimizeEnabled(bool enable)
        {
            this.btnChromeMinimize.IsEnabled = enable;
        }

        public void SetMaximizeEnabled(bool enable)
        {
            this.btnChromeMaximize.IsEnabled = enable;
        }

        public void SetCloseEnabled(bool enable)
        {
            this.btnChromeClose.IsEnabled = enable;
        }

        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        //e.GetPosition(this);
        //(e.Source as FrameworkElement).PointToScreen(new Point(0, 0));
        public static POINT GetMouse()
        {
            POINT mousestart = new POINT();
            GetCursorPos(out mousestart);

            return mousestart;
        }
    }
}
