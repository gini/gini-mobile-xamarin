using System;
using ExampleiOSApp.Helpers;
using Foundation;
using GiniBank.iOS;
using UIKit;

namespace ExampleiOSApp
{
    public class GiniBankSDKHelper
    {
        private readonly GiniCaptureDelegate _gcDelegate;
        private readonly GiniConfigurationProxy _gConfiguration;

        /// <summary>
        /// showingNoResultsScreen
        /// </summary>
        public Action<bool> OnGiniCaptureAnalysisDidFinishWithoutResults;

        /// <summary>
        /// result, sendFeedbackBlock
        /// </summary>
        public Action<AnalysisResultProxy, Action<ExtractionProxies>> OnGiniCaptureAnalysisDidFinishWithResult;

        public Action OnGiniCaptureDidCancelAnalysis;

        public GiniBankSDKHelper()
        {
            _gcDelegate = new GiniCaptureDelegate(this);

            _gConfiguration = new GiniConfigurationProxy
            {
                DebugModeOn = true,
                FileImportSupportedTypes = GiniCaptureImportFileTypesProxy.Pdf_and_images,
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
            UIView[] pages = _gConfiguration.OnboardingPages;
            UIView page1 = pages[0];
            pages[0] = pages[2];
            pages[2] = page1;

            //// Set the modified pages to be used for onboarding
            _gConfiguration.OnboardingPages = pages;
        }

        public void Start(UIViewController viewController)
        {
            var credentials = CredentialsHelper.GetGiniBankCredentials();

            GiniCaptureProxy gcProxy = new GiniCaptureProxy(
                credentials.clientDomain,
                credentials.clientId,
                credentials.clientPassword,
                _gConfiguration,
                _gcDelegate);

            var gcViewController = gcProxy.ViewController;
            _gcDelegate.GCViewController = gcViewController;

            viewController.ShowViewController(gcViewController, null);
        }
    }

    public class GiniCaptureDelegate : NSObject, GiniCaptureProxyDelegate
    {
        private readonly GiniBankSDKHelper _giniBankSDKHelper;

        public UIViewController GCViewController { get; set; }

        public GiniCaptureDelegate(GiniBankSDKHelper giniBankSDKHelper)
        {
            _giniBankSDKHelper = giniBankSDKHelper;
        }

        public void GiniCaptureAnalysisDidFinishWithoutResults(bool showingNoResultsScreen)
        {
            Console.WriteLine("GiniCapture finished without results.");

            GCViewController.DismissViewController(true, null);

            _giniBankSDKHelper.OnGiniCaptureAnalysisDidFinishWithoutResults?.Invoke(showingNoResultsScreen);
        }

        public void GiniCaptureAnalysisDidFinishWithResult(AnalysisResultProxy result, Action<ExtractionProxies> sendFeedbackBlock)
        {
            Console.WriteLine("Extractions returned:");

            _giniBankSDKHelper.OnGiniCaptureAnalysisDidFinishWithResult?.Invoke(result, sendFeedbackBlock);

            //foreach (ExtractionProxy extraction in result.Extractions.Extractions)
            //{
            //    Console.WriteLine("Entity: " + extraction.Entity);
            //    Console.WriteLine("Name: " + extraction.Name);
            //    Console.WriteLine("Value: " + extraction.Value);
            //    Console.WriteLine("");
            //}

            //GCViewController.DismissViewController(true, null);

            //// Let's simulate the user correcting the total value

            //int totalValueIndex = Array.FindIndex(result.Extractions.Extractions, extraction => extraction.Entity == "amount");
            //result.Extractions.Extractions[totalValueIndex].Value = "45.50:EUR";

            //sendFeedbackBlock(result.Extractions);
        }

        public void GiniCaptureDidCancelAnalysis()
        {
            Console.WriteLine("GiniCapture cancelled");

            GCViewController.DismissViewController(true, null);

            _giniBankSDKHelper.OnGiniCaptureDidCancelAnalysis?.Invoke();
        }
    }
}
