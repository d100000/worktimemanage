using Helper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using WorkTime.BaseModel;
using WorkTime.Entity;
using WorkTime.Windows;

namespace WorkTime.ViewModel
{
    /// <summary>
    /// HomeViewModel 绑定UserControler -> Home.xaml Mvvm
    /// </summary>
    public class HomeViewModel:BaseViewModel
    {
        #region  声明

        public ICommand AddNewDataCommand => new AnotherCommandImplementation(AddNewData);
        public AutoCommand DeleteDataGridItemCommand => new AutoCommand(DeleteDataGridItem);
        public AutoCommand LoadedCommand => new AutoCommand(Loaded);
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
        private WorkTimeData_ViewData _editItemData;

        #endregion

        #region 属性设置

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
                this.OnPropertyChanged("DataItems");
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

        #endregion

        #region 新增数据

        public WorkTimeData_ViewData EditItemData
        {
            get { return _editItemData; }
            set { _editItemData = value; }
        }

        /// <summary>
        /// 工作日
        /// </summary>
        public DateTime WorkDateTime
        {
            get { return _workDateTime; }
            set {  this.MutateVerbose(ref _workDateTime, value, RaisePropertyChanged()); }
        }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string Type
        {
            get { return _type; }
            set { this.MutateVerbose(ref _type, value, RaisePropertyChanged()); }
        }

        /// <summary>
        /// 项目标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { this.MutateVerbose(ref _title, value, RaisePropertyChanged()); }
        }

        /// <summary>
        /// 项目明细
        /// </summary>
        public string Detail
        {
            get { return _detail; }
            set { this.MutateVerbose(ref _detail, value, RaisePropertyChanged()); }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { this.MutateVerbose(ref _status, value, RaisePropertyChanged()); }
        }

        /// <summary>
        /// 工作开始时间
        /// </summary>
        public DateTime Begin_time
        {
            get { return _beginTime; }
            set
            {
                this.MutateVerbose(ref _beginTime, value, RaisePropertyChanged());
                this.OnPropertyChanged("Spend");
            }
        }

        /// <summary>
        /// 工作结束时间
        /// </summary>
        public DateTime End_time
        {
            get { return _endTime; }
            set
            {
                this.MutateVerbose(ref _endTime, value, RaisePropertyChanged());
                this.OnPropertyChanged("Spend");
            }
        }

