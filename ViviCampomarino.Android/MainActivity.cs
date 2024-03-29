﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Plugin.Firebase.Android;
using Plugin.Firebase.Auth;
using Android.Content;
using Plugin.Firebase.CloudMessaging;
using Android.Content.Res;
//using Plugin.LocalNotifications;

namespace ViviCampomarino.Droid
{
    [Activity(Label = "ViviCampomarino", Icon = "@drawable/Icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            CrossFirebase.Initialize(this, savedInstanceState, new Plugin.Firebase.Shared.CrossFirebaseSettings(isFirestoreEnabled: true, isStorageEnabled: true, isAuthEnabled: true, isCloudMessagingEnabled: true));
            CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
            
            //Plugin FirebaseAuth Wrapper
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            //

            ZXing.Net.Mobile.Forms.Android.Platform.Init(); //zxing init

            //FirebaseCloudMessagingImplementation.OnNewIntent(Intent);
            CreateNotificationChannelIfNeeded();

            App app;
            if (Intent.Extras != null) {
                app = new App(true);
            } else {
                app = new App();
            }

            

            LoadApplication(app);
            
        }

        //aggiunto per i cellulari che hanno i font ingranditi
        protected override void AttachBaseContext(Context @base) {
            var configuration = new Configuration(@base.Resources.Configuration);
            configuration.FontScale = 1.1f;
            var config = Application.Context.CreateConfigurationContext(configuration);
            base.AttachBaseContext(config);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
            base.OnActivityResult(requestCode, resultCode, data);
            FirebaseAuthImplementation.HandleActivityResultAsync(requestCode, resultCode, data);
        }
        protected override void OnNewIntent(Intent intent) {
            base.OnNewIntent(intent);
            //FirebaseCloudMessagingImplementation.OnNewIntent(intent);
        }

        private void CreateNotificationChannelIfNeeded() {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O) {
                CreateNotificationChannel();
            }
        }

        private void CreateNotificationChannel() {
            var channelId = $"{PackageName}.general";
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            var channel = new NotificationChannel(channelId, "General", NotificationImportance.Default);
            notificationManager.CreateNotificationChannel(channel);
            FirebaseCloudMessagingImplementation.ChannelId = channelId;
        }

    }
}