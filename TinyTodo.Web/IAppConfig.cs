namespace TinyTodo.Web;

public interface IAppConfig
{
    string PolicyStoreId { get; }
    string PolicyStoreNamespace { get; }
    string TodoListSharedAccessPolicyTemplateId { get; }
    string TodoListSharedAccessWithResharePolicyTemplateId { get; }
    string ActionType { get; }

    string GetConnectionString(string name);
}
