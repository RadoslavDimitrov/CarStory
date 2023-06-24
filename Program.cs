using CarStory.Data;
using CarStory.Data.Models;
using CarStory.Infrastructure;
using CarStory.Services.Admin;
using CarStory.Services.Car;
using CarStory.Services.RepairShop;
using CarStory.Services.Search;
using CarStory.Services.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    { options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<UserManager<AppUser>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ISearchService, SearchService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<ICarService, CarService>();
builder.Services.AddTransient<IRepairShopService, RepairShopService>();

builder.Services.AddAntiforgery();

var app = builder.Build();

app.PrepareDatabase();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();



app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
