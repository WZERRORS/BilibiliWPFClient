using BiliWpf.Services.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BiliWpf.Services
{
    public class BiliClient
    {
        public static readonly BiliClient Current = new BiliClient();
        private string sid = "";
        private string mid = "";
        private HttpClient _httpClient;
        private AccountService _account;
        public static string AccessToken { get; set; }
        public static AccountService Account { get { return Current._account; } }

        public BiliClient(string accessToken = "", string refreshToken = "", int expiry = 0)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            _httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36 Edge/18.19041");
            _httpClient.SendAsync(new HttpRequestMessage()
            {
                Method = new HttpMethod("HEAD"),
                RequestUri = new Uri("http://www.bilibili.com/")
            }).Result.EnsureSuccessStatusCode();

            var package = new TokenPackage(accessToken, refreshToken, expiry);
            AccessToken = accessToken;
            _account = new AccountService(package);
        }

        /// <summary>
        /// 从网络获取文本
        /// </summary>
        /// <param name="url"></param>
        /// <param name="total">标识返回整个数据包还是提取path的内容</param>
        /// <param name="path">提取的路径</param>
        /// <returns></returns>
        public async Task<string> GetStringFromWebAsync(string url)
        {
            var response = await _httpClient.GetAsync(new Uri(url));
            if(response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(str);
                return str;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.TemporaryRedirect
                || response.StatusCode == System.Net.HttpStatusCode.MovedPermanently)
            {
                string tempUrl = response.Headers.Location.AbsoluteUri;
                return await GetStringFromWebAsync(tempUrl);
            }
            else
            {
                return null;
            }
        }

        public async Task<JToken> GetJsonFromWebAsync(string url, bool total = false, string path = "data")
        {
            var response = await _httpClient.GetAsync(new Uri(url));
            if (response.IsSuccessStatusCode)
            {
                string str = await response.Content.ReadAsStringAsync();
                var jobj = JObject.Parse(str);
                System.Diagnostics.Debug.WriteLine(jobj.ToString());
                if (total)
                    return jobj;

                return jobj[path];
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.TemporaryRedirect
                || response.StatusCode == System.Net.HttpStatusCode.MovedPermanently)
            {
                string tempUrl = response.Headers.Location.AbsoluteUri;
                return await GetJsonFromWebAsync(tempUrl, total, path);
            }
            else
            {
                return null;
            }
        }

        public async Task<T> GetObjectFromWebAsync<T>(string url, Type type) where T : class
        {
            string response = await GetStringFromWebAsync(url);
            if (!string.IsNullOrEmpty(response))
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(response);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
            return null;
        }

        public async Task<Stream> GetStreamFromWebAsync(string url)
        {
            var response = await _httpClient.GetAsync(new Uri(url));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStreamAsync();
            }
            else
            {
                return null;
            }
        }

        public async Task<string> PostContentToWebAsync(string url, string content)
        {
            if (url.Contains("oauth2/login") && !string.IsNullOrEmpty(sid))
            {
                SetCookie("bilibili.com", "sid", sid);
            }
            var response = await _httpClient.PostAsync(new Uri(url), new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));
            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(str);
                return str;
            }
            else
            {
                //log exception
            }
            return "";
        }

        public void SetCookie(string url, string name, string value, long expire)
        {
            Application.SetCookie(new Uri(url), string.Format("{0}={1}; Expires={2}", name, value, BiliFactory.GetFormatDate(new DateTime(expire))));
        }

        public void SetCookie(string url, string name, string value)
        {
            Application.SetCookie(new Uri(url), string.Format("{0}={1}", name, value));
        }
    }
}
