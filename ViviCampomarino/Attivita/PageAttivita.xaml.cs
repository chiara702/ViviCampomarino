using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViviCampomarino.Attivita;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAttivita : ContentPage {
        private DataRow rowAttivita;
        public PageAttivita(DataRow RowAttivita) {
            InitializeComponent();
            rowAttivita = RowAttivita;
            CaricaImage(ImgLogoAttivita, RowAttivita["Logo"]);
            LblNomeAttivita.Text = Funzioni.Antinull(rowAttivita["Denominazione"]);
            LblDescrizione.Text = Funzioni.Antinull(rowAttivita["Descrizione"]);
            LblPaese.Text = Funzioni.Antinull(rowAttivita["Paese"]);
            LblIndirizzo.Text = Funzioni.Antinull(rowAttivita["Indirizzo"]);
            LblEmail.Text = Funzioni.Antinull(rowAttivita["Email"]);
            if (LblEmail.Text == "") StackEmail.IsVisible = false;
            LblWebSite.Text = Funzioni.Antinull(rowAttivita["SitoWeb"]);
            if (LblWebSite.Text == "") StackWebSite.IsVisible = false;
            LblAppStore.Text = Funzioni.Antinull(rowAttivita["Store"]);
            if (LblAppStore.Text == "") StackApp.IsVisible = false;
            LblTelefono.Text = Funzioni.Antinull(rowAttivita["Telefono"]);
            if (LblTelefono.Text == "") StackTelefono.IsVisible = false;
            LblCellulare1.Text = Funzioni.Antinull(RowAttivita["Cellulare1"]);
            if (LblCellulare1.Text == "") StackCellulare1.IsVisible = false;
            LblCellulare2.Text = Funzioni.Antinull(RowAttivita["Cellulare2"]);
            if (LblCellulare2.Text == "") StackCellulare2.IsVisible = false;
            CaricaImage(Img1, RowAttivita["Image1"]);
            CaricaImage(Img2, RowAttivita["Image2"]);
            CaricaImage(Img3, RowAttivita["Image3"]);
            CaricaImage(Img4, RowAttivita["Image4"]);
            if (Funzioni.Antinull(RowAttivita["OrarioLunedi"]) == "") StackOrari.IsVisible = false;
            LblOrari.Text = "Lunedì: " + Funzioni.Antinull(RowAttivita["OrarioLunedi"]) + "\n\r";
            LblOrari.Text += "Martedì: " + Funzioni.Antinull(RowAttivita["OrarioMartedi"]) + "\n\r";
            LblOrari.Text += "Mercoledì: " + Funzioni.Antinull(RowAttivita["OrarioMercoledi"]) + "\n\r";
            LblOrari.Text += "Giovedì: " + Funzioni.Antinull(RowAttivita["OrarioGiovedi"]) + "\n\r";
            LblOrari.Text += "Venerdì: " + Funzioni.Antinull(RowAttivita["OrarioVenerdi"]) + "\n\r";
            LblOrari.Text += "Sabato: " + Funzioni.Antinull(RowAttivita["OrarioSabato"]) + "\n\r";
            LblOrari.Text += "Domenica: " + Funzioni.Antinull(RowAttivita["OrarioDomenica"]);

            

        }
        private void CaricaImage(Image Pic, Object data) {
            if (Convert.IsDBNull(data) == false) Pic.Source = ImageSource.FromStream(() => new System.IO.MemoryStream((byte[])data));
        }

        private void TapTelefono_Tapped(object sender, EventArgs e) {
            try {
                Xamarin.Essentials.PhoneDialer.Open(LblTelefono.Text);
            } catch (Exception) {
                DisplayAlert("", "Formato numero non corretto per avviare il dialer!", "OK");
            }
        }
        private void TapCellulare1_Tapped(object sender, EventArgs e) {
            try {
                Xamarin.Essentials.PhoneDialer.Open(LblCellulare1.Text);
            } catch (Exception) {
                DisplayAlert("", "Formato numero non corretto per avviare il dialer!", "OK");
            }
        }
        private void TapCellulare2_Tapped(object sender, EventArgs e) {
            try {
                Xamarin.Essentials.PhoneDialer.Open(LblCellulare2.Text);
            } catch (Exception) {
                DisplayAlert("", "Formato numero non corretto per avviare il dialer!", "OK");
            }
        }

        private void TapWebSite_Tapped(object sender, EventArgs e) {
            try {
                if (Funzioni.Antinull(LblWebSite.Text) == "") return;
                Xamarin.Essentials.Browser.OpenAsync(LblWebSite.Text);
            } catch (Exception) {
                DisplayAlert("", "Url non valida", "OK");
            }
        }
        private async void TapIndirizzo_Tapped(object sender, EventArgs e) {
            try {
                Location location = (await Geocoding.GetLocationsAsync(LblNomeAttivita.Text + " " + LblIndirizzo.Text + " " + LblPaese.Text)).FirstOrDefault();
                if (location == null) { await DisplayAlert("", "Non riesco a trovare l'attività su maps!", "OK"); return; }
                await Map.OpenAsync(location);
            } catch (Exception) {
                await DisplayAlert("", "Non riesco a visualizzare l'indirizzo!", "OK");
            }
        }

        private void TapImg1_Tapped(object sender, EventArgs e) {
            var page = new PageZoomImage(Img1.Source);
            Navigation.PushAsync(page);
        }
        private void TapImg2_Tapped(object sender, EventArgs e) {
            var page = new PageZoomImage(Img2.Source);
            Navigation.PushAsync(page);
        }
        private void TapImg3_Tapped(object sender, EventArgs e) {
            var page = new PageZoomImage(Img3.Source);
            Navigation.PushAsync(page);
        }
        private void TapImg4_Tapped(object sender, EventArgs e) {
            var page = new PageZoomImage(Img4.Source);
            Navigation.PushAsync(page);
        }
    }
}