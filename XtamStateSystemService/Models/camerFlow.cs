using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtamStateSystemService.Models
{
     public class CamerFlow
    {
        public string numberCameras { get; set; }
        public string rtsp_url { get; set; }

       
    }
    public class camerFlowList
    {
        public List<CamerFlow> camerflow { get; set; }
    }
}
