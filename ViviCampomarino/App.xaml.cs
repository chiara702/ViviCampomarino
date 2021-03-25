using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    public partial class App : Application {

        public App() {
            InitializeComponent();

            var home = new PageHome();
            Application.Current.MainPage = new NavigationPage(home);
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
    }
}
