using Microsoft.EntityFrameworkCore;
using webbutveckling_labb2_LottaHarmonen.DataAccess.Entities;

namespace webbutveckling_labb2_LottaHarmonen.DataAccess.Repositories;

public class OrderWineRepository
{

    private readonly WineStoreDbContext _context;

    public OrderWineRepository(WineStoreDbContext context)
    {
        _context = context;
    }

    public async Task AddOrdersAndItemsToJunction(Order order)
    {
        foreach (var product in order.Wines)
        {
            var existingOrderWine = await _context.OrderWines
                .SingleOrDefaultAsync(ow => ow.OrderId == order.Id && ow.WineId == product.ProductId);

            if (existingOrderWine != null)
            {
                existingOrderWine.Quantity++;
                _context.OrderWines.Update(existingOrderWine);
            }
            else
            {
                var newOrderWine = new OrderWine()
                {
                    OrderId = order.Id,
                    WineId = product.ProductId,
                    Quantity = 1
                };
                await _context.OrderWines.AddAsync(newOrderWine);
            }
            await _context.SaveChangesAsync();
        }

    }

}