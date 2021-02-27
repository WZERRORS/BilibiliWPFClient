using BiliWpf.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliWpf.Services
{
    public class SkinService
    {
        public async Task<Skin> GetSkinAsync()
        {
            string url = BiliFactory.UrlContact(Api.SKIN_PACKAGE, hasAccessKey: true);
            var data = await BiliClient.ConvertEntityFromWebAsync<Skin>(url);
            return data;
        }
    }
}
