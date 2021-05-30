using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.Attivita {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSimpleHtml : ContentPage {
        private int Id;
        private String Titolo;
        private DataRow row;
        public PageSimpleHtml(int id, String titolo) {
            InitializeComponent();
            Id = id;
            Titolo = titolo;
            Task.Run(CaricaDati);
        }
        private void CaricaDati() {
            var Db = new MySqlvc();
            row = Db.EseguiRow($"Select * From VarieHtml Where Id={Id}");
            Db.CloseCommit();
            Device.BeginInvokeOnMainThread(() => {
                try {
                    LblTitolo.Text = Titolo;
                    var html = new HtmlWebViewSource() { Html = $"<html><body>{row["HTML"].ToString()}</body></html>" };
                    WebView1.Source = html;
                }catch(Exception e) {
                    DisplayAlert("", e.Message, "OK");
                }
            });
        }
    }
}