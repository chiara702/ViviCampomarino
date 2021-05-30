using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViviCampomarino.Attivita;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageScopriCampomarinoAttivita : ContentPage {
        private int IdCategoriaPartenza;
        public PageScopriCampomarinoAttivita(int idCategoriaPartenza) {
            InitializeComponent();
            IdCategoriaPartenza = idCategoriaPartenza;
            GridCategorie.Children.Clear();
            Task.Run(() => CaricaCategorie(idCategoriaPartenza));

        }

        public void CaricaCategorie(int idCategoria) {
            DataTable table, tableAtt, tableHtml=null;
            try {
                var Db = new MySqlvc();
                table = Db.EseguiQuery("Select * From Categorie Where ParentId=" + idCategoria.ToString() + " Order By Ordine");
                tableAtt = Db.EseguiQuery("Select * From Aziende Where CategoriaId=" + idCategoria.ToString() + " Order By Denominazione");
                if (idCategoria == 0) {
                    tableHtml = Db.EseguiQuery("Select Id From VarieHtml Where CHAR_LENGTH(HTML)>10");
                }
                Db.CloseCommit();
            } catch (Exception) {
                Device.BeginInvokeOnMainThread(() => {
                    Act1.IsVisible = false;
                    DisplayAlert("Errore", "Connessione internet assente o operazione impossibile in questo momento!", "OK");
                });
                return;
            }
            if (table.Rows.Count == 0) { //se non sono presenti sottocategorie visualizza attivita
                Device.BeginInvokeOnMainThread(() => {
                    Act1.IsVisible = false;
                    int count = 0;
                    foreach (DataRow x in tableAtt.Rows) {
                        var view = new ViewBtnAttivita(Convert.ToInt32(x["Id"]));
                        view.Label = x["Denominazione"].ToString();
                        if (Convert.IsDBNull(x["Logo"]) == false) view.ImageLogoByte = (byte[])x["Logo"];
                        Grid.SetColumn(view, count % 2);
                        Grid.SetRow(view, count / 2);
                        GridCategorie.Children.Add(view);
                        count++;
                        var tmprow = x;
                        view.Cliccato += (s, e) => {
                            ViewBtnAttivita view = (ViewBtnAttivita)s;
                            var p = new PageAttivita(tmprow);
                            Navigation.PushAsync(p);
                        };
                    }
                    if (tableAtt.Rows.Count == 0) {
                        var Lbl = new Label();
                        Lbl.Text = "Nessuna attività in questa categoria!";
                        Lbl.TextColor = Color.FromHex("#a4182f");
                        Lbl.FontSize = 18;
                        Lbl.HorizontalTextAlignment = TextAlignment.Center;
                        Lbl.HorizontalOptions = LayoutOptions.FillAndExpand;
                        Grid.SetColumn(Lbl, 0);
                        Grid.SetRow(Lbl, 0);
                        Grid.SetColumnSpan(Lbl, 2);
                        GridCategorie.Children.Add(Lbl);
                    }
                });
            } else {
                Device.BeginInvokeOnMainThread(() => {
                    Act1.IsVisible = false;
                    int count = 0;
                    foreach (DataRow x in table.Rows) {
                        var view = new ViewBtnCategorie(Convert.ToInt32(x["Id"]));
                        view.Label = x["Categoria"].ToString();
                        if (Convert.IsDBNull(x["Image"]) == false) view.ImageByte = (byte[])x["Image"];
                        Grid.SetColumn(view, count % 2);
                        Grid.SetRow(view, count / 2);
                        GridCategorie.Children.Add(view);
                        count++;
                        view.Cliccato += (s, e) => {
                            ViewBtnCategorie view = (ViewBtnCategorie)s;
                            var p = new PageScopriCampomarinoAttivita(view.idCategoria);
                            Navigation.PushAsync(p);
                        };
                    }
                    if (idCategoria == 0) {
                        if (tableHtml.Select("Id=1").Length == 1) { //View Taxy
                            var view = new ViewBtnCategorie(0);
                            view.Label = "NUMERI TAXI";
                            view.SetImageSource("taxi.jpg");
                            Grid.SetColumn(view, count % 2);
                            Grid.SetRow(view, count / 2);
                            GridCategorie.Children.Add(view);
                            count++;
                            view.Cliccato += (s, e) => {
                                ViewBtnCategorie view = (ViewBtnCategorie)s;
                                var p = new PageSimpleHtml(1, "NUMERI TAXI");
                                Navigation.PushAsync(p);
                            };
                        }
                        if (tableHtml.Select("Id=2").Length == 1) { //View Taxy
                            var view = new ViewBtnCategorie(0);
                            view.Label = "INFO PULMAN";
                            view.SetImageSource("pulman.png");
                            Grid.SetColumn(view, count % 2);
                            Grid.SetRow(view, count / 2);
                            GridCategorie.Children.Add(view);
                            count++;
                            view.Cliccato += (s, e) => {
                                ViewBtnCategorie view = (ViewBtnCategorie)s;
                                var p = new PageSimpleHtml(2, "INFO PULMAN");
                                Navigation.PushAsync(p);
                            };
                        }
                        if (tableHtml.Select("Id=3").Length == 1) { //View Taxy
                            var view = new ViewBtnCategorie(0);
                            view.Label = "NUMERI UTILI";
                            view.SetImageSource("AttNumeriUtili.png");
                            Grid.SetColumn(view, count % 2);
                            Grid.SetRow(view, count / 2);
                            GridCategorie.Children.Add(view);
                            count++;
                            view.Cliccato += (s, e) => {
                                ViewBtnCategorie view = (ViewBtnCategorie)s;
                                var p = new PageSimpleHtml(3, "NUMERI UTILI");
                                Navigation.PushAsync(p);
                            };
                        }
                    }
                });
            }
        }
    private async void BtnIndietro_Clicked(object sender, EventArgs e) {
        await Navigation.PopAsync();
    }

    private void BtnDormire_Clicked(object sender, EventArgs e) {

    }

    private void BtnMangiare_Clicked(object sender, EventArgs e) {

    }

    private void BtnTecnico_Clicked(object sender, EventArgs e) {

    }

    private void BtnCamping_Clicked(object sender, EventArgs e) {

    }

    private void BtnShopping_Clicked(object sender, EventArgs e) {

    }

    private void BtnNumeriUtili_Clicked(object sender, EventArgs e) {

    }
}
}