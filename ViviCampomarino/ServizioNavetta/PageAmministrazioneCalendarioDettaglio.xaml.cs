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
    public partial class PageAmministrazioneCalendarioDettaglio : ContentPage {
        private DateTime DataSeleziona;
        private DataTable Prenotazioni;
        public PageAmministrazioneCalendarioDettaglio(DateTime DataSelezionata) {
            InitializeComponent();
            this.DataSeleziona= DataSelezionata;
            LblData.Text=DataSeleziona.ToString("dd/MM/yyyy");
            var Db = new MySqlvc();
            Prenotazioni=Db.EseguiQuery($"Select * From NavettaPrenotazioni Where DATE(Giorno)='{DataSeleziona.ToString("yyyy-MM-dd")}'");
            foreach (DataRow x in Prenotazioni.Rows) {
                CreaDettaglio(x);
            }
        }

        private void CreaDettaglio(DataRow rigo) {
            var Panel = new Frame();
            Panel.BorderColor = Color.Black;
            Panel.BackgroundColor = Color.WhiteSmoke;
            var StkExt = new StackLayout();
            Panel.Content=StkExt;
            var Label = new Label();
            Label.TextType=TextType.Html;
            Label.Text= $"<div><b>Data Ora: {rigo["Giorno"]}</div>" +
                        $"<div><b>Cognome/Nome: {rigo["Nome"]}" +
                        $"<div><b>Telefono: {rigo["Telefono"]}";

            StkExt.Children.Add(Label);

            StkDettaglio.Children.Add(Panel);

        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync(true);
        }
    }
}