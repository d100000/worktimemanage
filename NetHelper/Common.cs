using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using MessageBox = System.Windows.MessageBox;

namespace Helper
{
    /// <summary>
    /// http 通用的系统方法
    /// - 获取时间戳：GetTimeSecond
    /// - 由时间戳转为系统时间：ReturnDateTime
    /// - SHA1 加密：Sha1Sign
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeSecond(DateTime dataTime)
        {
            return ((dataTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
        }

        /// <summary>
        /// 由时间戳到系统时间
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ReturnDateTime(string date)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(date + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// SHA1加密==PHP（SHA1）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Sha1Sign(string data)
        {
            byte[] temp1 = Encoding.UTF8.GetBytes(data);
            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            byte[] temp2 = sha.ComputeHash(temp1);
            sha.Clear();
            // 注意， 不能用这个
            //string output = Convert.ToBase64String(temp2);
            var output = BitConverter.ToString(temp2);
            output = output.Replace("-", "");
            output = output.ToLower();
            return output;
        }

    }

    public enum HttpEnum 
    {
        /// <summary>
        /// http post 方法
        /// </summary>
        Post=1,
        /// <summary>
        /// http get 方法
        /// </summary>
        Get=2,
        /// <summary>
        /// http put 方法
        /// </summary>
        Put=3,
        /// <summary>
        /// http delete 方法
        /// </summary>
        Delete=4
    }

}
