using Microsoft.EntityFrameworkCore;
using Sirena.Taxi.Core.Abstractions;
using Sirena.Taxi.Core.Abstractions.Repositories;
using Sirena.Taxi.Core.Kafka;
using Sirena.Taxi.Orders.Domain;
using Sirena.Taxi.Orders.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(x =>
{
    x.UseNpgsql(builder.Configuration.GetConnectionString("SirenaDb"));
    x.UseSnakeCaseNamingConvention();
    x.UseLazyLoadingProxies();
});
builder.Services.AddScoped(typeof(DbContext), typeof(DataContext));
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddSingleton(typeof(MessageProducer));
builder.Services.AddScoped(typeof(IEntityConsumerService), typeof(OrderEntityConsumerService));
builder.Services.AddHostedService<TopicConsumer>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();