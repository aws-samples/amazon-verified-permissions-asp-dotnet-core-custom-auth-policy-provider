using Amazon.CDK.AwsVerifiedpermissions;

using static Amazon.CDK.AwsVerifiedpermissions.CfnPolicy;

namespace TinyTodo.CDK.PolicyStore.StaticPolicies;

public class AllowActionsOnUserTodoListsPolicy : CfnPolicyProps
{
    public AllowActionsOnUserTodoListsPolicy(string policyStoreid)
    {
        PolicyStoreId = policyStoreid;
        Definition =  new PolicyDefinitionProperty 
        {
            Static = new StaticPolicyDefinitionProperty
            {
                Description = "Allow all actions for users on their own todo lists", 
                Statement =  @"permit(
                                principal,
                                action in [TinyTodoList::Action::""AddTodoItem"", 
                                            TinyTodoList::Action::""ShareTodoList"", 
                                            TinyTodoList::Action::""DeleteTodoList"",
                                            TinyTodoList::Action::""MakeTodoListPrivate""],
                                resource
                                )
                            when {
                                resource.Owner == principal.Email  
                            };"
            }
        };
    }
}
