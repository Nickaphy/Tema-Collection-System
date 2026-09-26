using WatchWorld.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationService();

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorUI", policy =>
        policy.WithOrigins(
            "https://localhost:7163",
            "http://localhost:5275") //I need another port :C
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

await app.Services.InitializeDatabaseAsync();

app.UseCors("BlazorUI");
app.MapControllers();
app.Run();
