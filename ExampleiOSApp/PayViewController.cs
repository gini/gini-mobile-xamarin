using System;
using ExampleiOSApp.Helpers;
using GiniBank.iOS;
using UIKit;

namespace ExampleiOSApp
{
	public partial class PayViewController : UIViewController
	{
        private GiniSDKProxy _giniSDK;
        private PaymentInfoProxy _paymentInfo;
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

            _giniSDK.ReceivePaymentRequestWithPaymentRequesId(SceneDelegate.PaymentRequestId,
                (paymentInfo) =>
                {
                    Console.WriteLine(paymentInfo);
                    _paymentInfo = paymentInfo;

                    textFieldRecipient.Text = _paymentInfo.Recipient;
                    textFieldIBAN.Text = _paymentInfo.Iban;
                    textFieldAmount.Text = _paymentInfo.Amount;
                    textFieldPurpose.Text = _paymentInfo.Purpose;

                    buttonPay.Hidden = false;
                },
                (error) =>
                {
                    Console.WriteLine(error);
                    var errorAlertController = UIAlertController.Create($"ReceivePaymentRequestWithPaymentRequesId:{SceneDelegate.PaymentRequestId}", error, UIAlertControllerStyle.Alert);
                    errorAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    PresentViewController(errorAlertController, true, null);
                });
        }

        partial void Pay(Foundation.NSObject sender)
        {
            var updatedPaymentInfo = new PaymentInfoProxy(
                textFieldRecipient.Text,
                textFieldIBAN.Text,
                _paymentInfo.Bic,
                textFieldAmount.Text,
                textFieldPurpose.Text);

            _giniSDK.ResolvePaymentRequestWithPaymentRequesId(SceneDelegate.PaymentRequestId, updatedPaymentInfo,
              (resolvedPaymentRequest) =>
              {
                  Console.WriteLine(resolvedPaymentRequest);
                  _resolvedPaymentRequest = resolvedPaymentRequest;

                  buttonPay.Hidden = true;
                  buttonGoToBusinessApp.Hidden = false;
              },
              (error) =>
              {
                  Console.WriteLine(error);
                  var errorAlertController = UIAlertController.Create($"ResolvePaymentRequestWithPaymentRequesId:{SceneDelegate.PaymentRequestId}", error, UIAlertControllerStyle.Alert);
                  errorAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                  PresentViewController(errorAlertController, true, null);
              });
        }

        partial void GoToBusinessApp(Foundation.NSObject sender)
        {
            _giniSDK.ReturnBackToBusinessAppHandlerWithResolvedPaymentRequest(_resolvedPaymentRequest);
        }
    }
}
