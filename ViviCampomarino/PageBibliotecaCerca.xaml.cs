using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageBibliotecaCerca : ContentPage {
        public PageBibliotecaCerca() {
            InitializeComponent();
            MenuTop.MenuLaterale = MenuLaterale;
            StkCerca.IsVisible = true;
        }

        private async void ImgMenu_Tapped(object sender, EventArgs e) {
            MenuLaterale.IsVisible = true;
            await MenuLaterale.Mostra();
        }

        private async void BtnCerca_Clicked(object sender, EventArgs e) {
            TxtCerca.Text = Funzioni.Antinull(TxtCerca.Text);
            if (TxtCerca.Text == null) {
                LblRicercaFallita.IsVisible = true;
                return;
            }
            var db = new Database<Libro>();
            var coll = db.GetCollection("/Libri/");
            var query = coll.WhereGreaterThanOrEqualsTo("Titolo", TxtCerca.Text).WhereLessThanOrEqualsTo("Titolo", TxtCerca.Text + "\uf8ff").LimitedTo(50);
            var ListaLibri = await query.GetDocumentsAsync<Libro>();
            var curStorage = Plugin.Firebase.Storage.CrossFirebaseStorage.Current.GetRootReference();
            //var listaFile = await curStorage.GetChild("Libri").ListAllAsync();
            StackView.Children.Clear();
            foreach (var x in ListaLibri.Documents) {
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
            //await Task.Run(() => {
                foreach (ViewRisultatiRicerca x in StackView.Children) {
                    try {
                        var nomefile = x.IdLibro + ".png";
                        var rifs = curStorage.GetChild("Libri/" + nomefile);
                        //var presente = false;
                        //foreach (var f in listaFile.Items) if (f.Name == nomefile) presente = true;
                        //if (presente == true) {
                            var a = rifs.DownloadFile(System.IO.Path.GetTempPath() + nomefile);
                            await a.AwaitAsync();
                        //}
                        if (System.IO.File.Exists(System.IO.Path.GetTempPath() + nomefile) == true) x.Image = ImageSource.FromFile(System.IO.Path.GetTempPath() + nomefile);
                    } catch (SystemException) {
                    } catch (Exception err) {
                    }

                }
            //});
            if (ListaLibri.Count == 0) {
                LblRicercaFallita.IsVisible = true;
            } else LblRicercaFallita.IsVisible = false;


            StackView.IsVisible = true;
            FrameRicerca.IsVisible = true;
            StkCerca.IsVisible = false;
            BtnNuovaRicerca.IsVisible = true;

           
        }

        private void BtnNuovaRicerca_Clicked(object sender, EventArgs e) {
            //await Navigation.PushAsync(new PageBibliotecaCerca());
            FrameRicerca.IsVisible = false;
            StkCerca.IsVisible = true;
            BtnNuovaRicerca.IsVisible = false;
            TxtCerca.Text = "";
            StackView.IsVisible = false;
        }

        
    }
}