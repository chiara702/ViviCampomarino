using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.GuestPass {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewListaAttivita : ContentView {
        private DataRow row;
        public ViewListaAttivita(DataRow row) {
            InitializeComponent();
            this.row=row;
            Task.Run(LoadImage);
            LblDenominazione.Text=row["Denominazione"].ToString();
            LblDescrizione.Text=row["Descrizione"].ToString();
            LblIndirizzo.Text=row["Indirizzo"].ToString();
            LblPaese.Text=row["Paese"].ToString();
            LblTelefono.Text=row["Telefono"].ToString();
            if (row["Telefono"].ToString()=="") BtnChiama.IsVisible=false;
            LblCellulare.Text=row["Cellulare1"].ToString();
            if (row["Cellulare1"].ToString()=="") BtnChiamaCellulare.IsVisible=false;
            if (row["PercentualeSconto"].ToString()=="5") ImgSconto.Source="Coupon5p.png";
            if (row["PercentualeSconto"].ToString()=="10") ImgSconto.Source="Coupon10p.png";
            if (row["PercentualeSconto"].ToString()=="15") ImgSconto.Source="Coupon15p.png";
            if (row["PercentualeSconto"].ToString()=="20") ImgSconto.Source="Coupon20p.png";
            if (row["PercentualeSconto"].ToString()=="30") ImgSconto.Source="Coupon30p.png";

        }

        private void LoadImage() {
            try {
                var db = new MySqlvc();
                var rowCompleta = db.EseguiRow("Select * From Aziende Where Id=" + row["Id"].ToString());
                db.CloseCommit();
                Device.BeginInvokeOnMainThread(() => {
                    ImgLogo.Source = ImageSource.FromStream(() => new System.IO.MemoryStream((byte[])rowCompleta["Logo"]));
                });
            } catch (Exception ex) {
                App.Current.MainPage.DisplayAlert("", ex.Message, "Ok");
            }
        }


        public event EventHandler Click;
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e) {
            Click.Invoke(this, e);
        }

        

        private async void BtnNaviga_Clicked(object sender, EventArgs e) {
            try {
                Location location = (await Geocoding.GetLocationsAsync(row["Denominazione"].ToString() + " " + row["Indirizzo"].ToString() + " " + row["Paese"].ToString())).FirstOrDefault();
                if (location == null) { await App.Current.MainPage.DisplayAlert("", "Non riesco a trovare l'attività su maps!", "OK"); return; }
                await Map.OpenAsync(location);
            } catch (Exception) {
                await App.Current.MainPage.DisplayAlert("", "Non riesco a visualizzare l'indirizzo!", "OK");
            }
        }

        private void BtnChiama_Clicked(object sender, EventArgs e) {
            try {
                Launcher.OpenAsync("tel:"+row["Telefono"].ToString());
            } catch (Exception err) {
                //App.Current.MainPage.DisplayAlert("", "Formato numero non corretto per avviare il dialer!"+err.Message, "OK");
            }
        }

        private void BtnChiamaCellulare_Clicked(object sender, EventArgs e) {
            try {
                Launcher.OpenAsync("tel:"+row["Cellulare1"].ToString());
            } catch (Exception) {
                //DisplayAlert("", "Formato numero non corretto per avviare il dialer!", "OK");
            }
        }
    }
}