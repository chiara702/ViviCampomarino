using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.EBike {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagePercorsoEBike : ContentPage {
        public PagePercorsoEBike() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            CaricaPin();
            var p = new Position(41.95582197035494, 15.03307138401569);
            var span = new MapSpan(p, 0.195, 0.195);
            map1.MoveToRegion(span);







        }
        DataTable TablePunti;
        DataRow RowSelezionata;
        public void CaricaPin(Boolean MostraGestore = false) {
            var Db = new MySqlvc();
            TablePunti = Db.EseguiQuery("Select * From Aziende Where CategoriaId=34");
            Db.CloseCommit();
            foreach (DataRow x in TablePunti.Rows) {
                var tmpRow = x;
                if (x["Coordinate"].ToString().Split(",").Count()!=2) continue;

                var pin = new Pin();
                pin.Position = new Position(Convert.ToDouble(x["Coordinate"].ToString().Split(",")[0].Replace(".", ",")), Convert.ToDouble(x["Coordinate"].ToString().Split(",")[1].Replace(".", ",")));
                pin.Label = x["Denominazione"].ToString();
                pin.MarkerClicked += (s, e) => {
                    Device.BeginInvokeOnMainThread(() => {
                        RowSelezionata = tmpRow;
                        FrmDettagli.FadeTo(0, 1);
                        FrmDettagli.IsVisible = true;
                        _ = FrmDettagli.FadeTo(1, 800);
                        LblDenominazione.Text = tmpRow["Denominazione"].ToString();
                        LblDescrizione.Text = tmpRow["Descrizione"].ToString();

                    });
                };
                map1.Pins.Add(pin);
            }

        }



        private void BtnDettagli_Clicked(object sender, EventArgs e) {
            
        }


        private void BtnInfo_Clicked(object sender, EventArgs e) {
            FrmDettagli.IsVisible = false;


        }



        private async void BtnNavigaVerso_Clicked(object sender, EventArgs e) {
            try {
                //Location location = (await Geocoding.GetLocationsAsync(RowSelezionata["Latitudine"].ToString() + "," + (RowSelezionata["Longitudine"].ToString()).FirstOrDefault();
                Location location = new Location(Convert.ToDouble(RowSelezionata["Coordinate"].ToString().Split(",")[0].Replace(".", ",")), Convert.ToDouble(RowSelezionata["Coordinate"].ToString().Split(",")[1].Replace(".", ",")));
                if (location == null) { await DisplayAlert("", "Non riesco a trovare l'attività su maps!", "OK"); return; }
                await Xamarin.Essentials.Map.OpenAsync(location);
            } catch (Exception) {
                await DisplayAlert("", "Non riesco a visualizzare l'indirizzo!", "OK");
            }
        }
    }
}