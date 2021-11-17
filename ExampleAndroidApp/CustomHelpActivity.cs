using Android.App;
using Android.OS;
using AndroidX.AppCompat.App;

namespace ExampleAndroidApp
{
    [Activity(Label = "CustomHelpActivity")]
    public class CustomHelpActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_custom_help);
        }
    }
}
