using Amazon.CDK.AwsVerifiedpermissions;
using static Amazon.CDK.AwsVerifiedpermissions.CfnPolicy;

namespace TinyTodo.CDK.PolicyStore.StaticPolicies;
public class AllowActionsOnApplicationPolicy : CfnPolicyProps
{
    public AllowActionsOnApplicationPolicy(string policyStoreid)
    {
        PolicyStoreId = policyStoreid;
        Definition =  new PolicyDefinitionProperty 
        {
            Static = new StaticPolicyDefinitionProperty
            {
                Description = "Allow all generic actions on application", 
                Statement =  @"permit(
                                principal,
                                action in [TinyTodoList::Action::""GetTodoLists"", 
                                            TinyTodoList::Action::""CreateTodoList""],
                                resource == TinyTodoList::Application::""TinyTodoListApp""
                                );"
            }
        };
    }
}
