using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using order_routing.Server.Data;
using order_routing.Server.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<OrderDbContext>(opt => opt.UseNpgsql(connString));
builder.Services.AddScoped<IOrderLineService, OrderLineService>();
builder.Services.AddDataProtection().PersistKeysToDbContext<OrderDbContext>();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
