using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAccountLibro : Grid {
        public String IdLibro {
            get;
            set;
        }
        public String Titolo {
            set { LblTitolo.Text = value; }
        }
        public String Autori {
            set { LblAutori.Text = value; }
        }
      
        public ImageSource Image {
            set { Device.BeginInvokeOnMainThread(() => ImgLibro.Source = value); }
        }
        public String ISBN {
            get;
            set;
        }


        public ViewAccountLibro() {
            InitializeComponent();

            
        }

        

        private void BtnDettaglio_Clicked(object sender, EventArgs e) {
            
        }

        private async void BtnCancellaPrenotazione_Clicked(object sender, EventArgs e) {
            try {
                var db = new Database<Libro>();
                var document = db.GetCollection("Libri").GetDocument(IdLibro);
                var Dict = new Dictionary<Object, Object>();
                Dict.Add("DataPrenotato", DateTimeOffset.Parse("01/01/1900"));
                Dict.Add("IdUtente", "");
                await document.SetDataAsync(Dict, Plugin.Firebase.Firestore.SetOptions.Merge());
                await Application.Current.MainPage.DisplayAlert("Prenotazione", "Prenotazione cancellata con successo!", "OK");
                this.IsVisible = false;
            } catch (Exception) {
                await Application.Current.MainPage.DisplayAlert("Verifica connessione", "Non sono riuscito a cancellare la prenotazione!", "OK");
            }
        }
    }
}
