using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace ExampleAndroidApp
{
    /*
    Shown when none of the Pay5 extractions were received.
    
    Displays information about rotating the image to the correct orientation.
    
    We recommend showing a similar screen in your app to aid the user in taking better pictures to improve the
    quality of the extractions.
    */

    [Activity(Label = "Empty")]
    public class NoExtractionsActivity : AppCompatActivity
    {
        public const int ACTION_TAKE_PHOTO_AGAIN = 100;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_no_extractions);

            FindViewById<Button>(Resource.Id.button_new).Click += (s, e) =>
            {
                SetResult(Result.Ok);
                Finish();
            };
        }

        public static Intent GetActionIndent(Activity activity)
        {
            var noExtractionsIndent = new Intent(activity, typeof(NoExtractionsActivity));
            return noExtractionsIndent;
        }
    }
}
