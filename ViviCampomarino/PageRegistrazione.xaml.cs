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

        private void LinkPrivacy_Tapped(object sender, EventArgs e){
            Xamarin.Essentials.Browser.OpenAsync("http://vivi-campomarino.web.app/Privacy.pdf");
        }

        private void CheckPrivacy_CheckedChanged(object sender, CheckedChangedEventArgs e){
            BtnRegistrati.IsEnabled=CheckPrivacy.IsChecked;
        }

        private async void TextBoxLampeggiante(Entry Source) {
            for (int x = 0; x <= 3; x++) {
                await ViewExtensions.ColorTo(Source, Color.White, Color.Red, i=>Source.BackgroundColor=i,150);
                await ViewExtensions.ColorTo(Source, Color.Red, Color.White, i => Source.BackgroundColor = i, 150);
            }
        }
        private async void BtnRegistrati_Clicked(object sender, EventArgs e){
            if (Funzioni.Antinull(TxtNome.Text).Length < 3) { _ = Task.Run(() => TextBoxLampeggiante(TxtNome)); _ = Scrool1.ScrollToAsync(0, 0, true); return; }
            if (Funzioni.Antinull(TxtCognome.Text).Length < 3) { _ = Task.Run(() => TextBoxLampeggiante(TxtCognome)); _ = Scrool1.ScrollToAsync(0, 100, true); return; }
            if (Funzioni.Antinull(TxtPaese.Text).Length < 3) { _ = Task.Run(() => TextBoxLampeggiante(TxtPaese)); _ = Scrool1.ScrollToAsync(0, 200, true); return; }
            if (Funzioni.Antinull(TxtTelefono.Text).Length < 3) { _ = Task.Run(() => TextBoxLampeggiante(TxtTelefono)); _ = Scrool1.ScrollToAsync(0, 300, true); return; }
            if (Funzioni.Antinull(TxtPassword.Text).Length < 6) { await Task.Run(() => TextBoxLampeggiante(TxtPassword)); await DisplayAlert("", "La password deve essere lunga almeno 6 caratteri!", "OK"); }
            if (Funzioni.Antinull(TxtPassword.Text) != Funzioni.Antinull(TxtPassword2.Text)) { _ = Task.Run(() => TextBoxLampeggiante(TxtPassword2)); return; }

            if (Funzioni.IsValidEmail(Funzioni.Antinull(TxtEmail.Text)) == false) {
                await DisplayAlert("Registrazione", "Email non valida!", "OK");
                _ = Task.Run(() => TextBoxLampeggiante(TxtEmail));
                return;
            }
            try {
                var ListaUtentiRegistratiConEmail = await authCurrent.Instance.FetchSignInMethodsForEmailAsync(Funzioni.Antinull(TxtEmail.Text).Trim());
                if (ListaUtentiRegistratiConEmail!=null && ListaUtentiRegistratiConEmail.Length>0 && String.IsNullOrEmpty(ListaUtentiRegistratiConEmail[0])==false) {
                    await DisplayAlert("Registrazione", "Email già presente nei nostri archivi. Se hai dimenticato la password puoi crearne una nuova!", "OK");
                    _ = Task.Run(() => TextBoxLampeggiante(TxtEmail));
                    return;
                }
            } catch (Exception er) {
                await DisplayAlert("", "Errore nell'email! "+ er.Message, "OK");
                _ = Task.Run(() => TextBoxLampeggiante(TxtEmail));
                return;
            }
            var tmp=await authCurrent.Instance.CreateUserWithEmailAndPasswordAsync(Funzioni.Antinull(TxtEmail.Text).Trim(), Funzioni.Antinull(TxtPassword.Text).Trim());
            var UidAuth = tmp.User.Uid; //tmp.Uid;
            var Db = new MySqlvc();
            var Bis = new MySqlvc.DBSqlBis(Db, "Login");
            var Par = Bis.GetParam;
            //var db = new Database<Login>();
            //var login = new Login();
            Par.AddWithValue("Cognome",Funzioni.Antinull(TxtCognome.Text).Trim());
            Par.AddWithValue("Nome", Funzioni.Antinull(TxtNome.Text).Trim());
            Par.AddWithValue("Email", Funzioni.Antinull(TxtEmail.Text).Trim());
            Par.AddWithValue("Password", Funzioni.Antinull(TxtPassword.Text).Trim());
            Par.AddWithValue("Paese", Funzioni.Antinull(TxtPaese.Text).Trim());
            Par.AddWithValue("Telefono", Funzioni.Antinull(TxtTelefono.Text).Trim());
            Par.AddWithValue("UidAuth", UidAuth);
            Bis.GeneraInsert();
            Db.CloseCommit();
            //db.WriteDocument("Login/" + UidAuth, login);
            await authCurrent.Instance.CurrentUser.SendEmailVerificationAsync();
            await DisplayAlert("Registrazione", "E' stata appena inviata una email per confermare la registrazione.", "OK");
            MySqlvc.WriteLog("Registrazione Ok: " + Funzioni.Antinull(TxtCognome.Text));
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

        

        protected override bool OnBackButtonPressed(){
            BtnIndietro_Clicked(null, null);
            return true;
        }
        private async void BtnIndietro_Clicked(object sender, EventArgs e){
            await Navigation.PopAsync();
        }

        private void TxtPassword_Unfocused(object sender, FocusEventArgs e)
        {

        }

        private void TxtPassword2_Unfocused(object sender, FocusEventArgs e)
        {

        }
    }
}