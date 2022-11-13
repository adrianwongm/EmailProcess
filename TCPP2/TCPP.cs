using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPP2
{
    public class TCPP
    {
        /// <summary>
        /// TCORDP = ORDEN PRODUCCION.
        /// </summary>
        public int TCORDP { get; set; }

        /// <summary>
        ///  TCSECU = SECUENCIA........
        /// </summary>
        public int TCSECU { get; set; }
              
        /// <summary>
        /// TCPROD = PRODUCTO.........
        /// </summary>
        public string TCPROD { get; set; }
       
        /// <summary>
        /// TCCANT = CANTIDAD.........
        /// </summary>
        public int TCCANT { get; set; }

        /// <summary>
        ///  TCFCPR = FECHA PROCESO....
        /// </summary>
        public int TCFCPR { get; set; }
        
        /// <summary>
        ///TCHRPR = HORA PROCESO..... 
        /// </summary>
        public int TCHRPR { get; set; }
       
        /// <summary>
        ///  TCUSER = USUARIO PROCESO..
        /// </summary>
        public string TCUSER { get; set; }
      
        /// <summary>
        /// TCESTA = ESTADO...........
        /// </summary>
        public string TCESTA { get; set; }

    }
}
