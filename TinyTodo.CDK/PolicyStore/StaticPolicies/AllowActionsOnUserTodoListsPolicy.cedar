permit (
    principal,
    action in
        [TinyTodoList::Action::"CreateTodoList",
         TinyTodoList::Action::"AddTodoItem",
         TinyTodoList::Action::"ShareTodoList",
         TinyTodoList::Action::"DeleteTodoList"],
    resource
)
when { resource.Owner == principal.Email };