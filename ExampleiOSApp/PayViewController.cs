using System;
using ExampleiOSApp.Helpers;
using GiniBank.iOS;
using UIKit;

namespace ExampleiOSApp
{
	public partial class PayViewController : UIViewController
	{
        private GiniSDKProxy _giniSDK;
        private ResolvedPaymentRequestProxy _resolvedPaymentRequest;

        public PayViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var credentials = CredentialsHelper.GetGiniBankCredentials();
            _giniSDK = new GiniSDKProxy(
                    credentials.clientDomain,
                    credentials.clientId,
                    credentials.clientPassword);

            _giniSDK.ReceivePaymentRequestWithPaymentRequesId("1",
                ReceivedPaymentRequestSuccess,
                (error) => Console.WriteLine(error));
        }

        private void ReceivedPaymentRequestSuccess(PaymentInfoProxy paymentInfo)
        {
            Console.WriteLine(paymentInfo);

            _giniSDK.ResolvePaymentRequestWithPaymentRequesId("1", paymentInfo,
                ResolvedPaymentRequestSuccess,
                (error) => Console.WriteLine(error));
        }

        private void ResolvedPaymentRequestSuccess(ResolvedPaymentRequestProxy resolvedPaymentRequest)
        {
            Console.WriteLine($"ResolvedPaymentRequestSuccess: {resolvedPaymentRequest}");

            _resolvedPaymentRequest = resolvedPaymentRequest;

            // TODO: show button "back to init app"
        }

        public void GoToInitApp()
        {
            _giniSDK.ReturnBackToBusinessAppHandlerWithResolvedPaymentRequest(_resolvedPaymentRequest);
        }
    }
}
