using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TPCS_ORDER;

namespace ExcelReader
{
    public class Lector
    {
        public static List<string> ErrorProcessList { get; set; }
        public Lector()
        {
            ErrorProcessList = new List<string>();
        }
    
        public static void procesarExcel(string filePath)
        { 
            try
            { 
                string celdaValorProveedor = ConfigurationManager.AppSettings["CeldaValorProveedor"];
                string celdaValorContactoProveedor = ConfigurationManager.AppSettings["CeldaValorContactoProveedor"];
                string celdaValorSucursal = ConfigurationManager.AppSettings["CeldaValorSucursal"];
                string celdaValorDireccionEntrega = ConfigurationManager.AppSettings["CeldaValorDireccionEntrega"];
                string celdaValorOrdenCompra = ConfigurationManager.AppSettings["CeldaValorOrdenCompra"];
                string celdaValorClienteSwiss = ConfigurationManager.AppSettings["CeldaValorClienteSwiss"];
                string celdaValorEntregarEn = ConfigurationManager.AppSettings["CeldaValorEntregarEn"];

                string celdaRangoInicialValores = ConfigurationManager.AppSettings["CeldaRangoInicialValores"];
                string columnaRangoFinalValores = ConfigurationManager.AppSettings["ColumnaRangoFinalValores"];
                string limiteMaximoValores = ConfigurationManager.AppSettings["LimiteMaximoValores"];

                string columnaCodigoCONAUTO = ConfigurationManager.AppSettings["ColumnaCodigoCONAUTO"];
                string columnaCodigoSWISSOIL = ConfigurationManager.AppSettings["ColumnaCodigoSWISSOIL"];
                string columnaDescripcionProducto = ConfigurationManager.AppSettings["ColumnaDescripcionProducto"];
                string columnaEmpaque = ConfigurationManager.AppSettings["ColumnaEmpaque"];
                string columnaCantidad = ConfigurationManager.AppSettings["ColumnaCantidad"];

                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                ExcelFile workbook = ExcelFile.Load(filePath);
                for (int sheetIndex = 0; sheetIndex < workbook.Worksheets.Count; sheetIndex++)
                {
                    ExcelWorksheet worksheet = workbook.Worksheets[sheetIndex];
                    var valorProveedor = worksheet.Cells[celdaValorProveedor].Value;
                    var valorContactoProveedor = worksheet.Cells[celdaValorContactoProveedor].Value;
                    var valorSucursal = worksheet.Cells[celdaValorSucursal].Value;
                    var valorDireccionEntrega = worksheet.Cells[celdaValorDireccionEntrega].Value;
                    var valorOrdenCompra = worksheet.Cells[celdaValorOrdenCompra].Value;
                    var valorClienteSwiss = worksheet.Cells[celdaValorClienteSwiss].Value;
                    var valorEntregarEn = worksheet.Cells[celdaValorEntregarEn].Value;

                  
                    int limiteActual = 1;
                    var rangoContenido = worksheet.Cells.GetSubrange(celdaRangoInicialValores, $"{ columnaRangoFinalValores}{ limiteMaximoValores}");
                    var columnaRangoIncialValores = rangoContenido.First().Column.Name;
                    ArchivoConauto.Detallado detallado = new ArchivoConauto.Detallado();
                     
                    List<string> ErrorList= new List<string>();
                    ArchivoConautoOperations.Validator.verifcaTexto(valorProveedor, nameof(valorProveedor) +$" Cell:({celdaValorProveedor})");
                    if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError))  ErrorList.Add(ArchivoConautoOperations.Validator._mensajeError) ;

                    ArchivoConautoOperations.Validator.verifcaTexto(valorContactoProveedor, nameof(valorContactoProveedor) + $" Cell:({celdaValorContactoProveedor})");
                    if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) ErrorList.Add(ArchivoConautoOperations.Validator._mensajeError);

                    ArchivoConautoOperations.Validator.verifcaTexto(valorSucursal, nameof(valorSucursal) + $" Cell:({celdaValorSucursal})");
                    if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) ErrorList.Add(ArchivoConautoOperations.Validator._mensajeError);

