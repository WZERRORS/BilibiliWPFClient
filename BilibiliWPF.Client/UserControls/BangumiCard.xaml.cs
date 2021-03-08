using BiliWpf.Services;
using BiliWpf.Services.Models.Anime;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiliWpf.Client.UserControls
{
    /// <summary>
    /// Interaction logic for BangumiCard.xaml
    /// </summary>
    public partial class BangumiCard : UserControl
    {
        public BangumiCard()
        {
            InitializeComponent();

            RenderOptions.SetBitmapScalingMode(CoverPicture, BitmapScalingMode.Fant);
        }

        public string Param { get; set; }

        public BangumiResponse Data
        {
            get { return (BangumiResponse)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(BangumiResponse), typeof(BangumiCard), new PropertyMetadata(null, new PropertyChangedCallback(Data_Changed)));

        private static void Data_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null && e.NewValue is BangumiResponse data)
            {
                var instance = d as BangumiCard;
                var cover = data.cover;

                ThreadPool.QueueUserWorkItem(async (callback) =>
                {
                    var path = await BiliClient.GetFileAsCacheAsync(cover);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        instance.CoverPicture.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                        instance.NameText.Text = data.title;
                        instance.NewEpText.Text = $"更新至: {data.newest_ep_index}";
                        instance.Param = data.param;
                    });
                });
            }
        }

        private void Card_MouseEnter(object sender, MouseEventArgs e)
        {
            CardShadow.BeginAnimation(DropShadowEffect.BlurRadiusProperty, new DoubleAnimation(
                CardShadow.BlurRadius,
                12,
                new Duration(TimeSpan.FromMilliseconds(200.0d))
            ));
            CardShadow.BeginAnimation(DropShadowEffect.ShadowDepthProperty, new DoubleAnimation(
                CardShadow.ShadowDepth,
                4,
                new Duration(TimeSpan.FromMilliseconds(200.0d))
            ));
        }

        private void Card_MouseLeave(object sender, MouseEventArgs e)
        {
            CardShadow.BeginAnimation(DropShadowEffect.BlurRadiusProperty, new DoubleAnimation(
                CardShadow.BlurRadius,
                2,
                new Duration(TimeSpan.FromMilliseconds(320.0d))
            ));
            CardShadow.BeginAnimation(DropShadowEffect.ShadowDepthProperty, new DoubleAnimation(
                CardShadow.ShadowDepth,
                0,
                new Duration(TimeSpan.FromMilliseconds(320.0d))
            ));
        }
    }
}
