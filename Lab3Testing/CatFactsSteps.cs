using NUnit.Framework;
using TechTalk.SpecFlow;
using ApiTestingProject.Services;
using Newtonsoft.Json.Linq;
using Lab3Testing;
using RestSharp;

namespace ApiTestingProject.Steps
{
    [Binding]
    public class CatFactsSteps
    {
        private readonly CatFactsService _catFactsService = new CatFactsService();
        private RestResponse response;

        [When(@"I request a random cat fact")]
        public void WhenIRequestARandomCatFact()
        {
            response = _catFactsService.GetRandomCatFact();
        }

        [Then(@"the status code should be (.*)")]
        public void ThenTheStatusCodeShouldBe(int expectedStatusCode)
        {
            Assert.AreEqual(expectedStatusCode, (int)response.StatusCode, "Status code is not as expected");
        }

        [Then(@"the response should contain a non-empty 'fact' field")]
        public void ThenTheResponseShouldContainANonEmptyFactField()
        {
            var jsonResponse = JObject.Parse(response.Content);
            Assert.IsTrue(jsonResponse.ContainsKey("fact"), "Response does not contain 'fact'");
            Assert.IsNotEmpty(jsonResponse["fact"].ToString(), "The 'fact' field is empty");
        }
    }
}
