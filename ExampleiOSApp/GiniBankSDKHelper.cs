using System;
using System.Threading.Tasks;
using ExampleiOSApp.Helpers;
using Foundation;
using GiniBank.iOS;
using UIKit;

namespace ExampleiOSApp
{
    public class GiniBankSDKHelper
    {
        private readonly GiniBankConfigurationProxy _gbConfiguration;
        private GiniBankProxy _gbProxy;
        private GiniCaptureDelegate _gcDelegate;

        private static GiniBankSDKHelper _instance;
        public static GiniBankSDKHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GiniBankSDKHelper();
                }

                return _instance;
            }
        }

        /// <summary>
        /// showingNoResultsScreen
        /// </summary>
        public Action<bool> OnCaptureAnalysisDidFinishWithoutResults;

        /// <summary>
        /// result, sendFeedbackBlock
        /// </summary>
        public Action<AnalysisResultProxy, Action<ExtractionProxies>> OnCaptureAnalysisDidFinishWithResult;

        public Action OnCaptureDidCancelAnalysis;

        public static Task<string> ReceivePaymentRequestIdFromUrlAsync(NSUrl url)
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            GiniSDKProxy.ReceivePaymentRequestIdFromUrlWithUrl(url,
                (paymentRequestId) =>
                {
                    tcs.SetResult(paymentRequestId);
                },
                (error) =>
                {
                    Console.WriteLine($"ReceivePaymentRequestIdFromUrlWithUrl error: {error}");
                    tcs.SetResult(null);
                });

            return tcs.Task;
        }

        public static Task<GiniCaptureDocumentProxy> BuildImportedDocumentWithAsync(NSUrl url)
        {
            TaskCompletionSource<GiniCaptureDocumentProxy> tcs = new TaskCompletionSource<GiniCaptureDocumentProxy>();

            var documentBuilder = new GiniCaptureDocumentBuilderProxy();

            try
            {
                documentBuilder.BuildWith(url,
                    (document) =>
                    {
                        tcs.SetResult(document);
                    });
            }
            catch (Exception ex)
            {
                tcs.SetResult(null);
            }

            return tcs.Task;
        }

        public GiniBankSDKHelper()
        {
            _gbConfiguration = new GiniBankConfigurationProxy
            {
                EnableReturnReasons = true,
                ReturnAssistantEnabled = true,
                OpenWithEnabled = true,
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
                HelpButtonResource = new SimplePreferredButtonResource(UIImage.FromBundle("Help"), null)
            };

            // You can change the order of the onboarding pages by getting the default pages and modifying the array
            UIView[] pages = _gbConfiguration.OnboardingPages;
            UIView page1 = pages[0];
            pages[0] = pages[2];
            pages[2] = page1;

            //// Set the modified pages to be used for onboarding
            _gbConfiguration.OnboardingPages = pages;
        }

        public void Start(UIViewController viewController, GiniCaptureDocumentProxy importedDocument = null)
        {
            if (_gcDelegate != null)
            {
                _gcDelegate.Dispose();
                _gbProxy.Dispose();
            }

            _gcDelegate = new GiniCaptureDelegate(this);

            var credentials = CredentialsHelper.GetGiniBankCredentials();

            _gbProxy = new GiniBankProxy(
                credentials.clientDomain,
                credentials.clientId,
                credentials.clientPassword,
                _gbConfiguration,
                _gcDelegate,
                importedDocument);


            var gcViewController = _gbProxy.ViewController;
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

            _giniBankSDKHelper.OnCaptureAnalysisDidFinishWithoutResults?.Invoke(showingNoResultsScreen);
        }

        public void GiniCaptureAnalysisDidFinishWithResult(AnalysisResultProxy result, Action<ExtractionProxies> sendFeedbackBlock)
        {
            Console.WriteLine("Extractions returned:");

            GCViewController.DismissViewController(true, null);

            _giniBankSDKHelper.OnCaptureAnalysisDidFinishWithResult?.Invoke(result, sendFeedbackBlock);

            //sendFeedbackBlock(result.Extractions);
        }

        public void GiniCaptureDidCancelAnalysis()
        {
            Console.WriteLine("GiniCapture cancelled");

            GCViewController.DismissViewController(true, null);

            _giniBankSDKHelper.OnCaptureDidCancelAnalysis?.Invoke();
        }
    }
}
