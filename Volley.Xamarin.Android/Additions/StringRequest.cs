using Java.Lang;

namespace Com.Android.Volley.Toolbox
{
    partial class StringRequest
    {
        protected override void DeliverResponse(Object response)
        {
            DeliverResponse((String)response);
        }
    }
}