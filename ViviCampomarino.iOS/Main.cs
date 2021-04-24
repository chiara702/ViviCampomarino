using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace ViviCampomarino.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            Firebase.Core.App.Configure(); //Inizializzazione per Plugin.FirebaseAuth

            UIApplication.Main(args, null, "AppDelegate");
            
            

        }
    }
}
