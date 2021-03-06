using BiliWpf.Client.Pages;
using BiliWpf.Controls;
using BiliWpf.Sevices.Models;
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
using ModernWpf.Media.Animation;
using BiliWpf.Services;

namespace BiliWpf.Client.UserControls
{
    /// <summary>
    /// Interaction logic for MainPageNavigationRoot.xaml
    /// </summary>
    public partial class PageRoot : UserControl
    {
        public WindowCaption WindowCaption { get { return windowCaption; } }

        public PageRoot()
        {
            InitializeComponent();

            BiliClient.Account.MeUpdate += (sender, args) =>
            {
                var data = BiliClient.Account.Me;

                Task.Run(() =>
                {
                    string path = BiliClient.GetFileAsCacheAsync(data.face);
                    data.face_path = path;

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        imageUserFace.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                    });
                });
            };
            RenderOptions.SetBitmapScalingMode(imageUserFace, BitmapScalingMode.Fant);
            contentFrame.Navigated += (sender, args) => contentFramePgRing.Visibility = Visibility.Collapsed;
        }
    }
}
