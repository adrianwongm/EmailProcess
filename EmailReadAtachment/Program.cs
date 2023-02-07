using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GemBox.Email;
using GemBox.Email.Imap;
using GemBox.Email.Pop;
using GemBox.Email.Smtp;
using GemBox.Email.Security;
using GemBox.Spreadsheet;
using System.Configuration;
using System.Threading;
using ExcelReader;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using NotificacionEmailMessage;
using LogErrores;

namespace EmailReadAtachment
{ 
    internal class Program
    {
        static string cfgImapClient = ConfigurationManager.AppSettings["ImapClient"];
        static string cfgUserClient = ConfigurationManager.AppSettings["UserClient"];
        static string cfgEmailClient = ConfigurationManager.AppSettings["EmailClient"];
        static string cfgPasswordClient = ConfigurationManager.AppSettings["PasswordClient"];
        static string cfgSubject = ConfigurationManager.AppSettings["Subject"];
        static string cfgExtensionFile = ConfigurationManager.AppSettings["ExtensionFile"];
        static string cfgPathFile = ConfigurationManager.AppSettings["PathFile"];
        static string cfgPathTxtBase = ConfigurationManager.AppSettings["PathTxtBase"];


        public static  void Main(string[] args)
        {
            // Create IMAP client.
            // 993 SSL
            ServicePointManager.ServerCertificateValidationCallback =

                 delegate (object s

                     , X509Certificate certificate

                     , X509Chain chain

                     , SslPolicyErrors sslPolicyErrors)

                 { return true; };

            try
            { 
                // If using Professional version, put your serial key below.
                ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                using (var imap = new ImapClient(cfgImapClient, true))
                {
                    //  Connect and sign to IMAP server.
                    imap.Connect();
                    imap.Authenticate(cfgUserClient, cfgPasswordClient);  //ruanotv@swissoil.com.ec

                    // Select INBOX folder.
                    imap.SelectInbox();
                    // RespuestaEmail("adrian.wong.m@gmail.com", ResponseEmailType.Recibido);
                    using (var listener = new ImapListener(imap))
                    {
                        listener.MessagesChanged += OnMessagesChanged;
                        imap.IdleEnable();

                        Console.WriteLine("Press any key to exit...");
                        Console.ReadKey();

                        imap.IdleDisable();
                        listener.MessagesChanged -= OnMessagesChanged;
                    }

                    // Read the number of currently available emails in selected mailbox folder.
                    int count = imap.SelectedFolder.Count;

                    Console.WriteLine(" NO. |     DATE     |          SUBJECT          ");
                    Console.WriteLine("------------------------------------------------");

                    IList<ImapMessageInfo> infos = imap.ListMessages();

                    // Display messages information.
                    foreach (ImapMessageInfo info in infos)
                        Console.WriteLine($"{info.Number} - [{info.Flags}] - [{info.Uid}] - {info.Size} Byte(s)");
                    // List INBOX folder flags.
                    IList<ImapFolderInfo> folders = imap.ListFolders();
                    foreach (string flag in imap.SelectedFolder.Flags)
                        Console.WriteLine(flag);
                    IList<int> messages = imap.SearchMessageNumbers("SUBJECT \"" + cfgSubject + "\"");
                    Console.WriteLine($"Number of messages with 'Example' in subject: {messages.Count}");

                }
            }
            catch (Exception ex)
            {
                funcionControlErrores(ex.Message);
            }

        

         
        }

