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

    /// <summary>
    /// 日志类
    /// </summary>
    public class Loghelper
    {

        // TODO:缺少对主线程写入错误的判断，很多写入错误无法写入日志

        private static readonly Mutex Mutex;
        static Loghelper()
        {
            Mutex = new Mutex();
        }

        #region 公开线程写入日志
        /// <summary>
        /// 消息日志记录
        /// </summary>
        /// <param name="message">消息</param>
        public static void Log(string message)
        {
            try
            {
                new Task(() => Writelog(message)).Start();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                // ignored
            }
        }
        /// <summary>
        /// 错误日志记录
        /// </summary>
        /// <param name="ex">错误</param>
        public static void Log(Exception ex)
        {
            try
            {
                new Task(() => WriteBuglog(ex)).Start();
            }
            catch(Exception exception)
            {
                 MessageBox.Show(exception.Message);
                // ignored
            }
        }

        /// <summary>
        /// 错误日志记录
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex">错误</param>
        public static void Log(string message, Exception ex)
        {
            try
            {
                new Task(() => WriteBuglog(message, ex)).Start();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                // ignored
            }
        }

        #endregion

        #region 日志分类
        /// <summary>
        /// 保存普通日志
        /// </summary>
        /// <param name="message"></param>
        private static bool Writelog(string message)
        {
            string logContent = $"[{DateTime.Now:yyyy-MM-dd hh:mm:ss}] =>{message}";
            return SetFile(@"Log.txt", logContent);
        }

        /// <summary>
        /// 保存错误信息日志
        /// </summary>
        /// <param name="ex"></param>
        private static bool WriteBuglog(Exception ex)
        {
            var logContent = $"[{DateTime.Now:yyyy-MM-dd hh:mm:ss}]错误发生在：{ex.Source}，\r\n 内容：{ex.Message}";
            logContent += $"\r\n 跟踪：{ex.StackTrace}";
            return SetFile(@"BugLog.txt", logContent);
        }

        /// <summary>
        /// 保存错误信息日志
        /// </summary>
        /// <param name="ex"></param>
        private static bool WriteBuglog(string message, Exception ex)
        {
            var logContent = string.Format("[{0:yyyy-MM-dd hh:mm:ss}]错误发生在：{1}，\r\n 信息：{3}\r\n 内容：{2}", DateTime.Now, ex.Source, ex.Message, message);
            logContent += $"\r\n 跟踪：{ex.StackTrace}";
            return SetFile(@"BugLog.txt", logContent);
        }
        #endregion

        #region 通用操作

        /// <summary>
        /// 标准化写入过程，继承之后可自定义写入内容
        /// 默认保存在debug目录的Log目录下
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="logContent">写入内容</param>
        private static bool SetFile(string filename, string logContent)
        {
            lock (Mutex)
            {
                try
                {
                    Isexist(); // 判断Log目录是否存在
                    string errLogFilePath = Environment.CurrentDirectory + @"\Log\" + filename.Trim();// 获取当前目录地址
                    StreamWriter sw;
                    if (!File.Exists(errLogFilePath))
                    {
                        FileStream fs1 = new FileStream(errLogFilePath, FileMode.Create, FileAccess.Write);
                        sw = new StreamWriter(fs1);
                    }
                    else
                    {
                        sw = new StreamWriter(errLogFilePath, true);
                    }
                    sw.WriteLine(logContent);
                    sw.Flush();
                    sw.Close();
                    return true;
                }
                catch
                {
                    // 忽略错误
                    return false;
                }
            }
        }

        // 判断是否存在日志文件
        private static void Isexist()
        {
            string path = Environment.CurrentDirectory + @"\Log\";
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        #endregion

        #region 定期删除Log


        public Dispatcher UiThread = Dispatcher.CurrentDispatcher;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void DeleteLog()
        {
            ThreadStart thread = delegate
            {

                string errLogFilePath = Environment.CurrentDirectory + @"\Log\" + @"BugLog.txt";// 获取当前目录地址
                string logFilePath = Environment.CurrentDirectory + @"\Log\" + @"Log.txt";// 获取当前目录地址
                if (File.Exists(errLogFilePath))
                {
                    FileInfo info = new FileInfo(errLogFilePath);
                    if (info.Length > 3 * 1024)
                    {
                        try
                        {
                            File.Delete(errLogFilePath);
                            Log("删除错误日志文件");
                        }
                        catch (Exception ex)
                        {
                            Log("错误日志删除失败", ex);
                        }

                    }
                }

                if (File.Exists(logFilePath))
                {
                    FileInfo loginfo = new FileInfo(logFilePath);

                    if (loginfo.Length > 3 * 1024)
                    {
                        try
                        {
                            File.Delete(logFilePath);
                            Log("删除日常日志文件");
                        }
                        catch (Exception ex)
                        {
                            Log("日常日志删除失败", ex);
                        }
                    }
                }

            };
            new Thread(thread).Start();
        }


        #endregion
    }
}
