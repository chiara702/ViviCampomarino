using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.ServizioNavetta {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageNavettaMenu : ContentPage {
        public PageNavettaMenu() {
            InitializeComponent();
            if (Debugger.IsAttached==true) BtnAmministratore.IsVisible=true;
            if (App.login != null && Convert.ToBoolean(App.login["AdminNavetta"])==true) BtnAmministratore.IsVisible=true;
        }

        private async void BtnPrenota_Clicked(object sender, EventArgs e) {
            if (NavettaImpostazioni.LeggiImpostazione("Abilita")=="0" && Debugger.IsAttached==false) { await DisplayAlert("", "Al momento non disponibile", "Ok"); return; }
            await Navigation.PushAsync(new PageNavettaPrenotaCalendario());
        }

        private void BtnAmministratore_Clicked(object sender, EventArgs e) {
            Navigation.PushAsync(new PageNavettaMenuAmministratore());
        }

        private async void BtnIndietro_Clicked(object sender, EventArgs e) {
            await Navigation.PopAsync();
        }

        private async void BtnMappa_Clicked(object sender, EventArgs e) {
            if (NavettaImpostazioni.LeggiImpostazione("Abilita")=="0") { await DisplayAlert("", "Al momento non disponibile", "Ok"); return; }
            await Navigation.PushAsync(new PageNavettaMappa());
        }

        private async void BtnOrari_Clicked(object sender, EventArgs e) {
            var f = new PageNavettaOrari();
            await Navigation.PushAsync(f);
        }
    }
}