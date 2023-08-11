using Amazon.CDK.AwsVerifiedpermissions;

using static Amazon.CDK.AwsVerifiedpermissions.CfnPolicy;

namespace TinyTodo.CDK.PolicyStore.StaticPolicies;

public class AdminModuleAccessPolicy : CfnPolicyProps
{
    public AdminModuleAccessPolicy(string policyStoreid)
    {
        PolicyStoreId = policyStoreid;
        Definition = new PolicyDefinitionProperty
        {
            Static = new StaticPolicyDefinitionProperty 
            {
                Description = "Users with Administrator role can access UserAdmin module", 
                Statement =  @"permit(
                                principal,
                                action in [TinyTodoList::Action::""UserAdmin""],
                                resource
                            ) 
                            when {
                                principal.Role == ""Administrator""
                            };"
            }
        };
    }
}