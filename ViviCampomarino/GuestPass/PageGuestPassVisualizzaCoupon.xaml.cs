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
    public partial class PageGuestPassVisualizzaCoupon : ContentPage {
        private int Id;
        private DataRow rowAzienda;
        public PageGuestPassVisualizzaCoupon(int Id) {
            InitializeComponent();
            this.Id=Id;
            var tdb=Task.Run(DbLoad);
            Task.Run(() => checkAccelerometro());
            Task.Run(UpdateTime);
            //tdb.Wait(500);
        }

        private void DbLoad() {
            var db = new MySqlvc();
            rowAzienda=db.EseguiRow("Select * From Aziende Where Id=" + Id);
            Device.BeginInvokeOnMainThread(() => {
                LblDenominazione.Text=rowAzienda["Denominazione"].ToString();
                LblDescrizione.Text=rowAzienda["Descrizione"].ToString();
                LblIndirizzo.Text=rowAzienda["Indirizzo"].ToString();
                LblPaese.Text=rowAzienda["Paese"].ToString();
                LblTelefono.Text=rowAzienda["Telefono"].ToString();
                LblCell1.Text=rowAzienda["Cellulare1"].ToString() + " " + rowAzienda["Cellulare2"].ToString();
                LogoAzienda.Source = ImageSource.FromStream(() => new System.IO.MemoryStream((byte[])rowAzienda["Logo"]));
                if (rowAzienda["PercentualeSconto"].ToString()=="5") ImgCoupon.Source="Coupon5p.png";
                if (rowAzienda["PercentualeSconto"].ToString()=="10") ImgCoupon.Source="Coupon10p.png";
                if (rowAzienda["PercentualeSconto"].ToString()=="15") ImgCoupon.Source="Coupon15p.png";
                if (rowAzienda["PercentualeSconto"].ToString()=="20") ImgCoupon.Source="Coupon20p.png";
                if (rowAzienda["PercentualeSconto"].ToString()=="30") ImgCoupon.Source="Coupon30p.png";
            });
        }

        private void UpdateTime() {
            while (true) {
                Device.BeginInvokeOnMainThread(() => {
                    LblDataOra.Text=DateTime.Now.ToString("dd/MM/yy HH:mm:ss");
                });
                Task.Delay(500);
            }
        }
        private void checkAccelerometro() {
            //Xamarin.Essentials.Accelerometer.Start(Xamarin.Essentials.SensorSpeed.Default);
            //int startX = ImgViviCampomarino.X;
            //while (true) {
            //    Task.Delay(100);
            //    Xamarin.Essentials.Accelerometer.ReadingChanged+=(s,e) => {
            //        e.Reading.
            //    };
            //}
        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Xamarin.Essentials.Accelerometer.Stop();
            Navigation.PopAsync();
        }
    }
}