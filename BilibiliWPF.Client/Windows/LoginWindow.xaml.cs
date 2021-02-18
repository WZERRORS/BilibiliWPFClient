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
        private Grid rootGrid;
        private WindowCaption windowCaption;
        private Grid actionContent;
        private Grid qrcodeContent;
        private Grid contentInputAccount;
        private Grid contentInputPass;
        private TextBox txtBoxAcc;
        private PasswordBox passBoxPswd;
        private Button useQRCode;
        private Button useAccount;
        private Image qrcodePresenter;
        private Border qrcodeOutdatedFilter;

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

            rootGrid = GetTemplateChild("rootGrid") as Grid;
            windowCaption = GetTemplateChild("windowCaption") as WindowCaption;
            actionContent = GetTemplateChild("actionContent") as Grid;
            qrcodeContent = GetTemplateChild("qrcodeContent") as Grid;
            contentInputAccount = GetTemplateChild("contentInputAccount") as Grid;
            contentInputPass = GetTemplateChild("contentInputPass") as Grid;
            txtBoxAcc = GetTemplateChild("txtBoxAcc") as TextBox;
            passBoxPswd = GetTemplateChild("passBoxPswd") as PasswordBox;
            useQRCode = GetTemplateChild("useQRCodeButton") as Button;
            useAccount = GetTemplateChild("useAccountButton") as Button;
            qrcodePresenter = GetTemplateChild("qrcodePresenter") as Image;
            qrcodeOutdatedFilter = GetTemplateChild("qrcodeOutdatedFilter") as Border;

            windowCaption.SetTargetWindow(this);
            windowCaption.SetMaximizeEnabled(false);

            ((Button)GetTemplateChild("nextStep")).Click += new RoutedEventHandler(NextStep_Click);
            ((Button)GetTemplateChild("lastStep")).Click += new RoutedEventHandler(LastStep_Click);

            useQRCode.Click += new RoutedEventHandler(UseQRCodeButton_Click);
            useAccount.Click += new RoutedEventHandler(UseAccountButton_Click);
            qrcodeOutdatedFilter.PreviewMouseLeftButtonUp += delegate
            {
                qrcodePresenter.Visibility = Visibility.Hidden;
                qrcodeOutdatedFilter.Visibility = Visibility.Hidden;
                ResetQRCode();
            };

            ShowContent();
        }

        private async void UseQRCodeButton_Click(object sender, RoutedEventArgs args)
        {
            useAccount.Visibility = Visibility.Visible;
            useQRCode.Visibility = Visibility.Collapsed;

            actionContent.Visibility = Visibility.Collapsed;
            qrcodeContent.Visibility = Visibility.Visible;

            ResetQRCode();
        }

        private void UseAccountButton_Click(object sender, RoutedEventArgs args)
        {
            useAccount.Visibility = Visibility.Collapsed;
            useQRCode.Visibility = Visibility.Visible;

            actionContent.Visibility = Visibility.Visible;
            qrcodeContent.Visibility = Visibility.Collapsed;
        }

        public async void ResetQRCode()
        {
            QRCode qrcode = null; // await AppCenter.Client.GetLoginQRCode();
            var bitmap = qrcode.GetGraphic(20);

            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bytes = ms.GetBuffer();  //byte[]   bytes=   ms.ToArray(); 这两句都可以
            ms.Close();
            //Convert it to BitmapImage
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(bytes);
            image.EndInit();
            qrcodePresenter.Source = image;
            qrcodePresenter.Visibility = Visibility.Visible;
            qrcodeOutdatedFilter.Visibility = Visibility.Hidden;
        }

        private void NextStep_Click(object sender, RoutedEventArgs args)
        {
            /**
             * handle checking account process HERE.
             * 
             * 1.check the textbox NotNull
             */

            var txtAcc = txtBoxAcc.Text;
            if(txtAcc == null || txtAcc == "")
            {
                return;
            }

            PlayAccGotoPass();
        }

        private void LastStep_Click(object sender, RoutedEventArgs args)
        {
            PlayPassGotoAcc();
        }

        private void PlayAccGotoPass()
        {
            ((Grid)GetTemplateChild("scanToLoginPanel")).Visibility = Visibility.Collapsed;

            var contentAccLeftMove = new ThicknessAnimation(contentInputAccount.Margin, new Thickness(-647, 0, 30, 0), new Duration(TimeSpan.FromMilliseconds(600)))
            {
                AccelerationRatio = 0.7d,
                EasingFunction = new CubicEase()
            };
            var contentPassLeftMove = new ThicknessAnimation(contentInputPass.Margin, new Thickness(30, 0, 30, 0), new Duration(TimeSpan.FromMilliseconds(600)))
            {
                AccelerationRatio = 0.7d,
                EasingFunction = new CubicEase()
            };

            contentAccLeftMove.Completed += delegate { contentInputAccount.Visibility = Visibility.Collapsed; };
            contentInputPass.Visibility = Visibility.Visible;

            contentInputAccount.BeginAnimation(MarginProperty, contentAccLeftMove);
            contentInputPass.BeginAnimation(MarginProperty, contentPassLeftMove);
        }

        private void PlayPassGotoAcc()
        {
            ((Grid)GetTemplateChild("scanToLoginPanel")).Visibility = Visibility.Visible;

            var contentAccRightMove = new ThicknessAnimation(contentInputAccount.Margin, new Thickness(30, 0, 30, 0), new Duration(TimeSpan.FromMilliseconds(600)))
            {
                AccelerationRatio = 0.7d,
                EasingFunction = new CubicEase()
            };
            var contentPassRightMove = new ThicknessAnimation(contentInputPass.Margin, new Thickness(430, 0, 30, 0), new Duration(TimeSpan.FromMilliseconds(600)))
            {
                AccelerationRatio = 0.7d,
                EasingFunction = new CubicEase()
            };

            contentInputAccount.Visibility = Visibility.Visible;
            contentPassRightMove.Completed += delegate { contentInputPass.Visibility = Visibility.Collapsed; };

            contentInputAccount.BeginAnimation(MarginProperty, contentAccRightMove);
            contentInputPass.BeginAnimation(MarginProperty, contentPassRightMove);
        }

        async void ShowContent()
        {
            await Task.Delay(4000);
            ((ModernWpf.Controls.ProgressRing)GetTemplateChild("loadingProgressRing")).BeginAnimation(OpacityProperty, new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromSeconds(0.25d))));
            ((Border)GetTemplateChild("filter")).BeginAnimation(OpacityProperty, new DoubleAnimation(0.0d, 1.0d, new Duration(TimeSpan.FromSeconds(1.2d))) { BeginTime = TimeSpan.FromSeconds(0.25d) });
            //((BlurEffect)GetTemplateChild("backImageBlurEffect")).BeginAnimation(BlurEffect.RadiusProperty, new DoubleAnimation(0, 50, new Duration(TimeSpan.FromSeconds(1))));

            ThemeShadowChrome tsc = GetTemplateChild("borderElement") as ThemeShadowChrome;
            tsc.Visibility = Visibility.Visible;
            tsc.BeginAnimation(OpacityProperty, new DoubleAnimation(
                0.0d,
                1.0d,
                new Duration(TimeSpan.FromSeconds(0.5d))
            )
            {
                BeginTime = TimeSpan.FromSeconds(0.5d),
                EasingFunction = new CubicEase(),
                AccelerationRatio = 0.6d,
            });
        }

    }
}
