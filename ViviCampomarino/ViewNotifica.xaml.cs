using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewNotifica : Grid {
        public ViewNotifica() {
            InitializeComponent();
        }
        public int IdNotifica{
            get;
            set;
        }
        
        public String Titolo {
            set { LblTitolo.Text = value; }
        }
        public String Descrizione {
            set {
                LblDescrizione.Text = Funzioni.Antinull(value);
                if (LblDescrizione.Text == "") LblDescrizione.IsVisible = false;
            }
        } 


        public event EventHandler EventoEliminaNotifica;

        private void ImageButton_Clicked(object sender, EventArgs e) {
            EventoEliminaNotifica.Invoke(this, e);
        }
    }
}