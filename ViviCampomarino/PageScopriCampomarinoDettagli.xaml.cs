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
    public partial class PageScopriCampomarinoDettagli : ContentPage {
        private DataRow row;
        public PageScopriCampomarinoDettagli(DataRow row) {
            InitializeComponent();
            this.row = row;
            TxtTitolo.Text = row["Titolo"].ToString();
            Task.Run(ScaricaLinkVideo);
        }

        public async void ScaricaLinkVideo() {
            try {
                Device.BeginInvokeOnMainThread(() => {
                    Act1.IsVisible = true;
                });
            
                var child = FirebaseStorage.current.GetRootReference().GetChild("VideoTour/" + row["Id"].ToString() + ".mpeg4");
                var url = await child.GetDownloadUrlAsync();
                Video1.SetStopAction(() => Navigation.PopAsync());
            
                Video1.SetStartAction(() => {
                    Act1.IsVisible = false;
                });
                Device.BeginInvokeOnMainThread(() => {
                    Video1.Source = url;
                    Video1.Start();
                });
            } catch (Exception) {
                Device.BeginInvokeOnMainThread(() => {
                    Act1.IsVisible = false;
                });
            }


        }
        protected override void OnDisappearing(){
            Task.Run(async() => {await Video1.Pause(); });
            base.OnDisappearing();
        }
        
        protected override bool OnBackButtonPressed(){
            
            return base.OnBackButtonPressed();
        }
    }
}