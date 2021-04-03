using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAccount : ContentPage {
        public PageAccount() {
            InitializeComponent();
            MenuTop.MenuLaterale = MenuLaterale;
        }

        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            MenuLaterale.IsVisible = true;
            await MenuLaterale.Mostra();
        }
    }
}