permit (
    principal,
    action in [TinyTodoList::Action::"UserAdmin"],
    resource == TinyTodoList::Application::"TinyTodoListApp"
)
when { principal.Role == "Administrator" };