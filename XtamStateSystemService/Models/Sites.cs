using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtamStateSystemService.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Site
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public string iptunelgre { get; set; }
        public string ipsimcard { get; set; }
        public string ipserver { get; set; }
        public DateTime datecreated { get; set; }
    }

    public class SiteList
    {
        public List<Site> sites { get; set; }
    }


}
