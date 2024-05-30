using Microsoft.EntityFrameworkCore;
using System;
using webbutveckling_labb2_LottaHarmonen.DataAccess.Entities;

namespace webbutveckling_labb2_LottaHarmonen.DataAccess.Repositories;

public class CustomerRepository
{
    private readonly WineStoreDbContext _context;

    public CustomerRepository(WineStoreDbContext context)
    {
        _context = context;
    }


    //GetAllCustomers
    public async Task<IEnumerable<Customer?>> GetAllCustomers()
    {
        return _context.Customers;
    }


    //GetCustomerById
    public async Task<Customer?> GetCustomerById(int id)
    {
        return await _context.Customers.FindAsync(id);
    }


    //GetCustomerByEmail
    public async Task<Customer?> GetCustomerByEmail(string email)
    {
        return await _context.Customers.FirstOrDefaultAsync(p => p.Email == email);
    }



    //AddNewCustomer
    public async Task AddNewCustomer(Customer newCustomer)
    {
        await _context.Customers.AddAsync(newCustomer);
        await _context.SaveChangesAsync();
    }

    //UpdateCustomer
    public async Task UpdateCustomer(int id, Customer newCustomerInfo)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer is null)
        {
            return;
        }

        customer.Email = newCustomerInfo.Email;
        customer.FirstName = newCustomerInfo.FirstName;
        customer.Surname = newCustomerInfo.Surname;
        customer.Address = newCustomerInfo.Address;
        customer.PhoneNumber = newCustomerInfo.PhoneNumber;
        customer.Orders = newCustomerInfo.Orders;

        await _context.SaveChangesAsync();
    }


    //DeleteCustomer
    public async Task DeleteCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer is null)
        {
            return;
        }

        _context.Customers.Remove(customer);
    }




    //public List<Customer> Customers { get; set; } = new()
        //{
        //    new Customer(){Id = 1, FirstName = "Lotta", Surname = "Harmonen", Address = "Kungvaegen 3, 456 gbg", Email = "lotta.harmonen@hej.com", PhoneNumber = "+7076485211", Orders = new List < Order >()},
        //    new Customer(){Id = 2, FirstName =  "Alex", Surname = "Harmnen", Address = "Kungvaegen 4, 456 gbg", Email = "alex.harmonen@hej.com", PhoneNumber = "+7068785211", Orders = new List<Order>()},
        //    new Customer(){Id = 3, FirstName = "Anne", Surname = "Harmn", Address = "Kungvaegen 5, 456 gbg", Email = "anne@hej.com", PhoneNumber = "+7076666711", Orders = new List<Order>()},

        //};

    }