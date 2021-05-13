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
    public partial class PageAttivita : ContentPage
    {
        public PageAttivita()
        {
            InitializeComponent();
        }

        private void TapTelefono_Tapped(object sender, EventArgs e)
        {
            Xamarin.Essentials.PhoneDialer.Open(LblTelefono.Text);
        }
    }
}