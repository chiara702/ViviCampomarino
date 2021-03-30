using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageBibliotecaCerca : ContentPage {
        public PageBibliotecaCerca() {
            InitializeComponent();
            MenuTop.MenuLaterale = MenuLaterale;
        }

        private async void ImgMenu_Tapped(object sender, EventArgs e) {
            MenuLaterale.IsVisible = true;
            await MenuLaterale.Mostra();
        }

        private async void BtnCerca_Clicked(object sender, EventArgs e) {
            if (TxtCerca.Text == null) {
                return;
            }
            var db = new Database<Libro>();
            var coll = db.GetCollection("/Libri/");
            var query = coll.WhereGreaterThanOrEqualsTo("Titolo", TxtCerca.Text).WhereLessThanOrEqualsTo("Titolo", TxtCerca.Text + "\uf8ff");
            var ListaLibri = await query.GetDocumentsAsync<Libro>();
            StackView.Children.Clear();
            foreach (var x in ListaLibri.Documents) {
                var el = new ViewRisultatiRicerca();
                StackView.Children.Add(el);
                el.Titolo = "" + x.Data.Titolo;
                el.Autori = "" + x.Data.Autori;
                el.Disponibile = "Bo";
            }
            if (ListaLibri == null) {
                LblRicercaFallita.IsVisible = true;
            } else LblRicercaFallita.IsVisible = false;
            
            
            FrameRicerca.IsVisible = true;
            StkCerca.IsVisible = false;
            BtnNuovaRicerca.IsVisible = true;
        }

        private void BtnNuovaRicerca_Clicked(object sender, EventArgs e) {
            Navigation.PushAsync(new PageBibliotecaCerca());
        }
    }
}