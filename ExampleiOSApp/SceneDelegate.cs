using System;
using System.Threading.Tasks;
using ExampleiOSApp.Helpers;
using Foundation;
using UIKit;

namespace ExampleiOSApp
{
    [Register("SceneDelegate")]
    public class SceneDelegate : UIResponder, IUIWindowSceneDelegate
    {
        public static string PaymentRequestId { get; set; }
        public static NSUrl ImportedDocumentUrl { get; set; }

        // handle url like "ginipay-bank://blablabla?id=blablabla"
        private void HandleGiniPayBankScheme(NSUrl url)
        {
            if (url.ToString().StartsWith("ginipay-bank://"))
            {
                var paymentRequestId = GiniBankSDKHelper.ReceivePaymentRequestIdFromUrlAsync(url).Result;
                if (!string.IsNullOrWhiteSpace(paymentRequestId))
                {
                    PaymentRequestId = paymentRequestId;
                }

                try
                {
                    var topController = ViewControllerHelper.GetTopViewController();
                    if (topController is ViewController)
                    {
                        (topController as ViewController).GoToPayPage();
                    }
                }
                catch
                {
                    // crash if run on startup
                }
            }
        }

        private void HandleDocumentScheme(NSUrl url)
        {
            if (!url.ToString().StartsWith("ginipay-bank://"))
            {
                ImportedDocumentUrl = url;

                try
                {
                    var topController = ViewControllerHelper.GetTopViewController();
                    if (topController is ViewController)
                    {
                        (topController as ViewController).Start(ImportedDocumentUrl);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // crash if run on startup
                }
            }
        }

        [Export("window")]
        public UIWindow Window { get; set; }

        [Export("scene:openURLContexts:")]
        public void OpenUrlContexts(UIScene scene, NSSet<UIOpenUrlContext> urlContexts)
        {
            var url = urlContexts?.AnyObject?.Url;
            if (url == null)
            {
                return;
            }

            HandleGiniPayBankScheme(url);
            HandleDocumentScheme(url);
        }

        [Export("scene:willConnectToSession:options:")]
        public void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
        {
            var url = connectionOptions.UrlContexts?.AnyObject?.Url;
            if (url == null)
            {
                return;
            }

            HandleGiniPayBankScheme(url);
            HandleDocumentScheme(url);
        }

        [Export("sceneDidDisconnect:")]
        public void DidDisconnect(UIScene scene)
        {
            // Called as the scene is being released by the system.
            // This occurs shortly after the scene enters the background, or when its session is discarded.
            // Release any resources associated with this scene that can be re-created the next time the scene connects.
            // The scene may re-connect later, as its session was not neccessarily discarded (see UIApplicationDelegate `DidDiscardSceneSessions` instead).
        }

        [Export("sceneDidBecomeActive:")]
        public void DidBecomeActive(UIScene scene)
        {
            // Called when the scene has moved from an inactive state to an active state.
            // Use this method to restart any tasks that were paused (or not yet started) when the scene was inactive.
        }

        [Export("sceneWillResignActive:")]
        public void WillResignActive(UIScene scene)
        {
            // Called when the scene will move from an active state to an inactive state.
            // This may occur due to temporary interruptions (ex. an incoming phone call).
        }

        [Export("sceneWillEnterForeground:")]
        public void WillEnterForeground(UIScene scene)
        {
            // Called as the scene transitions from the background to the foreground.
            // Use this method to undo the changes made on entering the background.
        }

        [Export("sceneDidEnterBackground:")]
        public void DidEnterBackground(UIScene scene)
        {
            // Called as the scene transitions from the foreground to the background.
            // Use this method to save data, release shared resources, and store enough scene-specific state information
            // to restore the scene back to its current state.
        }
    }
}
