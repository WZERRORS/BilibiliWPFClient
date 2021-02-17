using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliWpf.Services.Models
{
    public class VideoBase
    {
        public int aid { get; set; }
        public string bvid { get; set; }
        public int videos { get; set; }
        public int tid { get; set; }
        public string tname { get; set; }
        public int copyright { get; set; }
        public string pic { get; set; }
        public string title { get; set; }
        public int pubdate { get; set; }
        public int ctime { get; set; }
        public string desc { get; set; }
        public int state { get; set; }
    }

    public enum VideoState
    {
        
    }
}
