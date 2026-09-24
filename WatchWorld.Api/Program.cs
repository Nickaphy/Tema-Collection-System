using WatchWorld.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationService();

var app = builder.Build();

await app.Services.InitializeDatabaseAsync();

app.MapControllers();

app.Run();
