using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageHomeBiblioteca : ContentPage {
        public PageHomeBiblioteca() {
            InitializeComponent();
            MenuTop.MenuLaterale = MenuLaterale;
        }

        private async void ImgMenu_Tapped(object sender, EventArgs e) {
            MenuLaterale.IsVisible = true;
            await MenuLaterale.Mostra();
        }

        private async void BtnCerca_Clicked(object sender, EventArgs e) {
            await App.Current.MainPage.Navigation.PushAsync(new PageBibliotecaCerca());
        }

        private async void BtnAccount_Clicked(object sender, EventArgs e) {
            if (App.LoginUidAuth=="") await App.Current.MainPage.Navigation.PushAsync(new PageLogin());
            else await App.Current.MainPage.Navigation.PushAsync(new PageAccount());
        }
    }
}