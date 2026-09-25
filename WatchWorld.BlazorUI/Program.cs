using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Http;
using WatchWorld.BlazorUI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<WatchWorldApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:8080/"); // your API's base address
});

builder.Services.AddUIServices(builder.Configuration);

var app = builder.Build();

await app.RunAsync();
