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

namespace EmailReadAtachment
{
    public enum ResponseEmailType
    {
        None = 0,   
        Recibido=1
    }
    internal class Program
    {
        static string cfgImapClient = ConfigurationManager.AppSettings["ImapClient"];
        static string cfgUserClient = ConfigurationManager.AppSettings["UserClient"];
        static string cfgEmailClient = ConfigurationManager.AppSettings["EmailClient"];
        static string cfgPasswordClient = ConfigurationManager.AppSettings["PasswordClient"];
        static string cfgSubject = ConfigurationManager.AppSettings["Subject"];
        static string cfgExtensionFile = ConfigurationManager.AppSettings["ExtensionFile"];
        static string cfgPathFile = ConfigurationManager.AppSettings["PathFile"];
        static string cfgSubjectResponseForReceive = ConfigurationManager.AppSettings["SubjectResponseForReceive"];
        static string cfgBodyTextForReceive = ConfigurationManager.AppSettings["BodyTextForReceive"];
        public static  void Main(string[] args)
        {
            // If using Professional version, put your serial key below.
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            // Create IMAP client.
            // 993 SSL
            ServicePointManager.ServerCertificateValidationCallback =

                 delegate (object s

                     , X509Certificate certificate

                     , X509Chain chain

                     , SslPolicyErrors sslPolicyErrors)

                 { return true; };

            using (var imap = new ImapClient(cfgImapClient,true)) 
            {
              
                //procesarExcel("C:\\Users\\Adria\\source\\repos\\EmailReadAtachment\\EmailReadAtachment\\archivos\\2022-48-12-22-48-45_PedidosCONAUTO.xls");
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
                IList<int> messages = imap.SearchMessageNumbers("SUBJECT \"" + cfgSubject+ "\"");
                Console.WriteLine($"Number of messages with 'Example' in subject: {messages.Count}");

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
                                var nombreFinalArchivo = cfgPathFile + System.DateTime.Now.ToString("yyyy-mm-dd-HH-mm-ss") + "_" + item.FileName;
                                item.Save(nombreFinalArchivo);
                                //Validar arhivo
                                System.IO.FileInfo excel = new System.IO.FileInfo(nombreFinalArchivo);
                                if (excel.Exists)
                                {
                                    var receiver = message.From[0].Address;
                                    var respuestaEnvio = RespuestaEmail(receiver, ResponseEmailType.Recibido);
                                    ExcelReader.Lector.procesarExcel(excel.FullName, receiver);
                                    if (ExcelReader.Lector.ErrorProcessList.Count() > 0)
                                    {
                                        //Enviar correo con listado de errores
                                    }
                                    else
                                    {
                                        //Correo aceptado pendiente de aprobacion final
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

        static  bool RespuestaEmail(string receiver, ResponseEmailType responseEmailType)
        {
            try
            {
                //Responder correo
                GemBox.Email.MailMessage messageReponse = new GemBox.Email.MailMessage(
                            new GemBox.Email.MailAddress(cfgEmailClient, "Sender"),
                             new GemBox.Email.MailAddress(receiver, "First receiver"));
                if (responseEmailType == ResponseEmailType.Recibido)
                {
                    messageReponse.Subject = cfgSubjectResponseForReceive;
                    messageReponse.BodyText = cfgBodyTextForReceive;
                } 
                /*var mailSender = new System.Net.Mail.SmtpClient
                {
                    Host = cfgImapClient,
                    // Port = 587,
                    EnableSsl = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(cfgUserClient, cfgPasswordClient)
                };
                mailSender.Send(new System.Net.Mail.MailMessage(cfgEmailClient, receiver) { Subject = messageReponse.Subject, Body = messageReponse.BodyText });*/
                using (GemBox.Email.Smtp.SmtpClient smtp = new GemBox.Email.Smtp.SmtpClient(cfgImapClient,25, ConnectionSecurity.None))
                {
                    smtp.Connect();
                    smtp.Authenticate(cfgUserClient, cfgPasswordClient);
                    smtp.SendMessage(messageReponse);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            } 
        }
    
       static void procesarExcel(string filePath)
        {
            try
            {
                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                ExcelFile workbook = ExcelFile.Load(filePath);
                for (int sheetIndex = 0; sheetIndex < workbook.Worksheets.Count; sheetIndex++)
                {
                    ExcelWorksheet worksheet = workbook.Worksheets[sheetIndex];
                    ExcelRow rowCliente = worksheet.Rows[0];
                    ExcelRow rowClienteDireccion = worksheet.Rows[2];
                    var valorCliente = rowCliente.Cells["C"].Value;
                    var valorDireccionCliente = rowClienteDireccion.Cells["C"].Value;
                    int limite = 200;
                    var rangoContenido = worksheet.Cells.GetSubrange("A6", "J"+ limite);
                    
                    CellRangeEnumerator enumerator = rangoContenido.GetReadEnumerator();
                    
                    while (enumerator.MoveNext())
                    {
                        ExcelCell cell = enumerator.Current;
                        if (cell.Value!= null){
                            if (cell.Value.ToString().Contains("Moneda"))
                            {
                                  limite = cell.Row.Index;
                                  break;
                            }
                        }
                    }
                    //Columnas de productos
                    List<Tuple<string, decimal>> listaPedido= new List<Tuple<string, decimal>>();
                    List<string> listaEnBlanco = new List<string>();
                    var rangoCodigoProducto = worksheet.Cells.GetSubrange("A6", "A"+ limite);
                    var rangoCantidadProducto = worksheet.Cells.GetSubrange("I6", "I" + limite);
                    //rangoContenido = worksheet.Cells.GetSubrange("A6", "A" + limite);
                    foreach (var celda in rangoCodigoProducto)
                    {
                        bool blankRegister = true;
                        if ( celda!= null)
                        {
                            if(celda.ToString().Trim() != "")
                            {
                                var codigoProducto = celda.Value.ToString();
                                var cantidadProducto = decimal.Parse(rangoCantidadProducto.Where(x => x.Column.Name == "I" && x.Row.Index == celda.Row.Index).FirstOrDefault().Value?.ToString());
                                listaPedido.Add(new Tuple<string, decimal>(codigoProducto, cantidadProducto));
                                blankRegister= false;
                            } 
                        }
                        if (blankRegister)
                        {
                            listaEnBlanco.Add(celda.Column.Name + "" +celda.Row.Index.ToString());
                        }
                    }

                    if(listaEnBlanco.Count>0)
                    {
                        //Notifcar registros en blancos
                    }

                    if (listaPedido.Count > 0)
                    {
                        //Procesar registros en blancos

                    }
                    else
                    {
                        //Notificar no hay detalle
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
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
