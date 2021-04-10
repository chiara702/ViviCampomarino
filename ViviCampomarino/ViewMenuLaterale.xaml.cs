using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewMenuLaterale : Grid {
        public ViewMenuLaterale() {
            InitializeComponent();
            
        }

        public async Task Mostra() {
            await MenuLaterale.TranslateTo(-250, 0, 1, Easing.Linear);
            await MenuLaterale2.TranslateTo(-250, 0, 1, Easing.Linear);
            GridOverlay.IsVisible = true;
            _ = MenuLaterale.TranslateTo(0, 0, 400, Easing.Linear);
            await MenuLaterale2.TranslateTo(0, 0, 400, Easing.Linear);
        }



        private async void TapCloseMenu_Tapped(object sender, EventArgs e) {
            _ = MenuLaterale.TranslateTo(-250, 0, 400, Easing.Linear);
            await MenuLaterale2.TranslateTo(-250, 0, 400, Easing.Linear);
            GridOverlay.IsVisible = false;
        }


        //private void TapCercalibro_Tapped(object sender, EventArgs e) {
        //    //Application.Current.MainPage = new PageSuperuser();

        //}

        private void TapCercaLibro_Tapped(object sender, EventArgs e) {
            App.Current.MainPage.Navigation.PushAsync(new PageBibliotecaCerca());
        }

        private async void TapAccount_Tapped(object sender, EventArgs e) {
            if (App.LoginUidAuth == "") await App.Current.MainPage.Navigation.PushAsync(new PageLogin());
            else await App.Current.MainPage.Navigation.PushAsync(new PageAccount());
            //App.Current.MainPage.Navigation.PushAsync(new PageAccount());
        }


        private void TapHome_Tapped(object sender, EventArgs e) {
            App.Current.MainPage.Navigation.PushAsync(new PageHome());
        }

        private async void TapLogout_Tapped(object sender, EventArgs e) {
            App.LoginUidAuth = "";
            App.login = null;
            App.SalvaImpostazioni();
            await Navigation.PopToRootAsync();
        }
    }
}