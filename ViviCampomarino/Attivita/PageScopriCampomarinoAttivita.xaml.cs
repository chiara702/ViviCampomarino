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
    public partial class PageScopriCampomarinoAttivita : ContentPage {
        private int IdCategoriaPartenza;
        public PageScopriCampomarinoAttivita(int idCategoriaPartenza) {
            InitializeComponent();
            IdCategoriaPartenza = idCategoriaPartenza;
            CaricaCategorie(idCategoriaPartenza);

        }
        
        public async void CaricaCategorie(int idCategoria) {
            GridCategorie.Children.Clear();
            var Db = new MySqlvc();
            var table=await Task.Run(()=>Db.EseguiQuery("Select * From Categorie Where ParentId=" + idCategoria.ToString() + " Order By Ordine"));
            Db.CloseCommit();
            if (table.Rows.Count == 0) { //se non sono presenti sottocategorie visualizza attivita
                Db = new MySqlvc();
                var tableAtt = await Task.Run(() =>Db.EseguiQuery("Select * From Aziende Where CategoriaId=" + idCategoria.ToString() + " Order By Denominazione"));
                Db.CloseCommit();
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
                    Lbl.FontSize = 16;
                    Lbl.HorizontalTextAlignment = TextAlignment.Center;
                    Lbl.HorizontalOptions = LayoutOptions.FillAndExpand;
                    Grid.SetColumn(Lbl, 0);
                    Grid.SetRow(Lbl, 0);
                    Grid.SetColumnSpan(Lbl, 2);
                    GridCategorie.Children.Add(Lbl);
                }
            } else {
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