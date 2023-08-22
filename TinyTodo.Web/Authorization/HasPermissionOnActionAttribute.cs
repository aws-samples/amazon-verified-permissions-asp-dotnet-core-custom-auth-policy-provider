namespace TinyTodo.Web.Authorization;
using Microsoft.AspNetCore.Authorization;

public class HasPermissionOnActionAttribute : AuthorizeAttribute
{
    public HasPermissionOnActionAttribute(string actionName, string resourceType = "", string resourceIdFormElementName = "")
    {
        Policy = $"{Constants.PolicyPrefixes.HasPermissionOnAction}{actionName}_{resourceType}_{resourceIdFormElementName}";
    }
}



