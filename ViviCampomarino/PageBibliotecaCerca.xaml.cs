using Plugin.Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageBibliotecaCerca : ContentPage {
        Task TaskCaricaDb;
        public PageBibliotecaCerca() {
            InitializeComponent();
            MenuTop.MenuLaterale = MenuLaterale;
            StkCerca.IsVisible = true;
            TaskCaricaDb = new Task(CaricaDb);
            TaskCaricaDb.Start();
        }
        private IQuerySnapshot<Libro> ListaLibri = null;
        private void CaricaDb() {
            Debug.WriteLine("Start CaricaDb");
            var db = new Database<Libro>();
            var coll = db.GetCollection("Libri");
            try {
                Device.BeginInvokeOnMainThread(() => Act1.IsVisible = true);
                Debug.WriteLine("Start GetDocument");
                ListaLibri = coll.GetDocumentsAsync<Libro>().Result;
                Debug.WriteLine("End GetDocument");
                Device.BeginInvokeOnMainThread(() => Act1.IsVisible = false);
            } catch (Exception err) {
                DisplayAlert("Errore", "Errore: " + err.Message, "OK");
                return;
            }
        }

        private async void ImgMenu_Tapped(object sender, EventArgs e) {
            MenuLaterale.IsVisible = true;
            await MenuLaterale.Mostra();
        }

        private Boolean RicercaValida(Libro libro, String ricerca, Boolean SoloDisponibili) {
            var Ritorno = false;
            if (libro == null) return false;
            if (Funzioni.Antinull(libro.Titolo).Contains(ricerca, StringComparison.CurrentCultureIgnoreCase) == true) Ritorno=true;
            if (Funzioni.Antinull(libro.Autori).Contains(ricerca, StringComparison.CurrentCultureIgnoreCase) == true) Ritorno=true;
            if (Funzioni.Antinull(libro.Generi).Contains(ricerca,StringComparison.CurrentCultureIgnoreCase) == true) Ritorno = true;
            //if (Funzioni.Antinull(libro.ISBN).Contains(ricerca, StringComparison.CurrentCultureIgnoreCase) == true) Ritorno = true;
            //if (Funzioni.Antinull(libro.Editore).Contains(ricerca, StringComparison.CurrentCultureIgnoreCase) == true) Ritorno = true;
            if (SoloDisponibili == true) if (libro.LibroDisponibile() != Libro._Disponibile.Disponibile) Ritorno = false;
            return Ritorno;
        }

        

        private async void BtnCerca_Clicked(object sender, EventArgs e) {
            Debug.WriteLine("Start BtnCerca");
            TxtCerca.Text = Funzioni.Antinull(TxtCerca.Text);

            if (TxtCerca.Text == null) {
                LblRicercaFallita.IsVisible = true;
                return;
            }
            Act1.IsVisible = true;
            Debug.WriteLine("Start TaskWait");
            await Task.Run(()=>TaskCaricaDb.Wait());
            Debug.WriteLine("End TaskWait");
            Act1.IsVisible = false;
            //var db = new Database<Libro>();
            //var coll = db.GetCollection("Libri");
            //IQuerySnapshot<Libro> ListaLibri = null;
            //try {
            //    Act1.IsVisible = true;
            //    ListaLibri = await coll.GetDocumentsAsync<Libro>();
            //    var a=coll.GetDocumentsAsync<Libro>();
            //    Act1.IsVisible = false;
            //} catch (Exception err) {
            //    await DisplayAlert("Errore", "Errore: " + err.Message, "OK");
            //    return;
            //}
            var curStorage = FirebaseStorage.current.GetRootReference();
            StackView.Children.Clear();
            var LibriMostrati = 0;
            var CercaSoloDisponibili = false;
            if (CheckSoloDisponibili.IsChecked == true) CercaSoloDisponibili = true;
            foreach (var x in ListaLibri.Documents) {
                if (RicercaValida(x.Data,TxtCerca.Text,CercaSoloDisponibili)==false) continue;
                LibriMostrati++;
                if (LibriMostrati >= 50) break;
                var el = new ViewRisultatiRicerca();
                el.IdLibro = x.Reference.Id;
                el.Titolo = "" + x.Data.Titolo;
                el.Autori = "" + x.Data.Autori;
                el.IdLibro = x.Reference.Id;
                switch (x.Data.LibroDisponibile()) {
                    case Libro._Disponibile.Disponibile:
                        el.Disponibile = "Disponibile";
                        break;
                    case Libro._Disponibile.Prenotato:
                        el.Disponibile = "Non Disponibile";
                        break;
                    case Libro._Disponibile.Prestato:
                        el.Disponibile = "Non Disponibile";
                        break;
                }
                StackView.Children.Add(el);
            }

            if (LibriMostrati == 0) {
                LblRicercaFallita.IsVisible = true;
            } else LblRicercaFallita.IsVisible = false;
            FrameRicerca.IsVisible = true;
            StkCerca.IsVisible = false;

            _ = Task.Run(() => {
                foreach (ViewRisultatiRicerca x in StackView.Children) {
                    try {
                        var nomefile = x.IdLibro + ".png";
                        var rifs = curStorage.GetChild("Libri/" + nomefile);
                        //var presente = false;
                        //foreach (var f in listaFile.Items) if (f.Name == nomefile) presente = true;
                        //if (presente == true) {
                        if (System.IO.File.Exists(System.IO.Path.GetTempPath() + nomefile) == true) {
                            if (System.IO.File.GetCreationTime(System.IO.Path.GetTempPath() + nomefile) >= DateTime.Now.AddDays(-1)) {
                                x.Image = ImageSource.FromFile(System.IO.Path.GetTempPath() + nomefile);
                                continue;
                            }
                        }
                        var a = rifs.DownloadFile(System.IO.Path.GetTempPath() + nomefile);
                        a.AwaitAsync().Wait(3000);
                        if (System.IO.File.Exists(System.IO.Path.GetTempPath() + nomefile) == true) {
                            x.Image = ImageSource.FromFile(System.IO.Path.GetTempPath() + nomefile);
                        }

                        //await a.AwaitAsync();
                        //}

                    } catch (SystemException) {
                    } catch (Exception err) {
                        var c = 0;
                    }

                }
            });

            

        }

        

        private void BtnNuovaRicerca_Clicked(object sender, EventArgs e) {
            StackView.Children.Clear();
            FrameRicerca.IsVisible = false;
            StkCerca.IsVisible = true;
            TxtCerca.Text = "";
        }

        
    }
}