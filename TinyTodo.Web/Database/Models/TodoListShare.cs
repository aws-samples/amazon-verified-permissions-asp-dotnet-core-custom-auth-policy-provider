namespace TinyTodo.Web.Database.Models;

public class TodoListShare : IEntity
{
    public int Id { get; set; }
    public int TodoListId { get; set; }
    public string Email { get; set; }
    public bool AllowReshare { get; set; }
    public TodoList TodoList { get; set; }
    public DateTime Created {get; set; }
}
