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
    public partial class PageAttivita : ContentPage {
        private DataRow rowAttivita;
        public PageAttivita(DataRow RowAttivita) {
            InitializeComponent();
            rowAttivita = RowAttivita;
            CaricaImage(ImgLogoAttivita, RowAttivita["Logo"]);
            LblNomeAttivita.Text = Funzioni.Antinull(rowAttivita["Denominazione"]);
            LblPaese.Text = Funzioni.Antinull(rowAttivita["Paese"]);
            LblIndirizzo.Text = Funzioni.Antinull(rowAttivita["Indirizzo"]);
            LblEmail.Text = Funzioni.Antinull(rowAttivita["Email"]);
            LblWebSite.Text = Funzioni.Antinull(rowAttivita["SitoWeb"]);
            LblAppStore.Text = Funzioni.Antinull(rowAttivita["Store"]);
            LblTelefono.Text = Funzioni.Antinull(rowAttivita["Telefono"]);
            CaricaImage(Img1, RowAttivita["Image1"]);
            CaricaImage(Img2, RowAttivita["Image2"]);
            CaricaImage(Img3, RowAttivita["Image3"]);
            CaricaImage(Img4, RowAttivita["Image4"]);

        }
        private void CaricaImage(Image Pic, Object data) {
            if (Convert.IsDBNull(data) == false) Pic.Source = ImageSource.FromStream(() => new System.IO.MemoryStream((byte[])data));
        }

        private void TapTelefono_Tapped(object sender, EventArgs e) {
            Xamarin.Essentials.PhoneDialer.Open(LblTelefono.Text);
        }
    }
}