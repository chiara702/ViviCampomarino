using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRegistrazione : ContentPage{
        //private IFirebaseAuth authCurrent;
        private IFirebaseAuth authCurrent = Plugin.FirebaseAuth.CrossFirebaseAuth.Current;
        public PageRegistrazione(){
            InitializeComponent();
            //authCurrent = Plugin.Firebase.Auth.CrossFirebaseAuth.Current;
        }

        private void LinkPrivacy_Tapped(object sender, EventArgs e)
        {

        }

        private void CheckPrivacy_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            BtnRegistrati.IsEnabled=!CheckPrivacy.IsChecked;
        }

        private async void BtnRegistrati_Clicked(object sender, EventArgs e){
            if (Funzioni.IsValidEmail(Funzioni.Antinull(TxtEmail.Text)) == false) {
                await DisplayAlert("Registrazione", "Email non valida!", "OK");
                return;
            }
            try {
                var ListaUtentiRegistratiConEmail = await authCurrent.Instance.FetchSignInMethodsForEmailAsync(Funzioni.Antinull(TxtEmail.Text).Trim());
                if (ListaUtentiRegistratiConEmail!=null) {
                    await DisplayAlert("Registrazione", "Email già presente nei nostri archivi. Se hai dimenticato la password puoi crearne una nuova!", "OK");
                    return;
                }
            } catch (Exception) {
                await DisplayAlert("", "Errore nell'email!", "OK");
                return;
            }
            var tmp=await authCurrent.Instance.CreateUserWithEmailAndPasswordAsync(Funzioni.Antinull(TxtEmail.Text).Trim(), Funzioni.Antinull(TxtPassword.Text).Trim());
            var UidAuth = tmp.User.Uid; //tmp.Uid;
            var db = new Database<Login>();
            var login = new Login();
            login.Cognome = Funzioni.Antinull(TxtCognome.Text).Trim();
            login.Nome = Funzioni.Antinull(TxtNome.Text).Trim();
            login.Email = Funzioni.Antinull(TxtEmail.Text).Trim();
            login.Password = Funzioni.Antinull(TxtPassword.Text).Trim();
            login.Paese = Funzioni.Antinull(TxtPaese.Text).Trim();
            login.Telefono = Funzioni.Antinull(TxtTelefono.Text).Trim();
            login.UidAuth = UidAuth;
            db.WriteDocument("Login/" + UidAuth, login);
            //await tmp.SendEmailVerificationAsync();
            await authCurrent.Instance.CurrentUser.SendEmailVerificationAsync();
            await DisplayAlert("Registrazione", "E' stata appena inviata una email per confermare la registrazione.", "OK");
            await Navigation.PopAsync();

        }

        private async void TxtEmail_Unfocused(object sender, FocusEventArgs e) {
            try {
                var ListaUtentiRegistratiConEmail = await authCurrent.Instance.FetchSignInMethodsForEmailAsync(Funzioni.Antinull(TxtEmail.Text).Trim());
                if (ListaUtentiRegistratiConEmail.Count() > 0) {
                    _ = Task.Run(() => {
                        var colorOrig = TxtEmail.BackgroundColor;
                        Device.BeginInvokeOnMainThread(() => TxtEmail.BackgroundColor = Color.Red);
                        Task.Delay(1000).Wait();
                        Device.BeginInvokeOnMainThread(() => TxtEmail.BackgroundColor = colorOrig);
                    });
                    return;
                }
            } catch (Exception) { }
        }

        private void TxtPassword_Unfocused(object sender, FocusEventArgs e) {

        }

        private void TxtPassword2_Unfocused(object sender, FocusEventArgs e) {

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
    }
}