using Amazon.CDK.AwsVerifiedpermissions;

namespace TinyTodo.CDK.PolicyStore.PolicyTemplates;

public class TodoListSharedAccessWithResharePolicyTemplate : CfnPolicyTemplateProps
{
    public const string PolicyTemplateFilePath = @"PolicyStore/PolicyTemplates/TodoListSharedAccessWithResharePolicyTemplate.cedar";

    public TodoListSharedAccessWithResharePolicyTemplate(string policyStoreId)
    {
        PolicyStoreId = policyStoreId;
        Description = "User can add items to and reshare the shared todo list";
        Statement = File.ReadAllText(PolicyTemplateFilePath);
    }
}
