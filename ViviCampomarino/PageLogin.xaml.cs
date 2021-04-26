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
    public partial class PageLogin : ContentPage {
        private IFirebaseAuth authCurrent = Plugin.FirebaseAuth.CrossFirebaseAuth.Current;
        public PageLogin() {
            InitializeComponent();
        }

        private async void BtnAccedi_Clicked(object sender, EventArgs e) {
            TxtEmail.Text = Funzioni.Antinull(TxtEmail.Text);
            TxtPassword.Text = Funzioni.Antinull(TxtPassword.Text);
            if (TxtEmail.Text=="1") { TxtEmail.Text = "dimariafabio@gmail.com"; TxtPassword.Text = "123456"; }
            if (Funzioni.IsValidEmail(TxtEmail.Text) == false) await DisplayAlert("Errore", "E-mail in formato non corretto!", "OK");

            var ListaUtentiRegistratiConEmail = await authCurrent.Instance.FetchSignInMethodsForEmailAsync(TxtEmail.Text);
            if (ListaUtentiRegistratiConEmail.Count() == 0) {
                await DisplayAlert("Login", "Email non presente! Controlla la corretta digitazione o di esserti registrato!", "OK");
                return;
            }
            try {
                var tmp = await authCurrent.Instance.SignInWithEmailAndPasswordAsync(TxtEmail.Text, TxtPassword.Text);
                if (tmp.User.IsEmailVerified == false) {
                    await DisplayAlert("Login", "Account non ancora validato. E' stata inviata una nuova e-mail di validazione! Controlla la tua email e clicca il link per confermare la registrazione!", "OK");
                    await tmp.User.SendEmailVerificationAsync();
                    return;
                }
                //Login OK
                App.LoginUidAuth = tmp.User.Uid;
                App.SalvaImpostazioni();
                App.LeggiImpostazioni();
                
                var token=await Plugin.Firebase.CloudMessaging.CrossFirebaseCloudMessaging.Current.GetTokenAsync();
                var Db = new MySqlvc();
                Db.UpdateRapido("Login", Convert.ToInt32(App.login["Id"]), "TokenFcm", token);
                Db.UpdateRapido("Login", Convert.ToInt32(App.login["Id"]), "UltimoAccesso", DateTime.Now);
                Db.UpdateRapido("Login", Convert.ToInt32(App.login["Id"]), "NumeroAccessi", Convert.ToInt32(App.login["NumeroAccessi"])+1);
                Db.CloseCommit();
                App.FcmTopicsRefresh();
                await Navigation.PopAsync();
            }catch (FirebaseAuthException err) { //FirebaseAuthInvalidCredentialsException
                var DataError = err.Data;
                if (err.Message.Contains("The password is invalid")) await DisplayAlert("Login", "Password errata!","Ok");
                else await DisplayAlert("Login", "Errore generale: " + err.Message + " " + err.ErrorCode, "Ok");
                return;
            }
            
        }

        private void BtnRegistrati_Clicked(object sender, EventArgs e) {
            App.Current.MainPage.Navigation.PushAsync(new PageRegistrazione());
        }
        protected override bool OnBackButtonPressed()
        {
            BtnIndietro_Clicked(null, null);
            return true;
        }
        private async void BtnIndietro_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void BtnRecuperaPassword_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new PageRecuperaPass());
        }

 
    }
}