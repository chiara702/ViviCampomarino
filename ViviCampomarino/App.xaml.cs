using Plugin.Firebase.CloudMessaging;
//using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    public partial class App : Application {

        public static String LoginUidAuth = "";
        public static DataRow login=null;
        public static Boolean InterrompiLoading = false;
        public static DataRow RowAzienda=null;

        public App(Boolean IntentExtras) {
            InitializeComponent();
            if (IntentExtras == false) {
                var home = new PageLoading();
                MainPage = home;
            } else {
                MainPage = new PageNotifiche();
            }
        }
        public App() {
            InitializeComponent();
            InterrompiLoading = false;
            //IstanzaSqlLite = new SqlLiteDatabase();
            
            CrossFirebaseCloudMessaging.Current.NotificationReceived += Current_NotificationReceived;
            CrossFirebaseCloudMessaging.Current.NotificationTapped += Current_NotificationTapped;
            
            CrossFirebaseCloudMessaging.Current.Error += Current_Error;
            CrossFirebaseCloudMessaging.Current.TokenChanged += Current_TokenChanged;
            
            //Da Togliere
            //CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync("debug");
            
            LeggiImpostazioni();
            var home = new PageLoading();
            MainPage = home;
            


        }
        public static void SalvaImpostazioni() {
            Preferences.Set("LoginUidAuth", LoginUidAuth);
        }
        public static void LeggiImpostazioni() {
            LoginUidAuth=Preferences.Get("LoginUidAuth", "");
            if (LoginUidAuth != "") {
                try {
                    var Db = new MySqlvc();
                    //login=Task.Run(()=> Db.EseguiRow("Select * From Login Where UidAuth='" + LoginUidAuth + "'")).GetAwaiter().GetResult();
                    login = Db.EseguiRow("Select * From Login Where UidAuth='" + LoginUidAuth + "'");
                    if (login!= null && Convert.ToInt32(login["IdAzienda"])>0) {
                        RowAzienda=Db.EseguiRow($"Select * From Aziende Where Id={Convert.ToInt32(login["IdAzienda"])}");
                    }
                    Db.CloseCommit();
                }catch(Exception) {
                    Console.WriteLine("errore Login");
                }
            }
        }

        public static void FcmTopicsRefresh() {
            CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync("generale");
        }

        private void Current_TokenChanged(object sender, Plugin.Firebase.CloudMessaging.EventArgs.FCMTokenChangedEventArgs e) {
            CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync("generale");
        }

        private void Current_Error(object sender, Plugin.Firebase.CloudMessaging.EventArgs.FCMErrorEventArgs e) {
            throw new NotImplementedException();
        }

        private void Current_NotificationTapped(object sender, Plugin.Firebase.CloudMessaging.EventArgs.FCMNotificationTappedEventArgs e) {
            App.InterrompiLoading = true;
            Device.BeginInvokeOnMainThread(() => {
                //Application.Current.MainPage.DisplayAlert("Tapped", e.Notification.Body, "OK");
                App.Current.MainPage.Navigation.PushAsync(new PageNotifiche());
                //App.Current.MainPage = new PageNotifiche();
            });
            //throw new NotImplementedException();
        }
        
        private void Current_NotificationReceived(object sender, Plugin.Firebase.CloudMessaging.EventArgs.FCMNotificationReceivedEventArgs e) {
            App.InterrompiLoading = true;
            if (e.Notification.Data.ContainsKey("NotificaDisponibilita")) {
                Plugin.Firebase.CloudMessaging.CrossFirebaseCloudMessaging.Current.UnsubscribeFromTopicAsync(e.Notification.Data["Notifica"]);
            }
            

            Device.BeginInvokeOnMainThread(() => {
                //Application.Current.MainPage.DisplayAlert(e.Notification.Title, e.Notification.Body, "OK");
                try {
                    Application.Current.MainPage.Navigation.PushAsync(new PageNotifiche());
                } catch (Exception) { }
            });
            //throw new NotImplementedException();
        }

        protected override void OnStart() {
            //var home = new PageLoading();
            //MainPage = home;
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
            var x = 0;
        }
        
    }
}
