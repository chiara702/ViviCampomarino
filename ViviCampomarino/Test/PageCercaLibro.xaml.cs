using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.Test {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCercaLibro : ContentPage {
        public PageCercaLibro() {
            InitializeComponent();
        }
        private List<ViewRisultatiRicerca> ListaView = new List<ViewRisultatiRicerca>();
        private async void BtnCerca_Clicked(object sender, EventArgs e) {
            TxtCerca.Text = "" + TxtCerca.Text;
            var db = new Database<Libro>();
            var coll=db.GetCollection("/Libri/");
            var query=coll.WhereGreaterThanOrEqualsTo("Titolo", TxtCerca.Text).WhereLessThanOrEqualsTo("Titolo",TxtCerca.Text + "\uf8ff").LimitedTo(20);
            var ListaLibri=await query.GetDocumentsAsync<Libro>();
            StackView.Children.Clear();
            foreach (var x in ListaLibri.Documents) {
                var el = new ViewRisultatiRicerca();
                el.Titolo = "" + x.Data.Titolo;
                el.Autori = "" + x.Data.Autori;
                try {
                    
                    //Device.BeginInvokeOnMainThread(()=> {
                        try {
                        //el.Image = ImageSource.FromUri(new Uri(await FirebaseStorage.DownloadUrlFromStorage("Libri/" + x.Data.ISBN + ".png")));
                        el.Image = ImageSource.FromStream(()=> {
                            try {
                                var t = FirebaseStorage.DownloadStreamFromStorage("Libri/" + x.Data.ISBN + ".png");
                                t.Wait(1000);
                                var byt=t.Result.ReadByte();
                                return t.Result;
                            }catch(Exception e) {
                                Console.WriteLine("---" + e.Message);
                                return null;
                            }
                            });
                    } catch (Exception) { }
                    //});

                } catch (Exception) { }
                
                el.Disponibile = "Bo";
                //await Task.Delay(100);
                StackView.Children.Add(el);
            }

        }
    }
}