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
            //Task.Run(LeggiDaDb);
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

        private void BtnPrenota_Clicked(object sender, EventArgs e) {

        }
    }
}