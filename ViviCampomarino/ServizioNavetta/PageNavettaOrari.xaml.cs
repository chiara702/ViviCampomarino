using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.ServizioNavetta{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageNavettaOrari : ContentPage{
		public PageNavettaOrari (){
			InitializeComponent ();
            var Db = new MySqlvc();
			var tableFermate = Db.EseguiQuery("Select * From NavettaFermate Order By OrariValidi,Valore");
			var ListaOrariValidi = new List<string>();
			foreach (DataRow x in tableFermate.Rows) {
				foreach (String y in Funzioni.Antinull(x["OrariValidi"]).Split(",",StringSplitOptions.RemoveEmptyEntries)) {
					ListaOrariValidi.Add(y);
				}
			}
			ListaOrariValidi=ListaOrariValidi.Distinct().ToList();
			foreach (string x in ListaOrariValidi) {
				Lbl1.Text+=$"<b>Corsa delle: {x} <br>";
				foreach (DataRow y in tableFermate.Select($"OrariValidi like '%{x}%'")) {
					Lbl1.Text+=$"{y["FermataDescrizioneLunga"]} {y["OrarioFermata"]} <br>";
                }
				Lbl1.Text+=$"<br><br>";

            }
        }

        private async void BtnIndietro_Clicked(object sender, EventArgs e) {
			await Navigation.PopAsync();
        }
    }
}