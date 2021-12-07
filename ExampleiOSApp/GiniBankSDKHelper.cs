using System;
using System.Linq;
using Foundation;
using GiniBank.iOS;
using UIKit;

namespace ExampleiOSApp
{
    public class GVLDelegate : NSObject, GVLProxyDelegate
    {
        public UIKit.UIViewController gvlViewController;

        public void GiniVisionAnalysisDidFinishWithoutResults(bool showingNoResultsScreen)
        {
            Console.WriteLine("GVL finished without results.");
            gvlViewController.DismissViewController(true, null);
        }

        public void GiniVisionAnalysisDidFinishWithResult(AnalysisResultProxy result, Action<ExtractionProxies> sendFeedbackBlock)
        {
            Console.WriteLine("Extractions returned:");

            //foreach (ExtractionProxy extraction in result.Extractions.Extractions)
            //{
            //    Console.WriteLine("Entity: " + extraction.Entity);
            //    Console.WriteLine("Name: " + extraction.Name);
            //    Console.WriteLine("Value: " + extraction.Value);
            //    Console.WriteLine("");
            //}

            //gvlViewController.DismissViewController(true, null);

            //// Let's simulate the user correcting the total value

            //int totalValueIndex = Array.FindIndex(result.Extractions.Extractions, extraction => extraction.Entity == "amount");
            //result.Extractions.Extractions[totalValueIndex].Value = "45.50:EUR";

            sendFeedbackBlock(result.Extractions);
        }

        public void GiniVisionDidCancelAnalysis()
        {
            Console.WriteLine("GVL cancelled");
            gvlViewController.DismissViewController(true, null);
        }
    }

    public class GiniVisionLib 
    {

        private readonly String domain = "sorochak.com";
        private readonly String id = "gini-mobile-xamarin";
        private readonly String secret = "JACRqOCXIRuQqU7CqVluW3VC";

        private readonly GVLDelegate gvlDelegate;
        private readonly GiniConfigurationProxy gvlConfiguration;

        public GiniVisionLib()
        {
            gvlDelegate = new GVLDelegate();

            gvlConfiguration = new GiniConfigurationProxy
            {
                DebugModeOn = true,
                FileImportSupportedTypes = GiniVisionImportFileTypesProxy.Pdf_and_images,
                QrCodeScanningEnabled = true,
                MultipageEnabled = true,
                FlashToggleEnabled = true,
                OnboardingShowAtLaunch = false,
                OnboardingShowAtFirstLaunch = true,
                NavigationBarTintColor = UIColor.Blue,
                NavigationBarItemTintColor = UIColor.White,
                NavigationBarTitleFont = UIFont.FromName("Trebuchet MS", 20),
                DocumentPickerNavigationBarTintColor = UIColor.Blue,
                CloseButtonResource = new SimplePreferredButtonResource(null, "Close please"),
                HelpButtonResource = new SimplePreferredButtonResource(UIImage.FromBundle("helpButton"), null),
            };

            // You can change the order of the onboarding pages by getting the default pages and modifying the array
            //UIView[] pages = gvlConfiguration.OnboardingPages;
            //UIView page1 = pages[0];
            //pages[0] = pages[2];
            //pages[2] = page1;

            //// Set the modified pages to be used for onboarding
            //gvlConfiguration.OnboardingPages = pages;
        }

        public void Start()
        {
            Console.WriteLine("Start");

            GVLProxy gvlProxy = new GVLProxy(
                domain,
                id,
                secret,
                gvlConfiguration,
                gvlDelegate);

            UIKit.UIViewController gvlViewController = gvlProxy.ViewController;
            gvlDelegate.gvlViewController = gvlViewController;


            GetTopViewController().ShowViewController(gvlViewController, null);
        }

        private UIKit.UIViewController GetTopViewController()
        {
            var window = UIKit.UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;

            if (vc is UIKit.UINavigationController navController)
                vc = navController.ViewControllers.Last();

            return vc;
        }
    }
}
