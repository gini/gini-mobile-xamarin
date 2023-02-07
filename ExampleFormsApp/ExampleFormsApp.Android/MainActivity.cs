using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AndroidX.Activity.Result;
using Android.Content;
using Net.Gini.Android.Bank.Sdk.Capture;

namespace ExampleFormsApp.Droid
{
    [Activity(Label = "ExampleFormsApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IActivityResultCallback
    {
        public ActivityResultLauncher _captureLauncher;
        public ActivityResultLauncher _captureImportLauncher;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _captureLauncher = RegisterForActivityResult(new CaptureFlowContract(), this);
            _captureImportLauncher = RegisterForActivityResult(new CaptureFlowImportContract(), this);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            return;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnActivityResult(Java.Lang.Object p0)
        {
            //base.OnActivityResult(requestCode, resultCode, data);

            //if (requestCode == NoExtractionsActivity.ACTION_TAKE_PHOTO_AGAIN)
            //{
            //    GiniBank.Instance.StartCaptureFlow(_captureLauncher);
            //}
            //if (requestCode == NoExtractionsActivity.ACTION_TAKE_PHOTO_AGAIN)
            //{
            //    GiniBank.Instance.StartCaptureFlow(_captureLauncher);
            //}
            var r = 5;
        }
    }
}
