using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPCS_ORDER
{
    /// <summary>
    /// TPCS
    /// </summary>
    public class TPCS
    {
        /// <summary>
        /// ID. REGISTRO
        /// </summary>
        public int TCSECU { get; set; }
        /// <summary>
        /// COD. CLIENTE
        /// </summary>
        public int TCCLTE { get; set; }
        /// <summary>
        /// ID. ENTREGA
        /// </summary>
        public int TCDIRE { get; set; }
        /// <summary>
        /// HCPO ORDEN DE COMPRA
        /// </summary>
        public string TCORCM { get; set; }
        /// <summary>
        /// NRO. PEDIDO (POR DEFECTO 0)
        /// </summary>
        public int TCNMPE { get; set; }
        /// <summary>
        /// FECHA RECEPCION
        /// </summary>
        public int TCFCRE { get; set; }
        /// <summary>
        /// HORA RECEPCION
        /// </summary>
        public int TCHRRE { get; set; }
        /// <summary>
        /// ESTADO
        /// </summary>
        public int TCESTA { get; set; }
        /// <summary>
        /// USER ESTADO
        /// </summary>
        public string TCUSES { get; set; }
        /// <summary>
        /// FECHA ESTADO
        /// </summary>
        public int TCFCES { get; set; }
        /// <summary>
        /// HORA ESTADO
        /// </summary>
        public int TCHRES { get; set; }

        public TPDS[] Detalle { get; set; }
    }
}
