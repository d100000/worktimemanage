using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTime.BaseModel;

namespace WorkTime.Entity
{
    class WorkTimeData_ViewData
    {
        /// <summary>
        /// 原基础数据
        /// </summary>
        private WorkTimeData _workTimeData;

        public WorkTimeData_ViewData(WorkTimeData baseData)
        {
            _workTimeData = baseData;
        }

        public string title
        {
            get { return _workTimeData.title; }
            set { _workTimeData.title = value; }
        }

        public string work_date
        {
            get { return _workTimeData.work_date; }
            set { _workTimeData.work_date = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string project
        {
            get { return _workTimeData.project; }
            set { _workTimeData.project = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string type
        {
            get { return _workTimeData.type; }
            set { _workTimeData.type = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string message
        {
            get { return _workTimeData.message; }
            set { _workTimeData.message = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string work_info
        {
            get { return _workTimeData.work_info; }
            set { _workTimeData.work_info = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string detail
        {
            get { return _workTimeData.detail; }
            set { _workTimeData.detail = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string begint_time
        {
            get { return _workTimeData.begint_time; }
            set { _workTimeData.begint_time = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string end_time
        {
            get { return _workTimeData.end_time; }
            set { _workTimeData.end_time = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string spend
        {
            get { return _workTimeData.spend; }
            set { _workTimeData.spend = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string state
        {
            get { return _workTimeData.state; }
            set { _workTimeData.state = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string text
        {
            get { return _workTimeData.text; }
            set { _workTimeData.text = value; }
        }






    }

}
