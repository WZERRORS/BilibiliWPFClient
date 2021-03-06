using BiliWpf.Sevices.Models;
using System.Collections.Generic;

namespace BiliWpf.Services.Models.Account
{
    public class UserResponse
    {
        public User card { get; set; }
        public ArchiveResponse archive { get; set; }
        public SpaceImages images { get; set; }
        public LiveRoom live { get; set; }
        public ItemArray<VideoSimple> season { get; set; }
        public ItemArray<FansDress> fans_dress { get; set; }
        public ItemArray<FavouriteCollection> favourite2 { get; set; }
    }

    public class SpaceImages
    {
        public string imgUrl { get; set; }
        public bool has_grab { get; set; }
        public bool show_reset { get; set; }
        public bool goods_available { get; set; }
        public SpaceGrab grab { get; set; }
    }

    public struct SpaceGrab
    {
        public int grab_id { get; set; }
        public int image_id { get; set; }
        public string small_image { get; set; }
        public string large_image { get; set; }
        public string fans_label { get; set; }
        public string fans_number { get; set; }
    }
    
    public class ItemArray<T>
    {
        public int count { get; set; }
        public List<T> item { get; set; }
    }

    public class VideoSimple
    {
        public string title { get; set; }
        public string cover { get; set; }
        public string param { get; set; }
        public string @goto { get; set; }
    }

    public class User
    {
        public string mid { get; set; }
        public string name { get; set; }
        public bool approve { get; set; }
        public string sex { get; set; }
        public string rank { get; set; }
        public string face { get; set; }
        public string DisplayRank { get; set; }
        public int regtime { get; set; }
        public int spacesta { get; set; }
        public string birthday { get; set; }
        public string place { get; set; }
        public string description { get; set; }
        public int article { get; set; }
        public object attentions { get; set; }
        public int fans { get; set; }
        public int friend { get; set; }
        public int attention { get; set; }
        public string sign { get; set; }
        public LevelInfo level_info { get; set; }
        public Pendant pendant { get; set; }
        public Vip vip { get; set; }
        public int fans_group { get; set; }
        public int silence { get; set; }
        public int end_time { get; set; }
        public string silence_url { get; set; }
        public Likes likes { get; set; }
        public OfficialVerify official_verify { get; set; }
        public string pendant_url { get; set; }
        public string pendant_title { get; set; }
        public Relation relation { get; set; }
        public bool is_deleted { get; set; }
    }
    
    public class Likes
    {
        public int like_num { get; set; }
        public string skr_tip { get; set; }
    }

    public class Bbq
    {
        public string uri { get; set; }
        public string schema { get; set; }
    }

    public class Relation
    {
        public int status { get; set; }
        public int is_follow { get; set; }
        public int is_followed { get; set; }
    }

}
