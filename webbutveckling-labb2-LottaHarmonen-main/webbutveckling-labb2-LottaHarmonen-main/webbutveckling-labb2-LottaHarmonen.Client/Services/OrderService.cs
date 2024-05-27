namespace webbutveckling_labb2_LottaHarmonen.Client;

using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Xml.Linq;
using webbutveckling_labb2_LottaHarmonen.Shared.DTOs;

public class OrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("WineStoreApi");
    }


    public async Task<IEnumerable<OrdersByCustomerDTO>> GetAllOrdersByUser(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<OrdersByCustomerDTO>>($"/orders/{id}");

        if (response != null)
        {
            return response;
        }
        else
        {
            throw new HttpRequestException($"Failed to retrieve orders for customer with id: {id}");
        }
    }


    public async Task AddNewOrder(NewOrderDTO newOrder)
    {

        var jsonPayload = JsonSerializer.Serialize(newOrder);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/orders", content);

        // Check if the request was successful
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Order added successfully.");
        }
        else
        {
            // Read the error message from the response content
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {errorMessage}");
        }
    }

}