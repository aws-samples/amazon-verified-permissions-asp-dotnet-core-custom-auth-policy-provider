using System.Security.Claims;
using Amazon.VerifiedPermissions;
using Amazon.VerifiedPermissions.Model;
using TinyTodo.Web.Database.Models;
using Newtonsoft.Json;

namespace TinyTodo.Web.Authorization;

public class VerifiedPermissionsUtil : IVerifiedPermissionsUtil
{
    private readonly IAppConfig _appConfig;
    private readonly IAmazonVerifiedPermissions _verifiedPermissionsClient;

    public VerifiedPermissionsUtil(IAppConfig appConfig, IAmazonVerifiedPermissions verifiedPermissionsClient)
    {
        _appConfig = appConfig;
        _verifiedPermissionsClient = verifiedPermissionsClient;
    }

    public EntityItem ToEntityItem(ClaimsPrincipal? user)
    {
        return new EntityItem
        {
            Identifier = new EntityIdentifier
            {
                EntityType = $"{_appConfig.PolicyStoreSchemaNamespace}::User",
                EntityId = user?.Identity?.Name
            },
            Attributes = new Dictionary<string, AttributeValue>
            {
                {"Email", new AttributeValue {String = user.FindFirstValue(ClaimTypes.Email)}},
                {"Role", new AttributeValue {String = user.FindFirstValue(ClaimTypes.Role)}}
            },
        };
    }

    public EntityItem ToEntityItem(IEntity resource)
    {
        if (resource == null)
        {
            return null;
        }

        var entityItem = new EntityItem
        {
            Identifier = new EntityIdentifier
            {
                EntityType = $"{_appConfig.PolicyStoreSchemaNamespace}::{resource.GetType().Name}",
                EntityId = $"{resource.Id}"
            },
            Attributes = ToDictionary(resource)
        };
        return entityItem;
    }

    public EntityItem GetApplicationEntityItem()
    {
        return new EntityItem
        {
            Identifier = new EntityIdentifier
            {
                EntityType = $"{_appConfig.PolicyStoreSchemaNamespace}::Application",
                EntityId = "TinyTodoListApp"
            },
            Attributes = new()
        };
    }

    public async Task<IsAuthorizedResponse> IsAuthorizedAsync(ClaimsPrincipal user, string action, IEntity? entity)
    {
        var principal = ToEntityItem(user);

        var authorizationRequest = new IsAuthorizedRequest
        {
            PolicyStoreId = _appConfig.PolicyStoreId,
            Principal = principal.Identifier,
            Action = new ActionIdentifier { ActionType = _appConfig.ActionType, ActionId = action },
            Entities = new EntitiesDefinition
            {
                EntityList = new List<EntityItem> { principal }
            }
        };

        var resource =  entity != null ? ToEntityItem(entity) : GetApplicationEntityItem();
        
        authorizationRequest.Resource = resource.Identifier;
        authorizationRequest.Entities.EntityList.Add(resource);

        var isAuthorizedResponse = await _verifiedPermissionsClient.IsAuthorizedAsync(authorizationRequest);
        return isAuthorizedResponse;
    }    

    public async Task CreateSharePolicyAsync(string policyTemplateId, EntityIdentifier principal, EntityIdentifier resource)
    {
        var listPoliciesResponse = await ListPoliciesByTemplate(policyTemplateId, principal, resource);

        if (listPoliciesResponse.Policies.Any())
        {
            return;
        }

        var policyDefinition = new PolicyDefinition
        {
            TemplateLinked = new TemplateLinkedPolicyDefinition
            {
                PolicyTemplateId = policyTemplateId,
                Principal = principal,
                Resource = resource
            }
        };

        await _verifiedPermissionsClient.CreatePolicyAsync(new CreatePolicyRequest
        {
            PolicyStoreId = _appConfig.PolicyStoreId,
            Definition = policyDefinition
        });
    }

    public async Task DeleteSharePoliciesAsync(string policyTemplateId, EntityIdentifier resource)
    {
        var listPoliciesResponse = await ListPoliciesByTemplate(policyTemplateId, null, resource);

        foreach (var policy in listPoliciesResponse.Policies)
        {
            await _verifiedPermissionsClient.DeletePolicyAsync(new DeletePolicyRequest
            {
                PolicyId = policy.PolicyId,
                PolicyStoreId = _appConfig.PolicyStoreId
            });
        }
    }

    private async Task<ListPoliciesResponse> ListPoliciesByTemplate
            (string policyTemplateId, EntityIdentifier? principal, EntityIdentifier resource)
    {
        return await _verifiedPermissionsClient.ListPoliciesAsync(new ListPoliciesRequest
        {
            PolicyStoreId = _appConfig.PolicyStoreId,
            Filter = new PolicyFilter
            {
                PolicyTemplateId = policyTemplateId,
                PolicyType = PolicyType.TEMPLATE_LINKED,
                Principal = principal != null  ? new EntityReference { Identifier = principal } : null,
                Resource = new EntityReference { Identifier = resource }
            }
        });
    }

    private Dictionary<string, AttributeValue> ToDictionary(object obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        return dictionary.Select(x => new KeyValuePair<string, AttributeValue>(x.Key, new AttributeValue { String = x.Value }))
                .ToDictionary(x => x.Key, x => x.Value);
    }
}