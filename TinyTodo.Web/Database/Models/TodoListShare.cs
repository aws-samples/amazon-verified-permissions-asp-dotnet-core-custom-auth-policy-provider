namespace TinyTodo.Web.Database.Models;

public class TodoListShare : IEntity
{
    public Guid Id { get; set; }
    public Guid TodoListId { get; set; }
    public string Email { get; set; }
    public bool AllowReshare { get; set; }
    public TodoList TodoList { get; set; }
    public DateTime Created {get; set; }
}
