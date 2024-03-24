using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyBlogger.Models;
using MyBlogger.Repositories;
using MyBlogger.Services;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("local");
builder.Services.AddDbContext<AppDbContext>(o =>
{
    o.UseMySql(connection, ServerVersion.AutoDetect(connection));
});

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme
)
.AddCookie();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<EmailSetting>(
    builder.Configuration.GetSection("EmailSetting"))
    .AddSingleton(o => o.GetRequiredService<IOptions<EmailSetting>>().Value);
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
