using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogErrores
{
    public delegate void logDelegate(string arg);
    public class ProgramErrorClass
    {
        logDelegate logFunction;
        public void Run(string pMensaje)
        {
            if (logFunction == null)
            {
                logFunction =
                new logDelegate(FormasLog.ConsoleLog);
            }
            Log(pMensaje);
        }

        public void SetLogMethod(logDelegate metodo)
        {
            logFunction = metodo;
        }

        private void Log(string texto)
        {
            logFunction(texto);
        }

    }
}
