using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageHome : ContentPage {
        public PageHome() {
            InitializeComponent();
            
        }
        protected override void OnAppearing() {
            base.OnAppearing();
            if (App.login != null) {
                StackLogin.IsVisible=true;
                TxtLogin.Text=App.login["Nome"].ToString();
            } else StackLogin.IsVisible = false;
        }

        async void BtnBiblioteca_Clicked(object sender, EventArgs e) {
            await App.Current.MainPage.Navigation.PushAsync(new PageHomeBiblioteca());
        }

        async void BtnEventi_Clicked(object sender, EventArgs e) {
            //Test Database MySql
            //var Db = new MySqlvc();
            //var Table = Db.EseguiQuery("Select * From Libri");
            //await DisplayAlert("TEST", "Test MySql: Row count libri: " + Table.Rows.Count.ToString(), "OK");

            await App.Current.MainPage.Navigation.PushAsync(new PageEventiHome());
        }


        async void BtnScopri_Clicked(object sender, EventArgs e)
        {
           
            await App.Current.MainPage.Navigation.PushAsync(new PageScopriCampomarinoMenu(),true);

        }

        private async void BtnBell_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new PageNotifiche());
        }

        private async void BtnGuestPass_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new GuestPass.PageGuestPassIntro());
        }

        private async void TapLogout_Tapped(object sender, EventArgs e) {
            App.LoginUidAuth = "";
            App.login = null;
            App.SalvaImpostazioni();
            await Navigation.PopToRootAsync();
            StackLogin.IsVisible = false;
        }

        private async void BtnNavetta_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new ServizioNavetta.PageNavettaMenu());
        }
    }
}