using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.ServizioNavetta {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageNavettaPrenotaPosto : ContentPage {
        public DateTime DataSelezionata;
        public PageNavettaPrenotaPosto(DateTime DataSelezionata) {
            InitializeComponent();
            this.DataSelezionata = DataSelezionata;
            LblData.Text=DataSelezionata.ToString("dd/MM/yyyy");
            SetOccupatoLibero(LblPosto1, ImgPosto1, false);
            SetOccupatoLibero(LblPosto2, ImgPosto2, false);
            SetOccupatoLibero(LblPosto3, ImgPosto3, false);

        }
        public void SetOccupatoLibero(Label lbl, Image img, Boolean Stato) {
            if (Stato==false) {
                lbl.Text="Libero";
            } else {
                lbl.Text="Occupato";
            }
        }
        protected override void OnAppearing() {
            base.OnAppearing();
            //Verifica posti 
            Task.Run(() => {
                var Db = new MySqlvc();
                var Table = Db.EseguiQuery($"Select * From NavettaPrenotazioni Where Giorno='{DataSelezionata.ToString("yyyy-MM-dd HH:mm")}'");
                Device.BeginInvokeOnMainThread(() => {
                    foreach (DataRow x in Table.Rows) {
                        SetOccupatoLibero(LblPosto1, ImgPosto1, (int)x["Posto"]==1);
                        SetOccupatoLibero(LblPosto2, ImgPosto2, (int)x["Posto"]==2);
                        SetOccupatoLibero(LblPosto3, ImgPosto3, (int)x["Posto"]==3);
                    }
                });
            });
        }



        private void TapGestureRecognizer_Tapped(object sender, EventArgs e) {
            var Posto = Convert.ToInt16(((TappedEventArgs)e).Parameter);
            //Controllo se prenotazione già eseguita su altro posto
            var Db = new MySqlvc();
            var rowP = Db.EseguiRow($"Select * from NavettaPrenotazioni where Giorno='{DataSelezionata.ToString("yyyy-MM-dd HH:mm")}'");
            Db.CloseCommit();
            if (rowP !=null && (int)rowP["Posto"]!=Posto) {
                DisplayAlert("Prenotazione", "Hai già prenotato con un altro posto!", "ok");
                return;
            }
            //
            var form = new PageNavettaRegistrazione(DataSelezionata, Posto);
            Navigation.PushAsync(form); 
        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync();
        }
    }
}