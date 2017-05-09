using Helper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WorkTime.BaseModel;
using WorkTime.Entity;

namespace WorkTime.ViewModel
{

    public class HomeViewModel:BaseViewModel
    {
        #region  声明

        public ICommand AddNewDataCommand => new AnotherCommandImplementation(AddNewData);
        public AutoCommand DeleteDataGridItemCommand => new AutoCommand(DeleteDataGridItem);
        public AutoCommand DataGridMouseDoubleClickCommand=>new AutoCommand(DataGridMouseDoubleClick);
        public AutoCommand CleanDataCommand => new AutoCommand(CleanData);
        public AutoCommand EditDataCommand => new AutoCommand(EditData);

        private WorkTimeData _newData = new WorkTimeData();

        private ObservableCollection<WorkTimeData_ViewData> _dataItems = new ObservableCollection<WorkTimeData_ViewData>();

        private ObservableCollection<string> _statusCollection;
        private DateTime _workDateTime;
        private string _type;
        private string _title;
        private string _detail;
        private string _status;
        private ObservableCollection<string> _typeCollection;
        private DateTime _beginTime=DateTime.Now;
        private DateTime _endTime=DateTime.Now;
        private WorkTimeData_ViewData _selecTimeDataViewData;
        private string _snackBarMessage;
        private Visibility _addVisibility=Visibility.Visible;
        private Visibility _editVisibility=Visibility.Collapsed;

        #endregion

        public Visibility AddVisibility
        {
            get { return _addVisibility; }
            set
            {
                this.MutateVerbose(ref _addVisibility, value, RaisePropertyChanged());

            }
        }

        public Visibility EditVisibility
        {
            get { return _editVisibility; }
            set
            {
                this.MutateVerbose(ref _editVisibility, value, RaisePropertyChanged());

            }
        }

        public ObservableCollection<WorkTimeData_ViewData> DataItems
        {
            get { return _dataItems; }
            set
            {
                this.MutateVerbose(ref _dataItems, value, RaisePropertyChanged());
            }
        }

        public WorkTimeData_ViewData SelecTimeDataViewData
        {
            get { return _selecTimeDataViewData; }
            set
            {
                this.MutateVerbose(ref _selecTimeDataViewData, value, RaisePropertyChanged());

            }
        }

        public string SnackBarMessage
        {
            get { return _snackBarMessage; }
            set
            {
                this.MutateVerbose(ref _snackBarMessage, value, RaisePropertyChanged());
            }
        }

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

        public DateTime Begin_time
        {
            get { return _beginTime; }
            set
            {
                this.MutateVerbose(ref _beginTime, value, RaisePropertyChanged());
                this.OnPropertyChanged("Spend");
            }
        }

        public DateTime End_time
        {
            get { return _endTime; }
            set
            {
                this.MutateVerbose(ref _endTime, value, RaisePropertyChanged());
                this.OnPropertyChanged("Spend");
            }
        }

        private string Spend {
            get
            {
                return
                    (long.Parse(Common.GetTimeSecond(End_time)) - (long.Parse(Common.GetTimeSecond(Begin_time))))
                    .ToString();
            }
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

            if (datasObject.code != 0)
            {

            }
            else
            {
                foreach (var item in datasObject.data)
                {
                    DataItems.Add(new WorkTimeData_ViewData(item));
                }
            }

            // 初始化
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


        }

        public void AddNewData(object o)
        {
            WorkTimeData postWorkTimeData =new WorkTimeData()
            {
                work_date = Common.GetTimeSecond(WorkDateTime),
                title = this.Title,
                detail = this.Detail,
                type = this.Type,
                state = this.Status,
                begin_time = Common.GetTimeSecond(this.Begin_time),
                end_time = Common.GetTimeSecond(this.End_time),
                spend = (long.Parse(Common.GetTimeSecond(this.End_time))- (long.Parse(Common.GetTimeSecond(this.Begin_time)))).ToString()
            };

            string temp = NetHelper.getProperties(postWorkTimeData);

            string add_url = "http://api.timemanager.online/time_manager/data/add?access_token=" + MainStaticData.AccessToken;

            var datas = NetHelper.HttpCall(add_url, temp, HttpEnum.Post);

            var returnData = JsonHelper.Deserialize<ReturnData<WorkTimeData>>(datas);
            if (returnData.code == 0)
            {
                DataItems.Add(new WorkTimeData_ViewData(returnData.data));

                Title = "";
                Detail = "";
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(500);
                }).ContinueWith(t =>
                {
                    ((UserContorller.Home)o).SnackbarOne.MessageQueue.Enqueue(returnData.message);
                }, TaskScheduler.FromCurrentSynchronizationContext());

            }


        }

        public void DeleteDataGridItem(object o)
        {
            Int64 id = SelecTimeDataViewData.GetID();
            string deleteData = "http://api.timemanager.online/time_manager/data/delete?access_token=" + MainStaticData.AccessToken+"&id="+ id;
            var datas = NetHelper.HttpCall(deleteData, null, HttpEnum.Get);

            var returnData = JsonHelper.Deserialize<ReturnData<object>>(datas);
            if (returnData.code == 0)
            {
                DataItems.Remove(SelecTimeDataViewData);
            }
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        public void DataGridMouseDoubleClick(object o)
        {
            this.Title = SelecTimeDataViewData.title;
            this.Detail = SelecTimeDataViewData.detail;
            this.Begin_time = Common.ReturnDateTime(SelecTimeDataViewData.GetBaseData().begin_time) ;
            this.End_time = Common.ReturnDateTime(SelecTimeDataViewData.GetBaseData().end_time) ;
            this.Type = SelecTimeDataViewData.type;
            this.WorkDateTime = Common.ReturnDateTime(SelecTimeDataViewData.GetBaseData().work_date);
            this.Status = SelecTimeDataViewData.state;


            this.AddVisibility=Visibility.Collapsed;
            this.EditVisibility = Visibility.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        public void CleanData(object o)
        {
            Title = "";
            Detail = "";

            this.AddVisibility = Visibility.Visible;
            this.EditVisibility = Visibility.Collapsed;

            WorkDateTime = DateTime.Now;
            Begin_time= DateTime.Now;
            End_time= DateTime.Now;

            Status = StatusCollection.First();
        }

        public void EditData(object o)
        {

        }



    }
}
