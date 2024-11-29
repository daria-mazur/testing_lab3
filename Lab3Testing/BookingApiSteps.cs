using NUnit.Framework;
using TechTalk.SpecFlow;
using ApiTestingProject.Services;
using Newtonsoft.Json.Linq;
using RestSharp;
using Newtonsoft.Json;
using System.Net;

namespace ApiTestingProject.Steps
{
    [Binding]
    public class BookingApiSteps
    {
        private readonly BookingApiService _bookingApiService = new BookingApiService();
        private RestResponse response;
        private string bookingId;

        [Given(@"I am authenticated")]
        public void GivenIAmAuthenticated()
        {
            _bookingApiService.Authenticate();
        }

        [When(@"I create a new booking with the following details")]
        public void WhenICreateANewBookingWithTheFollowingDetails(Table table)
        {
            try
            {
                var bookingDetails = new
                {
                    firstname = table.Rows[0]["firstname"],
                    lastname = table.Rows[0]["lastname"],
                    totalprice = int.TryParse(table.Rows[0]["totalprice"], out var totalPrice) ? totalPrice : throw new Exception("Invalid totalprice"),
                    depositpaid = bool.TryParse(table.Rows[0]["depositpaid"], out var depositPaid) ? depositPaid : throw new Exception("Invalid depositpaid"),
                    bookingdates = new
                    {
                        checkin = DateTime.TryParse(table.Rows[0]["checkin"], out var checkinDate) ? checkinDate.ToString("yyyy-MM-dd") : throw new Exception("Invalid checkin date"),
                        checkout = DateTime.TryParse(table.Rows[0]["checkout"], out var checkoutDate) ? checkoutDate.ToString("yyyy-MM-dd") : throw new Exception("Invalid checkout date")
                    },
                    additionalneeds = table.Rows[0]["additionalneeds"]
                };

                // Виклик API
                response = _bookingApiService.CreateBooking(bookingDetails);

                // Перевірка відповіді
                if (!response.IsSuccessful)
                {
                    throw new Exception($"API error: {response.StatusCode} - {response.Content}");
                }

                // Обробка відповіді
                var jsonResponse = JObject.Parse(response.Content);
                bookingId = jsonResponse["bookingid"]?.ToString();

                if (string.IsNullOrEmpty(bookingId))
                {
                    throw new Exception("Booking ID is missing in the response");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing booking details: {ex.Message}");
            }
        }

        [Then(@"the booking should be created successfully")]
        public void ThenTheBookingShouldBeCreatedSuccessfully()
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Failed to create booking");
            Assert.IsNotNull(bookingId, "Booking ID is not present in the response");
        }
    }
}
