using System.IO;

namespace TinyTodo.CDK.PolicyStore;

public static class PolicyStoreDetails
{
    public const string Name = "TinyTodoPolicyStore";
    
    public const string SchemaJsonFilePath = @"PolicyStore/TinyTodoList.cedarschema.json";

    public static string SchemaJson
    {
        get
        {
            return File.ReadAllText(SchemaJsonFilePath);
        }
    }   
}
