using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using DeliveryStorage.Database.Data;
using DeliveryStorage.Database.Interfaces;
using DeliveryStorage.Database.Repositories;
using DeliveryStorage.Domain.Interfaces;
using DeliveryStorage.Domain.Services;

var builder = WebApplication.CreateBuilder(args);
 
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeliveryStorage API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IBoxService, BoxService>();
builder.Services.AddScoped<IPalletService, PalletService>();
builder.Services.AddScoped<IAssignBoxService, AssignBoxService>();

builder.Services.AddAutoMapper(typeof(DeliveryStorage.Domain.Mapping.MappingProfile));
builder.Services.AddAutoMapper(typeof(DeliveryStorage.API.Mapping.MappingProfile));

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Host.UseSerilog();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
await context.Database.MigrateAsync();

app.Run();
