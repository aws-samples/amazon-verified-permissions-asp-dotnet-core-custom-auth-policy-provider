using Amazon.CDK.AwsVerifiedpermissions;

using static Amazon.CDK.AwsVerifiedpermissions.CfnPolicy;

namespace TinyTodo.CDK.PolicyStore.StaticPolicies;

public class AdminModuleAccessPolicy : CfnPolicyProps
{
    public const string PolicyFilePath = @"PolicyStore/StaticPolicies/AdminModuleAccessPolicy.cedar";

    public AdminModuleAccessPolicy(string policyStoreId)
    {
        PolicyStoreId = policyStoreId;
        Definition = new PolicyDefinitionProperty
        {
            Static = new StaticPolicyDefinitionProperty 
            {
                Description = "Users with Administrator role can access UserAdmin module", 
                Statement = File.ReadAllText(PolicyFilePath)
            }
        };
    }
}