using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRisultatiRicerca : Grid {
        public String IdLibro {
            get;
            set;
        }
        public String Titolo {
            set { LblTitolo.Text = value; }
        }
        public String Autori {
            set { LblAutori.Text = value; }
        }
        public String Disponibile {
            set { LblDisponibile.Text = value; }
        }
        public ImageSource Image {
            set { ImgLibro.Source = value; }
        }
        public String ISBN {
            get;
            set;
        }


        public ViewRisultatiRicerca() {
            InitializeComponent();
        }

        private void BtnPrenota_Clicked(object sender, EventArgs e) {
             Navigation.PushAsync(new PageDettaglioLibro(IdLibro));
        }
    }
}