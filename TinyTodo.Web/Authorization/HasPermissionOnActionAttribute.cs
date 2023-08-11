namespace TinyTodo.Web.Authorization;
using Microsoft.AspNetCore.Authorization;

public class HasPermissionOnActionAttribute : AuthorizeAttribute
{
    public HasPermissionOnActionAttribute(string actionName)
    {
        Policy = $"{Constants.PolicyPrefixes.HasPermissionOnAction}{actionName}";
    }
}



