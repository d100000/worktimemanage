using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Helper;
using WorkTime.BaseModel;

namespace WorkTime.ViewModel
{

    public class HomeViewModel
    {

        public ICommand AddNewDataCommand;



        private ObservableCollection<WorkTimeData> _dataItems;

        public ObservableCollection<WorkTimeData> DataItems
        {
            get { return _dataItems; }
            set { _dataItems = value; }
        }


        public HomeViewModel()
        {

            string access_token = MainWindow.AccessToken;

            string get_data = "http://api.timemanager.online/time_manager/data/select?access_token=" + access_token;

            var datas = NetHelper.HttpCall(get_data, null, HttpEnum.Get);

            var datasObject = JsonHelper.Deserialize<ReturnData<ObservableCollection<WorkTimeData>>>(datas);
            DataItems = datasObject.data;
        }

        public void AddNewData(object o)
        {


        }


    }
}
