using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTime.BaseModel
{
    public  class WorkTimeData
    {
        public WorkTimeData()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public  Int64 __ID__ { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public  string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  string project { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  string work_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  string detail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  string begint_ime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  string end_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  string spend { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  string state { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  string text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  DateTime create_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  DateTime update_time { get; set; }

    }
}
