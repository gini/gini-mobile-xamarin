using Com.Android.Volley;
using Java.Lang;

namespace Net.Gini.Android.Core.Api.Authorization.Requests
{
    public partial class BearerByteArrayRequest
    {
        protected override void DeliverResponse(Object response)
        {
            DeliverResponse((byte[])response);
        }
    }
}
