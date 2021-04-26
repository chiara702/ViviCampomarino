using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageScopriCampomarino : ContentPage
    {
        public PageScopriCampomarino()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                CaricaPin();
                if (map1.Pins.Count > 0)
                {
                    var p = new Position(map1.Pins[0].Position.Latitude, map1.Pins[0].Position.Longitude);
                    var span = new MapSpan(p, 0.005, 0.005);
                    map1.MoveToRegion(span);
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("", e.Message, "ok");
            }

        }

        public void CaricaPin(Boolean MostraGestore = false)
        {
           

            }

        private void BtnScanPoint_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnDettagli_Clicked(object sender, EventArgs e)
        {

        }


        private async void BtnInfo_Clicked(object sender, EventArgs e)
        {
            FrmDettagli.IsVisible = false;
            _ = await FrmInfo.FadeTo(0, 1);
            FrmInfo.IsVisible = true;
            _ = await FrmInfo.FadeTo(1, 800);

        }

        private async void TapInfo_Tapped(object sender, EventArgs e)
        { 
            await DisplayAlert("Presto disponibile!", "Funzione presto disponibile", "OK");
            await Navigation.PopAsync();
            return;

            _ = await FrmInfo.FadeTo(0, 500);
            FrmInfo.IsVisible = false;
            _ = await FrmDettagli.FadeTo(0, 1);
            FrmDettagli.IsVisible = true;
            _ = FrmDettagli.FadeTo(1, 800);
           
        }
    }



    }
