namespace TinyTodo.Web.Database.Models;

public class User : IEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? Team { get; set; }
}