        /// <summary>
        /// 项目花费时间【单位：分钟】
        /// </summary>
        private string Spend {
            get
            {
                return
                    (long.Parse(Common.GetTimeSecond(End_time)) - (long.Parse(Common.GetTimeSecond(Begin_time))))
                    .ToString();
            }
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        public WorkTimeData NewData
        {
            get { return _newData; }
            set { _newData = value; }
        }

        #endregion

        #region 界面绑定方法

        Dispatcher Mainthread = Dispatcher.CurrentDispatcher;

        public HomeViewModel()
        {

        }

        public void Loaded(Object o)
        {
            Inint();
        }


        /// <summary>
        /// 初始化，读取在线配置信息
        /// </summary>
        private void Inint()
        {
            var loadingDialog = new LoadingDialog();

            var result = DialogHost.Show(loadingDialog, "RootDialog", delegate (object sender, DialogOpenedEventArgs args)
            {
                string access_token = MainStaticData.AccessToken;

                string get_data = "http://api.timemanager.online/time_manager/data/select?access_token=" + access_token;

                var datas = NetHelper.HttpCall(get_data, null, HttpEnum.Get);

                var datasObject = JsonHelper.Deserialize<ReturnData<ObservableCollection<WorkTimeData>>>(datas);

                ThreadStart start = delegate ()
                {

                    Mainthread.BeginInvoke((Action)delegate ()// 异步更新界面
                    {



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

                        StatusCollection = MainStaticData.StstusCollection;

                        TypeCollection = MainStaticData.TypeCollection;

                        WorkDateTime = DateTime.Now;

                        Status = StatusCollection.First();


                        args.Session.Close(false);
                    });

                };

                new Thread(start).Start(); // 启动线程

            });

        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="o"></param>
        public void AddNewData(object o)
        {
            WorkTimeData postWorkTimeData = new WorkTimeData()
            {
                work_date = Common.GetTimeSecond(WorkDateTime),
                title = this.Title,
                detail = this.Detail,
                type = this.Type,
                state = this.Status,
                begin_time = Common.GetTimeSecond(this.Begin_time),
                end_time = Common.GetTimeSecond(this.End_time),
                spend = (long.Parse(Common.GetTimeSecond(this.End_time)) - (long.Parse(Common.GetTimeSecond(this.Begin_time)))).ToString()
            };

            string temp = NetHelper.getProperties(postWorkTimeData);

            string addUrl = "http://api.timemanager.online/time_manager/data/add?access_token=" + MainStaticData.AccessToken;

            var datas = NetHelper.HttpCall(addUrl, temp, HttpEnum.Post);

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

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="o"></param>
        public void DeleteDataGridItem(object o)
        {
            Int64 id = SelecTimeDataViewData.GetID();
            string deleteData = "http://api.timemanager.online/time_manager/data/delete?access_token=" + MainStaticData.AccessToken + "&id=" + id;
            var datas = NetHelper.HttpCall(deleteData, null, HttpEnum.Get);

            var returnData = JsonHelper.Deserialize<ReturnData<object>>(datas);
            if (returnData.code == 0)
            {
                DataItems.Remove(SelecTimeDataViewData);
            }

        }

        /// <summary>
        /// 双击选中数据事件
        /// </summary>
        /// <param name="o"></param>
        public void DataGridMouseDoubleClick(object o)
        {
            EditItemData = SelecTimeDataViewData;

            this.Title = SelecTimeDataViewData.title;
            this.Detail = SelecTimeDataViewData.detail;
            this.Begin_time = Common.ReturnDateTime(SelecTimeDataViewData.GetBaseData().begin_time);
            this.End_time = Common.ReturnDateTime(SelecTimeDataViewData.GetBaseData().end_time);
            this.Type = SelecTimeDataViewData.type;
            this.WorkDateTime = Common.ReturnDateTime(SelecTimeDataViewData.GetBaseData().work_date);
            this.Status = SelecTimeDataViewData.state;


            this.AddVisibility = Visibility.Collapsed;
            this.EditVisibility = Visibility.Visible;
        }

        /// <summary>
        /// 清除输入框
        /// </summary>
        /// <param name="o"></param>
        public void CleanData(object o)
        {
            this.Title = "";
            this.Detail = "";

            this.AddVisibility = Visibility.Visible;
            this.EditVisibility = Visibility.Collapsed;

            this.WorkDateTime = DateTime.Now;
            this.Begin_time = DateTime.Now;
            this.End_time = DateTime.Now;

            this.Status = StatusCollection.First();
        }

        /// <summary>
        /// 编辑选择的数据
        /// </summary>
        /// <param name="o"></param>
        public void EditData(object o)
        {
            // update  data 
            EditItemData.title = Title;
            EditItemData.detail = Detail;
            EditItemData.work_date = Common.GetTimeSecond(WorkDateTime);
            EditItemData.type = this.Type;
            EditItemData.state = this.Status;
            EditItemData.begin_time = Common.GetTimeSecond(this.Begin_time);
            EditItemData.end_time = Common.GetTimeSecond(this.End_time);
            EditItemData.spend =
                (long.Parse(Common.GetTimeSecond(this.End_time)) - (long.Parse(Common.GetTimeSecond(this.Begin_time))))
                .ToString();

            for (int i = 0; i < DataItems.Count(); i++)
            {
                if (EditItemData.GetID() == DataItems[i].GetID())
                {
                    DataItems[i] = EditItemData;
                }
            }
        }

        #endregion

    }
}
