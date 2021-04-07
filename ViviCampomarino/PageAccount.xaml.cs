using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAccount : ContentPage {
        public PageAccount() {
            InitializeComponent();
            MenuTop.MenuLaterale = MenuLaterale;
            Task.Run(CaricaLibri);
        }

        private async void ImgMenu_Tapped(object sender, EventArgs e) {
            MenuLaterale.IsVisible = true;
            await MenuLaterale.Mostra();
        }

        
        private async void CaricaLibri() {
            var db = new Database<Libro>();
            var coll = db.GetCollection("Libri");
            var LibriSnap = await coll.WhereEqualsTo("IdUtente", App.LoginUidAuth).GetDocumentsAsync<Libro>();
            Device.BeginInvokeOnMainThread(() => { 
                StkLibriPresi.Children.Clear();
                foreach (var x in LibriSnap.Documents) {
                    var el = new ViewAccountLibro();
                    el.Titolo = Funzioni.Antinull(x.Data.Titolo);
                    StkLibriPresi.Children.Add(el);
                }
                if (LibriSnap.Documents.Count() == 0) {
                    StkNoPrestito.IsVisible=true;
                } else {
                    StkNoPrestito.IsVisible = false;
                }
            });
        }

    }
}