using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.Test {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCercaLibro : ContentPage {
        public PageCercaLibro() {
            InitializeComponent();
        }
        private List<ViewRisultatiRicerca> ListaView = new List<ViewRisultatiRicerca>();
        private async void BtnCerca_Clicked(object sender, EventArgs e) {
            TxtCerca.Text = "" + TxtCerca.Text;
            var db = new Database<Libro>();
            var coll=db.GetCollection("/Libri/");
            var query=coll.WhereGreaterThanOrEqualsTo("Titolo", TxtCerca.Text).WhereLessThanOrEqualsTo("Titolo",TxtCerca.Text + "\uf8ff").LimitedTo(100);
            var ListaLibri=await query.GetDocumentsAsync<Libro>();
            var curr = Plugin.Firebase.Storage.CrossFirebaseStorage.Current.GetRootReference();
            var listaFile=await curr.GetChild("Libri").ListAllAsync();
            StackView.Children.Clear();
            foreach (var x in ListaLibri.Documents) {
                var el = new ViewRisultatiRicerca();
                el.IdLibro = x.Reference.Id;
                el.Titolo = Funzioni.Antinull(x.Data.Titolo);
                el.Autori = Funzioni.Antinull(x.Data.Autori);
                el.ISBN = Funzioni.Antinull(x.Data.ISBN);                
                el.Disponibile = "Bo";
                StackView.Children.Add(el);
            }
            foreach (ViewRisultatiRicerca x in StackView.Children) {
                try {
                    var nomefile = x.IdLibro + ".png";
                    var rifs = curr.GetChild("Libri/" + nomefile);
                    var presente = false;
                    foreach (var f in listaFile.Items) if (f.Name == nomefile) presente = true;
                    if (presente == true) {
                        var a = rifs.DownloadFile(System.IO.Path.GetTempPath() + nomefile);
                    }
                    if (System.IO.File.Exists(System.IO.Path.GetTempPath() + nomefile)==true) x.Image = ImageSource.FromFile(System.IO.Path.GetTempPath() + nomefile);
                } catch (SystemException) { 
                } catch (Exception err) {
                    await DisplayAlert("", err.Message, "OK");
                }

            }


        }
    }
}