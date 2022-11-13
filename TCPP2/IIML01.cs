using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPP2
{
    
    /// <summary>
    /// Nombre de la Tabla: IIML01
    /// </summary>
    public class Producto
    {
        /// <summary>
        /// Nombre del campo: IID  
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Nombre del campo: IPROD
        /// </summary>
        public string Codigo { get; set; }
        /// <summary>
        /// Nombre del campo: IDESC
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Nombre del campo: IUMS
        /// </summary>
        public string Unidad { get; set; }
        /// <summary>
        /// Nombre del campo: IITYP
        /// </summary>
        public string Inventario { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Costo { get; set; }

        /// <summary>
        /// Nombre del campo: IWHS
        /// </summary>
        public string Bodega { get; set; }

        /// <summary>
        /// Nombre del campo: ILOC
        /// </summary>
        public string Localizacion { get; set; }
        public override string ToString()
        {
            string strOut = string.Format("{0} - {1}", Codigo, Descripcion);
            return strOut;
        }
        public string Descripcion2 { 
                get {
                    string strOut = string.Format("{0} - {1}", Codigo, Descripcion);
                    return strOut;
                } 
        }
    }
}
