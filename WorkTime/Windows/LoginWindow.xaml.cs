using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Helper;
using MaterialDesignThemes.Wpf;
using WorkTime.BaseModel;

namespace WorkTime.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string pw = PasswordBox.Password;

            string url = $"http://api.timemanager.online/time_manager/user/login?name={name}&pw={pw}";

            var ReturnDatastr = NetHelper.HttpCall(url, null, HttpEnum.Get);

            var ReturnDataObject = JsonHelper.Deserialize<ReturnData<User>>(ReturnDatastr);

            if (ReturnDataObject.code != 0)
            {
                MessageTips(ReturnDataObject.message,sender,e);
            }
            else
            {
                MainWindow mainWindow=new MainWindow(ReturnDataObject.data.access_token);
                mainWindow.ShowDialog();
                Hide();
            }
        }

        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = "";
            PasswordBox.Password = "";
        }

        public async void MessageTips(string message, object sender, RoutedEventArgs e)
        {
            var sampleMessageDialog = new MessageDialog
            {
                Message = { Text = message }
            };

            await DialogHost.Show(sampleMessageDialog, "LoginDialog");

        }
    }
}
