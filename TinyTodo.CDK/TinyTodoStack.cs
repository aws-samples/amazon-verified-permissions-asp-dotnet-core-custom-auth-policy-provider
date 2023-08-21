using Amazon.CDK;
using Amazon.CDK.AwsVerifiedpermissions;
using Constructs;
using TinyTodo.CDK.PolicyStore;
using TinyTodo.CDK.PolicyStore.PolicyTemplates;
using TinyTodo.CDK.PolicyStore.StaticPolicies;
using static Amazon.CDK.AwsVerifiedpermissions.CfnPolicyStore;

namespace TinyTodo.CDK;

public class TinyTodoCdkStack : Stack
{
    public TinyTodoCdkStack(Construct? scope = null, string? id = null, IStackProps? props = null) 
            : base(scope, id, props)
    {
        var tinyTodoPolicyStore = new CfnPolicyStore(this, PolicyStoreDetails.Name, 
            new CfnPolicyStoreProps 
            {
                ValidationSettings = new ValidationSettingsProperty 
                {
                    Mode = "STRICT"                
                },
                Schema = new SchemaDefinitionProperty 
                {
                    CedarJson = PolicyStoreDetails.SchemaJson
                }
            });

        var policyStoreId = tinyTodoPolicyStore.AttrPolicyStoreId;

        var allowActionsOnUserTodoLists 
                = new CfnPolicy(this, nameof(AllowActionsOnUserTodoListsPolicy), 
                        new AllowActionsOnUserTodoListsPolicy(policyStoreId));

        var adminModuleAccessPolicy 
                = new CfnPolicy(this, nameof(AdminModuleAccessPolicy), 
                        new AdminModuleAccessPolicy(policyStoreId));

        var sharedAccessPolicyTemplate 
                = new CfnPolicyTemplate(this, nameof(TodoListSharedAccessPolicyTemplate), 
                        new TodoListSharedAccessPolicyTemplate(policyStoreId));

        var sharedAccessWithResharePolicyTemplate 
                = new CfnPolicyTemplate(this, nameof(TodoListSharedAccessWithResharePolicyTemplate), 
                        new TodoListSharedAccessWithResharePolicyTemplate(policyStoreId));

        new CfnOutput(this, "PolicyStoreId", new CfnOutputProps {Value = policyStoreId});
        new CfnOutput(this, "PolicyStoreSchemaNamespace", new CfnOutputProps {Value = "TinyTodoList"});
        new CfnOutput(this, "TodoListSharedAccessPolicyTemplateId", 
                new CfnOutputProps {Value = sharedAccessPolicyTemplate.AttrPolicyTemplateId});
        new CfnOutput(this, "TodoListSharedAccessWithResharePolicyTemplateId", 
                new CfnOutputProps {Value = sharedAccessWithResharePolicyTemplate.AttrPolicyTemplateId});
    }
}
