using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Helper;
using WorkTime.BaseModel;

namespace WorkTime.ViewModel
{

    public class HomeViewModel:BaseViewModel
    {

        public ICommand AddNewDataCommand => new AnotherCommandImplementation(AddNewData);

        private WorkTimeData _newData;

        private ObservableCollection<WorkTimeData> _dataItems;

        public ObservableCollection<WorkTimeData> DataItems
        {
            get { return _dataItems; }
            set
            {
                this.MutateVerbose(ref _dataItems, value, RaisePropertyChanged());
            }
        }


        private ObservableCollection<string> _statusCollection;
        private DateTime _workDateTime;
        private string _type;
        private string _title;
        private string _detail;
        private string _status;
        private ObservableCollection<string> _typeCollection;
        private string _beginTime;
        private string _endTime;

        public ObservableCollection<string> StatusCollection
        {
            get { return _statusCollection; }
            set
            {
                this.MutateVerbose(ref _statusCollection, value, RaisePropertyChanged());
            }
        }

        public ObservableCollection<string> TypeCollection
        {
            get { return _typeCollection; }
            set
            {
                this.MutateVerbose(ref _typeCollection, value, RaisePropertyChanged());

            }
        }

        #region 新增数据

        public DateTime WorkDateTime
        {
            get { return _workDateTime; }
            set {  this.MutateVerbose(ref _workDateTime, value, RaisePropertyChanged()); }
        }

        public string Type
        {
            get { return _type; }
            set { this.MutateVerbose(ref _type, value, RaisePropertyChanged()); }
        }

        public string Title
        {
            get { return _title; }
            set { this.MutateVerbose(ref _title, value, RaisePropertyChanged()); }
        }

        public string Detail
        {
            get { return _detail; }
            set { this.MutateVerbose(ref _detail, value, RaisePropertyChanged()); }
        }

        public string Status
        {
            get { return _status; }
            set { this.MutateVerbose(ref _status, value, RaisePropertyChanged()); }
        }

        public string Begin_time
        {
            get { return _beginTime; }
            set { _beginTime = value; }
        }

        public string End_time
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        #endregion


        /// <summary>
        /// 新增数据
        /// </summary>
        public WorkTimeData NewData {
            get { return _newData; }
            set { _newData = value; }
        }

        public HomeViewModel()
        {

            string access_token = MainWindow.AccessToken;

            string get_data = "http://api.timemanager.online/time_manager/data/select?access_token=" + access_token;

            var datas = NetHelper.HttpCall(get_data, null, HttpEnum.Get);

            var datasObject = JsonHelper.Deserialize<ReturnData<ObservableCollection<WorkTimeData>>>(datas);
            DataItems = datasObject.data;
            inint();
        }
        /// <summary>
        /// 初始化，读取在线配置信息
        /// </summary>
        private void inint()
        {
            StatusCollection = MainStaticData.StstusCollection;

            TypeCollection = MainStaticData.TypeCollection;

            WorkDateTime=DateTime.Now;

            Status = StatusCollection.First();

            #region 注册点击事件




            #endregion 
        }

        public void AddNewData(object o)
        {
            WorkTimeData postWorkTimeData=new WorkTimeData()
            {
                work_date = WorkDateTime.ToLongDateString(),
                title = Title,
                detail = Detail,
                type = Type,
                state = Status,
                begint_time = Begin_time,
                end_time = End_time
            };

            string get_data = "http://api.timemanager.online/time_manager/data/select?access_token=" + MainStaticData.AccessToken;

            var datas = NetHelper.HttpCall(get_data, JsonHelper.Serialize(postWorkTimeData), HttpEnum.Post);

            string data = "";

        }



    }
}
