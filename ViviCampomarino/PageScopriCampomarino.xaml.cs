using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Data;
using System.Diagnostics;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;
using Xamarin.Essentials;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageScopriCampomarino : ContentPage {
        public PageScopriCampomarino() {
            InitializeComponent();
        }
        protected override void OnAppearing() {
            base.OnAppearing();
            CaricaPin(); 
            var p = new Position(41.95582197035494, 15.03307138401569);
            var span = new MapSpan(p, 0.015, 0.015);
            map1.MoveToRegion(span);
            
            FrmInfo.FadeTo(1, 800);
            
            

            

        }
        DataTable TablePunti;
        DataRow RowSelezionata;
        public void CaricaPin(Boolean MostraGestore = false) {
            var Db = new MySqlvc();
            TablePunti = Db.EseguiQuery("Select * From PuntiInteresse");
            Db.CloseCommit();
            foreach (DataRow x in TablePunti.Rows) {
                var tmpRow = x;
                var pin = new Pin();
                pin.Position = new Position(Convert.ToDouble(x["Latitudine"]), Convert.ToDouble(x["Longitudine"]));
                pin.Label = x["Nome"].ToString();
                pin.MarkerClicked += (s,e) => {
                    Device.BeginInvokeOnMainThread(() => {
                        RowSelezionata = tmpRow;
                        FrmDettagli.FadeTo(0, 1);
                        FrmDettagli.IsVisible = true;
                        _ = FrmDettagli.FadeTo(1, 800);
                        LblDenominazione.Text = tmpRow["Titolo"].ToString();
                        LblDescrizione.Text = tmpRow["Descrizione"].ToString();
                       
                    });
                };
                map1.Pins.Add(pin);
            }

        }

        private  async void BtnScanPoint_Clicked(object sender, EventArgs e) {
            var options = new MobileBarcodeScanningOptions {
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
                TryHarder = true
            };
            var overlay = new ZXingDefaultOverlay {
                TopText = "SCANSIONA IL QR-CODE PER INIZIARE IL TOUR",
                BottomText = ""
            };

            var Pagescanner = new ZXingScannerPage(options, overlay);
            //var Pagescanner = new ZXingScannerView();

            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted) {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }
            if (status != PermissionStatus.Granted)
                return;
            

            await Navigation.PushAsync(Pagescanner);
            Pagescanner.OnScanResult += async (x) => {
                //Device.BeginInvokeOnMainThread(() => {
                    Pagescanner.IsScanning = false;
                    var rows=TablePunti.Select("QrCode='" + x.Text + "'");
                    if (rows.Count() > 0) {
                    Device.BeginInvokeOnMainThread(async() =>
                    {
                        var f = new PageScopriCampomarinoDettagli(rows[0]);
                        await Navigation.PushAsync(f);
                        //await Navigation.PopAsync();
                    });
                    }
                //});
            };



        }

        private void BtnDettagli_Clicked(object sender, EventArgs e) {
            var f = new PageScopriCampomarinoDettagli(RowSelezionata);
            Navigation.PushAsync(f);
        }


        private void BtnInfo_Clicked(object sender, EventArgs e) {
            FrmDettagli.IsVisible = false;
            

        }

        private async void TapInfo_Tapped(object sender, EventArgs e) {
            ////if (Debugger.IsAttached == false) {
            ////    await DisplayAlert("Presto disponibile!", "Funzione presto disponibile", "OK");
            ////    await Navigation.PopAsync();
            ////    return;
            ////}

            _ = await FrmInfo.FadeTo(0, 500);
            FrmInfo.IsVisible = false;
            

        }

        private async void BtnNavigaVerso_Clicked(object sender, EventArgs e) {
            try {
                //Location location = (await Geocoding.GetLocationsAsync(RowSelezionata["Latitudine"].ToString() + "," + (RowSelezionata["Longitudine"].ToString()).FirstOrDefault();
                Location location = new Location(Convert.ToDouble(RowSelezionata["Latitudine"]), Convert.ToDouble(RowSelezionata["Longitudine"]));
                if (location == null) { await DisplayAlert("", "Non riesco a trovare l'attività su maps!", "OK"); return; }
                await Xamarin.Essentials.Map.OpenAsync(location);
            } catch (Exception) {
                await DisplayAlert("", "Non riesco a visualizzare l'indirizzo!", "OK");
            }
        }
        
    }



}
