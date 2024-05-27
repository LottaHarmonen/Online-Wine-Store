using Microsoft.EntityFrameworkCore;
using webbutveckling_labb2_LottaHarmonen.DataAccess.Entities;

namespace webbutveckling_labb2_LottaHarmonen.DataAccess.Repositories;

public class WineRepository
{
    private readonly WineStoreDbContext _context;


    public WineRepository(WineStoreDbContext context)
    {
        _context = context;
    }


    //GetAllWines
    public async Task<IEnumerable<Wine?>> GetAllWines()
    {
        return _context.Wines;
    }

    //GetWineByName
    public async Task<Wine?> GetWineByName(string name)
    {
        return await _context.Wines.FirstOrDefaultAsync(p => p.Name == name);
    }

    //GetWineByProductId
    public async Task<Wine?> GetWineById(int id)
    {
        return await _context.Wines.FindAsync(id);
    }

    //GetWineByType
    public async Task<IEnumerable<Wine>> GetWineByType(string type)
    {
        var wineTypeRepo = new WineTypeRepository(_context);
        var typeObject = await wineTypeRepo.GetWineTypeByName(type);

        return await _context.Wines.Where(p => p.TypeId == typeObject.Id).ToListAsync();
    }

    //GetWinesByOrder
    public async Task<IEnumerable<Wine>> GetWinesByOrderNumber(int orderNumber)
    {
        var orderWines = _context.OrderWines
            .Where(ow => ow.OrderId == orderNumber)
            .ToList();

        if (orderWines is null)
        {
            return Enumerable.Empty<Wine>();
        }

        //A list for the wines from one order 
        var wines = new List<Wine>();

        foreach (var order in orderWines)
        {
            //Get the right wine for this 
            var wine = await GetWineById(order.WineId);

            for (int i = 0; i < order.Quantity; i++)
            {
                var newWine = new Wine
                {
                    Name = wine.Name,
                    Description = wine.Description,
                    Price = wine.Price,
                    Status = wine.Status,
                    TypeId = wine.TypeId

                };

                wines.Add(newWine);
            }
        }

        return wines;

    }


    //AddNewWine
    public async Task AddNewWine(Wine newWine)
    {
        await _context.Wines.AddAsync(newWine);
        await _context.SaveChangesAsync();
    }

    //UpdateWineStatus
    public async Task UpdateWineStatus(int id, bool status)
    {
        var wine = await _context.Wines.FindAsync(id);
        if (wine is null)
        {
            return;
        }

        wine.Status = status;
        await _context.SaveChangesAsync();
    }

    //UpdateWine
    public async Task UpdateWine(int id, Wine newWineInfo)
    {
        var wine = await _context.Wines.FindAsync(id);
        if (wine is null)
        {
            return;
        }

        wine.Status = newWineInfo.Status;
        wine.Name = newWineInfo.Name;
        wine.Description = newWineInfo.Description;
        wine.Price = newWineInfo.Price;
        wine.TypeId = newWineInfo.TypeId;

        await _context.SaveChangesAsync();
    }

    //DeleteWine
    public async Task DeleteWine(int id)
    {
        var wine = await _context.Wines.FindAsync(id);
        if (wine is null)
        {
            return;
        }

        _context.Wines.Remove(wine);
        await _context.SaveChangesAsync();
    }

}