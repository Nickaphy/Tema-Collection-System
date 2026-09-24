using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WatchWorld.BlazorUI;
using WatchWorld.Infrastructure;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:8080/") //changed hardcoded 7123 to 8080 exposed docker port
});

builder.Services
    .AddInfrastructureService(builder.Configuration)
    .AddApplicationService()
    .AddUIServices(builder.Configuration);


var app = builder.Build();


// builder.Services.AddScoped<IListingService, ListingService>();

await builder.Build().RunAsync();