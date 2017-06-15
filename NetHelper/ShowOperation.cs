using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Helper
{
    public class ShowOperation
    {
        /// <summary>
        /// 界面淡入【一秒】
        /// </summary>
        /// <param name="control"></param>
        public static void Fade_Int_Window(System.Windows.Controls.Control control)
        {
            control.Opacity = 0;
            DoubleAnimation da = new DoubleAnimation
            {
                From = 0,//起始值
                To = 1,//结束值
                Duration = TimeSpan.FromSeconds(1)//动画持续时间
            };
            control.BeginAnimation(UIElement.OpacityProperty, da);//开始动画
        }

        /// <summary>
        /// 界面淡入
        /// </summary>
        /// <param name="control"></param>
        /// <param name="second">淡入时间</param>
        public static void Fade_Int_Window(System.Windows.Controls.Control control, double second)
        {
            control.Opacity = 1;
            DoubleAnimation da = new DoubleAnimation
            {
                From = 0,//起始值
                To = 1,//结束值
                Duration = TimeSpan.FromSeconds(second)//动画持续时间
            };
            control.BeginAnimation(UIElement.OpacityProperty, da);//开始动画
        }

        /// <summary>
        /// 界面淡出
        /// </summary>
        /// <param name="window"></param>
        /// <param name="second">淡出时间</param>
        public static void Fade_Out_Window(System.Windows.Controls.Control window, double second)
        {
            DoubleAnimation da = new DoubleAnimation
            {
                From = 1,//起始值
                To = 0,//结束值
                Duration = TimeSpan.FromSeconds(second)//动画持续时间
            };
            window.BeginAnimation(UIElement.OpacityProperty, da);//开始动画
        }

    }

    public class GridOperation
    {
        /// <summary>
        /// 设置Grid 的长度属性动画
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void ShowGridHeight(Grid grid, double from, double to)
        {
            grid.Height = from;
            DoubleAnimation da = new DoubleAnimation
            {
                From = from,//起始值
                To = to,//结束值
                Duration = TimeSpan.FromSeconds(0.2)//动画持续时间
            };
            grid.BeginAnimation(FrameworkElement.HeightProperty, da);//开始动画
        }

        /// <summary>
        /// 关闭高度时的动画
        /// </summary>
        /// <param name="grid"></param>
        public static void CloseGridHeight(Grid grid)
        {
            DoubleAnimation da = new DoubleAnimation
            {
                From = grid.ActualHeight,//起始值
                To = 0,//结束值
                Duration = TimeSpan.FromSeconds(0.2)//动画持续时间
            };
            grid.BeginAnimation(FrameworkElement.HeightProperty, da);//开始动画
        }

        /// <summary>
        /// 设置Grid 的长度属性动画
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void ShowHeight(FrameworkElement grid, double from, double to)
        {
            grid.Height = from;
            DoubleAnimation da = new DoubleAnimation
            {
                From = from,//起始值
                To = to,//结束值
                Duration = TimeSpan.FromSeconds(0.2)//动画持续时间
            };
            grid.BeginAnimation(FrameworkElement.HeightProperty, da);//开始动画
        }

        /// <summary>
        /// 关闭高度时的动画
        /// </summary>
        /// <param name="grid"></param>
        public static void CloseHeight(FrameworkElement grid)
        {
            DoubleAnimation da = new DoubleAnimation
            {
                From = grid.ActualHeight,//起始值
                To = 0,//结束值
                Duration = TimeSpan.FromSeconds(0.2)//动画持续时间
            };
            grid.BeginAnimation(FrameworkElement.HeightProperty, da);//开始动画
        }

        /// <summary>
        /// 界面淡入
        /// </summary>
        /// <param name="control"></param>
        /// <param name="second">淡入时间</param>
        public static void Fade_Int_Grid(FrameworkElement control, double second)
        {
            control.Visibility=Visibility.Visible;
            control.IsEnabled = true;
 
            DoubleAnimation da = new DoubleAnimation
            {
                From = 0,//起始值
                To = 1,//结束值
                Duration = TimeSpan.FromSeconds(second)//动画持续时间
            };
            control.BeginAnimation(UIElement.OpacityProperty, da);//开始动画
        }

        /// <summary>
        /// 界面淡出
        /// </summary>
        /// <param name="window"></param>
        /// <param name="second">淡出时间</param>
        public static void Fade_Out_Grid(FrameworkElement window, double second)
        {
            window.IsEnabled = false;
            Dispatcher mainthread = Dispatcher.CurrentDispatcher;
            DoubleAnimation da = new DoubleAnimation
            {
                From = 1,//起始值
                To = 0,//结束值
                Duration = TimeSpan.FromSeconds(second)//动画持续时间
            };
            window.BeginAnimation(UIElement.OpacityProperty, da);//开始动画

            ThreadStart start = delegate
            {
                Thread.Sleep((int)(second * 1000));
                mainthread.BeginInvoke((Action)delegate // 异步更新界面
                {
                    window.Visibility = Visibility.Hidden;
                    // 线程结束后的操作
                });
            };
            new Thread(start).Start(); // 启动线程

        }
    }
}
