using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Firebase.Firestore;


namespace ViviCampomarino.Test {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageLibro : ContentPage {
        private int IdLibro;
        public PageLibro(int IdLibro) {
            InitializeComponent();
            this.IdLibro = IdLibro;
            //if (IdLibro > 0) CaricaLibro();
            CaricaLibro();
        }
        public async void CaricaLibro() {
            var id = 6;
            Database<Libro> database = new Database<Libro>();
            var libro = await database.ReadDocument("/Libri/" + id);
            if (libro == null) return;
            TxtTitolo.Text = libro.Titolo;
            TxtAutori.Text = libro.Autori;
            TxtEditore.Text = libro.Editore;
            

        }
        public void SalvaLibro() {
            var id = 6;
            Database<Libro> database = new Database<Libro>();
            var libro = new Libro();
            libro.Titolo = "" + TxtTitolo.Text;
            libro.Autori = "" + TxtAutori.Text;
            libro.Editore = "" + TxtEditore.Text;
            database.WriteDocument("/Libri/" + id, libro);

        }

        private void BtnSalva_Clicked(object sender, EventArgs e) {
            SalvaLibro();

        }

        private void BtnAnnulla_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync();
        }
    }
    


    

}