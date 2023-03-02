using ExcelReader;
using System;
using System.Collections.Generic;
using TPCS_ORDER;
using static ExcelReader.ArchivoConauto;

internal static class ConverterHelpers
{
    public enum TipoFecha
    {
        Fecha,
        Hora,
        FechaHora
    }
    public static int ToIntFecha(this DateTime fecha, TipoFecha tipoFecha)
    {
        if (tipoFecha == TipoFecha.Fecha)
        {
            return fecha.Year * 10000 + fecha.Month * 100 + fecha.Day;
        }
        if (tipoFecha == TipoFecha.Hora)
        {
            return fecha.Hour * 10000 + fecha.Minute * 100 + fecha.Second;
        }
        return 1;
    }
    public static TPCS ToAS400(this Detallado detallado, string usuario, int estado)
    {
        return new TPCS()
        {
            TCCLTE = int.Parse(detallado.ClientSwiss),
            TCDIRE = int.Parse(detallado.EntregarEn),
            TCESTA = estado,
            TCUSES = usuario,
            TCORCM = detallado.OrdenCompra,
            TCSECU = 0,
            TCFCES = DateTime.Now.ToIntFecha(TipoFecha.Fecha),
            TCFCRE = DateTime.Now.ToIntFecha(TipoFecha.Fecha),
            TCHRES = DateTime.Now.ToIntFecha(TipoFecha.Hora),
            TCHRRE = DateTime.Now.ToIntFecha(TipoFecha.Hora),
            TCNMPE = 0,
            TCEMAI = detallado.Email,             
            Detalle = detallado.Detalles.ToAS400(estado),
        };
    }

    public static TPDS[] ToAS400(this DetalleArchivoConauto[] detalle, int estado)
    {
        List<TPDS> lista= new List<TPDS>();
        foreach (var item in detalle)
        {
            lista.Add(new TPDS() { 
                 TDSECU = item.Orden,
                 TDSEC2 = 0,
                 TDCANT = Convert.ToDecimal(item.Cantidad??0d),
                 TDCDCL = item.CodigoCONAUTO,
                 TDCDSW = item.CodigoSWISSOIL,
                 TDESTA = item.Estado,
                 TDCOST = Convert.ToDecimal(item.Costo),
                 TDPREC = Convert.ToDecimal(item.Precio),
            });
        }
        return lista.ToArray();
    }
}