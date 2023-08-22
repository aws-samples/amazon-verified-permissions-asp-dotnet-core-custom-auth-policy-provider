using Microsoft.AspNetCore.Authorization;

namespace TinyTodo.Web.Authorization;

public class HasPermissionRequirement : IAuthorizationRequirement
{
    public HasPermissionRequirement(string action) =>
        Action = action;

    public HasPermissionRequirement(string action, string resourceType, string resourceIdFormElementName)
    {
        Action = action;
        ResourceType = resourceType;
        ResourceIdFormElementName = resourceIdFormElementName;
    }       

    public string Action { get; }
    public string ResourceType { get; set; }
    public string ResourceIdFormElementName { get; set; }
}