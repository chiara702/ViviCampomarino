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

        async void BtnCerca_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new PageBibliotecaCerca());
        }

        async void BtnAccount_Clicked(object sender, EventArgs e) {
            //await Navigation.PushAsync(new PageAccount());
            await Navigation.PushAsync(new PageLogin());
        }
    }
}