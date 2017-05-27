using System;
using System.IO;
using System.Net;
using System.Text;

namespace Update
{
    /// <summary>
    /// http 操作类，通用的http操作类
    /// </summary>
    public static class NetHelper
    {
        public static string HttpCall(string url, string postData, HttpEnum method)
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            if (method == HttpEnum.Post)
            {
                myHttpWebRequest.Method = "POST";

                //采用UTF8编码
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byte1 = encoding.GetBytes(postData);
                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                myHttpWebRequest.ContentLength = byte1.Length;

                /*
                * 写请求体数据
                */

                Stream newStream = myHttpWebRequest.GetRequestStream();
                newStream.Write(byte1, 0, byte1.Length);
                newStream.Close();
            }
            else// get
                myHttpWebRequest.Method = "GET";
            myHttpWebRequest.ProtocolVersion = new Version(1, 1);   //Http/1.1版本

            //发送成功后接收返回的JSON信息
            HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.GetResponse();
            string lcHtml = string.Empty;
            Encoding enc = Encoding.GetEncoding("UTF-8");
            Stream stream = response.GetResponseStream();
            if (stream != null)
            {
                StreamReader streamReader = new StreamReader(stream, enc);
                lcHtml = streamReader.ReadToEnd();
            }
            return (lcHtml);
        }

        public static string GETProperties<T>(T t)
        {
            string tStr = string.Empty;
            if (t == null)
            {
                return tStr;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return tStr;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (value==null)
                    {
                        continue;
                    }
                    tStr += $"{name}={value}&";
                    
                }
                else
                {
                    GETProperties(value);
                }
            }
            tStr = tStr.Substring(0, tStr.Length - 1);
            return tStr;
        }
    }

    public enum HttpEnum
    {
        /// <summary>
        /// http post 方法
        /// </summary>
        Post = 1,
        /// <summary>
        /// http get 方法
        /// </summary>
        Get = 2,
        /// <summary>
        /// http put 方法
        /// </summary>
        Put = 3,
        /// <summary>
        /// http delete 方法
        /// </summary>
        Delete = 4
    }
}
