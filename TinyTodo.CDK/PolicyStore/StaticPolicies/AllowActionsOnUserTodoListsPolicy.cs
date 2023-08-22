using Amazon.CDK.AwsVerifiedpermissions;

using static Amazon.CDK.AwsVerifiedpermissions.CfnPolicy;

namespace TinyTodo.CDK.PolicyStore.StaticPolicies;

public class AllowActionsOnUserTodoListsPolicy : CfnPolicyProps
{
    public const string PolicyFilePath = @"PolicyStore/StaticPolicies/AllowActionsOnUserTodoListsPolicy.cedar";
    public AllowActionsOnUserTodoListsPolicy(string policyStoreId)
    {
        PolicyStoreId = policyStoreId;
        Definition =  new PolicyDefinitionProperty 
        {
            Static = new StaticPolicyDefinitionProperty
            {
                Description = "Allow all actions for users on their own todo lists", 
                Statement = File.ReadAllText(PolicyFilePath)
            }
        };
    }
}
