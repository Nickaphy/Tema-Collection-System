using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Http;
using WatchWorld.BlazorUI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<WatchWorldApiClient>(client =>
{
    BaseAddress = new Uri("http://localhost:8080/") //changed hardcoded 7123 to 8080 exposed docker port
});

builder.Services.AddUIServices(builder.Configuration);

var app = builder.Build();

await app.RunAsync();
