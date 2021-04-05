using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.Test {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageTest : ContentPage {
        public PageTest() {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e) {
            App.Current.MainPage.Navigation.PushAsync(new PageLibro(0));
        }

        private void Button_Clicked_1(object sender, EventArgs e) {
            App.Current.MainPage.Navigation.PushAsync(new PageCercaLibro());
        }
    }
}