using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageNotifiche : ContentPage {
        public PageNotifiche() {
            InitializeComponent();
            //Leggi notifiche da db
            //var conn = SqlLiteDatabase.Connessione;
            //var ListaNotifiche = conn.Query<SqlLiteNotifiche>("Select * From Notifiche Order By Data desc");
            //StkNotifiche.Children.Clear();
            //foreach (var x in ListaNotifiche) {
            //    var el = new ViewNotifica();
            //    el.Descrizione = x.Descrizione;
            //    StkNotifiche.Children.Add(el);
            //}
            Task.Run(LeggiNotifiche);
            
        }

        public async void LeggiNotifiche() {
            var db = new Database<NotificheGenerali>();
            var coll=db.GetCollection("NotificheGenerali");
            var querySnap = await coll.GetDocumentsAsync<NotificheGenerali>(Plugin.Firebase.Firestore.Source.Default);
            StkNotifiche.Children.Clear();
            Device.BeginInvokeOnMainThread(() => { 
                foreach (var x in querySnap.Documents) {
                    var el = new ViewNotifica();
                    el.Descrizione = Funzioni.Antinull(x.Data.Titolo);
                    StkNotifiche.Children.Add(el);
                }
            });

        }
    }
}