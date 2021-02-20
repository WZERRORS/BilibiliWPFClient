using BiliWpf.Client.Dialogs;
using BiliWpf.Services;
using BiliWpf.Services.Enums;
using BiliWpf.Services.Tools;
using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiliWpf.Client.UserControls
{
    /// <summary>
    /// Interaction logic for LoginComponent.xaml
    /// </summary>
    public partial class LoginComponent : UserControl
    {
        public LoginComponent()
        {
            InitializeComponent();
            RenderOptions.SetBitmapScalingMode(BiliIcon, BitmapScalingMode.Fant);

            qrcodeOutdatedFilter.PreviewMouseLeftButtonUp += delegate
            {
                qrcodePresenter.Visibility = Visibility.Hidden;
                qrcodeOutdatedFilter.Visibility = Visibility.Hidden;
                ResetQRCode();
            };

            ShowContent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs args)
        {
            if(string.IsNullOrWhiteSpace(txtBoxPswd.Password))
            {
                await new ContentDialog()
                {
                    Title = "密码不能为空",
                    CloseButtonText = "确认"
                }.ShowAsync();
                return;
            }
            ChangeIsLoginingState(true);
            await DoLoginAsync();
        }

        private async Task DoLoginAsync(string captcha = "")
        {
            var result = await BiliClient.Account.LoginAsync(txtBoxAcc.Text, txtBoxPswd.Password, captcha: captcha);
            switch (result.Status)
            {
                case LoginResultType.Success:
                    Task.Run(async () => 
                    {
                        await BiliClient.Account.GetCurrentUserAsync();
                        Application.Current.Dispatcher.Invoke(() => windowCaption.GetTargetWindow().Close());
                    });
                    break;
                case LoginResultType.NeedCaptcha:
                    var captchaBlock = new CaptchaBlock();
                    var mes = await captchaBlock.ShowAsync();
                    if (mes == ContentDialogResult.Primary)
                        await DoLoginAsync(captchaBlock.Code);
                    break;
                case LoginResultType.Fail:
                    await new ContentDialog
                    {
                        Title = "登陆失败",
                        Content = "账号或密码错误"
                    }.ShowAsync();
                    ChangeIsLoginingState(false);
                    break;
                case LoginResultType.Error:
                    await new ContentDialog
                    {
                        Title = "登陆失败",
                        Content = "账号或密码错误"
                    }.ShowAsync();
                    ChangeIsLoginingState(false);
                    break;
            }
        }

        private void ChangeIsLoginingState(bool start)
        {
            lastStep.Visibility = start ? Visibility.Collapsed : Visibility.Visible;
            doLogin.IsEnabled = !start;
            loadingProgressRing.IsActive = start;
        }

        private async void UseQRCodeButton_Click(object sender, RoutedEventArgs args)
        {
            useAccountButton.Visibility = Visibility.Visible;
            useQRCodeButton.Visibility = Visibility.Collapsed;

            actionContent.Visibility = Visibility.Collapsed;
            qrcodeContent.Visibility = Visibility.Visible;

            ResetQRCode();
        }

        private void UseAccountButton_Click(object sender, RoutedEventArgs args)
        {
            useAccountButton.Visibility = Visibility.Collapsed;
            useQRCodeButton.Visibility = Visibility.Visible;

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

        private async void NextStep_Click(object sender, RoutedEventArgs args)
        {
            /**
             * handle checking account process HERE.
             * 
             * 1.check the textbox NotNull
             */

            var txtAcc = txtBoxAcc.Text;
            if (txtAcc == null || txtAcc == "")
            {
                return;
            }
            var isPhoneNum = RegexTool.IsMobilePhone(txtAcc);
            System.Diagnostics.Debug.WriteLine(txtAcc);
            System.Diagnostics.Debug.WriteLine(isPhoneNum);
            if (!RegexTool.IsMobilePhone(txtAcc))
            {
                await new ContentDialog()
                {
                    Title = "这个手机号无效",
                    CloseButtonText = "确认",
                    DefaultButton = ContentDialogButton.Close
                }.ShowAsync();
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
            scanToLoginPanel.Visibility = Visibility.Collapsed;

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
            scanToLoginPanel.Visibility = Visibility.Visible;

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
            await Task.Delay(3000);
            loadingProgressRing.BeginAnimation(OpacityProperty, new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromSeconds(0.25d))));
            filter.BeginAnimation(OpacityProperty, new DoubleAnimation(0.0d, 1.0d, new Duration(TimeSpan.FromSeconds(1.2d))) { BeginTime = TimeSpan.FromSeconds(0.25d) });
            //((BlurEffect)GetTemplateChild("backImageBlurEffect")).BeginAnimation(BlurEffect.RadiusProperty, new DoubleAnimation(0, 50, new Duration(TimeSpan.FromSeconds(1))));

            ThemeShadowChrome tsc = borderElement as ThemeShadowChrome;
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