        static void OnMessagesChanged(object sender, ImapListenerEventArgs e)
        {
            foreach (var info in e.NewMessages)
            {
                Console.WriteLine($"Message '{info.Uid}' received.");
                var cliente = (((EmailReadAtachment.ImapListener)sender)).client;
                var message = cliente.GetMessage(info.Uid);
                if (message.Subject.Contains(cfgSubject))
                {
                    foreach (var item in message.Attachments)
                    {
                        if (!string.IsNullOrEmpty(item.FileName) && item.FileName.Contains(cfgExtensionFile))
                        {
                            Console.WriteLine($"  {info.Number}  |  {message.Date.ToShortDateString()}  |  {message.Subject}");
                            System.IO.FileInfo file = new System.IO.FileInfo(item.FileName);
                            if (file.Extension == ".xls" || file.Extension == ".xlsx" || file.Extension == ".xlsm") //Tipos de archivos de excel
                            {
                                var nombreFinalArchivo = cfgPathFile + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "_" + item.FileName;
                                item.Save(nombreFinalArchivo);
                                //Validar arhivo 
                                System.IO.FileInfo excel = new System.IO.FileInfo(nombreFinalArchivo);
                                if (excel.Exists)
                                {
                                    //Grabar txt Base temporal
                                    saveLocalTxt(info.Uid);
                                    //Notificar archivo recibido
                                    var receiver = message.From[0].Address;
                                    var respuestaEnvio = NotificationEmail.RespuestaEmail(receiver, ResponseEmailType.Recibido, null, null);
                                    //Validar Excel y procesar Excel
                                    var archivoConautoDetallados = ExcelReader.Lector.procesarExcel(excel.FullName, receiver);
                                    //Notificacion de envio o error por email
                                        var codigoHojasErrores = ExcelReader.Lector.ErrorProcessList
                                            //.Where(x => !string.IsNullOrEmpty(x.NombreHoja))
                                            .Select(p => p.NombreHoja).Distinct();

                                        var archivoOCConautoDetallado_Error = archivoConautoDetallados.Where(x => codigoHojasErrores.Contains(x.HojaOrigen)).Select(p => p.OrdenCompra).ToList();
                                        if (archivoOCConautoDetallado_Error?.Count > 0)
                                        {
                                            var respuestaError = NotificationEmail.RespuestaEmail(receiver, ResponseEmailType.Error,
                                                        string.Join(", ", archivoOCConautoDetallado_Error.ToArray()),
                                                        ExcelReader.Lector.ErrorProcessList.Select(x => $"{(string.IsNullOrEmpty(x.NombreHoja) ? "El documento " : "La hoja " + x.NombreHoja)}  presenta: {x.MensajeError}").ToList()
                                                        );
                                            foreach (var errorProcess in ExcelReader.Lector.ErrorProcessList)
                                            {
                                                var archivoConautoDetallado = archivoConautoDetallados.Where(x => x.HojaOrigen == errorProcess.NombreHoja).FirstOrDefault();
                                                funcionControlErrores($"Orden {archivoConautoDetallado?.OrdenCompra} : " + errorProcess?.NombreHoja??"" + " :" + errorProcess?.MensajeError??"");
                                            }
                                        } 
                                        //Correo aceptado pendiente de aprobacion final
                                        var archivoOCConautoDetallado_OK = archivoConautoDetallados.Where(x => !codigoHojasErrores.Contains(x.HojaOrigen)).Select(p=>p.OrdenCompra).ToList();
                                        if(archivoOCConautoDetallado_OK?.Count>0)
                                        { 
                                            var respuestaProcesado = NotificationEmail.RespuestaEmail(receiver, ResponseEmailType.Procesado,
                                                            string.Join(", ", archivoOCConautoDetallado_OK.ToArray()), null);
                                        }
                                   
                                }
                            }

                        }
                    }

                }
            } 
            foreach (var info in e.OldMessages)
                Console.WriteLine($"Message '{info.Uid}' deleted.");
        } 
        
        static void saveLocalTxt(string uid)
        {
            using (FileStream fs = new FileStream(cfgPathTxtBase, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(uid);
                }
            }
        }
        static bool readExistUIDLocalTxt(string uid)
        {
            string[] lines = System.IO.File.ReadAllLines(cfgPathTxtBase);
            if(lines.ToList().Exists(x => x == uid))
            {
                return true;
            }
            return false;
        }

        public static void funcionControlErrores(string pMensaje)
        {
            pMensaje = "Usuario: " + cfgUserClient + " - " + DateTime.Now.ToString() + " - " + pMensaje;
            ProgramErrorClass programa = new ProgramErrorClass();
            programa.SetLogMethod(
                    new logDelegate(FormasLog.FileLog));
            programa.Run(pMensaje);
        }
    }

    class ImapListener : IDisposable
    {
        public readonly ImapClient client;
        private Dictionary<string, ImapMessageInfo> messages;
        private bool running;
        private Thread listenerThread;

        public event EventHandler<ImapListenerEventArgs> MessagesChanged;

        public ImapListener(ImapClient client)
        {
            this.client = client;
            this.messages = this.GetMessages();
            this.running = true;
            this.listenerThread = new Thread(Listen) { IsBackground = true };
            this.listenerThread.Start();
        }

        private Dictionary<string, ImapMessageInfo> GetMessages()
        {
            return this.client.ListMessages().ToDictionary(info => info.Uid, info => info);
        }

        private void Listen()
        {
            while (this.running)
            {
                Thread.Sleep(100);

                // Compare the previous and current message count of the selected folder.
                int comparison = this.client.SelectedFolder.Count.CompareTo(this.messages.Count);
                if (comparison == 0)
                    continue;

                var currentMessages = this.GetMessages();
                var emptyMessages = Enumerable.Empty<ImapMessageInfo>();

                // New message(s) was added.
                if (comparison > 0)
                {
                    var newMessages = currentMessages
                        .Where(message => !this.messages.ContainsKey(message.Key))
                        .Select(message => message.Value);
                    this.MessagesChanged?.Invoke(this, new ImapListenerEventArgs(newMessages, emptyMessages));
                }
                // Old message(s) was deleted.
                else
                {
                    var oldMessages = this.messages
                        .Where(message => !currentMessages.ContainsKey(message.Key))
                        .Select(message => message.Value);
                    this.MessagesChanged?.Invoke(this, new ImapListenerEventArgs(emptyMessages, oldMessages));
                }

                this.messages = currentMessages;
            }
        }

        public void Dispose()
        {
            this.running = false;
            this.listenerThread?.Join(5000);
            this.listenerThread = null;
        }
    }

    class ImapListenerEventArgs : EventArgs
    {
        public IEnumerable<ImapMessageInfo> NewMessages { get; }
        public IEnumerable<ImapMessageInfo> OldMessages { get; }
        public ImapListenerEventArgs(IEnumerable<ImapMessageInfo> newMessages, IEnumerable<ImapMessageInfo> oldMessages)
        {
            this.NewMessages = newMessages;
            this.OldMessages = oldMessages;
        }
    }
}
