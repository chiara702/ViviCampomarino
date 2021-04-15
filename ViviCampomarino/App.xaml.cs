using Plugin.Firebase.CloudMessaging;
//using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino {
    public partial class App : Application {

        public static String LoginUidAuth = "";
        public static Login login=null;
        public static SqlLiteDatabase IstanzaSqlLite;
        public App() {
            InitializeComponent();
            LeggiImpostazioni();
            //IstanzaSqlLite = new SqlLiteDatabase();
            var home = new PageLoading();
            MainPage = home;
            CrossFirebaseCloudMessaging.Current.NotificationReceived += Current_NotificationReceived;
            CrossFirebaseCloudMessaging.Current.NotificationTapped += Current_NotificationTapped;
            CrossFirebaseCloudMessaging.Current.Error += Current_Error;
            CrossFirebaseCloudMessaging.Current.TokenChanged += Current_TokenChanged;
            //CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync("generale");
            

        }
        public static void SalvaImpostazioni() {
            Preferences.Set("LoginUidAuth", LoginUidAuth);
        }
        public async static void LeggiImpostazioni() {
            LoginUidAuth=Preferences.Get("LoginUidAuth", "");
            if (LoginUidAuth != "") {
                try {
                    var db = new Database<Login>();
                    App.login = await db.ReadDocument("Login/" + App.LoginUidAuth);
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
            Device.BeginInvokeOnMainThread(() => {
                Application.Current.MainPage.DisplayAlert("Tapped", e.Notification.Body, "OK");
            });
            //throw new NotImplementedException();
        }
        //public async static void CheckNotificaLibri() {
        //    var db = new Database<object>();
        //    var collNot = db.current.GetCollection("Login/" + App.LoginUidAuth + "/NotificheLibri");
        //    var docNot = await collNot.GetDocumentsAsync<NotificheLibri>();
        //    var collLibri = db.current.GetCollection("Libri");
        //    foreach (var x in docNot.Documents) {
        //        if (x.Data.NotificaDisponibile != true) continue;
        //        var LibroId = x.Reference.Id.ToString();
        //        var Libro = await collLibri.GetDocument(LibroId).GetDocumentSnapshotAsync<Libro>();
        //        if (Libro.Data.LibroDisponibile() == ViviCampomarino.Libro._Disponibile.Disponibile) {
        //            //CrossLocalNotifications.Current.Show("Libro ora disponibile", "Il libro '" + Libro.Data.Titolo + "' è ora disponibile!");
        //            await x.Reference.DeleteDocumentAsync();
        //        }
        //    }
        //}
        private void Current_NotificationReceived(object sender, Plugin.Firebase.CloudMessaging.EventArgs.FCMNotificationReceivedEventArgs e) {
            if (e.Notification.Data.ContainsKey("NotificaDisponibilita")) {
                Plugin.Firebase.CloudMessaging.CrossFirebaseCloudMessaging.Current.UnsubscribeFromTopicAsync(e.Notification.Data["Notifica"]);
            }
            //Memorizza Notifica su Sql Lite
            //var Notifica = new SqlLiteNotifiche();
            //Notifica.Data = DateTime.Now;
            //Notifica.Descrizione = e.Notification.Body;
            //Notifica.Letta = false;
            //SqlLiteDatabase.Connessione.Insert(Notifica);

            Device.BeginInvokeOnMainThread(() => {
                Application.Current.MainPage.DisplayAlert(e.Notification.Title, e.Notification.Body, "OK");
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
