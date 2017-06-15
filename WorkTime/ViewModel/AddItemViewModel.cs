using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Helper;
using MaterialDesignThemes.Wpf;
using WorkTime.BaseModel;
using WorkTime.Entity;
using WorkTime.Windows;

namespace WorkTime.ViewModel
{
    public class AddItemViewModel : BaseViewModel
    {

        #region 声明


        public ICommand AddTypeNameCommand => new AnotherCommandImplementation(AddTypeName);
        public ICommand DeleteItemCommand => new AnotherCommandImplementation(DeleteItem);

        public ICommand LoadedCommand => new AnotherCommandImplementation(Loaded);

        /// <summary>
        /// 选中的类型
        /// </summary>
        public string SelectType
        {
            get { return _selectType; }
            set
            {
                this.MutateVerbose(ref _selectType, value, RaisePropertyChanged());
                
            }

        }

        public string AddTypeText
        {
            get { return _addTypeText; }
            set
            {
                this.MutateVerbose(ref _addTypeText, value, RaisePropertyChanged());

            }

        }

        public ObservableCollection<string> TypeList
        {
            get { return _typeList; }
            set
            {
                this.MutateVerbose(ref _typeList, value, RaisePropertyChanged());

            }

        }

        Dispatcher Mainthread = Dispatcher.CurrentDispatcher;

        private ObservableCollection<string> _typeList = MainStaticData.TypeCollection;
        private string _selectType;
        private string _addTypeText;
        private object _thisControl;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public AddItemViewModel()
        {

        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        /// <param name="o"></param>
        public void Loaded(object o)
        {
            _thisControl = o;
        }

        /// <summary>
        /// 添加Type
        /// </summary>
        public void AddTypeName(object o)
        {
            if (this.AddTypeText==null||this.AddTypeText.Trim() == "")
            {
                MessageTips("空数据！");
                return;
            }
            // 添加数据

            var loadingDialog = new LoadingDialog();

            var result = DialogHost.Show(loadingDialog, "RootDialog", delegate (object sender, DialogOpenedEventArgs args)
            {
                string access_token = MainStaticData.AccessToken;

                string get_data = $"http://api.timemanager.online/time_manager/user/add_user_type?type_name={AddTypeText}&access_token=" + access_token;

                var datas = NetHelper.HttpCall(get_data, null, HttpEnum.Get);

                var datasObject = JsonHelper.Deserialize<ReturnData<object>>(datas);

                ThreadStart start = delegate ()
                {

                    Mainthread.BeginInvoke((Action)delegate ()// 异步更新界面
                    {
                        args.Session.Close(false);

                        if (datasObject.code != 0)
                        {
                            MessageTips(datasObject.message);
                            return;
                        }
                        else
                        {
                            TypeList.Add(AddTypeText);
                            AddTypeText = "";
                        }

                        
                    });

                };

                new Thread(start).Start(); // 启动线程

            });
        }

        /// <summary>
        /// 删除选中的数据
        /// </summary>
        /// <param name="o"></param>
        public void DeleteItem(object o)
        {
            if (this.SelectType == null || this.SelectType.Trim() == "")
            {
                MessageTips("空数据！");
                return;
            }
            // 添加数据

            var loadingDialog = new LoadingDialog();

            var result = DialogHost.Show(loadingDialog, "RootDialog", delegate (object sender, DialogOpenedEventArgs args)
            {
                string access_token = MainStaticData.AccessToken;

                string get_data = $"http://api.timemanager.online/time_manager/user/delete_user_type?type_name={SelectType}&access_token=" + access_token;

                var datas = NetHelper.HttpCall(get_data, null, HttpEnum.Get);

                var datasObject = JsonHelper.Deserialize<ReturnData<object>>(datas);

                ThreadStart start = delegate ()
                {

                    Mainthread.BeginInvoke((Action)delegate ()// 异步更新界面
                    {
                        args.Session.Close(false);

                        if (datasObject.code != 0)
                        {
                            MessageTips(datasObject.message);
                            return;
                        }
                        else
                        {
                            TypeList.Remove(SelectType);
                        }


                    });

                };

                new Thread(start).Start(); // 启动线程

            });
        }

        private void MessageShow(string message)
        {
            Task.Factory.StartNew(() =>
            {

            }).ContinueWith(t =>
            {
                ((UserContorller.AddItem)_thisControl).SnackbarOne.MessageQueue.Enqueue(message);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public async void MessageTips(string message)
        {
            var sampleMessageDialog = new MessageDialog
            {
                Message = { Text = message }
            };

            await DialogHost.Show(sampleMessageDialog, "RootDialog");

        }

    }
}
