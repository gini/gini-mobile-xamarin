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

        public void CaptureAnalysisDidFinishWithoutResults(bool showingNoResultsScreen)
        {
            this.PresentViewController(
                ViewControllerHelper.GetViewController<NoResultsViewController>(), false, null);
        }

        public void CaptureAnalysisDidFinishWithResult(AnalysisResultProxy result, Action<ExtractionProxies> sendFeedbackBlock)
        {
            
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
