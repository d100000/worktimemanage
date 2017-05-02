using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTime.BaseModel
{
    public class ReturnData <T> where T:new()
    {
        public  int code { get; set; }

        public  string message { get; set; }

        public  T data { get; set; }
    }
}
