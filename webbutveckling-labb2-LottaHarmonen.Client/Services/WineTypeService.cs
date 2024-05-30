using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using webbutveckling_labb2_LottaHarmonen.Shared.DTOs;
namespace webbutveckling_labb2_LottaHarmonen.Client;

public class WineTypeService
{
    private readonly HttpClient _httpClient;

    public WineTypeService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("WineStoreApi");
    }

    public async Task<IEnumerable<WineTypeDTO>> GetAllWineTypes()
    {
        var response = await _httpClient.GetAsync("/wineTypes");

        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<WineTypeDTO>();
        }

        var result = await response.Content.ReadFromJsonAsync<List<WineTypeDTO>>();
        return result ?? Enumerable.Empty<WineTypeDTO>();
    }

    public async Task AddNewWineType(string name)
    {
        var newWineType = new WineTypeDTO { Name = name };
        var jsonPayload = JsonSerializer.Serialize(newWineType);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/wineTypes", content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Wine type added successfully.");
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {errorMessage}");
        }
    }

    public async Task AddNewWine(WineDTO newWine)
    {
        var jsonPayload = JsonSerializer.Serialize(newWine);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/wines", content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Wine added successfully.");
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {errorMessage}");
        }
    }
}