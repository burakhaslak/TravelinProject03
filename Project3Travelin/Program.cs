using MailKit;
using Microsoft.Extensions.Options;
using Project3Travelin.Services.BookingServices;
using Project3Travelin.Services.CategoryService;
using Project3Travelin.Services.CommentServices;
using Project3Travelin.Services.DailyTourPlanServices;
using Project3Travelin.Services.MailService;
using Project3Travelin.Services.TourServices;
using Project3Travelin.Settings;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICommentServices, CommentService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<IDailyTourPlanServices, DailyTourPlanServices>();
builder.Services.AddScoped<IBookingService, BookingServices>();
builder.Services.AddScoped<IBookingMailService, BookingMailService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettingsKey"));

builder.Services.AddScoped<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});


// Add services to the container.
builder.Services.AddControllersWithViews();

var defaultCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
