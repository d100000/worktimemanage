using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Helper;
using MaterialDesignThemes.Wpf;
using WorkTime.BaseModel;
using WorkTime.Entity;
using WorkTime.ViewModel;
using WorkTime.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WorkTime
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public static string AccessToken = "";

        private string localVersion;

        Dispatcher Mainthread = Dispatcher.CurrentDispatcher;

        public MainWindow()
        {
            // 初始化登录窗口
            LoginWindow loginWindowDialog = new LoginWindow();

            loginWindowDialog.ShowDialog();
            if (string.IsNullOrEmpty(MainStaticData.AccessToken))
            {
                Close();
            }
            else
            {
                InitializeComponent();

                DataContext = new MainWindowViewModel();

            }
            Init();
            CheckUpdate();
            //Tips("Welcome to WorkTimeManager!");

        }

        public void Init()
        {

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "app.zip"))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "app.zip"); // 删除临时目录;
            }
        }

        /// <summary>
        /// 检查更新
        /// </summary>
        public void CheckUpdate()
        {
            ThreadStart start = delegate ()
            {

                Thread.Sleep(3000);

                string url = $"https://api.bobdong.cn/time_manager/system/get_system_data";

                var returnDatastr = NetHelper.HttpCall(url, null, HttpEnum.Get);

                var returnDataObject = JsonHelper.Deserialize<ReturnData<VersionData>>(returnDatastr);

                var VersionData = returnDataObject.data;

                var ThisPath = AppDomain.CurrentDomain.BaseDirectory;// 当前目录

                var VsrsionDataPath = ThisPath + "VersionData.json";

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
                        localVersion = version.Version;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.AddLog(ex);
                        return;
                    }
                }

                Mainthread.BeginInvoke((Action)delegate ()// 异步更新界面
                {
                    if (returnDataObject.code != 0)
                    {

                    }
                    else
                    {
                        StringBuilder stb = new StringBuilder();
                        stb.AppendLine("版本更新");
                        stb.AppendLine($@"当前版本:{localVersion}");
                        stb.AppendLine($@"最新版本:{VersionData.Version}");
                        stb.AppendLine("请选择是否要更新");
                        if (localVersion != VersionData.Version)
                        {
                            UpdateTipsWindow update = new UpdateTipsWindow("更新提示", stb.ToString());
                            update.Show();

                        }

                    }
                    // 线程结束后的操作
                });

            };

            new Thread(start).Start(); // 启动线程
        }

        #region  窗口动效事件



        #endregion

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            MenuToggleButton.IsChecked = false;

        }

        public void SetAccessToken(string accessToken)
        {

            AccessToken = accessToken;
        }


        /// <summary>
        /// 500毫秒后提醒
        /// </summary>
        /// <param name="message"></param>
        public void Tips(string message)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
            }).ContinueWith(t =>
            {
                MainSnackbar.MessageQueue.Enqueue(message);
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        public async void MessageTips(string message, object sender, RoutedEventArgs e)
        {
            var sampleMessageDialog = new MessageDialog
            {
                Message = { Text = message }
            };

            await DialogHost.Show(sampleMessageDialog, "RootDialog");

        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("Update" + ".exe");
            }
            catch (Exception ex)
            {

            }
        }
    }

    public class VersionData
    {
        public string Version;
        public string Date;
        public string DownLoadUrl;
        public string Message;

    }
}
