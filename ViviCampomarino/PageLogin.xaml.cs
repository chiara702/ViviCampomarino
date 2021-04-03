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
    public partial class PageLogin : ContentPage
    {
        public PageLogin()
        {
            InitializeComponent();
        }

        private void BtnAccedi_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnRegistrati_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageRegistrazione());
        }
    }
}