                    ArchivoConautoOperations.Validator.verifcaTexto(valorDireccionEntrega, nameof(valorDireccionEntrega) + $" Cell:({celdaValorDireccionEntrega})");
                    if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) ErrorList.Add(ArchivoConautoOperations.Validator._mensajeError);

                    ArchivoConautoOperations.Validator.verifcaTexto(valorOrdenCompra, nameof(valorOrdenCompra) + $" Cell:({celdaValorOrdenCompra})");
                    if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) ErrorList.Add(ArchivoConautoOperations.Validator._mensajeError);

                    ArchivoConautoOperations.Validator.verifcaTexto(valorClienteSwiss, nameof(valorClienteSwiss) + $" Cell:({celdaValorClienteSwiss})");
                    if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) ErrorList.Add(ArchivoConautoOperations.Validator._mensajeError);

                    ArchivoConautoOperations.Validator.verifcaTexto(valorEntregarEn, nameof(valorEntregarEn) + $" Cell:({celdaValorEntregarEn})");
                    if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) ErrorList.Add(ArchivoConautoOperations.Validator._mensajeError);
                  
                    detallado.Proveedor = valorProveedor?.ToString();
                    detallado.ContactoProveedor = valorContactoProveedor?.ToString();
                    detallado.Sucursal= valorSucursal?.ToString();
                    detallado.DireccionEntrega = valorDireccionEntrega?.ToString();
                    detallado.OrdenCompra= valorOrdenCompra?.ToString();
                    detallado.ClientSwiss= valorClienteSwiss?.ToString();
                    detallado.EntregarEn = valorEntregarEn?.ToString();
                    detallado.Detalles = new DetalleArchivoConauto[] { };
                  
                    IList<DetalleArchivoConauto> detalles = new List<DetalleArchivoConauto>(); 
                    var detalle = new DetalleArchivoConauto();
                    int orden = 1;
                    foreach (var celda in rangoContenido)
                    {
                        //Verificacion fila registro completa
                        var fila = worksheet.Cells.GetSubrange($"{columnaRangoIncialValores}{celda.Row.Name}", $"{columnaRangoFinalValores}{celda.Row.Name}");
                        if(fila.Where(x=>x.Value ==null).Count() == fila.LastColumnIndex + 1)
                        {
                            break;
                        }
                        if (fila.Where(x =>string.IsNullOrEmpty(x.Value?.ToString())).Count() == fila.LastColumnIndex + 1)
                        {
                            break;
                        }

                        if (celda.Column.Name== columnaRangoIncialValores) 
                        { detalle = new DetalleArchivoConauto(); } 

                        if (celda.Column.Name == columnaCodigoCONAUTO) 
                        {
                            ArchivoConautoOperations.Validator.verifcaTexto(celda.Value, nameof(columnaCodigoCONAUTO).Replace("columna", "") + $" Cell:({celda.Column.Name}{celda.Row.Name})");
                            if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) { ErrorList.Add(ArchivoConautoOperations.Validator._mensajeError); }
                            else { detalle.CodigoCONAUTO = celda?.StringValue; }
                        } 
                        if (celda.Column.Name == columnaCodigoSWISSOIL) 
                        {
                            ArchivoConautoOperations.Validator.verifcaTexto(celda.Value, nameof(columnaCodigoSWISSOIL).Replace("columna", "") + $" Cell:({celda.Column.Name}{celda.Row.Name})");
                            if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) { ErrorList.Add(ArchivoConautoOperations.Validator._mensajeError); }
                            else { detalle.CodigoSWISSOIL = celda?.StringValue; } 
                        }
                        if (celda.Column.Name == columnaDescripcionProducto) 
                        {
                            ArchivoConautoOperations.Validator.verifcaTexto(celda.Value, nameof(columnaDescripcionProducto).Replace("columna", "") + $" Cell:({celda.Column.Name}{celda.Row.Name})");
                            if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) { ErrorList.Add(ArchivoConautoOperations.Validator._mensajeError); }
                            else  { detalle.DescripcionProducto = celda?.StringValue;} 
                        }
                        if (celda.Column.Name == columnaEmpaque) {
                            ArchivoConautoOperations.Validator.verifcaTexto(celda.Value, nameof(columnaEmpaque).Replace("columna", "") + $" Cell:({celda.Column.Name}{celda.Row.Name})");
                            if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) { ErrorList.Add(ArchivoConautoOperations.Validator._mensajeError); }
                            else { detalle.Empaque = celda?.StringValue; } 
                        }
                        if (celda.Column.Name == columnaCantidad) { 
                            ArchivoConautoOperations.Validator.verifcaNumeroDouble(celda.Value, nameof(columnaCantidad).Replace("columna", "") + $" Cell:({celda.Column.Name}{celda.Row.Name})");
                            if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) { ErrorList.Add(ArchivoConautoOperations.Validator._mensajeError); }
                            else { detalle.Cantidad = celda?.DoubleValue; } 
                        }

                        if (celda.Column.Name  == columnaRangoFinalValores) 
                        {
                            detalle.Orden = orden;
                            detalles.Add(detalle); //limiteActual++;
                            orden++;
                        } 
                    }
                    if(detalles.Count<= 0) { ErrorList.Add("El archivo no tiene detalles"); }
                    detallado.Detalles = detalles.ToArray();
                    
                    if (ErrorList.Count > 0)
                    {
                        ErrorProcessList = ErrorList;
                        return;
                    }
                    string usuario = ConfigurationManager.AppSettings["User"];
                    var registro = detallado.ToAS400(usuario, 1);
                    TPCS_BLL obj = new TPCS_BLL();
                    var resp = obj.insertaRegistroTPCS(registro,"","");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
