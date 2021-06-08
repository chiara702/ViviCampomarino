using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageNotifiche : ContentPage {
        public PageNotifiche() {
            InitializeComponent();
            //if (App.login != null) Task.Run(LeggiNotifiche); else {
            //    LblNonDisponibile.IsVisible = true;
            //}
            Task.Run(LeggiNotifiche);

        }

        public void LeggiNotifiche() {
            var db = new MySqlvc();
            var TableNotifiche = db.EseguiQuery("Select * From NotificheGenerali");
            db.CloseCommit();
            Device.BeginInvokeOnMainThread(() => {
                StkNotifiche.Children.Clear();
                var NotificheNascoste = Preferences.Get("NotificheNascoste", "").Split(",", StringSplitOptions.RemoveEmptyEntries).ToList<String>();
                foreach (DataRow x in TableNotifiche.Rows) {
                    if (NotificheNascoste.Contains(x["Id"].ToString())) continue;
                    if (Convert.IsDBNull(x["DataEnd"])==false && Convert.ToDateTime(x["DataEnd"]) < DateTime.Now) continue;
                    if (Funzioni.Antinull(x["Token"]).Length > 100) {
                        if (App.login == null) continue;
                        if (Funzioni.Antinull(x["Token"]) != Funzioni.Antinull(App.login["TokenFcm"])) continue;

                    }
                    //if (x["Token"].ToString() != "" && x["Token"].ToString() != "Generale" && x["Token"].ToString() != App.login["TokenFcm"].ToString()) continue;
                    var el = new ViewNotifica();
                    el.IdNotifica = Convert.ToInt32(x["Id"]);
                    el.EventoEliminaNotifica += (s, e) =>
                    {
                        var NotificheNascoste = Preferences.Get("NotificheNascoste", "").Split(",",StringSplitOptions.RemoveEmptyEntries).ToList<String>();
                        NotificheNascoste.Add(el.IdNotifica.ToString());
                        Preferences.Set("NotificheNascoste", String.Join(",", NotificheNascoste));
                        StkNotifiche.Children.Remove(el);
                    };

                    el.Titolo = Funzioni.Antinull(x["Titolo"]);
                    el.Descrizione = Funzioni.Antinull(x["Descrizione"]);
                    //el.MinimumHeightRequest = 60;
                    StkNotifiche.Children.Add(el);
                }
                if (StkNotifiche.Children.Count==0) LblNonDisponibile.IsVisible = true;
            });

        }

        private void Button_Clicked(object sender, EventArgs e) {
            Application.Current.MainPage = new PageLoading();
        }
    }
}