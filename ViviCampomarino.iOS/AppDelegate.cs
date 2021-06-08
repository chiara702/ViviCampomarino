﻿using System;
using System.Collections.Generic;
using System.Linq;
//using Firebase.CloudMessaging;
using Foundation;
using Plugin.Firebase.CloudMessaging;
using Plugin.Firebase.iOS;
using Plugin.Firebase.Shared;
using Plugin.FirebasePushNotification;
using UIKit;
using UserNotifications;

namespace ViviCampomarino.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate {
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


            

            //Plugin.FirebaseAuth Wrapper
            //Firebase.Core.App.Configure();

            


            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            LoadApplication(new App());

            //Firebase.Core.App.Configure(); //Inizializzazione per Plugin.FirebaseAuth
            CrossFirebase.Initialize(app, options, new CrossFirebaseSettings(isCloudMessagingEnabled: true));
            CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();

            //FirebasePushNotificationPlugin
            FirebasePushNotificationManager.Initialize(options, true);
            //

            return base.FinishedLaunching(app, options);
        }
    }
}
