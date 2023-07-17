using System.Reflection;
using bookingSite.Models;
using bookingSite.Utils;

var builder = WebApplication.CreateBuilder(args);

//! Reads init configuration from appsettings.json, then registers the chosen types to the IoC container.
var initConfig = builder.Configuration.GetSection("initConfig");

    //// Get current assembly//// var assembly = Assembly.GetExecutingAssembly();
    //// get the type of a class from assembly//// var chosenType = assembly.GetType("bookingSite.Models.MongodbDataStoreAccess");

//! Add services to the container.
Type targetType = typeof(IDataStoreAccess);
Type chosenType = Type.GetType(initConfig["IDataStoreAccess"]); //returns MongodbDataStoreAccess or MysqlDataStoreAccess
builder.Services.AddTransient( targetType, chosenType );
builder.Services.AddTransient(typeof(AbstractHotelRepository),   Type.GetType(initConfig["AbstractHotelRepository"]));
//builder.Services.AddTransient(typeof(AbstractBookingRepository), Type.GetType(initConfig["AbstractBookingRepository"]));
//builder.Services.AddTransient(typeof(AbstractRoomRepository),    Type.GetType(initConfig["AbstractRoomRepository"]));
//builder.Services.AddTransient(typeof(AbstractBookingRepository), Type.GetType(initConfig["AbstractBookingRepository"]));
builder.Services.AddSingleton(typeof(ILog), Type.GetType(initConfig["ILog"]));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
