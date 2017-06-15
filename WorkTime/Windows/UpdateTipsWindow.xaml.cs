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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Helper;

namespace WorkTime.Windows
{
    /// <summary>
    /// UpdateTipsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateTipsWindow : Window
    {
        public UpdateTipsWindow(string title,string data)
        {
            InitializeComponent();

            TbTitle.Text = title;
            TbData.Text = data;
        }



        private void Win_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Left = SystemParameters.WorkArea.Width - Window.Width;
            Window.Top = SystemParameters.WorkArea.Height - Window.Height;
            Get();
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("Update" + ".exe");
            }
            catch (Exception ex)
            {
                LogHelper.AddLog(ex);
            }finally
            {
                End();
            }
        
        }


        public void Get()// 淡入
        {

            DoubleAnimation da = new DoubleAnimation
            {
                From = 0,//起始值
                To = 1,//结束值
                Duration = TimeSpan.FromSeconds(1)//动画持续时间
            };
            Window.BeginAnimation(OpacityProperty, da);//开始动画
        }

        public void End()// 淡出
        {

            DoubleAnimation da = new DoubleAnimation
            {
                From = 1,//起始值
                To = 0,//结束值
                Duration = TimeSpan.FromSeconds(1)//动画持续时间
            };
            Window.BeginAnimation(OpacityProperty, da);//开始动画

            Dispatcher uiThread = Dispatcher.CurrentDispatcher;// 获取主线程
            ThreadStart thread = delegate
            {
                Thread.Sleep(1000);
                uiThread.BeginInvoke(new Action(Close));
            };
            new Thread(thread).Start();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            End();
        }
    }
}
