using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAccountLibro : Grid {
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
      
        public ImageSource Image {
            set { ImgLibro.Source = value; }
        }
        public String ISBN {
            get;
            set;
        }


        public ViewAccountLibro() {
            InitializeComponent();

            
        }

        

        private void BtnDettaglio_Clicked(object sender, EventArgs e) {
            
        }
    }
}
