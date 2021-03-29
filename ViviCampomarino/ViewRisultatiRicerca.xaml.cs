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
        public String Titolo {
            set { LblTitolo.Text = value; }
        }
        public String Autori {
            set { LblAutori.Text = value; }
        }
        public String Disponibile {
            set { LblDisponibile.Text = value; }
        }
        public String Image {
            set {  }
        }


        public ViewRisultatiRicerca() {
            InitializeComponent();
        }
    }
}