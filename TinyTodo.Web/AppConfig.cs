namespace TinyTodo.Web;

public class AppConfig : IAppConfig
{
    private readonly IConfiguration _configuration;

    public AppConfig(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private IConfigurationSection VerifiedPermissions => _configuration.GetSection("VerifiedPermissions");

    public string PolicyStoreId
    {
        get
        {
            return VerifiedPermissions.GetValue<string>(nameof(PolicyStoreId));
        }
    }

    public string PolicyStoreNamespace
    {
        get
        {
            return VerifiedPermissions.GetValue<string>(nameof(PolicyStoreNamespace));
        }
    }

    public string TodoListSharedAccessPolicyTemplateId
    {
        get
        {
            return VerifiedPermissions.GetValue<string>(nameof(TodoListSharedAccessPolicyTemplateId));
        }
    }

    public string TodoListSharedAccessWithResharePolicyTemplateId
    {
        get
        {
            return VerifiedPermissions.GetValue<string>(nameof(TodoListSharedAccessWithResharePolicyTemplateId));
        }
    }

    public string GetConnectionString(string name)
    {
        return _configuration.GetConnectionString(name);
    }

    public string ActionType
    {
        get
        {
            return $"{PolicyStoreNamespace}::Action";
        }
    }
}
