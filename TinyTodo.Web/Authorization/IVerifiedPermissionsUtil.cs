using System.Security.Claims;
using Amazon.VerifiedPermissions.Model;
using TinyTodo.Web.Database.Models;

namespace TinyTodo.Web.Authorization;

public interface IVerifiedPermissionsUtil
{
    Task<IsAuthorizedResponse> IsAuthorizedAsync(ClaimsPrincipal user, string action, IEntity? entity);
    Task CreateSharePolicyAsync(string policyTemplateId, EntityIdentifier principal, EntityIdentifier resource);
    Task DeleteSharePoliciesAsync(string policyTemplateId, EntityIdentifier resource);
    EntityItem ToEntityItem(IEntity resource);
    EntityItem ToEntityItem(ClaimsPrincipal? user);
}
