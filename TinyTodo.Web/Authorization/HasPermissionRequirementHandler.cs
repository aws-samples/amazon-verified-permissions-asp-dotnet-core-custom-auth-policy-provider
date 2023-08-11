using Amazon.VerifiedPermissions;
using Amazon.VerifiedPermissions.Model;
using TinyTodo.Web.Database.Models;
using Microsoft.AspNetCore.Authorization;

namespace TinyTodo.Web.Authorization;

public class HasPermissionRequirementHandler : AuthorizationHandler<HasPermissionRequirement>
{
    private readonly IAmazonVerifiedPermissions _verifiedPermissionsClient;
    private readonly IAppConfig _appConfig;
    private readonly IVerifiedPermissionsUtil _verifiedPermissionsUtil;

    public HasPermissionRequirementHandler(IAmazonVerifiedPermissions verifiedPermissionsClient, 
                    IAppConfig appConfig,
                    IVerifiedPermissionsUtil verifiedPermissionsUtil)
    {
        _verifiedPermissionsClient = verifiedPermissionsClient;
        _appConfig = appConfig;
        _verifiedPermissionsUtil = verifiedPermissionsUtil;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
    {
        if(string.IsNullOrWhiteSpace(context.User.Identity?.Name))
        {
            context.Fail();
            return;
        }

        var resource = context.Resource as IEntity;
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
}
