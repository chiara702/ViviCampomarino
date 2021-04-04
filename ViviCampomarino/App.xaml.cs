using Plugin.Firebase.CloudMessaging;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    public partial class App : Application {

        public static String LoginUidAuth = "";
        public static Login login=null;
        public App() {
            InitializeComponent();
            LeggiImpostazioni();
            var home = new PageHome();
            Application.Current.MainPage = new NavigationPage(home);
            CrossFirebaseCloudMessaging.Current.NotificationReceived += Current_NotificationReceived;
            CrossFirebaseCloudMessaging.Current.NotificationTapped += Current_NotificationTapped;
            CrossFirebaseCloudMessaging.Current.Error += Current_Error;
            CrossFirebaseCloudMessaging.Current.TokenChanged += Current_TokenChanged;
            //CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync("generale");
            
        }
        public static void SalvaImpostazioni() {
            Preferences.Set("LoginUidAuth", LoginUidAuth);
        }
        public static void LeggiImpostazioni() {
            Preferences.Get("LoginUidAuth", "");
        }

        private void Current_TokenChanged(object sender, Plugin.Firebase.CloudMessaging.EventArgs.FCMTokenChangedEventArgs e) {
            CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync("generale");
        }

        private void Current_Error(object sender, Plugin.Firebase.CloudMessaging.EventArgs.FCMErrorEventArgs e) {
            throw new NotImplementedException();
        }

        private void Current_NotificationTapped(object sender, Plugin.Firebase.CloudMessaging.EventArgs.FCMNotificationTappedEventArgs e) {
            Device.BeginInvokeOnMainThread(() => {
                Application.Current.MainPage.DisplayAlert("Tapped", e.Notification.Body, "OK");
            });
            //throw new NotImplementedException();
        }

        private void Current_NotificationReceived(object sender, Plugin.Firebase.CloudMessaging.EventArgs.FCMNotificationReceivedEventArgs e) {
            Device.BeginInvokeOnMainThread(() => {
                Application.Current.MainPage.DisplayAlert("Received", e.Notification.Body, "OK");
            });
            //throw new NotImplementedException();
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
        
    }
}
