using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.TextField;
using Java.Lang;
using Net.Gini.Android.Bank.Api;
using Net.Gini.Android.Bank.Sdk;
using Net.Gini.Android.Bank.Sdk.Pay;
using Net.Gini.Android.Core.Api.Models;

namespace ExampleAndroidApp
{
    [Activity(Label = "Pay")]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { "android.intent.category.DEFAULT" }, 
        DataHost = "payment",
        DataScheme = "ginipay") ]
    public class PayActivity : AppCompatActivity
    {
        private string _paymentRequestId;
        private PaymentRequest _paymentRequest;
        private ResolvedPayment _resolvedPayment;
        private ResolvePaymentInput _resolvePaymentInput;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_pay);

            FindViewById<Button>(Resource.Id.resolve_payment).Click += (s, e) =>
            {
                try
                {
                    // TODO: resolve issue with suspend fun
                    _resolvedPayment = GiniBank.Instance.ResolvePaymentRequest(_paymentRequestId,
                        new ResolvePaymentInput(
                            FindViewById<TextInputEditText>(Resource.Id.recipient).Text,
                            FindViewById<TextInputEditText>(Resource.Id.iban).Text,
                            FindViewById<TextInputEditText>(Resource.Id.amount).Text,
                            FindViewById<TextInputEditText>(Resource.Id.purpose).Text,
                            _paymentRequest.Bic
                            ), null);

                    FindViewById<Button>(Resource.Id.resolve_payment).Visibility = Android.Views.ViewStates.Gone;
                    FindViewById<Button>(Resource.Id.return_to_payment_initiator_app).Visibility = Android.Views.ViewStates.Visible;
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            FindViewById<Button>(Resource.Id.return_to_payment_initiator_app).Click += (s, e) =>
            {
                try
                {
                    GiniBank.Instance.ReturnToPaymentInitiatorApp(this, _resolvedPayment);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            try
            {
                var giniBankApi = new GiniBankAPIBuilder(this,
                    Resources.GetString(Resource.String.gini_client_id),
                    Resources.GetString(Resource.String.gini_client_secret),
                    Resources.GetString(Resource.String.gini_email_domain))
                    .Build();

                GiniBank.Instance.ReleaseGiniApi();
                GiniBank.Instance.SetGiniApi((GiniBankAPI)giniBankApi);

                // test ginipay intent
                Uri.Builder builder = new Uri.Builder();
                builder.Scheme("ginipay")
                    .Authority("payment")
                    .AppendPath("1");
                var myUrl = builder.Build();
                var ginipayIntent = new Intent(Intent.ActionView, myUrl, this, typeof(PayActivity));
                // ----

                _paymentRequestId = PaymentRequestIntentKt.GetRequestId(ginipayIntent);
                // TODO: resolve issue with suspend fun
                _paymentRequest = GiniBank.Instance.GetPaymentRequest(_paymentRequestId, null);

                FindViewById<TextInputEditText>(Resource.Id.recipient).Text = _paymentRequest.Recipient;
                FindViewById<TextInputEditText>(Resource.Id.iban).Text = _paymentRequest.Iban;
                FindViewById<TextInputEditText>(Resource.Id.amount).Text = _paymentRequest.Amount;
                FindViewById<TextInputEditText>(Resource.Id.purpose).Text = _paymentRequest.Purpose;
            }
            catch (IllegalStateException ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }
    }
}
