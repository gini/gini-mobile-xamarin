using System;
using UIKit;

namespace ExampleiOSApp
{
	public partial class NoResultsViewController : UIViewController
	{
		public NoResultsViewController (IntPtr handle) : base (handle)
		{
		}

		partial void GotIt(Foundation.NSObject sender)
		{
			DismissViewController(false, null);
		}
	}
}