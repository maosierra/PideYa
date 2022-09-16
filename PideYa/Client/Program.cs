using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PideYa.Client;
using PideYa.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<IHttpService, HttpService>()
                .AddScoped<ILocalStorageService, LocalStorageService>()
                .AddScoped<IOrderService, OrderService>()
                .AddScoped<IDishService, DishService>()
                .AddScoped<IDishCategoryService, DishCategoryService>()
                .AddScoped<OrderNotifierService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var host = builder.Build();

var authenticationService = host.Services.GetRequiredService<IAuthenticationService>();
await authenticationService.Initialize();

await host.RunAsync();

