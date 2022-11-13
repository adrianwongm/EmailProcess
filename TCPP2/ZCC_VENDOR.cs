using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPP2
{
    public class ZCC_VENDOR
    { 
        /// <summary>
        /// CCID.Tipo Tabla, valor por defecto: CC
        public string CCID { get; set; }        /// </summary>


        /// <summary>
        /// CCTABL. Codigo Tabla, valor por defecto:  RCTPRDTR   
        /// </summary>
        public string CCTABL { get; set; }

        /// <summary>
        /// CCCODE. Codigo Registro, valor por defecto: 0001
        /// </summary>
        public string CCCODE { get; set; }

        /// <summary>
        /// CCSDSC, Descripcion Corta del registro, valor por defecto:  RECEPCION PRODUCTO TERMINADO  
        /// </summary>
        public string CCSDSC { get; set; }

        /// <summary>
        /// CCSDSC, Descripcion Corta del registro, valor por defecto:  RECEPCION PRODUCTO TERMINADO  
        /// </summary>
        public string CCDESC { get; set; }

        /// <summary>
        /// CCNOT1, Valor Adicional, los primeros 6 caracteres contiene el codigo Vendor y el ultimo contiene el tipo Customer/Vendor
        /// Consulte las propiedades para ver el valor de codigo de Vendor (CodVendor)
        /// Consulte las propiedades para ver el valor de codigo de Vendor (Custom_Vendor)
        /// </summary>
        public string CCNOT1 { get; set; }

        public int CodVendor { 
            get {
                int respuesta = 0;
                string cod = "0";
                if (CCNOT1.Length > 6)
                {
                    cod = CCNOT1.Substring(0, 6);
                    respuesta = int.Parse(cod);
                } 
                return respuesta;
            } 
        }
        public string Custom_Vendor
        {
            get
            {
                string respuesta = "";
                if (CCNOT1.Length > 6)
                {
                    respuesta = CCNOT1.Trim().Substring(6, 1);
                }
                return respuesta;
            }
        }

    }
}
