using Amazon.VerifiedPermissions;
using Amazon.VerifiedPermissions.Model;
using TinyTodo.Web.Authorization;
using TinyTodo.Web.Database;
using TinyTodo.Web.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TinyTodo.Web.Controllers;

[Authorize]
public class TodoListController : Controller
{
    private IAppConfig _appConfig;
    private IAuthorizationService _authorizationService;
    private readonly IAmazonVerifiedPermissions _verifiedPermissionsClient;
    private readonly IVerifiedPermissionsUtil _verifiedPermissionsUtil;

    public TodoListController(IAppConfig appConfig,
                        IVerifiedPermissionsUtil verifiedPermissionsUtil, 
                        IAuthorizationService authorizationService,
                        IAmazonVerifiedPermissions verifiedPermissionsClient)
    {
        _appConfig = appConfig;
        _authorizationService = authorizationService;
        _verifiedPermissionsClient = verifiedPermissionsClient;
        _verifiedPermissionsUtil = verifiedPermissionsUtil;
    }

    [HasPermissionOnAction(Constants.Actions.GetTodoLists)]
    public async Task<IActionResult> Index()
    {
        var todoLists = new List<TodoList>();
        using (var db = new TinyTodoDBContext(_appConfig))
        {
            todoLists = db.TodoLists.Where(x => x.Owner == User.Identity.Name)
                            .Include(t => t.TodoItems)
                            .Include(t => t.Shares)
                            .OrderByDescending(x => x.Created).ToList();

            var sharedTodoLists = db.TodoListShares.Where(x => x.Email == User.Identity.Name)
                            .Include(t => t.TodoList)
                            .Include(t => t.TodoList.TodoItems)                            
                            .Select(x => x.TodoList)                            
                            .OrderByDescending(x => x.Created).ToList();

            todoLists.AddRange(sharedTodoLists);

        }
        return View("List", todoLists);
    }

    [HttpPost]
    [HasPermissionOnAction(Constants.Actions.CreateTodoList)]    
    public async Task<IActionResult> CreateAsync(TodoList todoList)
    {
        using (var db = new TinyTodoDBContext(_appConfig))
        {
            if(db.TodoLists.Any(x => x.Title == todoList.Title))
            {
                return new JsonResult(new { success = false, message = "To-do list already exists" });
            }
            todoList.Owner = User.Identity.Name;
            todoList.Created = DateTime.Now;

            db.TodoLists.Add(todoList);
            db.SaveChanges();
        }
        return new JsonResult(new { success = true, message = "To-do list created" });
    }

    [HttpPost]
    [HasPermissionOnAction(actionName: Constants.Actions.AddTodoItem,  
                            resourceType: nameof(TodoList), 
                            resourceIdFormElementName: nameof(TodoItem.TodoListId))]
    public async Task<IActionResult> AddTodoItem(TodoItem todoItem)
    {
        using (var db = new TinyTodoDBContext(_appConfig))
        {
            var todoList = db.TodoLists.FirstOrDefault(x => x.Id == todoItem.TodoListId);

            if(db.TodoItems.Any(x => x.TodoListId == todoItem.TodoListId 
                                            && x.Title == todoItem.Title))
            {
                return new JsonResult(new { success = false, message = "To-do item already exists" });
            }
            todoItem.Created = DateTime.Now;
            db.TodoItems.Add(todoItem);
            db.SaveChanges();
        }
        return new JsonResult(new { success = true, message = "To-do item created" });
    }

    [HttpPost]
    [HasPermissionOnAction(actionName: Constants.Actions.ShareTodoList, 
                resourceType: nameof(TodoList), 
                resourceIdFormElementName: nameof(TodoListShare.TodoListId))]
    public async Task<IActionResult> ShareAsync(TodoListShare todoListShare)
    {
        if(todoListShare.Email.Equals(User.Identity.Name, StringComparison.OrdinalIgnoreCase))
        {
            return new JsonResult(new { success = false, message = "Invalid email address" });
        }
        using (var db = new TinyTodoDBContext(_appConfig))
        {                        
            if(db.TodoListShares.Any(x => x.Email == todoListShare.Email && x.TodoListId == todoListShare.TodoListId))
            {
                return new JsonResult(new { success = false, message = "To-do list already shared with this email" });
            }

            todoListShare.Created = DateTime.Now;

            db.TodoListShares.Add(todoListShare);
            db.SaveChanges();
        }

        var policyTemplateId = todoListShare.AllowReshare ? _appConfig.TodoListSharedAccessWithResharePolicyTemplateId 
                                        : _appConfig.TodoListSharedAccessPolicyTemplateId;
        
        await _verifiedPermissionsUtil.CreateSharePolicyAsync(policyTemplateId, 
            new EntityIdentifier { EntityId = todoListShare.Email,  EntityType = $"{_appConfig.PolicyStoreSchemaNamespace}::User" },
            new EntityIdentifier { EntityId = $"{todoListShare.TodoListId}",  EntityType = $"{_appConfig.PolicyStoreSchemaNamespace}::{typeof(TodoList).Name}"}
        );

        return new JsonResult(new { success = true, message = "To-do list shared with this email" });
    }    

    [HttpPost]
    [HasPermissionOnAction(actionName: Constants.Actions.DeleteTodoList, 
                resourceType: nameof(TodoList), 
                resourceIdFormElementName: nameof(TodoList.Id))]
    public async Task<IActionResult> DeleteAsync(TodoList todoList)
    {
        using (var db = new TinyTodoDBContext(_appConfig))
        {
            todoList = await db.TodoLists
                            .Include(t => t.TodoItems)
                            .Include(t => t.Shares)
                            .FirstOrDefaultAsync(x => x.Id == todoList.Id);

            db.TodoLists.Remove(todoList);
            db.SaveChanges();
        }
        return new JsonResult(new { success = true, message = "To-do list deleted" });
    }

    [HttpPost]
    [HasPermissionOnAction(actionName: Constants.Actions.MakeTodoListPrivate, 
            resourceType: nameof(TodoList), 
            resourceIdFormElementName: nameof(TodoList.Id))]
    public async Task<IActionResult> MakePrivateAsync(TodoList todoList)
    {
        using (var db = new TinyTodoDBContext(_appConfig))
        {
            var shares = db.TodoListShares.Where(x => x.TodoListId == todoList.Id);
            if(!shares.Any())
            {
                return new JsonResult(new { success = false, message = "Invalid to-do list or email" });
            }

            db.TodoListShares.RemoveRange(shares);
            db.SaveChanges();
        }
        
        await _verifiedPermissionsUtil.DeleteSharePoliciesAsync(_appConfig.TodoListSharedAccessPolicyTemplateId, 
            new EntityIdentifier { EntityId = $"{todoList.Id}",  
                EntityType = $"{_appConfig.PolicyStoreSchemaNamespace}::{typeof(TodoList).Name}"}
        );

        await _verifiedPermissionsUtil.DeleteSharePoliciesAsync(_appConfig.TodoListSharedAccessWithResharePolicyTemplateId, 
            new EntityIdentifier { EntityId = $"{todoList.Id}",  
                EntityType = $"{_appConfig.PolicyStoreSchemaNamespace}::{typeof(TodoList).Name}"}
        );

        return new JsonResult(new { success = true, message = "To-do list is marked as private." });
    }       
}

