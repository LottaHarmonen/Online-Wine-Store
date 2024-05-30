using System.Text.Json;
using System.Text;
using webbutveckling_labb2_LottaHarmonen.Shared.DTOs;

namespace webbutveckling_labb2_LottaHarmonen.Client;
public class WineService
{
    private readonly HttpClient _httpClient;
    
    public WineService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("WineStoreApi");
    }


    //get all wines
    public async Task<IEnumerable<WineDTO>> GetAllWines()
    {
        var response = await _httpClient.GetAsync("/wines");

        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<WineDTO>();
        }

        var result = await response.Content.ReadFromJsonAsync<List<WineDTO>>();
        return result ?? Enumerable.Empty<WineDTO>();
    }

    public async Task UpdateWine(WineDTO selectedWine)
    {

        var json = JsonSerializer.Serialize(selectedWine);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"/wines/{selectedWine.ProductId}", content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Wine updated successfully.");
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {errorMessage}");
        }
    }


    public async Task DeleteWine(WineDTO selectedWine)
    {
        var json = JsonSerializer.Serialize(selectedWine);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.DeleteAsync($"/wines/{selectedWine.ProductId}");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Wine removed successfully.");
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {errorMessage}");
        }
    }



    public async Task<IEnumerable<WineByOrderDTO>> GetWinesByOrderId(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<WineByOrderDTO>>($"/wines/Order/{id}");

        if (response != null)
        {
            return response;
        }
        else
        {
            throw new HttpRequestException($"Failed to retrieve wines from an order with id: {id}");
        }
    }


    //----------------------CART FUNCTIONALITY--------------------
    public List<WineDTO> Cart { get; set; } = new List<WineDTO>();

    public void AddToCart(WineDTO item)
    {
        Cart.Add(item);
    }

    public void RemoveFromCart(WineDTO item)
    {
        Cart.Remove(item);
    }


}