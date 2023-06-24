using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace ViviCampomarino.ServizioNavetta {
    internal class NavettaImpostazioni {
        private static DataTable TableImpostazioni=null;
        private static void InizializzaImpostazioni() {
            if (TableImpostazioni != null) return;
            var Db = new MySqlvc();
            TableImpostazioni=Db.EseguiQuery("Select * From NavettaImpostazioni");



        }
        public static String LeggiImpostazione(String Chiave) {
            InizializzaImpostazioni();
            foreach (DataRow x in TableImpostazioni.Rows) {
                if (x["Impostazione"].ToString()==Chiave) return x["Valore"].ToString();
            }
            return "";
        }
    }

    public class EmailSender {
        public static void SendEmail(string toEmail, string subject, string body) {
            if (toEmail=="") return;
            try {
                var smtpClient = new SmtpClient("smtp.gmail.com", 587) {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("dimariafabio@gmail.com", "nlzhrnewbdrhrbsg")
                };

                var mailMessage = new MailMessage {
                    From = new MailAddress("dimariafabio@gmail.com"),
                    Subject = subject,
                    Body = body
                };
                if (toEmail.Contains(",")==false) mailMessage.To.Add(toEmail);
                else {
                    foreach (String x in toEmail.Split(',')) {
                        mailMessage.To.Add(x); 
                    }
                }

                smtpClient.Send(mailMessage);
            } catch (Exception ex) {
                Console.WriteLine("Exception caught in sending email: " + ex.Message);
            }
        }
    }
}
