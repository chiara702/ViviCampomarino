﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Data;
using System.Diagnostics;

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

        }
        DataTable TablePunti;
        public void CaricaPin(Boolean MostraGestore = false) {
            var Db = new MySqlvc();
            TablePunti = Db.EseguiQuery("Select * From PuntiInteresse");
            foreach (DataRow x in TablePunti.Rows) {
                var tmpRow = x;
                var pin = new Pin();
                pin.Position = new Position(Convert.ToDouble(x["Latitudine"]), Convert.ToDouble(x["Longitudine"]));
                pin.Label = x["Nome"].ToString();
                pin.MarkerClicked += (s,e) => {
                    Device.BeginInvokeOnMainThread(() => {
                        FrmInfo.IsVisible = true;
                        LblInfoTitolo.Text = tmpRow["Titolo"].ToString();
                        LblInfoDescrizione.Text = tmpRow["Descrizione"].ToString();
                    });
                };
                map1.Pins.Add(pin);
            }

        }

        private void BtnScanPoint_Clicked(object sender, EventArgs e) {

        }

        private void BtnDettagli_Clicked(object sender, EventArgs e) {

        }


        private async void BtnInfo_Clicked(object sender, EventArgs e) {
            FrmDettagli.IsVisible = false;
            

        }

        private async void TapInfo_Tapped(object sender, EventArgs e) {
            if (Debugger.IsAttached == false) {
                await DisplayAlert("Presto disponibile!", "Funzione presto disponibile", "OK");
                await Navigation.PopAsync();
                return;
            }

            _ = await FrmInfo.FadeTo(0, 500);
            FrmInfo.IsVisible = false;
            _ = await FrmDettagli.FadeTo(0, 1);
            FrmDettagli.IsVisible = true;
            _ = FrmDettagli.FadeTo(1, 800);

        }
    }



}
