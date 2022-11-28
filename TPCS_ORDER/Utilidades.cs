using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TPCS_ORDER
{
    public class Utilidades
    {
        public Utilidades()
        {
        }

        public enum ParametrosString
        {
            DQB,
            DNS,
            DQBPRUEBA,
            CONEXIONSQLPRUEBA,
            CONEXIONSQL,
            UID,
            PWD,
            VALIDAR_CANTIDADES,
            FACTOR_DESVIO,
            PROCESS01
        };
        public enum MotoresString
        {
            DB2_ODBC,
            SQL_SERVER,
            ORACLE,
            MYSQL,
            MARIADB,
            MOTOR
        };

       

        public static string obtenerMotor(MotoresString valor)
        {
            try
            {
                string StrConnectionString = "";
                string providerName = "";
                string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "bin", "Datalayer.config");
                if (!File.Exists(configFile))
                {
                    configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Datalayer.config");
                }
                dynamic doc = new XmlDocument();
                doc.Load(configFile);
                XmlNode node = default(XmlNode);

                switch (valor)
                {
                    case MotoresString.DB2_ODBC:
                        node = doc.SelectSingleNode("/configuration/appSettings/add[@key='MotorDB2_Odbc']");
                        return node.Attributes["value"].Value;
                    case MotoresString.SQL_SERVER:
                        node = doc.SelectSingleNode("/configuration/appSettings/add[@key='MotorSQL_Server']");
                        return node.Attributes["value"].Value;
                    case MotoresString.MOTOR:
                        node = doc.SelectSingleNode("/configuration/appSettings/add[@key='MotorOrigen']");
                        return node.Attributes["value"].Value;
                    default:
                        node = doc.SelectSingleNode("");
                        break;
                }

                string env = node.Attributes["value"].Value;

                node = doc.SelectSingleNode(string.Format("/configuration/connectionStrings/add[@name='{0}']", env));
                StrConnectionString = node.Attributes["connectionString"].Value;
                providerName = node.Attributes["providerName"].Value;
                return StrConnectionString;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool obtenerTipoPrueba()
        {
            try
            {
                string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "bin", "Datalayer.config");
                if (!File.Exists(configFile))
                {
                    configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Datalayer.config");
                }
                dynamic doc = new XmlDocument();
                doc.Load(configFile);
                XmlNode node = default(XmlNode);

                node = doc.SelectSingleNode("/configuration/appSettings/add[@key='TipoPrueba']");
                return Convert.ToBoolean(node.Attributes["value"].Value);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool IsNumeric(string s)
        {
            try
            {
                float output;
                return float.TryParse(s, out output);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string obtenerParametro(ParametrosString valor)
        {
            try
            {
                string StrConnectionString = "";
                string providerName = "";
                string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "bin", "Datalayer.config");
                if (!File.Exists(configFile))
                {
                    configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Datalayer.config");
                }
                dynamic doc = new XmlDocument();
                doc.Load(configFile);
                XmlNode node = default(XmlNode);

                switch (valor)
                {
                    case ParametrosString.DQB:
                        node = doc.SelectSingleNode("/configuration/appSettings/add[@key='ConnectionStringDQB']");
                        return node.Attributes["value"].Value; 
                    case ParametrosString.DNS:
                        node = doc.SelectSingleNode("/configuration/appSettings/add[@key='ConnectionStringDNS']");
                        return node.Attributes["value"].Value;
                    case ParametrosString.DQBPRUEBA:
                        node = doc.SelectSingleNode("/configuration/appSettings/add[@key='ConnectionStringDQBPRUEBA']");
                        return node.Attributes["value"].Value;
                    case ParametrosString.CONEXIONSQL:
                        node = doc.SelectSingleNode("/configuration/appSettings/add[@key='ConnectionStringSQL']");
                        break;
                    case ParametrosString.UID:
                        node = doc.SelectSingleNode("/configuration/appSettings/add[@key='UID']");
                        return node.Attributes["value"].Value;
                    case ParametrosString.PWD:
                        node = doc.SelectSingleNode("/configuration/appSettings/add[@key='PWD']");
                        return node.Attributes["value"].Value;
                    case ParametrosString.VALIDAR_CANTIDADES:
                        node = doc.SelectSingleNode("/configuration/appSettings/add[@key='VALIDAR_CANTIDADES']");
                        return node.Attributes["value"].Value;
                    case ParametrosString.FACTOR_DESVIO:
                        node = doc.SelectSingleNode("/configuration/appSettings/add[@key='FACTOR_DESVIO']");
                        return node.Attributes["value"].Value;
                    case ParametrosString.PROCESS01:
                        node = doc.SelectSingleNode("/configuration/appSettings/add[@key='PROCESS01']");
                        return node.Attributes["value"].Value;
                    default:
                        node = doc.SelectSingleNode("");
                        break;
                }

                string env = node.Attributes["value"].Value;

                node = doc.SelectSingleNode(string.Format("/configuration/connectionStrings/add[@name='{0}']", env));
                StrConnectionString = node.Attributes["connectionString"].Value;
                providerName = node.Attributes["providerName"].Value;
                return StrConnectionString;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
