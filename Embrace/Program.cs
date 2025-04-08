using Embrace.Data;
using Embrace.Models;
using Embrace.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ApplicationDbContext>();

// MySQL DB connection
string _GetConnStringName = builder.Configuration.GetConnectionString("DefaultConnectionMySQL")!;
builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseMySql(_GetConnStringName, ServerVersion.AutoDetect(_GetConnStringName)));

// Register IEmailSender Service
builder.Services.AddSingleton<IEmailSender, MockEmailSender>();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

// TO-DO: set up so that it only does this if builder.Environment.IsDevelopment()
builder.Configuration.AddUserSecrets<Program>();

var app = builder.Build();

// Ensure database is seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Initialize the database and seed test data
    await DbInitializer.Initialize(scope.ServiceProvider, context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapIdentityApi<User>();

// Add protected endpoint for API; require auth before accessing endpoint
app.MapGet("Users/Profile", async (ClaimsPrincipal claims, ApplicationDbContext context) =>
{
    string userId = claims.Claims.First(claims => claims.Type == ClaimTypes.NameIdentifier).Value;
    return await context.UserClaims.FindAsync(userId);
})
.RequireAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
