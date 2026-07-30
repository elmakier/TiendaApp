using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TiendaApp.Data;
using TiendaApp.Interfaces;
using TiendaApp.Repositories;
using TiendaApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ==================== MIDDLEWARE DE MONITOREO ====================
app.Use(async (context, next) =>
{
    var timer = System.Diagnostics.Stopwatch.StartNew();

    context.Response.Headers.Append("X-Server-Performance", "Tracking");

    await next();

    timer.Stop();

    Console.WriteLine($"[MONITOR] {context.Request.Method} {context.Request.Path} - {timer.ElapsedMilliseconds}ms");
});

// ==================== SITEMAP.XML ====================
app.Map("/sitemap.xml", appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.ContentType = "application/xml";

        var sitemapContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
    <url>
        <loc>https://laeconomica.com/</loc>
        <priority>1.0</priority>
    </url>
    <url>
        <loc>https://laeconomica.com/Productos</loc>
        <priority>0.8</priority>
    </url>
    <url>
        <loc>https://laeconomica.com/Ofertas</loc>
        <priority>0.9</priority>
    </url>
</urlset>";

        await context.Response.WriteAsync(sitemapContent);
    });
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await RoleSeeder.SeedRolesAsync(roleManager);
}

app.Run();