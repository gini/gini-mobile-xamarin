using System;
using System.Threading;
using ExampleiOSApp.Helpers;
using Foundation;
using GiniBank.iOS;
using UIKit;

namespace ExampleiOSApp
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
            GiniBankSDKHelper.Instance.OnCaptureAnalysisDidFinishWithoutResults += CaptureAnalysisDidFinishWithoutResults;
            GiniBankSDKHelper.Instance.OnCaptureAnalysisDidFinishWithResult += CaptureAnalysisDidFinishWithResult;
            GiniBankSDKHelper.Instance.OnCaptureDidCancelAnalysis += OnCaptureDidCancelAnalysis;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (!string.IsNullOrWhiteSpace(SceneDelegate.PaymentRequestId))
            {
                GoToPayPage();
            }

            if (SceneDelegate.ImportedDocumentUrl != null)
            {
                Start(SceneDelegate.ImportedDocumentUrl);
            }
        }

        public void CaptureAnalysisDidFinishWithoutResults(bool showingNoResultsScreen)
        {
            var noResultsViewController = ViewControllerHelper.GetViewController<NoResultsViewController>();
            PresentViewController(noResultsViewController, true, null);
        }

        public void CaptureAnalysisDidFinishWithResult(AnalysisResultProxy result, Action<ExtractionProxies> sendFeedbackBlock)
        {
            var filledResultsViewController = ViewControllerHelper.GetViewController<FilledResultsViewController>();
            filledResultsViewController.Data = result;
            PresentViewController(filledResultsViewController, true, null);
        }

        public void OnCaptureDidCancelAnalysis()
        {
            
        }

        partial void ButtonStartClick(Foundation.NSObject sender)
        {
            Start();
        }

        public async void Start(NSUrl ImportedDocumentUrl = null)
        {
            if (ImportedDocumentUrl != null)
            {
                var document = await GiniBankSDKHelper.BuildImportedDocumentWithAsync(ImportedDocumentUrl);
                GiniBankSDKHelper.Instance.Start(this, document);
            }
            else
            {
                GiniBankSDKHelper.Instance.Start(this);
            }
        }

        public void GoToPayPage()
        {
            var payViewController = ViewControllerHelper.GetViewController<PayViewController>();
            PresentViewController(payViewController, true, null);
        }
    }
}
