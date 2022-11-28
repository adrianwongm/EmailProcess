using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TPCS_ORDER;
using static ExcelReader.ArchivoConauto;

namespace ExcelReader
{
    public class ArchivoConauto
    {
        public string Proveedor { get; set; }
        public string ContactoProveedor { get; set; }
        public string Sucursal { get; set; }
        public string DireccionEntrega { get; set; }
        public string OrdenCompra { get; set; }
        public string ClientSwiss { get; set; }
        public string EntregarEn { get; set; }

        public class Detallado : ArchivoConauto
        {
            public DetalleArchivoConauto[] Detalles { get; set; }

            
        }



    }

    public class DetalleArchivoConauto
    {
        public int Orden { get; set; }
        public string  CodigoCONAUTO { get; set; }
        public string  CodigoSWISSOIL { get; set; }
        public string DescripcionProducto { get; set; }
        public string   Empaque { get; set; }
        public double? Cantidad { get; set; }
    }

    public class ArchivoConautoOperations
    {
         
        public static class Validator
        {
            public static string _mensajeError;
            public static void verifcaTexto(object valor, string fieldName)
            {
                _mensajeError = "";
                if(valor == null) { _mensajeError = $"{fieldName} no posee valor"; return; }
                if (string.IsNullOrEmpty(valor.ToString())){  _mensajeError = $"{fieldName}  con valor en blanco"; return; }
            }
            public static void verifcaNumeroDouble(object valor, string fieldName)
            {
                _mensajeError = "";
                if (valor == null) { _mensajeError = $"{fieldName} no posee valor"; return; }
              
                if (string.IsNullOrEmpty(valor.ToString())) { _mensajeError = $"{fieldName}  con valor en blanco"; return; }
                var valorDoubleTemp = valor.ToString();
                if(!double.TryParse(valorDoubleTemp, out double valorDouble))
                {
                    _mensajeError = $"{fieldName}  con valor numerico invalido"; return;
                } 
            }

        }
    }

}
