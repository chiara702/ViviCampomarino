using Plugin.Firebase.Auth;
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
        private IFirebaseAuth authCurrent;
        public PageRegistrazione(){
            InitializeComponent();
            authCurrent = Plugin.Firebase.Auth.CrossFirebaseAuth.Current;
        }

        private void LinkPrivacy_Tapped(object sender, EventArgs e)
        {

        }

        private void CheckPrivacy_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }

        private async void BtnRegistrati_Clicked(object sender, EventArgs e){
            var ListaUtentiRegistratiConEmail = await authCurrent.FetchSignInMethodsAsync(TxtEmail.Text);
            if (ListaUtentiRegistratiConEmail.Count() > 0) {
                await DisplayAlert("Registrazione", "Email già presente nei nostri archivi. Se hai dimenticato la password puoi crearne una nuova!", "OK");
                return;
            }
            var tmp=await authCurrent.SignInWithEmailAndPasswordAsync(TxtEmail.Text, TxtPassword.Text);
            var UidAuth = tmp.Uid;
            var db = new Database<Login>();
            var login = new Login();
            login.Cognome = Funzioni.Antinull(TxtCognome.Text);
            login.Nome = Funzioni.Antinull(TxtNome.Text);
            login.Email = Funzioni.Antinull(TxtEmail.Text);
            login.Password = Funzioni.Antinull(TxtPassword.Text);
            login.Paese = Funzioni.Antinull(TxtPaese.Text);
            login.Telefono = Funzioni.Antinull(TxtTelefono.Text);
            login.UidAuth = UidAuth;
            db.WriteDocument("Login/" + UidAuth, login);
            await tmp.SendEmailVerificationAsync();
            await DisplayAlert("Registrazione", "E' stata appena inviata una email per confermare la registrazione.", "OK");
            await Navigation.PopAsync();

        }

        private async void TxtEmail_Unfocused(object sender, FocusEventArgs e) {
            var ListaUtentiRegistratiConEmail = await authCurrent.FetchSignInMethodsAsync(Funzioni.Antinull(TxtEmail.Text));
            if (ListaUtentiRegistratiConEmail.Count() > 0) {
                _=Task.Run(() => {
                    var colorOrig = TxtEmail.BackgroundColor;
                    Device.BeginInvokeOnMainThread(() =>TxtEmail.BackgroundColor=Color.Red);
                    Task.Delay(1000).Wait();
                    Device.BeginInvokeOnMainThread(() => TxtEmail.BackgroundColor = colorOrig);
                });
                return;
            }
        }

        private void TxtPassword_Unfocused(object sender, FocusEventArgs e) {

        }

        private void TxtPassword2_Unfocused(object sender, FocusEventArgs e) {

        }
    }
}