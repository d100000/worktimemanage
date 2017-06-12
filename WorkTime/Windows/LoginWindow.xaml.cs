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

        Dispatcher Mainthread = Dispatcher.CurrentDispatcher;

        private double AnimationTime = 0.7;

        public LoginWindow()
        {
            InitializeComponent();

        }
        

        private void Login_click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string pw = PasswordBox.Password;

            ShowLoadingDialog(name, pw);

        }

        private void SignIn_click(object sender, RoutedEventArgs e)
        {

            GridOperation.Fade_Out_Grid(LoginGrid, AnimationTime);
            GridOperation.Fade_Int_Grid(SignInGrid, AnimationTime);


        }

        public async void MessageTips(string message)
        {
            var sampleMessageDialog = new MessageDialog
            {
                Message = { Text = message }
            };
            
            await DialogHost.Show(sampleMessageDialog, "LoginDialog");
            
        }

        private  void ShowLoadingDialog(string name,string pw)
        {
            var loadingDialog = new LoadingDialog();

            var result =  DialogHost.Show(loadingDialog, "LoginDialog", delegate (object sender, DialogOpenedEventArgs args)
            {

                ThreadStart start = delegate()
                {
                    string url = $"http://api.timemanager.online/time_manager/user/login?name={name}&pw={pw}";

                    var ReturnDatastr = NetHelper.HttpCall(url, null, HttpEnum.Get);

                    var ReturnDataObject = JsonHelper.Deserialize<ReturnData<User>>(ReturnDatastr);

                    Mainthread.BeginInvoke((Action)delegate ()// 异步更新界面
                    {
                        
                        args.Session.Close(false);
                        if (ReturnDataObject.code != 0)
                        {
                            MessageTips(ReturnDataObject.message);
                        }
                        else
                        {
                            MainStaticData.AccessToken = ReturnDataObject.data.access_token;
                            Close();
                        }
                        // 线程结束后的操作
                    });

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

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/d100000/worktimemanage");  
        }

        private void BackToLogin_OnClick(object sender, RoutedEventArgs e)
        {
            GridOperation.Fade_Out_Grid(SignInGrid, AnimationTime);
            GridOperation.Fade_Int_Grid(LoginGrid, AnimationTime);
        }
        
        /// <summary>
        /// 注册新账号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SignInNew_OnClick(object sender, RoutedEventArgs e)
        {
            if (SignInPassWord.Password != SignInConfimPassWord.Password)
            {
                MessageTips("密码不一致！");
                return;
            }

            string name = SignInUserName.Text;
            string pw = SignInPassWord.Password;

            var loadingDialog = new LoadingDialog();

            var result = DialogHost.Show(loadingDialog, "LoginDialog", delegate (object senders, DialogOpenedEventArgs args)
            {

                ThreadStart start = delegate ()
                {
                    string url = $"http://api.timemanager.online/time_manager/user/register?name={name}&pw={pw}";

                    var ReturnDatastr = NetHelper.HttpCall(url, null, HttpEnum.Get);

                    var ReturnDataObject = JsonHelper.Deserialize<ReturnData<User>>(ReturnDatastr);

                    Mainthread.BeginInvoke((Action)delegate ()// 异步更新界面
                    {

                        args.Session.Close(false);
                        if (ReturnDataObject.code != 0)
                        {
                            MessageTips(ReturnDataObject.message);
                        }
                        else
                        {
                            MainStaticData.AccessToken = ReturnDataObject.data.access_token;
                            Close();
                        }
                        // 线程结束后的操作
                    });

                };

                new Thread(start).Start(); // 启动线程

            });
        }
    }
}
