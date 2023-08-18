public static class Constants 
{
    public const string AdminUserEmail = "admin@example.com";

    public static class Roles
    {
        public const string Administrator = "Administrator";
        public const string User = "User";
    }

    public static class Actions
    {
        public const string CreateTodoList = "CreateTodoList";
        public const string AddTodoItem = "AddTodoItem";
        public const string ShareTodoList = "ShareTodoList";
        public const string DeleteTodoList = "DeleteTodoList";
        public const string UserAdmin = "UserAdmin";
    }

    
    public static class ResourcePolicies    
    {
        public const string CreateTodoList = $"{PolicyPrefixes.HasPermissionOnResource}{Actions.CreateTodoList}";
        public const string AddTodoItem = $"{PolicyPrefixes.HasPermissionOnResource}{Actions.AddTodoItem}";
        public const string ShareTodoList = $"{PolicyPrefixes.HasPermissionOnResource}{Actions.ShareTodoList}";
        public const string DeleteTodoList = $"{PolicyPrefixes.HasPermissionOnResource}{Actions.DeleteTodoList}";
    }

    public static class PolicyPrefixes
    {
        public const string HasPermissionOnAction = "HasPermissionOnAction_";
        public const string HasPermissionOnResource = "HasPermissionOnResource_";
    }
}