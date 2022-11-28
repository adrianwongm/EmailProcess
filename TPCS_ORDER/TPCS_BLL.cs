﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using TA03;

namespace TPCS_ORDER
{
    public class TPCS_BLL
    {
        public string mensaje
        {
            get;
            set;
        }

        private TA03.TA03 UsuarioEmplSession
        {
            get;
            set;
        }

        public TPCS_BLL(TA03.TA03 pUsuarioEmplSession)
        {
            this.UsuarioEmplSession = pUsuarioEmplSession;
        }

        public TPCS_BLL()
        {
        }
         

        public bool insertaRegistroTPCS(TPCS pDatos, string usuario, string password)
        {
            bool flag = false;
           
            try
            {
                string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                string UID = Utilidades.obtenerParametro(Utilidades.ParametrosString.UID); //usuario;
                string PWD = Utilidades.obtenerParametro(Utilidades.ParametrosString.PWD); //password;
                string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                using (OdbcConnection conexion = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", new object[] { DSN, UID, PWD, DBQ })))
                {
                    conexion.Open();

                    OdbcCommand odbcCommand = new OdbcCommand()
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    };
                    //Verificar secuencial
                     pDatos.TCSECU = 0; 
                     OdbcDataReader reader = (new OdbcCommand("SELECT MAX(TCSECU) +  1 FROM TPCS ")
                        {
                            CommandType = CommandType.Text,
                            Connection = conexion
                        }).ExecuteReader();
                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (reader.IsDBNull(0))
                                {
                                    pDatos.TCSECU = 1;
                                }
                                else
                                {
                                    pDatos.TCSECU = Convert.ToInt32(reader.GetDecimal(0));
                                } 
                            }
                            reader.NextResult();
                        }
                        reader.Close(); 
                   

                    OdbcCommand SecuncialCommand = new OdbcCommand()
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    };


                    OdbcCommand TPCSCommand = new OdbcCommand("INSERT INTO TPCS (TCSECU, TCCLTE, TCDIRE, TCORCM, TCFCRE, TCHRRE, TCESTA, TCUSES, TCFCES, TCHRES) VALUES (?,  ?,  ?,  ?,  ?,  ?,  ?,  ?, ?,  ?)")
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    };
                    TPCSCommand.Parameters.Add("TCSECU", OdbcType.Numeric).Value = pDatos.TCSECU;
                    TPCSCommand.Parameters.Add("TCCLTE", OdbcType.Numeric).Value = pDatos.TCCLTE;
                    TPCSCommand.Parameters.Add("TCDIRE", OdbcType.Numeric).Value = pDatos.TCDIRE;
                    TPCSCommand.Parameters.Add("TCORCM", OdbcType.VarChar).Value = pDatos.TCORCM;
                    TPCSCommand.Parameters.Add("TCFCRE", OdbcType.Numeric).Value = pDatos.TCFCRE;
                    TPCSCommand.Parameters.Add("TCHRRE", OdbcType.Numeric).Value = pDatos.TCHRRE;
                    TPCSCommand.Parameters.Add("TCESTA", OdbcType.Numeric).Value = pDatos.TCESTA;
                    TPCSCommand.Parameters.Add("TCUSES", OdbcType.VarChar).Value = pDatos.TCUSES;
                    TPCSCommand.Parameters.Add("TCFCES", OdbcType.Numeric).Value = pDatos.TCFCES;
                    TPCSCommand.Parameters.Add("TCHRES", OdbcType.Numeric).Value = pDatos.TCHRES;
                    TPCSCommand.ExecuteNonQuery();
                    TPCSCommand.Dispose();

                    foreach (var detalle in pDatos.Detalle)
                    {
                        OdbcCommand TPDSCommand = new OdbcCommand("INSERT INTO TPDS (TDSECU, TDSEC2, TDCDCL, TDCDSW, TDCANT, TDESTA) VALUES (?,  ?,  ?,  ?,  ?,  ?)")
                        {
                            CommandType = CommandType.Text,
                            Connection = conexion
                        };
                        detalle.TDSEC2 = pDatos.TCSECU;
                        TPDSCommand.Parameters.Add("TDSECU", OdbcType.Numeric).Value = detalle.TDSECU;
                        TPDSCommand.Parameters.Add("TDSEC2", OdbcType.Numeric).Value = detalle.TDSEC2;
                        TPDSCommand.Parameters.Add("TDCDCL", OdbcType.VarChar).Value = detalle.TDCDCL;
                        TPDSCommand.Parameters.Add("TDCDSW", OdbcType.VarChar).Value = detalle.TDCDSW;
                        TPDSCommand.Parameters.Add("TDCANT", OdbcType.Numeric).Value = detalle.TDCANT;
                        TPDSCommand.Parameters.Add("TDESTA", OdbcType.Numeric).Value = detalle.TDESTA;
                        TPDSCommand.ExecuteNonQuery();
                        TPDSCommand.Dispose();
                    }
                    flag = true;
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                throw new Exception(string.Concat(new string[] { "Orden: ", pDatos.TCSECU.ToString(), " -> ", ex.Message }));
            }
            return flag;
        }

       
         

    }
}