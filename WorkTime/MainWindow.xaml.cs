using System;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();

            Tips("Welcome to WorkTimeManager!");
        }


        #region  窗口动效事件

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

        private void MainWindow_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MainDialogHost.Margin = new Thickness(0);
                // Environment.Exit(0);
                WindowState = WindowState.Normal;
            }
            else
            {
                MainDialogHost.Margin = new Thickness(-9);
                WindowState = WindowState.Maximized;
            }

        }

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

            MessageTips("Test get MessageWindow !",sender,e);

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

        public async void MessageTips(string message,object sender, RoutedEventArgs e)
        {
            var sampleMessageDialog = new MessageDialog
            {
                Message = { Text = message }
            };

            await DialogHost.Show(sampleMessageDialog, "RootDialog");

        }

        /// <summary>
        /// Minsize 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseWindow_OnClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
