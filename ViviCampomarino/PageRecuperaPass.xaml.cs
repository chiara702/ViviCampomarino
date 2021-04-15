using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRecuperaPass : ContentPage {
        private IFirebaseAuth authCurrent = Plugin.FirebaseAuth.CrossFirebaseAuth.Current;
        public PageRecuperaPass() {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed() {
            BtnIndietro_Clicked(null, null);
            return true;
        }
        private async void BtnIndietro_Clicked(object sender, EventArgs e) {
            await Navigation.PopAsync();
        }

        private async void BtnRecupera_Clicked(object sender, EventArgs e) {
            TxtEmail.Text = Funzioni.Antinull(TxtEmail.Text).Trim();
            if (Funzioni.IsValidEmail(TxtEmail.Text) == false) {
                await DisplayAlert("", "Email in formato non corretto", "Riprova");
                return;
            }
            try {
                await authCurrent.Instance.SendPasswordResetEmailAsync(TxtEmail.Text);
            } catch (Exception error) {
                await DisplayAlert("", error.Message, "OK");
            }
            await DisplayAlert("Reset password","Ti abbiamo inviato una e-mail per resettare la password","OK");
            
        }
    }
}