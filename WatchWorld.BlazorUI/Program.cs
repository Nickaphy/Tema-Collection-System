using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Http;
using WatchWorld.BlazorUI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<WatchWorldApiClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:8080/"); // Change back to http://localhost:8080 for real Client, https://localhost:64369/ for local testing
});

builder.Services.AddUIServices(builder.Configuration);

var app = builder.Build();

await app.RunAsync();
