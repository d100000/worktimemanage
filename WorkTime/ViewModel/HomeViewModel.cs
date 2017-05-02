using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper;
using WorkTime.BaseModel;

namespace WorkTime.ViewModel
{

    public class HomeViewModel
    {
        private ObservableCollection<WorkTimeData> _dataItems;
    
        public ObservableCollection<WorkTimeData> DataItems
        {
            get; set;
        }


        public HomeViewModel()
        {
            string url = "http://api.timemanager.online/time_manager/user/login?name=bobdong&pw=bobdong";

            var ReturnDatastr = NetHelper.HttpCall(url, null, HttpEnum.Get);

            var ReturnDataObject = JsonHelper.Deserialize<ReturnData<User>>(ReturnDatastr);

            string access_token = ReturnDataObject.data.access_token;

            string get_data = "http://api.timemanager.online/time_manager/data/select?access_token=" + access_token;

            var datas = NetHelper.HttpCall(get_data, null, HttpEnum.Get);

            var datasObject = JsonHelper.Deserialize<ReturnData<ObservableCollection<WorkTimeData>>>(datas);
            DataItems = datasObject.data;
        }






    }
}
