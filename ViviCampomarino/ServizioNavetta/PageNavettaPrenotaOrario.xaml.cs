using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace ViviCampomarino.ServizioNavetta {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageNavettaPrenotaOrario : ContentPage {
        public List<DateTime> ListaOrariAndata;
        public List<DateTime> ListaOrariRitorno;
        public DateTime DataSelezionata;
        public PageNavettaPrenotaOrario(DateTime DataSelezionata) {
            InitializeComponent();
            this.DataSelezionata= DataSelezionata;  
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
                        var BtnOrario = new Button() {Text=dt.ToString("HH:mm"), HeightRequest=40,BackgroundColor=Color.White, FontSize=14, TextColor=Color.FromHex("3C3C3B"), CornerRadius=15 };
                        BtnOrario.Clicked+=BtnOrario_Clicked;
                        StkAndata.Children.Add(BtnOrario);
                    }
                    foreach (DateTime dt in ListaOrariRitorno) {
                        var BtnOrario = new Button() { Text=dt.ToString("HH:mm"), HeightRequest=40, BackgroundColor=Color.White, FontSize=14, TextColor=Color.FromHex("3C3C3B"), CornerRadius=15 };
                        BtnOrario.Clicked+=BtnOrario_Clicked;
                        StkRitorno.Children.Add(BtnOrario);
                    }
                });
            }
            );
            
        }

        private void BtnOrario_Clicked(object sender, EventArgs e) {
            var form = new PageNavettaPrenotaPosto();
            form.DataOra=DateTime.Parse(DataSelezionata.ToString("dd/MM/yyyy") + " " + ((Button)sender).Text);
            

            Navigation.PushAsync(form);
        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync();  
        }
    }
}