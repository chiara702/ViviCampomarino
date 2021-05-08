using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewPageElencoAttivita : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public ListViewPageElencoAttivita()
        {
            InitializeComponent();

            Items = new ObservableCollection<string>
            {
                "Emilio",
                "De Michele",
                "Fruttivendolo tizio",
                "Item 4",
                "Item 5"
            };

            MyListView.ItemsSource = Items;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "Dettagli pagina", "OK");
            await App.Current.MainPage.Navigation.PushAsync(new PageAttivita());

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
