using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliWpf.Services.Models
{
    public class UserData
    {

        public UserData(JObject jobj)
        {

        }

        public async Task SetupNetResources()
        {

        }

        public async static Task<UserData> GetUserData(JObject jobj)
        {
            var data = new UserData(jobj);
            await data.SetupNetResources();
            return data;
        }
    }
}
