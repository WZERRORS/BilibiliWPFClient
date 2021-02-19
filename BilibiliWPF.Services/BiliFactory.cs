using BiliWpf.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BiliWpf.Services
{
    public class BiliFactory
    {
        public static ApiKeyInfo AndroidKey = new ApiKeyInfo("4409e2ce8ffd12b8", "59b43e04ad6965f34319062b478f83dd");
        public static ApiKeyInfo AndroidVideoKey = new ApiKeyInfo("iVGUTjsxvpLeuDCf", "aHRmhWMLkdeMuILqORnYZocwMBpMEOdt");
        public static ApiKeyInfo WebVideoKey = new ApiKeyInfo("84956560bc028eb7", "94aba54af9065f71de72f5508f1cd42e");
        public static ApiKeyInfo VideoKey = new ApiKeyInfo("", "1c15888dc316e05a15fdd0a02ed6584f");
        public static ApiKeyInfo IosKey = new ApiKeyInfo("4ebafd7c4951b366", "8cb98205e9b2ad3669aad0fce12a4c13");
        public const string BuildNumber = "5520400";

        public static async Task<string> EncryptAsPasswordAsync(string password)
        {
            string base64String;
            try
            {
                string param = UrlContact("").TrimStart('?');
                string content = await BiliClient.Current.PostContentToWebAsync(Api.PASSPORT_KEY_ENCRYPT, param);
                JObject jobj = JObject.Parse(content);

                string str = jobj["data"]["hash"].ToString();
                string str1 = jobj["data"]["key"].ToString();
                string str2 = string.Concat(str, password);
                string str3 = Regex.Match(str1, "BEGIN PUBLIC KEY-----(?<key>[\\s\\S]+)-----END PUBLIC KEY").Groups["key"].Value.Trim();
                byte[] numArray = Convert.FromBase64String(str3);
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(Encoding.Default.GetString(numArray));
                var cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
                return Convert.ToBase64String(cipherbytes);
            }
            catch (Exception)
            {
                base64String = password;
            }
            return base64String;
        }

        public static string GetFormatDate(DateTime expirationDate)
        {
            return expirationDate.ToString("ddd, dd-MMM-yyyy HH:mm:ss") + " GMT";
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="apiKeyInfo">取用的Api密钥</param>
        /// <returns></returns>
        public static string GetSign(string url, ApiKeyInfo apiKeyInfo = null)
        {
            if (apiKeyInfo == null)
            {
                apiKeyInfo = AndroidKey;
            }
            string result;
            if (url.StartsWith("http"))
                url.Substring(url.IndexOf("?", 4) + 1);
            List<string> list = url.Split('&').ToList();
            list.Sort();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string str1 in list)
            {
                stringBuilder.Append((stringBuilder.Length > 0 ? "&" : string.Empty));
                stringBuilder.Append(str1);
            }
            stringBuilder.Append(apiKeyInfo.Secret);
            result = MD5Tool.GetMd5String(stringBuilder.ToString()).ToLower();
            return result;
        }

        public static string UrlContact(string _baseUrl, Dictionary<string, string> parameters = null, bool hasAccessKey = false, bool useWeb = false,
            bool useiPhone = false)
        {
            if (parameters == null)
                parameters = new Dictionary<string, string>();

            parameters.Add("build", BuildNumber);
            if (useiPhone)
            {
                parameters.Add("appkey", IosKey.Appkey);
                parameters.Add("mobi_app", "iphone");
                parameters.Add("platform", "ios");
                parameters.Add("ts", GetNowSeconds().ToString());
            }
            else if (!useWeb)
            {
                parameters.Add("appkey", AndroidKey.Appkey);
                parameters.Add("mobi_app", "android");
                parameters.Add("platform", "android");
                parameters.Add("ts", GetNowSeconds().ToString());
            }
            else
            {
                parameters.Add("appkey", WebVideoKey.Appkey);
                parameters.Add("ts", GetNowMilliSeconds().ToString());
            }
            if (hasAccessKey && !string.IsNullOrEmpty(BiliClient.AccessToken))
                parameters.Add("access_key", BiliClient.AccessToken);
            string param = string.Empty;
            foreach (var item in parameters)
            {
                param += $"{item.Key}={item.Value}&";
            }
            param = param.TrimEnd('&');
            string sign = useWeb ? GetSign(param, WebVideoKey) : GetSign(param);
            param += $"&sign={sign}";
            return !string.IsNullOrEmpty(_baseUrl) ? _baseUrl + $"?{param}" : param;
        }

        /// <summary>
        /// 获取当前时间戳（秒）
        /// </summary>
        /// <returns></returns>
        public static int GetNowSeconds()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0);
            int seconds = Convert.ToInt32(ts.TotalSeconds);
            return seconds;
        }
        /// <summary>
        /// 获取当前时间戳（毫秒）
        /// </summary>
        /// <returns></returns>
        public static long GetNowMilliSeconds()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0);
            long seconds = Convert.ToInt64(ts.TotalMilliseconds);
            return seconds;
        }

        public static int GetTimeStampFuture(long timeLater)
        {
            TimeSpan ts = DateTime.Now.AddSeconds(timeLater) - new DateTime(1970, 1, 1, 8, 0, 0, 0);
            int seconds = Convert.ToInt32(ts.TotalSeconds);
            return seconds;
        }
    }

}
