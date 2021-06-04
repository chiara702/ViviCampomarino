using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.CloudMessaging;
using Foundation;
using Plugin.Firebase.CloudMessaging;
using Plugin.Firebase.iOS;
using UIKit;
using UserNotifications;

namespace ViviCampomarino.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            Xamarin.FormsMaps.Init();
            Rox.VideoIos.Init();

            //CrossFirebase.Initialize(app, options, new Plugin.Firebase.Shared.CrossFirebaseSettings(isCloudMessagingEnabled: true));
            LoadApplication(new App());
            

            //Plugin.FirebaseAuth Wrapper
            //Firebase.Core.App.Configure();

           
            

            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            return base.FinishedLaunching(app, options);
        }
    }
}
