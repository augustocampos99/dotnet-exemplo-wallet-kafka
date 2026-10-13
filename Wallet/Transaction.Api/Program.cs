using Microsoft.EntityFrameworkCore;
using Transaction.Api.Consumers;
using Transaction.Api.Data;
using Transaction.Api.Services;
using Transaction.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// DI Context
var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<PostgreSQLContext>(options =>
        options.UseNpgsql(connectionString));

// DI Services
builder.Services.AddTransient<ITransactionService, TransactionService>();

// Start Kafka consumers as BAckgroundService
builder.Services.AddHostedService<ConsumerTransactionService>();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

