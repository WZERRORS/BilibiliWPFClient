using BiliWpf.Services.Enums;
using BiliWpf.Services.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BiliWpf.Services
{
    public class BiliClient
    {
        static HttpClient _httpClient;
        public static string Sid { get; set; }
        public static string Mid { get; set; }
        public static AccountService Account { get; set; }
        public static string AccessToken { get; set; }

        static BiliClient()
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            _httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36 Edge/18.19041");
            _httpClient.SendAsync(new HttpRequestMessage()
            {
                Method = new HttpMethod("HEAD"),
                RequestUri = new Uri("http://www.bilibili.com/")
            }).Result.EnsureSuccessStatusCode();

            var package = new TokenPackage("", "", 0);
            AccessToken = "";
            Account = new AccountService(package);
        }

        /// <summary>
        /// 从网络获取文本
        /// </summary>
        /// <param name="url"></param>
        /// <param name="total">标识返回整个数据包还是提取path的内容</param>
        /// <param name="path">提取的路径</param>
        /// <returns></returns>
        public static async Task<string> GetStringFromWebAsync(string url)
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

        public static async Task<string> GetStringFromWebAsync(string url, bool total = false, string path = "data", bool needReferer = false)
        {
            try
            {
                //client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36 Edge/18.19041");
                var uri = new Uri(url);
                if (needReferer)
                    _httpClient.DefaultRequestHeaders.Referrer = new Uri(uri.Scheme + "://" + uri.Host);
                var response = await _httpClient.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    if (total)
                        return res;
                    var jobj = JObject.Parse(res);
                    string content = string.Empty;
                    if (jobj.SelectToken(path) != null)
                        content = jobj.SelectToken(path).ToString();
                    else
                        content = res;
                    return content;
                }
                else if (response.StatusCode == HttpStatusCode.TemporaryRedirect || response.StatusCode == HttpStatusCode.MovedPermanently)
                {
                    string tempUrl = response.Headers.Location.AbsoluteUri;
                    return await GetStringFromWebAsync(tempUrl, total, path);
                }
                else
                {
                    //_logger.Warn($"请求数据异常(Text)：URL: {url}; Message: {await response.Content.ReadAsStringAsync()}");
                    return null;
                }
            }
            catch (Exception)
            {
                //_logger.Error($"请求出现异常:{url}", ex);
                return null;
            }
        }

        public static async Task<T> ConvertEntityFromWebAsync<T>(string url, string path = "data", bool needReferer = false) where T : class
        {
            
            string response = await GetStringFromWebAsync(url);
            response = JObject.Parse(response)[path].ToString();
            if (!string.IsNullOrEmpty(response))
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(response);
                }
                catch (Exception)
                {
                    //_logger.Error($"数据转化失败: {nameof(T)}", ex);
                }
            }
            return null;
        }

        public static async Task<JToken> GetJsonFromWebAsync(string url, bool total = false, string path = "data")
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

        public static async Task<T> GetObjectFromWebAsync<T>(string url, Type type) where T : class
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

        public static async Task<Stream> GetStreamFromWebAsync(string url)
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

        public static async Task<string> PostContentToWebAsync(string url, string content)
        {
            if (url.Contains("oauth2/login") && !string.IsNullOrEmpty(Sid))
            {
                SetCookie("bilibili.com", "sid", Sid);
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

        public static void SetCookie(string url, string name, string value, long expire)
        {
            Application.SetCookie(new Uri(url), string.Format("{0}={1}; Expires={2}", name, value, BiliFactory.GetFormatDate(new DateTime(expire))));
        }

        public static void SetCookie(string url, string name, string value)
        {
            Application.SetCookie(new Uri(url), string.Format("{0}={1}", name, value));
        }

        /// <summary>
        /// 写入本地设置
        /// </summary>
        /// <param name="key">设置名</param>
        /// <param name="value">设置值</param>
        public static void WriteLocalSetting(Settings key, string value)
        {
            ConfigurationManager.AppSettings.Set("bilibili_" + key.ToString(), value);
        }

        /// <summary>
        /// 读取本地设置
        /// </summary>
        /// <param name="key">设置名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetLocalSetting(Settings key, string defaultValue)
        {
            return ConfigurationManager.AppSettings.Get(key.ToString());
            /**
            var localSetting = ApplicationData.Current.LocalSettings;
            var localcontainer = localSetting.CreateContainer("BiliBili", ApplicationDataCreateDisposition.Always);
            bool isKeyExist = localcontainer.Values.ContainsKey(key.ToString());
            if (isKeyExist)
            {
                return localcontainer.Values[key.ToString()].ToString();
            }
            else
            {
                WriteLocalSetting(key, defaultValue);
                return defaultValue;
            }
            */
        }

        /// <summary>
        /// 获取布尔值的设置
        /// </summary>
        /// <param name="key">设置名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static bool GetBoolSetting(Settings key, bool defaultValue = true)
        {
            return Convert.ToBoolean(GetLocalSetting(key, defaultValue.ToString()));
        }
    }
}
