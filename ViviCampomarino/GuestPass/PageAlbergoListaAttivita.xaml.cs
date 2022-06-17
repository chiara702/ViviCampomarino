using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.GuestPass
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageAlbergoListaAttivita : ContentPage
	{
		public PageAlbergoListaAttivita (){
			InitializeComponent ();
            Task.Run(() => {
                try {
                    CaricaStruttureRistorative();
                } catch (Exception e) {
                    DisplayAlert("", e.Message, "OK");
                }
            });
        }

        public void CaricaStruttureRistorative() {
            var db = new MySqlvc();
            var table = db.EseguiQuery("Select Id,Denominazione,Descrizione,Indirizzo,Paese,Telefono,Cellulare1,PercentualeSconto From Aziende Where FlagStrutturaRistorativa=1");
            db.CloseCommit();
            Device.BeginInvokeOnMainThread(() => {
                if (table.Rows.Count==0) LblNoAttivita.IsVisible=true;
                foreach (DataRow x in table.Rows) {
                    var tmp = new ViewListaAttivita(x);
                    tmp.Click+=(s, e) => {
                        //Niente
                    };
                    StkListaAttivita.Children.Add(tmp);
                }
            });
        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
			Navigation.PopAsync();
        }
    }
}