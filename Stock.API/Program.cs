using MassTransit;
using MongoDB.Driver;
using Shared;
using Stock.API.Consumers;
using Stock.API.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<OrderCreatedEventConsumer>();
    configurator.AddConsumer<PaymentFailedEventConsumer>();

    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);
        _configure.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue,e=>e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
        _configure.ReceiveEndpoint(RabbitMQSettings.Stock_PaymentFailedEventQueue,e=>e.ConfigureConsumer<PaymentFailedEventConsumer>(context));
    });
});

builder.Services.AddSingleton<MongoDBService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using IServiceScope scope=app.Services.CreateScope();
MongoDBService mongoDBService = scope.ServiceProvider.GetService<MongoDBService>();
var stockCollection = mongoDBService.GetCollection<Stock.API.Models.Stock>();
if (!stockCollection.FindSync(s => true).Any())
{
    await stockCollection.InsertOneAsync(new() { ProductId=Guid.NewGuid().ToString(),Count=100 });
    await stockCollection.InsertOneAsync(new() { ProductId=Guid.NewGuid().ToString(),Count=200 });
    await stockCollection.InsertOneAsync(new() { ProductId=Guid.NewGuid().ToString(),Count=50 });
    await stockCollection.InsertOneAsync(new() { ProductId=Guid.NewGuid().ToString(),Count=30 });
    await stockCollection.InsertOneAsync(new() { ProductId=Guid.NewGuid().ToString(),Count=5 });
}


app.Run();
