using TinyTodo.Web.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TinyTodo.Web.Controllers;

public class AdminController : Controller
{
    [HasPermissionOnAction(Constants.Actions.UserAdmin)]
    public IActionResult UserAdmin(int id)
    {
        return View();
    }
}
