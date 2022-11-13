using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPP2
{
    public class TCPP2
    {
        /// <summary>
        /// TPORDP = ORDEN PRODUCCION.
        /// </summary> 
        public int TPORDP { get; set; }

        /// <summary>
        ///TPITEM = PRODUCTO......... 
        /// </summary>
        public string TPITEM { get; set; }

        /// <summary>
        ///TPSECU = SECUENCIA........ 
        /// </summary>
        public int TPSECU { get; set; }

        /// <summary>
        ///TPSEC2 = SECUENCIA 02..... 
        /// </summary>
        public int TPSEC2 { get; set; }

        /// <summary>
        /// TPCANT = CANTIDAD.........
        /// </summary>
        public int TPCANT { get; set; }

        /// <summary>
        ///TPUSRP = USUARIO PROCESO.. 
        /// </summary>
        public string TPUSRP { get; set; }

        /// <summary>
        /// TPFCPR = FECHA PROCESO...
        /// </summary>
        public int TPFCPR { get; set; }

        /// <summary>
        ///TPHRPR = HORA PROCESO..... 
        /// </summary>
        public int TPHRPR { get; set; }

        /// <summary>
        ///  TPESTA = ESTADO...........
        /// </summary>
        public string TPESTA { get; set; }

        /// <summary>
        ///TPUSER = USUARIO ESTADO... 
        /// </summary>
        public string TPUSER { get; set; }

        /// <summary>
        ///TPUSER = USUARIO GENERACION... 
        /// </summary>
        public string TPUSGR { get; set; }
        

        /// <summary>
        ///TPFCST = FECHA ESTADO..... 
        /// </summary>
        public int TPFCST { get; set; } 

        /// <summary>
        /// TPHRST = HORA ESTADO......
        /// </summary>
        public int TPHRST { get; set; }


        /// <summary>
        ///TPFCST = FECHA GENERACION..... 
        /// </summary>
        public int TPFCGR { get; set; }

        /// <summary>
        /// TPHRST = HORA GENERACION......
        /// </summary>
        public int TPHRGR { get; set; }

        /// <summary>
        /// TPORDM = ORDEN MEZCLA.....
        /// </summary>
        public int TPORDM { get; set; }
        /// <summary>
        ///  TPSECM = SECUENCIA MEZCLA.
        /// </summary>
        public int TPSECM { get; set; }
        /// <summary>
        /// TPBODE = BODEGA... .......
        /// </summary>
        public string TPBODE { get; set; }

        /// <summary>
        ///TPLOCA = LOCALIZACION.....
        /// </summary>
        public string TPLOCA { get; set; } 

        /// <summary>
        ///ADICIONAL* TEXTO QR CLIENT SIDE                                                        
        /// </summary>
        public string CODEQR_TEXT { get; set; }

        public string MENSAJE_ESTADO { get; set; }

        /// <summary>
        ///ADICIONAL* IDESC = DESCRIPCION ITEM (FROM IIML01)
        /// </summary>
        public string TPIDES { get; set; }

        /// <summary>
        /// ADICIONAL* IVULP = CANTIDAD POR PALLETS
        /// </summary>
        public int IVULP { get; set; }
    }
}
