using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper;
using WorkTime.BaseModel;
using WorkTime.ViewModel;

namespace WorkTime.Entity
{
    public class WorkTimeData_ViewData: BaseViewModel
    {
        /// <summary>
        /// 原基础数据
        /// </summary>
        private WorkTimeData _workTimeData;



        public WorkTimeData_ViewData(WorkTimeData baseData)
        {
            _workTimeData = baseData;
        }

        public WorkTimeData_ViewData()
        {

        }

        public WorkTimeData GetBaseData()
        {
            return _workTimeData;
            
        }

        public void SetBaseData(WorkTimeData baseData)
        {
             _workTimeData= baseData;

        }

        public Int64 GetID()
        {
            return _workTimeData.__ID__;
        }

        public string title
        {
            get { return _workTimeData.title; }
            set
            {
                _workTimeData.title = value;
                this.OnPropertyChanged("title");
            }
        }

        public string work_date
        {
            get
            {
                return Common.ReturnDateTime(_workTimeData.work_date).ToShortDateString();

            }
            set
            {
                _workTimeData.work_date = (value);
                this.OnPropertyChanged("work_date");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string project
        {
            //get { return _workTimeData.project; }
            set
            {
                _workTimeData.project = value;
                this.OnPropertyChanged("project");

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string type
        {
            get { return _workTimeData.type; }
            set
            {
                _workTimeData.type = value;
                this.OnPropertyChanged("type");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string message
        {
            //get { return _workTimeData.message; }
            set
            {
                _workTimeData.message = value;
                this.OnPropertyChanged("message");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string work_info
        {
            //get { return _workTimeData.work_info; }
            set
            {
                _workTimeData.work_info = value;
                this.OnPropertyChanged("work_info");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string detail
        {
            get { return _workTimeData.detail; }
            set
            {
                _workTimeData.detail = value;
                this.OnPropertyChanged("detail");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string begin_time
        {
            get
            {
                return Common.ReturnDateTime(_workTimeData.begin_time).ToShortTimeString();

            }
            set
            {
                _workTimeData.begin_time = value;
                this.OnPropertyChanged("begin_time");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string end_time
        {
            get
            {
                return Common.ReturnDateTime(_workTimeData.end_time).ToShortTimeString();

            }
            set
            {
                _workTimeData.end_time = value;
                this.OnPropertyChanged("end_time");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string spend
        {
            get
            {
                if (_workTimeData.spend.Length < 1)
                {
                    return
                        ((long.Parse(_workTimeData.end_time) - long.Parse(_workTimeData.begin_time)) / 60).ToString() +
                        " Min";
                }
                else
                {
                    return (long.Parse(_workTimeData.spend) / 60).ToString() + " Min";
                }

            }
            set
            {
                _workTimeData.spend = value;
                this.OnPropertyChanged("spend");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string state
        {
            get { return _workTimeData.state; }
            set
            {
                _workTimeData.state = value;
                this.OnPropertyChanged("state");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string text
        {
            //get { return _workTimeData.text; }
            set
            {
                _workTimeData.text = value;
                this.OnPropertyChanged("text");
            }
        }





    }

}
