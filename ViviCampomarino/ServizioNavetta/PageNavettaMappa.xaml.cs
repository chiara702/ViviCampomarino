using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViviCampomarino.VinoEOlio;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.ServizioNavetta {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageNavettaMappa : ContentPage {
        public PageNavettaMappa() {
            InitializeComponent();
            StkWhatsUpChat.IsVisible=(NavettaImpostazioni.LeggiImpostazione("MostraChatWhatsApp")=="1");
            //Mappa
            LblInfo.Text="Attesa localizzazione navetta...";
            var p = new Xamarin.Forms.Maps.Position(41.95582197035494, 15.03307138401569);
            var span = new MapSpan(p, 0.100, 0.100);
            map1.MoveToRegion(span);
            Task.Run(() => { AggiornaPosizione();});
            Device.StartTimer(TimeSpan.FromSeconds(30), () =>{
                LblInfo.Text="Attesa localizzazione navetta...";
                AggiornaPosizione();
                return true; // True per continuare ad eseguire il timer
            });
        }
        private async void BtnIndietro_Clicked(object sender, EventArgs e) {
            await Navigation.PopAsync();
        }

        private async void AggiornaPosizione() {
            TraccarApiClient traccarApiClient = new TraccarApiClient();
            List<Position> positions = await traccarApiClient.GetPositionsAsync();
            await Device.InvokeOnMainThreadAsync(() => {
                map1.Pins.Clear();
                foreach (var position in positions) {
                    var pin = new Pin();
                    pin.Label="Navetta";
                    pin.Position=new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
                    
                    if (position.FixTime<DateTime.Now.AddMinutes(-5)) {
                        LblInfo.Text=$"Posizione non aggiornata: {position.FixTime.ToString("dd/MM/yy HH:mm:ss")} ultimo aggiornamento.";
                    } else {
                        LblInfo.Text=$"Posizione aggiornata. Velocita: {Math.Floor(position.Speed)}Km/h; Precisione: {position.Accurancy}mt; Altitudine: {Math.Floor(position.Altitude)}mt; ";
                    }
                    if (position.FixTime > DateTime.Now.AddMinutes(-30)) map1.Pins.Add(pin);
                    Console.WriteLine($"Latitudine: {position.Latitude}, Longitudine: {position.Longitude}, OrarioFix: {position.FixTime}");
                }
            });
        }

        private async void ApriChat_Tapped(object sender, EventArgs e) {
            string phoneNumber = NavettaImpostazioni.LeggiImpostazione("NumeroTelefonoAutista");
            if (phoneNumber.StartsWith("+39")==false) { phoneNumber = "+39" + phoneNumber; }
            await Launcher.OpenAsync($"https://api.whatsapp.com/send?phone={phoneNumber}");
        }
    }
}