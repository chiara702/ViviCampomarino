﻿using System;
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
            TxtCerca.Text = Funzioni.Antinull(TxtCerca.Text);
            if (TxtCerca.Text == null) {
                LblRicercaFallita.IsVisible = true;
                return;
            }
            var db = new Database<Libro>();
            var coll = db.GetCollection("/Libri/");
            var query = coll.WhereGreaterThanOrEqualsTo("Titolo", TxtCerca.Text).WhereLessThanOrEqualsTo("Titolo", TxtCerca.Text + "\uf8ff").LimitedTo(20);
            var ListaLibri = await query.GetDocumentsAsync<Libro>();
            var curStorage = Plugin.Firebase.Storage.CrossFirebaseStorage.Current.GetRootReference();
            var listaFile = await curStorage.GetChild("Libri").ListAllAsync();
            StackView.Children.Clear();
            foreach (var x in ListaLibri.Documents) {
                var el = new ViewRisultatiRicerca();
                el.IdLibro = x.Reference.Id;
                el.Titolo = "" + x.Data.Titolo;
                el.Autori = "" + x.Data.Autori;
                el.IdLibro = x.Reference.Id;
                switch (x.Data.LibroDisponibile()) {
                    case Libro._Disponibile.Disponibile:
                        el.Disponibile = "Disponibile";
                        break;
                    case Libro._Disponibile.Prenotato:
                        el.Disponibile = "Non Disponibile";
                        break;
                    case Libro._Disponibile.Prestato:
                        el.Disponibile = "Non Disponibile";
                        break;
                }
                StackView.Children.Add(el);
            }
            foreach (ViewRisultatiRicerca x in StackView.Children) {
                try {
                    var nomefile = x.IdLibro + ".png";
                    var rifs = curStorage.GetChild("Libri/" + nomefile);
                    var presente = false;
                    foreach (var f in listaFile.Items) if (f.Name == nomefile) presente = true;
                    if (presente == true) {
                        var a = rifs.DownloadFile(System.IO.Path.GetTempPath() + nomefile);
                    }
                    if (System.IO.File.Exists(System.IO.Path.GetTempPath() + nomefile) == true) x.Image = ImageSource.FromFile(System.IO.Path.GetTempPath() + nomefile);
                } catch (SystemException) {
                } catch (Exception err) {
                    await DisplayAlert("", err.Message, "OK");
                }

            }

            if (ListaLibri.Count == 0) {
                LblRicercaFallita.IsVisible = true;
            } else LblRicercaFallita.IsVisible = false;


            StackView.IsVisible = true;
            FrameRicerca.IsVisible = true;
            StkCerca.IsVisible = false;
            BtnNuovaRicerca.IsVisible = true;

           
        }

        private void BtnNuovaRicerca_Clicked(object sender, EventArgs e) {
            Navigation.PushAsync(new PageBibliotecaCerca());
            StackView.IsVisible = false;
        }

        
    }
}