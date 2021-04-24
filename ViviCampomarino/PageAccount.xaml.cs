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
            LblEmail.Text = App.login.Email;
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
                    el.IdLibro = x.Reference.Id;
                    el.Titolo = Funzioni.Antinull(x.Data.Titolo);
                    el.Autori = Funzioni.Antinull(x.Data.Autori);
                    el.InteroLibro = x.Data;
                    StkLibriPresi.Children.Add(el);
                }
                if (LibriSnap.Documents.Count() == 0) {
                    StkNoPrestito.IsVisible=true;
                } else {
                    StkNoPrestito.IsVisible = false;
                }

                _ = Task.Run(() => {
                    foreach (ViewAccountLibro x in StkLibriPresi.Children)
                    {
                        try
                        {
                            var nomefile = x.IdLibro + ".png";
                            var rifs = FirebaseStorage.current.GetRootReference().GetChild("Libri/" + nomefile);
                            //var presente = false;
                            //foreach (var f in listaFile.Items) if (f.Name == nomefile) presente = true;
                            //if (presente == true) {
                            if (System.IO.File.Exists(System.IO.Path.GetTempPath() + nomefile) == true)
                            {
                                if (System.IO.File.GetCreationTime(System.IO.Path.GetTempPath() + nomefile) >= DateTime.Now.AddDays(-1))
                                {
                                    x.Image = ImageSource.FromFile(System.IO.Path.GetTempPath() + nomefile);
                                    continue;
                                }
                            }
                            var a = rifs.DownloadFile(System.IO.Path.GetTempPath() + nomefile);
                            a.AwaitAsync().Wait(3000);
                            if (System.IO.File.Exists(System.IO.Path.GetTempPath() + nomefile) == true)
                            {
                                x.Image = ImageSource.FromFile(System.IO.Path.GetTempPath() + nomefile);
                            }

                            //await a.AwaitAsync();
                            //}

                        }
                        catch (SystemException)
                        {
                        }
                        catch (Exception err)
                        {
                        }

                    }
                });
            });
            
        }

  

    }
}