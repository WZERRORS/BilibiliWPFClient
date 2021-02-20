using BiliWpf.Client.UserControls;
using BiliWpf.Controls;
using ModernWpf.Controls.Primitives;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BiliWpf.Client.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        private WindowCaption windowCaption;

        public LoginWindow()
        {
            InitializeComponent();

            //AppCenter.Client.QrCodeOutdated += delegate
            //{
            //    qrcodeOutdatedFilter.Visibility = Visibility.Visible;
            //};

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            windowCaption = (GetTemplateChild("component") as LoginComponent).windowCaption;

            windowCaption.SetTargetWindow(this);
            windowCaption.SetMinimizeEnabled(false);
            windowCaption.SetMaximizeEnabled(false);
        }

        

    }
}
