using System;
using GiniBank.iOS;
using UIKit;

namespace ExampleiOSApp
{
    public partial class ViewController : UIViewController
    {
        private GiniBankSDKHelper _giniBankSDKHelper = new GiniBankSDKHelper();

        public ViewController(IntPtr handle) : base(handle)
        {
            _giniBankSDKHelper.OnGiniCaptureAnalysisDidFinishWithoutResults += GiniCaptureAnalysisDidFinishWithoutResults;
            _giniBankSDKHelper.OnGiniCaptureAnalysisDidFinishWithResult += GiniCaptureAnalysisDidFinishWithResult;
        }

        public void GiniCaptureAnalysisDidFinishWithoutResults(bool showingNoResultsScreen)
        {
            var r = 5;
            // TODO: go to NO results page
        }

        public void GiniCaptureAnalysisDidFinishWithResult(AnalysisResultProxy result, Action<ExtractionProxies> sendFeedbackBlock)
        {
            var r = 5;
            // TODO: go to results page
        }

        partial void ButtonStartClick(Foundation.NSObject sender)
        {
            _giniBankSDKHelper.Start(this);
        }
    }
}
