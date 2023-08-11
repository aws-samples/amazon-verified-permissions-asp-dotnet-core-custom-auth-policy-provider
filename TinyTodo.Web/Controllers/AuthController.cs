using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TinyTodo.Web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using TinyTodo.Web.Database;
using TinyTodo.Web.Database.Models;
using Microsoft.AspNetCore.Authorization;

namespace TinyTodo.Web.Controllers;

public class AuthController : Controller
{
    private IAppConfig _appConfig;

    public AuthController(IAppConfig appConfig)
    {
        _appConfig = appConfig;
    }
    
    [AllowAnonymous]
    [HttpGet]
    [Route("/auth/login")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Login(string returnUrl = null)
    {
        return View(new LoginViewModel
        {
            ReturnUrl = returnUrl
        });
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("/auth/login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            using(var dbContext = new TinyTodoDBContext(_appConfig))
            {
                if(!dbContext.Users.Any(u => u.Email == model.Email))
                {
                    dbContext.Users.Add(new User{Email = model.Email});
                    dbContext.SaveChanges();
                }
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role,
                    model.Email.ToLower().Equals(Constants.AdminUserEmail)
                    ? Constants.Roles.Administrator
                    : Constants.Roles.User)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties{};

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), 
                authProperties);

            return RedirectToAction("Index", "TodoList");
        }

        return View(model);
    }

    [HttpGet]
    [Route("/auth/logout")]
    [DebuggerStepThrough]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}