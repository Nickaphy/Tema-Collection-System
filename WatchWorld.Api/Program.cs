using WatchWorld.Infrastructure;
using WatchWorld.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

await app.Services.InitializeDatabaseAsync();

app.MapControllers();

app.Run();
