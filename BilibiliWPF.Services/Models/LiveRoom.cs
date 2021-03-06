using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliWpf.Services.Models
{
    public class LiveRoom
    {
        public int roomStatus { get; set; }
        public int roundStatus { get; set; }
        public int liveStatus { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string cover { get; set; }
        public int online { get; set; }
        public int roomid { get; set; }
        public bool broadcast_type { get; set; }
        public bool online_hidden { get; set; }
        public string link { get; set; }
    }
}
