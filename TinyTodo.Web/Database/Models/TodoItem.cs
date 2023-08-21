using System.ComponentModel.DataAnnotations.Schema;

namespace TinyTodo.Web.Database.Models;

public class TodoItem : IEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsDone { get; set; }
    public DateTime Created { get; set; }
    public Guid? TodoListId { get; set; }
    public TodoList? TodoList { get; set; }   
}
