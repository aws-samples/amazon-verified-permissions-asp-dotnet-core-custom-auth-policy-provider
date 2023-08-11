using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TinyTodo.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace TinyTodo.Web.Controllers;

public class AccountController : Controller
{
    [Authorize]
    public IActionResult Details()
    {
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}