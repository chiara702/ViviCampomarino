using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Plugin.Firebase.Android;
using Plugin.Firebase.Auth;
using Android.Content;
using Plugin.Firebase.CloudMessaging;
using Android.Content.Res;

namespace ViviCampomarino.Droid
{
    [Activity(Label = "ViviCampomarino", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CrossFirebase.Initialize(this, savedInstanceState, new Plugin.Firebase.Shared.CrossFirebaseSettings(isFirestoreEnabled: true, isStorageEnabled: true, isAuthEnabled: true, isCloudMessagingEnabled: true));
            //FirebaseCloudMessagingImplementation.OnNewIntent(this.Intent);
            LoadApplication(new App());
            //AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) => {
            //    var newExc = new ApplicationException("AndroidEnvironment_UnhandledExceptionRaiser", args.Exception);
            //};
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
            FirebaseCloudMessagingImplementation.OnNewIntent(intent);

        }
        
    }
}