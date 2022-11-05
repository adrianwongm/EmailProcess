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

namespace EmailReadAtachment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // If using Professional version, put your serial key below.
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            // Create IMAP client.
            // 993 SSL
            using (var imap = new ImapClient("swissmail.swissoil.local"))
            //using (var imap = new ImapClient("201.234.220.12", 143))
            {
               
                //  Connect and sign to IMAP server.
                imap.Connect();
                imap.Authenticate("ruanotv", ".$circ6HAVO7");  //ruanotv@swissoil.com.ec

                // Select INBOX folder.
                imap.SelectInbox();

                // Read the number of currently available emails in selected mailbox folder.
                int count = imap.SelectedFolder.Count;

                Console.WriteLine(" NO. |     DATE     |          SUBJECT          ");
                Console.WriteLine("------------------------------------------------");

                // Download and receive all email messages from selected mailbox folder.
                //for (int number = 1; number <= count; number++)
                for (int number = count; number > 0; number--)
                {
                    GemBox.Email.MailMessage message = imap.GetMessage(number);

                    // Read and display email's date and subject.
                    if(message.Subject== "PEDIDOS DE CLIENTES SWISSOIL" || message.Subject == "PEDIDOS DE CLIENTE SWISSOIL")
                    {
                        Console.WriteLine($"  {number}  |  {message.Date.ToShortDateString()}  |  {message.Subject}");
                        foreach (var item in message.Attachments)
                        {
                            if (!string.IsNullOrEmpty(item.FileName) && item.FileName.Contains(".xls"))
                            {
                                System.IO.FileInfo file = new System.IO.FileInfo(item.FileName);
                                if (file.Extension == ".xls" || file.Extension == ".xlsx" || file.Extension == ".xlsm")
                                {
                                    var nombreFinalArchivo = "C:\\Users\\Adria\\source\\repos\\EmailReadAtachment\\EmailReadAtachment\\archivos\\" + System.DateTime.Now.ToString("yyyy-mm-dd-HH-mm-ss") + "_" + item.FileName;
                                    item.Save(nombreFinalArchivo) ;
                                    //Validar arhivo
                                    System.IO.FileInfo excel = new System.IO.FileInfo(nombreFinalArchivo);

                                    //Responder correo
                                    GemBox.Email.MailMessage messageReponse = new GemBox.Email.MailMessage(
                                                new GemBox.Email.MailAddress("ruanotv@swissoil.com.ec", "Sender"),
                                                 new GemBox.Email.MailAddress(message.From[0].Address, "First receiver"));
                                    messageReponse.Subject = "Notificacion de archivo recibido";
                                    messageReponse.BodyText = "Archivo recibido, próximamente lo procesaremos.";
                                    SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

                                    // Load Excel workbook from file's path.
                                    ExcelFile workbook = ExcelFile.Load(nombreFinalArchivo);

                                    // Iterate through all worksheets in a workbook.
                                    foreach (ExcelWorksheet worksheet in workbook.Worksheets)
                                    {
                                        // Display sheet's name.
                                        Console.WriteLine("{1} {0} {1}\n", worksheet.Name, new string('#', 30));

                                        // Iterate through all rows in a worksheet.
                                        foreach (ExcelRow row in worksheet.Rows)
                                        {
                                            // Iterate through all allocated cells in a row.
                                            foreach (ExcelCell cell in row.AllocatedCells)
                                            {
                                                // Read cell's data.
                                                string value = cell.Value?.ToString() ?? "EMPTY";

                                                // Display cell's value and type.
                                                value = value.Length > 15 ? value.Remove(15) + "…" : value;
                                                Console.Write($"{value} [{cell.ValueType}]".PadRight(30));
                                            }

                                            Console.WriteLine();
                                        }
                                    }


                                    /*using (var smtp = new GemBox.Email.Smtp.SmtpClient("swissmail.swissoil.local",25,ConnectionSecurity.None))
                                    {
                                        //  Connect and sign to the SMTP server.
                                        smtp.Connect();
                                        smtp.Authenticate("ruanotv", ".$circ6HAVO7");

                                        // Create and send a new email.
                                        var messager2 = new GemBox.Email.MailMessage(); 
                                        smtp.SendMessage(messager2);
                                    }*/

                                    new System.Net.Mail.SmtpClient
                                    {
                                        Host = "swissmail.swissoil.local",
                                       // Port = 587,
                                        EnableSsl = false,
                                        DeliveryMethod = SmtpDeliveryMethod.Network,
                                        UseDefaultCredentials = false,
                                        Credentials = new NetworkCredential("ruanotv", ".$circ6HAVO7")
                                    }.Send(new System.Net.Mail.MailMessage("ruanotv@swissoil.com.ec", message.From[0].Address) { Subject = messageReponse.Subject, Body = messageReponse.BodyText });
                                    //
                                }
                            }
                        }
                    } 
                }
            }
        }
    }
}
