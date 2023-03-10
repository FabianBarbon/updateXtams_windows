using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtamStateSystemService.Utilities
{
    class Zipp
    {
        private static readonly log4net.ILog generalLog = log4net.LogManager.GetLogger("GeneralLogs");
        private static readonly log4net.ILog errorLog = log4net.LogManager.GetLogger("ErrorLogs");
        public static double  Bigger()
        {
            try
            {
                string[] filePaths = Directory.GetFiles(ConfigurationManager.AppSettings["RouteUpdate1"]); //ConfigurationManager.AppSettings["ApiUpdate"];
                int interactionss = filePaths.Length;
                string[] fileFound = new string[interactionss];
                char _ = '_';
                for (int i = 0; i < filePaths.Length; i++)
                {
                    string[] x = filePaths[i].Split(_);
                    fileFound[i] = x[1].Replace(".zip", "").Replace(".", ",");
                    Console.WriteLine("iterar versiones : " + fileFound[i]);
                }
                
                double max_value = Convert.ToDouble(fileFound.Max());
                //Console.WriteLine("  cual es el mayor::  " + fileFound.Max()  + "tipo dato " + max_value.GetType());
                return max_value;
            }
            catch (Exception ex)
            {
                errorLog.Error("Error al momento de calcular el mayor del .zip:" + ex.Message.ToString());
                throw;
            }
        }

        //Extraer archivos .zip
        public static bool Extract_zip(string bigg)
        {
            try
            {
                string zipFilePath = ConfigurationManager.AppSettings["RouteUpdate1"] + "/Update_" + bigg + ".zip";
                string exist_folder = ConfigurationManager.AppSettings["RouteUpdate1"] + "/Update_" + bigg;
                Console.WriteLine("Existe el folder : " + exist_folder);
                /*
                if(File.Exists(exist_folder) ){
                    Console.WriteLine("El .zip ya se descomprimio");
                }
                else
                {
                   
                }*/
                string extractionPath = ConfigurationManager.AppSettings["RouteUpdate1"];
                ZipFile.ExtractToDirectory(zipFilePath, extractionPath);
                Console.WriteLine("Extracted Successfully");
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }


}
