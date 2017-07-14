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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Helper;


namespace WorkTimeTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            test_net();

        }

        public void test_net()
        {
            string url = "https://api.bobdong.cn/time_manager/user/login?name=bobdong&pw=bobdong";



            var ReturnDatastr = NetHelper.HttpCall(url,null,HttpEnum.Get);

            var ReturnDataObject = JsonHelper.Deserialize<ReturnData<User>>(ReturnDatastr);

            string access_token = ReturnDataObject.data.access_token;

            string get_data = "https://api.bobdong.cn/time_manager/data/select?access_token="+access_token;

            var datas = NetHelper.HttpCall(get_data, null, HttpEnum.Get);

            var datasObject = JsonHelper.Deserialize<ReturnData<List<WorkTimeData>>>(datas);

            var tt = "";

        }




    }

    public class ReturnData<T> where T :new ()
    {
        public int code;
        public string message;
        public T data;
    }

    public class User {

        public User() {

        }

        /// <summary>
        /// 
        /// </summary>
        public object name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int64 access_level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int64 user_level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int64 level_score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime expire_in { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime create_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string detail_info { get; set; }

    }

    class WorkTimeData
    {
        public WorkTimeData()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public Int64 __ID__ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string project { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string work_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string detail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string begint_ime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string end_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string spend { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime create_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime update_time { get; set; }

    }
}
