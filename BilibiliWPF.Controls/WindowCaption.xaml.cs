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
        private Window window;

        public WindowCaption()
        {
            InitializeComponent();
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
