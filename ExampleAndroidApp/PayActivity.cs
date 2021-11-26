using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.TextField;
using Java.Lang;
using Net.Gini.Android.Bank.Api;
using Net.Gini.Android.Bank.Sdk;
using Net.Gini.Android.Bank.Sdk.Pay;
using Net.Gini.Android.Core.Api.Models;
using static Net.Gini.Android.Bank.Sdk.Util.CoroutineContinuationHelper;

namespace ExampleAndroidApp
{
    [Activity(Label = "Pay", Exported = true)]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { "android.intent.category.DEFAULT" }, 
        DataHost = "payment",
        DataScheme = "ginipay") ]
    public class PayActivity : AppCompatActivity
    {
        private string _paymentRequestId;
        private PaymentRequest _paymentRequest;
        private ResolvedPayment _resolvedPayment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_pay);

            FindViewById<Button>(Resource.Id.resolve_payment).Click +=
                (s, e) => Pay();

            FindViewById<Button>(Resource.Id.return_to_payment_initiator_app).Click +=
                (s, e) => ReturnToPayInitiatorApp();

            InitGiniBank();
            GetPaymentRequest();
        }

        private void InitGiniBank()
        {
            var giniBankApi = new GiniBankAPIBuilder(this,
                    Resources.GetString(Resource.String.gini_client_id),
                    Resources.GetString(Resource.String.gini_client_secret),
                    Resources.GetString(Resource.String.gini_email_domain))
                    .Build();

            GiniBank.Instance.ReleaseGiniApi();
            GiniBank.Instance.SetGiniApi((GiniBankAPI)giniBankApi);
        }

        private void GetPaymentRequest()
        {
            try
            {
                _paymentRequestId = PaymentRequestIntentKt.GetRequestId(Intent);

                GiniBank.Instance.GetPaymentRequest(_paymentRequestId,
                    CallbackContinuation(new ContinuationCallBack(
                        (result) =>
                        {
                            _paymentRequest = result as PaymentRequest;
                            FindViewById<TextInputEditText>(Resource.Id.recipient).Text = _paymentRequest.Recipient;
                            FindViewById<TextInputEditText>(Resource.Id.iban).Text = _paymentRequest.Iban;
                            FindViewById<TextInputEditText>(Resource.Id.amount).Text = _paymentRequest.Amount;
                            FindViewById<TextInputEditText>(Resource.Id.purpose).Text = _paymentRequest.Purpose;
                        },
                        (error) => Toast.MakeText(this, error.Message ?? error.StackTrace, ToastLength.Long).Show(),
                        () => Toast.MakeText(this, "GetPaymentRequest Canceled", ToastLength.Long).Show())
                    ));
            }
            catch (IllegalStateException ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        private void Pay()
        {
            try
            {
                var recepient = FindViewById<TextInputEditText>(Resource.Id.recipient).Text;
                var iban = FindViewById<TextInputEditText>(Resource.Id.iban).Text;
                var amount = FindViewById<TextInputEditText>(Resource.Id.amount).Text;
                var purpose = FindViewById<TextInputEditText>(Resource.Id.purpose).Text;
                var bic = _paymentRequest?.Bic ?? "";

                GiniBank.Instance.ResolvePaymentRequest(_paymentRequestId,
                    new ResolvePaymentInput(recepient, iban, amount, purpose, bic),
                    CallbackContinuation(new ContinuationCallBack(
                        (result) =>
                        {
                            _resolvedPayment = result as ResolvedPayment;

                            FindViewById<Button>(Resource.Id.resolve_payment).Visibility = Android.Views.ViewStates.Gone;
                            FindViewById<Button>(Resource.Id.return_to_payment_initiator_app).Visibility = Android.Views.ViewStates.Visible;
                        },
                        (error) => Toast.MakeText(this, error.Message ?? error.StackTrace, ToastLength.Long).Show(),
                        () => Toast.MakeText(this, "Canceled", ToastLength.Long).Show())
                    )
                );
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }

        private void ReturnToPayInitiatorApp()
        {
            try
            {
                GiniBank.Instance.ReturnToPaymentInitiatorApp(this, _resolvedPayment);
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }
    }

    // Helper class to resolve problem with "Kotlin Coroutines" using from C#
    internal class ContinuationCallBack : Java.Lang.Object, IContinuationCallback
    {
        public Action<Java.Lang.Object> Finished { get; set; }
        public Action<Throwable> Failed { get; set; }
        public Action Canceled { get; set; }

        public ContinuationCallBack(
            Action<Java.Lang.Object> finished,
            Action<Throwable> failed,
            Action canceled)
        {
            Finished = finished;
            Failed = failed;
            Canceled = canceled;
        }

        public void OnCancelled()
        {
            Canceled?.Invoke();
        }

        public void OnFailed(Throwable error)
        {
            Failed?.Invoke(error);
        }

        public void OnFinished(Java.Lang.Object result)
        {
            Finished?.Invoke(result);
        }
    }
}
