using Plugin.Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageBibliotecaCerca : ContentPage {
        Task TaskCaricaDb;
        public PageBibliotecaCerca() {
            InitializeComponent();
            MenuTop.MenuLaterale = MenuLaterale;
           
        }
        //private IQuerySnapshot<Libro> ListaLibri = null;
        

        private async void ImgMenu_Tapped(object sender, EventArgs e) {
            MenuLaterale.IsVisible = true;
            await MenuLaterale.Mostra();
        }




        int LastRecord = 0;
        private void Cerca(int DaRecord) {
            LastRecord = DaRecord;
            var curStorage = FirebaseStorage.current.GetRootReference();
            Debug.WriteLine("Start BtnCerca");
            TxtCerca.Text = Funzioni.Antinull(TxtCerca.Text);

            if (TxtCerca.Text == null) {
                LblRicercaFallita.IsVisible = true;
                return;
            }
            Act1.IsVisible = true;
            var CercaSoloDisponibili = false;
            if (CheckSoloDisponibili.IsChecked == true) CercaSoloDisponibili = true;
            
            var Where = "";
            if (CheckAutori.IsChecked == false && CheckGenere.IsChecked == false && CheckTitoli.IsChecked == false) {
                CheckAutori.IsChecked = true; CheckGenere.IsChecked = true; CheckAutori.IsChecked = true;
            }
            if (CheckAutori.IsChecked == true) {
                Where += String.Format("Autori like '%{0}%' or ", Funzioni.AntiAp(TxtCerca.Text));
            }
            if (CheckGenere.IsChecked == true) {
                Where += String.Format("Generi like '%{0}%' or ", Funzioni.AntiAp(TxtCerca.Text));
            }
            if (CheckTitoli.IsChecked == true) {
                Where += String.Format("Titolo like '%{0}%' or ", Funzioni.AntiAp(TxtCerca.Text));
            }
            Where += "1=0";

            var Db = new MySqlvc();
            var Table = Db.EseguiQuery("Select * From Libri Where " + Where + " limit " + DaRecord +",100");
            Db.CloseCommit();

            StackView.Children.Clear();
            int LibriMostrati = 0;
            foreach (DataRow x in Table.Rows) {
                if (LibriMostrati > 50) break;
                LastRecord += 1;
                LibriMostrati++;
                var el = new ViewRisultatiRicerca(x);
                var Disp = FunzioniLibri.LibroDisponibile(x);
                switch (Disp) {
                    case FunzioniLibri._Disponibile.Disponibile:
                        el.Disponibile = "Disponibile";
                        break;
                    case FunzioniLibri._Disponibile.Prenotato:
                        el.Disponibile = "Non Disponibile";
                        if (CercaSoloDisponibili == true) continue;
                        break;
                    case FunzioniLibri._Disponibile.Prestato:
                        el.Disponibile = "Non Disponibile";
                        if (CercaSoloDisponibili == true) continue;
                        break;
                }
                
                StackView.Children.Add(el);
            }

            if (LibriMostrati == 0) {
                LblRicercaFallita.IsVisible = true;
            } else LblRicercaFallita.IsVisible = false;
            FrameRicerca.IsVisible = true;
            StkCerca.IsVisible = false;
            Act1.IsVisible = false;
            _ = Task.Run(() => {
                foreach (ViewRisultatiRicerca x in StackView.Children) {
                    try {
                        var nomefile = x.rowLibro["Id"].ToString() + ".png";
                        var rifs = curStorage.GetChild("Libri/" + nomefile);
                        //var presente = false;
                        //foreach (var f in listaFile.Items) if (f.Name == nomefile) presente = true;
                        //if (presente == true) {
                        if (System.IO.File.Exists(System.IO.Path.GetTempPath() + nomefile) == true) {
                            if (System.IO.File.GetCreationTime(System.IO.Path.GetTempPath() + nomefile) >= DateTime.Now.AddDays(-1)) {
                                x.Image = ImageSource.FromFile(System.IO.Path.GetTempPath() + nomefile);
                                continue;
                            }
                        }
                        var a = rifs.DownloadFile(System.IO.Path.GetTempPath() + nomefile);
                        a.AwaitAsync().Wait(3000);
                        if (System.IO.File.Exists(System.IO.Path.GetTempPath() + nomefile) == true) {
                            x.Image = ImageSource.FromFile(System.IO.Path.GetTempPath() + nomefile);
                        }

                        //await a.AwaitAsync();
                        //}

                    } catch (SystemException) {
                    } catch (Exception err) {
                        var c = 0;
                    }

                }
            });

        }

        private void BtnCerca_Clicked(object sender, EventArgs e) {
            try {
                Cerca(0); //Cerca(LastRecord);
            } catch (Exception) {
                DisplayAlert("Errore", "Ricerca non riuscita! Verifica connessione internet.","OK");
            }

        }



        private void BtnNuovaRicerca_Clicked(object sender, EventArgs e) {
            StackView.Children.Clear();
            FrameRicerca.IsVisible = false;
            StkCerca.IsVisible = true;
            TxtCerca.Text = "";
        }

        private void BtnMostraAltri_Clicked(object sender, EventArgs e) {
            try {
                Cerca(LastRecord);
                ScrollLibri.ScrollToAsync(0, 0, true);
            } catch (Exception) {
                DisplayAlert("Errore", "Ricerca non riuscita! Verifica connessione internet.", "OK");
            }
        }
    }
}