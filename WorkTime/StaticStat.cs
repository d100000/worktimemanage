using System.Collections.ObjectModel;
using Helper;
using WorkTime.BaseModel;

namespace WorkTime
{
    public static class MainStaticData
    {
        private static ObservableCollection<string> _typeCollection=new ObservableCollection<string>();
        /// <summary>
        /// 全局静态变量读取在线数据
        /// </summary>
        public static ObservableCollection<string> TypeCollection {
            get
            {
                if (_typeCollection.Count == 0)
                {
                    string getData = "https://api.bobdong.cn/time_manager/user/get_user_type?access_token=" + MainStaticData.AccessToken;

                    var datas = NetHelper.HttpCall(getData, null, HttpEnum.Get);
                    var datasObject = JsonHelper.Deserialize<ReturnData<ObservableCollection<string>>>(datas);
                    _typeCollection = datasObject.code == 0 ? datasObject.data : new ObservableCollection<string>();
                    if (_typeCollection.Count == 0)
                    {
                        _typeCollection=new ObservableCollection<string>()
                        {
                            "Common",
                            "Other",
                            "Work",
                        };
                    }
                }
                return _typeCollection;
            }
            set { _typeCollection = value; }
        }
        /// <summary>
        /// 全局在线状态变量
        /// </summary>
        public static ObservableCollection<string> StstusCollection =new ObservableCollection<string>()
        {
            "待确认",
            "进行中",
            "已完成",
            "已取消"
        };

        public static string AccessToken { get; set; }
    }
}
