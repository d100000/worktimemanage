using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTime.BaseModel
{
    public  class WorkTimeData
    {
        private DateTime _createTime;
        private DateTime _updateTime;
        private long _id;
        private string _title;
        private string _project;
        private string _type;
        private string _message;
        private string _workInfo;
        private string _detail;
        private string _begintIme;
        private string _endTime;
        private string _spend;
        private string _state;
        private string _text;
        private string _workDate;

        public WorkTimeData()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public Int64 __ID__
        {
            //get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string work_date
        {
            get { return _workDate; }
            set { _workDate = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string project
        {
            get { return _project; }
            set { _project = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string work_info
        {
            get { return _workInfo; }
            set { _workInfo = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string detail
        {
            get { return _detail; }
            set { _detail = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string begint_time
        {
            get { return _begintIme; }
            set { _begintIme = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string end_time
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string spend
        {
            get { return _spend; }
            set { _spend = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string state
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime create_time
        {
            //get { return _createTime; }
            set { _createTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime update_time
        {
            //get { return _updateTime; }
            set { _updateTime = value; }
        }
    }
}
