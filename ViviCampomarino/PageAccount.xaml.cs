using System;
using System.Collections.Generic;
using System.Data;
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
            LblEmail.Text = App.login["Email"].ToString();
        }

        private async void ImgMenu_Tapped(object sender, EventArgs e) {
            MenuLaterale.IsVisible = true;
            await MenuLaterale.Mostra();
        }

        
        private void CaricaLibri() {
            var rowLibri = FunzioniLibri.ListaRowLibriPrenotatiPrestati(App.LoginUidAuth);

            Device.BeginInvokeOnMainThread(() => { 
                StkLibriPresi.Children.Clear();
                foreach (var x in rowLibri) {
                    var el = new ViewAccountLibro(x);
                    StkLibriPresi.Children.Add(el);
                }
                if (rowLibri.Count() == 0) {
                    StkNoPrestito.IsVisible=true;
                } else {
                    StkNoPrestito.IsVisible = false;
                }

                _ = Task.Run(() => {
                    foreach (ViewAccountLibro x in StkLibriPresi.Children)
                    {
                        try
                        {
                            var nomefile = x.rowLibro["Id"].ToString() + ".png";
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