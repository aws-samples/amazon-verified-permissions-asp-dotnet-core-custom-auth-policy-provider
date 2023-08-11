using Amazon.CDK.AwsVerifiedpermissions;

namespace TinyTodo.CDK.PolicyStore.PolicyTemplates;

public class TodoListSharedAccessPolicyTemplate : CfnPolicyTemplateProps
{
    public TodoListSharedAccessPolicyTemplate(string policyStoreid)
    {
        PolicyStoreId = policyStoreid;
        Description = "User can add items to the shared todo list"; 
        Statement =  @"permit(
                        principal in ?principal,
                        action in [TinyTodoList::Action::""AddTodoItem""], 
                        resource in ?resource
                    );";
    }
}
