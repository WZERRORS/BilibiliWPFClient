using BiliWpf.Client.Pages;
using BiliWpf.Controls.Helpers;
using BiliWpf.Services.Models.Account;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ArchiveResponseCard : UserControl
    {
        public ArchiveResponseCard()
        {
            InitializeComponent();
        }

        public void Init(ArchiveResponse archive, string mid)
        {
            CardHelper.SetCardTitle(ArchiveContent, "用户投稿");
        }
    }
}
