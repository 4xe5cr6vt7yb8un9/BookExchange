using Microsoft.EntityFrameworkCore;
using BookExchange.Data;

// Creates Web App and connects to SQL Server
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

// Shows 404 page if there is a 404 error
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

// Custom Route for books
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

// Default URL route 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Starts Application
app.Run();
