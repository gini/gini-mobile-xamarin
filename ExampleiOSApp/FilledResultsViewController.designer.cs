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
	[Register ("FilledResultsViewController")]
	partial class FilledResultsViewController
	{
		[Outlet]
		UIKit.UITableView ResultsTable { get; set; }

		[Action ("Close:")]
		partial void Close (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ResultsTable != null) {
				ResultsTable.Dispose ();
				ResultsTable = null;
			}
		}
	}
}
