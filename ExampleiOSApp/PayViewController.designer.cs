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
	[Register ("PayViewController")]
	partial class PayViewController
	{
		[Outlet]
		UIKit.UIButton buttonGoToBusinessApp { get; set; }

		[Outlet]
		UIKit.UIButton buttonPay { get; set; }

		[Outlet]
		UIKit.UITextField textFieldAmount { get; set; }

		[Outlet]
		UIKit.UITextField textFieldIBAN { get; set; }

		[Outlet]
		UIKit.UITextField textFieldPurpose { get; set; }

		[Outlet]
		UIKit.UITextField textFieldRecipient { get; set; }

		[Action ("GoToBusinessApp:")]
		partial void GoToBusinessApp (Foundation.NSObject sender);

		[Action ("Pay:")]
		partial void Pay (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (buttonGoToBusinessApp != null) {
				buttonGoToBusinessApp.Dispose ();
				buttonGoToBusinessApp = null;
			}

			if (buttonPay != null) {
				buttonPay.Dispose ();
				buttonPay = null;
			}

			if (textFieldAmount != null) {
				textFieldAmount.Dispose ();
				textFieldAmount = null;
			}

			if (textFieldIBAN != null) {
				textFieldIBAN.Dispose ();
				textFieldIBAN = null;
			}

			if (textFieldPurpose != null) {
				textFieldPurpose.Dispose ();
				textFieldPurpose = null;
			}

			if (textFieldRecipient != null) {
				textFieldRecipient.Dispose ();
				textFieldRecipient = null;
			}
		}
	}
}
