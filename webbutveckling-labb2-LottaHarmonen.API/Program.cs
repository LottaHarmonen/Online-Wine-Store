using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using webbutveckling_labb2_LottaHarmonen.API.Extensions;
using webbutveckling_labb2_LottaHarmonen.DataAccess;
using webbutveckling_labb2_LottaHarmonen.DataAccess.Entities;
using webbutveckling_labb2_LottaHarmonen.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("WineStoreDb");
builder.Services.AddDbContext<WineStoreDbContext>(
    options => options.UseSqlServer(connectionString)
    );


//TODO: change lifetime!!
builder.Services.AddScoped<WineRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<WineTypeRepository>();

var app = builder.Build();

app.MapWineStoreEndpoints();


app.Run();
