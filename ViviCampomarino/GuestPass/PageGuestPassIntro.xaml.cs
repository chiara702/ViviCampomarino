using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.GuestPass {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageGuestPassIntro : ContentPage {
        public PageGuestPassIntro() {
            InitializeComponent();
        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync();
        }

        private async void BtnAvanti_Clicked(object sender, EventArgs e) {
            //if (App.LoginUidAuth=="") {
            if (Plugin.Firebase.Auth.CrossFirebaseAuth.Current.CurrentUser==null || App.login==null) {
                var np = new ViviCampomarino.GuestPass.PageLogin();
                await Navigation.PushAsync(np);
                await np.semaforo.WaitAsync();
            } else {
                if (App.RowAzienda!=null && Convert.ToBoolean(App.RowAzienda["FlagStrutturaRicettiva"])==true) {
                    var p = new PageAlbergoMenu();
                    await Navigation.PushAsync(p);
                } else {
                    //Controllo se disponibile coupon o meno
                    try {
                        var mySql = new MySqlvc();
                        var query = $"Select Count(*) From Coupon Where Email='{App.login["Email"].ToString()}' And CheckIn<='{DateTime.Now.ToString("yyyy-MM-dd")}' And CheckOut>='{DateTime.Now.ToString("yyyy-MM-dd")}'";
                        int coupon=Convert.ToInt32( mySql.EseguiScalare(query,0));
                        if (coupon > 0) {
                            await Navigation.PushAsync(new PageGuestPassListaAttivita());
                        } else {
                            await Navigation.PushAsync(new PageGuestPassNessunCoupon());
                        }
                    }catch(Exception ex) {
                        await DisplayAlert("", "Internet non disponibile o funzione al momento non disponibile! " + ex.Message, "OK");
                        return;
                    }
                    var CouponDisponibile = false;
                    if (CouponDisponibile==true) {

                    } else {

                    }
                }
                
            }
            
            
            
        }
        protected override void OnAppearing() {
            base.OnAppearing();
        }

    }
}