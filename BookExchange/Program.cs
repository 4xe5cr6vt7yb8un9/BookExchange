using BookExchange;
using BookExchange.Actions;
using BookExchange.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BookExchange.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BookExchangeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookExchangeContext") ?? throw new InvalidOperationException("Connection string 'BookExchangeContext' not found.")));

// Add services to the container.

builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
