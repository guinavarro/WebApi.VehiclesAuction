using EllipticCurve;
using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApi.VehiclesAuction.Domain.BackgroundServices;
using WebApi.VehiclesAuction.Infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.ResolveDependencies();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<VehiclesAuctionContext>(
    x => x.UseNpgsql(connection));

builder.Services.AddHangfire(_ => _
.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
.UseSimpleAssemblyNameTypeSerializer()
.UseRecommendedSerializerSettings()
.UseConsole()
.UsePostgreSqlStorage(connection,
                new PostgreSqlStorageOptions { SchemaName = "public" }));

builder.Services.AddHangfireServer();

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

app.MapControllerRoute("hangfire", "/hangfire");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseHangfireDashboard("/hangfire",
    new DashboardOptions { IgnoreAntiforgeryToken = true });

RecurringJob
    .AddOrUpdate<JobsBackgroundServices>
    ("Auction Audit", s => s.AuctionAudit(null), "0 * * * *", TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
RecurringJob
    .AddOrUpdate<JobsBackgroundServices>
    ("Notify Winners", s => s.NotifyWinners(null), "0 8 * * *", TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));


app.Run();

