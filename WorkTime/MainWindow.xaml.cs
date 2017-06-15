using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using WorkTime.ViewModel;
using WorkTime.Windows;

namespace WorkTime
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public static string AccessToken = "";

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
            //Tips("Welcome to WorkTimeManager!");

        }

        public void Init()
        {

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory+"app.zip"))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory+"app.zip"); // 删除临时目录;
            }
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
}
