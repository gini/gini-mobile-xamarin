using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using ExampleFormsApp.Droid.Services;
using ExampleFormsApp.Services;
using Net.Gini.Android.Bank.Sdk;
using Net.Gini.Android.Bank.Sdk.Capture;
using Net.Gini.Android.Capture;
using Net.Gini.Android.Capture.Help;
using Net.Gini.Android.Capture.Network;
using Net.Gini.Android.Capture.Onboarding;
using Net.Gini.Android.Capture.Requirements;
using Net.Gini.Android.Capture.Util;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(GiniBankSDKService))]
namespace ExampleFormsApp.Droid.Services
{
    public class GiniBankSDKService : IGiniBankSDKService
    {
        private ICancellationToken _cancellationToken;
        private GiniCaptureDefaultNetworkService _giniNetworkService;
        private GiniCaptureDefaultNetworkApi _giniNetworkApi;

        private readonly MainActivity _context = (MainActivity)Platform.CurrentActivity;

        public void Start()
		{
            StartGiniCaptureSdkAsync();
        }

        private async Task StartGiniCaptureSdkAsync()
        {
            var caperaPermissionStatus = await GetCameraPermissionStatusAsync();
            if (caperaPermissionStatus != PermissionStatus.Granted)
            {
                return;
            }

            var report = GiniBank.Instance.CheckCaptureRequirements(_context);
            if (!report.IsFulfilled)
            {
                ShowUnfulfilledRequirementsToast(report);
            }

            ConfigureGiniBank();
            ConfigureGiniCapture();

            GiniBank.Instance.StartCaptureFlow(_context._captureLauncher);
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

            Toast.MakeText(_context, "Requirements not fulfilled:\n" + stringBuilder,
            ToastLength.Long).Show();
        }

        private void ConfigureGiniCapture()
        {
            GiniBank.Instance.ReleaseCapture(Android.App.Application.Context);

            var clientId = "";
            var clientSecret = "";
            var emailDomain = "";

            //Create the default network service instance which talks to the Gini API to upload images and pdfs and downloads the extractions
            _giniNetworkService = GiniCaptureDefaultNetworkService.InvokeBuilder(Android.App.Application.Context)
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
                
                },
                true, // GiniErrorLoggerIsOn
                null, // Error Logger listener
                5 * 1024 * 1024);

            GiniBank.Instance.SetCaptureConfiguration(config);
        }

        private void ConfigureGiniBank()
        {
            GiniBank.Instance.EnableReturnReasons = false;
        }

        public void OnActivityResult(Java.Lang.Object result)
        {
            if (result is CaptureResult captureResult)
            {
                //if (captureResult is CaptureResult.Success success)
                //{
                //    ExtractionsActivity.SpecificExtractions = success.SpecificExtractions;
                //    StartActivity(ExtractionsActivity.GetActionIndent(this));
                //}
                //else if (captureResult is CaptureResult.Error error)
                //{
                //    if (error.Value is ResultError.Capture errorCapture)
                //    {
                //        Toast.MakeText(
                //            this,
                //            $"Error: {errorCapture.GiniCaptureError.GetErrorCode()} {errorCapture.GiniCaptureError.Message}",
                //            ToastLength.Long
                //        ).Show();
                //    }
                //    else if (error.Value is ResultError.FileImport errorImport)
                //    {
                //        Toast.MakeText(
                //            this,
                //            $"Error: {errorImport.Code} {errorImport.Message}",
                //            ToastLength.Long
                //        ).Show();
                //    }
                //}
                //else if (captureResult is CaptureResult.Empty)
                //{
                //    StartActivityForResult(NoExtractionsActivity.GetActionIndent(this),
                //        NoExtractionsActivity.ACTION_TAKE_PHOTO_AGAIN);
                //}
                //else if (captureResult is CaptureResult.Cancel)
                //{

                //}
            }
        }

        // Handle Action from NoExtractionsActivity
        protected void OnActivityResult(int requestCode, [GeneratedEnum] Android.App.Result resultCode, Intent data)
        {
            //base.OnActivityResult(requestCode, resultCode, data);

            //if (requestCode == NoExtractionsActivity.ACTION_TAKE_PHOTO_AGAIN)
            //{
            //    GiniBank.Instance.StartCaptureFlow(_captureLauncher);
            //}
        }

        private bool IsIntentActionViewOrSend(Intent intent) =>
            Intent.ActionView == intent.Action ||
            Intent.ActionSend == intent.Action ||
            Intent.ActionSendMultiple == intent.Action;

        public void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}

