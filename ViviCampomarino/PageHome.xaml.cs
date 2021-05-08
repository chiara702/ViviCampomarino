using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageHome : ContentPage {
        public PageHome() {
            InitializeComponent();
        }

        async void BtnBiblioteca_Clicked(object sender, EventArgs e) {
            await App.Current.MainPage.Navigation.PushAsync(new PageHomeBiblioteca());
        }

        async void BtnEventi_Clicked(object sender, EventArgs e) {
            //Test Database MySql
            //var Db = new MySqlvc();
            //var Table = Db.EseguiQuery("Select * From Libri");
            //await DisplayAlert("TEST", "Test MySql: Row count libri: " + Table.Rows.Count.ToString(), "OK");

            await App.Current.MainPage.Navigation.PushAsync(new PageEventiHome());
        }


        async void BtnScopri_Clicked(object sender, EventArgs e)
        {
           
            await App.Current.MainPage.Navigation.PushAsync(new PageScopriCampomarinoMenu(),true);

        }
    }
}