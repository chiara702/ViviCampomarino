using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.GuestPass {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAlbergoMenu : ContentPage {
        public PageAlbergoMenu() {
            InitializeComponent();
            
        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopToRootAsync();
        }

        private void BtnGeneraCoupon_Clicked(object sender, EventArgs e) {
            Navigation.PushAsync(new PageAlbergoGeneraCoupon());
        }

        private void BtnModificaCoupon_Clicked(object sender, EventArgs e) {
            Navigation.PushAsync(new PageAlbergoModificaCoupon());
        }
    }
}