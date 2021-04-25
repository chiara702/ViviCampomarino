using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAccountLibro : Grid {

        public DataRow rowLibro;
        public ViewAccountLibro(DataRow RowLibro) {
            InitializeComponent();
            rowLibro = RowLibro;
            LblTitolo.Text = Funzioni.Antinull(rowLibro["Titolo"]);
            LblAutori.Text = Funzioni.Antinull(rowLibro["Autori"]);

            if (Convert.IsDBNull(rowLibro["DataPrestito"])==false && Convert.ToDateTime(rowLibro["DataPrestito"]) > DateTimeOffset.Parse("01/01/01")) {
                LblScadenzaPrestito.Text = Convert.ToDateTime(rowLibro["DataPrestito"]).AddDays(30).ToString("dd/MM/yy");
            } else {
                LblScadenzaPrestito.Text = "prenotato";
            }
        }




        public ImageSource Image {
            set { Device.BeginInvokeOnMainThread(() => ImgLibro.Source = value); }
        }









        private async void BtnCancellaPrenotazione_Clicked(object sender, EventArgs e) {
            try {
                var Db = new MySqlvc();
                Db.UpdateRapido("Libri", Convert.ToInt32(rowLibro["Id"]), "DataPrenotato", null);
                Db.UpdateRapido("Libri", Convert.ToInt32(rowLibro["Id"]), "IdUtente", "");
                await Application.Current.MainPage.DisplayAlert("Prenotazione", "Prenotazione cancellata con successo!", "OK");
                this.IsVisible = false;
            } catch (Exception) {
                await Application.Current.MainPage.DisplayAlert("Verifica connessione", "Non sono riuscito a cancellare la prenotazione!", "OK");
                return;
            }
            var data = new Dictionary<string, string>();
            data.Add("NotificaDisponibilita", "si");
            data.Add("IdLibro", rowLibro["Id"].ToString());
            data.Add("Notifica", "Disponibile" + rowLibro["Id"].ToString());
            data.Add("Da", App.LoginUidAuth);
            Funzioni.NotificaFcmLegacyToToken("/topics/Disponibile" + rowLibro["Id"].ToString(), data, "Libro ora disponibile", "Il libro: " + LblTitolo.Text + " è tornato ora disponibile!");


        }
    }
}
