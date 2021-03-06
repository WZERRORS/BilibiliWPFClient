using BiliWpf.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Core;

namespace BiliWpf.Services
{
    public class BiliFactory
    {
        public static ApiKeyInfo AndroidKey = new ApiKeyInfo("4409e2ce8ffd12b8", "59b43e04ad6965f34319062b478f83dd");
        public static ApiKeyInfo AndroidVideoKey = new ApiKeyInfo("iVGUTjsxvpLeuDCf", "aHRmhWMLkdeMuILqORnYZocwMBpMEOdt");
        public static ApiKeyInfo WebVideoKey = new ApiKeyInfo("84956560bc028eb7", "94aba54af9065f71de72f5508f1cd42e");
        public static ApiKeyInfo VideoKey = new ApiKeyInfo("", "1c15888dc316e05a15fdd0a02ed6584f");
        public static ApiKeyInfo IosKey = new ApiKeyInfo("4ebafd7c4951b366", "8cb98205e9b2ad3669aad0fce12a4c13");
        public const string BuildNumber = "6082000";

        public static async Task<string> EncryptAsPasswordAsync(string password)
        {
            string base64String;

            string param = UrlContact("").TrimStart('?');
            string content = await BiliClient.PostContentToWebAsync(Api.PASSPORT_KEY_ENCRYPT, param);
            JObject jobj = JObject.Parse(content);
            string str = jobj["data"]["hash"].ToString();
            string str1 = jobj["data"]["key"].ToString();
            string str2 = string.Concat(str, password);
            string str3 = Regex.Match(str1, "BEGIN PUBLIC KEY-----(?<key>[\\s\\S]+)-----END PUBLIC KEY").Groups["key"].Value.Trim();
            byte[] numArray = Convert.FromBase64String(str3);
            AsymmetricKeyAlgorithmProvider asymmetricKeyAlgorithmProvider = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithmNames.RsaPkcs1);
            CryptographicKey cryptographicKey = asymmetricKeyAlgorithmProvider.ImportPublicKey(WindowsRuntimeBufferExtensions.AsBuffer(numArray), 0);
            var buffer = CryptographicEngine.Encrypt(cryptographicKey, WindowsRuntimeBufferExtensions.AsBuffer(Encoding.UTF8.GetBytes(str2)), null);
            base64String = Convert.ToBase64String(WindowsRuntimeBufferExtensions.ToArray(buffer));
            /**
            try
            {
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                base64String = password;
            }
            */
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
            System.Diagnostics.Debug.WriteLine("sign=" + result);
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
            System.Diagnostics.Debug.WriteLine("appkey=" + AndroidKey.Appkey + "&mobi_app=android&platform=android&access_key=" + BiliClient.AccessToken + "&sign=" + sign);
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

        /// <summary>
        /// 获取数字的缩写
        /// </summary>
        /// <param name="number">数字</param>
        /// <returns></returns>
        public static string GetNumberAbbreviation(double number)
        {
            string result = string.Empty;
            if (number < 10000)
                result = number.ToString();
            else
                result = Math.Round(number / 10000.0, 1).ToString() + "万";
            return result;
        }

        public void DownloadFile(string serverFilePath, string targetPath)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverFilePath);
            WebResponse respone = request.GetResponse();
            Stream netStream = respone.GetResponseStream();
            using (Stream fileStream = new FileStream(targetPath, FileMode.Create))
            {
                byte[] read = new byte[1024];
                int realReadLen = netStream.Read(read, 0, read.Length);
                while (realReadLen > 0)
                {
                    fileStream.Write(read, 0, realReadLen);
                    realReadLen = netStream.Read(read, 0, read.Length);
                }
                netStream.Close();
                fileStream.Close();
            }
        }
    }

}
