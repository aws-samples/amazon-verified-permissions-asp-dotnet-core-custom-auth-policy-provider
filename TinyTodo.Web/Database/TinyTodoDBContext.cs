using System.Collections;
using TinyTodo.Web.Database.Models;
using Microsoft.EntityFrameworkCore;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace TinyTodo.Web.Database;

public class TinyTodoDBContext : DbContext
{
    public DbSet<TodoItem> TodoItems { get; set; }

    public DbSet<TodoList> TodoLists { get; set; }

    public DbSet<TodoListShare> TodoListShares { get; set; }

    public DbSet<User> Users { get; set; }

    private IAppConfig _appConfig;

    public TinyTodoDBContext(IAppConfig appConfig):
            base(new DbContextOptions<TinyTodoDBContext>())
    {
        _appConfig = appConfig;        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_appConfig.GetConnectionString("TinyTodoDatabase"));
    }
}