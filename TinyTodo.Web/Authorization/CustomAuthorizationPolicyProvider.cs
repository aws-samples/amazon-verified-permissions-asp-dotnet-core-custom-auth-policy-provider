using Amazon.VerifiedPermissions;
using Amazon.VerifiedPermissions.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace TinyTodo.Web.Authorization;

public class CustomAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _defaultPolicyProvider;

    public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)  
    {  
        _defaultPolicyProvider = new DefaultAuthorizationPolicyProvider(options); 
    }  

    public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return await _defaultPolicyProvider.GetDefaultPolicyAsync();
    }

    public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return await _defaultPolicyProvider.GetDefaultPolicyAsync();
    }

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policyRequirements = await GetPolicyRequirements(policyName);
        var policyBuilder = new AuthorizationPolicyBuilder();   
        policyBuilder.AddRequirements(policyRequirements);
        return policyBuilder.Build();
    }

    private async Task<IAuthorizationRequirement[]> GetPolicyRequirements(string policyName)
    {
        if(policyName.StartsWith(Constants.PolicyPrefixes.HasPermissionOnAction))
        {
            var action = policyName.Replace(Constants.PolicyPrefixes.HasPermissionOnAction, "");
            var actionParts = action.Split("_");
            return new [] {new HasPermissionRequirement(action: actionParts[0], 
                                                    resourceType: actionParts[1], 
                                                    resourceIdFormElementName: actionParts[2])};
        }        

        throw new NotImplementedException("Unknown policy type");
    }
}
