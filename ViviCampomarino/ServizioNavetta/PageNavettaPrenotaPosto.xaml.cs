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
            //Verifica posti 
            Task.Run(() => { 
                var Db = new MySqlvc();
                var Table = Db.EseguiQuery($"Select * From NavettaPrenotazioni Where Giorno='{DataSelezionata.ToString("yyyy-MM-dd HH:mm")}'");
                Device.BeginInvokeOnMainThread(() => {
                    foreach (DataRow x in Table.Rows) {
                        if ((int)x["Posto"]==1) {
                            LblPosto1.Text="Occupato";
                        }
                        if ((int)x["Posto"]==2) {
                            LblPosto2.Text="Occupato";
                        }
                        if ((int)x["Posto"]==3) {
                            LblPosto3.Text="Occupato";
                        }

                    }
                });
            });
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e) {
            var Posto = Convert.ToInt16(((TappedEventArgs)e).Parameter);
            var form = new PageNavettaRegistrazione(DataSelezionata, Posto);
            Navigation.PushAsync(form); 
        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync();
        }
    }
}