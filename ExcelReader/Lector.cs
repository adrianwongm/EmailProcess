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
        public class ErroList
        {
            public string NombreHoja { get; set; }
            public string MensajeError { get; set; }
        }
        public static List<ErroList> ErrorProcessList { get; set; }
        public Lector()
        {
            ErrorProcessList = new List<ErroList>();
        }
    
        public static List<ArchivoConauto.Detallado> procesarExcel(string filePath, string email)
        {
            bool resp=false;
            List<ArchivoConauto.Detallado> detallados = new List<ArchivoConauto.Detallado>();
            ArchivoConauto.Detallado detallado = new ArchivoConauto.Detallado();
            ErrorProcessList = new List<ErroList>(); 
            List<ErroList> ErrorListado = new List<ErroList>();
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
                string NombreHojaActual = "";
                string NombreArchivoActual = "";
                for (int sheetIndex = 0; sheetIndex < workbook.Worksheets.Count; sheetIndex++)
                {
                    ExcelWorksheet worksheet = workbook.Worksheets[sheetIndex];
                    NombreHojaActual = worksheet.Name;
                    //NombreArchivoActual ="";
                    var valorProveedor = worksheet.Cells[celdaValorProveedor].Value;
                    var valorContactoProveedor = worksheet.Cells[celdaValorContactoProveedor].Value;
                    var valorSucursal = worksheet.Cells[celdaValorSucursal].Value;
                    var valorDireccionEntrega = worksheet.Cells[celdaValorDireccionEntrega].Value;
                    var valorOrdenCompra = worksheet.Cells[celdaValorOrdenCompra].Value;
                    var valorClienteSwiss = worksheet.Cells[celdaValorClienteSwiss].Value;
                    var valorEntregarEn = worksheet.Cells[celdaValorEntregarEn].Value;

                    //int limiteActual = 1;
                    var rangoContenido = worksheet.Cells.GetSubrange(celdaRangoInicialValores, $"{ columnaRangoFinalValores}{ limiteMaximoValores}");
                    var columnaRangoIncialValores = rangoContenido.First().Column.Name;


                    ErrorListado = new List<ErroList>();
                    ArchivoConautoOperations.Validator.verifcaTexto(valorProveedor, nameof(valorProveedor) +$" Cell:({celdaValorProveedor})");
                    if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError))  ErrorListado.Add(new ErroList()  { 
                                                                                                                              NombreHoja= NombreHojaActual , 
                                                                                                                              MensajeError = ArchivoConautoOperations.Validator._mensajeError }) ;

                    ArchivoConautoOperations.Validator.verifcaTexto(valorContactoProveedor, nameof(valorContactoProveedor) + $" Cell:({celdaValorContactoProveedor})");
                    if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) ErrorListado.Add(new ErroList() {
                                                                                                                                NombreHoja = NombreHojaActual,
                                                                                                                                MensajeError = ArchivoConautoOperations.Validator._mensajeError
                                                                                                                            });

                    ArchivoConautoOperations.Validator.verifcaTexto(valorSucursal, nameof(valorSucursal) + $" Cell:({celdaValorSucursal})");
                    if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) ErrorListado.Add(new ErroList()   {
                                                                                                                                NombreHoja = NombreHojaActual,
                                                                                                                                MensajeError = ArchivoConautoOperations.Validator._mensajeError
                                                                                                                            });

                    ArchivoConautoOperations.Validator.verifcaTexto(valorDireccionEntrega, nameof(valorDireccionEntrega) + $" Cell:({celdaValorDireccionEntrega})");
                    if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) ErrorListado.Add(new ErroList()  {
                                                                                                                                NombreHoja = NombreHojaActual,
                                                                                                                                MensajeError = ArchivoConautoOperations.Validator._mensajeError
                                                                                                                            });

                    ArchivoConautoOperations.Validator.verifcaTexto(valorOrdenCompra, nameof(valorOrdenCompra) + $" Cell:({celdaValorOrdenCompra})");
                    if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) ErrorListado.Add(new ErroList()  {
                                                                                                                                NombreHoja = NombreHojaActual,
                                                                                                                                MensajeError = ArchivoConautoOperations.Validator._mensajeError
                                                                                                                            });

                    ArchivoConautoOperations.Validator.verifcaTexto(valorClienteSwiss, nameof(valorClienteSwiss) + $" Cell:({celdaValorClienteSwiss})");
                    if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) ErrorListado.Add(new ErroList()  {
                                                                                                                                NombreHoja = NombreHojaActual,
                                                                                                                                MensajeError = ArchivoConautoOperations.Validator._mensajeError
                                                                                                                            });

                    ArchivoConautoOperations.Validator.verifcaTexto(valorEntregarEn, nameof(valorEntregarEn) + $" Cell:({celdaValorEntregarEn})");
                    if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) ErrorListado.Add(new ErroList()  {
                                                                                                                                NombreHoja = NombreHojaActual,
                                                                                                                                MensajeError = ArchivoConautoOperations.Validator._mensajeError
                                                                                                                            });
                    detallado = new ArchivoConauto.Detallado();
                    detallado.HojaOrigen = NombreHojaActual;
                    detallado.Proveedor = valorProveedor?.ToString();
                    detallado.ContactoProveedor = valorContactoProveedor?.ToString();
                    detallado.Sucursal= valorSucursal?.ToString();
                    detallado.DireccionEntrega = valorDireccionEntrega?.ToString();
                    detallado.OrdenCompra = valorOrdenCompra?.ToString();
                    detallado.ClientSwiss = valorClienteSwiss?.ToString();//Validacion
                    detallado.EntregarEn = valorEntregarEn?.ToString();
                    detallado.Email = email;
                    detallado.Detalles = new DetalleArchivoConauto[] { };//Validacion
                  
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
                            if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError)) { ErrorListado.Add(new ErroList()  {
                                                                                                                            NombreHoja = NombreHojaActual,
                                                                                                                            MensajeError = ArchivoConautoOperations.Validator._mensajeError}); }
                            else { detalle.CodigoCONAUTO = celda?.StringValue; }
                        } 
                        if (celda.Column.Name == columnaCodigoSWISSOIL) 
                        {
                            ArchivoConautoOperations.Validator.verifcaTexto(celda.Value, nameof(columnaCodigoSWISSOIL).Replace("columna", "") + $" Cell:({celda.Column.Name}{celda.Row.Name})");
                            if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError))   { ErrorListado.Add(new ErroList()
                                                                                                                        {
                                                                                                                            NombreHoja = NombreHojaActual,
                                                                                                                            MensajeError = ArchivoConautoOperations.Validator._mensajeError }); }
                            else { detalle.CodigoSWISSOIL = celda?.StringValue; } 
                        }
                        if (celda.Column.Name == columnaDescripcionProducto) 
                        {
                            ArchivoConautoOperations.Validator.verifcaTexto(celda.Value, nameof(columnaDescripcionProducto).Replace("columna", "") + $" Cell:({celda.Column.Name}{celda.Row.Name})");
                            if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError))   {  ErrorListado.Add(new ErroList()  {
                                                                                                                            NombreHoja = NombreHojaActual,
                                                                                                                            MensajeError = ArchivoConautoOperations.Validator._mensajeError  });  }
                            else  { detalle.DescripcionProducto = celda?.StringValue;} 
                        }
                        if (celda.Column.Name == columnaEmpaque) {
                            ArchivoConautoOperations.Validator.verifcaTexto(celda.Value, nameof(columnaEmpaque).Replace("columna", "") + $" Cell:({celda.Column.Name}{celda.Row.Name})");
                            if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError))   {  ErrorListado.Add(new ErroList()  {
                                                                                                                            NombreHoja = NombreHojaActual,
                                                                                                                            MensajeError = ArchivoConautoOperations.Validator._mensajeError  });  }
                            else { detalle.Empaque = celda?.StringValue; } 
                        }
                        if (celda.Column.Name == columnaCantidad) { 
                            ArchivoConautoOperations.Validator.verifcaNumeroDouble(celda.Value, nameof(columnaCantidad).Replace("columna", "") + $" Cell:({celda.Column.Name}{celda.Row.Name})");
                            if (!string.IsNullOrEmpty(ArchivoConautoOperations.Validator._mensajeError))   {  ErrorListado.Add(new ErroList()  {  
                                                                                                                            NombreHoja = NombreHojaActual,
                                                                                                                            MensajeError = ArchivoConautoOperations.Validator._mensajeError  });  }
                            else { detalle.Cantidad = celda?.DoubleValue; } 
                        }

                        if (celda.Column.Name  == columnaRangoFinalValores) 
                        {
                            detalle.Orden = orden;
                            detalles.Add(detalle); //limiteActual++;
                            orden++;
                        } 
                    }
                    ArchivoConautoOperations.Validator._mensajeError = "";
                    if (detalles.Count<= 0) {   ErrorListado.Add(new ErroList()  {
                            NombreHoja = NombreHojaActual,
                            MensajeError = "No hay detalles de productos a procesar." });
                    }
                    detallado.Detalles = detalles.ToArray();
                    
                    if (ErrorListado.Count > 0)
                    {
                        ErrorProcessList.AddRange(ErrorListado);
                        detallados.Add(detallado);
                        return detallados;
                    }

                    TPCS_BLL obj = new TPCS_BLL();
                    //Validar Clientes
                    if(!obj.validarCliente(detallado.ClientSwiss, "", ""))
                    {
                        ErrorListado.Add(new ErroList() {
                            NombreHoja = NombreHojaActual,
                            MensajeError = $"El codigo de cliente {detallado.ClientSwiss} , no es válido." });
                    }
                    if (detallado.EntregarEn != "00" && !obj.validarDireccionCliente(detallado.ClientSwiss, detallado.EntregarEn, "", ""))
                    {
                        ErrorListado.Add(new ErroList()  {
                            NombreHoja = NombreHojaActual,
                            MensajeError = $"El codigo de entrega ({detallado.EntregarEn}) del" +
                            $" codigo de cliente {detallado.ClientSwiss} no es válido."  });
                    }
                    
                    //Validar detalles 
                    string usuario = ConfigurationManager.AppSettings["User"];
                    int i = 1;
                    foreach (var detalleValidar in detallado.Detalles)
                    {
                        if (!obj.validaCodigoProducto(detalleValidar.CodigoSWISSOIL, "", "")) {
                            ErrorListado.Add(new ErroList()   {
                                NombreHoja = NombreHojaActual,
                                MensajeError = $"El codigo de producto ({detalleValidar.CodigoSWISSOIL})" +
                               $" no es válido. Linea #{i}"   });
                        }
                        if (!obj.validaCostoProducto(detalleValidar.CodigoSWISSOIL, "", "", out double costo)) {
                            ErrorListado.Add(new ErroList()  {
                                NombreHoja = NombreHojaActual,
                                MensajeError =  $"El codigo de producto ({detalleValidar.CodigoSWISSOIL})" +
                                $" no tiene costo. Linea #{i}"   });
                        }
                        else
                        {
                            detalleValidar.Costo= costo;
                        }
                        if (!obj.validaPrecioProducto (detalleValidar.CodigoSWISSOIL, "", "", out double precio)) {
                            ErrorListado.Add(new ErroList()  {
                                NombreHoja = NombreHojaActual,
                                MensajeError = $"El codigo de producto ({detalleValidar.CodigoSWISSOIL})" +
                                    $" no tiene precio. Linea #{i}" });
                        }
                        else
                        {
                            detalleValidar.Precio = precio;
                        }
                        i++;
                        //detalleValidar.CodigoCONAUTO
                    }
                    bool hasError = false;
                    if (ErrorListado.Count > 0)
                    {
                        ErrorProcessList.AddRange(ErrorListado);
                        hasError = true;
                        //return detallado;
                    }

                    var registro = detallado.ToAS400(usuario, 1);
                    registro.TCESTA = (hasError == true ? 9 : 1);
                    detallados.Add(detallado);
                    try
                    { 
                        resp = obj.insertaRegistroTPCS(registro, "", "");
                    }
                    catch (Exception ex)
                    {
                        ErrorListado.Add(new ErroList()
                        {
                            NombreHoja = NombreHojaActual,
                            MensajeError = $"Error en procesamiento ({ex.Message})"
                        });
                        ErrorProcessList.AddRange(ErrorListado);
                    }
                }
                if (detallados != null)
                {
                    return detallados;
                }
                return new List<ArchivoConauto.Detallado>();
            }
            catch (Exception ex)
            {
                ErrorListado.Add(new ErroList()
                {
                    NombreHoja = string.Empty,
                    MensajeError =  $"Error en procesamiento ({ex.Message})"
                });
                ErrorProcessList.AddRange(ErrorListado);
                return detallados;
            }
        }
    }
}
