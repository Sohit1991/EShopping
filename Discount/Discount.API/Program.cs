using Discount.API.Service;
using Discount.Application;
using Discount.Application.Mapper;
using Discount.Core.Repositories;
using Discount.Infrastructure.Repositories;
using Discount.Infrastructure.Extensions;
using Common.Logging;
using Serilog;
using Common.Logging.Correlation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationDepedency();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
builder.Services.AddAutoMapper(typeof(DiscountProfile));
builder.Services.AddGrpc();
builder.Host.UseSerilog(Logging.configureLogger);

var app = builder.Build();

app.MigrateDatabase<Program>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapGrpcService<DiscountService>();
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Commmunication with gRPC endpoints must be made through a gRPC client");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
