using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Net.Gini.Android.Capture;
using Net.Gini.Android.Capture.Network;
using Net.Gini.Android.Capture.Network.Model;

namespace ExampleAndroidApp
{
    /**
    Displays the Pay5 extractions: paymentRecipient, iban, bic, amount and paymentReference.
    
    A menu item is added to send feedback. The amount is changed to 10.00:EUR or an amount of
    10.00:EUR is added, if missing.
    */
    [Activity(Label = "Extractions")]
    public class ExtractionsActivity : AppCompatActivity, IGiniCaptureNetworkCallback
    {
        internal static IDictionary<string, GiniCaptureSpecificExtraction> SpecificExtractions { get; set; }

        internal static Intent GetActionIndent(Activity activity)
        {
            var extractionsIndent = new Intent(activity, typeof(ExtractionsActivity));
            return extractionsIndent;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_extractions);

            SetExtractionList();
        }

        private void SetExtractionList()
        {
            var extractionsAsStrings = GetExtractionsAsListItems(SpecificExtractions);
            var extractionsAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, extractionsAsStrings);
            FindViewById<ListView>(Resource.Id.listview_extractions).Adapter = extractionsAdapter;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_extractions, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.feedback)
            {
                SendFeedback(SpecificExtractions);
            }

            return base.OnOptionsItemSelected(item);
        }

        private string[] GetExtractionsAsListItems(IDictionary<string, GiniCaptureSpecificExtraction> extractions)
        {
            var list = new List<string>();

            foreach (var extraction in extractions)
            {
                list.Add($"{extraction.Value.Name}: {extraction.Value.Value}");
            }

            return list.ToArray();
        }

        private void SendFeedback(IDictionary<string, GiniCaptureSpecificExtraction> extractions)
        {
            // An example for sending feedback where we change the amount or add one if it is missing
            // Feedback should be sent only for the user visible fields. Non-visible fields should be filtered out.
            // In a real application the user input should be used as the new value.

            var amount = extractions["amountToPay"];
            if (amount != null)
            {   // Let's assume the amount was wrong and change it
                amount.Value = "10.00:EUR";
                Toast.MakeText(this, "Amount changed to 10.00:EUR", ToastLength.Long).Show();
            }
            else
            {   // Amount was missing, let's add it
                extractions.Add("amountToPay", new GiniCaptureSpecificExtraction("amountToPay", "10.00:EUR", "amount", null, new List<GiniCaptureExtraction>()));
                Toast.MakeText(this, "Added amount of 10.00:EUR", ToastLength.Short).Show();
            }

            // Assuming the users is shown the amountToPay and the iban extractions
            Dictionary<string, GiniCaptureSpecificExtraction> userVisibleExtractions = new Dictionary<string, GiniCaptureSpecificExtraction>();
            userVisibleExtractions.Add("amountToPay", extractions["amountToPay"]);
            userVisibleExtractions.Add("iban", extractions["iban"]);

            // Now we can send feedback for the user-visible extractions
            // IMPORTANT: send feedback only for extractions the user has seen
            ShowProgressIndicator();
            GiniCapture.Instance.GiniCaptureNetworkApi.SendFeedback(userVisibleExtractions, this);
        }

        private void ShowProgressIndicator()
        {
            FindViewById<ListView>(Resource.Id.listview_extractions).Animate().Alpha(0.5f);
            FindViewById<LinearLayout>(Resource.Id.layout_progress).Visibility = ViewStates.Visible;
        }

        private void HideProgressIndicator()
        {
            FindViewById<ListView>(Resource.Id.listview_extractions).Animate().Alpha(1.0f);
            FindViewById<LinearLayout>(Resource.Id.layout_progress).Visibility = ViewStates.Gone;
        }

        #region IGiniCaptureNetworkCallback interface implementation

        public void Success(Java.Lang.Object p0)
        {
            HideProgressIndicator();
            Toast.MakeText(this, "Feedback sending succeeded", ToastLength.Long).Show();
        }

        public void Failure(Java.Lang.Object p0)
        {
            HideProgressIndicator();

            Error error = (Error)p0;
            Toast.MakeText(this, $"Feedback sending failed: {error}", ToastLength.Long).Show();
        }

        public void Cancelled()
        {
            HideProgressIndicator();
            Toast.MakeText(this, $"Feedback sending cancelled", ToastLength.Long).Show();
        }

        #endregion
    }
}
