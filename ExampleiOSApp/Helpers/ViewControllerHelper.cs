using System.Linq;
using UIKit;

namespace ExampleiOSApp.Helpers
{
    public class ViewControllerHelper
    {
        public static T GetViewController<T>() where T : UIViewController
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var viewController = storyboard.InstantiateViewController(typeof(T).Name);

            return (T)viewController;
        }

        public static UIViewController GetTopViewController()
        {
            var window = UIKit.UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;

            if (vc is UIKit.UINavigationController navController)
                vc = navController.ViewControllers.Last();

            return vc;
        }
    }
}
