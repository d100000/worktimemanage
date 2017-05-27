using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Update
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// UI 线程
        /// </summary>
        Dispatcher Mainthread = Dispatcher.CurrentDispatcher;

        private XmlDocument XmlDoc;

        private VersionData VersionData;

        private readonly string ThisPath;

        private string TmpPath;

        private readonly string VsrsionDataPath;

        private string LocalVersionData;

        private string NewVersionData;

        private string AppName;

        private string NewDownloadUrl;

        public MainWindow()
        {
            InitializeComponent();

            ThisPath = AppDomain.CurrentDomain.BaseDirectory;// 当前目录

            AppName = "WorkTime";

            VsrsionDataPath = ThisPath + "VersionData.json";

            TmpPath = "tmp\\";

            XmlDoc= new XmlDocument();

            if (!File.Exists(VsrsionDataPath))
            {
                DetailText.Text = "配置文件缺失！";
                NewVersion.Text = "配置文件缺失！";
            }
            else
            {
                using (StreamReader sr = new StreamReader(VsrsionDataPath))
                {
                    try
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Converters.Add(new JavaScriptDateTimeConverter());
                        serializer.NullValueHandling = NullValueHandling.Ignore;

                        //构建Json.net的读取流  
                        JsonReader reader = new JsonTextReader(sr);
                        //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                        var version = serializer.Deserialize<VersionData>(reader);
                        LocalVersion.Text = version.Version;
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                    }
                }

                Init();
            }
        }

        public void Init()
        {
            ThreadStart start = delegate ()
            {
                string url = $"http://api.timemanager.online/time_manager/system/get_system_data";

                var returnDatastr = NetHelper.HttpCall(url, null, HttpEnum.Get);

                var returnDataObject = JsonHelper.Deserialize<ReturnData<VersionData>>(returnDatastr);

                VersionData = returnDataObject.data;

                Mainthread.BeginInvoke((Action)delegate ()// 异步更新界面
                {
                    if (returnDataObject.code != 0)
                    {

                    }
                    else
                    {
                        NewVersion.Text = returnDataObject.data.Version;
                        DetailText.Text = returnDataObject.data.Message;
                        NewDownloadUrl = returnDataObject.data.DownLoadUrl;
                        if (NewVersion.Text != LocalVersion.Text)
                        {
                            UpdateButton.IsEnabled = true;
                        }
                    }
                    // 线程结束后的操作
                });

            };

            new Thread(start).Start(); // 启动线程

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateButton.IsEnabled = false;

            try
            {
                WebRequest request = WebRequest.Create(NewDownloadUrl);
                WebResponse respone = request.GetResponse();
                PbProgress.Maximum = respone.ContentLength;
                if (!Directory.Exists(ThisPath+TmpPath))
                {
                    Directory.CreateDirectory(ThisPath+ TmpPath);
                }
                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    Stream netStream = respone.GetResponseStream();
                    Stream fileStream = new FileStream(ThisPath+ TmpPath + "app.zip", FileMode.Create);
                    byte[] read = new byte[1024];
                    long progressBarValue = 0;
                    int realReadLen = netStream.Read(read, 0, read.Length);
                    while (realReadLen > 0)
                    {
                        fileStream.Write(read, 0, realReadLen);
                        progressBarValue += realReadLen;
                        PbProgress.Dispatcher.BeginInvoke(new ProgressBarSetter(SetProgressBar), progressBarValue);
                        realReadLen = netStream.Read(read, 0, read.Length);
                    }
                    netStream.Close();
                    fileStream.Close();
                    Mainthread.BeginInvoke(new Action(() =>
                    {
                        //BtOk.IsEnabled = true;

                        //BtOk.Content = "安装新版";
                        DetailText.AppendText($@"文件app.zip下载完成！" + "\r\n 准备安装……");
                        ReplaceOperation();

                    }));
                }, null);
            }
            catch (Exception ex)
            {
                Mainthread.BeginInvoke(new Action(() =>
                {
                    DetailText.AppendText("更新失败！\r\n" + ex.Message + "\r\n" + ex.InnerException);
                }));
            }
        }
        private bool ReplaceFile()// 替换文件
        {

            if (!OpreateMainForm(true))
            {
                return false;
            }
            try
            {
                string fileName = "app.zip";
                if (fileName.Substring(fileName.LastIndexOf('.')).StartsWith(".zip"))
                {
                    Mainthread.BeginInvoke(new Action(() =>
                    {
                        DetailText.AppendText("正在解压替换" + "\r\n");
                    }));

                    UnZipClass.UnZip(TmpPath + fileName, ThisPath);// 程序的压缩文件，解压操作

                    Mainthread.BeginInvoke(new Action(() =>
                    {
                        DetailText.AppendText("替换完成正在删除临时目录" + "\r\n");
                    }));

                    Directory.Delete(TmpPath, true); // 删除临时目录;

                }
                SaveLocalXml();
                return true;
            }
            catch (Exception ex)
            {
                Mainthread.BeginInvoke(new Action(() =>
                {
                    DetailText.AppendText("解压替换文件失败！\r\n" + ex.Message + "\r\n" + ex.InnerException);
                }));
                return false;
            }
        }

        private void SaveLocalXml()// 替换Xml文件
        {
            try
            {
                string JsonString = "VersionData.json";
                string JsonData = JsonHelper.Serialize(VersionData);
                XMLHelper xmlHelper=new XMLHelper();
                xmlHelper.WriteJson_string(ThisPath+JsonString, JsonData);

            }
            catch (Exception ex)
            {
                Mainthread.BeginInvoke(new Action(() =>
                {
                    LocalVersion.Text = VersionData.Version;
                    DetailText.Text = "替换文件失败！\r\n" + ex.Message + "\r\n" + ex.InnerException; ;
                }));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReplaceOperation()
        {
            OpreateMainForm(true);
            bool isReplace = false;
            ThreadStart thread = delegate
            {
                Func<bool> fu = ReplaceFile;
                isReplace = fu();
                if (!isReplace)
                {
                    Mainthread.BeginInvoke(new Action(() =>
                    {
                        DetailText.AppendText("正在重试！");
                    }));
                    Thread.Sleep(3000);
                    isReplace = fu();
                }
                if (isReplace)
                {
                    Mainthread.BeginInvoke(new Action(() =>
                    {
                        OpreateMainForm(false);
                    }));

                }
                else
                {
                    Mainthread.BeginInvoke(new Action(() =>
                    {
                        //BtCancel.IsEnabled = true;
                        //BtOk.IsEnabled = true;
                        UpdateButton.IsEnabled = true;
                        DetailText.AppendText("请重试！");
                    }));
                    return;
                }
                Thread.Sleep(3000);
                Mainthread.BeginInvoke(new Action(() =>
                {
                    if (isReplace)
                        Environment.Exit(0);
                }));
            };
            try
            {
                new Thread(thread).Start();
            }
            catch (Exception ex)
            {
                Mainthread.BeginInvoke(new Action(() =>
                {
                    DetailText.AppendText("安装文件失败！\r\n" + ex.Message + "\r\n" + ex.InnerException);
                }));
            }
        }

        private bool OpreateMainForm(bool IsClose)// 关闭或打开主程序
        {
            if (IsClose)
            {
                System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process p in ps)
                {
                    if (p.ProcessName.ToUpper() == AppName.ToUpper() ||
                        p.ProcessName.ToUpper() == (AppName + ".vshost").ToUpper())
                    {
                        try
                        {
                            p.Kill();
                        }
                        catch (Exception ex)
                        {
                            Mainthread.BeginInvoke(new Action(() =>
                            {
                                DetailText.Text = "关闭主程序失败！\r\n" + ex.Message + "\r\n" + ex.InnerException;
                            }));
                            return false;
                        }
                    }
                }
            }
            else
            {
                try
                {
                    System.Diagnostics.Process.Start(AppName + ".exe");
                }
                catch (Exception e)
                {
                    DetailText.Text = "\r\n无法自动启动软件，请手动开启！更新程序将自动关闭！";
                    return false;
                }

            }

            return true;
        }

        #region 辅助
        public delegate void ProgressBarSetter(double value);
        public void SetProgressBar(double value)
        {
            PbProgress.Value = value;
        }
        #endregion

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }

    public class VersionData
    {
        public string Version;
        public string Date;
        public string DownLoadUrl;
        public string Message;

    }
    public class ReturnData<T> where T : new()
    {
        public int code { get; set; }

        public string message { get; set; }

        public T data { get; set; }
    }
}
