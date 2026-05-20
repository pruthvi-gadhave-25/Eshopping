using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Services;
using System;

var builder = WebApplication.CreateBuilder(args);



string connetionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(connetionString));


builder.Services.AddScoped<IProductService, ProductsService>();

// Add services to the container.

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

app.MapGet("/test", () => "product service working");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
