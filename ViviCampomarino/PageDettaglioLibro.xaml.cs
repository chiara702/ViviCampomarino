using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageDettaglioLibro : ContentPage {
        private String idLibro;

        public PageDettaglioLibro(String IdLibro) {
            InitializeComponent();
            idLibro = IdLibro;
            Task.Run(LeggiDaDb);
        }
        public void LeggiDaDb() {
            var db = new Database<Libro>();
            var Libro = db.ReadDocument("/Libri/" + idLibro).Result;
            try {
                FirebaseStorage.DownloadFromStorage("Libri/" + idLibro + ".png", System.IO.Path.GetTempPath() + idLibro + ".png").Wait();
            } catch(Exception){}
            Device.BeginInvokeOnMainThread(() => {
                LblAutori.Text = Funzioni.Antinull(Libro.Autori);
                LblTitolo.Text = Funzioni.Antinull(Libro.Titolo);
                LblSottotitolo.Text = Funzioni.Antinull(Libro.Sommario);
                if (System.IO.File.Exists(System.IO.Path.GetTempPath() + idLibro + ".png") == true) ImgLibro.Source = ImageSource.FromFile(System.IO.Path.GetTempPath() + idLibro + ".png");
            });
        }

        private async void BtnPrenota_Clicked(object sender, EventArgs e) {
            var db = new Database<Libro>();
            var snap = await db.GetCollection("Libri").GetDocument(idLibro).GetDocumentSnapshotAsync<Libro>(Plugin.Firebase.Firestore.Source.Server);
            var x = snap.Data.Autori;
            snap.Data.Autori = "prova";
            var y = snap.Data.Autori;

            if (snap == null) {
                await DisplayAlert("Errore", "Non riesco a recuperare i dati del libro online!", "OK");
                return;
            }
            if (App.LoginUidAuth=="") {
                await DisplayAlert("Errore", "Devi eseguire il login!", "OK");
                await Navigation.PushAsync(new PageLogin());
                return;
            }
            
            if (snap.Data.LibroDisponibile() == Libro._Disponibile.Disponibile) {
                var dictUpdate = new Dictionary<Object, Object>();
                dictUpdate.Add("IdUtente", App.LoginUidAuth);
                dictUpdate.Add("DataPrenotato", DateTimeOffset.Now);
                dictUpdate.Add("DataPrestito", DateTimeOffset.Parse("01/01/1900"));

                try {
                    await snap.Reference.SetDataAsync(dictUpdate, Plugin.Firebase.Firestore.SetOptions.Merge());
                } catch(Exception err) {
                    await DisplayAlert("Errore", "Errore nel salvataggio! " + err.Message, "OK");
                    return;
                }
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
    }
}