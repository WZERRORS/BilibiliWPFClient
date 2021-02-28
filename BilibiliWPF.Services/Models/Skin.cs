using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliWpf.Services.Models
{
    public class Skin
    {
        public UserEquip user_equip { get; set; }
        public LoadEquip load_equip { get; set; }
    }

    public class UserEquip
    {
        public int id { get; set; }
        public string name { get; set; }
        public string preview { get; set; }
        public int ver { get; set; }
        public string package_url { get; set; }
        public string package_md5 { get; set; }
        public Data data { get; set; }
    }

    public class LoadEquip
    {
        public int id { get; set; }
        public string name { get; set; }
        public int ver { get; set; }
        public string loading_url { get; set; }
    }

    public class Data
    {
        public string color_mode { get; set; }
        public string color { get; set; }
        public string color_second_page { get; set; }
        public string side_bg_color { get; set; }
        public string tail_color { get; set; }
        public string tail_color_selected { get; set; }
        public bool tail_icon_ani { get; set; }
        public string tail_icon_ani_mode { get; set; }
    }
}
