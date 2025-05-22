using System.Data;
using System.IO.Compression;
using AdvancedTests.ECommerce.Application.Queries;
using AdvancedTests.ECommerce.Application.UseCases.CreateOrder;
using AdvancedTests.ECommerce.Application.UseCases.ListOrders;
using AdvancedTests.ECommerce.Domain;
using AdvancedTests.ECommerce.Domain.Repositories;
using AdvancedTests.ECommerce.Infrastructure;
using AdvancedTests.ECommerce.Infrastructure.Queries;
using AdvancedTests.ECommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddScoped<IOrderQueries, OrderQueries>()
    .AddScoped<IOrderRepository, OrderRepository>()
    .AddScoped<ICustomerRepository, CustomerRepository>()
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddScoped<ICreateOrder, CreateOrder>()
    .AddScoped<IListOrders, ListOrders>()
    .AddScoped<IDbConnection>(_ =>
    {
        var connectionString = builder.Configuration.GetConnectionString("OrdersDb");
        var connection = new MySqlConnection(connectionString);
        connection.Open();
        return connection;
    });

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });

var app = builder.Build();
builder.WebHost.UseUrls("http://*:5000", "https://*:5001");

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/orders", async ([FromQuery] int customerId, IListOrders listOrder, CancellationToken cancellationToken)
        => await listOrder.ExecuteAsync(new ListOrdersInput(customerId), cancellationToken))
    .WithOpenApi();

app.MapPost("/orders", async (CreateOrderInput input, ICreateOrder createOrder)
        => await createOrder.ExecuteAsync(input))
    .WithOpenApi();

app.Run();