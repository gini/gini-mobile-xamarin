﻿using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.Activity.Result;
using AndroidX.AppCompat.App;
using Net.Gini.Android.Bank.Api;
using Net.Gini.Android.Bank.Sdk;
using Net.Gini.Android.Bank.Sdk.Capture;
using Net.Gini.Android.Capture;
using Net.Gini.Android.Capture.Help;
using Net.Gini.Android.Capture.Network;
using Net.Gini.Android.Capture.Network.Model;
using Net.Gini.Android.Capture.Onboarding;
using Net.Gini.Android.Capture.Requirements;
using Net.Gini.Android.Capture.Util;
using Xamarin.Essentials;

namespace ExampleAndroidApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    [IntentFilter(new[] { Intent.ActionView, Intent.ActionSend, Intent.ActionSendMultiple },
        Categories = new[] { "android.intent.category.DEFAULT" }, DataMimeType = "image/*")]
    [IntentFilter(new[] { Intent.ActionView, Intent.ActionSend },
        Categories = new[] { "android.intent.category.DEFAULT" }, DataMimeType = "application/pdf")]
    public class MainActivity : AppCompatActivity, IActivityResultCallback, IGiniCaptureNetworkCallback
    {
        private ActivityResultLauncher _captureLauncher;
        private ActivityResultLauncher _captureImportLauncher;
        private ICancellationToken _cancellationToken;
        private GiniCaptureDefaultNetworkService _giniNetworkService;
        private GiniCaptureDefaultNetworkApi _giniNetworkApi;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Show SDK Version
            FindViewById<TextView>(Resource.Id.text_gini_bank_version).Text =
                $"Gini Bank SDK v{Net.Gini.Android.Bank.Sdk.BuildConfig.VersionName}";

            // Set Listener
            FindViewById<Button>(Resource.Id.button_start_scanner).Click +=
                async (s, e) => await StartGiniCaptureSdkAsync();

            _captureLauncher = RegisterForActivityResult(new CaptureFlowContract(), this);
            _captureImportLauncher = RegisterForActivityResult(new CaptureFlowImportContract(), this);

            if (savedInstanceState == null)
            {
                if (IsIntentActionViewOrSend(Intent))
                {
                    StartGiniCaptureSdkAsync(Intent);
                }
            }

            var i = new Intent(this, typeof(PayActivity));
            StartActivityForResult(i, 42);
        }

        private async Task StartGiniCaptureSdkAsync(Intent intent = null)
        {
            var caperaPermissionStatus = await GetCameraPermissionStatusAsync();
            if (caperaPermissionStatus != PermissionStatus.Granted)
            {
                if (intent != null)
                {
                    Finish();
                }
                return;
            }

            var report = GiniBank.Instance.CheckCaptureRequirements(this);
            if (!report.IsFulfilled)
            {
                ShowUnfulfilledRequirementsToast(report);
            }

            ConfigureGiniCapture();

            if (intent != null)
            {
                _cancellationToken = GiniBank.Instance.StartCaptureFlowForIntent(
                    _captureImportLauncher, this, intent);
            }
            else
            {
                GiniBank.Instance.StartCaptureFlow(_captureLauncher);
            }
        }

        private async Task<PermissionStatus> GetCameraPermissionStatusAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status == PermissionStatus.Granted)
                return status;

            if (Permissions.ShouldShowRationale<Permissions.Camera>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.Camera>();

            return status;
        }

        private void ShowUnfulfilledRequirementsToast(RequirementsReport report)
        {
            var stringBuilder = new StringBuilder();

            foreach (var requirementReport in report.RequirementReports)
            {
                if (!requirementReport.IsFulfilled)
                {
                    stringBuilder.Append(requirementReport.RequirementId);
                    stringBuilder.Append(": ");
                    stringBuilder.Append(requirementReport.Details);
                    stringBuilder.Append("\n");
                }
            }

            Toast.MakeText(this.ApplicationContext, "Requirements not fulfilled:\n" + stringBuilder,
            ToastLength.Long).Show();
        }

        private void ConfigureGiniCapture()
        {
            GiniBank.Instance.ReleaseCapture(this);

            var clientId = Resources.GetString(Resource.String.gini_client_id);
            var clientSecret = Resources.GetString(Resource.String.gini_client_secret);
            var emailDomain = Resources.GetString(Resource.String.gini_email_domain);

            //Create the default network service instance which talks to the Gini API to upload images and pdfs and downloads the extractions
            _giniNetworkService = GiniCaptureDefaultNetworkService.InvokeBuilder(this)
                .SetClientCredentials(clientId, clientSecret, emailDomain)
                .Build();

            // Create the default network api instance which allows easy upload of extraction feedback
            _giniNetworkApi = GiniCaptureDefaultNetworkApi.InvokeBuilder()
                .WithGiniCaptureDefaultNetworkService(_giniNetworkService)
                .Build();

            // You can change the order of the onboarding pages
            IList<OnboardingPage> onboardingPages = DefaultPagesPhone.AsArrayList();
            OnboardingPage page1 = onboardingPages[0];
            onboardingPages[0] = onboardingPages[2];
            onboardingPages[2] = page1;

            var config = new CaptureConfiguration(
                _giniNetworkService,
                _giniNetworkApi,
                true, // Show Onboarding on first run
                onboardingPages,
                false, // Show Onboarding
                true, // MultiPage Enabled
                DocumentImportEnabledFileTypes.PdfAndImages,
                true, // QR Code Scanning Enabled
                true, // File Import Enabled
                true, // Supported FormatsHelpScreen Enabled
                true, // Flash Button Enabled
                false, // Flash on by default
                true, // Back Buttons Enabled
                true, // Return Assistant Enabled
                null, // Event Tracker
                new List<HelpItem.Custom> // Custom Help Items
                {
                    new HelpItem.Custom(Resource.String.custom_help_screen_title,
                        new Intent(this, typeof(CustomHelpActivity))) 
                },  
                true, // GiniErrorLoggerIsOn
                null); // Error Logger listener

            GiniBank.Instance.SetCaptureConfiguration(config);
        }

        private void ShowExtractions(IDictionary<string, GiniCaptureSpecificExtraction> extractions)
        {
            var msg = "";

            foreach (var extraction in extractions)
            {
                msg += $"{extraction.Value.Name}: {extraction.Value.Value}\n";
            }

            var dialog = new AndroidX.AppCompat.App.AlertDialog.Builder(this);
            dialog.SetTitle("Extractions");
            dialog.SetMessage(msg);
            dialog.SetNegativeButton("Cancel", (s, e) => { });
            dialog.SetPositiveButton("Send Feedback", (s, e) => { SendFeedback(extractions); });
            dialog.Show();
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
            _giniNetworkApi.SendFeedback(userVisibleExtractions, this);
        }

        #region IGiniCaptureNetworkCallback interface implementation

        public void Success(Java.Lang.Object p0)
        {
            Toast.MakeText(this, "Feedback sending succeeded", ToastLength.Long).Show();
        }

        public void Failure(Java.Lang.Object p0)
        {
            Error error = (Error)p0;
            Toast.MakeText(this, $"Feedback sending failed: {error}", ToastLength.Long).Show();
        }

        public void Cancelled()
        {
            Toast.MakeText(this, $"Feedback sending cancelled", ToastLength.Long).Show();
        }

        #endregion

        public void OnActivityResult(Java.Lang.Object result)
        {
            if (result is CaptureResult captureResult)
            {
                if (captureResult is CaptureResult.Success)
                {
                    ShowExtractions(((CaptureResult.Success)captureResult).SpecificExtractions);
                }
                else if (captureResult is CaptureResult.Error error)
                {
                    if (error.Value is ResultError.Capture errorCapture)
                    {
                        Toast.MakeText(
                            this,
                            $"Error: {errorCapture.GiniCaptureError.GetErrorCode()} {errorCapture.GiniCaptureError.Message}",
                            ToastLength.Long
                        ).Show();
                    }
                    else if (error.Value is ResultError.FileImport errorImport)
                    {
                        Toast.MakeText(
                            this,
                            $"Error: {errorImport.Code} {errorImport.Message}",
                            ToastLength.Long
                        ).Show();
                    }
                }
                else if (captureResult is CaptureResult.Empty)
                {
                    StartActivityForResult(NoExtractionsActivity.GetActionIndent(this),
                        NoExtractionsActivity.ACTION_TAKE_PHOTO_AGAIN);
                }
                else if (captureResult is CaptureResult.Cancel)
                {

                }
            }
        }

        // Handle Action from NoExtractionsActivity
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == NoExtractionsActivity.ACTION_TAKE_PHOTO_AGAIN)
            {
                GiniBank.Instance.StartCaptureFlow(_captureLauncher);
            }
        }

        private bool IsIntentActionViewOrSend(Intent intent) =>
            Intent.ActionView == intent.Action ||
            Intent.ActionSend == intent.Action ||
            Intent.ActionSendMultiple == intent.Action;

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            // cancellationToken shouldn't be canceled when activity is recreated.
            _cancellationToken?.Cancel();
        }
    }
}
