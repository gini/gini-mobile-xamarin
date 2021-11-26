using Android.Util;
using Net.Gini.Android.Capture.Tracking;

namespace ExampleAndroidApp
{
    public class GiniCaptureEventTracker : Java.Lang.Object, IEventTracker
    {
        private readonly string _tag = nameof(GiniCaptureEventTracker);

        public void OnAnalysisScreenEvent(Event p0)
        {
            switch (p0.Type.ToString())
            {
                case var value when value == AnalysisScreenEvent.Error.Name():
                    var errorObj = p0.Details[AnalysisScreenEvent.ERROR_DETAILS_MAP_KEY.ErrorObject];
                    var errorMsg = p0.Details[AnalysisScreenEvent.ERROR_DETAILS_MAP_KEY.Message];
                    Log.Info(_tag, $"Analysis failed:\nerror:{errorObj}\nmessage: {errorMsg}");
                    break;
                case var value when value == AnalysisScreenEvent.Retry.Name():
                    Log.Info(_tag, "Retry analysis");
                    break;
                case var value when value == AnalysisScreenEvent.Cancel.Name():
                    Log.Info(_tag, "Analysis cancelled");
                    break;
                default:
                    break;
            }
        }

        public void OnCameraScreenEvent(Event p0)
        {
            switch (p0.Type.ToString())
            {
                case var value when value == CameraScreenEvent.TakePicture.Name():
                    Log.Info(_tag, "Take picture");
                    break;
                case var value when value == CameraScreenEvent.Help.Name():
                    Log.Info(_tag, "Show help");
                    break;
                case var value when value == CameraScreenEvent.Exit.Name():
                    Log.Info(_tag, "Exit");
                    break;
                default:
                    break;
            }
        }

        public void OnOnboardingScreenEvent(Event p0)
        {
            switch (p0.Type.ToString())
            {
                case var value when value == OnboardingScreenEvent.Start.Name():
                    Log.Info(_tag, "Onboarding started");
                    break;
                case var value when value == OnboardingScreenEvent.Finish.Name():
                    Log.Info(_tag, "Onboarding finished");
                    break;
                default:
                    break;
            }
        }

        public void OnReviewScreenEvent(Event p0)
        {
            switch (p0.Type.ToString())
            {
                case var value when value == ReviewScreenEvent.Next.Name():
                    Log.Info(_tag, "Go next to analyse");
                    break;
                case var value when value == ReviewScreenEvent.Back.Name():
                    Log.Info(_tag, "Go back to the camera");
                    break;
                case var value when value == ReviewScreenEvent.UploadError.Name():
                    var errorObj = p0.Details[ReviewScreenEvent.UPLOAD_ERROR_DETAILS_MAP_KEY.ErrorObject];
                    var errorMsg = p0.Details[ReviewScreenEvent.UPLOAD_ERROR_DETAILS_MAP_KEY.Message];
                    Log.Info(_tag, $"Upload failed:\nerror: {errorObj}\nmessage: {errorMsg}");
                    break;
                default:
                    break;
            }
        }
    }
}
