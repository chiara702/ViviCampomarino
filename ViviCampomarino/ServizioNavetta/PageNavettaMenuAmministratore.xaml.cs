using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.ServizioNavetta {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageNavettaMenuAmministratore : ContentPage {
        public PageNavettaMenuAmministratore() {
            InitializeComponent();
        }

        

        private void BtnSettings_Clicked(object sender, EventArgs e) {
            Navigation.PushAsync(new PageAmministratoreSettings());
        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync();
        }

        private void BtnCalendario_Clicked(object sender, EventArgs e) {
            Navigation.PushAsync(new PageAmministratoreCalendario());
        }
    }
}