using Amazon.VerifiedPermissions;
using TinyTodo.Web;
using TinyTodo.Web.Authorization;
using TinyTodo.Web.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, 
        o => { o.LoginPath = "/Auth/Login"; });

var appConfig = new AppConfig(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IAppConfig>(appConfig);
builder.Services.AddTransient<IAmazonVerifiedPermissions, AmazonVerifiedPermissionsClient>();
builder.Services.AddTransient<IVerifiedPermissionsUtil, VerifiedPermissionsUtil>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
builder.Services.AddTransient<IAuthorizationHandler, HasPermissionRequirementHandler>();

using (var db = new TinyTodoDBContext(appConfig))
{
    db.Database.EnsureCreated();
    db.Database.Migrate();
}

var app = builder.Build();

app.UseAuthentication();

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



