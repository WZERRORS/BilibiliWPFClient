using BiliWpf.Client.Pages;
using BiliWpf.Services;
using ModernWpf.Controls.Primitives;
using ModernWpf.Media.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace BiliWpf.Client.UserControls
{
    public partial class PageRoot
    {
        private bool _isExpanded;
        private DispatcherTimer expandWaiter;

        public bool IsConstant { get; set; }
        public bool IsExpanded
        {
            get
            {
                return _isExpanded || IsConstant;
            }
            set
            {
                if (IsConstant)
                    return;
                _isExpanded = value;

                BeginAnimation_NavigationViewExpand();
            }
        }

        private void NavigationViewMenu_Click(object sender, RoutedEventArgs args)
        {
            if (expandWaiter != null)
            {
                expandWaiter.Stop();
                expandWaiter = null;
            }

            IsExpanded = !IsExpanded;
        }

        private void NavigationView_MouseEnter(object sender, MouseEventArgs e)
        {
            expandWaiter = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(650) };
            expandWaiter.Tick += (sender, args) =>
            {
                IsExpanded = true;

                expandWaiter.Stop();
                expandWaiter = null;
            };
            expandWaiter.Start();
        }

        private void NavigationView_MouseLeave(object sender, MouseEventArgs e)
        {
            if (expandWaiter != null)
            {
                expandWaiter.Stop();
                expandWaiter = null;
            }

            IsExpanded = false;
        }

        private void AccountDetail_Click(object sender, RoutedEventArgs e)
        {
            if (BiliClient.Account.Me == null)
                return;

            contentFrame.Navigate(new AccountPage(), null, new DrillInNavigationTransitionInfo());
        }

        private void BeginAnimation_NavigationViewExpand()
        {
            if (IsConstant)
            {
                return;
            }

            var isMaximized = Application.Current.MainWindow.WindowState == WindowState.Maximized;
            Duration duration = new Duration(TimeSpan.FromMilliseconds(isMaximized ? 350 : 240));
            navigationViewRoot.BeginAnimation(
                WidthProperty,
                new DoubleAnimation(
                    navigationViewRoot.Width,
                    IsExpanded ? (isMaximized ? 350 : 230) : 48,
                    duration
                )
                {
                    DecelerationRatio = 0.3d,
                    EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseOut }
                }
            );
            navigationViewShadowChrome.BeginAnimation(
                ThemeShadowChrome.DepthProperty,
                new DoubleAnimation(
                    navigationViewShadowChrome.Depth,
                    IsExpanded ? 16 : 0,
                    duration
                )
                {
                    AccelerationRatio = 0.75d,
                    EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
                }
            );
            imageUserFace.BeginAnimation(
                WidthProperty,
                new DoubleAnimation(
                    imageUserFace.Width,
                    IsExpanded ? 60 : 32,
                    duration
                )
                {
                    AccelerationRatio = 0.75d,
                    EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseOut }
                }
            );
            imageUserFace.BeginAnimation(
                MarginProperty,
                new ThicknessAnimation(
                    imageUserFace.Margin,
                    new Thickness(6, IsExpanded ? 35 : 15, 6, 6),
                    duration
                )
                {
                    AccelerationRatio = 0.5d,
                    EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
                }
            );
        }
    }
}
