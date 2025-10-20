using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopLite.Data;
using ShopLite.Models;
var builder = WebApplication.CreateBuilder(args);


// Œcie¿ka do SQLite zale¿na od œrodowiska
var env = builder.Environment;
string dbPath;

if (env.IsDevelopment())
{
    // lokalnie - folder App_Data w projekcie
    var appDataPath = Path.Combine(env.ContentRootPath, "App_Data");
    if (!Directory.Exists(appDataPath))
        Directory.CreateDirectory(appDataPath);

    dbPath = Path.Combine(appDataPath, "ShopLiteContext-aa70459f-40bc-4f1c-81f3-f8f8a05469e8.db");
}
else
{
    // Azure Free / Linux - zapis w /home/data
    var homePath = Environment.GetEnvironmentVariable("HOME") ?? "/home";
    var azureDataPath = Path.Combine(homePath, "data");
    if (!Directory.Exists(azureDataPath))
        Directory.CreateDirectory(azureDataPath);

    dbPath = Path.Combine(azureDataPath, "ShopLiteContext-aa70459f-40bc-4f1c-81f3-f8f8a05469e8.db");
}

builder.Services.AddDbContext<ShopLiteContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));



// Add Session Service
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



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

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();


app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();




using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ShopLiteContext>();
    db.Database.Migrate();
}

// Seed Database with data if it is empty. 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}


app.Run();
