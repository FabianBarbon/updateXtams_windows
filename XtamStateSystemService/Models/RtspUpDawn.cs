using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtamStateSystemService.Models
{
    class RtspUpDawn
    {
        public class Mensaje
        {
            public string rtsp_url { get; set; }
            public int id { get; set; }
            public string descripcion { get; set; }
            public int cameraid { get; set; }
        }

        public class Root
        {
            public bool status { get; set; }
            public List<Mensaje> mensaje { get; set; }
        }


    }
}
