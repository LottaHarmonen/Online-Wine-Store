using Microsoft.EntityFrameworkCore;
using webbutveckling_labb2_LottaHarmonen.DataAccess.Entities;

namespace webbutveckling_labb2_LottaHarmonen.DataAccess.Repositories;

public class OrderRepository
{

    private readonly WineStoreDbContext _context;

    public OrderRepository(WineStoreDbContext context)
    {
        _context = context;
    }


    public async Task AddNewOrder(Order newOrder)
    {
        var orderWineRepo = new OrderWineRepository(_context);
        await _context.Orders.AddAsync(newOrder);
        await _context.SaveChangesAsync();

        await orderWineRepo.AddOrdersAndItemsToJunction(newOrder);

    }

    //GetOrderById
    public async Task<Order?> GetOrderById(int id)
    {
        return await _context.Orders.FindAsync(id);
    }


    //GetAllOrdersByCustomerId
    public async Task<List<Order>> GetAllOrdersByCustomerId(int Id)
    {
        var listOfOrders = await _context.Orders.Where(p => p.CustomerId == Id).ToListAsync();

        return listOfOrders;

    }

}