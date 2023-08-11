
using Newtonsoft.Json;

namespace TinyTodo.Web.Database.Models;

public class TodoList : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Owner { get; set; } = "";
    public DateTime Created { get; set; }

    [JsonIgnore]
    public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();

    [JsonIgnore]
    public ICollection<TodoListShare> Shares  { get; set; } = new List<TodoListShare>();
}