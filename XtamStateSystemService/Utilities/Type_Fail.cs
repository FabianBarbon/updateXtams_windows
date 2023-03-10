using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json.Linq;

namespace XtamStateSystemService.Utilities
{
     public class Type_Fail
    {
        private static readonly log4net.ILog generalLog = log4net.LogManager.GetLogger("GeneralLogs");
        private static readonly log4net.ILog errorLog = log4net.LogManager.GetLogger("ErrorLogs");
        public static bool PingXtams( string ipRequest)
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send(ipRequest, 10000);//10 sregundos timeout
                generalLog.Info("Estado de la respuesta del ping : " + reply.Status.ToString());
                if (reply.Status.ToString() == "Success")
                {
                    return true;
                }else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorLog.Error("Error realizando ping :" + ex.Message.ToString() +"IP : " + ipRequest);
                return false;
            } 
        }


     
    }
}
