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
    public partial class PageNavettaPrenotaOrario : ContentPage {
        public List<DateTime> ListaOrariAndata;
        public List<DateTime> ListaOrariRitorno;
        public PageNavettaPrenotaOrario(DateTime DataSelezionata) {
            InitializeComponent();
            LblData.Text = DataSelezionata.ToString("dd/MM/yyyy");
            var t=Task.Run(() => {
                var Db = new MySqlvc();
                DataRow rowGiorniAbilitati=Db.EseguiRow($"Select * From NavettaGiorniAbilitati Where GiornoAbilitato='{DataSelezionata.ToString("yyyy-MM-dd")}'");
                ListaOrariAndata = new List<DateTime>();
                ListaOrariRitorno = new List<DateTime>();
                foreach (String x in rowGiorniAbilitati["OrariAndata"].ToString().Split(",",StringSplitOptions.RemoveEmptyEntries)) {
                    ListaOrariAndata.Add(DateTime.Parse(DataSelezionata.ToString("dd/MM/yyyy") + " " + x));
                }
                foreach (String x in rowGiorniAbilitati["OrariRitorno"].ToString().Split(",", StringSplitOptions.RemoveEmptyEntries)) {
                    ListaOrariRitorno.Add(DateTime.Parse(DataSelezionata.ToString("dd/MM/yyyy") + " " + x));
                }
                Device.BeginInvokeOnMainThread(() => {
                    foreach (DateTime dt in ListaOrariAndata) {
                        var Label = new Label();
                        Label.Text = dt.ToString();
                        StkAndata.Children.Add(Label);
                    }
                    foreach (DateTime dt in ListaOrariRitorno) {
                        var Label = new Label();
                        Label.Text = dt.ToString();
                        StkRitorno.Children.Add(Label);
                    }
                });
            }
            );
            
        }


        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync();  
        }
    }
}