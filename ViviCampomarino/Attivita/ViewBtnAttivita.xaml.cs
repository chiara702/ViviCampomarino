using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewBtnAttivita : ContentView {
        public event EventHandler Cliccato;
        public int idCategoria;
        public ViewBtnAttivita(int id) {
            InitializeComponent();
            idCategoria = id;
            BtnImg.Source = "ImgLogoAttivita.jpg";
        }
        

        public byte[] ImageLogoByte {
            set {
                BtnImg.Source = ImageSource.FromStream(() => new System.IO.MemoryStream(value));
            }
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
            //await App.Current.MainPage.Navigation.PushAsync(new ListViewPageSottocategorieAttivita());
            Cliccato.Invoke(this, null);
        }

    }
}
