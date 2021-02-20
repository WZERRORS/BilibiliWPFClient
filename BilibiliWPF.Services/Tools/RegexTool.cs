using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BiliWpf.Services.Tools
{
    public static class RegexTool
    {
        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$");
        private static Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
        private static Regex RegTell = new Regex("^(([0-9]{3,4}-)|[0-9]{3.4}-)?[0-9]{7,8}$");
        private static Regex RegSend = new Regex("[0-9]{1}([0-9]+){5}");
        private static Regex RegUrl = new Regex("^[a-zA-z]+://(\\w+(-\\w+)*)(\\.(\\w+(-\\w+)*))*(\\?\\S*)?|[a-zA-z]+://((?:(?:25[0-5]|2[0-4]\\d|[01]?\\d?\\d)\\.){3}(?:25[0-5]|2[0-4]\\d|[01]?\\d?\\d))$");
        private static Regex RegMobilePhone = new Regex("^13|15|18[0-9]{9}$");
        private static Regex RegMoney = new Regex("^[0-9]+|[0-9]+[.]?[0-9]+$");

        #region 数字字符串检查

        /// <summary>
        /// 是否数字字符串
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        public static bool IsNumber(string inputData)
        {
            if (!string.IsNullOrEmpty(inputData))
            {
                Match m = RegNumber.Match(inputData);
                return m.Success;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        public static bool IsNumberSign(string inputData)
        {
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        public static bool IsDecimal(string inputData)
        {
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        public static bool IsDecimalSign(string inputData)
        {
            Match m = RegDecimalSign.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 中文检测

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        public static bool IsHasCHZN(string inputData)
        {
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 邮件地址
        /// <summary>
        /// 是否是邮箱
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        public static bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 电话,邮政编码,网络地址,手机号码,价格
        /// <summary>
        /// 验证是否是电话
        /// </summary>
        public static bool IsPhone(string inputDate)
        {
            if (!string.IsNullOrEmpty(inputDate))
            {
                Match m = RegTell.Match(inputDate);
                return m.Success;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是否是邮政编码
        /// </summary> 
        public static bool IsSend(string inputDate)
        {
            if (!string.IsNullOrEmpty(inputDate))
            {
                Match m = RegSend.Match(inputDate);
                return m.Success;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是否是网络地址
        /// </summary>
        public static bool IsUrl(string inputData)
        {
            Match m = RegUrl.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是手机号码
        /// </summary>
        public static bool IsMobilePhone(string inputData)
        {
            Match m = RegMobilePhone.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是价格
        /// </summary>
        public static bool IsMoney(string inputDate)
        {
            Match m = RegMoney.Match(inputDate);
            return m.Success;
        }
        #endregion
        #region 是否是时间格式
        /// <summary>
        /// 判断一个字符串是否时间格式
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDateTime(string inputData)
        {
            try
            {
                Convert.ToDateTime(inputData);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
