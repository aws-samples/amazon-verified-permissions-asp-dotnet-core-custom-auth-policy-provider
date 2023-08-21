permit (
    principal,
    action in [TinyTodoList::Action::"UserAdmin"],
    resource
)
when { principal.Role == "Administrator" };