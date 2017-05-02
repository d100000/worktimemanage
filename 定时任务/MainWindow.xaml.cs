using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using 定时任务.tips_window;
using Application = System.Windows.Application;
using ApplicationForm = System.Windows.Forms.Application;


namespace 定时任务
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        WindowState _wsl;
        
        static int hourTime = 3600000;

        System.Timers.Timer _t = new System.Timers.Timer(hourTime);

        NotifyIcon notifyIcon;

        tips _tips = new tips();

        public DateTime NextTime ;

        public MainWindow()
        {
            InitializeComponent();

            icon();
            //保证窗体显示在上方。
            _wsl = WindowState;

            SetComboxData();


            //实例化Timer类，设置间隔时间为10000毫秒；     

            _t.Elapsed += new System.Timers.ElapsedEventHandler(theout);

            //到达时间的时候执行事件；   

            _t.AutoReset = true;

            //设置是执行一次（false）还是一直执行(true)；     

            NextTime=DateTime.Now.AddSeconds((_t.Interval/1000));

            TipLable.Content= NextTime.ToString("HH:mm:ss");

            _t.Enabled = true;
            //需要调用 timer.Start()或者timer.Enabled = true来启动它， timer.Start()的内部原理还是设置timer.Enabled = true;  


        }

        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            new Thread(() => {
                this.Dispatcher.Invoke(new Action(() => {

                    NextTime = DateTime.Now.AddSeconds((_t.Interval / 1000));

                    TipLable.Content = NextTime.ToString("HH:mm:ss");

                    if (_tips.IsVisible == true)
                    {

                    }
                    else
                    {

                        _tips =new tips();
                        _tips.ShowDialog();
                    }

                }));
            }).Start();
        }

      

        #region 下拉列表选项

        public void SetComboxData()
        {
            List<string> ComboList =new List<string>()
            {
                "1小时提醒一次",
                "2小时提醒一次",
                "3小时提醒一次",
                "半小时提醒一次",
                "20秒提醒一次",
                "10秒提醒一次",
            };

            ComboBoxTime.ItemsSource= ComboList;
            ComboBoxTime.SelectedItem = "1小时提醒一次";
        }

        #endregion


        #region 线程提醒




        #endregion

        #region  开机自启

        private void WriteRegistry()
        {
            string strName = ApplicationForm.ExecutablePath;
            if (File.Exists(strName))
            {
                string strNewName = Path.GetFileName(strName);
                RegistryKey reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (reg == null)
                {
                    reg = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                }
                else
                {
                    if (reg.GetValue(strNewName) == null)
                    {
                        reg.SetValue(strNewName, strName);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 设置为开启自启
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WriteRegistry();
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void icon()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.BalloonTipText = "软件启动啦，么么哒！"; //设置程序启动时显示的文本

            this.notifyIcon.Text = "定时提醒小程序";//最小化到托盘时，鼠标点击时显示的文本

            this.notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);//程序图标
            this.notifyIcon.Visible = true;
            notifyIcon.MouseDoubleClick += OnNotifyIconDoubleClick;
            this.notifyIcon.ShowBalloonTip(1000);
        }
        private void OnNotifyIconDoubleClick(object sender, EventArgs e)
        {
            this.Show();
            WindowState = _wsl;
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void ComboBoxTime_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string time = (string)ComboBoxTime.SelectedItem;
            switch (time)
            {
                case "1小时提醒一次":
                    _t.Interval = hourTime;
                    break;
                case "2小时提醒一次":
                    _t.Interval = hourTime * 2;
                    break;
                case "3小时提醒一次":
                    _t.Interval = hourTime * 3;
                    break;
                case "10秒提醒一次":
                    _t.Interval = 10000;
                    break;
                case "半小时提醒一次":
                    _t.Interval = hourTime*0.5;
                    break;
                case "20秒提醒一次":
                    _t.Interval = 1000 * 30;
                    break;
            }
            _t.Enabled = false;
            NextTime = DateTime.Now.AddSeconds((_t.Interval / 1000));

            TipLable.Content = NextTime.ToString("HH:mm:ss");

            _t.Enabled = true;
        }
    }
}
