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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/PageNotFound";
        await next();
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Books",
    pattern: "Books/{action=Page}/{pageNumber}",
    defaults: new
    {
        controller = "Books",
        action = "Page",
        pageNumber = 1,
    }
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
