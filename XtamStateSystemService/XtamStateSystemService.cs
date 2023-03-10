using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;
using XtamStateSystemService.Models;
using XtamStateSystemService.Utilities;

namespace XtamStateSystemService
{
    public partial class XtamStateSystemService : ServiceBase
    {
        Thread statusThread;
        System.Timers.Timer tmServiceStatus = null;
        bool busyStatus = false; //Ocupado
        private static readonly log4net.ILog generalLog = log4net.LogManager.GetLogger("GeneralLogs");
        private static readonly log4net.ILog errorLog = log4net.LogManager.GetLogger("ErrorLogs");
        public static int id_sitio;
        public static string name_site;
        public static string state;
        public string contentFile;
        public static string fail_type;

        routeWindows contentFile_json;
        public XtamStateSystemService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        public void Start()  //inicia service
        {
            //ejecutar
            try
            {
                //llama al thread espera 5 s e inicia el timmer
                generalLog.Info("Iniciando Servicio Actualizaciones Xtam Windows...");
                statusThread = new Thread(StartStatus);
                Thread.Sleep(10);
                statusThread.Start();
                /////Termina hilo streaming
            }
            catch (Exception ex)
            {

                errorLog.Error("Error Procesando Estado del sistema: " + ex.Message.ToString());
                OnStop();
            }
        }

        private void StartStatus()
        {
            //cada que se ejecute el statuds, 
            // Inicializacion del timer
            tmServiceStatus = new System.Timers.Timer(int.Parse(ConfigurationManager.AppSettings["timeCheckStatus"]));
            tmServiceStatus.Elapsed += new ElapsedEventHandler(tmServicio_Elapsed); //se dispara tmServicio_Elapsed

            // Have the timer fire repeated events (true is the default)
            tmServiceStatus.AutoReset = true;

            // Start the timer
            tmServiceStatus.Enabled = true;

            // If the timer is declared in a long-running method, use KeepAlive to prevent garbage collection
            // from occurring before the method ends.
            GC.KeepAlive(tmServiceStatus);

            ///////////////////////////////////////////////////////////////////////////
        }

        private void tmServicio_Elapsed(object sender, ElapsedEventArgs e)
        {
            StartStatusProcess();
        }

        private void StartStatusProcess()
        {
            contentFile_json = JsonConvert.DeserializeObject<routeWindows>(What_ip());
            Console.WriteLine("Ipserver:: " + contentFile_json.ipserver);
            if (contentFile_json.ipserver == "")
            {
                Console.WriteLine("No se encontro el archivo de routeo");
            } else if  (contentFile_json.ipserver != "")
            {
                Console.WriteLine(contentFile_json.ipserver);
                //Segunndo paso 
                try
                {
                    if (!busyStatus)
                    {
                        busyStatus = true;
                        generalLog.Info("Inicio procesamiento busqueda de actualizaciones " + DateTime.Now.ToString("dd/MM/yyyy/ HH:mm:ss"));
                        Console.WriteLine("Inicio procesamiento busqueda de actualizaciones  " + DateTime.Now.ToString("dd/MM/yyyy/ HH:mm:ss"));

                        var Updates_avaliables = HttpController.ExistUpdateAsync(contentFile_json.ipserver);

                        response_update Updates_avaliables_json = JsonConvert.DeserializeObject<response_update>(Updates_avaliables);
                        Console.WriteLine("que ve:: " + Updates_avaliables_json.status);
                        
                        if (!Updates_avaliables_json.status)
                        {
                            Console.WriteLine(" No tiene actualizaciones disponibles. ");
                            generalLog.Info("El quipo con la siguiente ip; " + Updates_avaliables_json.ip + " no tiene actualizaciones disponibles " + DateTime.Now.ToString("dd/MM/yyyy/ HH:mm:ss"));
                        }
                        else
                        {
                            // logica interina xtam actualizacion 
                            bool flag_reset_update = false;
                            double upt_bigger = Zipp.Bigger();
                            string str_upt_bigger = upt_bigger.ToString().Replace(",",".");

                            
                            bool extract = Zipp.Extract_zip(str_upt_bigger);
                            //Console.WriteLine("que puede ver::  " + extract); 
                            
                            if (extract)
                            {
                                //Es este el que se ejecuta
                                string script_pyy = String.Format(ConfigurationManager.AppSettings["RouteUpdate1"] + "/Update_{0}/Update_{0}.py", str_upt_bigger);
                                string script_shh = String.Format(ConfigurationManager.AppSettings["RouteUpdate1"] + "/Update_{0}/Update_{0}.sh", str_upt_bigger);

                                if (File.Exists(script_pyy))
                                {
                                    Console.WriteLine("el archivo " + ".py" + "Existe");
                                    Execute_py(script_pyy);
                                    flag_reset_update = true;
                                }
                                
                                if (flag_reset_update)
                                {
                                    //Updates_avaliables_json.vsion
                                    var set_update = HttpController.SetUpdate(Updates_avaliables_json.vsion);
                                    Updatee upt_set = JsonConvert.DeserializeObject<Updatee>(set_update);
                                    if (upt_set.rta == "ok")
                                    {
                                        Console.WriteLine(" Se actualizo de manera correcta ");
                                        generalLog.Info("Se llevo a cabo la actualizacion de manera correcta ");
                                    }
                                }
                            }
                            else
                            {
                                errorLog.Error("Error al momento de extraer el mayor del .zip:");
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    errorLog.Error("Error Procesando Estado del sistema: " + ex.Message.ToString());
                    busyStatus = false;
                }
            }     
        }
        
        protected override void OnStop()
        {

        }

    
        public string  What_ip()
        {
            //primer paso: cual es la ip del xtam 
            if (File.Exists(ConfigurationManager.AppSettings["RouteC"]))
            {
                Console.WriteLine("Existe el archivo en el dico C");
                contentFile = RouterWindows.routerWindows(ConfigurationManager.AppSettings["RouteC"]);
            }
            else if (File.Exists(ConfigurationManager.AppSettings["RouteD"]))
            {
                Console.WriteLine("Existe el archivo en el disco D");
                contentFile = RouterWindows.routerWindows(ConfigurationManager.AppSettings["RouteD"]);
            }

            else
            {
                Console.WriteLine("No existe el archivo");
                routeWindows string_empty = new routeWindows();
                string_empty.ipserver = "";
                contentFile = JsonConvert.SerializeObject(string_empty);
            }
            return contentFile;
        }

        public static void Execute_py(string script_py)
        {
            try
            {
                var psi = new ProcessStartInfo();
                //var script_py = @"C:\Users\Verytel S.A\source\repos\updates\Update_1.4\Update_1.4.py";
                //var name = "Fabian ";

                psi.FileName = @"C:\Python311\python.exe";
                psi.Arguments = $"\"{script_py}\"";
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;

                var errors = "";
                var results = "";

                using (Process process = Process.Start(psi))
                {
                    errors = process.StandardError.ReadToEnd();
                    results = process.StandardOutput.ReadToEnd();
                }

                Console.WriteLine("erros");
                Console.WriteLine(errors);
                Console.WriteLine();
                Console.WriteLine("results");
                Console.WriteLine(results);
            }
            catch (Exception)
            {
                Console.WriteLine("SE ha generado un error en la funcion Execute_py");
                throw;
            }
        }


      
       
    }
}
//