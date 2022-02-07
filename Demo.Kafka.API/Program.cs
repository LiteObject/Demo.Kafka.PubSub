using Demo.Kafka.API.Data;
using Demo.Kafka.API.Data.Contexts;
using Demo.Kafka.API.Data.Repositories;
using Demo.Kafka.API.Domain.Entities;
using Demo.Kafka.API.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Savorboard.CAP.InMemoryMessageQueue;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging(x => x.AddConsole());
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<OrderDbContext>(sp =>
{
    var dbContextOptionsBuilder = new DbContextOptionsBuilder<OrderDbContext>()
       .EnableSensitiveDataLogging()
       .LogTo(Console.WriteLine, LogLevel.Debug)
       .EnableDetailedErrors()
       .UseSqlServer(
           "Server=localhost;Port=5432;database=postgres;Username=postgres;Password=Demo.01",
           option => { option.EnableRetryOnFailure(); });

    var context = new OrderDbContext(dbContextOptionsBuilder.Options);

    // context.OnSaveEventHandlers = EntityEventHandler.OnSave;
    context.OnSaveEventHandlers += (entries) =>
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(">>>>> Delegate has been invoked on save.");
        Console.ResetColor();
    };

    // Option #2:
    // context.SavingChanges += Context_SavingChanges;

    return context;
});

// services.AddScoped<IRepository<Order>, GenericRepository<Order, OrderDbContext>>();
builder.Services.AddScoped<IRepository<Order>, OrderRepository>();

builder.Services.AddScoped<IDiscountService, NewYearDiscountService>();
builder.Services.AddScoped<IDiscountService, SpecialDiscountService>();

builder.Services.AddCap(x =>
{
    /* 
         * Use in-memory options if you don't want to set up infrastructure.
            x.UseInMemoryStorage();
            x.UseInMemoryMessageQueue(); 
        */

    x.UsePostgreSql("Server=localhost;Port=5432;database=postgres;Username=postgres;Password=Demo.01");
    x.UseRabbitMQ(z =>
    {
        z.HostName = "localhost";
        z.Port = 5672;
        z.UserName = "guest";
        z.Password = "guest";
    });

    x.FailedRetryCount = 5;
    x.FailedThresholdCallback = failed =>
    {
        var logger = failed.ServiceProvider.GetService<ILogger<Program>>();
        logger?.LogError($@"A message of type {failed.MessageType} failed after executing {x.FailedRetryCount} several times, 
                        requiring manual troubleshooting. Message name: {failed.Message.ToString()}");
    };
    x.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task SeedDatabase(OrderDbContext context)
{
    var orders = DataUtil.GenerateOrders();
    await context.Orders.AddRangeAsync(orders);
    int count = await context.SaveChangesAsync();
}

