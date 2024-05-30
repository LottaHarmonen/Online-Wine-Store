using Microsoft.EntityFrameworkCore;
using webbutveckling_labb2_LottaHarmonen.DataAccess.Entities;

namespace webbutveckling_labb2_LottaHarmonen.DataAccess.Repositories;

public class WineTypeRepository
{

    private readonly WineStoreDbContext _context;

    public WineTypeRepository(WineStoreDbContext context)
    {
        _context = context;
    }



    //GetAllWineTypes
    public async Task<IEnumerable<WineType>> GetAllWineTypes()
    {
        return _context.WineTypes;
    }

    //AddNewWineType
    public async Task AddNewWineType(WineType newType)
    {
        await _context.WineTypes.AddAsync(newType);
        await _context.SaveChangesAsync();
    }

    //GetTypeById
    public async Task<WineType?> GetWineTypeById(int id)
    {
        var hej = await _context.WineTypes.FindAsync(id);
        return hej;
    }


    //UpdateWineType
    public async Task UpdateWineType(int id, WineType newWineTypeInfo)
    {
        var type = await _context.WineTypes.FindAsync(id);

        type.Name = newWineTypeInfo.Name;
        await _context.SaveChangesAsync();
    }

    public async Task<WineType?> GetWineTypeByName(string name)
    {
        return await _context.WineTypes.FirstOrDefaultAsync(p => p.Name == name);

    }
}