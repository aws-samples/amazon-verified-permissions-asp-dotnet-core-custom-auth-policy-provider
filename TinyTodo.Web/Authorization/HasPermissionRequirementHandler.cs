using Amazon.VerifiedPermissions;
using Amazon.VerifiedPermissions.Model;
using TinyTodo.Web.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using TinyTodo.Web.Database;
using System.ComponentModel;


namespace TinyTodo.Web.Authorization;

public class HasPermissionRequirementHandler : AuthorizationHandler<HasPermissionRequirement>
{
    private readonly IAmazonVerifiedPermissions _verifiedPermissionsClient;
    private readonly IAppConfig _appConfig;
    private readonly IVerifiedPermissionsUtil _verifiedPermissionsUtil;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HasPermissionRequirementHandler(IAmazonVerifiedPermissions verifiedPermissionsClient, 
                    IAppConfig appConfig,
                    IVerifiedPermissionsUtil verifiedPermissionsUtil,
                    IHttpContextAccessor httpContextAccessor)
    {
        _verifiedPermissionsClient = verifiedPermissionsClient;
        _appConfig = appConfig;
        _verifiedPermissionsUtil = verifiedPermissionsUtil;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
    {
        if (string.IsNullOrWhiteSpace(context.User.Identity?.Name))
        {
            context.Fail();
            return;
        }

        IEntity? resource = null;
        if(!string.IsNullOrWhiteSpace(requirement.ResourceType) && 
                !string.IsNullOrWhiteSpace(requirement.ResourceIdFormElementName))
        {
            var resourceId = GetResourceIdFromForm(requirement.ResourceIdFormElementName);
            resource = GetResource(requirement.ResourceType, resourceId);
        }        

        var isAuthorizedResponse = await _verifiedPermissionsUtil.IsAuthorizedAsync(context.User, requirement.Action, resource);

        if (isAuthorizedResponse.Decision == Decision.ALLOW)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }

    private IEntity? GetResource(string resourceType, string resourceId)
    {
        var allowedResourceTypes = new List<string> { nameof(TodoList) };

        if (!allowedResourceTypes.Contains(resourceType))
        {
            throw new InvalidEnumArgumentException("Unknown resource type");
        }

        IEntity? resource = null;
        if (resourceType == nameof(TodoList))
        {
            using (var db = new TinyTodoDBContext(_appConfig))
            {
                resource = db.TodoLists.FirstOrDefault(x => x.Id == Guid.Parse(resourceId));
            }
        }
        return resource;
    }

    private string? GetResourceIdFromForm(string formElementName)
    {
        if(!string.IsNullOrWhiteSpace(formElementName))
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            StringValues formValues = new();
            var formElementNameKey = request?.Form.Keys.FirstOrDefault(k => k.ToLower() == formElementName.ToLower());
            request?.Form.TryGetValue(formElementNameKey, out formValues);
            var resourceId = formValues.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(resourceId))
            {
                return resourceId;
            }
        }        
        throw new InvalidEnumArgumentException("Invalid form element name");
    }
}
