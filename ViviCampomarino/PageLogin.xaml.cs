using Plugin.Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageLogin : ContentPage {
        private IFirebaseAuth authCurrent = Plugin.Firebase.Auth.CrossFirebaseAuth.Current;
        public PageLogin() {
            InitializeComponent();
        }

        private async void BtnAccedi_Clicked(object sender, EventArgs e) {
            var ListaUtentiRegistratiConEmail = await authCurrent.FetchSignInMethodsAsync(TxtEmail.Text);
            if (ListaUtentiRegistratiConEmail.Count() == 0) {
                await DisplayAlert("Login", "Email non presente! Controlla la corretta digitazione o di esserti registrato!", "OK");
                return;
            }
            try {
                var tmp = await authCurrent.SignInWithEmailAndPasswordAsync(TxtEmail.Text, TxtPassword.Text);
                if (tmp.IsEmailVerified == false) {
                    await DisplayAlert("Login", "Account non ancora validato. E' stata inviata una nuova e-mail di validazione! Controlla la tua email e clicca il link per confermare la registrazione!", "OK");
                    await tmp.SendEmailVerificationAsync();
                    return;
                }
                //Login OK
                App.LoginUidAuth = tmp.Uid;
                App.SalvaImpostazioni();
                var db = new Database<Login>();
                App.login = await db.ReadDocument("Login/" + App.LoginUidAuth);
                await App.Current.MainPage.Navigation.PopToRootAsync();
                await App.Current.MainPage.Navigation.PushAsync(new PageHome());
            }catch (Exception err) { //FirebaseAuthInvalidCredentialsException
                if (err.Message.Contains("The password is invalid")) await DisplayAlert("Login", "Password errata!","Ok");
                else await DisplayAlert("Login", "Errore generale: " + err.Message, "Ok");
                return;
            }
            
        }

        private void BtnRegistrati_Clicked(object sender, EventArgs e) {
            App.Current.MainPage.Navigation.PushAsync(new PageRegistrazione());
        }
    }
}