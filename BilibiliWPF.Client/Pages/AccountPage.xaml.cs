using BiliWpf.Services;
using BiliWpf.Services.Models.Account;
using BiliWpf.Sevices.Models;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiliWpf.Client.Pages
{
    /// <summary>
    /// Interaction logic for AccountPage.xaml
    /// </summary>
    public partial class AccountPage : ModernWpf.Controls.Page
    {
        public AccountPage()
        {
            InitializeComponent();
            RenderOptions.SetBitmapScalingMode(TopPhotoComponent, BitmapScalingMode.Fant);
            RenderOptions.SetBitmapScalingMode(FaceComponent, BitmapScalingMode.Fant);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            int uid = (int)e.ExtraData;
            Controls.WindowCaption.Captions[Application.Current.MainWindow].ShowBackground = true;
            ThreadPool.QueueUserWorkItem(async delegate
            {
                var response = await BiliClient.Account.GetUserSpaceAsync(uid);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    NameText.Text = response.card.name;
                    SignText.Text = response.card.sign;
                    UidText.Text = $"Uid:{response.card.mid}";

                    {
                        switch(response.card.sex)
                        {
                            case "男": SexText.Text = "\u2642"; SexText.Foreground = new SolidColorBrush(Colors.DodgerBlue); break;
                            case "女": SexText.Text = "\u2640"; SexText.Foreground = new SolidColorBrush(Colors.Lavender); break;
                            default: SexText.Text = "\u2642\u00BF\u003F\u2640"; SexText.Foreground = new SolidColorBrush(Colors.Gray); break;
                        }
                    }

                    {
                        Vip vipData = response.card.vip;
                        if (vipData.vipStatus == 1)
                        {
                            BigVipElement.Visibility = Visibility.Visible;
                            BigVipText.Text = vipData.label.text;
                        }
                        else
                            BigVipElement.Visibility = Visibility.Collapsed;
                    }

                    {
                        var live = response.live;
                        LiveRoomElement.Visibility = live.liveStatus == 1 ? Visibility.Visible : Visibility.Collapsed;
                    }

                    {
                        var archive = response.archive;
                        ArchiveElement.Visibility = archive.count > 0 ? Visibility.Visible : Visibility.Collapsed;
                    }

                    {
                        var article = response.article;
                        SetCardCount(ArticleElement, article.count.ToString());
                    }

                    {
                        var season = response.season;
                        SetCardCount(SeasonContent, season.count.ToString());
                    }

                    SetCardTitle(ArchiveContent, "用户投稿");
                    SetCardTitle(SeasonContent, "追番");
                    SetCardTitle(ArticleContent, "专栏");

                });

                if(response.images != null)
                {
                    var topPhotoPath = await BiliClient.GetFileAsCacheAsync(response.images.imgUrl);
                    Application.Current.Dispatcher.Invoke(() => TopPhotoComponent.Source = new BitmapImage(new Uri(topPhotoPath, UriKind.RelativeOrAbsolute)));
                }

                var facePath = await BiliClient.GetFileAsCacheAsync(response.card.face);
                Application.Current.Dispatcher.Invoke(() => FaceComponent.Source = new BitmapImage(new Uri(facePath, UriKind.RelativeOrAbsolute)));
            });
            
            
        }

        #region
        public static readonly DependencyProperty CardTitleProperty
            = DependencyProperty.RegisterAttached("CardTitle", typeof(string), typeof(AccountPage), new PropertyMetadata(""));

        public static string GetCardTitle(DependencyObject element) => (string)element.GetValue(CardTitleProperty);
        public static void SetCardTitle(DependencyObject element, string value) => element.SetValue(CardTitleProperty, value);
        #endregion

        #region
        public static readonly DependencyProperty CardCountProperty
            = DependencyProperty.RegisterAttached("CardCount", typeof(string), typeof(AccountPage), new PropertyMetadata(""));

        public static string GetCardCount(DependencyObject element) => (string)element.GetValue(CardCountProperty);
        public static void SetCardCount(DependencyObject element, string value) => element.SetValue(CardCountProperty, value);
        #endregion
    }
}
