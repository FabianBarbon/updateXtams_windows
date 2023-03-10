using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtamStateSystemService.Models
{
    class SendUpdate
    {
        public bool status { get; set; }
        public string ip { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public object folder { get; set; }
        public string so { get; set; }
        public string vsion { get; set; }

        public object description { get; set; }

    }

 
}
