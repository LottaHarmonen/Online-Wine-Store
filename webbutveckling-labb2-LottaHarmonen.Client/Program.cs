using Microsoft.AspNetCore.Authentication.Cookies;
using webbutveckling_labb2_LottaHarmonen.Client.Components;
using MudBlazor.Services;
using webbutveckling_labb2_LottaHarmonen.Client;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddMudServices();

builder.Services.AddHttpClient("WineStoreApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5062");
});

builder.Services.AddScoped<WineService>();
builder.Services.AddScoped<WineTypeService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<OrderService>();



// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Authentication builder
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/LogInView";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
        //options.AccessDeniedPath = "/Access-denied";
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
