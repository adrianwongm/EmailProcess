using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA03
{
    public class ConexionODBCTA03  
    {
        public string mensaje = "";
        public string DSN = "";
        public string DBQ = "";
        public  TA03 UsuarioEmplSession { get; set; }
        private bool EsPrueba = false;
        public ConexionODBCTA03(TA03 pUsuarioEmplSession)
        {
            UsuarioEmplSession = pUsuarioEmplSession;
            seteoVariablesConexion();
        }
        //public ConexionODBCTA03()
        //{
        //    seteoVariablesConexion();
        //}
        //public ConexionODBCTA03(bool pPrueba)
        //{
        //    EsPrueba = pPrueba;
        //    seteoVariablesConexion();
        //}
        private void seteoVariablesConexion()
        {
            DSN = Utilidades.obtenerParametro(Utilidades.ParametrosString.DNS);
            //UsuarioEmplSession = new TA03();
            //UsuarioEmplSession.Login = UsuarioEmplSession.Login;// Utilidades.obtenerParametro(Utilidades.ParametrosString.UID);
            //UsuarioEmplSession.PasswordSistema = UsuarioEmplSession.PasswordSistema;// Utilidades.obtenerParametro(Utilidades.ParametrosString.PWD);
            if (EsPrueba)
            {
                DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQBPRUEBA);
            }
            else
            {
                DBQ = Utilidades.obtenerParametro(Utilidades.ParametrosString.DQB);
            }
        }

        public List<TA03> cargaDataPrueba()
        {
            List<TA03> ListaPruebaGerente = Enumerable.Range(1, 10).Select(i => new TA03
            {
                Area = "00001",
                Cargo = "CARGO GERENCIAL",
                Codigo = i + 100,
                Company = 1,
                Email = "email" + (i + 100).ToString() + "@gmail.com",
                EsGerente = "S",
                EsJefe = "",
                ID = "TH",
                Login = "login" + (i + 100).ToString(),
                LoginSuperior = "SDIAZ",
                MontoMaximo = 0,
                Nombres = "NOMBRES" + (i + 100).ToString(),
                ProfitCenter = "00001",
                Unidad = "00001"
            }).ToList();

            List<TA03> ListaPruebajefe = Enumerable.Range(1, 10).Select(i => new TA03
            {
                Area = "00002",
                Cargo = "CARGO JEFETURA",
                Codigo = i + 200,
                Company = 1,
                Email = "email" + (i + 200).ToString() + "@gmail.com",
                EsGerente = "",
                EsJefe = "S",
                ID = "TH",
                Login = "login" + (i + 200).ToString(),
                LoginSuperior = "login" + (i + 100).ToString(),
                MontoMaximo = 0,
                Nombres = "NOMBRES" + (i + 200).ToString(),
                ProfitCenter = "00002",
                Unidad = "00002"
            }).ToList();

            List<TA03> ListaPruebaEmpleados = Enumerable.Range(1, 10).Select(i => new TA03
            {
                Area = "00003",
                Cargo = "CARGO EMPLEADO",
                Codigo = i + 300,
                Company = 1,
                Email = "email" + (i + 300).ToString() + "@gmail.com",
                EsGerente = "",
                EsJefe = "",
                ID = "TH",
                Login = "login" + (i + 300).ToString(),
                LoginSuperior = "login" + (i + 200).ToString(),
                MontoMaximo = 0,
                Nombres = "NOMBRES" + (i + 300).ToString(),
                ProfitCenter = "00003",
                Unidad = "00003"
            }).ToList();

            List<TA03> listaEmpleadosFinal = ListaPruebaEmpleados.AsEnumerable().Concat(ListaPruebajefe.AsEnumerable().Concat(ListaPruebaGerente.AsEnumerable())).ToList();
            listaEmpleadosFinal.Add(new TA03()
            {
                Area = "00004",
                Cargo = "CARGO GERENTE GENERAL",
                Codigo = 9999,
                Company = 1,
                Email = "email" + (9999).ToString() + "@gmail.com",
                EsGerente = "S",
                EsJefe = "",
                ID = "TH",
                Login = "login" + (9999).ToString(),
                LoginSuperior = "",
                MontoMaximo = 0,
                Nombres = "NOMBRES" + (9999).ToString(),
                ProfitCenter = "00004",
                Unidad = "00004"
            });
            return listaEmpleadosFinal;
        }

     
        public List<TA03> getListadoTA03()
        {
            if (EsPrueba)
            {
                return cargaDataPrueba();
            }

            try
            {
                List<TA03> listadoUsuarioEmpleados = new List<TA03>();
                TA03 pUsuarioEmpleado = new TA03();
                dynamic cadenaConexion = string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", DSN, UsuarioEmplSession.Login, UsuarioEmplSession.PasswordSistema, DBQ);
                using (OdbcConnection conexion = new OdbcConnection(cadenaConexion))
                {
                    OdbcCommand command = new OdbcCommand("SELECT TAID,   TACOMP, TACODE, TAUSER, TANAME, " +
                                                                 "TACARG, TAAREA, TAUNID, TAMAIL, TAVLMT, " +
                                                                 "TANEXT, TAPRFC, TAVEND, TAJEFE, TAGERE FROM TA03");// necesito un estado
                    command.CommandType = CommandType.Text;
                    command.Connection = conexion;
                    conexion.Open();
                    OdbcDataReader reader = command.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            pUsuarioEmpleado = new TA03();
                            pUsuarioEmpleado.ID = reader.GetString(0).Trim();
                            pUsuarioEmpleado.Company = reader.GetInt32(1);
                            pUsuarioEmpleado.Codigo = reader.GetInt32(2);
                            pUsuarioEmpleado.Login = reader.GetString(3).Trim();
                            pUsuarioEmpleado.Nombres = reader.GetString(4).Trim();
                            pUsuarioEmpleado.Cargo = reader.GetString(5).Trim();
                            pUsuarioEmpleado.Area = reader.GetString(6).Trim();
                            pUsuarioEmpleado.Unidad = reader.GetString(7).Trim();
                            pUsuarioEmpleado.Email = reader.GetString(8).Trim();
                            pUsuarioEmpleado.MontoMaximo = reader.GetDouble(9);
                            pUsuarioEmpleado.LoginSuperior = reader.GetString(10).Trim();
                            pUsuarioEmpleado.ProfitCenter = reader.GetString(11).Trim();
                            pUsuarioEmpleado.CodigoVendor = reader.GetInt32(12);
                            pUsuarioEmpleado.EsJefe = reader.GetString(13).Trim();
                            pUsuarioEmpleado.EsGerente = reader.GetString(14).Trim();
                            listadoUsuarioEmpleados.Add(pUsuarioEmpleado);
                        }
                        reader.NextResult();
                    }
                    return listadoUsuarioEmpleados;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                throw new Exception(ex.Message);
            }
        }

        public List<TA03> searchListadoTA03(List<TA03> pListado, string pOpcionBusqueda, object parametros)
        {
            List<TA03> listadoSearchUsuarioEmpleados = new List<TA03>();
            if (pListado.Count == 0)
            {
                listadoSearchUsuarioEmpleados = getListadoTA03();
            }

            if (pOpcionBusqueda == "NIVEL")
            {
                if (parametros.ToString() == "0")
                {
                    var a = pListado.Where(x => x.EsGerente != "" || x.EsJefe != "").ToList();
                    listadoSearchUsuarioEmpleados = a;
                }
                else if (parametros.ToString() == "J")
                {
                    var a = pListado.Where(x => x.EsGerente != "").ToList();
                    listadoSearchUsuarioEmpleados = a;
                }
                else if (parametros.ToString() == "G")
                {
                    var a = pListado.Where(x => x.EsGerente != "" && x.LoginSuperior == "").ToList();
                    listadoSearchUsuarioEmpleados = a;
                }
            }
            if (pOpcionBusqueda == "LOGIN")
            {
                if (parametros.ToString() == "")
                {
                    var a = pListado;
                    listadoSearchUsuarioEmpleados = a;
                }
                if (parametros.ToString() != "")
                {
                    var a = pListado.Where(x => x.Login == parametros.ToString()).ToList();
                    listadoSearchUsuarioEmpleados = a;
                }
            }
            if (pOpcionBusqueda == "LOGINSUPERIOR")
            {
                if (parametros.ToString() != "")
                {
                    var a = pListado.Where(x => x.LoginSuperior == parametros.ToString()).ToList();
                    listadoSearchUsuarioEmpleados = a;
                }
            }
            return listadoSearchUsuarioEmpleados;
        }
        public TA03 getTA03(int pCodigoEmpleado)
        {
            try
            {
                TA03 pUsuarioEmpleado = new TA03();
                dynamic cadenaConexion = string.Format("DSN={0};Uid={1};Pwd={2};DBQ={3}", DSN, UsuarioEmplSession.Login, UsuarioEmplSession.PasswordSistema, DBQ);
                using (OdbcConnection conexion = new OdbcConnection(cadenaConexion))
                {
                    OdbcCommand command = new OdbcCommand("SELECT TAID,   TACOMP, TACODE, TAUSER, TANAME, " +
                                                                 "TACARG, TAAREA, TAUNID, TAMAIL, TAVLMT, " +
                                                                 "TANEXT, TAPRFC, TAVEND, TAJEFE, TAGERE FROM TA03 WHERE TACODE = " + pCodigoEmpleado.ToString() + "");// necesito un estado
                    command.CommandType = CommandType.Text;
                    command.Connection = conexion;
                    conexion.Open();
                    OdbcDataReader reader = command.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            pUsuarioEmpleado.ID = reader.GetString(0).Trim();
                            pUsuarioEmpleado.Company = reader.GetInt32(1);
                            pUsuarioEmpleado.Codigo = reader.GetInt32(2);
                            pUsuarioEmpleado.Login = reader.GetString(3);
                            pUsuarioEmpleado.Nombres = reader.GetString(4);
                            pUsuarioEmpleado.Cargo = reader.GetString(5);
                            pUsuarioEmpleado.Area = reader.GetString(6);
                            pUsuarioEmpleado.Unidad = reader.GetString(7);
                            pUsuarioEmpleado.Email = reader.GetString(8);
                            pUsuarioEmpleado.MontoMaximo = reader.GetDouble(9);
                            pUsuarioEmpleado.LoginSuperior = reader.GetString(10);
                            pUsuarioEmpleado.ProfitCenter = reader.GetString(11);
                            pUsuarioEmpleado.CodigoVendor = reader.GetInt32(12);
                            pUsuarioEmpleado.EsJefe = reader.GetString(13);
                            pUsuarioEmpleado.EsGerente = reader.GetString(14);
                        }
                        reader.NextResult();
                    }
                    return pUsuarioEmpleado;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                throw new Exception(ex.Message);
            }
        }
 
    }
}
