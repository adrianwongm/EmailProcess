using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogErrores
{
    public class FormasLog
    {
        private static StreamWriter fw;

        public static void ConsoleLog(string texto)
        {
            System.Console.WriteLine(texto);
        }

        public static void FileLog(string texto)
        {
            try
            {
                bool append = true;
                if (fw == null)
                {
                    append = false;
                }
                string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "bin", "ficherolog.log");
                if (!File.Exists(configFile))
                {
                    configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ficherolog.log");
                }

                fw =
                new StreamWriter(configFile, append);
                fw.WriteLine(texto);
                fw.Close();
            }
            catch (IOException e)
            {
                FormasLog.ConsoleLog(e.Message);
            }
        }
    }
}
