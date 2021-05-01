using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageEventiHome : ContentPage {
        public PageEventiHome() {
            InitializeComponent();
            MenuTop.MenuLaterale = MenuLaterale;
        }

        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            MenuLaterale.IsVisible = true;
            await MenuLaterale.Mostra();
        }

        private async void BtnDownload_Clicked(object sender, EventArgs e) {
            try {
                var rifs = FirebaseStorage.current.GetRootReference().GetChild("Eventi/Evento.pdf");
                System.IO.File.Delete(System.IO.Path.GetTempPath() + "Evento.pdf");
                var a = rifs.DownloadFile(System.IO.Path.GetTempPath() + "Evento.pdf");
                await a.AwaitAsync();
                if (System.IO.File.Exists(System.IO.Path.GetTempPath() + "Evento.pdf") == false) {
                    await DisplayAlert("Errore", "Non disponibile!", "OK");
                    return;
                }
                var OpenFile = new Xamarin.Essentials.OpenFileRequest("", new Xamarin.Essentials.ReadOnlyFile(System.IO.Path.GetTempPath() + "Evento.pdf"));
                await Xamarin.Essentials.Launcher.OpenAsync(OpenFile);

            } catch(Exception) {
                await DisplayAlert("Errore", "File non disponibile!", "OK");
            }

        }
    }
}