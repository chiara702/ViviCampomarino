using Plugin.Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ViviCampomarino {
    public class Database<T> {
        private IFirebaseFirestore current = CrossFirebaseFirestore.Current;
        public Database() { }
        public async void WriteDocument(String Patch, T oggetto) {
            await current.GetDocument(Patch).SetDataAsync(oggetto);
        }
        public async Task<T> ReadDocument(String Patch) {
            var ritorno = await current.GetDocument(Patch).GetDocumentSnapshotAsync<T>();
            T risultato = ritorno.Data;
            return risultato;
        }
        public ICollectionReference GetCollection(String Path) {
            return current.GetCollection(Path);
        }
    }

    class FirebaseStorage {
        public static async Task DownloadFromStorage(String storagePath, String localPath) {
            var storage = Plugin.Firebase.Storage.CrossFirebaseStorage.Current;
            var child = storage.GetRootReference().GetChild(storagePath);
            await child.DownloadFile(localPath).AwaitAsync();
        }
        public static async Task<Stream> DownloadStreamFromStorage(String storagePath) {
            var storage = Plugin.Firebase.Storage.CrossFirebaseStorage.Current;
            //var child = storage.GetRootReference().GetChild(storagePath);
            var rif=storage.GetReferenceFromPath(storagePath);
            return await rif.GetStreamAsync(10000);
        }
        public static Task<String> DownloadUrlFromStorage(String storagePath) {
            var storage = Plugin.Firebase.Storage.CrossFirebaseStorage.Current;
            var child = storage.GetRootReference().GetChild(storagePath);
            var urlTask = child.GetDownloadUrlAsync();
            return urlTask;
        }
        public static async Task UploadToStorage(String storagePath, String localPath) {
            var storage = Plugin.Firebase.Storage.CrossFirebaseStorage.Current;
            var child = storage.GetRootReference().GetChild(storagePath);
            await child.PutFile(localPath).AwaitAsync();
        }

    }

    public class Libro {
        [FirestoreProperty("Titolo")] public String Titolo { get; set; }
        [FirestoreProperty("Autori")] public String Autori { get; set; }
        [FirestoreProperty("Editore")] public String Editore { get; set; }
        [FirestoreProperty("ISBN")] public String ISBN { get; set; }
        [FirestoreProperty("Sommario")] public String Sommario { get; set; }
        [FirestoreProperty("DataPubblicazione")] public String DataPubblicazione { get; set; }
        [FirestoreProperty("Pagine")] public String Pagine { get; set; }
        [FirestoreProperty("Generi")] public String Generi { get; set; }
        [FirestoreProperty("Image")] public String Image { get; set; }
        [FirestoreProperty("Descrizione")] public String Descrizione { get; set; }
        [FirestoreProperty("IdUtente")] public String IdUtente { get; set; }
        [FirestoreProperty("DataPrenotato")] public DateTimeOffset DataPrenotato { get; set; }
        [FirestoreProperty("Disponibile")] public Boolean Disponibile { get; set; }
        [FirestoreProperty("DataPrestito")] public DateTimeOffset DataPrestito { get; set; }

        public enum _Disponibile {
            Disponibile,
            Prenotato,
            Prestato
        }
        public _Disponibile LibroDisponibile() {
            if (Disponibile == true) {
                if (DataPrenotato == null) return _Disponibile.Disponibile;
                if (System.DateTime.Now - DataPrenotato >= TimeSpan.FromDays(7)) return _Disponibile.Disponibile; else return _Disponibile.Prenotato;
            } else {
                return _Disponibile.Prestato;
            }
        }

    }

    public class Funzioni {

        public static string CreateMD5(string input) {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++) {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string Antinull(object input) {
            if (input == null) return ""; else return input.ToString();
        }
        public static string AntiAp(String stringa) {
            return stringa.Replace("'", "''");
        }
        
        public static bool IsValidEmail(string email) {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            } catch {
                return false;
            }
        }


        public static int VerificaPassword(String Password) {
            //ritorni: 0:OK
            //-1:caratteri ammessi non validi (caratteri validi a-Z,A-Z,0-9,!,@,#,$,%,*,.,-,_) o lunghezza non corretta (Password da 8 a 20 caratteri)
            //-2:la password richiede almeno un carattere maiuscolo
            //-3:la password richiede almeno un carattere minuscolo
            //-4:la password richiede almeno un numero
            //-5:la password richiede almeno un carattere speciale tra: !,@,#,$,%,*,.,-,_
            if (Regex.IsMatch(Password, "^[A-Za-z0-9/!/@/#/$/&/*/%/./-/_/+/?/</>]{8,20}$") == false) return -1;
            if (Regex.IsMatch(Password, "[A-Z]{1,}") == false) return -2; //controllo della presenza di almeno una lettera maiuscola
            if (Regex.IsMatch(Password, "[a-z]{1,}") == false) return -3; //controllo della presenza di almeno una lettera minuscola
            if (Regex.IsMatch(Password, "[0-9]{1,}") == false) return -4; //controllo della presenza di almeno un numero
            if (Regex.IsMatch(Password, "[/!/@/#/$/&/*/%/./-/_/+/?/</>]") == false) return -5;
            return 0;
        }


        
        public static void SendEmail(string Destinatario, string Mittente, string Oggetto, string Messaggio) {
            // imposta destinatario
            if (Destinatario == "") return;
            MailAddress sendTo = new MailAddress(Destinatario);
            // imposta mittente
            MailAddress from = new MailAddress("ripremiasupport@ecocontrolgsm.it");
            // istanzia l'oggetto MailMessage
            MailMessage message = new MailMessage(from, sendTo);
            //message.To.Add("ripremianoreply@gmail.com");

            // campi del messaggio
            message.IsBodyHtml = true;
            message.Subject = Oggetto;
            message.Body = Messaggio;
            if (Messaggio.Contains("<")) message.IsBodyHtml = true;
            // credenziali di accesso
            //System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential("info@ecocontrolgsm.it", "fabio123456");
            // imposta connessione con il server GMAIL
            SmtpClient SMTPServer = new SmtpClient("smtp.ecocontrolgsm.it");
            //SmtpClient SMTPServer = new SmtpClient("smtp.gmail.com");
            SMTPServer.UseDefaultCredentials = false;
            SMTPServer.Port = 587;
            //SMTPServer.EnableSsl = true;

            //SMTPServer.Credentials = new System.Net.NetworkCredential("ripremianoreply@gmail.com", "Ripremia123456");
            SMTPServer.Credentials = new System.Net.NetworkCredential("ripremiasupport@ecocontrolgsm.it", "fabio123456");

            // SMTPServer.EnableSsl = True
            // invio della mail
            /*try {
                SMTPServer.Send(message);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }*/
            Task.Run(() => SMTPServer.Send(message));
        }

        public static String ToQueryData(DateTime data) {
            return data.ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}
