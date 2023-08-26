using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InforceTask.Data;
using InforceTask.Data.Entity;
using InforceTask.Data.Repositories;
using InforceTask.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;


var builder = WebApplication.CreateBuilder(args);
var dbConnectionString = builder.Configuration.GetConnectionString("DefaultDbConnection") ??
                         throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// var ob = new DbContextOptionsBuilder<ShortenerDbContext>();
// ob.UseSqlite(dbConnectionString);
// using (var context = new ShortenerDbContext(ob.Options))
// {
//     context.Database.EnsureDeleted();
//     context.SaveChanges();
//     context.Database.EnsureCreated();
//     context.SaveChanges();
// }

// Add services to the container.
var identityConnectionString = builder.Configuration.GetConnectionString("DefaultIdentityConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(identityConnectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddDbContext<ShortenerDbContext>(options =>
    options.UseSqlite(dbConnectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "/ClientApp";
});
builder.Services.AddScoped<IRepository<AboutTextAreaData>, AboutRepository>();
builder.Services.AddScoped<IRepository<UrlsItem>, UrlsRepository>();
var app = builder.Build();

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
    app.UseSpaStaticFiles();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ApiUrls}/{action=UrlsTable}");
app.MapRazorPages();

app.Run();