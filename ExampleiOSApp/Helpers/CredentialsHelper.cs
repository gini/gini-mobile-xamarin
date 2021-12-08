using Foundation;

namespace ExampleiOSApp.Helpers
{
    public static class CredentialsHelper
    {
        public static (string clientId, string clientPassword, string clientDomain) GetGiniBankCredentials()
        {
            var path = NSBundle.MainBundle.PathForResource("Credentials", "plist");
            var data = NSDictionary.FromFile(path);

            var clientDomain = data["client_domain"].ToString();
            var clientPassword = data["client_password"].ToString();
            var clientId = data["client_id"].ToString();

            return (clientId, clientPassword, clientDomain);
        }
    }
}
