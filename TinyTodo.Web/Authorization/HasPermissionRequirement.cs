using Microsoft.AspNetCore.Authorization;

namespace TinyTodo.Web.Authorization;

public class HasPermissionRequirement : IAuthorizationRequirement
{
    public HasPermissionRequirement(string action) =>
        Action = action;

    public string Action { get; }
}