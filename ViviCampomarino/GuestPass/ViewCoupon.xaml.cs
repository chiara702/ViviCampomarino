using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.GuestPass {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewCoupon : ContentView {
        public ViewCoupon() {
            InitializeComponent();
        }
        private DataRow rowCoupon;
        public int getId {
            get {
                return Convert.ToInt32(rowCoupon["Id"]);
            }
        }
        public void Inizializza(DataRow rowCoupon) {
            this.rowCoupon=rowCoupon;
            LblEmail.Text=rowCoupon["Email"].ToString();
            LblCheckIn.Text=Convert.ToDateTime(rowCoupon["CheckIn"]).ToString("dd/MM/yy");
            LblCheckOut.Text=Convert.ToDateTime(rowCoupon["CheckOut"]).ToString("dd/MM/yy");

        }
        public event EventHandler onDelete;
        private async void BtnDelete_Clicked(object sender, EventArgs e) {
            if (await Application.Current.MainPage.DisplayAlert("", "Sicuro di voler eliminare questo coupon?", "Si", "No")==false) return;
            onDelete.Invoke(this, null);
        }
    }
}