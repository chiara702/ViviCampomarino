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
    public partial class PageAlbergoModificaCoupon : ContentPage {
        public PageAlbergoModificaCoupon() {
            InitializeComponent();
            Task.Run(()=>LoadCoupon());
        }
        public void LoadCoupon() {
            var mySql = new MySqlvc();
            var tableCoupon=mySql.EseguiQuery($"Select * From Coupon Where IdAzienda={App.RowAzienda["Id"].ToString()} And CheckOut>='{DateTime.Now.ToString("yyyy-MM-dd")}'");
            App.RowAzienda=mySql.EseguiRow($"Select * From Aziende Where Id={Convert.ToInt32(App.login["IdAzienda"])}");
            mySql.CloseCommit();
            Device.BeginInvokeOnMainThread(() => {
                foreach (DataRow x in tableCoupon.Rows) {
                    var tmp = new ViewCoupon();
                    tmp.Inizializza(x);
                    tmp.onDelete += (s,e) => {
                        var mySql = new MySqlvc();
                        mySql.EseguiCommand("Delete From Coupon Where Id=" + tmp.getId.ToString());
                        mySql.CloseCommit();
                        Stk.Children.Remove(tmp);
                    };
                    Stk.Children.Add(tmp);
                }
            });

        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync();
        }
    }
}