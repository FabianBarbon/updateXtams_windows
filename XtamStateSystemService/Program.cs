using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XtamStateSystemService
{
    static class Program
    {
        //Inicia el servicio
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
            #if (!DEBUG)
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[]
                    {
                        new XtamStateSystemService()
                    };
                    ServiceBase.Run(ServicesToRun);
        #else
            XtamStateSystemService service = new XtamStateSystemService();
                                service.Start();
                                Thread.Sleep(System.Threading.Timeout.Infinite);
                    #endif
                }
            }
}
