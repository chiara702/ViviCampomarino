using Plugin.Firebase.CloudMessaging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageDettaglioLibro : ContentPage {
        private DataRow rowLibro;
        

        public PageDettaglioLibro(DataRow rowLibro) {
            InitializeComponent();
            this.rowLibro = rowLibro;
            LblAutori.Text = Funzioni.Antinull(rowLibro["Autori"]);
            LblTitolo.Text = Funzioni.Antinull(rowLibro["Titolo"]);
            LblSottotitolo.Text = Funzioni.Antinull(rowLibro["Sommario"]);
            LblCasaEditrice.Text = "Casa editrice: " + Funzioni.Antinull(rowLibro["Editore"]);
            LblAnnoPubblicazione.Text = "Pubblicazione: " + Funzioni.Antinull(rowLibro["DataPubblicazione"]);
            LblGenere.Text = "Genere: " + Funzioni.Antinull(rowLibro["Generi"]);
            LblPagine.Text = "Pagine: " + Funzioni.Antinull(rowLibro["Pagine"]);
            LblDescrizione.Text = Funzioni.Antinull(rowLibro["Descrizione"]);
            switch (FunzioniLibri.LibroDisponibile(rowLibro)) {
                case FunzioniLibri._Disponibile.Disponibile:
                    LblDisponibilita.Text = "Disponibile";
                    BtnAvvisa.IsVisible = false;
                    break;
                case FunzioniLibri._Disponibile.Prenotato:
                    LblDisponibilita.Text = "Prenotato";
                    BtnPrenota.IsEnabled = false;
                    BtnAvvisa.IsVisible = true;
                    break;
                case FunzioniLibri._Disponibile.Prestato:
                    LblDisponibilita.Text = "Momentaneamente non disponibile";
                    BtnPrenota.IsEnabled = false;
                    BtnAvvisa.IsVisible = true;
                    break;
                case FunzioniLibri._Disponibile.NonDisponibile:
                    LblDisponibilita.Text = "Non disponibile";
                    BtnPrenota.IsEnabled = false;
                    BtnAvvisa.IsVisible = true;
                    break;
            }
            if (System.IO.File.Exists(System.IO.Path.GetTempPath() + rowLibro["Id"].ToString() + ".png") == true) {
                ImgLibro.Source = ImageSource.FromFile(System.IO.Path.GetTempPath() + rowLibro["Id"].ToString() + ".png");
            } else {
                ImgLibro.Source = "ImmagineLibro";
            }
        }
        

        private async void BtnPrenota_Clicked(object sender, EventArgs e) {
            
            if (App.LoginUidAuth=="") {
                await DisplayAlert("Errore", "Devi eseguire il login!", "OK");
                await Navigation.PushAsync(new PageLogin());
                return;
            }
            
            if (FunzioniLibri.LibroDisponibile(rowLibro) == FunzioniLibri._Disponibile.Disponibile) {
                var rowLibri = FunzioniLibri.ListaRowLibriPrenotatiPrestati(App.LoginUidAuth);
                if (rowLibri.Count>=5) {
                    await DisplayAlert("", "Hai superato il numero max di libri prenotabili!", "OK");
                    return;
                }

                try {
                    MySqlvc dbSql = new MySqlvc();
                    var dbBis = new MySqlvc.DBSqlBis(dbSql, "Libri");
                    dbBis.GetParam.AddWithValue("IdUtente", App.login["Id"]);
                    dbBis.GetParam.AddWithValue("DataPrenotato", DateTimeOffset.Now);
                    dbBis.GetParam.AddWithValue("DataPrestito", null);
                    dbBis.GeneraUpdate(Convert.ToInt32(rowLibro["Id"]));
                    dbSql.CloseCommit();
                } catch(Exception err) {
                    await DisplayAlert("Errore", "Errore nel salvataggio! " + err.Message, "OK");
                    return;
                }
                MySqlvc.WriteLog("Prenotazione Libro Ok: " + rowLibro["Id"].ToString());
                await DisplayAlert("Prenotazione", "Prenotazione avvenuta con successo!", "OK");

                await Navigation.PushAsync(new PageAccount(),true);

                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                var pageToRemove = new List<Page>();
                foreach (var page in Navigation.NavigationStack) {
                    if (page is PageBibliotecaCerca) pageToRemove.Add(page);
                }


            } else {
                await DisplayAlert("Prenotazione", "Libro già prenotato!", "OK");
            }

        }


        private async void BtnAvvisa_Clicked(object sender, EventArgs e){
            await DisplayAlert("Notifiche attivate","Riceverai una notifica non appena il libro tornerà disponibile!","OK");
            BtnAvvisa.Text = "Notifica attivate";
            await CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync("Disponibile" + rowLibro["Id"].ToString());

            //Crea nuova notifica
            //var db = new Database<object>();
            //var coll = db.current.GetCollection("Login/" + App.LoginUidAuth + "/NotificheLibri");
            //var doc = coll.GetDocument(idLibro);
            //var dic = new Dictionary<Object, Object>();
            //dic.Add("LibroId", idLibro);
            //dic.Add("NotificaDisponibile", true);
            //await doc.SetDataAsync(dic);


            //var db = new Database<NotificheLibri>();
            //var coll=db.GetCollection("notificheLibri");
            //var Not = new NotificheLibri();
            //Not.LoginId = App.LoginUidAuth;
            //Not.LibroId = idLibro;
            //Not.Data = DateTimeOffset.Now;
            //await coll.AddDocumentAsync(Not);
            ////Controllo notifiche da inviare
            //var snap = await coll.GetDocumentsAsync<NotificheLibri>();
            //foreach (var x in snap.Documents) {
            //    if (x.Data.Data <= DateTimeOffset.Now.AddYears(-1)) await x.Reference.DeleteDocumentAsync();
            //    //da verificare notifiche soddisfatte
            //}

        }
    }
}