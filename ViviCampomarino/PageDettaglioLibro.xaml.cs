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
        public async void LeggiDaDb() {
            var db = new Database<Libro>();
            var Libro = await db.ReadDocument("/Libri/" + idLibro);
            //var url = await FirebaseStorage.DownloadUrlFromStorage("Libri/" + Libro.ISBN + ".png");
            Device.BeginInvokeOnMainThread(() => {
                LblAutori.Text = Funzioni.Antinull(Libro.Autori);
                LblTitolo.Text = Funzioni.Antinull(Libro.Titolo);
                LblSottotitolo.Text = Funzioni.Antinull(Libro.Sommario);
                //ImgLibro.Source = ImageSource.FromUri(new Uri(url));
            });
        }

        private void BtnPrenota_Clicked(object sender, EventArgs e) {

        }
    }
}