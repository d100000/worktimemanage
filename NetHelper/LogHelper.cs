using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace Helper
{
    public static class LogHelper
    {
        private static readonly Queue LogQueue = new Queue();

        private static bool _isStreamClose = true;

        private static bool _isThreadBegin = false;

        private static StreamWriter _fileStreamWriter;

        private static readonly string fileName =@"BugLog.txt";

        static int _intervalTime = 10000;// 一分钟

        static System.Timers.Timer _timer = new System.Timers.Timer(_intervalTime);

        /// <summary>
        /// 添加日志队列
        /// </summary>
        /// <param name="message"></param>
        public static void AddLog(string message)
        {
            string logContent = $"[{DateTime.Now:yyyy-MM-dd hh:mm:ss}] =>{message}";
            LogQueue.Enqueue(logContent);
            if (!_isThreadBegin)
            {
                BeginThread();
            }
        }

        public static void AddLog(Exception ex)
        {
            var logContent = $"[{DateTime.Now:yyyy-MM-dd hh:mm:ss}]错误发生在：{ex.Source}，\r\n 内容：{ex.Message}";
            logContent += $"\r\n  跟踪：{ex.StackTrace}";
            LogQueue.Enqueue(logContent);
            if (!_isThreadBegin)
            {
                BeginThread();
            }
        }

        /// <summary>
        /// 读取日志队列的一条数据
        /// </summary>
        /// <returns></returns>
        private static object GetLog()
        {
            return LogQueue.Dequeue();
        }

        /// <summary>
        /// 开启定时查询线程
        /// </summary>
        public static void BeginThread()
        {
            _isThreadBegin = true;

            //实例化Timer类，设置间隔时间为10000毫秒；     

            _timer.Interval = _intervalTime;

            _timer.Elapsed += SetLog;

            //到达时间的时候执行事件；   

            _timer.AutoReset = true;

            //设置是执行一次（false）还是一直执行(true)；     

            _timer.Enabled = true;
        }


        /// <summary>
        /// 写入日志
        /// </summary>
        private static void SetLog(object source, System.Timers.ElapsedEventArgs e)
        {
            if (LogQueue.Count == 0)
            {
                if (_isStreamClose) return;
                _fileStreamWriter.Flush();
                _fileStreamWriter.Close();
                _isStreamClose = true;
                return;
            }
            if (_isStreamClose)
            {
                Isexist();
                string errLogFilePath = Environment.CurrentDirectory + @"\Log\" + fileName.Trim();
                if (!File.Exists(errLogFilePath))
                {
                    FileStream fs1 = new FileStream(errLogFilePath, FileMode.Create, FileAccess.Write);
                    _fileStreamWriter = new StreamWriter(fs1);
                }
                else
                {
                    _fileStreamWriter = new StreamWriter(errLogFilePath, true);
                }
                _isStreamClose = false;
            }

            var strLog = new StringBuilder();

            var onceTime = 50;

            var lineNum = LogQueue.Count > onceTime ? onceTime : LogQueue.Count;

            for (var i = 0; i < lineNum; i++)
            {
                strLog.AppendLine(GetLog().ToString());
            }

            _fileStreamWriter.WriteLine(strLog.ToString());

        }

        /// <summary>
        /// 判断是否存在日志文件
        /// </summary>
        private static void Isexist()
        {
            string path = Environment.CurrentDirectory + @"\Log\";
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
