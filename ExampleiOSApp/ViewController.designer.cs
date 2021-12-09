// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace ExampleiOSApp
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIButton ButtonStart { get; set; }

		[Action ("ButtonStartClick:")]
		partial void ButtonStartClick (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonStart != null) {
				ButtonStart.Dispose ();
				ButtonStart = null;
			}
		}
	}
}
