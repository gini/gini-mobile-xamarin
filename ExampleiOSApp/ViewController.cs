using System;
using UIKit;

namespace ExampleiOSApp
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {

        }

        partial void ButtonStartClick(Foundation.NSObject sender)
        {
            try
            {
                var giniBankSDKHelper = new GiniBankSDKHelper();
                giniBankSDKHelper.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
