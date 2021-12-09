using System;
using Foundation;
using GiniBank.iOS;
using UIKit;

namespace ExampleiOSApp
{
    public partial class FilledResultsViewController : UIViewController
    {
        public AnalysisResultProxy Data { get; set; }

        public FilledResultsViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.ResultsTable.Source = new ResultsTableSource(Data.Extractions.Extractions);
        }

        partial void Close(Foundation.NSObject sender)
		{
			DismissModalViewController(true);
		}
	}

    public class ResultsTableSource : UITableViewSource
    {
        private ExtractionProxy[] _items;
        private string cellIdentifier = "ResultItemCell";

        public ResultsTableSource(ExtractionProxy[] items)
        {
            _items = items;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _items.Length;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
            var item = _items[indexPath.Row];
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIdentifier);
            }
            cell.TextLabel.Text = item.Value;
            cell.DetailTextLabel.Text = item.Name;
            return cell;
        }
    }
}
