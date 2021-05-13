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
    public partial class ListViewPageSottocategorieAttivita : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public ListViewPageSottocategorieAttivita()
        {
            InitializeComponent();

            Items = new ObservableCollection<string>
            {
                "Alimentare",
                "Macellaio",
                "Fruttivendolo",
                "Pescheria",
            
            };

            MyListView.ItemsSource = Items;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");
            await App.Current.MainPage.Navigation.PushAsync(new ListViewPageElencoAttivita());

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
