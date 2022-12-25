using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Runtime.CompilerServices;
using TA03;

namespace TCPP2
{
    public  class TCPP2_BLL
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

        public TCPP2_BLL(TA03.TA03 pUsuarioEmplSession)
        {
            this.UsuarioEmplSession = pUsuarioEmplSession;
        }

        public TCPP2_BLL()
        {
        }

        public bool CallProceso(string pUsuario = "", string pPassword = "")
        {
            bool flag;
            string proceso = Utilidades.obtenerParametro(Utilidades.ParametrosString.PROCESS01);
            try
            {
                string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                string UID = "";
                string PWD = "";
                if (pUsuario != "")
                {
                    UID = pUsuario.Trim();
                    PWD = pPassword.Trim();
                }
                else
                {
                    UID = Utilidades.obtenerParametro(Utilidades.ParametrosString.UID);
                    PWD = Utilidades.obtenerParametro(Utilidades.ParametrosString.PWD);
                }
                string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                using (OdbcConnection conexion = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", new object[] { DSN, UID, PWD, DBQ })))
                {
                    conexion.Open();
                    (new OdbcCommand(string.Concat("CALL ", proceso, ".TXI640C3"))
                    {
                        CommandType = CommandType.StoredProcedure,
                        Connection = conexion
                    }).ExecuteNonQuery();
                    flag = true;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return flag;
        }

        public bool getDatosAdicionlesValidacion(ref TCPP2 pTCPP2, string usuario, string password)
        {
            bool flag;
            bool bandera = false;
            try
            {
                string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                string UID = usuario;
                string PWD = password;
                string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                using (OdbcConnection conexion = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", new object[] { DSN, UID, PWD, DBQ })))
                {
                    conexion.Open();
                    OdbcDataReader reader = (new OdbcCommand(string.Concat("SELECT IVULP , IDESC FROM IIML01 WHERE IPROD='", pTCPP2.TPITEM.Trim(), "'"))
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    }).ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            pTCPP2.IVULP = reader.GetInt32(0);
                            pTCPP2.TPIDES = reader.GetString(1).Trim();
                            bandera = true;
                        }
                        reader.NextResult();
                    }
                    reader.Close();
                    flag = bandera;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return flag;
        }

       
        public string getDatosAdicionlesValidacion(string pITem, string usuario, string password)
        {
            string str;
            string pDescripcion = "";
            try
            {
                string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                string UID = usuario;
                string PWD = password;
                string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                using (OdbcConnection conexion = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", new object[] { DSN, UID, PWD, DBQ })))
                {
                    conexion.Open();
                    OdbcDataReader reader = (new OdbcCommand(string.Concat("SELECT IVULP , IDESC FROM IIML01 WHERE IPROD='", pITem.Trim(), "'"))
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    }).ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            pDescripcion = reader.GetString(1);
                        }
                        reader.NextResult();
                    }
                    reader.Close();
                    str = pDescripcion;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return str;
        }

        public bool getDatosLocalizacion(ref TCPP2 pTCPP2, string usuario, string password)
        {
            bool flag;
            bool bandera = false;
            try
            {
                string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                string UID = usuario;
                string PWD = password;
                string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                using (OdbcConnection conexion = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", new object[] { DSN, UID, PWD, DBQ })))
                {
                    conexion.Open();
                    OdbcDataReader reader = (new OdbcCommand(string.Concat("SELECT IWHS , ILOC FROM IIML01 WHERE IPROD ='", pTCPP2.TPITEM.Trim(), "'"))
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    }).ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            pTCPP2.TPBODE = reader.GetString(0).Trim();
                            pTCPP2.TPLOCA = reader.GetString(1).Trim();
                            bandera = true;
                        }
                        reader.NextResult();
                    }
                    reader.Close();
                    flag = bandera;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return flag;
        }

        public bool getDatosLocalizacionExistencia(string pItem, string usuario, string password)
        {
            bool flag;
            bool bandera = false;
            try
            {
                string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                string UID = usuario;
                string PWD = password;
                string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                using (OdbcConnection conexion = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", new object[] { DSN, UID, PWD, DBQ })))
                {
                    conexion.Open();
                    OdbcDataReader reader = (new OdbcCommand(string.Concat("SELECT TXBODE , TXLOCA FROM TIIM2 WHERE TXPROD ='", pItem.Trim(), "'"))
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    }).ExecuteReader();
                    TCPP2 pTCPP2 = new TCPP2()
                    {
                        TPITEM = pItem.Trim()
                    };
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            pTCPP2.TPBODE = reader.GetString(0).Trim();
                            pTCPP2.TPLOCA = reader.GetString(1).Trim();
                            bandera = true;
                        }
                        reader.NextResult();
                    }
                    reader.Close();
                    flag = bandera;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return flag;
        }

        public TCPP2 getTCCP2FromQRText(string pCode)
        {
            TCPP2 tCPP2;
            try
            {
                TCPP2 registro = new TCPP2();
                if (pCode.Contains("BULK"))
                {
                    pCode = pCode.Replace("BULK", "BULKKK");
                }
                registro.TPORDP = int.Parse(pCode.Substring(0, 6).Trim());
                registro.TPSECU = int.Parse(pCode.Substring(6, 2).Trim());
                registro.TPCANT = int.Parse(pCode.Substring(8, 4).Trim());
                if (!pCode.Contains("BULK"))
                {
                    registro.TPORDM = int.Parse(pCode.Substring(12, 6).Trim());
                }
                else
                {
                    registro.TPORDM = 999999;
                }
                registro.TPSECM = int.Parse(pCode.Substring(18, 2).Trim());
                registro.TPSEC2 = int.Parse(pCode.Substring(20, 3).Trim());
                registro.TPITEM = pCode.Substring(23, pCode.Length - 23).Trim();
                tCPP2 = registro;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return tCPP2;
        }

        public bool insertaSerie(TCPP2 pDatos, string usuario, string password)
        {
            bool flag;
            string pTPORDP = "";
            string pTPITEM = "";
            ZCC_VENDOR objZCC = new ZCC_VENDOR();
            objZCC = this.obtenerParametrizacion(usuario, password);
            string codigoProducto = pDatos.TPITEM;
            pDatos.TPITEM = this.obtenerCodigoHomologado(objZCC, codigoProducto, usuario, password);
            pDatos.TPSEC2 = this.obtenerSubSecuencia(pDatos.TPORDP, pDatos.TPSECU, pDatos.TPITEM.Trim(), usuario, password);
            DateTime now = DateTime.Now;
            int year = now.Year * 10000;
            now = DateTime.Now;
            int month = year + now.Month * 100;
            now = DateTime.Now;
            int fc = month + now.Day;
            now = DateTime.Now;
            int hour = now.Hour * 10000;
            now = DateTime.Now;
            int minute = hour + now.Minute * 100;
            now = DateTime.Now;
            int hr = minute + now.Second;
            pDatos.TPFCPR = fc;
            pDatos.TPHRPR = hr;
            pDatos.TPFCST = fc;
            pDatos.TPHRST = hr;
            try
            {
                string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                string UID = usuario;
                string PWD = password;
                string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                using (OdbcConnection conexion = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", new object[] { DSN, UID, PWD, DBQ })))
                {
                    conexion.Open();
                    OdbcCommand odbcCommand = new OdbcCommand()
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    };
                    OdbcCommand tPORDP = new OdbcCommand("INSERT INTO TCPP2 (TPORDP, TPITEM, TPSECU, TPSEC2, TPCANT, TPUSRP, TPFCPR, TPHRPR, TPESTA, TPUSER, TPFCST, TPHRST) VALUES (?,  ?,  ?,  ?,  ?,  ?,  ?,  ?, ?,  ?,  ?,  ?)")
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    };
                    pTPORDP = pDatos.TPORDP.ToString();
                    pTPITEM = pDatos.TPITEM.ToString();
                    tPORDP.Parameters.Add("TPORDP", OdbcType.Numeric).Value = pDatos.TPORDP;
                    tPORDP.Parameters.Add("TPITEM", OdbcType.VarChar).Value = pDatos.TPITEM;
                    tPORDP.Parameters.Add("TPSECU", OdbcType.Numeric).Value = pDatos.TPSECU;
                    tPORDP.Parameters.Add("TPSEC2", OdbcType.Numeric).Value = pDatos.TPSEC2;
                    tPORDP.Parameters.Add("TPCANT", OdbcType.Numeric).Value = pDatos.TPCANT;
                    tPORDP.Parameters.Add("TPUSRP", OdbcType.VarChar).Value = ""; //pDatos.TPUSRP;
                    tPORDP.Parameters.Add("TPFCPR", OdbcType.Numeric).Value = 0; //pDatos.TPFCPR;
                    tPORDP.Parameters.Add("TPHRPR", OdbcType.Numeric).Value = 0; //pDatos.TPHRPR;
                    tPORDP.Parameters.Add("TPESTA", OdbcType.VarChar).Value = "1"; //pDatos.TPESTA;
                    tPORDP.Parameters.Add("TPUSER", OdbcType.VarChar).Value = pDatos.TPUSER;
                    tPORDP.Parameters.Add("TPFCST", OdbcType.Numeric).Value = pDatos.TPFCST;
                    tPORDP.Parameters.Add("TPHRST", OdbcType.Numeric).Value = pDatos.TPHRST;
                    tPORDP.ExecuteNonQuery();
                    tPORDP.Dispose();
                    flag = true;
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                throw new Exception(string.Concat(new string[] { "Orden: ", pTPORDP, " Producto:", pTPITEM, " -> ", ex.Message }));
            }
            return flag;
        }

        public bool insertaSerieQR(TCPP2 pDatos, string usuario, string password)
        {
            bool flag;
            string pTPORDP = "";
            string pTPITEM = "";
            try
            {
                if (this.obtenerListado(pDatos.TPORDP, usuario, password, pDatos.TPSECU, pDatos.TPITEM, pDatos.TPSEC2).Count<TCPP2>() <= 0)
                {
                    DateTime now = DateTime.Now;
                    int year = now.Year * 10000;
                    now = DateTime.Now;
                    int month = year + now.Month * 100;
                    now = DateTime.Now;
                    int fc = month + now.Day;
                    now = DateTime.Now;
                    int hour = now.Hour * 10000;
                    now = DateTime.Now;
                    int minute = hour + now.Minute * 100;
                    now = DateTime.Now;
                    int hr = minute + now.Second;
                    pDatos.TPFCPR = fc;
                    pDatos.TPHRPR = hr;
                    pDatos.TPFCST = fc;
                    pDatos.TPHRST = hr;

                    pDatos.TPFCGR = fc;
                    pDatos.TPHRGR = hr;

                    if (!this.getDatosLocalizacion(ref pDatos, usuario, password))
                    {
                        this.mensaje = "ERROR|Falta la localizacion del producto capturado";
                    }
                }
                else
                {
                    this.mensaje = "ERROR|Etiqueta ya tiene una captura anterior";
                    flag = false;
                    return flag;
                }
            }
            catch (Exception exception)
            {
                this.mensaje = exception.Message;
                flag = false;
                return flag;
            }
            try
            {
                string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                string UID = usuario;
                string PWD = password;
                string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                using (OdbcConnection conexion = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", new object[] { DSN, UID, PWD, DBQ })))
                {
                    conexion.Open();
                    OdbcCommand odbcCommand = new OdbcCommand()
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    };
                    OdbcCommand tPORDP = new OdbcCommand("INSERT INTO TCPP2 (TPORDP, TPITEM, TPSECU, TPSEC2, TPCANT, TPESTA, TPUSER, TPFCST, TPHRST, TPORDM, TPSECM, TPBODE, TPLOCA, TPUSGR, TPFCGR, TPHRGR) " +
                        "VALUES (?,  ?,  ?,  ?,  ?,  ?,  ?,  ?,  ?,  ?,  ?,  ?,  ?, ?, ?, ?)")
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    };
                    pTPORDP = pDatos.TPORDP.ToString();
                    pTPITEM = pDatos.TPITEM.ToString();
                    tPORDP.Parameters.Add("TPORDP", OdbcType.Numeric).Value = pDatos.TPORDP;
                    tPORDP.Parameters.Add("TPITEM", OdbcType.VarChar).Value = pDatos.TPITEM;
                    tPORDP.Parameters.Add("TPSECU", OdbcType.Numeric).Value = pDatos.TPSECU;
                    tPORDP.Parameters.Add("TPSEC2", OdbcType.Numeric).Value = pDatos.TPSEC2;
                    tPORDP.Parameters.Add("TPCANT", OdbcType.Numeric).Value = pDatos.TPCANT;
                    //tPORDP.Parameters.Add("TPUSRP", OdbcType.VarChar).Value = pDatos.TPUSRP;
                    //tPORDP.Parameters.Add("TPFCPR", OdbcType.Numeric).Value = pDatos.TPFCPR;
                    //tPORDP.Parameters.Add("TPHRPR", OdbcType.Numeric).Value = pDatos.TPHRPR;
                    tPORDP.Parameters.Add("TPESTA", OdbcType.VarChar).Value = "1"; // pDatos.TPESTA;
                    tPORDP.Parameters.Add("TPUSER", OdbcType.VarChar).Value = pDatos.TPUSER;
                    tPORDP.Parameters.Add("TPFCST", OdbcType.Numeric).Value = pDatos.TPFCST;
                    tPORDP.Parameters.Add("TPHRST", OdbcType.Numeric).Value = pDatos.TPHRST;
                    tPORDP.Parameters.Add("TPORDM", OdbcType.Numeric).Value = pDatos.TPORDM;
                    tPORDP.Parameters.Add("TPSECM", OdbcType.Numeric).Value = pDatos.TPSECM;
                    tPORDP.Parameters.Add("TPBODE", OdbcType.VarChar).Value = pDatos.TPBODE;
                    tPORDP.Parameters.Add("TPLOCA", OdbcType.VarChar).Value = pDatos.TPLOCA;

                    tPORDP.Parameters.Add("TPUSGR", OdbcType.VarChar).Value = pDatos.TPUSGR;
                    tPORDP.Parameters.Add("TPFCGR", OdbcType.VarChar).Value = pDatos.TPFCGR;
                    tPORDP.Parameters.Add("TPHRGR", OdbcType.VarChar).Value = pDatos.TPHRGR;
                    
                    tPORDP.ExecuteNonQuery();
                    tPORDP.Dispose();
                    flag = true;
                }
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                throw new Exception(string.Concat(new string[] { "Orden: ", pTPORDP, " Producto:", pTPITEM, " -> ", ex.Message }));
            }
            return flag;
        }

        public string obtenerCodigoHomologado(ZCC_VENDOR pParametros, string pCodigoBarra, string usuario, string password)
        {
            string str;
            string codigoHomologado = "";
            try
            {
                string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                string UID = usuario;
                string PWD = password;
                string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                using (OdbcConnection conexion = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", new object[] { DSN, UID, PWD, DBQ })))
                {
                    conexion.Open();
                    string[] strArrays = new string[] { "SELECT IXITEM, IXPROD, IXDESC , IXID FROM  EIXL01 WHERE  IXITEM ='", pCodigoBarra, "' AND IXCVFL = ", pParametros.Custom_Vendor, " AND IXCUST = ", null };
                    strArrays[5] = pParametros.CodVendor.ToString();
                    OdbcDataReader reader = (new OdbcCommand(string.Concat(strArrays))
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    }).ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            codigoHomologado = reader.GetString(1).Trim();
                        }
                        reader.NextResult();
                    }
                    reader.Close();
                    str = codigoHomologado;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return str;
        }

        public decimal obtenerFactorDesvio()
        {
            return decimal.Parse(Utilidades.obtenerParametro(Utilidades.ParametrosString.FACTOR_DESVIO));
        }

        public List<TCPP2> obtenerListado(int numLote, string usuario, string password, int numEnvio = 0, string codigoProducto = "", int numEtiqueta = 0)
        {
            List<TCPP2> tCPP2s;
            List<TCPP2> list = new List<TCPP2>();
            try
            {
                string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                string UID = usuario;
                string PWD = password;
                string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                using (OdbcConnection conexion = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", new object[] { DSN, UID, PWD, DBQ })))
                {
                    conexion.Open();
                    string consultaSQl = string.Concat("SELECT TPORDP, TPITEM, TPSECU, TPSEC2, TPCANT, TPUSRP, TPFCPR, TPHRPR, TPESTA, TPUSER, TPFCST, TPHRST FROM TCPP2  WHERE TPORDP = ", numLote.ToString(), "  AND TPSECU = ", numEnvio.ToString());
                    if (codigoProducto != "")
                    {
                        consultaSQl = string.Concat(consultaSQl, " AND TPITEM = '", codigoProducto.Trim(), "' ");
                    }
                    if (numEtiqueta != 0)
                    {
                        consultaSQl = string.Concat(consultaSQl, " AND TPSEC2 = ", numEtiqueta.ToString().Trim(), " ");
                    }
                    OdbcDataReader reader = (new OdbcCommand(consultaSQl)
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    }).ExecuteReader();
                    TCPP2 obj = new TCPP2();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            obj = new TCPP2()
                            {
                                TPORDP = reader.GetInt32(0),
                                TPITEM = reader.GetString(1),
                                TPSECU = reader.GetInt32(2),
                                TPSEC2 = reader.GetInt32(3),
                                TPCANT = reader.GetInt32(4),
                                TPUSRP = reader.GetString(5),
                                TPFCPR = reader.GetInt32(6),
                                TPHRPR = reader.GetInt32(7),
                                TPESTA = reader.GetString(8),
                                TPUSER = reader.GetString(9),
                                TPFCST = reader.GetInt32(10),
                                TPHRST = reader.GetInt32(11)
                            };
                            list.Add(obj);
                        }
                        reader.NextResult();
                    }
                    reader.Close();
                    tCPP2s = list;
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                throw new Exception(string.Concat(new string[] { "Orden: ", numLote.ToString(), " Producto:", codigoProducto, " -> ", ex.Message }));
            }
            return tCPP2s;
        }

        public ZCC_VENDOR obtenerParametrizacion(string usuario, string password)
        {
            ZCC_VENDOR zCCVENDOR;
            this.mensaje = "";
            ZCC_VENDOR obj = new ZCC_VENDOR();
            try
            {
                string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                string UID = usuario;
                string PWD = password;
                string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                using (OdbcConnection conexion = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", new object[] { DSN, UID, PWD, DBQ })))
                {
                    conexion.Open();
                    OdbcDataReader reader = (new OdbcCommand("SELECT CCID, CCTABL, CCCODE, CCDESC, CCSDSC, CCNOT1 FROM   ZCC WHERE CCTABL = 'RCTPRDTR'")
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    }).ExecuteReader();
                    obj = new ZCC_VENDOR();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            obj = new ZCC_VENDOR()
                            {
                                CCID = reader.GetString(0),
                                CCTABL = reader.GetString(1),
                                CCCODE = reader.GetString(2),
                                CCDESC = reader.GetString(3),
                                CCSDSC = reader.GetString(4),
                                CCNOT1 = reader.GetString(5)
                            };
                        }
                        reader.NextResult();
                    }
                    reader.Close();
                    zCCVENDOR = obj;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(string.Concat("obtenerParametrizacion:  -> ", exception.Message));
            }
            return zCCVENDOR;
        }

        public int obtenerSubSecuencia(int numLote, int numEnvio, string codigoProducto, string usuario, string password)
        {
            int num;
            int subSecuencia = 0;
            try
            {
                string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                string UID = usuario;
                string PWD = password;
                string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                using (OdbcConnection conexion = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", new object[] { DSN, UID, PWD, DBQ })))
                {
                    conexion.Open();
                    OdbcDataReader reader = (new OdbcCommand(string.Concat(new string[] { "SELECT MAX(TPSEC2) FROM TCPP2  WHERE TPORDP = ", numLote.ToString(), "  AND TPITEM = '", codigoProducto.Trim(), "' AND TPSECU = ", numEnvio.ToString() }))
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    }).ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                subSecuencia = reader.GetInt32(0);
                                subSecuencia++;
                            }
                            catch
                            {
                                subSecuencia++;
                            }
                        }
                        reader.NextResult();
                    }
                    reader.Close();
                    num = subSecuencia;
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                throw new Exception(string.Concat(new string[] { "Orden: ", numLote.ToString(), " Producto:", codigoProducto, " -> ", ex.Message }));
            }
            return num;
        }

        public string obtenerValidaCantidades()
        {
            return Utilidades.obtenerParametro(Utilidades.ParametrosString.VALIDAR_CANTIDADES);
        }

        public bool verifcarProductoLote(int numLote, int numEnvio, string codigoProducto, int cantidad, bool verficaCantidades, string usuario, string password)
        {
            bool flag;
            this.mensaje = "";
            bool bandera = false;
            try
            {
                ZCC_VENDOR objZCC = new ZCC_VENDOR();
                objZCC = this.obtenerParametrizacion(usuario, password);
                string codigoHomologado = "";
                codigoHomologado = this.obtenerCodigoHomologado(objZCC, codigoProducto, usuario, password);
                if (codigoHomologado != "")
                {
                    string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                    string UID = usuario;
                    string PWD = password;
                    string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                    Utilidades.obtenerParametro(Utilidades.ParametrosString.VALIDAR_CANTIDADES);
                    decimal FACTOR_DESVIO = decimal.Parse(Utilidades.obtenerParametro(Utilidades.ParametrosString.FACTOR_DESVIO));
                    using (OdbcConnection conexion = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", new object[] { DSN, UID, PWD, DBQ })))
                    {
                        string consultaSQl = "";
                        conexion.Open();
                        consultaSQl = (numEnvio <= 0 ? string.Concat(new string[] { "SELECT SORD, 0 AS TCSECU, SPROD, SQREQ  FROM FSOL01 WHERE SORD =", numLote.ToString(), " AND SPROD ='", codigoHomologado.Trim(), "'" }) : string.Concat(new string[] { "SELECT TCORDP, TCSECU, TCPROD, TCCANT  FROM TCPP WHERE TCORDP = ", numLote.ToString(), "  AND TCPROD = '", codigoHomologado.Trim(), "' AND TCSECU = ", numEnvio.ToString() }));
                        OdbcDataReader reader = (new OdbcCommand(consultaSQl)
                        {
                            CommandType = CommandType.Text,
                            Connection = conexion
                        }).ExecuteReader();
                        if (!reader.HasRows)
                        {
                            this.mensaje = "Producto no existe en la orden/secuencia de orden de envase";
                            bandera = false;
                            flag = bandera;
                        }
                        else
                        {
                            if (!verficaCantidades)
                            {
                                reader.Close();
                            }
                            bandera = true;
                            if (verficaCantidades)
                            {
                                TCPP obj = new TCPP();
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        obj.TCORDP = reader.GetInt32(0);
                                        obj.TCSECU = reader.GetInt32(1);
                                        obj.TCPROD = reader.GetString(2);
                                        if (numEnvio <= 0)
                                        {
                                            obj.TCCANT = Convert.ToInt32(reader.GetDecimal(3));
                                        }
                                        else
                                        {
                                            obj.TCCANT = reader.GetInt32(3);
                                        }
                                    }
                                    reader.NextResult();
                                }
                                reader.Close();
                                List<TCPP2> listVer = new List<TCPP2>();
                                listVer = this.obtenerListado(numLote, usuario, password, numEnvio, codigoProducto, 0);
                                int num = cantidad;
                                int CantidadRegistrada = 0;
                                if (listVer.Count > 0)
                                {
                                    CantidadRegistrada = listVer.Sum<TCPP2>((TCPP2 p) => p.TPCANT);
                                }
                                if (((num + CantidadRegistrada / obj.TCCANT) - decimal.One) > FACTOR_DESVIO)
                                {
                                    bandera = false;
                                    this.mensaje = string.Concat("Cantidad de producto excedida de la orden (", FACTOR_DESVIO.ToString(), ")");
                                }
                            }
                            flag = bandera;
                        }
                    }
                }
                else
                {
                    this.mensaje = "Producto no existe en el registro de etiquetas(EIXL01)";
                    bandera = false;
                    flag = bandera;
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                throw new Exception(string.Concat(new string[] { "Orden: ", numLote.ToString(), " Producto:", codigoProducto, " -> ", ex.Message }));
            }
            return flag;
        }

        public TCPP2 verifcarProductoLoteQR(string pCodeQR, string usuario, string password, bool verficaCantidades = false)
        {
            TCPP2 tCPP2;
            int tPORDP;
            int numEnvio = 0;
            int numLote = 0;
            string codigoProducto = "";
            this.mensaje = "";
            int numEnvioMezcla = 0;
            try
            {
                TCPP2 obj = new TCPP2()
                {
                    MENSAJE_ESTADO = this.mensaje
                };
                obj = this.getTCCP2FromQRText(pCodeQR);
                if (obj.TPORDP <= 0)
                {
                    this.mensaje = "ERROR|Producto no existe en el registro de QR esta mal configurado";
                    obj.MENSAJE_ESTADO = this.mensaje;
                    tCPP2 = obj;
                }
                else if (!this.getDatosAdicionlesValidacion(ref obj, usuario, password))
                {
                    this.mensaje = "ERROR|Producto no existe en la consulta en el catalogo del producto IIML01";
                    obj.MENSAJE_ESTADO = this.mensaje;
                    tCPP2 = obj;
                }

                else if (obj.TPIDES.Trim() != "")
                {
                    numEnvio = obj.TPSECU;
                    numLote = obj.TPORDP;
                    numEnvioMezcla = obj.TPSECM;
                    codigoProducto = obj.TPITEM.Trim();
                    if (this.obtenerListado(numLote, usuario, password, numEnvio, codigoProducto, obj.TPSEC2).Count<TCPP2>() <= 0)
                    {
                        string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                        string UID = usuario;
                        string PWD = password;
                        string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                        Utilidades.obtenerParametro(Utilidades.ParametrosString.VALIDAR_CANTIDADES);
                        decimal.Parse(Utilidades.obtenerParametro(Utilidades.ParametrosString.FACTOR_DESVIO));
                        using (OdbcConnection conexion = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", new object[] { DSN, UID, PWD, DBQ })))
                        {
                            string consultaSQl = "";
                            conexion.Open();
                            if (numEnvio <= 0)
                            {
                                string[] str = new string[] { "SELECT SORD, 0 AS TCSECU, SPROD, SQREQ  FROM FSOL01 WHERE SORD =", null, null, null, null };
                                tPORDP = obj.TPORDP;
                                str[1] = tPORDP.ToString();
                                str[2] = " AND SPROD ='";
                                str[3] = obj.TPITEM.Trim();
                                str[4] = "'";
                                consultaSQl = string.Concat(str);
                            }
                            else
                            {
                                string[] strArrays = new string[] { "SELECT TCORDP, TCSECU, TCPROD, TCCANT  FROM TCPP WHERE TCORDP = ", null, null, null, null, null };
                                tPORDP = obj.TPORDP;
                                strArrays[1] = tPORDP.ToString();
                                strArrays[2] = "  AND TCPROD = '";
                                strArrays[3] = obj.TPITEM.Trim();
                                strArrays[4] = "' AND TCSECU = ";
                                tPORDP = obj.TPSECU;
                                strArrays[5] = tPORDP.ToString();
                                consultaSQl = string.Concat(strArrays);
                            }
                            OdbcDataReader reader = (new OdbcCommand(consultaSQl)
                            {
                                CommandType = CommandType.Text,
                                Connection = conexion
                            }).ExecuteReader();
                            if (!reader.HasRows)
                            {
                                this.mensaje = "Producto no existe en la orden/secuencia de envase";
                                obj.MENSAJE_ESTADO = this.mensaje;
                                tCPP2 = obj;
                            }
                            else
                            {
                                if (!verficaCantidades)
                                {
                                    reader.Close();
                                }
                                obj.MENSAJE_ESTADO = "";
                                if (obj.TPORDM != 999999)
                                {
                                    consultaSQl = "";
                                    if (numEnvioMezcla <= 0)
                                    {
                                        tPORDP = obj.TPORDM;
                                        consultaSQl = string.Concat("SELECT SORD, 0 AS TCSECU, SPROD, SQREQ  FROM FSOL01 WHERE SORD =", tPORDP.ToString(), " ");
                                    }
                                    else
                                    {
                                        string str1 = obj.TPORDM.ToString();
                                        tPORDP = obj.TPSECM;
                                        consultaSQl = string.Concat("SELECT TCORDP, TCSECU, TCPROD, TCCANT  FROM TCPP WHERE TCORDP = ", str1, "  AND TCSECU = ", tPORDP.ToString());
                                    }
                                    reader = (new OdbcCommand(consultaSQl)
                                    {
                                        CommandType = CommandType.Text,
                                        Connection = conexion
                                    }).ExecuteReader();
                                    if (!reader.HasRows)
                                    {
                                        this.mensaje = "Producto no existe en la orden/secuencia de mezcla ";
                                        obj.MENSAJE_ESTADO = this.mensaje;
                                        tCPP2 = obj;
                                        return tCPP2;
                                    }
                                    else
                                    {
                                        if (!verficaCantidades)
                                        {
                                            reader.Close();
                                        }
                                        obj.MENSAJE_ESTADO = "";
                                    }
                                }
                                tCPP2 = obj;
                            }
                        }
                    }
                    else
                    {
                        this.mensaje = "ERROR|Etiqueta ya tiene una captura anterior";
                        obj.MENSAJE_ESTADO = this.mensaje;
                        tCPP2 = obj;
                    }
                }
                else
                {
                    this.mensaje = "ERROR|Producto no existe en el registro en el catalogo del producto IIML01";
                    obj.MENSAJE_ESTADO = this.mensaje;
                    tCPP2 = obj;
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                throw new Exception(string.Concat(new string[] { "Orden: ", numLote.ToString(), " Producto:", codigoProducto, " -> ", ex.Message }));
            }
            return tCPP2;
        }

        public List<Producto> getListProducto(string usuario, string password)
        {
            List<Producto> listadoProductos = new List<Producto>();
            try
            {
                string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                string UID = usuario;
                string PWD = password;
                string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                dynamic cadenaConexion = string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", DSN, UID, PWD, DBQ);
                using (OdbcConnection conexion = new OdbcConnection(cadenaConexion))
                {
                    string consultaSQl = "SELECT IID, IPROD, IDESC, IUMS, IITYP, IWHS, ILOC FROM  IIML01 WHERE IID = 'IM' AND IITYP<> '6'   ";
                    OdbcCommand command = new OdbcCommand(consultaSQl);
                    command.CommandType = CommandType.Text;
                    command.Connection = conexion;
                    conexion.Open();
                    OdbcDataReader reader = command.ExecuteReader();
                    Producto oProducto = new Producto();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oProducto = new Producto();
                            oProducto.ID = reader.GetString(0).Trim();
                            oProducto.Codigo = reader.GetString(1).Trim();
                            oProducto.Descripcion = reader.GetString(2).Trim();
                            oProducto.Unidad = reader.GetString(3).Trim();
                            oProducto.Inventario = reader.GetString(4).Trim();
                            oProducto.Costo = 0;
                            oProducto.Bodega = reader.GetString(5).Trim();  
                            oProducto.Localizacion= reader.GetString(6).Trim();
                            listadoProductos.Add(oProducto);
                        }
                        reader.NextResult();
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return listadoProductos;
        }

        public TCPP2 setTCCP2FromQRText(int pOrdenProduccion, int pSecuenciaProduccion,  int pOrdenMezcla, int pSecuenciaMezcla, int pCantidad, int NumSecuenciaEtiqueta,
            string pItem)
        {
            TCPP2 tCPP2;
            try
            {
                TCPP2 registro = new TCPP2();
                registro.TPORDP = pOrdenProduccion;
                registro.TPSECU = pSecuenciaProduccion;
                registro.TPCANT = pCantidad;
                registro.TPORDM = pOrdenMezcla;
                if(registro.TPORDM == 999999) 
                {
                    registro.TPORDM = 999999;
                }
                registro.TPSECM = pSecuenciaMezcla  ;
                registro.TPSEC2 = NumSecuenciaEtiqueta;
                registro.TPITEM = pItem;

                registro.CODEQR_TEXT = registro.TPORDP.ToString().PadLeft(6,'0') + registro.TPSECU.ToString().PadLeft(2, '0') + registro.TPCANT.ToString().PadLeft(4, '0') +
                    (registro.TPORDM == 999999 ? "BULK" : registro.TPORDM.ToString().PadLeft(6,'0')) + registro.TPSECM.ToString().PadLeft(2, '0') +
                    registro.TPSEC2.ToString().PadLeft(3, '0') + registro.TPITEM.ToString().Trim();

                tCPP2 = registro;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return tCPP2;
        }

        public int secuencia2Max(int pOrdenProduccion, int pSecuenciaProduccion, int pOrdenMezcla, int pSecuenciaMezcla)
        {
            int secuencia = 0;
            try
            {

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return secuencia;
        }

        public bool getVerificacionBodegaParaEnvase(string usuario, string password, string bodega)
        {
            bool flag;
            bool bandera = false;
            try
            {
                string DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
                string UID = usuario;
                string PWD = password;
                string DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
                using (OdbcConnection conexion = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", new object[] { DSN, UID, PWD, DBQ })))
                {
                    conexion.Open();
                    OdbcDataReader reader = (new OdbcCommand(string.Concat("SELECT CCCODE FROM zccl01 WHERE CCTABL = 'BDPTCDQR' AND CCCODE='", bodega.ToString().Trim(), "'"))
                    {
                        CommandType = CommandType.Text,
                        Connection = conexion
                    }).ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            bandera = true;
                        }
                        reader.NextResult();
                    }
                    reader.Close();
                    flag = bandera;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return flag;
        }

        


    }
}