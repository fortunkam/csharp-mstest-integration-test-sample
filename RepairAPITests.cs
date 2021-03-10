using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpIntegrationTests
{
    [TestClass]
    public class RepairAPITests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            BaseUrl = Environment.GetEnvironmentVariable("BaseUrl");
            SubscriptionKey = Environment.GetEnvironmentVariable("SubscriptionKey");
        }

        private string BaseUrl;
        private string SubscriptionKey;

        [TestMethod]
        public async Task CallToRepairAPIReturnsSuccessful()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://{BaseUrl}/play/repair/123"),
                Headers =
                {
                    { "ocp-apim-subscription-key", SubscriptionKey }
                },
            };
            using (var response = await client.SendAsync(request))
            {
                Assert.IsTrue(response.IsSuccessStatusCode);
            }
        }

        [TestMethod]
        public async Task CallToRepairAPIWithoutIdReturns400()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://{BaseUrl}/play/repair"),
                Headers =
                {
                    { "ocp-apim-subscription-key", SubscriptionKey }
                },
            };
            using (var response = await client.SendAsync(request))
            {
                Assert.AreEqual(System.Net.HttpStatusCode.BadRequest,response.StatusCode);
            }
        }
    }
}
