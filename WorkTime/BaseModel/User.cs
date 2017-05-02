using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTime.BaseModel
{
    public class User
    {

        public User()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public object name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int64 access_level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int64 user_level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int64 level_score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime expire_in { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime create_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string detail_info { get; set; }

    }
}
