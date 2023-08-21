using Amazon.CDK.AwsVerifiedpermissions;

namespace TinyTodo.CDK.PolicyStore.PolicyTemplates;

public class TodoListSharedAccessPolicyTemplate : CfnPolicyTemplateProps
{
    public const string PolicyTemplateFilePath = @"PolicyStore/PolicyTemplates/TodoListSharedAccessPolicyTemplate.cedar";

    public TodoListSharedAccessPolicyTemplate(string policyStoreId)
    {
        PolicyStoreId = policyStoreId;
        Description = "User can add items to the shared todo list"; 
        Statement = File.ReadAllText(PolicyTemplateFilePath);
    }
}
