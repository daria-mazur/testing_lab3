using RestSharp;
using Newtonsoft.Json.Linq;

namespace ApiTestingProject.Services
{
    public class CatFactsService
    {
        private readonly RestClient client;

        public CatFactsService()
        {
            client = new RestClient("https://catfact.ninja/");
        }

        public RestResponse GetRandomCatFact()
        {
            var request = new RestRequest("fact", Method.Get);
            return client.Execute(request);
        }

        public RestResponse GetCatFactWithLengthLimit(int maxLength)
        {
            var request = new RestRequest("fact", Method.Get);
            request.AddParameter("max_length", maxLength);
            return client.Execute(request);
        }
    }
}
