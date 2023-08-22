using Amazon.CDK.AwsVerifiedpermissions;
using static Amazon.CDK.AwsVerifiedpermissions.CfnPolicy;

namespace TinyTodo.CDK.PolicyStore.StaticPolicies;
public class AllowActionsOnApplicationPolicy : CfnPolicyProps
{
    public const string PolicyFilePath = @"PolicyStore/StaticPolicies/AllowActionsOnApplicationPolicy.cedar";

    public AllowActionsOnApplicationPolicy(string policyStoreId)
    {
        PolicyStoreId = policyStoreId;
        Definition =  new PolicyDefinitionProperty 
        {
            Static = new StaticPolicyDefinitionProperty
            {
                Description = "Allow all generic actions on application", 
                Statement = File.ReadAllText(PolicyFilePath)
            }
        };
    }
}
