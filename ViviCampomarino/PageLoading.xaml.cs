using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageLoading : ContentPage {
        public PageLoading() {
            InitializeComponent();
            Task.Run(Animazione);
        }

        private async void Animazione() {
            await LogoCampomarino.FadeTo(0, 1);
            _ = LogoCampomarino.RelScaleTo(0.1, 1);
            _ = LogoCampomarino.RelScaleTo(0.4, 3000);
            await LogoCampomarino.FadeTo(1, 3000);
            await Task.Delay(1000);
            Device.BeginInvokeOnMainThread(() => {
                if (App.Current.MainPage is PageNotifiche == false) {
                    var Nav = new NavigationPage(new PageHome());
                    App.Current.MainPage = Nav;
                    Nav.BarBackgroundColor = Color.FromHex("3c3c3b");
                    Nav.BarTextColor = Color.White;
                }
            });

        }

        protected async override void OnAppearing() {
            base.OnAppearing();
            var currentVersion = VersionTracking.CurrentVersion;
            LblVersion.Text = "vers." + currentVersion;

        }

    }
}