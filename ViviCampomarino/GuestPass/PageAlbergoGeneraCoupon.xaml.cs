using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.GuestPass {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAlbergoGeneraCoupon : ContentPage {
        public PageAlbergoGeneraCoupon() {
            InitializeComponent();
            DateCheckIn.Date = DateTime.Now;
            DateCheckOut.Date = DateTime.Now.AddDays(1);
        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync();
        }


        Regex regexEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        private void DateCheckIn_DateSelected(object sender, DateChangedEventArgs e) {
            var totalDay = (DateCheckOut.Date-DateCheckIn.Date).TotalDays;
            LabelGiorni.Text="";
            if (totalDay < 1 || totalDay > 30) {
                LabelGiorni.Text="Errore nelle date di CheckIn/CheckOut";
                //BtnGeneraCoupon.IsEnabled=false;
            } else {
                LabelGiorni.Text="";
                //BtnGeneraCoupon.IsEnabled=true;
            }
            //Valida Email
            
            if (TxtEmail.Text!="" && regexEmail.IsMatch(TxtEmail.Text)==false) {
                LabelGiorni.Text="Email in formato errato!";
            }

        }

        private void DateCheckOut_DateSelected(object sender, DateChangedEventArgs e) {
            DateCheckIn_DateSelected(null, null);
            
        }

        private Boolean VerificaDisponibilitaCoupon() {
            var mySql = new MySqlvc();
            var tableCoupon = mySql.EseguiQuery($"Select * From Coupon Where IdAzienda={App.login["IdAzienda"].ToString()}");
            mySql.CloseCommit();
            var stanzeDisponibili = Convert.ToInt32(App.RowAzienda["NumeroCamere"]);
            var stanzeOccupateMax = 0;
            for (DateTime data = DateCheckIn.Date; data<DateCheckOut.Date; data=data.AddDays(1)) {
                var tmp = 0;
                foreach (DataRow coupon in tableCoupon.Rows) {
                    if (Convert.ToDateTime(coupon["CheckIn"])<=data && Convert.ToDateTime(coupon["CheckOut"]).AddDays(-1)>=data) tmp++;
                }
                Console.WriteLine($"Data: {data.ToString()} StanzeOccupate: {tmp}");
                if (tmp>stanzeOccupateMax) stanzeOccupateMax=tmp;
            }
            if (stanzeOccupateMax>=stanzeDisponibili) return false; else return true;
        }

        private void BtnGeneraCoupon_Clicked(object sender, EventArgs e) {
            if (regexEmail.IsMatch(TxtEmail.Text)==false) {
                DisplayAlert("", "Occorre inserire una email in formato corretto!", "OK");
                return;
            }
            if ((DateCheckOut.Date-DateCheckIn.Date).TotalDays<1 || (DateCheckOut.Date-DateCheckIn.Date).TotalDays>30) {
                DisplayAlert("", "Data di Check-In/Check-Out errata!", "OK");
                return;
            }
            if (VerificaDisponibilitaCoupon()==false) {
                DisplayAlert("", "Disponibilità coupon superata per queste date!", "OK");
                return;
            }
            try { 
                var mySql = new MySqlvc();
                var bis = new MySqlvc.DBSqlBis(mySql, "Coupon");
                bis.GetParam.AddWithValue("IdAzienda", App.login["IdAzienda"]);
                bis.GetParam.AddWithValue("Email", TxtEmail.Text);
                bis.GetParam.AddWithValue("CheckIn", DateCheckIn.Date);
                bis.GetParam.AddWithValue("CheckOut", DateCheckOut.Date);
                bis.GetParam.AddWithValue("Visualizzazioni", 0);
                bis.GeneraInsert();
                mySql.CloseCommit();
            } catch (Exception err) {
                DisplayAlert("", err.Message, "OK");
                return;
            }
            DisplayAlert("", "Coupon registrato con successo!","OK");
            Navigation.PopAsync();
        }
    }
}