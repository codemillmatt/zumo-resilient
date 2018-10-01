using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin.Forms;
using VSLiveToDo.Core;

namespace VSLiveToDo.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //public static NSData PushDeviceToken { get; private set; } = null;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            LoadApplication(new App());

            //if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            //{
            //    var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
            //        UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
            //        new NSSet());
            //    UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
            //    UIApplication.SharedApplication.RegisterForRemoteNotifications();
            //}

            return base.FinishedLaunching(app, options);
        }


        //public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        //{
        //    AppDelegate.PushDeviceToken = deviceToken;
        //}

        //public override void DidReceiveRemoteNotification(UIApplication application,
        //    NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        //{
        //    ProcessNotification(userInfo, false);
        //}

        //private void ProcessNotification(NSDictionary options, bool fromFinishedLoading)
        //{
        //if (!(options != null && options.ContainsKey(new NSString("aps"))))
        //{
        //    // Short circuit - nothing to do
        //    return;
        //}

        //NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

        //if (!fromFinishedLoading)
        //{
        //var alertString = GetStringFromOptions(aps, "alert");
        //// Manually show an alert
        //if (!string.IsNullOrEmpty(alertString))
        //{
        //var pictureString = GetStringFromOptions(aps, "picture");

        //// Show an alert
    }

    //        var opString = GetStringFromOptions(aps, "op");
    //        if (!string.IsNullOrEmpty(opString) && opString.Equals("sync"))
    //        {
    //            var syncMessage = new PushToSync()
    //            {
    //                Table = GetStringFromOptions(aps, "table"),
    //                Id = GetStringFromOptions(aps, "id")
    //            };
    //            MessagingCenter.Send<PushToSync>(syncMessage, "ItemsChanged");
    //        }
    //    }
    //}

    //private string GetStringFromOptions(NSDictionary options, string key)
    //{
    //    string v = string.Empty;
    //    if (options.ContainsKey(new NSString(key)))
    //    {
    //        v = (options[new NSString(key)] as NSString).ToString();
    //    }
    //    return v;
    //}
}

// Notes taken from https://adrianhall.github.io/develop-mobile-apps-with-csharp-and-azure/