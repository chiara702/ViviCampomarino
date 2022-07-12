using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViviCampomarino.EBike;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageScopriCampomarinoMenu : ContentPage
    {
        public PageScopriCampomarinoMenu()
        {
            InitializeComponent();
        }

        private async void BtnIndietro_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
       async void BtnAttivita_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushAsync(new PageScopriCampomarinoAttivita(0), false);

        }

        async void BtnScopri_Clicked(object sender, EventArgs e)
        {

            await App.Current.MainPage.Navigation.PushAsync(new PageScopriCampomarino(), false);

        }

        private async void BtnPercorsoOlioVino_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new PagePercorsoVinoEOlio());
        }

        

        private async void BtnEbike_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new PagePercorsoEBike());
        }
    }
}