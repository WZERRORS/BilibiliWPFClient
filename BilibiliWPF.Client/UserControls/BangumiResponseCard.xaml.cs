using BiliWpf.Controls.Helpers;
using BiliWpf.Services.Models.Anime;
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

namespace BiliWpf.Client.UserControls
{
    /// <summary>
    /// Interaction logic for SeasonResponseCard.xaml
    /// </summary>
    public partial class BangumiResponseCard : UserControl
    {
        public BangumiResponseCard()
        {
            InitializeComponent();
        }

        public void Init(List<BangumiResponse> archive, string mid)
        {
            CardHelper.SetCardTitle(BangumiContent, "追番");
            List<BangumiResponse> NewList = new List<BangumiResponse>();
            for (int l = 0; l < archive.Capacity && l < 8; l++)
                NewList.Add(archive[l]);
            UniformGridLayoutRepeater.ItemsSource = NewList;
        }
    }
}
