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
        
        public String Descrizione {
            set { LblDescrizione.Text = value; }
        }
        public event EventHandler EventoEliminaNotifica;

        private void ImageButton_Clicked(object sender, EventArgs e) {
            EventoEliminaNotifica.Invoke(this, e);
        }
    }
}