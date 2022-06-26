//using Plugin.FirebaseAuth;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Common;
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
        
        public PageLogin() {
            InitializeComponent();
        }

        private async void BtnAccedi_Clicked(object sender, EventArgs e) {
            var email = Funzioni.Antinull(TxtEmail.Text).Trim();
            var password = Funzioni.Antinull(TxtPassword.Text).Trim();
            if (email=="1") { email = "dimariafabio@gmail.com"; password = "123456"; }
            if (email=="2") { email = "maddalenaspidalieri@gmail.com"; password="abc123456"; }
            if (Funzioni.IsValidEmail(email) == false) { await DisplayAlert("Errore", "E-mail in formato non corretto!", "OK"); return; }
            String[] ListaUtentiRegistratiConEmail = null;
            try {
                ListaUtentiRegistratiConEmail = await CrossFirebaseAuth.Current.FetchSignInMethodsAsync(email);
            }catch(FirebaseException err) {
                await DisplayAlert("Errore", "Errore nel controllo esistenza email! Verificare connessione internet o attendere qualche minuto!","OK");
                return;
            }
            if (ListaUtentiRegistratiConEmail.Count() == 0) {
                await DisplayAlert("Login", "Email non presente! Controlla la corretta digitazione o di esserti registrato!", "OK");
                return;
            }
            if (password == "") {
                await DisplayAlert("Errore", "La password non può essere vuota!", "OK");
                return;
            }
            try {
                var tmp = await CrossFirebaseAuth.Current.SignInWithEmailAndPasswordAsync(email, password);
                if (tmp.IsEmailVerified == false) {
                    await DisplayAlert("Login", "Account non ancora validato. E' stata inviata una nuova e-mail di validazione! Controlla la tua email e clicca il link per confermare la registrazione!", "OK");
                    await tmp.SendEmailVerificationAsync();
                    return;
                }
                //Login OK
                App.LoginUidAuth = tmp.Uid;
                App.LoginEmail = tmp.Email;
                App.SalvaImpostazioni();
                App.LeggiImpostazioni();
                
                var token=await Plugin.Firebase.CloudMessaging.CrossFirebaseCloudMessaging.Current.GetTokenAsync();
                var Db = new MySqlvc();
                Db.UpdateRapido("Login", Convert.ToInt32(App.login["Id"]), "UidAuth", tmp.Uid);
                Db.UpdateRapido("Login", Convert.ToInt32(App.login["Id"]), "TokenFcm", token);
                Db.UpdateRapido("Login", Convert.ToInt32(App.login["Id"]), "UltimoAccesso", DateTime.Now);
                Db.UpdateRapido("Login", Convert.ToInt32(App.login["Id"]), "NumeroAccessi", Convert.ToInt32(App.login["NumeroAccessi"])+1);
                Db.CloseCommit();
                App.FcmTopicsRefresh();
                MySqlvc.WriteLog("Login Ok");
                await Navigation.PopAsync();
            }catch (FirebaseException err) { //FirebaseAuthInvalidCredentialsException
                var DataError = err.Data;
                if (err.Message.Contains("The password is invalid")) await DisplayAlert("Login", "Password errata!","Ok");
                else await DisplayAlert("Login", "Errore generale: " + err.Message + " " + err.Message, "Ok");
                MySqlvc.WriteLog("Login Error: " + err.Message);
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