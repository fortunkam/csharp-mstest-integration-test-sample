using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpIntegrationTests
{
    [TestClass]
    public class SecureAPITests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            TenantId = Environment.GetEnvironmentVariable("TenantId");
            ClientId = Environment.GetEnvironmentVariable("ClientId");
            ClientSecret = Environment.GetEnvironmentVariable("ClientSecret");
            Scope = Environment.GetEnvironmentVariable("Scope");
            BaseUrl = Environment.GetEnvironmentVariable("BaseUrl");
            SubscriptionKey = Environment.GetEnvironmentVariable("SubscriptionKey");
        }

        private string TenantId;
        private string ClientId;
        private string ClientSecret;
        private string Scope;
        private string BaseUrl;
        private string SubscriptionKey;

        private async Task<string> GetAccessToken()
        {
            var formData = $"client_id={ClientId}&scope={Scope}&client_secret={ClientSecret}&grant_type=client_credentials";
            var azureadbaseurl = "https://login.microsoftonline.com/";
            var client = new HttpClient();
            

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                
                
                RequestUri = new Uri($"{azureadbaseurl}{TenantId}/oauth2/v2.0/token"),
                Headers =
                {
                    { "ocp-apim-subscription-key", SubscriptionKey }
                },
                Content = new StringContent(formData,System.Text.ASCIIEncoding.ASCII, "application/x-www-form-urlencoded")
            };
  

            using (var response = await client.SendAsync(request))
            {
                Assert.IsTrue(response.IsSuccessStatusCode);
                var content = await response.Content.ReadAsStringAsync();
                dynamic jsonContent = JsonConvert.DeserializeObject<dynamic>(content);

                return jsonContent.access_token.ToString();


            }
        }

        [TestMethod]
        public async Task CallToOAuthV2ReturnsIfAuthenticated()
        {

            var accessToken = await GetAccessToken();
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://{BaseUrl}/oauthv2"),
                Headers =
                {
                    { "ocp-apim-subscription-key", SubscriptionKey },
                    { "Authorization", $"Bearer {accessToken}" }
                },
            };
            using (var response = await client.SendAsync(request))
            {
                Assert.IsTrue(response.IsSuccessStatusCode);
            }
        }

        [TestMethod]
        public async Task CallToOAuthV2APIWithoutTokenReturns401()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://{BaseUrl}/oauthv2"),
                Headers =
                {
                    { "ocp-apim-subscription-key", SubscriptionKey }
                },
            };
            using (var response = await client.SendAsync(request))
            {
                Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }
    }
}
