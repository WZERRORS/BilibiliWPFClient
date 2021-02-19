using BiliWpf.Services.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliWpf.Services.Models
{
    public struct TokenInfo
    {
        public long Mid { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int Expires { get; set; }
        
        public TokenInfo(JObject jobj)
        {
            Mid = jobj.Value<long>("mid");
            AccessToken = jobj.Value<string>("access_token");
            RefreshToken = jobj.Value<string>("refresh_token");
            Expires = jobj.Value<int>("expires_in");
        }
    }

    public class Cookies
    {
        public string name { get; set; }
        public string value { get; set; }
        public int http_only { get; set; }
        public int expires { get; set; }
    }

    public struct CookieInfo
    {
        public List<Cookies> cookies { get; set; }
        public List<string> domains { get; set; }
    }

    public class LoginResult
    {
        public int Status { get; set; }
        public TokenInfo Token { get; set; }
        public CookieInfo Cookie { get; set; }
        public List<string> SSO { get; set; }
        public string Url { get; set; }
        public long Mid { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int Expires { get; set; }

        public LoginResult(JObject jobj)
        {
            Status = jobj.Value<int>("status");
            Token = new TokenInfo(jobj.Value<JObject>("token_info"));
            Cookie = JsonConvert.DeserializeObject<CookieInfo>(jobj["cookie_info"].ToString());
            SSO = JsonConvert.DeserializeObject<List<string>>(jobj["sso"].ToString());
            Url = jobj.Value<string>("url");
            Mid = jobj.Value<long>("mid");
            AccessToken = jobj.Value<string>("access_token");
            RefreshToken = jobj.Value<string>("refresh_token");
            Expires = jobj.Value<int>("expires_in");
        }
    }

    public class LoginCallback
    {
        public LoginResultType Status { get; set; }
        public string Url { get; set; }
    }

    public class TokenPackage
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int Expiry { get; set; }
        public TokenPackage()
        {
            AccessToken = "";
            RefreshToken = "";
            Expiry = 0;
        }
        public TokenPackage(string acc, string refe, int exp)
        {
            AccessToken = acc;
            RefreshToken = refe;
            Expiry = exp;
        }
    }
}
