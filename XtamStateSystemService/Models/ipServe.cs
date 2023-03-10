using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtamStateSystemService.Models
{
    public class Ipserve
    {
        public string ip { get; set; }
    }
    public class IpserveList
    {
        public List<Ipserve> Ipserves { get; set; }
    }
}
