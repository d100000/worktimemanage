using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace 定时任务.tips_window
{
    /// <summary>
    /// tips.xaml 的交互逻辑
    /// </summary>
    public partial class tips : Window
    {
        public tips()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        /// <summary>
        ///     移动窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveWindows_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
