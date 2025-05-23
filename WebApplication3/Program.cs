using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.SecuritySettings;
using Services.ProcessingTime;
using WebApplication3.Data;
using WebApplication3.Models;
using YourAppNamespace.Middleware;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("default");
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlite(connectionString));

builder.Services.AddIdentity<AppUser, IdentityRole>(
    options =>
    {
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuring UseXFrameOptions
builder.Services.AddAntiforgery(options =>
{
    if(SecurityProvider.GetRule("UseXFrameOptions"))
    {
        options.SuppressXFrameOptionsHeader = false;
    }
    else
    {
        options.SuppressXFrameOptionsHeader = true;
    }
});

// Configuring CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost",
                "http://localhost:5000",
                "https://localhost:5001",
                "http://127.0.0.1",
                "localhost:5222"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add processing time service
var appDataPath = Path.Combine(AppContext.BaseDirectory, "App_Data");
Directory.CreateDirectory(appDataPath);
var timingsFilePath = Path.Combine(appDataPath, "request_timings.json");
builder.Services.AddSingleton<IRequestTimingService>(new RequestTimingService(timingsFilePath));


var app = builder.Build();

// Configuring RefererChecking
if (SecurityProvider.GetRule("RefererChecking"))
{
    app.Use(async (context, next) =>
    {
        if (context.Request.Method == "POST")
        {
            string referer = context.Request.Headers["Referer"]!;
            if (!string.IsNullOrEmpty(referer) && !referer.StartsWith("http://localhost", StringComparison.OrdinalIgnoreCase))
            {
                // ��������� ������, ���� Referer �� ���
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Forbidden: invalid referer");
                return;
            }
        }
        await next();
    });
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configuring CORS
if (SecurityProvider.GetRule("UseCORS"))
{
    app.UseCors("DefaultPolicy");
}

// Configuring CSP
if(SecurityProvider.GetRule("UseCSP"))
{
    app.Use(async (context, next) =>
    {
        /*context.Response.Headers.Append("Content-Security-Policy",
            "default-src 'self'; script-src 'self'");*/ 
        context.Response.Headers.Append("Content-Security-Policy",
            "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'");
        await next();
    });
}
else
{
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Remove("Content-Security-Policy");
        await next();
    });
}

// Configuring X-XSS-Protection
if(SecurityProvider.GetRule("UseCSP"))
{
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
        await next();
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseRequestTiming();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();