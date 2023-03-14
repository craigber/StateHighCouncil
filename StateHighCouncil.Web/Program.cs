using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using StateHighCouncil.Web.Data;
using StateHighCouncil.Web.Services;
using StateHighCouncil.Web.WebUpdater.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite("DataSource=StateHighCouncil.db"));

builder.Services.AddScoped<IApiUpdater, RosterApiUpdater>();
builder.Services.AddScoped<IBillsUpdater, BillsUpdater>();
builder.Services.AddScoped<IUpdateOrchestrator, ApiUpdateOrchestrator>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<ILegislatorService, LegislatorService>();
builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<IStatsService, StatsService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dataContext = services.GetRequiredService<DataContext>();
    DbInitializer.Initialize(dataContext);
}

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
