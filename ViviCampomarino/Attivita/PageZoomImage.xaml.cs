using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.Attivita {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageZoomImage : ContentPage {
        double currentScale = 1;
        double startScale;
        public PageZoomImage(ImageSource imgSource) {
            InitializeComponent();
            Img1.Source=imgSource;
            //var pinchGestureRecognizer = new PinchGestureRecognizer();
            //pinchGestureRecognizer.PinchUpdated += OnPinchUpdated;
            //Img1.GestureRecognizers.Add(pinchGestureRecognizer);
            //Img1.MinimumWidthRequest=1800;

        }
        

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync();
        }
    }
}