using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XtamStateSystemService.Models;

namespace XtamStateSystemService.Utilities
{
    public class HttpController
    {
        private static readonly log4net.ILog generalLog = log4net.LogManager.GetLogger("GeneralLogs");
        private static readonly log4net.ILog errorLog = log4net.LogManager.GetLogger("ErrorLogs");
        /// <summary>
        /// Obtener sitios de xtam central
        /// </summary>
        /// <returns></returns>
        /// 
        public static string ExistUpdateAsync(string ip_server)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var endpoint = ConfigurationManager.AppSettings["ApiUpdate"];
                    var newStatee = new Dictionary<string, string>
                    {
                        { "ip" , ip_server }
                    };
                    var sendNState = JsonConvert.SerializeObject(newStatee);

                    var payload = new StringContent(sendNState, Encoding.UTF8, "application/json");
                    var result = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;
                    return result;
                }
            }
            catch (Exception ex)
            {
                errorLog.Error("Error obteniendo sitios desde la API:" + ex);
                throw ex;
            }
        }

        //Establecer actualizacion en base de datos
        public static string SetUpdate(string version)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var endpoint = "http://localhost:3000/api/uptStateXyams/";
                    Updatee updatee = new Updatee
                    {
                        ip = "127.0.0.1",
                        actualizacion = version
                    };

                    var sendNState = JsonConvert.SerializeObject(updatee);

                    var payload = new StringContent(sendNState, Encoding.UTF8, "application/json");

                    var result = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;

                    return result;

                }
            }
            catch (Exception ex)
            {
                return "";
                
                throw ex;
            }
        }


    }
}
