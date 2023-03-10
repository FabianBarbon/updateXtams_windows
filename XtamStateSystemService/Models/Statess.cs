using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtamStateSystemService.Models
{
    class Statess
    {
        public string ip_server { get; set; }
        public string id_system_state { get; set; }
        public int id_sitio { get; set; }
        public string name_site { get; set; }
        public string state { get; set; }
        public string fail_type { get; set; }
        public string date_update { get; set; }

        public class SatessList
        {
            public List<Statess> sites { get; set; }
        }
    }
}
