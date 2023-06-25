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
            if (Convert.ToBoolean(App.login["AdminNavetta"])==true || Debugger.IsAttached==true) BtnAmministratore.IsEnabled=true;
        }

        private async void BtnPrenota_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new PageNavettaPrenotaCalendario());
        }

        private void BtnAmministratore_Clicked(object sender, EventArgs e) {
            Navigation.PushAsync(new PageNavettaMenuAmministratore());
        }

        private async void BtnIndietro_Clicked(object sender, EventArgs e) {
            await Navigation.PopAsync();
        }

        private async void BtnMappa_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new PageNavettaMappa());
        }
    }
}