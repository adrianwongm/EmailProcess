using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA03
{
    public class TA03 : ICloneable
    {
        /// <summary>
        /// TAID (2),
        /// Campo de estado ID (TH/TZ)
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// TACOMP (2)
        /// Codigo de la compania
        /// </summary>
        public int Company { get; set; }

        /// <summary>
        /// TACODE (5)
        /// Llave del empleado
        /// </summary>
        public int Codigo { get; set; }

        /// <summary>
        /// TANAME (10)
        /// Login del usuario
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// TANAME(40)
        /// Nombres del empleado
        /// </summary>
        public string Nombres { get; set; }

        /// <summary>
        /// TACARG (30)
        /// Cargo del empleado
        /// </summary>
        public string Cargo { get; set; }

        /// <summary>
        /// TAAREA  (10)
        /// Area del empleado
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// TAUNID (10)
        /// Unidad del empleado
        /// </summary>
        public string Unidad { get; set; }

        /// <summary>
        /// TAMAIL (50)
        /// Correo del email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// TAVLMT(15,2)
        /// Monto Maximo para aprobar
        /// </summary>
        public double MontoMaximo { get; set; }

        /// <summary>
        /// TANEXT(10)
        /// Codigo del superior
        /// </summary>
        public string LoginSuperior { get; set; }

        public string DesSuperior { get; set; }

        /// <summary>
        /// TAPRFC(16)
        /// Profit Center
        /// </summary>
        public string ProfitCenter { get; set; }

        /// <summary>
        /// TAVEND (5)
        /// Codigo del Vendor
        /// </summary>
        public int CodigoVendor { get; set; }

        /// <summary>
        /// TAJEFE(1)
        /// Es Jefe
        /// </summary>
        public string EsJefe { get; set; }

        /// <summary>
        /// TAGERE(1)
        /// Es Gerente
        /// </summary>
        public string EsGerente { get; set; }

        /// <summary>
        /// Contraseña dentro del sistema, se usa para reutilizarlo como la sesion de usario dentro del sistema web
        /// </summary>
        public string PasswordSistema { get; set; }

        public string JefeAprueba { get; set; }
        public string GerenteUnidadAprueba { get; set; }
        public string GerenteAprueba { get; set; }

        public object Clone()
        {
            TA03 newTA03 = (TA03)this.MemberwiseClone();
            return newTA03;
        }
    }
}
