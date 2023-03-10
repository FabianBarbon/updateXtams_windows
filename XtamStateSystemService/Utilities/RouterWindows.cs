using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtamStateSystemService.Utilities
{
    class RouterWindows
    {
        public static string routerWindows(string pathh)
        {
            TextReader readFile = new StreamReader(pathh);
            string contenFile = readFile.ReadToEnd().Replace("RUTEO DE WINDOWS", "").Replace("\n", " "); ;
            readFile.Close();
            return contenFile;
        }
    }
}
