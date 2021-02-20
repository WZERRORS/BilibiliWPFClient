using BiliWpf.Services;
using ModernWpf.Controls;
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

namespace BiliWpf.Client.Dialogs
{
    /// <summary>
    /// Interaction logic for CaptchaBlock.xaml
    /// </summary>
    public partial class CaptchaBlock : ContentDialog
    {
        public CaptchaBlock()
        {
            InitializeComponent();
        }

        public string Code
        {
            get { return (string)GetValue(CodeProperty); }
            set 
            {
                SetValue(CodeProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Code.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register("Code", typeof(string), typeof(CaptchaBlock), new PropertyMetadata(""));


        public async Task RefreshCode()
        {
            CaptchaImage.Visibility = Visibility.Collapsed;
            LoadingRing.IsActive = true;
            var image = await BiliClient.Account.GetCaptchaAsync();
            CaptchaImage.Source = image;
            LoadingRing.IsActive = false;
            CaptchaImage.Visibility = Visibility.Visible;
        }

        private async void Captcha_MouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            Code = "";
            await RefreshCode();
        }
    }
}
