using BiliWpf.Services;
using BiliWpf.Services.Models.Account;
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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            int uid = (int)e.ExtraData;
            ThreadPool.QueueUserWorkItem(async delegate
            {
                var response = await BiliClient.Account.GetUserSpaceAsync(uid);
                if(response.images != null)
                {
                    var topPhotoPath = await BiliClient.GetFileAsCacheAsync(response.images.imgUrl);
                    Application.Current.Dispatcher.Invoke(() => SpaceTopPhoto.Source = new BitmapImage(new Uri(topPhotoPath, UriKind.RelativeOrAbsolute)));
                }
                
            });
            
            
        }
    }
}
