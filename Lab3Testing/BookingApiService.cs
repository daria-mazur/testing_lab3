using RestSharp;
using Newtonsoft.Json.Linq;

namespace ApiTestingProject.Services
{
    public class BookingApiService
    {
        private readonly RestClient client;
        private string token;

        public BookingApiService()
        {
            client = new RestClient("https://restful-booker.herokuapp.com/");
        }

        public void Authenticate()
        {
            var authRequest = new RestRequest("auth", Method.Post);
            authRequest.AddJsonBody(new { username = "admin", password = "password123" });
            var authResponse = client.Execute(authRequest);
            var authJson = JObject.Parse(authResponse.Content);
            token = authJson["token"]?.ToString();
        }

        public RestResponse CreateBooking(object bookingDetails)
        {
            var request = new RestRequest("booking", Method.Post);
            request.AddJsonBody(bookingDetails);
            return client.Execute(request);
        }

        public RestResponse GetBookingById(string bookingId)
        {
            var request = new RestRequest($"booking/{bookingId}", Method.Get);
            return client.Execute(request);
        }

        public RestResponse UpdateBooking(string bookingId, object updatedDetails)
        {
            var request = new RestRequest($"booking/{bookingId}", Method.Put);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", $"token={token}");
            request.AddJsonBody(updatedDetails);
            return client.Execute(request);
        }

        public RestResponse DeleteBooking(string bookingId)
        {
            var request = new RestRequest($"booking/{bookingId}", Method.Delete);
            request.AddHeader("Cookie", $"token={token}");
            return client.Execute(request);
        }
    }
}
