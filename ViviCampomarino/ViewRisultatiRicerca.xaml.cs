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
    public partial class ViewRisultatiRicerca : Grid {
        public DataRow rowLibro;
        public ViewRisultatiRicerca(DataRow rowLibro) {
            InitializeComponent();
            this.rowLibro = rowLibro;
            LblTitolo.Text = rowLibro["Titolo"].ToString();
            LblAutori.Text = rowLibro["Autori"].ToString();
            
        }
        
        public String Disponibile {
            set { 
                LblDisponibile.Text = value;
                if (LblDisponibile.Text != "Disponibile") LblDisponibile.TextColor = Color.Red;
            }
        }
        public ImageSource Image {
            set { Device.BeginInvokeOnMainThread(() => ImgLibro.Source = value); }
        }
        


        

        private async void BtnPrenota_Clicked(object sender, EventArgs e) {
             await App.Current.MainPage.Navigation.PushAsync(new PageDettaglioLibro(rowLibro));
        }
    }
}