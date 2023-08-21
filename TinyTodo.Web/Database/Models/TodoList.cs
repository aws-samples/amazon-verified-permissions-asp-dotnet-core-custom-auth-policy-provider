
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TinyTodo.Web.Database.Models;

public class TodoList : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Owner { get; set; } = "";
    public DateTime Created { get; set; }

    [JsonIgnore]
    public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();

    [JsonIgnore]
    public ICollection<TodoListShare> Shares  { get; set; } = new List<TodoListShare>();
}