
using System.Net;
using System.Text.Json;
using System.Text;
using webbutveckling_labb2_LottaHarmonen.Shared.DTOs;
namespace webbutveckling_labb2_LottaHarmonen.Client;

public class CustomerService
{
    private readonly HttpClient _httpClient;
    public CustomerDTO? CurrentUser { get; set; } = null;


    public CustomerService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("WineStoreApi");
    }


    public async Task<IEnumerable<CustomerDTO?>> GetAllCustomer()
    {
        var response = await _httpClient.GetAsync("/customers");

        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<CustomerDTO>();
        }

        var result = await response.Content.ReadFromJsonAsync<List<CustomerDTO>>();
        return result ?? Enumerable.Empty<CustomerDTO>();
    }

    public async Task<bool> AddNewCustomer(CustomerDTO NewCustomer)
    {

        var response = await _httpClient.PostAsJsonAsync("/customers", NewCustomer);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public async Task<CustomerDTO> GetCustomerByEmail(String email)
    {
        var response = await _httpClient.GetAsync($"/customers/email/{email}");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<CustomerDTO>();
            return result;
        }
        return null;

    }

    public async Task UpdateCustomer(CustomerDTO currentCustomerDto)
    {

        var json = JsonSerializer.Serialize(currentCustomerDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"/customers/{currentCustomerDto.Id}", content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Customer updated successfully.");
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {errorMessage}");
        }
    }
}
