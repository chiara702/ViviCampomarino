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
            var db = new Database<Libro>();
            var coll=db.GetCollection("/Libri/");
            var query=coll.WhereGreaterThanOrEqualsTo("Titolo", TxtCerca.Text).WhereLessThanOrEqualsTo("Titolo",TxtCerca.Text + "\uf8ff");
            var ListaLibri=await query.GetDocumentsAsync<Libro>();
            StackView.Children.Clear();
            foreach (var x in ListaLibri.Documents) {
                var el = new ViewRisultatiRicerca();
                StackView.Children.Add(el);
                el.Titolo = "" + x.Data.Titolo;
                el.Autori = "" + x.Data.Autori;
                try {
                    var url = await FirebaseStorage.DownloadUrlFromStorage("Libri/" + x.Data.ISBN + ".png");
                    Device.BeginInvokeOnMainThread(()=>el.Image = ImageSource.FromUri(new Uri(url)));
                } catch (Exception) { }
                
                el.Disponibile = "Bo";
            }

        }
    }
}