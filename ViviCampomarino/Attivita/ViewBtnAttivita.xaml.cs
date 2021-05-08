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
    public partial class ViewBtnAttivita : ContentView
    {
        public ViewBtnAttivita()
        {
            InitializeComponent();
        }



        async void TapBtn_Tapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushAsync(new ListViewPageSottocategorieAttivita());

        }


    }
}