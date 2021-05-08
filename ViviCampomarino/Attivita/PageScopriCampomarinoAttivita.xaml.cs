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
    public partial class PageScopriCampomarinoAttivita : ContentPage
    {
        public PageScopriCampomarinoAttivita()
        {
            InitializeComponent();
        }
        private async void BtnIndietro_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void BtnDormire_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnMangiare_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnTecnico_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnCamping_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnShopping_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnNumeriUtili_Clicked(object sender, EventArgs e)
        {

        }
    }
}