using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
            Panel.Padding = new Thickness(3);
            var StkExt = new StackLayout();
            Panel.Content=StkExt;
            var Label = new Label();
            Label.TextType=TextType.Html;
            Label.Text= $"<div>Data Ora: <b>{rigo["Giorno"]}</b></div>" +
                        $"<div>Cognome/Nome: <b>{rigo["Nome"]}</b>" +
                        $"<div>Telefono: <b>{rigo["Telefono"]}</b>";
            if (Convert.ToBoolean(rigo["Accompagnatore"])==true) {
                Label.Text+=$"<div>Cognome/Nome: <b>{rigo["NomeAccompagnatore"]}</b>" +
                            $"<div>Telefono: <b>{rigo["TelefonoAccompagnatore"]}</b>";
            }
            Label.Text += $"<div>Indirizzo presa: <b>{rigo["IndirizzoPresa"]}</b>" +
                        $"<div>Indirizzo rilascio: <b>{rigo["IndirizzoRilascio"]}</b>";
            if(rigo["Note"].ToString().Trim() != "") Label.Text += $"<div>Note: <b>{rigo["Note"]}</b>";
            Label.Text += $"<div>Data Ora Creazione: <b>{rigo["DataOraCreazione"]}</b>" +
                        $"<div>Data Ora Modifica: <b>{rigo["DataOraModifica"]}</b>";

            StkExt.Children.Add(Label);
            var Button=new Button();
            Button.Text="Funzioni";
            Button.HorizontalOptions=LayoutOptions.Center;
            Button.Clicked+=Button_Clicked;
            Button.CommandParameter=rigo["Id"].ToString();
            StkExt.Children.Add(Button);
            StkDettaglio.Children.Add(Panel);

        }

        private async void Button_Clicked(object sender, EventArgs e) {
            var id = Convert.ToInt16(((Button)sender).CommandParameter);
            string[] opzioni = { "Chiama","Chat Whats App", "Chiama Accompagnatore", "Chat Accompagnatore", "Naviga verso indirizzo presa", "Naviga verso indirizzo rilascio"};
            String opzioneSelezionata = await DisplayActionSheet("Opzioni", "annulla", null, opzioni);
            if (string.IsNullOrEmpty(opzioneSelezionata) || opzioneSelezionata == "annulla") return;
            var rowPrenotazione = Prenotazioni.Select("Id=" + id)[0];
            if (opzioneSelezionata==opzioni[0]) { Xamarin.Essentials.PhoneDialer.Open(rowPrenotazione["Telefono"].ToString()); }
            if (opzioneSelezionata==opzioni[1]) { await Launcher.OpenAsync(new Uri($"whatsapp://send?phone=+39{rowPrenotazione["Telefono"]}")); }
            if (opzioneSelezionata==opzioni[2] && Funzioni.Antinull(rowPrenotazione["TelefonoAccompagnatore"])=="") { await DisplayAlert("Attenzione", "Numero non disponibile", "Ok"); return; }
            if (opzioneSelezionata==opzioni[2]) { Xamarin.Essentials.PhoneDialer.Open(rowPrenotazione["TelefonoAccompagnatore"].ToString());}
            if (opzioneSelezionata==opzioni[3] && Funzioni.Antinull(rowPrenotazione["TelefonoAccompagnatore"])=="") { await DisplayAlert("Attenzione", "Numero non disponibile", "Ok"); return; }
            if (opzioneSelezionata==opzioni[3]) { await Launcher.OpenAsync(new Uri($"whatsapp://send?phone=+39{rowPrenotazione["TelefonoAccompagnatore"]}")); }
            if (opzioneSelezionata==opzioni[4]) { await Launcher.OpenAsync(new Uri($"https://maps.google.com/?q={rowPrenotazione["IndirizzoPresa"]}")); }
            if (opzioneSelezionata==opzioni[5]) { await Launcher.OpenAsync(new Uri($"https://maps.google.com/?q={rowPrenotazione["IndirizzoRilascio"]}")); }
        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync(true);
        }
    }
}