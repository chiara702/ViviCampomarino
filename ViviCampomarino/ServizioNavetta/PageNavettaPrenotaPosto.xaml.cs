using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.ServizioNavetta {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageNavettaPrenotaPosto : ContentPage {
        public DateTime DataOra;
        public PageNavettaPrenotaPosto() {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e) {
            DisplayAlert("", sender.ToString() + DataOra.ToLongDateString(), "ok");
        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync();
        }
    }
}