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

        

        

        

        private async void BtnCerca_Clicked(object sender, EventArgs e) {
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
            var Db = new MySqlvc();
            var Table = await Task.Run(()=> Db.EseguiQuery(String.Format("Select * From Libri Where Titolo like '%{0}%' or Autori like '%{0}%' or Generi like '{0}%' limit 30", Funzioni.AntiAp(TxtCerca.Text))));
            Db.CloseCommit();
          
            StackView.Children.Clear();
            int LibriMostrati = 0;
            foreach (DataRow x in Table.Rows) {
                if (LibriMostrati > 15) break;
                LibriMostrati++;
                var el = new ViewRisultatiRicerca(x);
                var Disp = FunzioniLibri.LibroDisponibile(x);
                switch (Disp) {
                    case FunzioniLibri._Disponibile.Disponibile:
                        el.Disponibile = "Disponibile";
                        break;
                    case FunzioniLibri._Disponibile.Prenotato:
                        el.Disponibile = "Non Disponibile";
                        break;
                    case FunzioniLibri._Disponibile.Prestato:
                        el.Disponibile = "Non Disponibile";
                        break;
                }
                if (CercaSoloDisponibili == true) continue;
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



        private void BtnNuovaRicerca_Clicked(object sender, EventArgs e) {
            StackView.Children.Clear();
            FrameRicerca.IsVisible = false;
            StkCerca.IsVisible = true;
            TxtCerca.Text = "";
        }

        
    }
}