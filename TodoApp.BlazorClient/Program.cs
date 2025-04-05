using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TodoApp.BlazorClient;
using TodoApp.BlazorClient.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://defaulturl.com"; 

// Configura HttpClient para apuntar a tu Web API (ajusta la URL según corresponda)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

// Registrar el servicio que consumirá los endpoints de la API
builder.Services.AddScoped<TodoApiService>();

await builder.Build().RunAsync();