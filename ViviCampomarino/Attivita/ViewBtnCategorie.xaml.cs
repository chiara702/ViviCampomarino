using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewBtnCategorie : ContentView {
        public event EventHandler Cliccato;
        public int idCategoria;

        public ViewBtnCategorie(int id) {
            InitializeComponent();
            idCategoria = id;
            BtnImg.Source = "ImgLogoAttivita.jpg";
        }

        public byte[] ImageByte {
            set {
                BtnImg.Source = ImageSource.FromStream(()=>new System.IO.MemoryStream(value));
            }
        }
        public void SetImageSource(String Source) {
            BtnImg.Source = Source;
        }
        public String Label {
            set {
                BtnLabel.Text = value;
            }
            get {
                return BtnLabel.Text;
            }
        }


        private void TapBtn_Tapped(object sender, EventArgs e) {
            Cliccato.Invoke(this, null);
        }


    }
}