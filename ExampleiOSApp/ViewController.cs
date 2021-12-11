using System;
using ExampleiOSApp.Helpers;
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

            // Go to Pay page if navigated by scheme like "ginipay-bank://blablabla?id=blablabla"
            if (!string.IsNullOrWhiteSpace(PayViewController.PaymentRequestId))
            {
                var payViewController = ViewControllerHelper.GetViewController<PayViewController>();
                PresentViewController(payViewController, true, null);
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
            GiniBankSDKHelper.Instance.Start(this);
        }
    }
}
