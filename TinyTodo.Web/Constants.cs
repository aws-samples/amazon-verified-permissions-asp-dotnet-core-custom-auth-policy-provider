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
        public const string GetTodoLists = "GetTodoLists";
        public const string CreateTodoList = "CreateTodoList";
        public const string AddTodoItem = "AddTodoItem";
        public const string ShareTodoList = "ShareTodoList";
        public const string DeleteTodoList = "DeleteTodoList";
        public const string MakeTodoListPrivate = "MakeTodoListPrivate";
        public const string UserAdmin = "UserAdmin";
    }

    public static class PolicyPrefixes
    {
        public const string HasPermissionOnAction = "HasPermissionOnAction_";
    }
}