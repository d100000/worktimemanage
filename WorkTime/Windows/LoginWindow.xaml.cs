using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using Helper;
using MaterialDesignThemes.Wpf;
using WorkTime.BaseModel;

namespace WorkTime.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow 
    {

        public LoginWindow()
        {
            InitializeComponent();

        }
        

        private void Login_click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string pw = PasswordBox.Password;

            ShowLoadingDialog(name,pw);
        }

        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = "";
            PasswordBox.Password = "";
        }

        public async void MessageTips(string message)
        {
            var sampleMessageDialog = new MessageDialog
            {
                Message = { Text = message }
            };
            
            await DialogHost.Show(sampleMessageDialog, "LoginDialog");
            
        }

        private async void ShowLoadingDialog(string name,string pw)
        {
            var loadingDialog = new LoadingDialog();

            var result = await DialogHost.Show(loadingDialog, "LoginDialog", delegate (object sender, DialogOpenedEventArgs args)
            {
                Dispatcher Mainthread = Dispatcher.CurrentDispatcher;

                ThreadStart start = delegate()
                {
                    string url = $"http://api.timemanager.online/time_manager/user/login?name={name}&pw={pw}";

                    Thread.Sleep(3000);

                    var ReturnDatastr = NetHelper.HttpCall(url, null, HttpEnum.Get);

                    var ReturnDataObject = JsonHelper.Deserialize<ReturnData<User>>(ReturnDatastr);

                    Mainthread.BeginInvoke(new Action(() =>// 异步更新界面
                    {
                        
                        args.Session.Close(false);
                        if (ReturnDataObject.code != 0)
                        {
                            MessageTips(ReturnDataObject.message);
                        }
                        else
                        {
                            MainStaticData.AccessToken = ReturnDataObject.data.access_token;
                            MainWindow mainWindow = new MainWindow(ReturnDataObject.data.access_token);
                            Hide();
                            mainWindow.ShowDialog();
                            Close();
                        }
                        // 线程结束后的操作
                    }), DispatcherPriority.Normal);

                };

                new Thread(start).Start(); // 启动线程

            });

        }

        private void LoginWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key==Key.Enter)
            //{
            //    this.Login_click(sender,e);
            //}
        }

        private void PasswordBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Login_click(sender, e);
            }
        }
    }
}
