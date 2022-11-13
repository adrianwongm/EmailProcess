using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA03
{
    public class TA03_BLL
    {
        TA03 UsuarioEmplSession { get; set; }
        public TA03_BLL(TA03 pUsuarioEmplSession)
        {
            UsuarioEmplSession = pUsuarioEmplSession;
        }
        public TA03 obtenerTA03(string usuario, string password)
        {
            TA03 oTA03 = new TA03();
            ConexionODBCTA03 oCon = new ConexionODBCTA03(UsuarioEmplSession);
            List<TA03> listadoUsuarios = oCon.getListadoTA03();
            oTA03 = listadoUsuarios.Where(p => p.Login == usuario).FirstOrDefault();
            oTA03.PasswordSistema = password;
            //Caso normal
            if (oTA03.EsJefe != "Y" && oTA03.EsGerente != "Y") {
                TA03 JefoTA03 = listadoUsuarios.Where(p => p.Login == oTA03.LoginSuperior).FirstOrDefault();
                oTA03.JefeAprueba = JefoTA03.Login;
                TA03 GeUfoTA03 = listadoUsuarios.Where(p => p.Login == JefoTA03.LoginSuperior).FirstOrDefault();
                oTA03.GerenteUnidadAprueba = GeUfoTA03.Login;
                TA03 GefoTA03 = listadoUsuarios.Where(p => p.Login == GeUfoTA03.LoginSuperior).FirstOrDefault();
                oTA03.GerenteAprueba = GefoTA03.Login;
            }
            //Caso Jefe
            if (oTA03.EsJefe == "Y" && oTA03.EsGerente != "Y")
            { 
                oTA03.JefeAprueba = "";
                TA03 GeUfoTA03 = listadoUsuarios.Where(p => p.Login == oTA03.LoginSuperior).FirstOrDefault();
                oTA03.GerenteUnidadAprueba = GeUfoTA03.Login;
                TA03 GefoTA03 = listadoUsuarios.Where(p => p.Login == GeUfoTA03.LoginSuperior).FirstOrDefault();
                oTA03.GerenteAprueba = GefoTA03.Login;
            }
            //Caso Gerente Unidad
            if (oTA03.EsJefe != "Y" && oTA03.EsGerente == "Y" && oTA03.LoginSuperior.Trim() != "")
            {
                oTA03.JefeAprueba = ""; 
                oTA03.GerenteUnidadAprueba = "";
                TA03 GefoTA03 = listadoUsuarios.Where(p => p.Login == oTA03.LoginSuperior).FirstOrDefault();
                oTA03.GerenteAprueba = GefoTA03.Login;
            }
            //Caso Gerente Unidad
            if (oTA03.EsJefe != "Y" && oTA03.EsGerente == "Y" && oTA03.LoginSuperior.Trim() == "")
            {
                oTA03.JefeAprueba = "";
                oTA03.GerenteUnidadAprueba = "";
                oTA03.GerenteAprueba = oTA03.Login;
            }

            return oTA03;
        }
    }
}
