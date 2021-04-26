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
            
            Task.Run(LeggiNotifiche);
            
        }

        public async void LeggiNotifiche() {
            var db = new MySqlvc();
            var TableNotifiche = db.EseguiQuery("Select * From NotificheGenerali");
            StkNotifiche.Children.Clear();
            Device.BeginInvokeOnMainThread(() => {
                var NotificheNascoste = Preferences.Get("NotificheNascoste", "").Split(",", StringSplitOptions.RemoveEmptyEntries).ToList<String>();
                foreach (DataRow x in TableNotifiche.Rows) {
                    if (NotificheNascoste.Contains(x["Id"].ToString())) continue;
                    if (Convert.IsDBNull(x["DataEnd"])==false && Convert.ToDateTime(x["DataEnd"]) < DateTime.Now) continue;
                    if (x["Token"].ToString() != "" && x["Token"].ToString() != App.login["TokenFcm"].ToString()) continue;
                    var el = new ViewNotifica();
                    el.IdNotifica = Convert.ToInt32(x["Id"]);
                    el.EventoEliminaNotifica += (s, e) =>
                    {
                        var NotificheNascoste = Preferences.Get("NotificheNascoste", "").Split(",",StringSplitOptions.RemoveEmptyEntries).ToList<String>();
                        NotificheNascoste.Add(el.IdNotifica.ToString());
                        Preferences.Set("NotificheNascoste", String.Join(",", NotificheNascoste));
                        StkNotifiche.Children.Remove(el);
                    };

                    el.Descrizione = Funzioni.Antinull(x["Titolo"]);
                    StkNotifiche.Children.Add(el);
                }
            });

        }
    }
}